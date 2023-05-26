using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MinecraftLauncherUniversal
{
    public class Globals
    {
        public static Page MainPage;
        public static Microsoft.UI.Xaml.Controls.NavigationView MainNavigation;
        public static BreadcrumbBar MainBreadcrumb;
        public static Frame RootFrame;





        public static List<string> AllVersionsArray = new List<string>();

        public static string CurrentBuild;

        public static string VersionString = "1.0";

        //configs
        public static string Username = "Steve";
        public static int DownloadRateLimit = 1024;
        public static HashSet<string > Recents = new HashSet<string>();
        public static int Theme = 0; //0 = Dark, 1 = Light

        public static ObservableCollection<string> CurrentMainBreadcrumbDisplay = new ObservableCollection<string>();

        public static void UpdateBreadcrumb()
        {
            Globals.MainBreadcrumb.ItemsSource = Globals.CurrentMainBreadcrumbDisplay;
        }
    }
}
