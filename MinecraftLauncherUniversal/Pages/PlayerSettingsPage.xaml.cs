using CmlLib.Core;
using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.WindowsAppSDK.Runtime;
using MinecraftLauncherUniversal.Core;
using MinecraftLauncherUniversal.Dialogs;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.NetworkOperators;
using Windows.Perception.Spatial.Preview;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayerSettingsPage : Page
    {
        string _id;
        bool bUnloading = false;
        bool bRestartingAfterAccDeleted = false;
        string GetCurrentID()
        {
            return _id;
        }
        public PlayerSettingsPage()
        {
            this.InitializeComponent();

            UsernameSettingsBox.Text = Globals.Settings.Username;
        }

        private void UsernameSettingsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshBtn.Visibility = Visibility.Visible;
        }

        private async void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (bRestartingAfterAccDeleted)
            {
                return;
            }

            ProfileSelector.Items.Clear();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private async void ExploreBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(MinecraftPath.WindowsDefaultPath + "\\cachedImages\\skins"))
            {
                Directory.CreateDirectory(MinecraftPath.WindowsDefaultPath + "\\cachedImages\\skins");
            }

            Process.Start("explorer.exe", @MinecraftPath.WindowsDefaultPath + "\\cachedImages\\skins");
        }

        private void AddSkinFileFlyoutButton_Click(object sender, RoutedEventArgs e)
        {
            AddSkinFileFlyout.Hide();
        }

        private void NewSkinFilePath_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private async void AdvancedDialogBtn_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush transparentBrush = new SolidColorBrush(Colors.Transparent);

            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = Globals.MainGridXamlRoot;
            dialog.Title = "Advanced Settings";
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Content = new AdvancedPlayerSettingsDialogContent();
            //dialog.Background = transparentBrush;

            dialog.CloseButtonText = "Exit";
            dialog.CloseButtonClick += Dialog_CloseButtonClick;

            dialog.PrimaryButtonText = "Save";
            dialog.PrimaryButtonClick += Dialog_PrimaryButtonClick;

            dialog.DefaultButton = ContentDialogButton.Close;

            await dialog.ShowAsync();
        }

        private void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void Dialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            sender.Hide();
        }

        private void ProfileSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void AddProfileCard_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void RemoveProfileCard_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog loaddialog = new ContentDialog();
            loaddialog.XamlRoot = this.Content.XamlRoot;
            loaddialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            loaddialog.Title = "Are you sure?";
            loaddialog.Content = "You are about to delete a profile with name " + UsernameBlock.Text + ". \n This action canot be undone!";

            loaddialog.CloseButtonText = "Cancel";
            loaddialog.CloseButtonClick += Loaddialog_CloseButtonClick;

            loaddialog.PrimaryButtonText = "Delete";
            loaddialog.PrimaryButtonClick += Loaddialog_PrimaryButtonClick;

            loaddialog.DefaultButton = ContentDialogButton.Close;

            await loaddialog.ShowAsync();
        }

        private void Loaddialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            NavigationService.Navigate(typeof(SettingsPage), "Settings", true);
            NavigationService.Navigate(typeof(PlayerSettingsPage), "Player Settings", false);
        }

        private void Loaddialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            sender.Hide();
        }
    }
}
