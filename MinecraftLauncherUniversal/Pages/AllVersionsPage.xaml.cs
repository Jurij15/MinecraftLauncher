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
using Serilog;
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

        List<VersionItem> VersionsSource;
        bool bInitFinished = false;

        void LoadVersions(bool bOnlyReleases)
        {
            VersionsSource = new List<VersionItem>();
            VersionsSource.Clear();
            try
            {
                Log.Verbose("[AllVersionsPage]LoadVersions Called");
                Log.Verbose("[AllVersionsPage]LoadVersions Called");
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

                //VersionsSource = new List<VersionItem>();
                foreach (var item in versions)
                {
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
                TotalCountBlock.Text = "Total Versions: " + VersionsSource.Count;
                ShowInstalledStatus.IsEnabled = true;
                Log.Verbose("[AllVersionsPage]LoadVersions Call finished");
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException.Message);
                MessageBox.Show(ex.InnerException.Message);
                throw;
            }
        }

        public AllVersionsPage()
        {
            Log.Verbose("[AllVersionsPage]Constructing Page");
            try
            {
                this.InitializeComponent();
                Log.Verbose("Loading versions!");
                VersionsSource = new List<VersionItem>();
                LoadVersions(true);
                Log.Verbose("Loaded!");
                ReleasesOnly.IsChecked = true;

                //ShowInstalledStatus.IsChecked = true;

                bInitFinished = true;
                Log.Information("InitFinished!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            Log.Verbose("[AllVersionsPage]Construction finished");
        }

        private void ReleasesOnly_Unchecked(object sender, RoutedEventArgs e)
        {
            Log.Verbose("[AllVersionsPage]ReleasesOnly_Unchecked Called");
            LoadVersions(false);
            Log.Verbose("[AllVersionsPage]ReleasesOnly_Unchecked Call finished");
        }

        private void ReleasesOnly_Checked(object sender, RoutedEventArgs e)
        {
            Log.Verbose("[AllVersionsPage]ReleasesOnly_Checked Called");
            LoadVersions(true);
            Log.Verbose("[AllVersionsPage]ReleasesOnly_Checked Call finished");
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            Log.Verbose("[AllVersionsPage]SearchBox_TextChanged Called");
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
            Log.Verbose("[AllVersionsPage]SearchBox_TextChanged Call finished");
        }

        private void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Log.Verbose("[AllVersionsPage]SearchBox_SuggestionChosen Called");
            if (args.SelectedItem.ToString() == "No results found")
            {
                sender.Text = string.Empty;
                return;
            }
            string version = args.SelectedItem.ToString();

            Globals.CurrentVersion = version;

            NavigationService.NavigateSuppressedAnim(typeof(PlayVersionPage), "Play " + version, true, false);

            Log.Verbose("[AllVersionsPage]SearchBox_SuggestionChosen call finished");
        }

        private void ShowInstalledStatus_Checked(object sender, RoutedEventArgs e)
        {
            Log.Verbose("[AllVersionsPage]ShowInstalledStatus_Checked Called");
            if (!bInitFinished)
            {
                return;
            }
            LoadVersions(Convert.ToBoolean(ReleasesOnly.IsChecked));
            Log.Verbose("[AllVersionsPage]ShowInstalledStatus_Checked Call finished");
        }

        private void ShowInstalledStatus_Unchecked(object sender, RoutedEventArgs e)
        {
            Log.Verbose("[AllVersionsPage]ShowInstalledStatus_Unchecked Called");
            if (!bInitFinished)
            {
                return;
            }
            LoadVersions(Convert.ToBoolean(ReleasesOnly.IsChecked));
            Log.Verbose("[AllVersionsPage]ShowInstalledStatus_Unchecked Call finished");
        }

        private async void ItemsPanel_Loaded(object sender, RoutedEventArgs e)
        {
            Log.Verbose("[AllVersionsPage]ItemsPanel_Loaded Called");
            if (_storedCard != null)
            {
                ItemsPanel.ScrollIntoView(_storedCard, ScrollIntoViewAlignment.Leading);
                ItemsPanel.UpdateLayout();
                try
                {
                    // Retrieve and start the connected animation for back navigation
                    //MessageBox.Show("test");
                    var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackAnim");
                    if (animation != null)
                    {
                        bool result = animation.TryStart(_storedCard);
                    }

                    var imganimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ImgBackAnim");
                    if (imganimation != null)
                    {
                        bool result = imganimation.TryStart(_storedCard.MinecraftImage);
                    }

                    var textanim = ConnectedAnimationService.GetForCurrentView().GetAnimation("TextBackAnim");
                    if (textanim != null)
                    {
                        bool result = textanim.TryStart(_storedCard.VersionTextBlock);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
            await Task.Delay(80); //delay it so that everything loads
            LoadVersions(Convert.ToBoolean(ReleasesOnly.IsChecked));
            //ISSUE: Upon refresh, it does not scroll back into view.
            await Task.Delay(80); //delay it again so that everything loads
            if (_storedCard != null)
            {
                ItemsPanel.ScrollIntoView(_storedCard);
                _storedCard = null;
            }
            Log.Verbose("[AllVersionsPage]ItemsPanel_Loaded Call finished");
        }

        private void VersionCardControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Log.Verbose("[AllVersionsPage]VersionCardControl_PointerPressed Called");
            VersionCardControl card = sender as VersionCardControl;
            if (card != null)
            {
                //card.MinecraftImage.Visibility = Visibility.Collapsed;

                string version = card.Version;

                Globals.CurrentVersion = version;
                _storedCard = card;

                var imageanim = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("forwardImageAnim", _storedCard.MinecraftImage);
                var animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", _storedCard);
                var textanim = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardTextAnim", _storedCard.VersionTextBlock);

                NavigationService.NavigateSuppressedAnim(typeof(PlayVersionPage), "Play " + version, false, false);
            }
            Log.Verbose("[AllVersionsPage]VersionCardControl_PointerPressed Call finished");
        }
    }
}
