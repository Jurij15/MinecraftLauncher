using CmlLib.Core;
using CmlLib.Core.VersionLoader;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherUniversal.Helpers
{
    public class VersionsHelper
    {
        public static async Task<string[]> GetAllVersionsAsync()
        {
            List<string > AllVersions = new List<string>(); 
            System.Net.ServicePointManager.DefaultConnectionLimit = Globals.DownloadRateLimit;

            //var path = new MinecraftPath("game_directory_path");
            var path = new MinecraftPath(); // use default directory

            var launcher = new CMLauncher(path);

            var versions = await launcher.GetAllVersionsAsync();
            foreach (var item in versions)
            {
                // show all version names
                // use this version name in CreateProcessAsync method.
                AllVersions.Add(item.Name);
            }

            return AllVersions.ToArray();
        }

        public static string[] GetAllVersions()
        {
            List<string> AllVersions = new List<string>();

            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = Globals.DownloadRateLimit;

                //var path = new MinecraftPath("game_directory_path");
                var path = new MinecraftPath(); // use default directory

                var launcher = new CMLauncher(path);

                var versions = launcher.GetAllVersions();
                foreach (var item in versions)
                {
                    // show all version names
                    // use this version name in CreateProcessAsync method.
                    AllVersions.Add(item.Name);
                }
            }
            catch (Exception ex)
            {
                return AllVersions.ToArray() ;
                //throw;
            }

            return AllVersions.ToArray();
        }

        public static bool bIsReleaseVersion(string Name)
        {
            bool RetVal = true;

            if (string.IsNullOrEmpty(Name))
            {
                RetVal = false;
            }

            foreach (char c in Name)
            {
                if (!char.IsDigit(c) && c != '.')
                {
                    RetVal = false;
                }
            }

            return RetVal;
        }

        public static bool bIsOptifine(string Name)
        {
            bool RetVal = false;

            if (string.IsNullOrEmpty(Name))
            {
                RetVal = false;
            }

            if (Name.Contains("OptiFine"))
            {
                RetVal = true;
            }

            return RetVal;
        }

        public static string[] GetAllInstalledVersions()
        {
            List<string> RetVal = new List<string>();

            var path = new MinecraftPath();
            LocalVersionLoader l = new LocalVersionLoader(path);

            var versions = l.GetVersionMetadatas();
            foreach (var item in versions)
            {
                // show all version names
                // use this version name in CreateProcessAsync method.
                RetVal.Add(item.Name);
            }

            return RetVal.ToArray();
        }

        public static bool bIsVersionInstalled(string VersionName)
        {
            bool ret = false;

            List<string> Installedversions = GetAllInstalledVersions().ToList();

            foreach (var version in Installedversions)
            {
                if (version == VersionName)
                {
                    ret = true;
                    break;
                }
            }

            Installedversions.Clear();

            return ret;
        }
    }
}
