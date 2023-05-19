using MinecraftLauncher.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

namespace MinecraftLauncher.Pages
{
    /// <summary>
    /// Interaction logic for AboutPage.xaml
    /// </summary>
    public partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();

            VersionBlock.Text = "Version: " + Globals.VersionString;
        }

        private void CheckForUpdatesBtn_Click(object sender, RoutedEventArgs e)
        {
            bool value = Updater.bIsUpToDate();
            if (value)
            {
                StatusBox.Text = "Up to date";
                GoodBadge.Visibility = Visibility.Visible;
                AditionalStuffBox.Text = "No action required";
            }
            else if (!value)
            {
                StatusBox.Text = "Update Available";
                BadBadge.Visibility = Visibility.Visible;
                AditionalStuffBox.Text = "Please check GitHub for a new version!";
            }
            CheckUpdatesFlyout.IsOpen = true;
        }
    }
}
