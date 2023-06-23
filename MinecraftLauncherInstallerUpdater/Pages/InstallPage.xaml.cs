using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinecraftLauncherInstallerUpdater.Pages
{
    /// <summary>
    /// Interaction logic for InstallPage.xaml
    /// </summary>
    public partial class InstallPage : Page
    {
        public InstallPage()
        {
            InitializeComponent();

            PathBox.Text = Config.InstallPath;


            StatusBlock.Text = "";
        }

        private void PinToTaskbarCheck_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void PinToTaskbarCheck_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CreateDesktopShortcut_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CreateDesktopShortcut_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
