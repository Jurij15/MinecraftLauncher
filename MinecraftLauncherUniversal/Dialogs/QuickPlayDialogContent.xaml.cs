using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.WindowsAppSDK.Runtime;
using MinecraftLauncherUniversal.Core;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Dialogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuickPlayDialogContent : Page
    {
        ContentDialog dialog = new ContentDialog();
        bool bLaunch =false;
        public QuickPlayDialogContent(ContentDialog sender)
        {
            this.InitializeComponent();
            sender.PrimaryButtonClick += Sender_PrimaryButtonClick;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UsernameBox.Text = Globals.Username;
            if (VersionsHelper.bVersionSupportsSkins(Globals.CurrentVersion))
            {
                ShowSkinCheck.IsChecked = true;
            }
        }

        private async void Sender_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            bLaunch = true;
            dialog.Content = new SimpleLoadingDialog("Launching...");
            dialog.XamlRoot = Globals.MainGridXamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            sender.Hide();
            await dialog.ShowAsync();
        }

        private async void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (bLaunch)
            {
                //the launching procedure
                int memooryinmb = Convert.ToInt32(RamAmountBox.Value) * 1024;

                PlayCore core = new PlayCore(Globals.CurrentVersion, memooryinmb, Convert.ToBoolean(FullscreenCheck.IsChecked), Globals.CustomUUID, Globals.AccessToken);
                bool result = await core.Launch();
                dialog.Hide();
                if (!result) { DialogService.ShowSimpleDialog("An Error Occured", core.GetLaunchErrors()); }
            }
        }
    }
}
