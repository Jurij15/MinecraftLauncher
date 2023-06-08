using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Controls;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
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

            NavigationService.NavigateHiearchical(typeof(SelectedVersionPage), "Play " + version, false);
        }

        private void PlayOptifine_Click(object sender, RoutedEventArgs e)
        {
            Globals.MainNavigation.SelectedItem = Globals.MainNavigation.MenuItems[3];
            Globals.MainFrame.Navigate(typeof(OptiFinePage));
            NavigationService.UpdateBreadcrumb("OptiFine", true);
        }

        private void HomeHeader_Loaded(object sender, RoutedEventArgs e)
        {
            HomeHeader.SelectedPageIndex = Gallery.SelectedIndex;
        }
    }
}
