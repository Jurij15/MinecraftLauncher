using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherWinUI.Assets;
using MinecraftLauncherWinUI.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static System.Net.WebRequestMethods;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherWinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WinUIEx.WindowEx
    {
        void SetTitleBar()
        {
            this.ExtendsContentIntoTitleBar = true;
            ///this.IsTitleBarVisible = false;
            this.SetTitleBar(AppTitleBar);
        }
        public MainWindow()
        {
            this.InitializeComponent();
            SetTitleBar();

            MainNavigation.SelectedItem = MainNavigation.MenuItems.OfType<NavigationViewItem>().First();
            ContentFrame.Navigate(
                       typeof(HomePage),
                       null,
                       new Microsoft.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo()
                       );
        }

        private void MainNavigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            NavigationViewItem CurrentPage = (NavigationViewItem)MainNavigation.SelectedItem;
            if (CurrentPage== HomePage)
            {
                ContentFrame.Navigate(typeof(HomePage));
            }
            else if (CurrentPage == AllVersionsPage)
            {
                ContentFrame.Navigate(typeof(AllVersionsPage));
                //MainNavigation.Header = AllVersionsPage.Content;
            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            MainNavigation.IsBackEnabled = ContentFrame.CanGoBack;

            MainNavigation.Header = ((NavigationViewItem)MainNavigation.SelectedItem)?.Content?.ToString();
        }
    }
}
