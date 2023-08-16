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
using WinUIEx.Messaging;
using Windows.UI.Popups;
using MinecraftLauncherUniversal.Interop;
using Windows.UI.WebUI;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Animation;
using MinecraftLauncherUniversal.Helpers;
using MineStatLib;
using Windows.ApplicationModel.Contacts;
using Microsoft.UI.Xaml.Media.Imaging;
using CmlLib.Core.Mojang;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ServersPage : Page
    {
        bool _isPageUnloading = false;
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

        async void LoadGuids()
        {
            ItemsPanel.Items.Clear();
            ServersManager manager = new ServersManager();
            string[] guids = manager.GetAllServerGuids();

            foreach (string guid in guids)
            {
                ServerJson json = manager.GetServerJson(guid);

                AddCard(json.ServerName, json.GUID, json);
            }

            await Task.Delay(400);//wait for servers to show

            LoadStatus();
        }

        async void LoadStatus()
        {
            LoadingStatus.Visibility = Visibility.Visible;

            await Task.Delay(100);//wait for status to show

            foreach (SettingsCard item in ItemsPanel.Items)
            {
                if (_isPageUnloading)
                {

                }
                ServerJson JsonClass = item.Tag as ServerJson;
                InfoBadge statusbadge = null ;

                //find the statusbadge in the card
                foreach (var carddesc in (item.Description as StackPanel).Children)
                {
                    if (carddesc.GetType() == typeof(StackPanel))
                    {
                        foreach (var panel in (carddesc as StackPanel).Children)
                        {
                            if (panel.GetType() == typeof(InfoBadge))
                            {
                                if ((panel as InfoBadge).Tag == "StatusBadge")
                                {
                                    statusbadge = panel as InfoBadge; break;
                                }
                            }
                        }
                    }
                }

                if (statusbadge == null)
                {
                    MessageBox.Show("Failed to find status as badge is missing! Please Retry!", "Error");
                    return; //an error occurred before while it was loading or something idk
                }

                //bool status = await Task.Run<bool>(ServersHelper.IsServerOnline(JsonClass.ServerIP, JsonClass.ServerPort, 2)));

                if (ServersHelper.IsServerOnline(JsonClass.ServerIP, JsonClass.ServerPort, 2))
                {
                    statusbadge.Style = Application.Current.Resources["SuccessIconInfoBadgeStyle"] as Style;
                    ToolTipService.SetToolTip(statusbadge, "Online");
                }
                else
                {
                    statusbadge.Style = Application.Current.Resources["CriticalIconInfoBadgeStyle"] as Style;
                    ToolTipService.SetToolTip(statusbadge, "Offline");
                }

                await Task.Delay(50); //wait for the status to appear
            }

            LoadingStatus.Visibility = Visibility.Collapsed;
        }

        async void AddCard(string ServerName, string ServerGUID, ServerJson JsonClass)
        {
            SettingsCard card = new SettingsCard();
            
            //card basics
            card.Header = ServerName;
            card.FontSize = 16;
            card.IsClickEnabled = true;
            card.Tag = JsonClass;
            card.Click += Card_Click;
            card.IsActionIconVisible = false;

            //description
            StackPanel rowPanel = new StackPanel();
            StackPanel statusColumnPanel = new StackPanel();

            statusColumnPanel.Orientation = Orientation.Horizontal;
            statusColumnPanel.Spacing = 2;
            rowPanel.Orientation = Orientation.Vertical;

            InfoBadge statusbadge = new InfoBadge();
            statusbadge.Tag = "StatusBadge";
            statusbadge.Style = Application.Current.Resources["InformationalIconInfoBadgeStyle"] as Style;
            ToolTipService.SetToolTip(statusbadge, "Checking");

            TextBlock statusblock = new TextBlock();
            statusblock.Text = "";
            statusblock.Style = Application.Current.Resources["CaptionTextBlockStyle"] as Style;
            statusblock.Foreground = Application.Current.Resources["TextFillColorSecondaryBrush"] as SolidColorBrush;

            statusColumnPanel.Children.Add(statusbadge);
            statusColumnPanel.Children.Add(statusblock);

            TextBlock versionblock = new TextBlock();
            statusblock.Text = JsonClass.ServerVersion;
            statusblock.Style = Application.Current.Resources["CaptionTextBlockStyle"] as Style;
            statusblock.Foreground = Application.Current.Resources["TextFillColorTertiaryBrush"] as SolidColorBrush;

            rowPanel.Children.Add(statusColumnPanel);
            //rowPanel.Children.Add(versionblock);

            card.Description = rowPanel;

            //button
            SplitButton button = new SplitButton();
            button.Tag = JsonClass;
            button.Content = "Play";
            button.Click += Button_Click;
            //button.Style = Application.Current.Resources["AccentButtonStyle"] as Style; //this doest work

            //flyout and items
            MenuFlyout flyout = new MenuFlyout();
            MenuFlyoutItem edititem = new MenuFlyoutItem();
            MenuFlyoutItem deleteitem = new MenuFlyoutItem();
            MenuFlyoutItem moreStatsitem  = new MenuFlyoutItem();

            edititem.Text = "Edit";
            edititem.Icon = new SymbolIcon() {Symbol = Symbol.Edit };
            edititem.Tag = JsonClass;
            //implement editing a server

            deleteitem.Text = "Delete";
            deleteitem.Icon = new SymbolIcon() { Symbol = Symbol.Delete };
            deleteitem.Tag = JsonClass;
            deleteitem.Click += Deleteitem_Click;

            moreStatsitem.Text = "More";
            moreStatsitem.Icon = new SymbolIcon() { Symbol = Symbol.More };
            moreStatsitem.Tag = JsonClass;

            //flyout.Items.Add(edititem);//TODO: Finish This
            flyout.Items.Add(deleteitem);
            //flyout.Items.Add(moreStatsitem); //i dont think we need this anymore

            //transitions
            TransitionCollection transitions = new TransitionCollection
            {
                new ContentThemeTransition()
            };

            button.ContentTransitions = transitions;

            //other stuff
            button.Flyout = flyout;

            card.Content = button;

            ItemsPanel.Items.Add(card);
        }

        private async void Card_Click(object sender, RoutedEventArgs e)
        {
            MoreStatusPanel.Visibility = Visibility.Visible;

            ServerJson json = (sender as SettingsCard).Tag as ServerJson;

            ServerNameBlock.Text = json.ServerName;
            ServerIPBlock.Text = json.ServerIP;
            ServerPortBlock.Text = json.ServerPort;

            PlayBtn.Tag = json;

            //reset stats that will be loaded
            FaviconImage.Source = null;
            ServerLatencyBlock.Text = null;
            ServerGamemodeBlock.Text = null;

            LoadingServerStats.Visibility = Visibility.Visible;

            await Task.Delay(50);

            MineStat server = ServersHelper.GetServer(json.ServerIP, json.ServerPort);

            BitmapImage icon = await ImageHelper.GetBitmapAsync(server.FaviconBytes);
            FaviconImage.Source = icon;

            await Task.Delay(50);

            if (server.ServerUp)
            {
                ServerStatusBlock.Text = "Online";
            }
            else
            {
                ServerStatusBlock.Text = "Offline";
            }

            await Task.Delay(50);

            ServerLatencyBlock.Text = server.Latency.ToString() + " ms";
            await Task.Delay(50);
            ServerGamemodeBlock.Text = server.Gamemode;
            await Task.Delay(50);

            LoadingServerStats.Visibility = Visibility.Collapsed;
        }

        private void Deleteitem_Click(object sender, RoutedEventArgs e)
        {
            SplitButton btn = null;
            MenuFlyoutItem button = sender as MenuFlyoutItem;
            ServerJson json = button.Tag as ServerJson;
            foreach (SettingsCard item in ItemsPanel.Items)
            {
                ServerJson j = item.Tag as ServerJson;
                if (j.GUID == json.GUID)
                {
                    btn = item.Content as SplitButton;
                }
            }

            if (btn == null)
            {
                return;
            }

            Flyout flyout = new Flyout();

            StackPanel content = new StackPanel();
            content.Spacing = 12;

            TextBlock text = new TextBlock();
            text.Text = "Are you sure you want to delete this server?";
            text.Style = Application.Current.Resources["CaptionTextBlockStyle"] as Style;

            Button confirm = new Button();
            confirm.Tag = button.Tag;
            confirm.Content = "Yes";
            confirm.Click += Confirm_Click;

            content.Children.Add(text);
            content.Children.Add(confirm);

            flyout.Content = content;

            flyout.ShowAt(btn, new FlyoutShowOptions() { Placement  = FlyoutPlacementMode.Bottom, ShowMode = FlyoutShowMode.Auto});
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string guid = (((Button)sender).Tag as ServerJson).GUID;

            ServersManager manager = new ServersManager();
            manager.DeleteServer(guid);

            LoadGuids();
        }

        public async void Button_Click(SplitButton sender, SplitButtonClickEventArgs args)
        {
            //sender.Height = sender.ActualHeight;
            //sender.Width = sender.ActualWidth;

            ProgressRing ring = new ProgressRing();
            ring.IsIndeterminate = true;
            //ring.Height = sender.ActualHeight;
            //ring.Width = sender.ActualWidth;
            sender.Content = ring;

            ServerJson json = sender.Tag as ServerJson;
            PlayCore core = new PlayCore(json.ServerVersion, Globals.Settings.MemoryAllocationInGB * 1024, Globals.Settings.Fullscreen, Globals.Settings.CustomUUID, Globals.Settings.CustomAccessToken);
            await core.LaunchServer(json.ServerIP, json.ServerPort);

            FontIcon icon = new FontIcon();
            icon.Glyph = "\uE73E";
            sender.Content = icon;

            await Task.Delay(950);

            sender.Content = "Play";
        }

        private void ItemsPanel_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void RefreshToolbarBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadGuids();
        }

        private void PlayerSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(SettingsPage), "Settings", true);
            NavigationService.Navigate(typeof(PlayerSettingsPage), "Player Settings", false);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadGuids();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error occured");
                throw;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _isPageUnloading = true;
        }

        private async void PlayBtn_Click(object sende, RoutedEventArgs e)
        {
            //sender.Height = sender.ActualHeight;
            //sender.Width = sender.ActualWidth;

            Button sender = sende as Button;

            ProgressRing ring = new ProgressRing();
            ring.IsIndeterminate = true;
            ring.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Black);
            //ring.Height = sender.ActualHeight;
            //ring.Width = sender.ActualWidth;
            sender.Content = ring;

            ServerJson json = sender.Tag as ServerJson;
            MessageBox.Show(Globals.Settings.Fullscreen.ToString());
            PlayCore core = new PlayCore(json.ServerVersion, Globals.Settings.MemoryAllocationInGB * 1024, Globals.Settings.Fullscreen, Globals.Settings.CustomUUID, Globals.Settings.CustomAccessToken);
            await core.LaunchServer(json.ServerIP, json.ServerPort);

            FontIcon icon = new FontIcon();
            icon.Glyph = "\uE73E";
            sender.Content = icon;

            await Task.Delay(950);

            sender.Content = "Play";
        }
    }
}
