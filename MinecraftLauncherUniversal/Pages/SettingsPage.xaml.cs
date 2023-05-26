using MinecraftLauncherUniversal.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

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

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem SelectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            if (SelectedItem.Name == "DarkCombo")
            {
                Globals.Theme = 0;
                PersonalizationHelper h = new PersonalizationHelper(Window.Current);
                h.SetTheme();
            }
            else
            {
                Globals.Theme = 1;
                PersonalizationHelper h = new PersonalizationHelper(Window.Current);
                h.SetTheme();
            }
        }

        private void SoundToggle_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Globals.CurrentMainBreadcrumbDisplay.Remove("Settings");
            Globals.UpdateBreadcrumb();
        }

        private void PlayerSettings_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
