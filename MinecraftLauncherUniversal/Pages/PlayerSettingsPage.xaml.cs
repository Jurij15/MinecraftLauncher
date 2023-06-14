using CmlLib.Core;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Core;
using MinecraftLauncherUniversal.Dialogs;
using MinecraftLauncherUniversal.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public PlayerSettingsPage()
        {
            this.InitializeComponent();

            UsernameSettingsBox.Text = Globals.Username;
            SubText.Text = Globals.SubText;
            //new Thickness(Height, 0, 0, 0);

            /*
            if (ProfileManager.bIsSkinFilePresent())
            {
                SkinSet.Visibility = Visibility.Visible;
                SkinStatus.Text = "Skin Set";
            }
            else
            {
                SkinNotSet.Visibility = Visibility.Visible;
                SkinStatus.Text = "Skin Not Set";
            }
            */
        }

        private void UsernameSettingsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(UsernameSettingsBox.Text) || !string.IsNullOrWhiteSpace(UsernameSettingsBox.Text))
            {
                Globals.Username = UsernameSettingsBox.Text;
                Settings.SaveNewUsername();
            }
            if (!string.IsNullOrEmpty(SubText.Text) || !string.IsNullOrWhiteSpace(SubText.Text))
            {
                Globals.SubText = SubText.Text;
                Settings.SaveNewSubText();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
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
    }
}
