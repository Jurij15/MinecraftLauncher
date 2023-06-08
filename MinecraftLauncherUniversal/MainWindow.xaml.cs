using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Pages;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowEx
    {
        void InitDesgin() 
        {
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            this.CenterOnScreen();
            this.SetIsResizable(false);

           // Title = "MinecraftLauncher";
        }

        void SetGlobalObjects()
        {
            Globals.MainWindow = this;
            Globals.MainFrame = RootFrame;
            Globals.MainNavigationBreadcrumb = MainBreadcrumb;
            Globals.MainNavigation = MainNavigation;
        }
        public MainWindow()
        {
            this.InitializeComponent();
            InitDesgin();
            SetGlobalObjects();
            Settings.GetSettings();

            this.Title = "Minecraft Launcher";

            //navigate home
            MainNavigation.SelectedItem = HomeItem;
            RootFrame.Navigate(typeof(HomePage));
            NavigationService.UpdateBreadcrumb("Home", true);
            NavigationService.HideBreadcrumb();


            //MainNavigation.PaneTitle = Globals.Username;
            UsernameBlock.Text = Globals.Username;
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
            if (content == "Home")
            {
                RootFrame.Navigate(typeof(HomePage));
                NavigationService.UpdateBreadcrumb("Home", true);
                NavigationService.HideBreadcrumb();
            }
            if (content == "All Versions")
            {
                NavigationService.UpdateBreadcrumb("All Versions", true);
                NavigationService.ShowBreadcrumb();
                RootFrame.Navigate(typeof(AllVersionsPage));
            }
            if (content == "OptiFine")
            {
                NavigationService.UpdateBreadcrumb("OptiFine", true);
                NavigationService.ShowBreadcrumb();
                RootFrame.Navigate(typeof(OptiFinePage));
            }
            if (content == "About")
            {
                NavigationService.UpdateBreadcrumb("About", true);
                NavigationService.ShowBreadcrumb();
                RootFrame.Navigate(typeof(AboutPage));
            }

            //MainNavigation.PaneTitle = Globals.Username;
            UsernameBlock.Text = Globals.Username;
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
    }
}
