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
        string GetCurrentID()
        {
            return _id;
        }
        public PlayerSettingsPage()
        {
            this.InitializeComponent();

            UsernameSettingsBox.Text = Globals.Username;
            SubText.Text = Globals.SubText;
        }

        private void UsernameSettingsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Profile p = new Profile(GetCurrentID());
            if (!string.IsNullOrEmpty(UsernameSettingsBox.Text) || !string.IsNullOrWhiteSpace(UsernameSettingsBox.Text))
            {
                Globals.Username = UsernameSettingsBox.Text;
                Settings.SaveNewUsername();
                p.Update(UsernameSettingsBox.Text);
            }
            if (!string.IsNullOrEmpty(SubText.Text) || !string.IsNullOrWhiteSpace(SubText.Text))
            {
                Globals.SubText = SubText.Text;
                Settings.SaveNewSubText();
                p.Update(null, SubText.Text);
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            #region Init Profiles 
            ProfileManager manager = new ProfileManager();
            List<string> ids = await manager.GetAllIDS();
            foreach (var id in ids)
            {
                Profile p = new Profile(id);

                ComboBoxItem item = new ComboBoxItem();
                StackPanel content = new StackPanel();

                TextBlock username = new TextBlock();
                TextBlock subText = new TextBlock();

                username.Text = p.GetUsername();
                subText.Text = p.GetSubtext();

                username.FontSize = 16;
                username.FontWeight = FontWeights.Medium;

                item.Name = id;

                content.Children.Add(username);
                content.Children.Add(subText);

                item.Content = content;

                ProfileSelector.Items.Add(item);
            }
            #endregion
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
            _id = ((ComboBoxItem)ProfileSelector.SelectedItem).Name;
        }

        private async void AddProfileCard_Click(object sender, RoutedEventArgs e)
        {
            ProfileManager manager = new ProfileManager();
            await manager.AddNewUser("Player", "A Minecraft Player"); //default
        }

        private void RemoveProfileCard_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem item = ((ComboBoxItem)ProfileSelector.SelectedItem);
            Profile p = new Profile(item.Name);
            p.Delete();
        }
    }
}
