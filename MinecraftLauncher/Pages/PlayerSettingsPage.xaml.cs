using MinecraftLauncher.Helpers;
using System.Windows;
using System.Windows.Controls;

namespace MinecraftLauncher.Pages
{
    /// <summary>
    /// Interaction logic for PlayerSettingsPage.xaml
    /// </summary>
    public partial class PlayerSettingsPage : Page
    {
        public PlayerSettingsPage()
        {
            InitializeComponent();

            UsernameSettingsBox.Text = Globals.Username;
        }

        private void UsernameSettingsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameSettingsBox.Text))
            {
                return;
            }
            else
            {
                Globals.Username = UsernameSettingsBox.Text;
                Settings.SaveNewUsername();
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            UsernameSettingsBox_TextChanged(null, null);
        }
    }
}
