using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Dialogs;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Pages.SetupPages;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartupPage : Page
    {
        int LoadTimeout = 6000;
        public StartupPage()
        {
            this.InitializeComponent();
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.TitleBarPaneToggleButton.Visibility = Visibility.Collapsed;

            //just to test, i like it more like this
            if (Globals.Settings.Theme == ElementTheme.Dark)
            {
                Ring.Foreground = new SolidColorBrush(Microsoft.UI.Colors.LightGreen) as SolidColorBrush;
            }
            else if(Globals.Settings.Theme == ElementTheme.Light)
            {
                Ring.Foreground = new SolidColorBrush(Microsoft.UI.Colors.DarkGreen) as SolidColorBrush;
            }

            await Task.Delay(100);//wait for the actual page to load

            Task taskToAwait = PreloadArrays();

            Task completedTask = await Task.WhenAny(taskToAwait, Task.Delay(LoadTimeout));

            if (completedTask == taskToAwait)
            {
                if (Globals.bIsFirstTimeRun)
                {
                    MainWindow.MainWindowFrame.Navigate(typeof(SetupRootPage));
                }
                else
                {
                    // task completed within timeout
                    ThemeService.BackdropExtension.SetBackdrop(ThemeService.BackdropExtension.Backdrop.Mica);

                    //fix for if the arrays are empty for some reason
                    if (VersionManager.AllVersionsGlobal.Count < 1)
                    {
                        DialogService.ShowSimpleDialog("Error", "An error occured while loading versions. Please restart your launcher!");
                    }

                    MainWindow.TitleBarPaneToggleButton.Visibility = Visibility.Visible;
                    DrillInNavigationTransitionInfo info = new DrillInNavigationTransitionInfo();
                    MainWindow.MainWindowFrame.Navigate(typeof(ShellPage), null, info);
                }
            }
            else
            {
                // timeout logic
                ContentDialog ServerConnectionFailedDialog = new ContentDialog();
                ServerConnectionFailedDialog.XamlRoot = this.Content.XamlRoot;
                ServerConnectionFailedDialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                ServerConnectionFailedDialog.Title = "Error";
                ServerConnectionFailedDialog.Content = "Failed to connect to Mojang Servers! Please check your internet connection and restart the app!";

                ServerConnectionFailedDialog.CloseButtonText = "Close App";
                ServerConnectionFailedDialog.CloseButtonClick += ServerConnectionFailedDialog_CloseButtonClick;

                ServerConnectionFailedDialog.PrimaryButtonText = "Restart App";
                ServerConnectionFailedDialog.PrimaryButtonClick += ServerConnectionFailedDialog_PrimaryButtonClick;

                ServerConnectionFailedDialog.DefaultButton = ContentDialogButton.Primary;

                await ServerConnectionFailedDialog.ShowAsync();
            }
        }

        private void ServerConnectionFailedDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Globals.RestartApp();
        }

        async Task PreloadArrays()
        {
            ContentDialog loaddialog = new ContentDialog();
            loaddialog.XamlRoot = this.Content.XamlRoot;
            loaddialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            loaddialog.Content = new SimpleLoadingDialog("Preparing...");

            //loaddialog.ShowAsync();

            if (NetworkInformation.GetInternetConnectionProfile()?.NetworkAdapter == null)
            {
                ContentDialog Nonetworkdialog = new ContentDialog();
                Nonetworkdialog.XamlRoot = this.Content.XamlRoot;
                Nonetworkdialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                Nonetworkdialog.Title = "Error";
                Nonetworkdialog.Content = "Network is not available. Launcher might not function properly!";

                Nonetworkdialog.CloseButtonText = "Close App";
                Nonetworkdialog.CloseButtonClick += ServerConnectionFailedDialog_CloseButtonClick;

                loaddialog.Hide();
                await Nonetworkdialog.ShowAsync();
            }
            VersionManager manager = new VersionManager();
            await manager.PreloadVersionArrays();

            //ShowMessageDialogAsync(VersionManager.AllVersionsGlobal.Count.ToString(), "test");

            if (VersionManager.AllVersionsGlobal.Count <= 0)
            {
                ContentDialog dialog = new ContentDialog();
                dialog.XamlRoot = this.Content.XamlRoot;
                dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                dialog.Title = "Error";
                dialog.Content = "An Error occured while searching for all available versions. Please restart the app";

                dialog.CloseButtonText = "OK";
                dialog.CloseButtonClick += Dialog_CloseButtonClick;

                loaddialog.Hide();
                await dialog.ShowAsync();
            }
            else
            {
                loaddialog.Content = new SimpleLoadingDialog("Done...");
                await manager.PrefetchStats();
            }

            loaddialog.Hide();
        }

        private void ServerConnectionFailedDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Application.Current.Exit();
        }

        private void Nonetworkdialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            sender.Hide();
        }


        private void Dialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Globals.RestartApp();
        }
    }
}
