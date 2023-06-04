using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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
        public static int Theme = 0;
        public static int DownloadRateLimit = 1024;
        public static bool bIsFirstTimeRun = false;

        public static string CurrentVersion {  get; set; }  

        public static string Username {  get; set; }

        public static string VersionString = "2.0 - DEV1";

        public static Window MainWindow;
        public static NavigationView MainNavigation;
        public static BreadcrumbBar MainNavigationBreadcrumb;
        public static Frame MainFrame;

        //i should use binding for this, instead of just constantly updating it
        public static ObservableCollection<string> Breadcrumbs = new ObservableCollection<string>();
        public static void UpdateBreadcrumb()
        {
            MainNavigationBreadcrumb.ItemsSource = Breadcrumbs;
        }
    }
}
