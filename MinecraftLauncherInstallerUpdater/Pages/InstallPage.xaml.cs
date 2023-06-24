using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;

namespace MinecraftLauncherInstallerUpdater.Pages
{
    /// <summary>
    /// Interaction logic for InstallPage.xaml
    /// </summary>
    public partial class InstallPage : Page
    {
        public InstallPage()
        {
            InitializeComponent();

            PathBox.Text = Config.InstallPath;


            StatusBlock.Text = "";
        }

        private void PinToTaskbarCheck_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void PinToTaskbarCheck_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void InstallBtn_Click(object sender, RoutedEventArgs e)
        {
            string DownloadLauncherPath = "https://raw.githubusercontent.com/Jurij15/MinecraftLauncher/master/docs/api/latestRelease.zip";
            string DownloadInstallerPath = "https://raw.githubusercontent.com/Jurij15/MinecraftLauncher/master/docs/api/latestInstaller.zip";

            if (!Directory.Exists(Config.InstallPath))
            {
                Directory.CreateDirectory(Config.InstallPath);
            }

            try
            {
                using (var client = new HttpClient())
                {
                    using (var s = client.GetStreamAsync(DownloadLauncherPath))
                    {
                        using (var fs = new FileStream(Config.TempFIlePath, FileMode.OpenOrCreate))
                        {
                            s.Result.CopyTo(fs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please check your internet connection!", "Download Failed");
                throw;
            }

            ZipFile.ExtractToDirectory(Config.TempFIlePath, Config.LauncherPath);
            System.IO.File.Delete(Config.TempFIlePath);

            try
            {
                using (var client = new HttpClient())
                {
                    using (var s = client.GetStreamAsync(DownloadInstallerPath))
                    {
                        using (var fs = new FileStream(Config.TempFIlePath, FileMode.OpenOrCreate))
                        {
                            s.Result.CopyTo(fs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please check your internet connection!", "Download Failed");
                throw;
            }

            ZipFile.ExtractToDirectory(Config.TempFIlePath, Config.UpdaterDirPath);
            System.IO.File.Delete(Config.TempFIlePath);

            Process.Start(Config.LauncherPath + "MinecraftLauncherUniversal.exe");
        }
    }
}
