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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllVersionsPage : Page
    {
        Microsoft.UI.Composition.Compositor _compositor = Microsoft.UI.Xaml.Media.CompositionTarget.GetCompositorForCurrentThread();
        Microsoft.UI.Composition.SpringVector3NaturalMotionAnimation _springAnimation;

        private void CreateOrUpdateSpringAnimation(float finalValue)
        {
            if (_springAnimation == null)
            {
                _springAnimation = _compositor.CreateSpringVector3Animation();
                _springAnimation.Target = "Scale";
            }

            _springAnimation.FinalValue = new Vector3(finalValue);
        }
        List<string> items = new List<string>();
        public void CreateCard(string VersionName)
        {
            items.Add(VersionName);

            ItemsPanel.ItemsSource = items;
        }

        public AllVersionsPage()
        {
            this.InitializeComponent();


            List<string> items = new List<string>();
            foreach (var item in VersionsHelper.GetAllVersions())
            {
                items.Add(item);
            }
            if (items.Count == 0)
            {
                HyperlinkButton refreshbtn = new HyperlinkButton();
                refreshbtn.Click += Refreshbtn_Click;
                ItemsPanel.Items.Add(refreshbtn);
            }
            foreach (var item in VersionsHelper.GetAllVersions())
            {
                if (VersionsHelper.bIsReleaseVersion(item))
                {
                    CreateCard(item);
                }
            }

            AllSortRadio.IsChecked = true;
            ReleasesOnly.IsChecked = true;
        }

        private void Refreshbtn_Click(object sender, RoutedEventArgs e)
        {
            List<string> items = new List<string>();
            foreach (var item in VersionsHelper.GetAllVersions())
            {
                items.Add(item);
            }
            if (items.Count == 0)
            {
                HyperlinkButton refreshbtn = new HyperlinkButton();
                refreshbtn.Click += Refreshbtn_Click;
                ItemsPanel.Items.Add(refreshbtn);
            }
            foreach (var item in VersionsHelper.GetAllVersions())
            {
                if (VersionsHelper.bIsReleaseVersion(item))
                {
                    CreateCard(item);
                }
            }
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

        private void ViewCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ReleasesOnly_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void ReleasesOnly_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void OptiFineOnly_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void OptiFineOnly_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ItemsPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TotalCountBlock.Text = "Total Versions: " + ItemsPanel.Items.Count;
        }

        private void CardAction_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.2f);

            (sender as UIElement).StartAnimation(_springAnimation);

        }

        private void CardAction_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            // Scale back down to 1.0
            CreateOrUpdateSpringAnimation(1.0f);

            (sender as UIElement).StartAnimation(_springAnimation);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
