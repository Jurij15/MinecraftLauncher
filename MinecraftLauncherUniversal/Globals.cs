using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncher
{
    public class Globals
    {
        public static List<string> AllVersionsArray = new List<string>();

        public static string CurrentBuild;

        public static string VersionString = "1.0";

        //configs
        public static string Username { get; set; }
        public static int DownloadRateLimit = 1024;
        public static HashSet<string > Recents = new HashSet<string>();
    }
}
