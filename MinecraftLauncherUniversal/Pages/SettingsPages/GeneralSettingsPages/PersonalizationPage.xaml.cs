using Microsoft.UI.Composition.SystemBackdrops;
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
        public PersonalizationPage()
        {
            this.InitializeComponent();
            if (Globals.Theme == 1)
            {
                ThemesCombo.SelectedItem = DarkItem;
            }
            else if (Globals.Theme == 0)
            {
                ThemesCombo.SelectedItem = LightItem;
            }

            if (Globals.SoundPlayerState == ElementSoundPlayerState.On)
            {
                SoundToggle.IsOn = true;
            }

            var _enumval = Enum.GetValues(typeof(Backdrop)).Cast<Backdrop>();
            BackdropCombo.ItemsSource = _enumval;

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
                Globals.SoundPlayerState = ElementSoundPlayerState.On;
                ElementSoundPlayer.State = ElementSoundPlayerState.On;
            }
            else
            {
                Globals.SoundPlayerState = ElementSoundPlayerState.Off;
                ElementSoundPlayer.State = ElementSoundPlayerState.Off;
            }
        }

        private void ThemesCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!InitFinished)
            {
                return;
            }
            if (ThemesCombo.SelectedIndex == 0)
            {
                ThemeService.ChangeTheme(ElementTheme.Dark);
                Globals.Theme = 1;
            }
            else
            {
                ThemeService.ChangeTheme(ElementTheme.Light);
                Globals.Theme = 0;
            }
        }

        private void BackdropCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ThemeService.BackdropExtension.SetBackdrop((ThemeService.BackdropExtension.Backdrop)BackdropCombo.SelectedItem);
        }
    }
}
