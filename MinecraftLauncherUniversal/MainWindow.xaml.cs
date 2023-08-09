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
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using WinUIEx;
using WinUIEx.Messaging;
using static MinecraftLauncherUniversal.Services.NavigationService;

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

           
            Title = "Minecraft Launcher";
            SetCapitionButtonColorForWin10();

            MicaBackdrop abackdrop = new MicaBackdrop();
            if (Globals.Settings.Theme == ElementTheme.Light)
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
                //MainNavigationDisableContentBackgroundDictionary.ThemeDictionaries.Clear();
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

            InitializeNavigationService(MainNavigation, MainBreadcrumb, RootFrame);

            OnMainWindowStartup();
        }

        void OnMainWindowStartup()
        {
            try
            {
                AppNotificationManager.Default.Register();
            }
            catch (Exception ex)
            {
                Globals.ToastFailedInit = true;
            }

            //navigate home
            MainNavigation.SelectedItem = HomeItem;
            ChangeBreadcrumbVisibility(false);

            if (Globals.bIsFirstTimeRun)
            {
                UsernameTip.Target = (FrameworkElement)MainNavigation.PaneCustomContent;
                //UsernameTip.IsOpen = true;
            }
        }

        async Task PreloadArrays()
        {
            ContentDialog loaddialog = new ContentDialog();
            loaddialog.XamlRoot = this.Content.XamlRoot;
            loaddialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            loaddialog.Content = new SimpleLoadingDialog("Preparing...");

            loaddialog.ShowAsync();

            if (NetworkInformation.GetInternetConnectionProfile()?.NetworkAdapter == null)
            {
                ContentDialog Nonetworkdialog = new ContentDialog();
                Nonetworkdialog.XamlRoot = this.Content.XamlRoot;
                Nonetworkdialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                Nonetworkdialog.Title = "Error";
                Nonetworkdialog.Content = "Network is not available. Launcher might not function properly!";

                Nonetworkdialog.CloseButtonText = "OK";
                Nonetworkdialog.CloseButtonClick += Nonetworkdialog_CloseButtonClick;

                loaddialog.Hide();
                await Nonetworkdialog.ShowAsync();            
            }
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

        private void Nonetworkdialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            sender.Hide();
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

        private void MainBreadcrumb_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
        {
            if (args.Index < NavigationService.BreadCrumbs.Count - 1)
            {
                var crumb = (Breadcrumb)args.Item;
                crumb.NavigateToFromBreadcrumb(args.Index);
            }

            ElementSoundPlayer.Play(ElementSoundKind.GoBack);
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

            string version = args.SelectedItem.ToString();

            Globals.CurrentVersion = version;

            Globals.MainNavigation.SelectedItem = AllVersionsPage;
            Navigate(typeof(AllVersionsPage), "Select a Version", true);

            Navigate(typeof(SelectedVersionPage), "Play", false);
        }

        private async void RootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            Globals.MainGridXamlRoot = this.Content.XamlRoot;
            await PreloadArrays();

            if (Globals.ToastFailedInit && !Globals.bIsFirstTimeRun)
            {
                DialogService.ShowSimpleDialog("Error", "Notifications failed to initialize and will be disabled during app runtime!");
            }

            if (Globals.bIsFirstTimeRun)
            {
                ContentDialog loaddialog = new ContentDialog();
                loaddialog.XamlRoot = this.Content.XamlRoot;
                loaddialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                loaddialog.Title = "Welcome to MinecraftLauncher V2!";
                loaddialog.Content = new WelcomeV2DialogContent();

                loaddialog.CloseButtonText = "OK";
                loaddialog.CloseButtonClick += (sender, args) => { loaddialog.Hide(); };

                loaddialog.DefaultButton = ContentDialogButton.Close;

                loaddialog.ShowAsync();
            }
        }

        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.SourcePageType == typeof(HomePage))
            {
                ChangeBreadcrumbVisibility(false);
            }

            GC.Collect(); //idk, trying to lower ram usage
        }

        private void WindowEx_Closed(object sender, WindowEventArgs args)
        {
            Globals.CloseConsole();
        }

        private void MainNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            string content = ((Microsoft.UI.Xaml.Controls.NavigationViewItem)sender.SelectedItem).Content.ToString();
            if (args.IsSettingsSelected)
            {
                Navigate(typeof(SettingsPage), "Settings", true);
            }
            if (args.SelectedItemContainer == HomeItem)
            {
                Navigate(typeof(HomePage), "Home", true);
                ChangeBreadcrumbVisibility(false);
            }
            if (args.SelectedItemContainer == AllVersionsPage)
            {
                Navigate(typeof(AllVersionsPage), "Select a Version", true);
            }
            if (args.SelectedItemContainer == OptifineItem)
            {
                Navigate(typeof(OptiFinePage), "OptiFine", true);
            }
            if (args.SelectedItemContainer == AboutItem)
            {
                Navigate(typeof(AboutPage), "About", true);
            }
            if (args.SelectedItemContainer == ServersItem)
            {
                Navigate(typeof(ServersPage), "Servers", true);
            }

            GC.Collect(); //idk, trying to lower ram usage

            Logger.Log("NAVIGATION", "Navigated!");
        }

        private void AppTitlePaneButton_Click(object sender, RoutedEventArgs e)
        {
            bool Current = MainNavigation.IsPaneOpen;
            if (Current)
            {
                MainNavigation.IsPaneOpen = false;
            }
            else
            {
                MainNavigation.IsPaneOpen = true;
            }
        }
    }
}
