using MinecraftLauncher.Helpers;
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

namespace MinecraftLauncher.Pages
{
    /// <summary>
    /// Interaction logic for PlayPage.xaml
    /// </summary>
    public partial class PlayPage : Page
    {
        public PlayPage()
        {
            InitializeComponent();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            AsyncLaunch();
        }

        async void AsyncLaunch()
        {
            int memooryinmb = Convert.ToInt32(RamAmountBox.Value) * 1024;

            LaunchingBar.Visibility = Visibility.Visible;

            await PlayHelper.Launch(Globals.CurrentBuild, memooryinmb, Convert.ToBoolean(FullScreenMode.IsChecked));
            LaunchingBar.Visibility = Visibility.Collapsed;
            //Globals.snackbarService.Show("Minecraft Launched", "Version " + Globals.CurrentBuild + " was launched", Wpf.Ui.Controls.ControlAppearance.Info, null, TimeSpan.FromSeconds(2));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Globals.CurrentBuild.Contains("OptiFine"))
            {
                VersionNameBlock.Text =Globals.CurrentBuild;
            }
            else
            {
                VersionNameBlock.Text = "Minecraft " + Globals.CurrentBuild;
            }
            UsernameBoxPlayPage_TextChanged(sender, null);
            LaunchingBar.Visibility = Visibility.Collapsed;

            UsernameBoxPlayPage.Text  = Globals.Username;

            bool installed = VersionsHelper.bIsVersionInstalled(Globals.CurrentBuild);

            if (installed)
            {
                PlayButton.Visibility = Visibility.Visible;
            }
            else if (!installed)
            {
                DownloadButton.Visibility = Visibility.Visible;
            }
            else
            {
                VersionNotDetected.Visibility = Visibility.Visible;
            }
        }

        private void UsernameBoxPlayPage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameBoxPlayPage.Text))
            {
                PlayButton.IsEnabled = false;
            }
            else
            {
                Globals.Username = UsernameBoxPlayPage.Text;
                PlayButton.IsEnabled = true;
            }
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadBefore();
        }

        async void DownloadBefore()
        {
            int memooryinmb = Convert.ToInt32(RamAmountBox.Value) * 1024;

            LaunchingBar.Visibility = Visibility.Visible;

            await PlayHelper.Download(Globals.CurrentBuild, memooryinmb, Convert.ToBoolean(FullScreenMode.IsChecked));
            LaunchingBar.Visibility = Visibility.Collapsed;
            
            DownloadButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
        }
    }
}
