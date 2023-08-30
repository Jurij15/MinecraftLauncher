using CommunityToolkit.Labs.WinUI;
using CommunityToolkit.WinUI.UI.Animations;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using MinecraftLauncherUniversal.Controls;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Interop;
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
using Windows.Foundation.Metadata;
using Windows.UI.Composition;
using Windows.Web.Http.Diagnostics;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    //This page is a bit of a mess, needs organization
    public class VersionItem
    {
        public string Version { get; set; }
        public string VersionInstalledState { get; set; }
    }
    public sealed partial class AllVersionsPage : Page
    {
        public static VersionCardControl _storedCard;

        HashSet<VersionItem> VersionsSource;
        bool bInitFinished = false;
        public enum PopInAnimationDuration
        {
            VeryShort,
            Short,
            Normal,
            Long
        }
        //weird fix for duplication
        bool CheckIfCardAlreadyExists(string Header)
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
                    await Task.Delay(1);
                    //await Task.Delay(5);
                    break;
                case PopInAnimationDuration.Normal:
                    await Task.Delay(1);
                    //await Task.Delay(7);
                    break;
                case PopInAnimationDuration.Long:
                    await Task.Delay(1);
                    //await Task.Delay(9);
                    break;
                default:
                    await Task.Delay(1);
                    //await Task.Delay(7);
                    break;
            }

            if (!CheckIfCardAlreadyExists(VersionName))
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
            NavigationService.Navigate(typeof(SelectedVersionPage), "Play " + version, false);
        }

        async void LoadVersions(bool bOnlyReleases)
        {
            ReleasesOnly.IsEnabled = false;
            ShowInstalledStatus.IsEnabled = false;
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

            VersionsSource = new HashSet<VersionItem>();
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
                //await CreateCardAsync(item, duration, Convert.ToBoolean(ShowInstalledStatus.IsChecked));

                VersionItem version = new VersionItem();
                version.Version = item;
                version.VersionInstalledState = "Unknown";

                if (VersionsHelper.bIsVersionInstalled(item))
                {
                    version.VersionInstalledState = "Installed";
                }
                else
                {
                    version.VersionInstalledState = "Not Installed";
                }

                VersionsSource.Add(version);
            }

            ItemsPanel.ItemsSource = VersionsSource;

            versions.Clear();
            ReleasesOnly.IsEnabled = true;
            ShowInstalledStatus.IsEnabled = true;

            TotalCountBlock.Text = "Total Versions: " + ItemsPanel.Items.Count;
            Globals.AllVersionsNavigationViewItemInfoBadge.Value = ItemsPanel.Items.Count;
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
            LoadVersions(false);
        }

        private void ReleasesOnly_Checked(object sender, RoutedEventArgs e)
        {
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

            NavigationService.Navigate(typeof(SelectedVersionPage), "Play " + version, false);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //var itemsSource = ItemsPanel.ItemsSource as IList ;
        }

        private void ShowInstalledStatus_Checked(object sender, RoutedEventArgs e)
        {
            if (!bInitFinished)
            {
                return;
            }
            LoadVersions(Convert.ToBoolean(ReleasesOnly.IsChecked));
        }

        private void ShowInstalledStatus_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!bInitFinished)
            {
                return;
            }
            LoadVersions(Convert.ToBoolean(ReleasesOnly.IsChecked));
        }

        Type NavigatedFromPageType;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigatedFromPageType = e.SourcePageType as Type;
            base.OnNavigatedTo(e);
        }

        private async void ItemsPanel_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(500);
            if (NavigatedFromPageType.GetType() == typeof(PlayVersionPage))
            {
                // Retrieve and start the connected animation for back navigation
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackConnectedAnimation");
                if (_storedCard == null)
                {
                    MessageBox.Show("null!");
                }
                if (animation != null)
                {
                    animation.TryStart(_storedCard);
                }

                animation.Completed += Animation_Completed;
            }
        }

        private void Animation_Completed(ConnectedAnimation sender, object args)
        {
            MessageBox.Show("back anim completed!");
        }

        private void VersionCardControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            VersionCardControl card = sender as VersionCardControl;
            if (card != null)
            {
                string version = card.Version;
                //MessageBox.Show(version);
                //DialogService.ShowSimpleDialog("content", version);
                //string version = "test";

                Globals.CurrentVersion = version;
                _storedCard = card;

                // Prepare the connected animation
                var animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", card);

                NavigationService.NavigateSuppressedAnim(typeof(PlayVersionPage), "Play " + version, false, false);
            }
        }
    }
}
