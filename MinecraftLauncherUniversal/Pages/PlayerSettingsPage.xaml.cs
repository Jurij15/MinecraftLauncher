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
    public sealed partial class PlayerSettingsPage : Page
    {
        public PlayerSettingsPage()
        {
            this.InitializeComponent();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Globals.CurrentMainBreadcrumbDisplay.Remove("Player Settings");
            Globals.UpdateBreadcrumb();

            if (string.IsNullOrWhiteSpace(UsernameEditBox.Text))
            {
                return;
            }
            else if (string.IsNullOrEmpty(UsernameEditBox.Text))
            {
                return;
            }
            Globals.Username = UsernameEditBox.Text;
        }

        private void UsernameEditBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameEditBox.Text))
            {
                return;
            }
            else if (string.IsNullOrEmpty(UsernameEditBox.Text))
            {
                return;
            }
            Globals.Username = UsernameEditBox.Text;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UsernameEditBox.Text = Globals.Username;
        }
    }
}
