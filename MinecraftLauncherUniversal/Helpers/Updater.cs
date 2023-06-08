using CmlLib.Core.VersionMetadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MinecraftLauncherUniversal.Helpers
{
    public class Updater
    {
        public static string GetLatestVersionStringFromGitHub()
        {
            string RetVal = string.Empty;

            if (File.Exists("Settings/VersionTemp"))
            {
                File.Delete("Settings/VersionTemp");
            }

            WebClient wc = new WebClient();
            wc.DownloadFile("https://raw.githubusercontent.com/Jurij15/MinecraftLauncher/master/docs/api/latestVersion.txt", "Settings/VersionTemp");

            RetVal = File.ReadAllText("Settings/VersionTemp");
            return RetVal;
        }

        public static bool bIsUpToDate()
        {
            bool retVal = false;
            if (GetLatestVersionStringFromGitHub() == Globals.VersionString)
            {
                retVal = true;
            }

            return retVal;
        }

        public static bool bIsPrerelease()
        {
            bool retVal = false;
            if (Globals.VersionString.Contains("PR") || Globals.VersionString.Contains("DEV") || Globals.VersionString.Contains("Beta", StringComparison.OrdinalIgnoreCase))
            {
                retVal = true;
            }

            return retVal;
        }
    }
}
