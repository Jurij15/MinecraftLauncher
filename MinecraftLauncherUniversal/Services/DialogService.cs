using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIEx.Messaging;
using MinecraftLauncherUniversal.Dialogs;
using Serilog.Core;
using Serilog;

namespace MinecraftLauncherUniversal.Services
{
    public class DialogService
    {
        public static async void ShowSimpleDialog(string title, string message)
        {
            Log.Verbose($"Showing a contentdialog with title: {title}, and message: {message}");
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = Globals.MainGridXamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = title;
            dialog.Content = message;

            dialog.CloseButtonText = "OK";
            dialog.CloseButtonClick += Dialog_CloseButtonClick;

            await dialog.ShowAsync();
        }

        public static async void C_ShowSimpleDialog(string message, string title = "")
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = Globals.MainGridXamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = title;
            dialog.Content = message;

            dialog.CloseButtonText = "OK";
            dialog.CloseButtonClick += Dialog_CloseButtonClick;

            await dialog.ShowAsync();
        }

        private static void Dialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            sender.Hide();
        }


        public static async void ShowWelcomeSetupDialog()
        {
            Log.Verbose($"Showing the welcome dialog");
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = Globals.MainGridXamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            //dialog.Title = "Welcome";
            dialog.Content = new WelcomeSetupDialog(dialog);

            //dialog.CloseButtonText = "OK";
            //dialog.CloseButtonClick += Dialog_CloseButtonClick;

            await dialog.ShowAsync();
        }

        public static ContentDialog CreateContentDialog(string Title, object Content)
        {
            Log.Verbose($"Creating a contentdialog with title: {Title}");
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = Globals.MainGridXamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = Title;
            dialog.Content = Content;

            return dialog;
        }
    }
}
