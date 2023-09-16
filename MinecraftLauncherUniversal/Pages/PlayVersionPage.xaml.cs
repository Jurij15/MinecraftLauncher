using CmlLib.Core;
using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.UI.Animations;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using MinecraftLauncherUniversal.Core;
using MinecraftLauncherUniversal.Core.Server;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Interop;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.UserDataTasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using WinUIEx;
using WinUIEx.Messaging;
using static MinecraftLauncherUniversal.Controls.ServerCardControl;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayVersionPage : Page
    {
        public static ServerClass PlayServerClass;
        public static bool IsPlayingNormalVersion = false; // naviating from play page
        public static bool IsPlayingServer = false;
        public PlayVersionPage()
        {
            this.InitializeComponent();

            if (NavigationService.MainBreadcrumb.Visibility == Visibility.Visible)
            {
                NavigationService.ChangeBreadcrumbVisibility(false);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainWindow.TitleBarGoBackButton.Visibility = Visibility.Visible;
            MainWindow.TitleBarGoBackButton.Click += TitleBarGoBackButton_Click;

            if (IsPlayingNormalVersion)
            {
                // Retrieve and start the connected animation
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardConnectedAnimation");
                if (animation != null)
                {
                    animation.TryStart(InfoPresenterGrid, new UIElement[] { });
                }
                var imganim = ConnectedAnimationService.GetForCurrentView().GetAnimation("forwardImageAnim");
                if (imganim != null)
                {
                    imganim.TryStart(MCImg);
                }
                var textanim = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardTextAnim");
                if (textanim != null)
                {
                    textanim.TryStart(MinecraftVersionBlock);
                }
            }
            else if( IsPlayingServer)
            {
                ServerPanel.Visibility = Visibility.Visible;
                Seperator.Visibility = Visibility.Visible;
                ServerInfoBlock.Visibility = Visibility.Visible;

                // Retrieve and start the connected animation
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardServerConnectedAnimation");
                if (animation != null)
                {
                    animation.TryStart(ServerPanel, new UIElement[] { });
                }
                var badgeanimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardServerbadgeConnectedAnimation");
                if (badgeanimation != null)
                {
                    badgeanimation.TryStart(ServerInfoBadge, new UIElement[] { });
                }
                var textanimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardServernameConnectedAnimation");
                if (textanimation != null)
                {
                    textanimation.TryStart(ServerNameBlock, new UIElement[] { });
                }

                var motdanimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardServermotdConnectedAnimation");
                if (motdanimation != null)
                {
                    motdanimation.TryStart(ServerMOTDBlock, new UIElement[] { });
                }

                var playersanimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardServerplayersConnectedAnimation");
                if (playersanimation != null)
                {
                    playersanimation.TryStart(PlayersPanel, new UIElement[] { });
                }

                ServerNameBlock.Text = PlayServerClass.Json.ServerName;
                ServerMOTDBlock.Text = PlayServerClass.MineStat.Stripped_Motd;

                CurrentPlayersBlock.Text = PlayServerClass.MineStat.CurrentPlayers;
                TotalPlayersBlock.Text = PlayServerClass.MineStat.MaximumPlayers;

                ServerState ServerStatus = PlayServerClass.State;

                if (ServerStatus == ServerState.Online)
                {
                    ServerInfoBadge.Style = Application.Current.Resources["SuccessIconInfoBadgeStyle"] as Style;
                    ToolTipService.SetToolTip(ServerInfoBadge, "Online");
                }
                else if (ServerStatus == ServerState.Offline)
                {
                    ServerInfoBadge.Style = Application.Current.Resources["CriticalIconInfoBadgeStyle"] as Style;
                    ToolTipService.SetToolTip(ServerInfoBadge, "Offline");
                }
            }
            else
            {

            }
            base.OnNavigatedTo(e);

            MinecraftVersionBlock.Text ="Minecraft "+ Globals.CurrentVersion;

            //BitmapImage bitmapImage = new BitmapImage();
            //bitmapImage.UriSource = new Uri("ms-appx:///"+ "/Assets/MinecraftPlayIcon.png", UriKind.Absolute);
            //MCImg.Source = bitmapImage;
        }

        private async void Background_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Globals.Settings.ShowImageBackgroundInPlayPage)
            {
                BlurTint.Opacity = 0;
                BackgroundBrush.Opacity = 0;
            }
            else
            {
                await AnimationBuilder
                            .Create()
                            .Opacity(to: 1, duration: TimeSpan.FromMilliseconds(1250), easingType: EasingType.Linear, from: 0)
                            .StartAsync(BackgroundGrid);
            }
        }

        bool BackButtonPressed = false;
        private void TitleBarGoBackButton_Click(object sender, RoutedEventArgs e)
        {
            BackButtonPressed = true;
            //this is only meant for AllVersionsPage and servers
            MainWindow.TitleBarGoBackButton.Visibility = Visibility.Collapsed;
            MainWindow.TitleBarGoBackButton.Click -= TitleBarGoBackButton_Click;

            if (IsPlayingNormalVersion)
            {
                NavigationService.NavigateSuppressedAnim(typeof(AllVersionsPage), "Select a Version", true, true);
            }
            else if (IsPlayingServer)
            {
                NavigationService.NavigateSuppressedAnim(typeof(ServersPage), "Servers", true, true);
            }
            else
            {
                NavigationService.Navigate(typeof(AllVersionsPage), "Select a Version", true, SlideNavigationTransitionEffect.FromLeft, true);
            }

            //reset the values
            IsPlayingNormalVersion = false;
            IsPlayingServer = false;
            PlayServerClass = null;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            MainWindow.TitleBarGoBackButton.Visibility = Visibility.Collapsed;
            MainWindow.TitleBarGoBackButton.Click -= TitleBarGoBackButton_Click;
            base.OnNavigatingFrom(e);

            if (BackButtonPressed && IsPlayingNormalVersion)
            {
                //MessageBox.Show(e.SourcePageType.Name.ToString());
                var animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("BackAnim", InfoPresenterGrid);
                animation.Configuration = new DirectConnectedAnimationConfiguration();

                var imgbackanim = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ImgBackAnim", MCImg);
                imgbackanim.Configuration = new DirectConnectedAnimationConfiguration();

                var TextBackAnim = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("TextBackAnim", MinecraftVersionBlock);
                TextBackAnim.Configuration = new DirectConnectedAnimationConfiguration();
            }
            else if (BackButtonPressed && IsPlayingServer)
            {
                var animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ServerBackAnim", InfoPresenterGrid);
                animation.Configuration = new DirectConnectedAnimationConfiguration();
            }

            if (IsPlayingNormalVersion && !IsNaviatingToPlayerSettings || IsPlayingServer && !IsNaviatingToPlayerSettings)
            {
                IsPlayingNormalVersion = false;
                IsPlayingServer = false;
                PlayServerClass = null;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private ProgressRing DownloadProgressRing;
        async void DownloadBefore()
        {
            DownloadButton.IsEnabled = false;
            StatusBox.Text = "Downloading...";
            int memooryinmb = Globals.Settings.MemoryAllocationInGB * 1024;

            ProgressRing r = new ProgressRing();
            PlayButton.Content = r;

            r = PlayButton.Content as ProgressRing;
            DownloadProgressRing = r;

            PlayCore core = new PlayCore(Globals.CurrentVersion, memooryinmb, Globals.Settings.Fullscreen, Globals.Settings.CustomUUID, Globals.Settings.CustomAccessToken);
            await core.Download(OnProgressChanged);

            FontIcon icon = new FontIcon();
            icon.Glyph = "\uE8FB";
            PlayButton.Content = icon;

            await Task.Delay(400);

            FontIcon icon2 = new FontIcon();
            icon2.Glyph = "\uE768";
            PlayButton.Content = icon2;

            DownloadButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
            StatusBox.Text = "Ready to Play";

            NotificationService.SendSimpleToast("Download Complete", "Minecraft " + Globals.CurrentVersion + " is ready to play!", 1.5);
        }

        async void OnProgressChanged(int value)
        {
            DownloadProgressRing.IsIndeterminate = false;
            DownloadProgressRing.Value = value;
        }

        async void AsyncLaunch()
        {
            if (IsPlayingNormalVersion)
            {
                bool bSucess = false;
                int memooryinmb = Globals.Settings.MemoryAllocationInGB * 1024;

                ProgressRing r = new ProgressRing();
                r.IsIndeterminate = true;
                PlayButton.Content = r;

                r = PlayButton.Content as ProgressRing;

                PlayButton.IsEnabled = false;
                StatusBox.Text = "Launching...";

                PlayCore core = new PlayCore(Globals.CurrentVersion, memooryinmb, Globals.Settings.Fullscreen, Globals.Settings.CustomAccessToken, Globals.Settings.CustomAccessToken);
                bool result = await core.Launch();
                if (!result) { DialogService.ShowSimpleDialog("An Error Occured", core.GetLaunchErrors()); } else { bSucess = true; }
                StatusBox.Text = "Playing";

                FontIcon icon = new FontIcon();
                icon.Glyph = "\uE8FB";
                PlayButton.Content = icon;

                await Task.Delay(800);

                FontIcon icon2 = new FontIcon();
                icon.Glyph = "\uE768";
                PlayButton.Content = icon2;

                //by now, it has already been launched, now store the build in recents
                VersionManager manager = new VersionManager();
                manager.AddNewBuildToRecents(Globals.CurrentVersion);
                MinecraftLaunchedInfo.IsOpen = true;
                PlayButton.IsEnabled = true;

                if (bSucess)
                {
                    Globals.MainWindow.Minimize();
                    NotificationService.SendSimpleToast("Launched", "Minecraft " + Globals.CurrentVersion + " launched successfully!", 1.5);
                }
            }
            else if (IsPlayingServer)
            {
                //this is a mess
                if (PlayServerClass.State == ServerState.Offline)
                {
                    ContentDialog warningofflinedialog = DialogService.CreateContentDialog("Warning", "You are launching into a server that is offline, which can cause crashes. Do you want to continue?");
                    warningofflinedialog.PrimaryButtonText = "Launch Anyway";
                    warningofflinedialog.CloseButtonText = "Cancel";
                    warningofflinedialog.DefaultButton = ContentDialogButton.Close;

                    ContentDialogResult dialogresultresult = await warningofflinedialog.ShowAsync();
                    if (dialogresultresult == ContentDialogResult.Primary)
                    {
                        bool bSucess = false;
                        int memooryinmb = Globals.Settings.MemoryAllocationInGB * 1024;

                        ProgressRing r = new ProgressRing();
                        r.IsIndeterminate = true;
                        PlayButton.Content = r;

                        r = PlayButton.Content as ProgressRing;

                        PlayButton.IsEnabled = false;
                        StatusBox.Text = "Launching...";

                        PlayCore core = new PlayCore(Globals.CurrentVersion, memooryinmb, Globals.Settings.Fullscreen, Globals.Settings.CustomAccessToken, Globals.Settings.CustomAccessToken);
                        bool result = await core.LaunchServer(PlayServerClass.Json.ServerIP, PlayServerClass.Json.ServerPort);
                        if (!result) { DialogService.ShowSimpleDialog("An Error Occured", core.GetLaunchErrors()); } else { bSucess = true; }

                        StatusBox.Text = "Playing";

                        FontIcon icon = new FontIcon();
                        icon.Glyph = "\uE8FB";
                        PlayButton.Content = icon;

                        await Task.Delay(800);

                        FontIcon icon2 = new FontIcon();
                        icon2.Glyph = "\uE768";
                        PlayButton.Content = icon2;

                        //by now, it has already been launched, now store the build in recents
                        VersionManager manager = new VersionManager();
                        manager.AddNewBuildToRecents(Globals.CurrentVersion);
                        MinecraftLaunchedInfo.IsOpen = true;
                        PlayButton.IsEnabled = true;

                        if (bSucess)
                        {
                            Globals.MainWindow.Minimize();
                            NotificationService.SendSimpleToast("Launched", "Minecraft Server " + PlayServerClass.Json.ServerName + " launched successfully!", 1.5);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    bool bSucess = false;
                    int memooryinmb = Globals.Settings.MemoryAllocationInGB * 1024;

                    ProgressRing r = new ProgressRing();
                    r.IsIndeterminate = true;
                    PlayButton.Content = r;

                    r = PlayButton.Content as ProgressRing;

                    PlayButton.IsEnabled = false;
                    StatusBox.Text = "Launching...";

                    PlayCore core = new PlayCore(Globals.CurrentVersion, memooryinmb, Globals.Settings.Fullscreen, Globals.Settings.CustomAccessToken, Globals.Settings.CustomAccessToken);
                    bool result = await core.LaunchServer(PlayServerClass.Json.ServerIP, PlayServerClass.Json.ServerPort);
                    if (!result) { DialogService.ShowSimpleDialog("An Error Occured", core.GetLaunchErrors()); } else { bSucess = true; }

                    StatusBox.Text = "Playing";

                    FontIcon icon = new FontIcon();
                    icon.Glyph = "\uE8FB";
                    PlayButton.Content = icon;

                    await Task.Delay(800);

                    FontIcon icon2 = new FontIcon();
                    icon2.Glyph = "\uE768";
                    PlayButton.Content = icon2;

                    //by now, it has already been launched, now store the build in recents
                    VersionManager manager = new VersionManager();
                    manager.AddNewBuildToRecents(Globals.CurrentVersion);
                    MinecraftLaunchedInfo.IsOpen = true;
                    PlayButton.IsEnabled = true;

                    if (bSucess)
                    {
                        Globals.MainWindow.Minimize();
                        NotificationService.SendSimpleToast("Launched", "Minecraft Server " + PlayServerClass.Json.ServerName + " launched successfully!", 1.5);
                    }
                }
            }
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadBefore();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            AsyncLaunch();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
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
        }

        private void ConfirmDelete_Click(object sender, RoutedEventArgs e)
        {
            Directory.Delete(MinecraftPath.WindowsDefaultPath + "\\" + "versions" + "\\" + Globals.CurrentVersion, true);

            NavigationService.Navigate(typeof(AllVersionsPage), "All Versions", true);
        }

        static bool IsNaviatingToPlayerSettings = false;

        private void PlayerSettingsCard_Click(object sender, RoutedEventArgs e)
        {
            IsNaviatingToPlayerSettings = true;
            NavigationService.Navigate(typeof(PlayerSettingsPage), "Player Settings", false);
            IsNaviatingToPlayerSettings = false;
        }

        private void ServerMoreFlyout_Opened(object sender, object e)
        {
            GameModeBlock.Text = PlayServerClass.MineStat.Gamemode;
            LatencyBlock.Text = PlayServerClass.MineStat.Latency.ToString() + " ms";
        }
    }
}
