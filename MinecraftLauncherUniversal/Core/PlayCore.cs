using CmlLib.Core.Auth;
using CmlLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherUniversal.Core
{
    public class PlayCore
    {
        private string _version { get; set; }
        private int _memoryMB { get; set; }
        private bool _bfullscreen { get; set; }

        public PlayCore(string VersionName, int MemoryInMB, bool LaunchInFullscreen)
        {
            _version = VersionName;
            _memoryMB = MemoryInMB;
            _bfullscreen = LaunchInFullscreen;
        }

        public async Task Launch()
        {
            // increase connection limit to fast download
            System.Net.ServicePointManager.DefaultConnectionLimit = 512;

            var path = new MinecraftPath(); // use default directory

            var launcher = new CMLauncher(path);

            var launchOption = new MLaunchOption
            {
                MaximumRamMb = _memoryMB,
                Session = MSession.GetOfflineSession(Globals.Username),
                FullScreen = _bfullscreen,
                GameLauncherName = "MinecraftLauncher",
                //ScreenWidth = 1600,
                //ScreenHeight = 900,
                //ServerIp = "mc.hypixel.net"
            };

            try
            {
                var process = await launcher.CreateProcessAsync(_version, launchOption);

                process.Start();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error message: "+ ex.Message, "An error occured");
            }
        }

        public async Task LaunchWithUUID()
        {

        }

        public async Task Download()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 512;

            var path = new MinecraftPath();

            var launcher = new CMLauncher(path);

            var launchOption = new MLaunchOption
            {
                MaximumRamMb = _memoryMB,
                Session = MSession.GetOfflineSession(Globals.Username),
                FullScreen = _bfullscreen
            };

            try
            {
                var process = await launcher.CreateProcessAsync(_version, launchOption);

                process.Dispose();
                //process.Start(); we do just not launch the process, simple
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error message: " + ex.Message, "An error occured");
            }
        }
    }
}
