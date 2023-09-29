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
using System.Threading;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WikiPage : Page
    {
        public WikiPage()
        {
            this.InitializeComponent();
        }

        private void RefreshToolbarBtn_Click(object sender, RoutedEventArgs e)
        {
            WebView.CoreWebView2.Reload();
        }

        private async void WebView_Loaded(object sender, RoutedEventArgs e)
        {
            await WebView.EnsureCoreWebView2Async();
            WebView.CoreWebView2.Navigate("https://minecraft.wiki/");
        }

        private void HomeToolbarBtn_Click(object sender, RoutedEventArgs e)
        {
            WebView.CoreWebView2.Navigate("https://minecraft.wiki/");
        }

        private void WebView_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            UrlBox.Text = WebView.Source.ToString();

            if (!WebView.CoreWebView2.CanGoBack)
            {
                BackToolbarBtn.IsEnabled = false;
            }
            else
            {
                BackToolbarBtn.IsEnabled = true;
            }

            if (!WebView.CoreWebView2.CanGoForward)
            {
                ForwardToolbarBtn.IsEnabled = false;
            }
            else
            {
                ForwardToolbarBtn.IsEnabled = true;
            }
        }

        private void UrlBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            UrlBox.Text = WebView.Source.ToString();
        }

        private void UrlBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var package = new DataPackage();
            package.SetText(WebView.Source.ToString());
            Clipboard.SetContent(package);

            TeachingTip tip = new TeachingTip();
            tip.Title = "Link copied to clipboard!";
            tip.Target = UrlBox;
            tip.PreferredPlacement = TeachingTipPlacementMode.Bottom;
            //tip.IsOpen =true; //this crashes it for some reason
        }

        private void BackToolbarBtn_Click(object sender, RoutedEventArgs e)
        {
            if (WebView.CoreWebView2.CanGoBack)
            {
                WebView.CoreWebView2.GoBack();
            }
        }

        private void ForwardToolbarBtn_Click(object sender, RoutedEventArgs e)
        {
            if (WebView.CoreWebView2.CanGoForward)
            {
                WebView.CoreWebView2.GoForward();
            }
        }
    }
}
