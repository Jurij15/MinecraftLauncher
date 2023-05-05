using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Controls.Window;

namespace MinecraftLauncher
{
    public class Globals
    {
        public static List<string> AllVersionsArray = new List<string>();

        public static string Username { get; set; }

        public static FluentWindow MainWindow;
        public static NavigationView MainNavigation;
        public static NavigationViewItem PlayMenuItem;

        public static string CurrentBuild;
    }
}
