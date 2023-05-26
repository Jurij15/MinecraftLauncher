using Microsoft.UI.Xaml.Controls;
using MinecraftLauncher;
using MinecraftLauncherUniversal.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MinecraftLauncherUniversal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        void SetTitleBar()
        {
            var ttitleBar = CoreApplication.GetCurrentView().TitleBar;
            CoreApplicationViewTitleBar coreApplicationViewTitleBar = ttitleBar;
            ttitleBar.ExtendViewIntoTitleBar = true;

            Window.Current.SetTitleBar(TitleBarGrid);

            ApplicationViewTitleBar titleBar =
        ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
        }

        void ApplyBackdrop()
        {
            if (Environment.OSVersion.Version.Build >= 22000)
            {
                SetValue(BackdropMaterial.ApplyToRootOrPageBackgroundProperty, true);
            }

            //Globals.ThemeService = new ThemeService(Window.Current);
            //Globals.ThemeService.SetTheme();

            ApplicationView.PreferredLaunchViewSize = new Size(1236, 690);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
        }
        public MainPage()
        {
            this.InitializeComponent();

            SetTitleBar();
            ApplyBackdrop();

            //Helpers.Settings.GetSettings();

            Globals.MainPage = this;
            Globals.MainNavigation = MainNavigation;
            Globals.MainBreadcrumb = MainBreadcrumb;
            Globals.RootFrame = RootFrame;
        }

        void PostNavigation()
        {
            MainBreadcrumb.ItemsSource = Globals.CurrentMainBreadcrumbDisplay;
            MainNavigation.PaneTitle = "Username PH";
        }

        private void MainNavigation_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            string content = ((Microsoft.UI.Xaml.Controls.NavigationViewItem)sender.SelectedItem).Content.ToString();
            if (args.IsSettingsInvoked)
            {
                Globals.CurrentMainBreadcrumbDisplay.Add("Settings");
                RootFrame.Navigate(typeof(SettingsPage));
            }
            if (content == "Player")
            {
                Globals.CurrentMainBreadcrumbDisplay.Add("Player Settings");
                RootFrame.Navigate(typeof(PlayerSettingsPage));
            }
            if (content == "Home")
            {
                Globals.CurrentMainBreadcrumbDisplay.Add("Home");
                RootFrame.Navigate(typeof(HomePage));
            }
            if (content == "All Versions")
            {
                Globals.CurrentMainBreadcrumbDisplay.Add("All Versions");
                RootFrame.Navigate(typeof(AllVersionsPage));
            }
            if (content == "OptiFine")
            {
                Globals.CurrentMainBreadcrumbDisplay.Add("OptiFine");
                RootFrame.Navigate(typeof(OptiFinePage));
            }
            PostNavigation();
        }
    }
}
