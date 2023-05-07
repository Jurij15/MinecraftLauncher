using CmlLib.Core;
using CmlLib.Core.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MinecraftLauncher.Helpers
{
    public class PlayHelper
    {
        public static async Task Launch(string VersionName, int MemoryAmountInMB, bool bFullscreen)
        {
            // increase connection limit to fast download
            System.Net.ServicePointManager.DefaultConnectionLimit = 512;

            var path = new MinecraftPath(); // use default directory

            var launcher = new CMLauncher(path);

            var launchOption = new MLaunchOption
            {
                MaximumRamMb = MemoryAmountInMB,
                //Session = MSession.GetOfflineSession(Globals.Username), // replace this with login session value. ex) Session = MSession.GetOfflineSession("hello")
                FullScreen = bFullscreen
                //ScreenWidth = 1600,
                //ScreenHeight = 900,
                //ServerIp = "mc.hypixel.net"
            };

            try
            {
                var process = await launcher.CreateProcessAsync(VersionName, launchOption);

                process.Start(); 
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error message: "+ ex.Message, "An error occured");
            }
        }

        public static async Task Download(string VersionName, int MemoryAmountInMB, bool bFullscreen)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 512;

            var path = new MinecraftPath();

            var launcher = new CMLauncher(path);

            var launchOption = new MLaunchOption
            {
                MaximumRamMb = MemoryAmountInMB,
                ///Session = MSession.GetOfflineSession(Globals.Username),
                FullScreen = bFullscreen
            };

            try
            {
                var process = await launcher.CreateProcessAsync(VersionName, launchOption);

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
