﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.Windows.AppLifecycle;
using MinecraftLauncherUniversal.Core;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Interop;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Management.Deployment;
using WinUIEx;
using WinUIEx.Messaging;

namespace MinecraftLauncherUniversal
{
    public class Globals
    {
        public static string RootDir = GetRootDir();
        public static string SettingsFile = Path.Combine(RootDir, "settings.json");
        public static string RecentBuildsFile = Path.Combine(RootDir, "recents");
        public static int DownloadRateLimit = 1024;
        public static bool bIsFirstTimeRun = false;
        public static HashSet<string> Recents = new HashSet<string>();

        public static NavigationViewItem AboutMenuItem;

        private static string GetRootDir()
        {
            string returnval = "Settings/";
            if (File.Exists(Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "MinecraftLauncher", "launcherConfig.json")))
            {
                InstallerConfigJson json = JsonConvert.DeserializeObject<InstallerConfigJson>(File.ReadAllText(Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "MinecraftLauncher", "launcherConfig.json")));
                if (!json.RunAsPortable)
                {
                    returnval = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "MinecraftLauncher", "Settings");
                }
            }
            //MessageBox.Show(Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "MinecraftLauncher", "Settings"));
            return returnval;
        }

        public static string CurrentVersion {  get; set; }  

        public static string VersionString = "2.1";

        public static bool ToastFailedInit = false;

        public static SettingsJson Settings;

        #region Objects
        public static WindowEx MainWindow;
        public static NavigationView MainNavigation;
        public static BreadcrumbBar MainNavigationBreadcrumb;
        public static Frame MainFrame;
        public static Grid MainGrid;
        public static XamlRoot MainGridXamlRoot;
        public static InfoBadge AllVersionsNavigationViewItemInfoBadge;
        public static Window m_window;
        #endregion

        public static void ResetApp(bool bSendNotification)
        {
            //delete the server directory
            foreach (var item in Directory.GetDirectories(RootDir))
            {
                Directory.Delete(item);
            }
            foreach (var item in Directory.GetFiles(RootDir))
            {
                File.Delete(item);
            }
            if (bSendNotification) { NotificationService.SendSimpleToast("MinecraftLauncher was reset", "Restart was required to complete", 1.9); }

            RestartApp();
        }

        public static async void RestartApp()
        {
            Process p = Process.Start(Regex.Replace(System.Reflection.Assembly.GetExecutingAssembly().Location, "\\.dll", ".exe"));
            Process.GetCurrentProcess().Kill();
        }

        #region Console
        public static List<string> LoggerHistory = new List<string>();

        public static bool bShowConsole = false;
        private static bool _isConsoleVisible = false;

        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();

        public static void SetupConsole()
        {
            AllocConsole();
        }

        [DllImport("kernel32.dll", EntryPoint = "FreeConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int FreeConsole();

        public static void CloseConsole()
        {
            FreeConsole();
        }
        #endregion
    }
}
