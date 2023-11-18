using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        }

        private void AboutAppBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
