using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using MinecraftLauncherUniversal.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ICSharpCode.SharpZipLib.Zip.ZipEntryFactory;

namespace MinecraftLauncherUniversal.Services
{
    public class ThemeService
    {
        public static void ChangeTheme(ElementTheme theme)
        {
            if (Globals.m_window.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = theme;
            }
        }

        public static bool IsDarkTheme()
        {
            bool RetVal = false;

            if (Globals.m_window.Content is FrameworkElement frameworkElement)
            {
                if (frameworkElement.RequestedTheme == ElementTheme.Dark)
                {
                    RetVal = true;
                }
            }

            return RetVal;
        }

        public static bool IsLightTheme()
        {
            bool RetVal = false;

            if (Globals.m_window.Content is FrameworkElement frameworkElement)
            {
                if (frameworkElement.RequestedTheme == ElementTheme.Light)
                {
                    RetVal = true;
                }
            }

            return RetVal;
        }

        public static class BackdropExtension
        {
            public static Window MainWindow = Globals.m_window;
            public enum Backdrop
            {
                Mica,
                MicaAlt,
                Acrylic,
                None,
            }
            public static void SetBackdrop(Backdrop Backdrop)
            {
                switch (Backdrop)
                {
                    case Backdrop.Mica:
                        MainWindow.SystemBackdrop = null;
                        (MainWindow.Content as Grid).Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
                        MicaBackdrop micaBackdrop = new MicaBackdrop();
                        micaBackdrop.Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base;

                        MainWindow.SystemBackdrop = micaBackdrop;
                    break;
                    case Backdrop.MicaAlt:
                        MainWindow.SystemBackdrop = null;
                        (MainWindow.Content as Grid).Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
                        MicaBackdrop micaaltBackdrop = new MicaBackdrop();
                        micaaltBackdrop.Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt;

                        MainWindow.SystemBackdrop = micaaltBackdrop;
                    break;

                    case Backdrop.Acrylic:
                        MainWindow.SystemBackdrop = null;
                        (MainWindow.Content as Grid).Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
                        DesktopAcrylicBackdrop backdrop = new DesktopAcrylicBackdrop();

                        MainWindow.SystemBackdrop = backdrop;
                    break;

                    case Backdrop.None:
                        MainWindow.SystemBackdrop = null;
                        (MainWindow.Content as Grid).Background = App.Current.Resources["ApplicationPageBackgroundThemeBrush"] as SolidColorBrush;
                    break;
                }
            }
        }
    }
}
