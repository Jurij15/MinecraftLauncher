using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherInstallerUpdater
{
    public class ArgumentsHelper
    {
        public static bool bIsUpdating = false;
        public static bool bIsInstalling = false;
        public static string bVersion = string.Empty;

        public static void ParseLaunchArguments()
        {
            string[] args = Environment.GetCommandLineArgs();

            foreach (var arg in args)
            {
                if (arg.Contains("-Update") && !bIsInstalling)
                {
                    bIsUpdating = true;
                }
                else if (!bIsUpdating)
                {
                    bIsInstalling = true;
                }
            }
        }
    }
}
