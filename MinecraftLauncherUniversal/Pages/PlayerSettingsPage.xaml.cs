using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Perception.Spatial.Preview;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayerSettingsPage : Page
    {
        public PlayerSettingsPage()
        {
            this.InitializeComponent();

            UsernameSettingsBox.Text = Globals.Username;
        }

        private void UsernameSettingsBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(UsernameSettingsBox.Text) || !string.IsNullOrWhiteSpace(UsernameSettingsBox.Text))
            {
                Globals.Username = UsernameSettingsBox.Text;
                Settings.SaveNewUsername();
            }
        }
    }
}
