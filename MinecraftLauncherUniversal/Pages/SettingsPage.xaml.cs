using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Core;
using MinecraftLauncherUniversal.Dialogs;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using static MinecraftLauncherUniversal.Services.ThemeService.BackdropExtension;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        bool InitFinished = false;
        public enum Theme
        {
            //
            // Summary:
            //     Use the Application.RequestedTheme value for the element. This is the default.
            Default,
            //
            // Summary:
            //     Use the **Light** default theme.
            Light,
            //
            // Summary:
            //     Use the **Dark** default theme.
            Dark
        }
        public SettingsPage()
        {
            this.InitializeComponent();


            SoundToggle.IsOn = Globals.Settings.Sound;
            PlayPageBackgroundToggle.IsOn = Globals.Settings.ShowImageBackgroundInPlayPage;

            var _enumval = Enum.GetValues(typeof(Backdrop)).Cast<Backdrop>();
            BackdropCombo.ItemsSource = _enumval;

            var _themeenumval = Enum.GetValues(typeof(Theme)).Cast<Theme>();
            ThemesCombo.ItemsSource = _themeenumval;

            ThemesCombo.SelectedIndex = (int)Globals.Settings.Theme;

            InitFinished = true;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(PlayerSettingsPage), "Player Settings", false);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void SoundToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (!InitFinished)
            {
                return;
            }
            if (SoundToggle.IsOn)
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.On;
                Globals.Settings.Sound = true;
            }
            else
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.Off;
                Globals.Settings.Sound = false;
            }

            SettingsJson.SaveSettings();
        }

        private void ThemesCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!InitFinished)
            {
                return;
            }
            ElementTheme theme = (ElementTheme)((ComboBox)sender).SelectedItem;

            ThemeService.ChangeTheme(theme);
            Globals.Settings.Theme = theme;

            SettingsJson.SaveSettings();
        }

        private void BackdropCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ThemeService.BackdropExtension.SetBackdrop((ThemeService.BackdropExtension.Backdrop)BackdropCombo.SelectedItem);
        }


        private void DownloadRateLimitCard_Loaded(object sender, RoutedEventArgs e)
        {
            DownloadConnectionLimitBox.Value = Globals.DownloadRateLimit;
        }

        private void DownloadConnectionLimitBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            Globals.DownloadRateLimit = Convert.ToInt32(args.NewValue);
        }


        private void ToggleConsoleBtn_Checked(object sender, RoutedEventArgs e)
        {
            Globals.SetupConsole();
            Globals.bShowConsole = true;
        }

        private void ToggleConsoleBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            Globals.CloseConsole();
            Globals.bShowConsole = false;
        }

        private void ResetAppBtn_Click(object sender, RoutedEventArgs e)
        {
            Globals.ResetApp(true);
        }

        private void ToggleConsoleBtn_Loaded(object sender, RoutedEventArgs e)
        {
            if (Globals.bShowConsole)
            {
                ((ToggleButton)sender).IsChecked = true;
            }
            else
            {
                ((ToggleButton)sender).IsChecked = false;
            }
        }

        private void PlayPageBackgroundCard_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void PlayPageBackgroundToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (!InitFinished)
            {
                return;
            }
            if (PlayPageBackgroundToggle.IsOn)
            {
                Globals.Settings.ShowImageBackgroundInPlayPage = true;
            }
            else
            {
                Globals.Settings.ShowImageBackgroundInPlayPage = false;
            }

            SettingsJson.SaveSettings();
        }
    }
}
