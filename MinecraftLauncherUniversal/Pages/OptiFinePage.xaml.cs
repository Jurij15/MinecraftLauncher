using CmlLib.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.WindowsAppSDK.Runtime;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (VersionsHelper.bIsVersionInstalled("OptiFine 1.17.1"))
            {
                //version is installed
                Play.Visibility = Visibility.Visible;
                InstallBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                Play.Visibility = Visibility.Collapsed;
                InstallBtn.Visibility = Visibility.Visible;
            }
        }

        private void InstallBtn_Click(object sender, RoutedEventArgs e)
        {
            InstallOptifine();
            if (VersionsHelper.bIsVersionInstalled("OptiFine 1.17.1"))
            {
                //version is installed
                Play.Visibility = Visibility.Visible;
                InstallBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                Play.Visibility = Visibility.Collapsed;
                InstallBtn.Visibility = Visibility.Visible;
            }
        }

        async void InstallOptifine()
        {
            string resName = "MinecraftLauncherUniversal.OptiFine.OptiFine.zip";

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resName))
            {
                if (stream == null)
                {
                    //MessageBox.Show("Failed to load resource: " + resName);
                    var messagedialog = new MessageDialog("Failed to load resource: " + resName);
                    await messagedialog.ShowAsync();
                    return;
                }

                // Copy the stream to a file
                using (FileStream file = new FileStream("Settings/OptiFine.zip", FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    file.Write(buffer, 0, buffer.Length);
                }
            }

            string zipPath = "Settings/OptiFine.zip";
            string extractPath = MinecraftPath.GetOSDefaultPath() + "/versions";

            //Directory.CreateDirectory(extractPath);
            /*
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string filePath = Path.Combine(extractPath, entry.FullName);

                    // Create the directory for the file if it doesn't exist
                    //Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    // Extract the file
                    ZipFile.ExtractToDirectory(filePath, extractPath);
                }
            }
            */
            ZipFile.ExtractToDirectory(zipPath, extractPath);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            string version = "OptiFine 1.17.1";
            Globals.CurrentVersion = version;

            NavigationService.NavigateHiearchical(typeof(SelectedVersionPage), "Play " + version, false);
        }
    }
}
