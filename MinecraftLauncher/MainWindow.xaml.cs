using CmlLib.Core;
using MinecraftLauncher.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MinecraftLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Wpf.Ui.Controls.Window.FluentWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Globals.Username = "Jurij15gq";
        }

        private void FluentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainNavigation.IsPaneOpen = false;
            MainNavigation.Navigate(typeof(HomePage));

            MainNavigation.PaneTitle = Globals.Username;

            Globals.MainWindow = this;
            Globals.MainNavigation = MainNavigation;
            Globals.PlayMenuItem = PlayPageTitle;
            //test();
        }

        async void test()
        {
            // increase connection limit to fast download
            System.Net.ServicePointManager.DefaultConnectionLimit = 256;

            //var path = new MinecraftPath("game_directory_path");
            var path = new MinecraftPath(); // use default directory

            var launcher = new CMLauncher(path);

            // show launch progress to console
            launcher.FileChanged += (e) =>
            {
                MessageBox.Show(e.FileKind.ToString()+e.FileName+ e.ProgressedFileCount+ e.TotalFileCount);
            };
            launcher.ProgressChanged += (s, e) =>
            {
                MessageBox.Show("{0}%" + e.ProgressPercentage.ToString());
            };

            var versions = await launcher.GetAllVersionsAsync();
            foreach (var item in versions)
            {
                // show all version names
                // use this version name in CreateProcessAsync method.
                MessageBox.Show(item.Name);
            }
        }
    }
}
