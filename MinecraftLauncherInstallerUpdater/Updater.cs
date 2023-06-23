using System;
using System.Collections.Generic;
using System.Data;
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

            if (File.Exists("VersionTemp"))
            {
                File.Delete("VersionTemp");
            }

            Uri uri = new Uri("https://raw.githubusercontent.com/Jurij15/MinecraftLauncher/master/docs/api/latestVersion.txt");
            WebClient wc = new WebClient();
            try
            {
                wc.DownloadFile(uri, "VersionTemp");
            }
            catch (WebException ex)
            {
                MessageBox.Show("Cannot check for updates, exception " + ex.Message, "Fail");
                throw;
            }

            RetVal = File.ReadAllText("VersionTemp");
            return RetVal;
        }
    }
}
