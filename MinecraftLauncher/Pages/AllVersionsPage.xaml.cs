using MinecraftLauncher.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MinecraftLauncher.Pages
{
    /// <summary>
    /// Interaction logic for AllVersionsPage.xaml
    /// </summary>
    public partial class AllVersionsPage : Page
    {
        void CreateCard(string VersionName)
        {
            Wpf.Ui.Controls.CardAction newCard = new Wpf.Ui.Controls.CardAction();
            StackPanel panel = new StackPanel();

            TextBlock name = new TextBlock();

            string CorrectVersionName = StringHelper.ReplaceDisallowedChars(VersionName);

            name.Text = VersionName;
            name.TextWrapping = TextWrapping.NoWrap;
            name.TextTrimming = TextTrimming.CharacterEllipsis;

            panel.Children.Add(name);

            newCard.ToolTip = VersionName;

            newCard.Content = panel;

            newCard.Name = CorrectVersionName;

            newCard.Width = 132;

            newCard.Margin = new Thickness(2);

            newCard.IsChevronVisible = false;

            newCard.Click += NewCard_Click;

            ItemsPanel.Children.Add(newCard);

            TotalCountBlock.Text = "Total Versions: " + ItemsPanel.Children.Count.ToString();
        }

        private void NewCard_Click(object sender, RoutedEventArgs e)
        {
            string name = StringHelper.ReturnDisallowedChars(((Wpf.Ui.Controls.CardAction)sender).Name);
            Globals.CurrentBuild = name;

            Globals.PlayMenuItem.Content = "Play " + name;

            Globals.MainNavigation.NavigateWithHierarchy(typeof(PlayPage));
        }

        public AllVersionsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshDefault();
            ReleasesOnly_Checked(sender, e);
            ReleasesOnly.IsChecked = true;
        }

        void RefreshDefault()
        {
            Globals.AllVersionsArray.Clear();
            ItemsPanel.Children.Clear();
            OnStart();
            CreateCardsForArray();
        }

        async void OnStart()
        {
            foreach (var version in VersionsHelper.GetAllVersions())
            {
                Globals.AllVersionsArray.Add(version);
            }
            //MessageBox.Show(Globals.AllVersionsArray.Count.ToString());

            SearchBox.ItemsSource = Globals.AllVersionsArray;

            if(Globals.AllVersionsArray.Count < 1)
            {
                Wpf.Ui.Controls.Hyperlink link = new Wpf.Ui.Controls.Hyperlink();
                link.Click += (s, e) =>
                {
                    Page_Loaded(null, null) ;
                };
                link.Content = "Refresh";
                ItemsPanel.Children.Add(link);
            }
        }

        public void CreateCardsForArray()
        {
            foreach (var item in Globals.AllVersionsArray)
            {
                //MessageBox.Show(item.ToString());
                CreateCard(item);
            }

            AllSortRadio.IsChecked = true;
        }

        private void SearchBox_TextChanged(Wpf.Ui.Controls.AutoSuggestBoxControl.AutoSuggestBox sender, Wpf.Ui.Controls.AutoSuggestBoxControl.AutoSuggestBoxTextChangedEventArgs args)
        {
            List<string> ResultsArray = new List<string>();
            foreach (var item in Globals.AllVersionsArray)
            {
                if (item.Contains(SearchBox.Text))
                {
                    ResultsArray.Add(item);
                }
            }

            ItemsPanel.Children.Clear();

            foreach (var item in ResultsArray)
            {
                CreateCard(item);
            }
        }

        void ClearArrays()
        {
            Globals.AllVersionsArray.Clear();
            ItemsPanel.Children.Clear();
        }

        private void ReleasesOnly_Checked(object sender, RoutedEventArgs e)
        {
            if (OptiFineOnly.IsChecked == true)
            {
                OptiFineOnly.IsChecked = false;
            }
            ClearArrays();
            foreach (var item in VersionsHelper.GetAllVersions())
            {
                if (VersionsHelper.bIsReleaseVersion(item))
                {
                    Globals.AllVersionsArray.Add(item);
                }
            }

            Array.Sort(Globals.AllVersionsArray.ToArray(), new Comparison<string>((x, y) =>
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

            foreach (var item in Globals.AllVersionsArray)
            {
                CreateCard(item);
            }
        }

        private void ReleasesOnly_Unchecked(object sender, RoutedEventArgs e)
        {
            RefreshDefault();
        }

        private void OptiFineOnly_Checked(object sender, RoutedEventArgs e)
        {
            if (ReleasesOnly.IsChecked == true)
            {
                ReleasesOnly.IsChecked = false;
            }

            ClearArrays();
            foreach (var item in VersionsHelper.GetAllVersions())
            {
                if (VersionsHelper.bIsOptifine(item))
                {
                    Globals.AllVersionsArray.Add(item);
                }
            }

            Array.Sort(Globals.AllVersionsArray.ToArray(), new Comparison<string>((x, y) =>
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

            foreach (var item in Globals.AllVersionsArray)
            {
                CreateCard(item);
            }
        }

        private void OptiFineOnly_Unchecked(object sender, RoutedEventArgs e)
        {
            RefreshDefault();
        }

        private void ItemsPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TotalCountBlock.Text = "Total Versions: " + ItemsPanel.Children.Count.ToString();
            if (ItemsPanel.Children.Count < 1)
            {
                Wpf.Ui.Controls.Hyperlink link = new Wpf.Ui.Controls.Hyperlink();
                link.Click += (s, e) =>
                {
                    Page_Loaded(null, null);
                };
                link.Content = "Refresh";
                ItemsPanel.Children.Add(link);
            }
        }

        private void InstalledSortRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (ReleasesOnly.IsChecked == true)
            {
                ClearArrays();
                foreach (var item in VersionsHelper.GetAllInstalledVersions())
                {
                    if (VersionsHelper.bIsReleaseVersion(item))
                    {
                        Globals.AllVersionsArray.Add(item);
                    }
                }

                Array.Sort(Globals.AllVersionsArray.ToArray(), new Comparison<string>((x, y) =>
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

                foreach (var item in Globals.AllVersionsArray)
                {
                    CreateCard(item);
                }
            }
            else
            {
                ClearArrays();
                foreach (var item in VersionsHelper.GetAllInstalledVersions())
                {
                    if (VersionsHelper.bIsOptifine(item))
                    {
                        Globals.AllVersionsArray.Add(item);
                    }
                }

                Array.Sort(Globals.AllVersionsArray.ToArray(), new Comparison<string>((x, y) =>
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

                foreach (var item in Globals.AllVersionsArray)
                {
                    CreateCard(item);
                }
            }
        }

        private void AllSortRadio_Checked(object sender, RoutedEventArgs e)
        {
            Page_Loaded(sender, e);
        }
    }
}
