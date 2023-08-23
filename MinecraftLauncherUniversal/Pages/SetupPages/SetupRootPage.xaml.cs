using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Pages.SetupPages.ContentPages;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml.Media.Animation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages.SetupPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SetupRootPage : Page
    {
        public static int AuthType = 0;

        SlideNavigationTransitionInfo transitionInfo;
        public SetupRootPage()
        {
            this.InitializeComponent();

            ThemeService.BackdropExtension.SetBackdrop(ThemeService.BackdropExtension.Backdrop.MicaAlt);
            MainWindow.TitleBarPaneToggleButton.Visibility = Visibility.Collapsed;
            
            transitionInfo = new SlideNavigationTransitionInfo();
            transitionInfo.Effect = SlideNavigationTransitionEffect.FromRight;

            SetupPagesFrame.Navigate(typeof(AuthPage), null, transitionInfo);

            PageStatusBox.Text = $"Page 1 of 4";
        }

        //this is NOT a good way of navigation, i will improve it at some point later
        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SetupPagesFrame.CurrentSourcePageType == typeof(AuthPage))
            {
                if (AuthType == 0)
                {
                    SetupPagesFrame.Navigate(typeof(OfflineAuthPage), null, transitionInfo);
                    PageStatusBox.Text = $"Page 2 of 4";
                }
            }
            else if (SetupPagesFrame.CurrentSourcePageType == typeof(OfflineAuthPage))
            {
                SetupPagesFrame.Navigate(typeof(GameSettingsPage), null, transitionInfo);
                PageStatusBox.Text = $"Page 3 of 4";
            }
            else if (SetupPagesFrame.CurrentSourcePageType == typeof(GameSettingsPage))
            {
                SetupPagesFrame.Navigate(typeof(LauncherSettingsPage), null, transitionInfo);
                PageStatusBox.Text = $"Page 4 of 4";

                ((Button)sender).Content = "Finish and restart";
            }
            else
            {
                Globals.RestartApp();
            }
            
        }
    }
}
