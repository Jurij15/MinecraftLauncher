using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Appearance;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Controls.Window;

namespace MinecraftLauncher
{
    public class Globals
    {
        public static ISnackbarService snackbarService;
        public static List<string> AllVersionsArray = new List<string>();

        public static FluentWindow MainWindow;
        public static NavigationView MainNavigation;
        public static NavigationViewItem PlayMenuItem;

        public static string CurrentBuild;

        public static string VersionString = "0.1";

        //configs
        public static string Username { get; set; }
        public static int DownloadRateLimit = 1024;
        public static ThemeType UserTheme { get; set; }
        public static List<string > Recents = new List<string>();
    }
}
