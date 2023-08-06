using CmlLib.Core;
using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.WindowsAppSDK.Runtime;
using MinecraftLauncherUniversal.Core;
using MinecraftLauncherUniversal.Dialogs;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.NetworkOperators;
using Windows.Perception.Spatial.Preview;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayerSettingsPage : Page
    {
        string _id;
        bool bUnloading = false;
        bool bRestartingAfterAccDeleted = false;
        string GetCurrentID()
        {
            return _id;
        }
        public PlayerSettingsPage()
        {
            this.InitializeComponent();

            UsernameSettingsBox.Text = Globals.Settings.Username;
        }

        private void UsernameSettingsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Globals.Settings.Username = UsernameSettingsBox.Text;
            SettingsJson.SaveSettings();
        }

        private async void AdvancedDialogBtn_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush transparentBrush = new SolidColorBrush(Colors.Transparent);

            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = Globals.MainGridXamlRoot;
            dialog.Title = "Advanced Settings";
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Content = new AdvancedPlayerSettingsDialogContent(dialog);
            //dialog.Background = transparentBrush;

            dialog.CloseButtonText = "Exit";

            dialog.PrimaryButtonText = "Save";

            dialog.DefaultButton = ContentDialogButton.Close;

            await dialog.ShowAsync();
        }
    }
}
