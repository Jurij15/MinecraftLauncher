using CmlLib.Core.VersionLoader;
using CmlLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherUniversal.Managers
{
    public class VersionManager
    {
        public VersionManager() { }

        public async Task<string[]> GetAllVersionsAsync()
        {
            List<string> AllVersions = new List<string>();
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

        public string[] GetAllVersions()
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
                return AllVersions.ToArray();
                //throw;
            }

            return AllVersions.ToArray();
        }

        public bool bIsReleaseVersion(string Name)
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

        public bool bIsOptifine(string Name)
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

        public string[] GetAllInstalledVersions()
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

        public bool bIsVersionInstalled(string VersionName)
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

        public bool bVersionSupportsSkins(string VersionName)
        {
            bool ret = false;
            if (VersionName.Contains("ForgeOptiFine 1.17.1"))
            {
                ret = true;
            }

            return ret;
        }
    }
}
