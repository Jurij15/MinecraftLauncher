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
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    //This page is a bit of a mess, needs organization
    public sealed partial class AllVersionsPage : Page
    {
        List<string> items = new List<string>();
        public void CreateCard(string VersionName)
        {
            items.Add(VersionName);

            ItemsPanel.ItemsSource = items;
        }

        void LoadVersions(bool bOnlyReleases)
        {

            List<string> versions = new List<string>();

            foreach (var item in VersionsHelper.GetAllVersions())
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
                CreateCard(item);
            }

            versions.Clear();
        }

        public AllVersionsPage()
        {
            this.InitializeComponent();
            LoadVersions(true);
            ReleasesOnly.IsChecked = true;
        }

        void CreateCardsForSpecifiedArray(List<string> items)
        {
            ItemsPanel.Items.Clear();
            foreach (var item in items)
            {
                CreateCard(item);
            }
        }

        private void CardAction_PointerPressed(object sender, PointerRoutedEventArgs e) //when the version card is pressed
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

            NavigationService.NavigateHiearchical(typeof(SelectedVersionPage),"Play "+version, false);
        }

        private void ReleasesOnly_Unchecked(object sender, RoutedEventArgs e)
        {
            var itemsSource = ItemsPanel.ItemsSource as System.Collections.IList;
            itemsSource.Clear();
            LoadVersions(false);
        }

        private void ReleasesOnly_Checked(object sender, RoutedEventArgs e)
        {
            var itemsSource = ItemsPanel.ItemsSource as System.Collections.IList;
            itemsSource.Clear();
            LoadVersions(true);
        }

        private void ItemsPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TotalCountBlock.Text = "Total Versions: " + ItemsPanel.Items.Count;
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
                foreach (var cat in VersionsHelper.GetAllVersions())
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
            string version = args.SelectedItem.ToString();

            Globals.CurrentVersion = version;

            NavigationService.NavigateHiearchical(typeof(SelectedVersionPage), "Play " + version, false);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
