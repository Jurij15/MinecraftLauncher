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
using System.ComponentModel;
using System.Data.Entity.Core.Mapping;
using System.Diagnostics;
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
        public static string ForgeVersion;
        //this should be moved to an enum, and used as an arg in the constructor
        public static bool IsPlayingNormalVersion = false; // naviating from play page
        public static bool IsPlayingServer = false;
        public static bool IsPlayingForge = false;

        public static bool IsSearchedForAVersion = false;
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
            else if (IsPlayingForge)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri("ms-appx:///"+ "Assets/forgeLogo.jpg");
                MCImg.Source = bitmapImage;

                // Retrieve and start the connected animation
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardForgeConnectedAnimation");
                if (animation != null)
                {
                    animation.TryStart(InfoPresenterGrid, new UIElement[] { });
                }
                var imganim = ConnectedAnimationService.GetForCurrentView().GetAnimation("forwardForgeImageAnim");
                if (imganim != null)
                {
                    imganim.TryStart(MCImg);
                }
            }
            else if(IsSearchedForAVersion)
            {
                //just finish navigation
            }
            base.OnNavigatedTo(e);

            if (IsPlayingForge)
            {
                MinecraftVersionBlock.Text = "Forge " + Globals.CurrentVersion;
            }
            else if (VersionsHelper.bIsReleaseVersion(Globals.CurrentVersion))
            {
                MinecraftVersionBlock.Text = "Minecraft " + Globals.CurrentVersion;
            }
            else if (VersionsHelper.bIsOptifine(Globals.CurrentVersion))
            {
                MinecraftVersionBlock.Text =Globals.CurrentVersion;
            }
            else
            {
                MinecraftVersionBlock.Text = "Minecraft " + Globals.CurrentVersion;
            }

            //BitmapImage bitmapImage = new BitmapImage();
            //bitmapImage.UriSource = new Uri("ms-appx:///"+ "/Assets/MinecraftPlayIcon.png", UriKind.Absolute);
            //MCImg.Source = bitmapImage;

            ContentGrid.Visibility = Visibility.Visible;
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
            else if(IsSearchedForAVersion)
            {
                NavigationService.Navigate(typeof(AllVersionsPage), "Select a Version", true, SlideNavigationTransitionEffect.FromLeft, true);
            }
            else if (IsPlayingForge)
            {
                NavigationService.NavigateSuppressedAnim(typeof(ForgePage), "Forge", true, true);
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
            else if (BackButtonPressed && IsPlayingForge)
            {
                var animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("BackForgeAnim", InfoPresenterGrid);
                animation.Configuration = new DirectConnectedAnimationConfiguration();

                var imgbackanim = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ImgForgeBackAnim", MCImg);
                imgbackanim.Configuration = new DirectConnectedAnimationConfiguration();
            }
            else if (BackButtonPressed && IsSearchedForAVersion)
            {
                //just finish navigation
            }

            if (IsPlayingNormalVersion && !IsNaviatingToPlayerSettings || IsPlayingServer && !IsNaviatingToPlayerSettings || IsPlayingForge && !IsNaviatingToPlayerSettings)
            {
                IsPlayingNormalVersion = false;
                IsPlayingServer = false;
                IsPlayingForge = false;
                PlayServerClass = null;
                ForgeVersion = null;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private ProgressRing DownloadProgressRing;
        async void DownloadBefore()
        {
            PlayerSettingsCard.IsEnabled = false;
            DownloadButton.IsEnabled = false;
            StatusBox.Text = "Downloading...";
            int memooryinmb = Globals.Settings.MemoryAllocationInGB * 1024;

            ProgressRing r = new ProgressRing();
            r.IsIndeterminate = false;
            DownloadProgressRing = r;

            if (IsPlayingForge)
            {
                PlayCore core = new PlayCore(Globals.CurrentVersion.Remove(0, 6), memooryinmb, Globals.Settings.Fullscreen, Globals.Settings.CustomUUID, Globals.Settings.CustomAccessToken);
                await core.InstallForge(OnProgressChanged, ForgeVersion);
            }
            else
            {
                PlayCore core = new PlayCore(Globals.CurrentVersion, memooryinmb, Globals.Settings.Fullscreen, Globals.Settings.CustomUUID, Globals.Settings.CustomAccessToken);
                await core.Download(OnProgressChanged);
            }

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
            PlayerSettingsCard.IsEnabled = true;
        }

        async void OnProgressChanged(int value)
        {
            DownloadProgressRing.IsIndeterminate = false;
            DownloadProgressRing.Value = value;
        }

        async void AsyncLaunch()
        {
            bool bSucess = false;
            int memooryinmb = Globals.Settings.MemoryAllocationInGB * 1024;

            PlayerSettingsCard.IsEnabled = false;

            ProgressRing r = new ProgressRing();
            r.IsIndeterminate = true;
            PlayButton.Content = r;

            r = PlayButton.Content as ProgressRing;

            PlayButton.IsEnabled = false;
            StatusBox.Text = "Launching...";

            if (IsPlayingNormalVersion || IsSearchedForAVersion)
            {
                PlayCore core = new PlayCore(Globals.CurrentVersion, memooryinmb, Globals.Settings.Fullscreen, Globals.Settings.CustomAccessToken, Globals.Settings.CustomAccessToken);
                bool result = await core.Launch();
                if (!result) { DialogService.ShowSimpleDialog("An Error Occured", core.GetLaunchErrors()); } else { bSucess = true; }
            }
            else if (IsPlayingForge)
            {
                PlayCore core = new PlayCore(Globals.CurrentVersion, memooryinmb, Globals.Settings.Fullscreen, Globals.Settings.CustomAccessToken, Globals.Settings.CustomAccessToken);
                //MessageBox.Show(ForgeVersion);
                bool result = await core.LaunchForge(ForgeVersion);
                if (!result) 
                {
                    if (core.GetLaunchErrors() == "Object reference not set to an instance of an object") //the requested version probably does not work
                    {
                        DialogService.ShowSimpleDialog("An Error Occured", "This forge version is not working. Try another one.");
                    }
                    DialogService.ShowSimpleDialog("An Error Occured", core.GetLaunchErrors()); } else { bSucess = true; 
                }
            }
            else if (IsPlayingServer)
            {
                PlayCore core = new PlayCore(Globals.CurrentVersion, memooryinmb, Globals.Settings.Fullscreen, Globals.Settings.CustomAccessToken, Globals.Settings.CustomAccessToken);
                bool result = await core.LaunchServer(PlayServerClass.Json.ServerIP, PlayServerClass.Json.ServerPort);
                if (!result) { DialogService.ShowSimpleDialog("An Error Occured", core.GetLaunchErrors()); } else { bSucess = true; }
            }

            if (bSucess)
            {
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

                Globals.MainWindow.Minimize();
                NotificationService.SendSimpleToast("Launched", "Minecraft " + Globals.CurrentVersion + " launched successfully!", 1.5);
            }
            else
            {
                StatusBox.Text = "Launch Failed";
                FontIcon icon2 = new FontIcon();
                icon2.Glyph = "\uE768";
                PlayButton.Content = icon2;

                PlayButton.IsEnabled = true;
            }

            PlayerSettingsCard.IsEnabled = true;
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

        GridViewItem _storedImageItem;
        private void ImageList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ConnectedAnimation animation = null;

            // Get the collection item corresponding to the clicked item.
            if (ImageList.ContainerFromItem(e.ClickedItem) is GridViewItem container)
            {
                // Stash the clicked item for use later. We'll need it when we connect back from the detailpage.
                _storedImageItem = container;

                ImageList.ScrollIntoView(_storedImageItem);
                ImageList.UpdateLayout();

                // Prepare the connected animation.
                // Notice that the stored item is passed in, as well as the name of the connected element. 
                // The animation will actually start on the Detailed info page.
                animation = ImageList.PrepareConnectedAnimation("forwardScAnimation", container.Content, "connectedItem");
                animation.Configuration = new DirectConnectedAnimationConfiguration();
                animation.Completed += Anim_Completed;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(container.Content.ToString());

                ScreenshotImage.Source = bitmapImage;

                if (Globals.MainWindow.WindowState == WindowState.Maximized)
                {
                    ScreenshotImage.MaxWidth = 1600;
                    ScreenshotImage.MaxHeight = 900;
                }
                else
                {
                    ScreenshotImage.MaxWidth = 1024;
                    ScreenshotImage.MaxHeight = 576;
                }

                container.Visibility = Visibility.Collapsed;
            }

            ScreenshotBackgroundGrid.Visibility = Visibility.Visible;

            animation.TryStart(ScreenshotImage);
        }

        private void Anim_Completed(ConnectedAnimation sender, object args)
        {
            //ScreenshotImage.Visibility = Visibility.Collapsed;
            /*
                         BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri((ImageList.ItemsSource as List<string>)[ImageList.SelectedIndex].ToString());

            ScreenshotImage.Source = bitmapImage;
            */
            //ScreenshotImage.Visibility = Visibility.Visible;
        }

        private void ImageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ImageList_Loaded(object sender, RoutedEventArgs e)
        {
            //load all screenshots
            if (Directory.Exists(System.IO.Path.Combine(MinecraftPath.WindowsDefaultPath, "screenshots")))
            {
                List<string> AllImages = Directory.GetFiles(System.IO.Path.Combine(MinecraftPath.WindowsDefaultPath, "screenshots")).ToList<string>();
                if (AllImages.Count != 0)
                {
                    //user has screenshots

                    /*
                    foreach (var item in AllImages)
                    {
                        Image img = new Image();
                        img.Tag = item;

                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.UriSource = new Uri(item);

                        img.Source = bitmapImage;

                        ImageList.Items.Add(img);
                    }
                    */

            ImageList.ItemsSource = AllImages;
                }

                //Debug.Assert(AllImages.Count > 0, "No Images Found, but there should be some", "idk");
            }
            else
            {
                GalleryView.Visibility = Visibility.Collapsed;
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            //add anims for screenshots

            var animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backSCAnim", ScreenshotImage);
            animation.Configuration = new DirectConnectedAnimationConfiguration();

            animation.TryStart(_storedImageItem);

            ImageList.ScrollIntoView(_storedImageItem);
            ImageList.UpdateLayout();

            _storedImageItem.Visibility = Visibility.Visible;

            ScreenshotBackgroundGrid.Visibility = Visibility.Collapsed;

            //ScreenshotImage.Source = null;
        }

        private void ScreenshotImage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ScreenshotImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Globals.MainWindow.WindowState == WindowState.Maximized)
            {
                ScreenshotImage.MaxWidth = 1600;
                ScreenshotImage.MaxHeight = 900;
            }
            else
            {
                ScreenshotImage.MaxWidth = 1024;
                ScreenshotImage.MaxHeight = 576;
            }
        }

        private void ScreenshotBackgroundGrid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                CloseBtn_Click(null, null);
            }
        }
    }
}
