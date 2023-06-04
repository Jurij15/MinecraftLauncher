using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Versioning;
using Windows.Foundation;
using Windows.Foundation.Collections;
using MinecraftLauncherUniversal.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SelectedVersionPage : Page
    {
        public SelectedVersionPage()
        {
            this.InitializeComponent();
            UsernameBox.Text = Globals.Username;

            UsernameTip.Target = UsernameBox;
            UsernameTip.IsOpen = true;
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadBefore();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            AsyncLaunch();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Globals.CurrentVersion.Contains("OptiFine"))
            {
                MinecraftVersionBlock.Text = Globals.CurrentVersion;
            }
            else
            {
                MinecraftVersionBlock.Text = "Minecraft " + Globals.CurrentVersion;
            }

            if (VersionsHelper.bIsVersionInstalled(Globals.CurrentVersion))
            {
                //the version is installed and ready to launch
                PlayButton.Visibility = Visibility.Visible;
                DownloadButton.Visibility = Visibility.Collapsed;
                StatusBox.Text = "Ready To Play";
            }
            else
            {
                DownloadButton.Visibility = Visibility.Visible;
                PlayButton.Visibility = Visibility.Collapsed;
                StatusBox.Text = "Download Required";
            }
        }

        async void DownloadBefore()
        {
            DownloadButton.Visibility = Visibility.Collapsed;
            StatusBox.Text = "Downloading...";
            int memooryinmb = Convert.ToInt32(RamAmountBox.Value) * 1024;

            LoadingRing.Visibility = Visibility.Visible;

            await PlayHelper.Download(Globals.CurrentVersion, memooryinmb, Convert.ToBoolean(FullscreenCheck.IsChecked));
            LoadingRing.Visibility = Visibility.Collapsed;
            DownloadButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
            StatusBox.Text = "Ready to Play";
        }

        async void AsyncLaunch()
        {
            int memooryinmb = Convert.ToInt32(RamAmountBox.Value) * 1024;

            LoadingRing.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Collapsed;
            StatusBox.Text = "Launching...";

            await PlayHelper.Launch(Globals.CurrentVersion, memooryinmb, Convert.ToBoolean(FullscreenCheck.IsChecked));
            LoadingRing.Visibility = Visibility.Collapsed;
            StatusBox.Text = "Playing";

            //by now, it has already been launched, now store the build in recents
            Settings.SaveRecentBuild(Globals.CurrentVersion);
            MinecraftLaunchedInfo.IsOpen = true;
            PlayButton.Visibility = Visibility.Visible;
        }

        private void UsernameTip_CloseButtonClick(TeachingTip sender, object args)
        {
            sender.IsOpen = false;
        }
    }
}
