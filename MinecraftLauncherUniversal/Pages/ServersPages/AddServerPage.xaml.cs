using CommunityToolkit.WinUI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Pages.ServersPages.AddServerPages;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages.ServersPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddServerPage : Page
    {
        public AddServerPage()
        {
            this.InitializeComponent();
        }

        private void AddServerPageRootFrame_Loaded(object sender, RoutedEventArgs e)
        {
            AddServerPageRootFrame.Navigate(typeof(ImportServerPage), null, new SuppressNavigationTransitionInfo());
        }

        private async void Segmented_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null && AddServerPageRootFrame != null)
            {
                int SelectedIndex = (sender as Segmented).SelectedIndex;
                if (SelectedIndex == 0)
                {
                    AddServerPageRootFrame.Navigate(typeof(ImportServerPage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
                }
                else
                {
                    //AddServerPageRootFrame.Navigate(typeof(CreateServerPage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
                    ContentDialog dialog = new ContentDialog();
                    dialog.XamlRoot = Globals.MainGridXamlRoot;
                    dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;

                    dialog.Title = "Add Server";
                    dialog.Content = new Dialogs.AddServerDialog(dialog);

                    dialog.Closed += Dialog_Closed;

                    await dialog.ShowAsync();
                }
            }
        }

        private void Dialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            if (args.Result == ContentDialogResult.None)
            {
                SegmentedControl.SelectedIndex = 0;
            }
            else
            {
                NavigationService.Navigate(typeof(ServersPage), "Servers", true);
            }
        }
    }
}
