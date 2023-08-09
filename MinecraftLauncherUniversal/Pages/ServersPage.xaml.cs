using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Services;
using CommunityToolkit.Labs.WinUI;
using MinecraftLauncherUniversal.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ServersPage : Page
    {
        public ServersPage()
        {
            this.InitializeComponent();
        }

        private async void AddToolbarBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = Globals.MainGridXamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;

            dialog.Title = "Add Server";
            dialog.Content = new Dialogs.AddServerDialog(dialog);

            dialog.Closed += Dialog_Closed;

            await dialog.ShowAsync();
        }

        private void Dialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            LoadGuids();
        }

        void LoadGuids()
        {
            ServersManager manager = new ServersManager();
            string[] guids = manager.GetAllServerGuids();

            foreach (string guid in guids)
            {
                ServerJson json = manager.GetServerJson(guid);

                AddCard(json.ServerName, json.GUID);
            }
        }

        void AddCard(string ServerName, string ServerGUID)
        {
            SettingsCard card = new SettingsCard();
            
            card.Header = ServerName;
            card.IsClickEnabled = false;

            SplitButton button = new SplitButton();
            button.Content = "Play";
            button.Style = Application.Current.Resources["AccentButtonStyle"] as Style;

            card.Content = button;

            ItemsPanel.Items.Add(card);
        }

        private void ItemsPanel_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGuids();
        }
    }
}
