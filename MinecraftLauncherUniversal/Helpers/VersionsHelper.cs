using CmlLib.Core;
using CmlLib.Core.VersionLoader;
using MinecraftLauncherUniversal.Managers;
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

        public static bool bIsVersionInstalled(string VersionName)
        {
            bool ret = false;

            foreach (var version in VersionManager.AllVersionsGlobal)
            {
                if (version == VersionName)
                {
                    ret = true;
                    break;
                }
            }

            return ret;
        }
    }
}
