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

            MainNavigation.SelectedItem = HomeItem;
            RootFrame.Navigate(typeof(HomePage));
            NavigationService.UpdateBreadcrumb("Home", true);
        }

        private void MainNavigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            string content = ((Microsoft.UI.Xaml.Controls.NavigationViewItem)sender.SelectedItem).Content.ToString();
            if (args.IsSettingsInvoked)
            {
                RootFrame.Navigate(typeof(SettingsPage));
                NavigationService.UpdateBreadcrumb("Settings", true);
            }
            if (content == "Home")
            {
                RootFrame.Navigate(typeof(HomePage));
                NavigationService.UpdateBreadcrumb("Home", true);
            }
            if (content == "All Versions")
            {
                RootFrame.Navigate(typeof(AllVersionsPage));
                NavigationService.UpdateBreadcrumb("All Versions", true);
            }
            if (content == "OptiFine")
            {
                RootFrame.Navigate(typeof(OptiFinePage));
                NavigationService.UpdateBreadcrumb("OptiFine", true);
            }
            if (content == "About")
            {
                RootFrame.Navigate(typeof(AboutPage));
                NavigationService.UpdateBreadcrumb("About", true);
            }
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
    }
}
