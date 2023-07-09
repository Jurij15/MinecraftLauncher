using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Pages.SettingsPages.GeneralSettingsPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static MinecraftLauncherUniversal.Services.NavigationService;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages.SettingsPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GeneralSettingsPageIndex : Page
    {
        public GeneralSettingsPageIndex()
        {
            this.InitializeComponent();
        }

        private void PersonalizationCard_Click(object sender, RoutedEventArgs e)
        {
            Navigate(typeof(PersonalizationPage), "Personalization", false);
        }

        private void DownloadRateLimitCard_Loaded(object sender, RoutedEventArgs e)
        {
            DownloadConnectionLimitBox.Value = Globals.DownloadRateLimit;
        }

        private void DownloadConnectionLimitBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            Globals.DownloadRateLimit = Convert.ToInt32(args.NewValue);
        }
    }
}
