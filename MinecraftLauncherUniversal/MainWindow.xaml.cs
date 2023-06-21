using CommunityToolkit.WinUI.UI.Helpers;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.AppNotifications;
using MinecraftLauncherUniversal.Dialogs;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Pages;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;
using WinUIEx.Messaging;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowEx
    {
        //from https://github.com/microsoft/microsoft-ui-xaml/issues/7009
        void SetCapitionButtonColorForWin10()
        {
            var res = Microsoft.UI.Xaml.Application.Current.Resources;
            Action<Windows.UI.Color> SetTitleBarButtonForegroundColor = (Windows.UI.Color color) => { res["WindowCaptionForeground"] = color; };
            var currentTheme = ((FrameworkElement)Content).ActualTheme;
            if (currentTheme == ElementTheme.Dark)
            {
                SetTitleBarButtonForegroundColor(Colors.White);
            }
            else if (currentTheme == ElementTheme.Light)
            {
                SetTitleBarButtonForegroundColor(Colors.Black);
            }
            else
            {
                if (App.Current.RequestedTheme == ApplicationTheme.Dark)
                {
                    SetTitleBarButtonForegroundColor(Colors.White);
                }
                else
                {
                    SetTitleBarButtonForegroundColor(Colors.Black);
                }
            }
        }

        async void InitDesgin() 
        {
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            ThemeListener listener = new ThemeListener();

            var titlebar = AppWindow.TitleBar;

            this.CenterOnScreen();
            this.SetIsResizable(false);

            // Title = "MinecraftLauncher";
            SetCapitionButtonColorForWin10();

            //Application.Current.FocusVisualKind = FocusVisualKind.HighVisibility;

            //DesktopAcrylicBackdrop abackdrop = new DesktopAcrylicBackdrop();
            MicaBackdrop abackdrop = new MicaBackdrop();
            if (Globals.Theme == 0)
            {
                //light
                abackdrop.Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt; //idk if i should keep this in, it looks nice but idk
            }
            else
            {
                abackdrop.Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base;
            }
            this.SystemBackdrop = abackdrop;

            if (Environment.OSVersion.Version.Build <= 22000) //enable the normal look of navigationview on windows 10
            {
                MainNavigationDisableContentBackgroundDictionary.ThemeDictionaries.Clear();
            }

            //Application.Current.FocusVisualKind = FocusVisualKind.Reveal;
        }

        void SetGlobalObjects()
        {
            Globals.MainWindow = this;
            Globals.MainFrame = RootFrame;
            Globals.MainNavigationBreadcrumb = MainBreadcrumb;
            Globals.MainNavigation = MainNavigation;
            Globals.MainGrid = RootGrid;
            Globals.AllVersionsNavigationViewItemInfoBadge = AllVersionsInfoBadge;
        }
        public MainWindow()
        {
            this.InitializeComponent();
            this.Title = "Minecraft Launcher";
            InitDesgin();
            SetGlobalObjects();

            OnMainWindowStartup();
        }

        void OnMainWindowStartup()
        {
            AppNotificationManager.Default.Register();
            Settings.GetSettings();

            if (Globals.bIsFirstTimeRun)
            {
                OnFirstTimeRun();
            }
            else
            {
                //init last used user profile
                CustomProfileDataManager managet = new CustomProfileDataManager();
                managet.GetLastUsedProfile();
            }

            //MainNavigation.PaneTitle = Globals.Username;
            UsernameBlock.Text = Globals.Username;
            ProfileSubtext.Text = Globals.SubText;
            ToolTipService.SetToolTip(PlayerPaneContent, "Edit Profile " + UsernameBlock.Text);

            //navigate home
            MainNavigation.SelectedItem = HomeItem;
            RootFrame.Navigate(typeof(HomePage));
            NavigationService.UpdateBreadcrumb("Home", true);
            NavigationService.HideBreadcrumb();

            if (Globals.bIsFirstTimeRun)
            {
                UsernameTip.Target = (FrameworkElement)MainNavigation.PaneCustomContent;
                //UsernameTip.IsOpen = true;
            }

            if (!Globals.bIsFirstTimeRun)
            {
                InfoDot.Visibility = Visibility.Collapsed;
            }
        }

        void OnFirstTimeRun()
        {
            CustomProfileDataManager manager = new CustomProfileDataManager();
            manager.InitProfiles();
        }

        async Task PreloadArrays()
        {
            ContentDialog loaddialog = new ContentDialog();
            loaddialog.XamlRoot = this.Content.XamlRoot;
            loaddialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            loaddialog.Content = new SimpleLoadingDialog("Preparing...");

            loaddialog.ShowAsync();

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
                SetStats();
            }

            loaddialog.Hide();
        }

        void SetStats()
        {
            TotalInstalledBlock.Text = "Installed " + VersionManager.PrefetchedStatistics.TotalInstalled;
            TotalVersionsBlock.Text = "Total  " + VersionManager.PrefetchedStatistics.TotalAvailable;
            TotalReleasesBlock.Text = "Releases " + VersionManager.PrefetchedStatistics.TotalReleases;
            TotalOptiFineBlock.Text = "OptiFine " + VersionManager.PrefetchedStatistics.TotalOptiFine;
        }

        private void Dialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Globals.RestartApp();
        }

        private void MainNavigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            string content = ((Microsoft.UI.Xaml.Controls.NavigationViewItem)sender.SelectedItem).Content.ToString();
            if (args.IsSettingsInvoked)
            {
                NavigationService.UpdateBreadcrumb("Settings", true);
                NavigationService.ShowBreadcrumb();
                RootFrame.Navigate(typeof(SettingsPage));
            }
            if (args.InvokedItemContainer == HomeItem)
            {
                RootFrame.Navigate(typeof(HomePage));
                NavigationService.UpdateBreadcrumb("Home", true);
                NavigationService.HideBreadcrumb();
            }
            if (args.InvokedItemContainer == AllVersionsPage)
            {
                NavigationService.UpdateBreadcrumb("All Versions", true);
                NavigationService.ShowBreadcrumb();
                RootFrame.Navigate(typeof(AllVersionsPage));
            }
            if (args.InvokedItemContainer == OptifineItem)
            {
                NavigationService.UpdateBreadcrumb("OptiFine", true);
                NavigationService.ShowBreadcrumb();
                RootFrame.Navigate(typeof(OptiFinePage));
            }
            if (args.InvokedItemContainer == AboutItem)
            {
                NavigationService.UpdateBreadcrumb("About", true);
                NavigationService.ShowBreadcrumb();
                RootFrame.Navigate(typeof(AboutPage));
            }

            //MainNavigation.PaneTitle = Globals.Username;
            UsernameBlock.Text = Globals.Username;
            ProfileSubtext.Text = Globals.SubText;

            GC.Collect(); //idk, trying to lower ram usage
        }

        private void MainBreadcrumb_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
        {
            RootFrame.GoBack();
            Globals.Breadcrumbs.RemoveAt(1);
        }

        private void AppTitleBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack())
            {
                AppTitleBackButton.IsEnabled = true;
                NavigationService.FrameGoBack();
            }
        }

        private void PlayerPaneContent_Click(object sender, RoutedEventArgs e)
        {
            MainNavigation.SelectedItem = MainNavigation.SettingsItem;
            NavigationService.UpdateBreadcrumb("Settings", true);
            NavigationService.ShowBreadcrumb();
            RootFrame.Navigate(typeof(SettingsPage));

            NavigationService.NavigateHiearchical(typeof(PlayerSettingsPage), "Player Settings", false);

            UsernameBlock.Text = Globals.Username;
            ProfileSubtext.Text = Globals.SubText;

            InfoDot.Visibility = Visibility.Collapsed;
        }

        private void AppTitlePaneOpenButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainNavigation.IsPaneOpen)
            {
                MainNavigation.IsPaneOpen = false;
            }
            else
            {
                MainNavigation.IsPaneOpen = true;
            }
        }

        private void NavigationSearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var cat in VersionManager.AllVersionsGlobal)
                {
                    var found = splitText.All((key) =>
                    {
                        return cat.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(cat);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No results found");
                }
                sender.ItemsSource = suitableItems;

            }
        }

        private void NavigationSearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem.ToString() == "No results found")
            {
                sender.Text = string.Empty;
                return;
            }
            Globals.MainNavigation.SelectedItem = AllVersionsPage;
            NavigationService.UpdateBreadcrumb("All Versions", true);
            NavigationService.ShowBreadcrumb();
            RootFrame.Navigate(typeof(AllVersionsPage));

            string version = args.SelectedItem.ToString();

            Globals.CurrentVersion = version;

            NavigationService.NavigateHiearchical(typeof(SelectedVersionPage), "Play " + version, false);
        }

        private async void RootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            Globals.MainGridXamlRoot = this.Content.XamlRoot;
            await PreloadArrays();

            if (Globals.bIsFirstTimeRun)
            {
                ContentDialog loaddialog = new ContentDialog();
                loaddialog.XamlRoot = this.Content.XamlRoot;
                loaddialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                //loaddialog.Content = new SimpleLoadingDialog("Preparing...");
                loaddialog.Title = "Welcome to MinecraftLauncher V2!";

                loaddialog.ShowAsync();
            }
        }

        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            //MainNavigation.PaneTitle = Globals.Username;
            UsernameBlock.Text = Globals.Username;
            ProfileSubtext.Text = Globals.SubText;

            GC.Collect(); //idk, trying to lower ram usage
        }
    }
}
