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
using CommunityToolkit.Labs.WinUI.SettingsControlsRns;
using CommunityToolkit.Labs.WinUI;
using WinUIEx.Messaging;
using MinecraftLauncherUniversal.Interop;
using MinecraftLauncherUniversal.Controls;
using System.Threading;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Media.Animation;
using System.Threading.Tasks;
using MinecraftLauncherUniversal.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ForgePage : Page
    {
        static ForgeVersionCardControl _storedCard;
        static List<string> mcversions = new List<string>();
        //i just quickly generated this with ai
        static bool IsVersionHigherOrEqual(string version1, string version2)
        {
            // Split the versions and parse the first two segments
            string[] version1Segments = version1.Split('.');
            string[] version2Segments = version2.Split('.');

            int majorVersion1 = int.Parse(version1Segments[0]);
            int minorVersion1 = int.Parse(version1Segments[1]);

            int majorVersion2 = int.Parse(version2Segments[0]);
            int minorVersion2 = int.Parse(version2Segments[1]);

            // Compare the first two segments
            if (majorVersion1 > majorVersion2 || (majorVersion1 == majorVersion2 && minorVersion1 >= minorVersion2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public ForgePage()
        {
            this.InitializeComponent();

            //load all forge versions
            //ForgeVersionLoader loader = new ForgeVersionLoader(new System.Net.Http.HttpClient());

            foreach (var item in VersionManager.AllVersionsGlobal)
            {
                if (VersionsHelper.bIsReleaseVersion(item))
                {
                    if (IsVersionHigherOrEqual(item, "1.7.10"))
                    {
                        mcversions.Add(item);
                    }
                }
            }
        }

        void LoadVersions()
        {
            ForgeVersionsPanel.Items.Clear();
            foreach (var item in mcversions)
            {
                ForgeVersionCardControl card = new ForgeVersionCardControl();
                card.Version = "Forge " + item;

                //card.Margin = new Thickness(4);

                if (VersionsHelper.bIsVersionInstalled(item))
                {
                    card.VersionInstalledState = "Installed";
                }
                else
                {
                    card.VersionInstalledState = "Not Installed";
                }

                card.Width = 210;

                card.PointerReleased += Card_PointerReleased;

                ForgeVersionsPanel.Items.Add(card);
            }
        }

        private async void ForgeVersionsPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (_storedCard != null)
            {
                ForgeVersionsPanel.ScrollIntoView(_storedCard, ScrollIntoViewAlignment.Leading);
                ForgeVersionsPanel.UpdateLayout();
                try
                {
                    // Retrieve and start the connected animation for back navigation
                    //MessageBox.Show("test");
                    var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackForgeAnim");
                    if (animation != null)
                    {
                        bool result = animation.TryStart(_storedCard);
                    }

                    var imganimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ImgForgeBackAnim");
                    if (imganimation != null)
                    {
                        bool result = imganimation.TryStart(_storedCard.MinecraftImage);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
            await Task.Delay(100); //delay it so that everything loads
            LoadVersions();
        }

        private void Card_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ForgeVersionCardControl card = sender as ForgeVersionCardControl;
            if (card != null)
            {
                //card.MinecraftImage.Visibility = Visibility.Collapsed;

                string version = card.Version;

                Globals.CurrentVersion = version;
                _storedCard = card;

                var imageanim = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("forwardForgeImageAnim", _storedCard.MinecraftImage);
                var animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardForgeConnectedAnimation", _storedCard);

                //idk if this is better or not
                imageanim.Configuration = new BasicConnectedAnimationConfiguration();
                animation.Configuration = new BasicConnectedAnimationConfiguration();

                PlayVersionPage.IsPlayingForge = true;
                NavigationService.NavigateSuppressedAnim(typeof(PlayVersionPage), "Play " + version, false, false);
            }
        }
    }
}
