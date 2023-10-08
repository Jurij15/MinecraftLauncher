using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using CommunityToolkit;
using WinUIEx.Messaging;
using MinecraftLauncherUniversal.Interop;
using MinecraftLauncherUniversal.Controls;
using System.Threading;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Media.Animation;
using System.Threading.Tasks;
using MinecraftLauncherUniversal.Services;
using CmlLib.Core.Installer.Forge;
using CmlLib.Core.Installer.Forge.Versions;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ForgePage : Page
    {
        // from https://github.com/CmlLib/CmlLib.Core.Installer.Forge/blob/main/SampleForgeInstaller/AllInstaller.cs
        string[] versions = new string[]
        {
            "1.20.1", // ok
            "1.20", // ok
            "1.19.4", // ok
            "1.19.3", // ok
            "1.19.2", //ok
            "1.19.1", //ok
            "1.19", //ok
            "1.18.2", // ok
            "1.18.1", // ok
            "1.18", // ok
            "1.17.1", // ok
            "1.16.5", // ok
            "1.16.4", // ok
            "1.16.3", // ok
            "1.16.2", // ok
            "1.16.1", // ok
            "1.15.2", // ok
            "1.15.1", // ok
            "1.15", // ok
            "1.14.4", // ok
            "1.14.3", // ok
            "1.14.2", // ok
            "1.13.2", // ok
            "1.12.2", // ok
            "1.12.1", // ok
            "1.12", // ok
            "1.11.2", // ok
            "1.11", // ok
            "1.10.2", // ok
            "1.10", // ok
            "1.9.4", // ok
            "1.9", // ok
            "1.8.9", // ok
            "1.8.8", // ok
            "1.7.10", // ok
                      //"1.7.10_pre4", // cannot find 1.7.10_pre4
                      //"1.7.2", // crash
                      //"1.6.4", // crash
                      //"1.6.3", // crash
                      //"1.6.2", // crash
                      //"1.6.1", // crash, wrong version name
                      //"1.5.2" // crash
        };

        static ForgeVersionCardControl _storedCard;
        public ForgePage()
        {
            this.InitializeComponent();

            //load all forge versions
            //ForgeVersionLoader loader = new ForgeVersionLoader(new System.Net.Http.HttpClient());

            this.DataContext = this;
        }

        string PickedForgeVersion = null;
        string PickedMinecraftVersion = null;

        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PickedMinecraftVersion = ((ComboBox)sender).SelectedItem.ToString();

            ForgeLoadingRing.Visibility = Visibility.Visible;
            ForgeVersionLoader loader = new ForgeVersionLoader(new System.Net.Http.HttpClient());
            var versions = await loader.GetForgeVersions(PickedMinecraftVersion);
            Thread.Sleep(100);
            await Task.Delay(200);
            ForgeVersionsBox.Items.Clear();
            ForgeVersionsBox.SelectedItem = null;
            foreach (var item in versions)
            {
                ForgeVersionsBox.Items.Add(item.ForgeVersionName);
            }

            MinecraftVersionCard.IsEnabled = false;
            ForgeVersionCard.IsEnabled = true;
            PlayCard.IsEnabled = false;

            ForgeLoadingRing.Visibility = Visibility.Collapsed;
        }

        private void ForgeVersionGoBack_Click(object sender, RoutedEventArgs e)
        {
            MinecraftVersionCard.IsEnabled = true;
            ForgeVersionCard.IsEnabled = false;
            PlayCard.IsEnabled = false;
            ForgeVersionsBox.SelectedItem = null;

            ForgeVersionsBox.Items.Clear();
        }

        private void ForgeVersionsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedItem != null)
            {
                PickedForgeVersion = ((ComboBox)sender).SelectedItem.ToString();
            }

            ForgeVersionCard.IsEnabled = false;
            PlayCard.IsEnabled = true;

            PlayCard.Header = "Forge "+PickedMinecraftVersion;
            PlayCard.Description = PickedForgeVersion;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardForgeConnectedAnimation", PlayCard);
            animation.Configuration = new DirectConnectedAnimationConfiguration();

            var imgbackanim = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("forwardForgeImageAnim", PlayCard.HeaderIcon);
            imgbackanim.Configuration = new DirectConnectedAnimationConfiguration();

            PlayVersionPage.IsPlayingForge = true;
            PlayVersionPage.ForgeVersion = PickedForgeVersion;
            Globals.CurrentVersion = PickedMinecraftVersion;
            NavigationService.NavigateSuppressedAnim(typeof(PlayVersionPage), "Play Forge" + PickedMinecraftVersion, false, false);
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            //reset everything
            PlayCard.IsEnabled = false;
            ForgeVersionCard.IsEnabled = false;

            MinecraftVersionCard.IsEnabled = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Retrieve and start the connected animation
            var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackForgeAnim");
            if (animation != null)
            {
                animation.TryStart(PlayCard, new UIElement[] { });
            }
            var imganim = ConnectedAnimationService.GetForCurrentView().GetAnimation("ImgForgeBackAnim");
            if (imganim != null)
            {
                imganim.TryStart(PlayCard.HeaderIcon);
            }

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }
    }
}
