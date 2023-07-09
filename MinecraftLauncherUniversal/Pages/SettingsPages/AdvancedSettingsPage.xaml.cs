using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages.SettingsPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdvancedSettingsPage : Page
    {
        public AdvancedSettingsPage()
        {
            this.InitializeComponent();
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
    }
}
