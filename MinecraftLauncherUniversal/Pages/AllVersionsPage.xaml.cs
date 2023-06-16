using CommunityToolkit.Labs.WinUI;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using MinecraftLauncherUniversal.Controls;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.Web.Http.Diagnostics;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    //This page is a bit of a mess, needs organization
    public sealed partial class AllVersionsPage : Page
    {
        bool bInitFinished = false;
        public enum PopInAnimationDuration
        {
            VeryShort,
            Short,
            Normal,
            Long
        }
        //weird fix for duplication
        async Task<bool> CheckIfCardAlreadyExists(string Header)
        {
            bool RetVal = false;

            foreach (var item in ItemsPanel.Items)
            {
                if ((item as SettingsCard).Header.ToString() == Header)
                {
                    RetVal = true;
                }
            }

            return RetVal;
        }
        public async Task CreateCardAsync(string VersionName, PopInAnimationDuration AnimDuration, bool bCheckIfInstalled)
        {
            if (VersionManager.AllVersionsGlobal.Count <= 0)
            {
                ContentDialog dialog = new ContentDialog();
                dialog.XamlRoot = Globals.MainGridXamlRoot;
                dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                dialog.Title = "Error";
                dialog.Content = "An Error occured while searching for all available versions. Please restart the app";

                dialog.CloseButtonText = "OK";
                dialog.CloseButtonClick += Dialog_CloseButtonClick;

                await dialog.ShowAsync();
            }

            SettingsCard NewCard = new SettingsCard();
            NewCard.Header = VersionName.ToString();
            NewCard.IsEnabled = true;

            //NewCard.Margin = new Thickness(2, 2, 2, 2);

            NewCard.IsActionIconVisible = false;
            NewCard.IsClickEnabled = true;
            NewCard.Click += NewCard_Click;

            if (bCheckIfInstalled)
            {
                if (VersionsHelper.bIsVersionInstalled(VersionName))
                {
                    NewCard.Description = "Installed";
                }
                else
                {
                    NewCard.Description = "Not Installed";
                }
            }

            switch (AnimDuration)
            {
                case PopInAnimationDuration.VeryShort:
                    await Task.Delay(1);
                    break;
                case PopInAnimationDuration.Short:
                    await Task.Delay(5);
                    break;
                case PopInAnimationDuration.Normal:
                    await Task.Delay(7);
                    break;
                case PopInAnimationDuration.Long:
                    await Task.Delay(9);
                    break;
                default:
                    await Task.Delay(7);
                    break;
            }

            if (!await CheckIfCardAlreadyExists(VersionName))
            {
                ItemsPanel.Items.Add(NewCard);
            }

            TotalCountBlock.Text = "Total Versions: " + ItemsPanel.Items.Count;
            Globals.AllVersionsNavigationViewItemInfoBadge.Value = ItemsPanel.Items.Count;
        }

        private void Dialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Globals.RestartApp();
        }

        private void NewCard_Click(object sender, RoutedEventArgs e)
        {
            SettingsCard content = sender as SettingsCard;
            string version = content.Header.ToString();
            //DialogService.ShowSimpleDialog("content", version);
            //string version = "test";

            Globals.CurrentVersion = version;

            ItemsPanel.Items.Clear();

            NavigationService.NavigateHiearchical(typeof(SelectedVersionPage), "Play "+version, false);
            //NavigationService.NavigateHiearchical(typeof(AboutPage), "test", false);
        }

        async void LoadVersions(bool bOnlyReleases)
        {
            ReleasesOnly.IsEnabled = false;
            VersionManager manager = new VersionManager();
            HashSet<string> versions = new HashSet<string>();

            foreach (var item in VersionManager.AllVersionsGlobal)
            {
                if (bOnlyReleases)
                {
                    if (VersionsHelper.bIsReleaseVersion(item))
                    {
                        versions.Add(item);
                    }
                }
                else if (!bOnlyReleases)
                {
                    versions.Add(item);
                }
            }

            //sort the array, but only of releases, no snapshots
            if (bOnlyReleases)
            {
                Array.Sort(versions.ToArray(), new Comparison<string>((x, y) =>
                {
                    // Split the strings into segments separated by decimal points
                    string[] xSegments = x.Split('.');
                    string[] ySegments = y.Split('.');

                    // Skip the first segment and parse the remaining segments as decimals
                    decimal xDecimal = decimal.Parse(string.Join("", xSegments.Skip(1)));
                    decimal yDecimal = decimal.Parse(string.Join("", ySegments.Skip(1)));

                    // Compare the two decimal values
                    return yDecimal.CompareTo(xDecimal);
                }));
            }
            
            foreach (var item in versions)
            {
                PopInAnimationDuration duration = PopInAnimationDuration.Normal;
                if (versions.Count <= 10)
                {
                    duration = PopInAnimationDuration.Long;
                }
                if (versions.Count <= 30)
                {
                    duration = PopInAnimationDuration.Normal;
                }
                if (versions.Count <= 80)
                {
                    duration = PopInAnimationDuration.Short;
                }
                if (versions.Count > 120)
                {
                    duration = PopInAnimationDuration.VeryShort;
                }
                await CreateCardAsync(item, duration, Convert.ToBoolean(ShowInstalledStatus.IsChecked));
            }

            versions.Clear();
            ReleasesOnly.IsEnabled = true;
        }

        public AllVersionsPage()
        {
            this.InitializeComponent();
            LoadVersions(true);
            ReleasesOnly.IsChecked = true;

            //ShowInstalledStatus.IsChecked = true;

            bInitFinished = true;
        }

        private void ReleasesOnly_Unchecked(object sender, RoutedEventArgs e)
        {
            ItemsPanel.Items.Clear();
            LoadVersions(false);
        }

        private void ReleasesOnly_Checked(object sender, RoutedEventArgs e)
        {
            ItemsPanel.Items.Clear();
            LoadVersions(true);
        }

        private void ItemsPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TotalCountBlock.Text = "Total Versions: " + ItemsPanel.Items.Count;
            Globals.AllVersionsNavigationViewItemInfoBadge.Value = ItemsPanel.Items.Count;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
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

        private void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem.ToString() == "No results found")
            {
                sender.Text = string.Empty;
                return;
            }
            string version = args.SelectedItem.ToString();

            Globals.CurrentVersion = version;

            NavigationService.NavigateHiearchical(typeof(SelectedVersionPage), "Play " + version, false);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //var itemsSource = ItemsPanel.ItemsSource as IList ;
            ItemsPanel.Items.Clear();
        }

        private void ShowInstalledStatus_Checked(object sender, RoutedEventArgs e)
        {
            if (!bInitFinished)
            {
                return;
            }
            ItemsPanel.Items.Clear();
            LoadVersions(Convert.ToBoolean(ReleasesOnly.IsChecked));
        }

        private void ShowInstalledStatus_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!bInitFinished)
            {
                return;
            }
            ItemsPanel.Items.Clear();
            LoadVersions(Convert.ToBoolean(ReleasesOnly.IsChecked));
        }
    }
}
