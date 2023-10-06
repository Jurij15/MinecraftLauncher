using CmlLib.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Core;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Services;
using MinecraftServersParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages.ServersPages.AddServerPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public class Server
    {
        public string Name;
        public string IP;
        public MServer mServer;
    }

    public sealed partial class ImportServerPage : Page
    {
        List<Server> Servers;
        public ImportServerPage()
        {
            this.InitializeComponent();

            this.DataContext = this;
        }

        private async void MinecraftServerssPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Path.Combine(MinecraftPath.WindowsDefaultPath, "servers.dat")))
            {
                Servers = new List<Server>();
                ServerParser parser = new ServerParser(MinecraftPath.WindowsDefaultPath);

                foreach (var item in parser.GetServers())
                {
                    Server s = new Server();
                    s.Name = item.Name;
                    s.IP = item.IP;
                    s.mServer = item;

                    Servers.Add(s);
                }

                MinecraftServerssPanel.ItemsSource = Servers;
            }
            else
            {
                ContentDialog dialog = new ContentDialog();
                dialog.XamlRoot = Globals.MainGridXamlRoot;
                dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;

                dialog.Title = "Add Server";
                dialog.Content = new Dialogs.AddServerDialog(dialog);

                dialog.Closed += Dialog_Closed;

                await dialog.ShowAsync();
            }
        }

        private void MinecraftServerssPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                StartMessage.Visibility = Visibility.Collapsed;
                ServerDetails.Visibility = Visibility.Visible;

                Server s = MinecraftServerssPanel.SelectedItem as Server;

                ServerNameBox.Text = s.Name;
                ServerIPBox.Text = s.IP;
                ServerPortBox.Value = s.mServer.Port;

                foreach (var item in VersionManager.AllVersionsGlobal)
                {
                    if (VersionsHelper.bIsReleaseVersion(item))
                    {
                        VersionsCombo.Items.Add(item);
                    }
                }
            }
        }

        private void VersionsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem != null)
            {
                ImportBtn.IsEnabled = true;
            }
            else
            {
                SelectVersionTip.IsOpen = true;
            }
        }

        private async void ImportBtn_Click(object sender, RoutedEventArgs e)
        {
            ServersManager manager = new ServersManager();
            ServerJson json = new ServerJson();

            json.ServerName = ServerNameBox.Text;
            json.ServerIP = ServerIPBox.Text;
            json.ServerPort = ServerPortBox.Value.ToString();
            json.ServerVersion = VersionsCombo.SelectedItem.ToString();

            ImportBtn.IsEnabled = false;
            ImportBtn.Content = "Importing...";
            await manager.AddServer(json);

            FontIcon icon = new FontIcon();
            icon.Glyph = "\uE73E";
            ImportBtn.Content = icon;

            await Task.Delay(400);

            NavigationService.Navigate(typeof(ServersPage), "Servers", true);
        }

        private void Dialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            NavigationService.Navigate(typeof(ServersPage), "Servers", true);
        }
    }
}
