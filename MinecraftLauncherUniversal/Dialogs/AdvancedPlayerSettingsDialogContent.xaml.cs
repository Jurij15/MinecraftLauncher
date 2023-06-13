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
using Windows.UI.Input.Inking.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Dialogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdvancedPlayerSettingsDialogContent : Page
    {
        public AdvancedPlayerSettingsDialogContent()
        {
            this.InitializeComponent();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CustomUUIDBox.Text) && !string.IsNullOrWhiteSpace(CustomUUIDBox.Text))
            {
                Globals.CustomUUID = CustomUUIDBox.Text;
            }
            if (!string.IsNullOrEmpty(AccessTokenBox.Text) && !string.IsNullOrWhiteSpace(AccessTokenBox.Text))
            {
                Globals.AccessToken = AccessTokenBox.Text;
            }
        }
    }
}
