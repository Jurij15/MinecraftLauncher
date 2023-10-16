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
using CommunityToolkit.WinUI.Controls;
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
using MinecraftLauncherUniversal.Core.Server;
using MinecraftLauncherUniversal.Controls;
using Serilog;
using MinecraftLauncherUniversal.Pages.ServersPages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ServersPage : Page
    {
        ServerCardControl _storedCard;
        bool _isPageUnloaded = false;

        bool _guidsLoaded = true;
        public ServersPage()
        {
            this.InitializeComponent();
        }

        bool ServerAlreadyExists(string GUID)
        {
            bool ReturnValue = false;

            foreach (ServerCardControl item in ItemsPanel.Items)
            {
                if (item.ServerClass.Json.GUID == GUID)
                {
                    ReturnValue = true;
                    break;
                }
            }

            return ReturnValue;
        }

        private async void AddToolbarBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = Globals.MainGridXamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;

            dialog.Title = "Add Server";
            dialog.Content = new Dialogs.AddServerDialog(dialog);

            dialog.Closed += Dialog_Closed;

            //await dialog.ShowAsync();

            NavigationService.Navigate(typeof(AddServerPage), "Add Server", false);
        }

        private void Dialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            LoadGuids();
        }

        async void LoadGuids()
        {
            _guidsLoaded = false;
            LoadingStatus.Visibility = Visibility.Visible;
            await Task.Delay(20);
            ItemsPanel.Items.Clear();
            await Task.Delay(80);
            ServersManager manager = new ServersManager();
            string[] guids = manager.GetAllServerGuids();

            foreach (string guid in guids)
            {
                ServerJson json = manager.GetServerJson(guid);
                MineStat minestat = null;
                if (!_isPageUnloaded)
                {
                    minestat = await Task.Run(() => ServersHelper.GetServer(json.ServerIP, json.ServerPort, 2));
                }
                if (minestat != null)
                {
                    bool stat = ServersHelper.IsServerUp(minestat);

                    ServerClass Class = new ServerClass();
                    Class.Json = json;
                    Class.MineStat = minestat;
                    Class.State = (Controls.ServerCardControl.ServerState)Convert.ToInt32(stat);

                    ServerCardControl control = new ServerCardControl();
                    control.ServerName = json.ServerName;
                    control.ServerStatus = Class.State;
                    control.ServerClass = Class;
                    control.ServerMOTD = minestat.Stripped_Motd;

                    control.RightTapped += Control_RightTapped;
                    control.PointerReleased += VersionCardControl_PointerPressed;
                    control.Loaded += ServerCardControl_Loaded;

                    control.Width = 320;
                    control.Height = 70;

                    control.ServerCurrentPlayers = minestat.CurrentPlayers;
                    control.ServerTotalPlayers = minestat.MaximumPlayers;

                    if (!ServerAlreadyExists(json.GUID))
                    {
                        ItemsPanel.Items.Add(control);
                    }
                }

                await Task.Delay(100); //wait for the server to appear
            }

            if (ItemsPanel.Items.Count < 1)
            {
                TextBlock text = new TextBlock();
                text.Text = "You do not have any servers added. Try adding one by pressing Add";
                text.Foreground = Application.Current.Resources["TextFillColorSecondaryBrush"] as SolidColorBrush;
                text.Style = Application.Current.Resources["BodyTextBlockStyle"] as Style;

                ItemsPanel.Items.Add(text);

                return;
            }

            LoadingStatus.Visibility = Visibility.Collapsed;
            await Task.Delay(50);//wait for servers to show

            _guidsLoaded = true;
        }

        bool ShowContextMenu = false;
        private void Control_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            //shift was pressed, show the context menu
            ServerCardControl control = (ServerCardControl)sender;
            if (control != null)
            {
                ShowContextMenu = true;
                MenuFlyout flyout = new MenuFlyout();

                MenuFlyoutItem edititem = new MenuFlyoutItem();
                edititem.Text = "Edit";
                edititem.Tag = control.ServerClass.Json;
                FontIcon editicon = new FontIcon();
                editicon.Glyph = "\uE70F";
                edititem.Icon = editicon;
                edititem.Click += Edititem_Click;

                MenuFlyoutItem deleteitem = new MenuFlyoutItem();
                deleteitem.Text = "Delete";
                deleteitem.Tag = control.ServerClass.Json;
                FontIcon deleteicon = new FontIcon();
                deleteicon.Glyph = "\uE74D";
                deleteitem.Icon = deleteicon;
                deleteitem.Click += Deleteitem_Click;

                flyout.Items.Add(edititem);
                flyout.Items.Add(new MenuFlyoutSeparator());
                flyout.Items.Add(deleteitem);

                flyout.ShouldConstrainToRootBounds = false;
                flyout.ShowAt(control, new FlyoutShowOptions() { Placement = FlyoutPlacementMode.Auto });

                flyout.Closed += Flyout_Closed;
            }
        }

        private void Flyout_Closed(object sender, object e)
        {
            ShowContextMenu = false;
        }

        private void Deleteitem_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem btn  = (sender as MenuFlyoutItem);
            ServerJson json = btn.Tag as ServerJson;

            ServerCardControl control = null; 

            foreach (ServerCardControl item in ItemsPanel.Items)
            {
                if (item.ServerClass.Json.GUID == json.GUID)
                {
                    control = item;
                    break;
                }
            }

            if (control == null)
            {
                return;
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
            confirm.Tag = json;
            confirm.Content = "Yes";
            confirm.Click += Confirm_Click;

            content.Children.Add(text);
            content.Children.Add(confirm);

            flyout.Content = content;
            flyout.SystemBackdrop = new MicaBackdrop();
            flyout.ShowAt(control, new FlyoutShowOptions() { Placement = FlyoutPlacementMode.Auto, ShowMode = FlyoutShowMode.Auto });
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string guid = (((Button)sender).Tag as ServerJson).GUID;

            ServersManager manager = new ServersManager();
            manager.DeleteServer(guid);

            LoadGuids();
        }

        private async void ItemsPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (_storedCard != null)
            {
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ServerBackAnim");
                if (animation != null)
                {
                    bool result = animation.TryStart(_storedCard);
                }
            }

            await Task.Delay(100);//wait for anim to finish
            LoadGuids(); //reload guids
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (!_guidsLoaded)
            {
                //e.Cancel = true;
            }
            base.OnNavigatingFrom(e);
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
            _isPageUnloaded = false;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _isPageUnloaded = true;
        }

        private async void VersionCardControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            await Task.Delay(100);
            ServerCardControl control = (ServerCardControl)sender;
            if (control != null && e.KeyModifiers != Windows.System.VirtualKeyModifiers.Shift && !ShowContextMenu)
            {
                //MessageBox.Show(ShowContextMenu.ToString());
                string version = control.ServerClass.Json.ServerVersion;

                Globals.CurrentVersion = version;
                _storedCard = control;

                var animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardServerConnectedAnimation", _storedCard);
                var badgeanimation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardServerbadgeConnectedAnimation", _storedCard.StatusBadge);
                var nameanimation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardServernameConnectedAnimation", _storedCard.ServerNameTextBlock);
                var motdanimation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardServermotdConnectedAnimation", _storedCard.VersionMOTDTextBlock);
                var playersanimation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardServerplayersConnectedAnimation", _storedCard.PlayersStatsPanel);

                //idk if this is better or not
                animation.Configuration = new BasicConnectedAnimationConfiguration();
                badgeanimation.Configuration = new BasicConnectedAnimationConfiguration();
                nameanimation.Configuration = new BasicConnectedAnimationConfiguration();
                motdanimation.Configuration = new BasicConnectedAnimationConfiguration();
                playersanimation.Configuration = new BasicConnectedAnimationConfiguration();

                PlayVersionPage.IsPlayingServer = true;
                PlayVersionPage.PlayServerClass = control.ServerClass;
                NavigationService.NavigateSuppressedAnim(typeof(PlayVersionPage), "Play " + version, false, false);
            }
            else
            {
                //shift was pressed, show the context menu

                MenuFlyout flyout = new MenuFlyout();
                
                MenuFlyoutItem edititem = new MenuFlyoutItem();
                edititem.Text = "Edit";
                edititem.Tag = control.ServerClass.Json;
                FontIcon editicon = new FontIcon();
                editicon.Glyph = "\uE70F";
                edititem.Icon = editicon;
                edititem.Click += Edititem_Click;

                MenuFlyoutItem deleteitem = new MenuFlyoutItem();
                deleteitem.Text = "Delete";
                deleteitem.Tag = control.ServerClass.Json;
                FontIcon deleteicon = new FontIcon();
                deleteicon.Glyph = "\uE74D";
                deleteitem.Icon = deleteicon;
                deleteitem.Click += Deleteitem_Click;

                flyout.Items.Add(edititem);
                flyout.Items.Add(new MenuFlyoutSeparator());
                flyout.Items.Add(deleteitem);

                flyout.ShouldConstrainToRootBounds = false;
                flyout.ShowAt(control, new FlyoutShowOptions() { Placement = FlyoutPlacementMode.Auto});
            }
        }

        private async void Edititem_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = Globals.MainGridXamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;

            dialog.Title = "Edit "+ ((sender as MenuFlyoutItem).Tag as ServerJson).ServerName;
            dialog.Content = new Dialogs.EditServerDialog(dialog, (sender as MenuFlyoutItem).Tag as ServerJson);

            dialog.Closed += Dialog_Closed;

            await dialog.ShowAsync();
        }

        private void ServerCardControl_Loaded(object sender, RoutedEventArgs e)
        {
            ((ServerCardControl)sender).UpdateStatus();
        }
    }
}
