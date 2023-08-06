using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Core;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static MinecraftLauncherUniversal.Services.ThemeService.BackdropExtension;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages.SettingsPages.GeneralSettingsPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PersonalizationPage : Page
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
        public PersonalizationPage()
        {
            this.InitializeComponent();

            SoundToggle.IsOn = Globals.Settings.Sound;

            var _enumval = Enum.GetValues(typeof(Backdrop)).Cast<Backdrop>();
            BackdropCombo.ItemsSource = _enumval;

            var _themeenumval = Enum.GetValues(typeof(Theme)).Cast<Theme>();
            ThemesCombo.ItemsSource = _themeenumval;

            ThemesCombo.SelectedIndex = (int)Globals.Settings.Theme;

            InitFinished = true;
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
    }
}
