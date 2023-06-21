using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.Windows.AppLifecycle;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Management.Deployment;
using WinUIEx;

namespace MinecraftLauncherUniversal
{
    public class Globals
    {
        public static string SQLiteConnectionPath = "Data Source=Profiles.db;Version=3;";

        public static int Theme = 1;
        public static int DownloadRateLimit = 1024;
        public static bool bIsFirstTimeRun = false;
        public static HashSet<string> Recents = new HashSet<string>();

        public static ElementSoundPlayerState SoundPlayerState;

        public static string LastUsedProfileID { get; set; }

        public static string CurrentVersion {  get; set; }  

        public static string Username {  get; set; }
        public static string SubText { get; set; }
        public static string CustomUUID { get; set; }
        public static string AccessToken { get; set; } //this will not save, for now

        public static string VersionString = "2.0 - BETA1";

        #region Objects
        public static WindowEx MainWindow;
        public static NavigationView MainNavigation;
        public static BreadcrumbBar MainNavigationBreadcrumb;
        public static Frame MainFrame;
        public static Grid MainGrid;
        public static XamlRoot MainGridXamlRoot;
        public static InfoBadge AllVersionsNavigationViewItemInfoBadge;
        #endregion

        //i should use binding for this, instead of just constantly updating it
        public static ObservableCollection<string> Breadcrumbs = new ObservableCollection<string>();
        public static void UpdateBreadcrumb()
        {
            MainNavigationBreadcrumb.ItemsSource = Breadcrumbs;
        }

        public static async void ResetApp(bool bSendNotification)
        {
            Directory.Delete(Settings.RootDir, true);
            if (bSendNotification) { NotificationService.SendSimpleToast("MinecraftLauncher was reset", "Restart was required to complete", 1.9); }

            //CustomProfileDataManager manager = new CustomProfileDataManager();
            //manager.DeleteProfile();

            RestartApp();
            CustomProfileDataManager.ResetProfiles();
        }

        public static async void RestartApp()
        {
            Process p = Process.Start("MinecraftLauncherUniversal.exe");
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
            _isConsoleVisible = true;
            foreach (var item in LoggerHistory)
            {
                Console.WriteLine(item);
            }
            Logger.Log("core", "Console Enabled!");
            DialogService.ShowSimpleDialog("", "Console Enabled!");
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
