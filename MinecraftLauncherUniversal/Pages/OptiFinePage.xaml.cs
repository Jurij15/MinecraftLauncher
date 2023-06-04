using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.WindowsAppSDK.Runtime;
using MinecraftLauncherUniversal.Helpers;
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

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OptiFinePage : Page
    {
        public OptiFinePage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (VersionsHelper.bIsVersionInstalled("OptiFine 1.17.1"))
            {
                //version is installed
                Play.Visibility = Visibility.Visible;
                InstallBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                Play.Visibility = Visibility.Collapsed;
                InstallBtn.Visibility = Visibility.Visible;
            }
        }

        private void InstallBtn_Click(object sender, RoutedEventArgs e)
        {
            OptifineDownloader.DownloadOptifine();
            if (VersionsHelper.bIsVersionInstalled("OptiFine 1.17.1"))
            {
                //version is installed
                Play.Visibility = Visibility.Visible;
                InstallBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                Play.Visibility = Visibility.Collapsed;
                InstallBtn.Visibility = Visibility.Visible;
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            string version = "OptiFine 1.17.1";
            Globals.CurrentVersion = version;

            NavigationService.NavigateHiearchical(typeof(SelectedVersionPage), "Play " + version, false);
        }
    }
}
