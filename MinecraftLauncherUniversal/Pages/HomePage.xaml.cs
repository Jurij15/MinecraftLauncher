using CmlLib.Core;
using CmlLib.Core.Version;
using CmlLib.Core.VersionLoader;
using CmlLib.Utils;
using CommunityToolkit.WinUI.Controls;
using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Dialogs;
using MinecraftLauncherUniversal.Enums;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Shell;
using WinUIEx;
using static MinecraftLauncherUniversal.Services.NavigationService;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
            HomeHeader.NumberOfPages = Gallery.Items.Count;
            HomeHeader.SelectedPageIndex = Gallery.SelectedIndex;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            HomeHeader.NumberOfPages = Gallery.Items.Count;
            HomeHeader.SelectedPageIndex = Gallery.SelectedIndex;

            VersionManager manager = new VersionManager();

            ItemsPanel.Items.Clear();
            foreach (var item in manager.GetAllRecentVersions())
            {
                SettingsCard card = new SettingsCard();
                card.Header = item;

                card.IsActionIconVisible = false;
                card.IsClickEnabled = true;

                card.Click += Card_Click;

                ItemsPanel.Items.Add(card);
            }

            NavigationService.ChangeBreadcrumbVisibility(false);
        }

        private void Card_Click(object sender, RoutedEventArgs e)
        {
            string Version = ((SettingsCard)sender).Header.ToString();

            Globals.CurrentVersion = Version;

            ItemsPanel.Items.Clear();

            Globals.MainNavigation.SelectedItem = Globals.MainNavigation.MenuItems[2];
            Navigate(typeof(AllVersionsPage), "Select a Version", true);

            PlayVersionPage.IsSearchedForAVersion = true;
            Navigate(typeof(PlayVersionPage), "Play", false, false);
        }

        private void HomeHeader_SelectedIndexChanged(PipsPager sender, PipsPagerSelectedIndexChangedEventArgs args)
        {
            Gallery.SelectedIndex = sender.SelectedPageIndex;
        }

        private void Gallery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            if (HomeHeader.IsLoaded)
            {
                return;
            }
            else
            {
                HomeHeader.SelectedPageIndex = Gallery.SelectedIndex;
            }
            */
        }

        private void ItemsPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //TotalCountBlock.Text = "Total Versions: " + ItemsPanel.Items.Count;
        }

        private void PlayOptifine_Click(object sender, RoutedEventArgs e)
        {
            Globals.MainNavigation.SelectedItem = Globals.MainNavigation.MenuItems[2];
            //NavigationService.Navigate(typeof(OptiFinePage), "OptiFine", false);
        }

        private void HomeHeader_Loaded(object sender, RoutedEventArgs e)
        {
            HomeHeader.SelectedPageIndex = Gallery.SelectedIndex;
        }

        private async void HomeContent_Loaded(object sender, RoutedEventArgs e)
        {
            SetUpdateStatus();


            if (Globals.bIsFirstTimeRun)
            {
                //DialogService.ShowWelcomeSetupDialog();
            }
        }

        void SetUpdateStatus()
        {
            switch ( Updater.GetUpdateStatus())
            {
                case UpdateStatus.Fail:
                    UpdatesInfoBar.Title = "Checking Failed";
                    UpdatesInfoBar.Message = "Please check your internet connection!";
                    UpdatesInfoBar.Severity = InfoBarSeverity.Error;
                    //UpdatesInfoBar.ActionButton.Visibility = Visibility.Collapsed;
                    break;
                case UpdateStatus.UpToDateWell:
                    UpdatesInfoBar.Title = "Up to Date";
                    UpdatesInfoBar.Message = "You are running the latest version of the launcher!";
                    UpdatesInfoBar.Severity = InfoBarSeverity.Success;
                    //UpdatesInfoBar.ActionButton.Visibility = Visibility.Collapsed;
                    break;
                case UpdateStatus.UpdateAvailable:
                    UpdatesInfoBar.Title = "Update Available";
                    UpdatesInfoBar.Message = "Check the GitHub page for updates!";
                    UpdatesInfoBar.Severity = InfoBarSeverity.Warning;
                    //UpdatesInfoBar.ActionButton.Visibility = Visibility.Visible;
                    break;
                case UpdateStatus.PreRelease:
                    UpdatesInfoBar.Title = "PreRelease";
                    UpdatesInfoBar.Message = "Updates are disabled on PreRelease!";
                    UpdatesInfoBar.Severity = InfoBarSeverity.Informational;
                    //UpdatesInfoBar.ActionButton.Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }

            UpdatesInfoBar.IsOpen = true;
        }

        private void PlayVersionCard_Click(object sender, RoutedEventArgs e)
        {
            Globals.MainNavigation.SelectedItem = Globals.MainNavigation.MenuItems[2];
            //NavigationService.Navigate(typeof(AllVersionsPage), "Select a Version", true);
        }

        private void PlayOptifineCard_Click(object sender, RoutedEventArgs e)
        {
            Globals.MainNavigation.SelectedItem = Globals.MainNavigation.MenuItems[6];
            //NavigationService.Navigate(typeof(OptiFinePage), "OptiFine", true);
        }

        private void ChangeUsernameCard_Click(object sender, RoutedEventArgs e)
        {
            Globals.MainNavigation.SelectedItem = Globals.MainNavigation.SettingsItem;
            NavigationService.Navigate(typeof(SettingsPage), "Settings", true);
            NavigationService.Navigate(typeof(PlayerSettingsPage), "Player Settings", false);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationService.ChangeBreadcrumbVisibility(false);
        }

        private void OpenPatchNotesBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(AboutPage), "About", true);
            NavigationService.Navigate(typeof(NewsPage), "What's new in " + Globals.VersionString, false);
        }
    }
}
