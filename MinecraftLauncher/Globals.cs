using MinecraftLauncher.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wpf.Ui.Appearance;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls.BreadcrumbControl;
using Wpf.Ui.Controls.ContentDialogControl;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Controls.SnackbarControl;
using Wpf.Ui.Controls.Window;
using Wpf.Ui.Services;

namespace MinecraftLauncher
{
    public class Globals
    {
        public static ISnackbarService snackbarService;
        public static List<string> AllVersionsArray = new List<string>();

        public static FluentWindow MainWindow;
        public static NavigationView MainNavigation;
        public static NavigationViewItem PlayMenuItem;
        public static BreadcrumbBar MainNavigationBreadcrumb;

        public static ContentPresenter MainContentPresenter;
        public static SnackbarPresenter MainSnackbarPresenter;

        public static ContentDialogService MainContentDialogService;
        public static SnackbarService MainSnackbarService;

        public static string CurrentBuild;

        public static string VersionString = "1.1";

        //configs
        public static string Username { get; set; }
        public static int DownloadRateLimit = 1024;
        public static ThemeType UserTheme { get; set; }
        public static HashSet<string > Recents = new HashSet<string>();

        public static AllVersionsView CurrentView;
    }
}
