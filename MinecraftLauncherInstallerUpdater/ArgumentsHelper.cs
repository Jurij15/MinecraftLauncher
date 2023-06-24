using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                MessageBox.Show(arg);
                if (arg.Contains("-Update") && !bIsInstalling)
                {
                    bIsInstalling = false;
                    break;
                }
                else if (!bIsUpdating)
                {
                    bIsInstalling = true;
                }
            }
        }
    }
}
