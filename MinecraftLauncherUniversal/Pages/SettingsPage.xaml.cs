using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            if (Globals.SoundPlayerState == ElementSoundPlayerState.On)
            {
                SoundToggle.IsOn = true;
            }
            else
            {
                SoundToggle.IsOn = false;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DownloadConnectionLimitBox.Value = Globals.DownloadRateLimit;
        }

        private void TempWarning_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void TempTestStop_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void SoundToggle_Toggled(object sender, RoutedEventArgs e)
        {
            bool state = ((ToggleSwitch)sender).IsOn;
            if (state == true)
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.On;
                Globals.SoundPlayerState = ElementSoundPlayerState.On;
            }
            else
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.Off;
                Globals.SoundPlayerState = ElementSoundPlayerState.Off;
            }
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            // Save theme choice to LocalSettings. 
            // ApplicationTheme enum values: 0 = Light, 1 = Dark
            bool toggleswitchstate = ((ToggleSwitch)sender).IsOn;

            Globals.Theme = Convert.ToInt32(toggleswitchstate);

            Helpers.Settings.SaveNewTheme();

            ThemeConfigAppRestartMessage.Visibility = Visibility.Visible;
        }

        private void ToggleSwitch_Loaded(object sender, RoutedEventArgs e)
        {
            ((ToggleSwitch)sender).IsOn = Convert.ToBoolean(Globals.Theme);
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateHiearchical(typeof(PlayerSettingsPage), "Player Settings", false);
        }

        private void DownloadConnectionLimitBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            Globals.DownloadRateLimit = Convert.ToInt32(args.NewValue);
        }

        private void DownloadRateLimitCard_Loaded(object sender, RoutedEventArgs e)
        {
            DownloadConnectionLimitBox.Value = Globals.DownloadRateLimit;
        }

        private void BackdropCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BackdropManager manager = new BackdropManager();
            string content = ((ComboBoxItem)sender).Content.ToString();
            if (content == "Mica")
            {
                manager.ApplyMicaKind(Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base);
            }
            else if(content == "MicaAlt")
            {
                manager.ApplyMicaKind(Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt);
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Globals.DownloadRateLimit = Convert.ToInt32(DownloadConnectionLimitBox.Value);
        }
    }
}
