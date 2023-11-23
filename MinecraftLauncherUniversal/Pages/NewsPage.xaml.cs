using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewsPage : Page
    {
        public NewsPage()
        {
            this.InitializeComponent();

            if (Updater.bIsPrerelease())
            {
                PreReleaseVersionInfoBar.IsOpen = true;
            }
        }

        private void PathchNotesBox_Loaded(object sender, RoutedEventArgs e)
        {
            //await Task.Delay(200);
            PathchNotesBox.AllowFocusOnInteraction = true;
            PathchNotesBox.IsReadOnly = false;
            PathchNotesBox.Document.SetText(Microsoft.UI.Text.TextSetOptions.FormatRtf, File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "UpdatePatchNotes.rtf")));
            PathchNotesBox.AllowFocusOnInteraction = false;
            PathchNotesBox.IsReadOnly = true;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
