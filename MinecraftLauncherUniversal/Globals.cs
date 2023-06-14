using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Management.Deployment;

namespace MinecraftLauncherUniversal
{
    public class Globals
    {
        public static int Theme = 1;
        public static int DownloadRateLimit = 1024;
        public static bool bIsFirstTimeRun = false;
        public static HashSet<string> Recents = new HashSet<string>();

        public static ElementSoundPlayerState SoundPlayerState;

        public static string CurrentVersion {  get; set; }  

        public static string Username {  get; set; }
        public static string SubText { get; set; }
        public static string CustomUUID { get; set; }
        public static string AccessToken { get; set; } //this will not save, for now

        public static string VersionString = "2.0 - BETA1";

        public static Window MainWindow;
        public static NavigationView MainNavigation;
        public static BreadcrumbBar MainNavigationBreadcrumb;
        public static Frame MainFrame;
        public static Grid MainGrid;
        public static XamlRoot MainGridXamlRoot;

        //i should use binding for this, instead of just constantly updating it
        public static ObservableCollection<string> Breadcrumbs = new ObservableCollection<string>();
        public static void UpdateBreadcrumb()
        {
            MainNavigationBreadcrumb.ItemsSource = Breadcrumbs;
        }
    }
}
