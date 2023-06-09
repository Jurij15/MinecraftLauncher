﻿using MinecraftLauncher.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls.ContentDialogControl;

namespace MinecraftLauncher.Pages
{
    /// <summary>
    /// Interaction logic for PlayPage.xaml
    /// </summary>
    public partial class PlayPage : Page
    {
        public PlayPage()
        {
            InitializeComponent();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            AsyncLaunch();
        }

        async void AsyncLaunch()
        {
            int memooryinmb = Convert.ToInt32(RamAmountBox.Value) * 1024;

            LaunchingBar.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Collapsed;
            VersionStatusBox.Text = "Launching...";

            await PlayHelper.Launch(Globals.CurrentBuild, memooryinmb, Convert.ToBoolean(FullScreenMode.IsChecked));
            LaunchingBar.Visibility = Visibility.Collapsed;
            VersionStatusBox.Text = "Playing";

            //by now, it has already been launched, now store the build in recents
            Settings.SaveRecentBuild(Globals.CurrentBuild);
            PlayButton.Visibility = Visibility.Visible;

            Globals.MainSnackbarService.Show("Minecraft Launched", "Version " + Globals.CurrentBuild + " was launched", Wpf.Ui.Controls.ControlAppearance.Success, null, TimeSpan.FromSeconds(2));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Globals.CurrentBuild.Contains("OptiFine"))
            {
                VersionNameBlock.Text =Globals.CurrentBuild;
            }
            else
            {
                VersionNameBlock.Text = "Minecraft " + Globals.CurrentBuild;
            }
            UsernameBoxPlayPage_TextChanged(sender, null);
            LaunchingBar.Visibility = Visibility.Collapsed;

            UsernameBoxPlayPage.Text  = Globals.Username;

            bool installed = VersionsHelper.bIsVersionInstalled(Globals.CurrentBuild);

            if (installed)
            {
                PlayButton.Visibility = Visibility.Visible;
                VersionStatusBox.Text = "Ready to Play";
            }
            else if (!installed)
            {
                DownloadButton.Visibility = Visibility.Visible;
                VersionStatusBox.Text = "Download required";
            }
            else
            {
                VersionNotDetected.Visibility = Visibility.Visible;
                VersionStatusBox.Text = "";
            }

            //Globals.MainNavigationBreadcrumb.Visibility = Visibility.Collapsed;
        }

        private void UsernameBoxPlayPage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameBoxPlayPage.Text))
            {
                PlayButton.IsEnabled = false;
            }
            else
            {
                Globals.Username = UsernameBoxPlayPage.Text;
                PlayButton.IsEnabled = true;
            }
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadBefore();
        }

        async void DownloadBefore()
        {
            VersionStatusBox.Text = "Downloading...";
            int memooryinmb = Convert.ToInt32(RamAmountBox.Value) * 1024;

            LaunchingBar.Visibility = Visibility.Visible;

            await PlayHelper.Download(Globals.CurrentBuild, memooryinmb, Convert.ToBoolean(FullScreenMode.IsChecked));
            LaunchingBar.Visibility = Visibility.Collapsed;
            
            DownloadButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
            VersionStatusBox.Text = "Ready to Play";
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Globals.MainNavigationBreadcrumb.Visibility = Visibility.Visible;
        }
    }
}
