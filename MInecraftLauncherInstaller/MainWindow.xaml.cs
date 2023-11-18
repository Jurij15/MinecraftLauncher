using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherSharedResources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MInecraftLauncherInstaller
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WinUIEx.WindowEx
    {
        string LauncherVersion = "1.0-DEV";
        void UpdateProgressBar(bool Indeterminate)
        {
            InstallProgress.IsIndeterminate = Indeterminate;
        }
        void UpdateProgressBar(int Progress, int maximum = 100)
        {
            InstallProgress.IsIndeterminate = false;
            InstallProgress.Maximum = maximum;
            InstallProgress.Value = Progress;
        }
        void UpdateStatusHeader(string Header)
        {
            InstallProgressHeader.Header = Header;
        }
        public MainWindow()
        {
            this.InitializeComponent();

            this.Title = "MinecraftLauncher - Installer";

            AppWindow.Resize(new Windows.Graphics.SizeInt32(650, 440));
            AppWindow.IsShownInSwitchers = false;

            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(AppTitleBar);

            AppWindow.TitleBar.IconShowOptions = Microsoft.UI.Windowing.IconShowOptions.HideIconAndSystemMenu;

            this.IsMaximizable = false;
            this.SetIsMaximizable(false);
            this.SetIsResizable(false);
            this.IsResizable = false;
            this.CenterOnScreen();

            if (!Directory.Exists(Paths.RootLauncherDir))
            {
                UnInstallBtn.Visibility = Visibility.Collapsed;
                UpdateBtn.Visibility = Visibility.Collapsed;

                InstallBtn.Visibility = Visibility.Visible;
            }

            UpdateStatusHeader("Ready to install");
            UpdateProgressBar(0);
        }

        private void AboutAppBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void InstallBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateDesktopShortcutCheck.IsEnabled = false;
            RunAsPortableCheck.IsEnabled = false;
            (sender as Button).IsEnabled = false;
            UpdateStatusHeader("Creating Directories...");
            UpdateProgressBar(true);

            Directory.CreateDirectory(Paths.RootLauncherDir);
            Directory.CreateDirectory(Paths.LauncherDir);
            Directory.CreateDirectory(Paths.SettingsDir);
            Directory.CreateDirectory(Paths.InstallerDir);

            using (StreamWriter sw = File.CreateText(Paths.MinecraftLauncherInstallerConfig))
            {
                sw.WriteLine(JsonConvert.SerializeObject(new InstallerConfigJson() { RunAsPortable = Convert.ToBoolean(RunAsPortableCheck.IsChecked) }));
                sw.Close();
            }

            UpdateStatusHeader("Downloading...");
            UpdateProgressBar(0);
            await DownloadAndExtractFileAsync(InstallProgress, "https://github.com/Jurij15/MinecraftLauncher/raw/master/docs/api/latestVersion.zip", Paths.RootLauncherDir);

            UpdateStatusHeader("Done");
            InstallProgress.Visibility = Visibility.Collapsed;

            InstallBtn.Visibility = Visibility.Collapsed;

            OpenBtn.Visibility = Visibility.Visible;
        }

        //thanks chatgpt
        public async Task DownloadAndExtractFileAsync(ProgressBar progressBar, string DownloadUrl, string ExtractedDirectoryName)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Download the file
                var response = await httpClient.GetAsync(new Uri(DownloadUrl), HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                // Get the total size of the content to calculate progress
                long totalBytes = response.Content.Headers.ContentLength ?? -1;

                using (var contentStream = await response.Content.ReadAsStreamAsync())
                {
                    // Create a temporary file to store the downloaded content
                    var tempFile = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, "temp.zip");

                    using (var fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        // Buffer for reading and writing
                        byte[] buffer = new byte[8192];
                        int bytesRead;
                        long totalBytesRead = 0;

                        // Download and write to the temporary file
                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);

                            // Update progress
                            totalBytesRead += bytesRead;
                            UpdateProgressBar(progressBar, totalBytes, totalBytesRead);
                        }
                    }
                    UpdateProgressBar(true);
                    UpdateStatusHeader("Extracting...");
                    // Extract the contents to the specified directory
                    var extractionPath = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, ExtractedDirectoryName);
                    ZipFile.ExtractToDirectory(tempFile, extractionPath);

                    // Clean up: delete the temporary file
                    File.Delete(tempFile);
                }
            }
        }

        private void UpdateProgressBar(ProgressBar progressBar, long totalBytes, long bytesRead)
        {
            if (totalBytes > 0)
            {
                double progress = (double)bytesRead / totalBytes;
                progressBar.Value = progress * 100;
            }
            else
            {
                // If totalBytes is not available, just indeterminate progress
                progressBar.IsIndeterminate = true;
            }
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Paths.MinecraftLauncherExecutablePath);
            Process.GetCurrentProcess().Kill();
        }
    }
}
