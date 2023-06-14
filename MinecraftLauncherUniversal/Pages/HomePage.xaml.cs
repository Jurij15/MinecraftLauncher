using CmlLib.Core;
using CmlLib.Core.Version;
using CmlLib.Core.VersionLoader;
using CmlLib.Utils;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Controls;
using MinecraftLauncherUniversal.Dialogs;
using MinecraftLauncherUniversal.Enums;
using MinecraftLauncherUniversal.Helpers;
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
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        List<string> items = new List<string>();
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

            foreach (var item in Globals.Recents)
            {
                items.Add(item);

                ItemsPanel.ItemsSource = items;
            }
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

        private void CardAction_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
        }

        private void CardAction_PointerExited(object sender, PointerRoutedEventArgs e)
        {
        }

        private async void CardAction_PointerPressed(object sender, PointerRoutedEventArgs e) //when the version card is pressed
        {
            StackPanel content = (StackPanel)(((CardAction)sender).Content);
            string version = "";
            foreach (var item in content.Children)
            {
                if (item.GetType() == typeof(TextBlock))
                {
                    TextBlock nameblock = (TextBlock)item;
                    version = nameblock.Text;
                }
            }

            Globals.CurrentVersion = version;
            /*
            NavigationService.NavigateHiearchical(typeof(SelectedVersionPage), "Play " + version, false);
            */

            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = Globals.MainGridXamlRoot;
            dialog.Title = "Play "+version;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Content = new QuickPlayDialogContent(dialog);

            dialog.CloseButtonText = "Close";
            dialog.CloseButtonClick += Dialog_CloseButtonClick;

            if (!Globals.CurrentVersion.Contains("OptiFine"))
            {
                dialog.Title = "Play Minecraft " + version;
            }
            else
            {
                dialog.Title = "Play " + version;
            }

            dialog.PrimaryButtonText = "Play";
            //dialog.PrimaryButtonClick += Dialog_PrimaryButtonClick;

            dialog.DefaultButton = ContentDialogButton.Primary;

            await dialog.ShowAsync();
        }

        private async void Dialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            sender.Hide();
        }

        private void PlayOptifine_Click(object sender, RoutedEventArgs e)
        {
            Globals.MainNavigation.SelectedItem = Globals.MainNavigation.MenuItems[2];
            Globals.MainFrame.Navigate(typeof(OptiFinePage));
            NavigationService.ShowBreadcrumb();
            NavigationService.UpdateBreadcrumb("OptiFine", true);
        }

        private void HomeHeader_Loaded(object sender, RoutedEventArgs e)
        {
            HomeHeader.SelectedPageIndex = Gallery.SelectedIndex;
        }

        private async void HomeContent_Loaded(object sender, RoutedEventArgs e)
        {
            SetUpdateStatus();
        }

        void SetUpdateStatus()
        {
            switch ( Updater.GetUpdateStatus())
            {
                case UpdateStatus.Fail:
                    UpdatesInfoBar.Title = "Checking Failed";
                    UpdatesInfoBar.Message = "Please check your internet connection!";
                    UpdatesInfoBar.Severity = InfoBarSeverity.Error;
                    UpdatesInfoBar.ActionButton.Visibility = Visibility.Collapsed;
                    break;
                case UpdateStatus.UpToDateWell:
                    UpdatesInfoBar.Title = "Up to Date";
                    UpdatesInfoBar.Message = "You are running the latest version of the launcher!";
                    UpdatesInfoBar.Severity = InfoBarSeverity.Success;
                    UpdatesInfoBar.ActionButton.Visibility = Visibility.Collapsed;
                    break;
                case UpdateStatus.UpdateAvailable:
                    UpdatesInfoBar.Title = "Update Available";
                    UpdatesInfoBar.Message = "Check the GitHub page for updates!";
                    UpdatesInfoBar.Severity = InfoBarSeverity.Warning;
                    UpdatesInfoBar.ActionButton.Visibility = Visibility.Visible;
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
    }
}
