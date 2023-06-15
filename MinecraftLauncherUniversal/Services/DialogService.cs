using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIEx.Messaging;
using MinecraftLauncherUniversal.Dialogs;

namespace MinecraftLauncherUniversal.Services
{
    public class DialogService
    {
        public static void ShowSimpleDialog(string title, string message)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = Globals.MainGridXamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = title;
            dialog.Content = message;

            dialog.CloseButtonText = "OK";
            dialog.CloseButtonClick += Dialog_CloseButtonClick;

            dialog.ShowAsync();
        }

        private static void Dialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            sender.Hide();
        }


        public static async void ShowWelcomeSetupDialog()
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = Globals.MainGridXamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            //dialog.Title = "Welcome";
            dialog.Content = new WelcomeSetupDialog(dialog);

            //dialog.CloseButtonText = "OK";
            //dialog.CloseButtonClick += Dialog_CloseButtonClick;

            await dialog.ShowAsync();
        }
    }
}
