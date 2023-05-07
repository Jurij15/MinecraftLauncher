using MinecraftLauncher.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinecraftLauncher.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            UsernameSettingsBox.Text = Globals.Username;
            if (Globals.UserTheme == Wpf.Ui.Appearance.ThemeType.Light)
            {
                LightRadio.IsChecked = true;
            }
            else {
                DarkRadio.IsChecked = true;
            }
        }

        private void DarkRadio_Checked(object sender, RoutedEventArgs e)
        {
            Globals.UserTheme = Wpf.Ui.Appearance.ThemeType.Dark;
            Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark);
        }

        private void LightRadio_Checked(object sender, RoutedEventArgs e)
        {
            Globals.UserTheme = Wpf.Ui.Appearance.ThemeType.Light;
            Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light);
        }

        private void UsernameConfig_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            
        }

        private void UsernameSettingsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameSettingsBox.Text))
            {
                UsernameSettingsBox.Text = "Player";
            }

            Globals.Username = UsernameSettingsBox.Text;
            Settings.SaveNewUsername();
        }

        private void ResetSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            Settings.ResetSettings();
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }
    }
}
