using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.AppNotifications;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Pages.WorldsPages;
using MinecraftLauncherUniversal.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static MinecraftLauncherUniversal.Services.NavigationService;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        public ShellPage()
        {
            this.InitializeComponent();
            Globals.AllVersionsNavigationViewItemInfoBadge = AllVersionsInfoBadge;
            Globals.MainFrame = RootFrame;
            Globals.MainNavigationBreadcrumb = MainBreadcrumb;
            Globals.MainNavigation = MainNavigation;


            InitializeNavigationService(MainNavigation, MainBreadcrumb, RootFrame);

            OnMainShellStartup();
        }

        void OnMainShellStartup()
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

            SetStats();
        }


        void SetStats()
        {
            TotalInstalledBlock.Text = "Installed " + VersionManager.PrefetchedStatistics.TotalInstalled;
            TotalVersionsBlock.Text = "Total  " + VersionManager.PrefetchedStatistics.TotalAvailable;
            TotalReleasesBlock.Text = "Releases " + VersionManager.PrefetchedStatistics.TotalReleases;
            TotalOptiFineBlock.Text = "OptiFine " + VersionManager.PrefetchedStatistics.TotalOptiFine;
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

            PlayVersionPage.IsSearchedForAVersion = true;
            Navigate(typeof(PlayVersionPage), "Play", false, false);
        }


        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {

            //GC.Collect(); //idk, trying to lower ram usage
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
                Navigate(typeof(HomePage), "Home", true, true);
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
            if (args.SelectedItemContainer == WorldsItem)
            {
                Navigate(typeof(AllWorldsPage), "Select a World", true);
            }
            if (args.SelectedItemContainer == WikiItem)
            {
                Navigate(typeof(WikiPage), "Wiki", true, false);
            }
            if (args.SelectedItemContainer == ForgeItem)
            {
                Navigate(typeof(ForgePage), "Forge", true);
            }
            if (args.SelectedItemContainer == ModsItem)
            {
                Navigate(typeof(ModsPage), "Mods", true);
            }

            //GC.Collect(); //idk, trying to lower ram usage

            Log.Verbose("Navigated!");
        }
    }
}
