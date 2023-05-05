using CmlLib.Core;
using CmlLib.Core.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncher.Helpers
{
    public class PlayHelper
    {
        public static async Task Launch(string VersionName, int MemoryAmountInMB)
        {
            // increase connection limit to fast download
            System.Net.ServicePointManager.DefaultConnectionLimit = 512;

            var path = new MinecraftPath(); // use default directory

            var launcher = new CMLauncher(path);

            var launchOption = new MLaunchOption
            {
                MaximumRamMb = MemoryAmountInMB,
                Session = MSession.GetOfflineSession(Globals.Username), // replace this with login session value. ex) Session = MSession.GetOfflineSession("hello")

                //ScreenWidth = 1600,
                //ScreenHeight = 900,
                //ServerIp = "mc.hypixel.net"
            };

            var process = await launcher.CreateProcessAsync(VersionName, launchOption);

            process.Start();
        }
    }
}
