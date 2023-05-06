using CmlLib.Core;
using MinecraftLauncher.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace MinecraftLauncher.Pages
{
    /// <summary>
    /// Interaction logic for SpecialsPage.xaml
    /// </summary>
    public partial class SpecialsPage : Page
    {
        public SpecialsPage()
        {
            InitializeComponent();
            if (VersionsHelper.bIsVersionInstalled("OptiFine 1.17.1"))
            {
                PlayOptiFine1_17_1.Visibility = Visibility.Visible;
            }
            else
            {
                DownloadOptiFine1_17_1.Visibility = Visibility.Visible;
            }
        }

        private void DownloadOptiFine1_17_1_Click(object sender, RoutedEventArgs e)
        {
            //Download();
            string resName = "MinecraftLauncher.OptiFine.OptiFine.zip";

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resName))
            {
                if (stream == null)
                {
                    MessageBox.Show("Failed to load resource: " + resName);
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
            string extractPath = MinecraftPath.GetOSDefaultPath()+"/versions/";

            //Directory.CreateDirectory(extractPath);

            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string filePath = Path.Combine(extractPath, entry.FullName);

                    // Create the directory for the file if it doesn't exist
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    // Extract the file
                    entry.ExtractToFile(filePath, true);
                }
            }

            DownloadOptiFine1_17_1.Visibility = Visibility.Collapsed;
            PlayOptiFine1_17_1.Visibility = Visibility.Visible;
        }

        async void Download()
        {
            await OptifineDownloader.DownloadOptifine();
        }

        private void PlayOptiFine1_17_1_Click(object sender, RoutedEventArgs e)
        {
            string name = "OptiFine 1.17.1";
            Globals.CurrentBuild = name;

            Globals.PlayMenuItem.Content = "Play " + name;

            Globals.MainNavigation.NavigateWithHierarchy(typeof(PlayPage));
        }
    }
}
