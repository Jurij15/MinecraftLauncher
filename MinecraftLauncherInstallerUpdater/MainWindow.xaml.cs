using MinecraftLauncherInstallerUpdater.Pages;
using MinecraftLauncherUniversal.Helpers;
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

namespace MinecraftLauncherInstallerUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Wpf.Ui.Controls.Window.FluentWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            if (Wpf.Ui.Appearance.Theme.GetSystemTheme() == Wpf.Ui.Appearance.SystemThemeType.Light)
            {
                Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light);
            }
            else
            {
                Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark);
            }

            ArgumentsHelper.bVersion = Updater.GetLatestVersionStringFromGitHub();
            ArgumentsHelper.ParseLaunchArguments();

            VersionBlock.Text = "Version: "+ ArgumentsHelper.bVersion;

            if (ArgumentsHelper.bIsInstalling)
            {
                RootFrame.Navigate(new InstallPage());
            }
            else if (ArgumentsHelper.bIsUpdating)
            {
                AppTitleBar.Title = "Updater";

            }
        }
    }
}
