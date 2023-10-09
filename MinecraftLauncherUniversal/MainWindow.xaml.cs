using CommunityToolkit.WinUI.UI.Helpers;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.AppNotifications;
using MinecraftLauncherUniversal.Dialogs;
using MinecraftLauncherUniversal.Helpers;
using MinecraftLauncherUniversal.Managers;
using MinecraftLauncherUniversal.Pages;
using MinecraftLauncherUniversal.Pages.SetupPages;
using MinecraftLauncherUniversal.Services;
using Serilog.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.UI;
using WinUIEx;
using WinUIEx.Messaging;
using static MinecraftLauncherUniversal.Services.NavigationService;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowEx
    {
        public static Button TitleBarPaneToggleButton;
        public static Button TitleBarGoBackButton;
        public static Frame MainWindowFrame;
        #region Design and objects
        void InitDesgin() 
        {
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            this.CenterOnScreen();
            this.SetIsResizable(false);
           
            Title = "Minecraft Launcher";

            if (Environment.OSVersion.Version.Build <= 22000) //enable the normal look of navigationview on windows 10
            {
                //MainNavigationDisableContentBackgroundDictionary.ThemeDictionaries.Clear();
            }
        }

        void SetGlobalObjects()
        {
            Globals.MainWindow = this;
            Globals.MainGrid = RootGrid;
        }

        #endregion
        public MainWindow()
        {
            this.InitializeComponent();
            this.Title = "Minecraft Launcher";
            InitDesgin();
            SetGlobalObjects();

            MainWindowFrame = MainWindowRootFrame;
            TitleBarPaneToggleButton = AppTitlePaneButton;
            TitleBarGoBackButton = AppTitleBackButton;

            this.SetIcon("Assets\\LogoNew.ico");

            //only show console on prerelease
            if (Updater.bIsPrerelease())
            {
                //Globals.SetupConsole();
            }
            //setup logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Verbose)
                .CreateLogger();
            Log.Information("MinecraftLauncher by Jurij15, Version: " + Globals.VersionString);
        }

        private async void RootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            Globals.MainGridXamlRoot = this.Content.XamlRoot;

            //await Task.Delay(100);//wait for the actual page to load

            if (Globals.ToastFailedInit && !Globals.bIsFirstTimeRun)
            {
                Log.Error("Notifications failed to initialize!");
                DialogService.ShowSimpleDialog("Error", "Notifications failed to initialize and will be disabled during app runtime!");
            }
        }

        private void WindowEx_Closed(object sender, WindowEventArgs args)
        {
            Globals.CloseConsole();
        }

        private void AppTitlePaneButton_Click(object sender, RoutedEventArgs e)
        {
            Log.Verbose("Pane toggle button in titlebar pressed!");
            bool Current = MainNavigation.IsPaneOpen;
            if (Current)
            {
                MainNavigation.IsPaneOpen = false;
            }
            else
            {
                MainNavigation.IsPaneOpen = true;
            }
        }

        private void MainWindowRootFrame_Loaded(object sender, RoutedEventArgs e)
        {
            Log.Verbose("MainWindowFrame Loaded");
            ThemeService.BackdropExtension.SetBackdrop(ThemeService.BackdropExtension.Backdrop.None);
            Log.Verbose("Will navigate to StartupPage");
            MainWindowFrame.Navigate(typeof(StartupPage));
        }

        private void RootGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetTitleBar(AppTitleBar);//every time the buttons are reoredered, the titlebar drag region is set
        }
    }
}
