using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Dialogs;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Pages.SettingsPages;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

            if (!toggleswitchstate)
            {
                //ThemeService.SetTheme(ApplicationTheme.Light);
            }
            else
            {
                //ThemeService.SetTheme(ApplicationTheme.Dark);
            }

            Globals.Theme = Convert.ToInt32(toggleswitchstate);

            Helpers.Settings.SaveNewTheme();

            //ThemeConfigAppRestartMessage.Visibility = Visibility.Visible;
        }

        private void ToggleSwitch_Loaded(object sender, RoutedEventArgs e)
        {
            ((ToggleSwitch)sender).IsOn = Convert.ToBoolean(Globals.Theme);
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(PlayerSettingsPage), "Player Settings", false);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private async void ResetAppBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = Globals.MainGridXamlRoot;
            dialog.Title = "Reset";
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Content = "Resetting will delete all saved configurations. A restart will be required.";
            //dialog.Background = transparentBrush;

            dialog.CloseButtonText = "Cancel";
            dialog.CloseButtonClick += Dialog_CloseButtonClick; ;

            dialog.PrimaryButtonText = "Reset and restart";
            dialog.PrimaryButtonClick += Dialog_PrimaryButtonClick;

            dialog.DefaultButton = ContentDialogButton.Close;

            await dialog.ShowAsync();
        }

        private void Dialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            sender.Hide();
        }

        private void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Globals.ResetApp(true);
        }

        private void PersonalizeNavigationCard_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(GeneralSettingsPageIndex), "General", false);
        }

        private void AdvancedNavigationCard_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(AdvancedSettingsPage), "Advanced", false);
        }
    }
}
