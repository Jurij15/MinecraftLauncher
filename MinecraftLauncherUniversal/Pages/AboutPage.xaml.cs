using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Dialogs;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        public AboutPage()
        {
            this.InitializeComponent();
            VersionBox.Text = "Version " + Globals.VersionString;
        }

        private async void CheckForUpdatesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!NetworkService.GetIsConnectedToNetwork())
            {
                ContentDialog dialog = DialogService.CreateContentDialog("Cannot check for updates", "No network connection is available.");
                dialog.CloseButtonText = "Close";
                await dialog.ShowAsync();
                return;
            }
            bool value = Updater.bIsUpToDate();
            if (value)
            {
                StatusBox.Text = "Up to date";
                NoUpdatesIcon.Visibility = Visibility.Visible;
                AditionalStuffBox.Text = "No action needed";
            }
            else if (Updater.bIsPrerelease())
            {
                StatusBox.Text = "Pre-Release";
                CannotUpdateIcon.Visibility = Visibility.Visible;
                AditionalStuffBox.Text = "Updates are disabled on pre-release!";
            }
            else if (!value)
            {
                StatusBox.Text = "Update Available";
                UpdateIcon.Visibility = Visibility.Visible;
                AditionalStuffBox.Text = "Please check GitHub for a new version!";
            }
        }

        private void OpenNewsCard_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(NewsPage), "What's new in " + Globals.VersionString, false);
        }
    }
}
