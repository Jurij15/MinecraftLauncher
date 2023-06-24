using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherInstallerUpdater
{
    public class Config
    {
        public static string InstallPath = Environment.GetEnvironmentVariable("LocalAppData") + "\\MinecraftLauncher";
        public static string SettingsDirPath = InstallPath + @"\\Launcher\\Settings";
        public static string LauncherPath = InstallPath + "\\Launcher";
        public static string UpdaterDirPath = InstallPath + "\\Updater";

        public static string TempFIlePath = InstallPath + "Temp";
    }
}
