using MinecraftLauncherUniversal.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MinecraftLauncherUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OptiFinePage : Page
    {
        public OptiFinePage()
        {
            this.InitializeComponent();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Globals.CurrentMainBreadcrumbDisplay.Remove("OptiFine");
            Globals.UpdateBreadcrumb();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //check if optifine 1.17.1 is installed
            InstallBtn.Content = VersionsHelper.bIsVersionInstalled("OptiFine 1.17.1").ToString(); //this does not work due to "limitations": every uwp app is sandboxed and access to folders like .roaming is denied by default
        }
    }
}
