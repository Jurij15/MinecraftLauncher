using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
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
        public static void SetTheme(ElementTheme theme)
        {
            FrameworkElement element = Window.Current.Content as FrameworkElement;
            element.RequestedTheme = theme;
        }
    }
}
