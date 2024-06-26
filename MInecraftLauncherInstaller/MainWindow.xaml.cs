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
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using WinUIEx;
using IWshRuntimeLibrary;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MInecraftLauncherInstaller
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WinUIEx.WindowEx
    {
        string LauncherVersion = "1.1";
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

        public static string GetLatestVersionStringFromGitHub()
        {
            string RetVal = string.Empty;

            if (System.IO.File.Exists(Path.Combine(Paths.RootAppDataPath, "VersionTemp")))
            {
                System.IO.File.Delete(Path.Combine(Paths.RootAppDataPath, "VersionTemp"));
            }

            Uri uri = new Uri("https://raw.githubusercontent.com/Jurij15/MinecraftLauncher/master/docs/api/latestVersion.txt");
            try
            {
                using (var client = new HttpClient())
                {
                    using (var s = client.GetStreamAsync(uri))
                    {
                        using (var fs = new FileStream(Path.Combine(Paths.RootAppDataPath, "VersionTemp"), FileMode.OpenOrCreate))
                        {
                            s.Result.CopyTo(fs);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                //DialogService.ShowSimpleDialog("Error", "An error occured while checking for updates");
                return null;
            }

            RetVal = System.IO.File.ReadAllText(Path.Combine(Paths.RootAppDataPath, "VersionTemp"));
            System.IO.File.Delete(Path.Combine(Paths.RootAppDataPath, "VersionTemp"));
            return RetVal;
        }

        public MainWindow()
        {
            this.InitializeComponent();

            this.Title = "MinecraftLauncher - Installer";

            AppWindow.Resize(new Windows.Graphics.SizeInt32(650, 440));

            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(AppTitleBar);

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

            InstallPathBox.Text = Paths.LauncherDir;

            VersionBox.Text = LauncherVersion;
            VersionBlock.Text = GetLatestVersionStringFromGitHub();

            this.SetIcon("Assets\\LogoNew.png");
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

            using (StreamWriter sw = System.IO.File.CreateText(Paths.MinecraftLauncherInstallerConfig))
            {
                sw.WriteLine(JsonConvert.SerializeObject(new InstallerConfigJson() { RunAsPortable = Convert.ToBoolean(RunAsPortableCheck.IsChecked) }));
                sw.Close();
            }

            UpdateStatusHeader("Downloading...");
            UpdateProgressBar(0);
            await DownloadAndExtractFileAsync(InstallProgress, "https://github.com/Jurij15/MinecraftLauncher/raw/master/docs/api/latestVersion.zip", Paths.LauncherDir);

            if (Convert.ToBoolean(CreateDesktopShortcutCheck.IsChecked))
            {
                UpdateStatusHeader("Creating Shortcut...");
                await Task.Delay(1000);
                CreateDesktopShortcut(Paths.MinecraftLauncherExecutablePath);
            }

            UpdateStatusHeader("Done");
            InstallProgress.Visibility = Visibility.Collapsed;

            InstallBtn.Visibility = Visibility.Collapsed;

            OpenBtn.Visibility = Visibility.Visible;
        }

        //thanks chatgpt
        public async Task DownloadAndExtractFileAsync(ProgressBar progressBar, string DownloadUrl, string ExtractedDirectoryName)
        {
            using (WebClient webClient = new WebClient())
            {
                // Subscribe to the DownloadProgressChanged event to track the progress
                webClient.DownloadProgressChanged += (sender, e) =>
                {
                    UpdateProgressBar(progressBar, e.TotalBytesToReceive, e.BytesReceived);
                };

                // Download the file asynchronously
                var tempFile = Path.Combine(Paths.RootLauncherDir, "temp.zip");
                await webClient.DownloadFileTaskAsync(new Uri(DownloadUrl), tempFile);

                UpdateProgressBar(true);
                UpdateStatusHeader("Extracting...");

                // Extract the contents to the specified directory
                var extractionPath = ExtractedDirectoryName;
                ZipFile.ExtractToDirectory(tempFile, extractionPath);

                // Clean up: delete the temporary file
                System.IO.File.Delete(tempFile);
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

        private async void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Paths.MinecraftLauncherExecutablePath);
            UpdateStatusHeader("Opening...");
            await Task.Delay(4000);

            Process.GetCurrentProcess().Kill();
        }

        static void CreateDesktopShortcut(string executablePath)
        {
            /*
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string shortcutPath = Path.Combine(desktopPath, "MinecraftLauncher.lnk");

                // Create a shortcut file
                using (StreamWriter writer = new StreamWriter(shortcutPath))
                {
                    writer.Write("[InternetShortcut]\r\n");
                    writer.Write("URL=file:///" + executablePath.Replace('\\', '/') + "\r\n");
                    writer.Write("IconFile=" + executablePath + "\r\n");
                    writer.Write("IconIndex=0\r\n");
                    writer.Write("IDList=\r\n");
                    writer.Flush();
                }

                Console.WriteLine("Shortcut created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating shortcut: {ex.Message}");
            }
            */
            try
            {
                object shDesktop = (object)"Desktop";
                WshShell shell = new WshShell();
                string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\Minecraft Launcher.lnk";
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                shortcut.TargetPath = Paths.MinecraftLauncherExecutablePath;
                shortcut.Save();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
