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
using static CommunityToolkit.WinUI.UI.Animations.Expressions.ExpressionValues;
using System.Threading.Tasks;
using MinecraftLauncherUniversal.Core;
using MinecraftLauncherUniversal.Services;
using WinUIEx;
using CmlLib.Utils;

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
            if (Globals.bIsFirstTimeRun)
            {
                OpenUsernameTip();
            }
        }

        async void OpenUsernameTip()
        {
            UsernameTip.Target = UsernameBox;
            // bugbug: without this delay, the tip opens, but won't close
            await Task.Delay(100);

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

        private async void Page_Loaded(object sender, RoutedEventArgs e)
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

            if (VersionsHelper.bVersionSupportsSkins(Globals.CurrentVersion))
            {
                ShowSkinCheck.IsChecked = true;
            }
        }

        async void DownloadBefore()
        {
            DownloadButton.Visibility = Visibility.Collapsed;
            StatusBox.Text = "Downloading...";
            int memooryinmb = Convert.ToInt32(RamAmountBox.Value) * 1024;

            LoadingRing.Visibility = Visibility.Visible;

            PlayCore core = new PlayCore(Globals.CurrentVersion, memooryinmb, Convert.ToBoolean(FullscreenCheck.IsChecked), Globals.CustomUUID, Globals.AccessToken);
            await core.Download(OnProgressChanged);
            LoadingRing.Visibility = Visibility.Collapsed;
            DownloadButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
            StatusBox.Text = "Ready to Play";
        }

        async void OnProgressChanged(int value)
        {
            LoadingRing.IsIndeterminate = false;
            LoadingRing.Value = value;
        }

        async void AsyncLaunch()
        {
            bool bSucess = false;
            int memooryinmb = Convert.ToInt32(RamAmountBox.Value) * 1024;

            LoadingRing.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Collapsed;
            StatusBox.Text = "Launching...";

            PlayCore core = new PlayCore(Globals.CurrentVersion, memooryinmb, Convert.ToBoolean(FullscreenCheck.IsChecked), Globals.CustomUUID, Globals.AccessToken);
            bool result = await core.Launch();
            if (!result) { DialogService.ShowSimpleDialog("An Error Occured", core.GetLaunchErrors()); } else { bSucess = true; }   
            LoadingRing.Visibility = Visibility.Collapsed;
            StatusBox.Text = "Playing";

            //by now, it has already been launched, now store the build in recents
            Settings.SaveRecentBuild(Globals.CurrentVersion);
            MinecraftLaunchedInfo.IsOpen = true;
            PlayButton.Visibility = Visibility.Visible;

            if (bSucess)
            {
                await Task.Delay(800);

                Globals.MainWindow.Minimize();
            }
        }

        private void UsernameTip_CloseButtonClick(TeachingTip sender, object args)
        {
            sender.IsOpen = false;
        }

        private void UsernameTip_Closing(TeachingTip sender, TeachingTipClosingEventArgs args)
        {
            //UsernameTip.Content = "null";
        }

        private void QuickPlayerSettingsExpander_Collapsed(Expander sender, ExpanderCollapsedEventArgs args)
        {
            UsernameTip.IsOpen = false;
        }

        private void ShowSkinCheck_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
