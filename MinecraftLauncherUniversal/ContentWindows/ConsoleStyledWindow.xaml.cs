using CommunityToolkit.WinUI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Core;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.ContentWindows
{
    /// <summary>
    /// This was an idea i got out of boredroom, might not finish it but it looks a little cool
    /// </summary>
    public sealed partial class ConsoleStyledWindow : WindowEx
    {
        string CurrentVersion;
        public ConsoleStyledWindow()
        {
            this.InitializeComponent();

            this.CenterOnScreen();

            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(AppTitleBar);

            this.Title = "MinecraftLauncher - Console View";

            this.PresenterKind = Microsoft.UI.Windowing.AppWindowPresenterKind.FullScreen;
        }

        private void ItemsPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void ItemsPanel_Loaded(object sender, RoutedEventArgs e)
        {
            VersionManager manager = new VersionManager();  
            foreach (var item in VersionManager.AllVersionsGlobal)
            {
                if (VersionsHelper.bIsReleaseVersion(item))
                {
                    SettingsCard card = new SettingsCard();
                    card.IsClickEnabled = true;
                    card.Header = item;
                    card.Click += Card_Click;

                    ItemsPanel.Items.Add(card);
                }
            }

            if (ItemsPanel.Items.Count == 0)
            {
                DialogService.ShowSimpleDialog("Error", "this shouldnt happen, please restart your app!");
            }

            InitCard(((SettingsCard)ItemsPanel.Items[1]));
        }

        void InitCard(SettingsCard Card)
        {
            CurrentVersion = Card.Header.ToString();
            if (CurrentVersion.Contains("OptiFine"))
            {
                MinecraftVersionBlock.Text = CurrentVersion;
            }
            else
            {
                MinecraftVersionBlock.Text = "Minecraft " + CurrentVersion;
            }

            if (VersionsHelper.bIsVersionInstalled(CurrentVersion))
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

        async void DownloadBefore()
        {
            DownloadButton.Visibility = Visibility.Collapsed;
            StatusBox.Text = "Downloading...";
            int memooryinmb = Globals.Settings.MemoryAllocationInGB * 1024;

            LoadingRing.Visibility = Visibility.Visible;

            PlayCore core = new PlayCore(CurrentVersion, memooryinmb, Globals.Settings.Fullscreen, Globals.Settings.CustomUUID, Globals.Settings.CustomAccessToken);
            await core.Download(OnProgressChanged);
            LoadingRing.Value = 0;
            LoadingRing.Visibility = Visibility.Collapsed;
            DownloadButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
            StatusBox.Text = "Ready to Play";

            LoadingRing.Value = 0;

            NotificationService.SendSimpleToast("Download Complete", "Minecraft " + Globals.CurrentVersion + " is ready to play!", 1.5);
        }

        void OnProgressChanged(int value)
        {
            LoadingRing.IsIndeterminate = false;
            LoadingRing.Value = value;
        }

        async void AsyncLaunch()
        {
            bool bSucess = false;
            int memooryinmb = Globals.Settings.MemoryAllocationInGB * 1024;

            LoadingRing.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Collapsed;
            StatusBox.Text = "Launching...";

            PlayCore core = new PlayCore(CurrentVersion, memooryinmb, Globals.Settings.Fullscreen, Globals.Settings.CustomAccessToken, Globals.Settings.CustomAccessToken);
            bool result = await core.Launch();
            if (!result) { DialogService.ShowSimpleDialog("An Error Occured", core.GetLaunchErrors()); } else { bSucess = true; }
            LoadingRing.Visibility = Visibility.Collapsed;
            StatusBox.Text = "Playing";

            //by now, it has already been launched, now store the build in recents
            VersionManager manager = new VersionManager();
            //manager.AddNewBuildToRecents(Globals.CurrentVersion);
            PlayButton.Visibility = Visibility.Visible;

            if (bSucess)
            {
                await Task.Delay(800);

                this.Minimize();
                NotificationService.SendSimpleToast("Launched", "Minecraft " + Globals.CurrentVersion + " launched successfully!", 1.5);
            }
        }

        private void Card_Click(object sender, RoutedEventArgs e)
        {
            InitCard(sender as SettingsCard);
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadBefore();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            AsyncLaunch();
        }
    }
}
