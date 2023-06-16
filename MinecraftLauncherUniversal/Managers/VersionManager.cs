using CmlLib.Core.VersionLoader;
using CmlLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftLauncherUniversal.Helpers;
using Windows.Devices.WiFi;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace MinecraftLauncherUniversal.Managers
{
    public class VersionManager
    {
        public class PrefetchedStatistics //like how many builds are installed, total available, optifine etc.
        {
            public static int TotalAvailable;
            public static int TotalInstalled;
            public static int TotalReleases;
            public static int TotalOptiFine;
        }
        public static HashSet<string> AllVersionsGlobal = new HashSet<string>();
        public VersionManager() { }

        public async Task<string[]> GetAllVersionsAsync()
        {
            List<string> AllVersions = new List<string>();
            try
            {
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
            }
            catch (Exception)
            {
                ContentDialog dialog = new ContentDialog();
                dialog.XamlRoot = Globals.MainGridXamlRoot;
                dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                dialog.Title = "Error";
                dialog.Content = "An Error occured while searching for all available versions. Please restart the app";

                dialog.CloseButtonText = "OK";
                dialog.CloseButtonClick += Dialog_CloseButtonClick;
                throw;
            }

            return AllVersions.ToArray();
        }

        private void Dialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Globals.RestartApp();
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

        public async Task PreloadVersionArrays()
        {
            foreach (var item in await GetAllVersionsAsync())
            {
                AllVersionsGlobal.Add(item);
            }
        }

        public async Task PrefetchStats()
        {
            PrefetchedStatistics.TotalAvailable = AllVersionsGlobal.Count;
            foreach (var ver in AllVersionsGlobal)
            {
                if (bIsVersionInstalled(ver))
                {
                    PrefetchedStatistics.TotalInstalled++;
                }
                if (bIsOptifine(ver))
                {
                    PrefetchedStatistics.TotalOptiFine++;
                }
                if (bIsReleaseVersion(ver))
                {
                    PrefetchedStatistics.TotalReleases++;
                }
                if (bIsVersionInstalled(ver))
                {
                    PrefetchedStatistics.TotalInstalled++;
                }
            }
        }
    }
}
