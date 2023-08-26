using CommunityToolkit.Labs.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Interop;
using MinecraftLauncherUniversal.Managers;
using MinecraftNbtWorld;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx.Messaging;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages.WorldsPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllWorldsPage : Page
    {
        void CreateWorldCard(MWorld World, string Path)
        {
            SettingsCard card = new SettingsCard();
            card.Tag = World;

            Log.Verbose("World with path:" + Path);

            card.Header = World.Level.LevelName;

            ItemsPanel.Items.Add(card);
        }
        public AllWorldsPage()
        {
            this.InitializeComponent();
        }

        private void ItemsPanel_Loaded(object sender, RoutedEventArgs e)
        {
            ItemsPanel.Items.Clear();
            WorldsManager manager = new WorldsManager();
            manager.LoadAllPaths();
            foreach (var item in WorldsManager.WorldsPaths)
            {
                if (File.Exists(item+"\\level.dat"))
                {
                    Log.Verbose("Creating world card from path: " + item);
                    CreateWorldCard(new MWorld(item), item);
                }
            }
        }

        private void ItemsPanel_Unloaded(object sender, RoutedEventArgs e)
        {
            ItemsPanel.Items.Clear();
        }
    }
}
