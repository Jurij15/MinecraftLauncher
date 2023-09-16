using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Core;
using MinecraftLauncherUniversal.Core.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Controls
{
    public sealed partial class ServerCardControl : UserControl
    {
        public enum ServerState
        {
            Offline,
            Online,
            Unknown
        }
        public static readonly DependencyProperty ServerNameProperty =
         DependencyProperty.Register("ServerName", typeof(string), typeof(ServerCardControl), new PropertyMetadata(null));
        public string ServerName
        {
            get { return (string)GetValue(ServerNameProperty); }
            set { SetValue(ServerNameProperty, value); }
        }

        public static readonly DependencyProperty ServerStatusProperty =
           DependencyProperty.Register("ServerStatus", typeof(ServerState), typeof(ServerCardControl), new PropertyMetadata(null));
        public ServerState ServerStatus
        {
            get { return (ServerState)GetValue(ServerStatusProperty); }
            set { SetValue(ServerStatusProperty, value); }
        }

        public string ServerMOTD
        {
            get { return (string)GetValue(ServerMOTDProperty); }
            set { SetValue(ServerMOTDProperty, value); }
        }

        public static readonly DependencyProperty ServerMOTDProperty =
            DependencyProperty.Register("ServerMOTD", typeof(string), typeof(ServerCardControl), new PropertyMetadata(""));

        public ServerClass ServerClass
        {
            get { return (ServerClass)GetValue(ServerClassProperty); }
            set { SetValue(ServerClassProperty, value); }
        }

        public static readonly DependencyProperty ServerClassProperty =
            DependencyProperty.Register("ServerClass", typeof(ServerClass), typeof(ServerCardControl), new PropertyMetadata(""));

        public string ServerCurrentPlayers
        {
            get { return (string)GetValue(ServerCurrentPlayersProperty); }
            set { SetValue(ServerCurrentPlayersProperty, value); }
        }

        public static readonly DependencyProperty ServerCurrentPlayersProperty =
            DependencyProperty.Register("ServerCurrentPlayers", typeof(string), typeof(ServerCardControl), new PropertyMetadata(""));

        public string ServerTotalPlayers
        {
            get { return (string)GetValue(ServerTotalPlayersProperty); }
            set { SetValue(ServerTotalPlayersProperty, value); }
        }

        public static readonly DependencyProperty ServerTotalPlayersProperty =
            DependencyProperty.Register("ServerTotalPlayers", typeof(string), typeof(ServerCardControl), new PropertyMetadata(""));

        public InfoBadge StatusBadge { get; private set; }
        public TextBlock ServerNameTextBlock { get; private set; }
        public TextBlock VersionMOTDTextBlock { get; private set; }
        public StackPanel PlayersStatsPanel { get; private set; }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("InfoBadge", typeof(InfoBadge), typeof(VersionCardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty VersionBlockProperty =
            DependencyProperty.Register("NameTextBlock", typeof(TextBlock), typeof(VersionCardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty VersionStateBlock =
            DependencyProperty.Register("MOTDTextBlock", typeof(TextBlock), typeof(VersionCardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty PlayerStatsPanelProperty =
    DependencyProperty.Register("PlayerStatsPanel", typeof(StackPanel), typeof(VersionCardControl), new PropertyMetadata(null));
        public ServerCardControl()
        {
            this.InitializeComponent();
            this.DataContext = this;

            StatusBadge = StatusBadgeO;
            ServerNameTextBlock = ServerNameTextBlockO;
            VersionMOTDTextBlock = ServerMOTDTextBlockO;
            PlayersStatsPanel = PlayersStats;
        }

        public void UpdateStatus()
        {
            if (ServerStatus == ServerState.Online)
            {
                StatusBadge.Style = Application.Current.Resources["SuccessIconInfoBadgeStyle"] as Style;
                ToolTipService.SetToolTip(StatusBadge, "Online");
            }
            else if (ServerStatus == ServerState.Offline)
            {
                StatusBadge.Style = Application.Current.Resources["CriticalIconInfoBadgeStyle"] as Style;
                ToolTipService.SetToolTip(StatusBadge, "Offline");
            }
        }

        private void SetPointerNormalState(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void SetPointerOverState(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PointerOver", true);
        }

        private void SetPointerPressedState(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Pressed", true);
        }
    }
}
