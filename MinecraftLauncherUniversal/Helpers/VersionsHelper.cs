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

        public static bool bVersionSupportsSkins(string VersionName)
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
