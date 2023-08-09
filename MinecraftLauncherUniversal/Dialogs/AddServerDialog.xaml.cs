using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Core;
using MinecraftLauncherUniversal.Managers;
using CommunityToolkit.WinUI.Helpers;
using MinecraftLauncherUniversal.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Dialogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddServerDialog : Page
    {
        string _serverName;
        string _serverIP;
        string _serverVersion;
        string _serverPort;

        ContentDialog _dialog;
        public AddServerDialog(ContentDialog PresenterDialog)
        {
            this.InitializeComponent();

            PresenterDialog.PrimaryButtonClick += PresenterDialog_PrimaryButtonClick;

            PresenterDialog.PrimaryButtonText = "Add";
            PresenterDialog.CloseButtonText = "Cancel";

            PresenterDialog.DefaultButton = ContentDialogButton.Primary;

            _dialog = PresenterDialog;  

            foreach (var item in VersionManager.AllVersionsGlobal)
            {
                if (VersionsHelper.bIsReleaseVersion(item))
                {
                    VersionsBox.Items.Add(item);
                }
            }
        }

        private async void PresenterDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ServerJson json = new ServerJson();

            json.ServerName = _serverName;
            json.ServerIP = _serverIP;
            json.ServerVersion = _serverVersion;
            json.ServerPort = _serverPort;

            //do if checks

            _dialog.PrimaryButtonText = "Loading...";

            ServersManager manager = new ServersManager();
            await manager.AddServer(json);

            sender.Hide();
        }

        private void VersionsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _serverVersion = (string)VersionsBox.SelectedItem;
        }
    }
}
