// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using ABI.Microsoft.UI.Xaml;
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
using Windows.UI.Popups;
using Application = Microsoft.UI.Xaml.Application;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherWinUI.Assets
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllVersionsPage : Page
    {
        public void CreateCard(string VersionName)
        {
            Button newCard = new Button();
            TextBlock versionName = new TextBlock();

            versionName.Text = VersionName;

            newCard.CornerRadius = new Microsoft.UI.Xaml.CornerRadius(8);
            newCard.BorderThickness = new Microsoft.UI.Xaml.Thickness(1);
            newCard.Padding = new Microsoft.UI.Xaml.Thickness(12);

            newCard.BorderBrush = Application.Current.Resources["CardStrokeColorDefaultBrush"] as Brush;
            newCard.Background = Application.Current.Resources["CardBackgroundFillColorDefaultBrush"] as Brush;

            newCard.Content = versionName;

            ItemsPanel.Children.Add(newCard);
        }
        public AllVersionsPage()
        {
            this.InitializeComponent();
            MessageDialog dlg = new MessageDialog("test");
            dlg.ShowAsync();
        }

        private void testBtn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            CreateCard("test");
        }
    }
}
