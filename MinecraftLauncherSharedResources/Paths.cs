using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherSharedResources
{
    public class Paths
    {
        public static string RootAppDataPath = Environment.GetEnvironmentVariable("LocalAppData");
        public static string RootLauncherDir = Path.Combine(RootAppDataPath, "MinecraftLauncher");

        public static string LauncherDir = Path.Combine(RootLauncherDir, "Launcher");
        public static string SettingsDir = Path.Combine(RootLauncherDir, "Settings");
        public static string InstallerDir = Path.Combine(RootLauncherDir, "Installer");

        public static string MinecraftLauncherExecutablePath = Path.Combine(LauncherDir, "MinecraftLauncherUniversal.exe");
    }
}
