using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Controls
{
    public sealed partial class VersionCardControl : UserControl
    {
        public static readonly DependencyProperty VersionProperty =
                 DependencyProperty.Register("Version", typeof(string), typeof(VersionCardControl), new PropertyMetadata(null));
        public string Version
        {
            get { return (string)GetValue(VersionProperty); }
            set { SetValue(VersionProperty, value); }
        }

        // VersionInstalledState Property
        public string VersionInstalledState
        {
            get { return (string)GetValue(VersionInstalledStateProperty); }
            set { SetValue(VersionInstalledStateProperty, value); }
        }

        public static readonly DependencyProperty VersionInstalledStateProperty =
            DependencyProperty.Register("VersionInstalledState", typeof(string), typeof(VersionCardControl), new PropertyMetadata(""));

        public Image MinecraftImage { get; private set; }
        public TextBlock VersionTextBlock { get; private set; }
        public TextBlock VersionStateTextBlock;

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(Image), typeof(VersionCardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty VersionBlockProperty =
            DependencyProperty.Register("VersionTextBlock", typeof(TextBlock), typeof(VersionCardControl), new PropertyMetadata(null));
        public static readonly DependencyProperty VersionStateBlock =
            DependencyProperty.Register("VersionStateTextBlock", typeof(TextBlock), typeof(VersionCardControl), new PropertyMetadata(null));
        public VersionCardControl()
        {
            this.InitializeComponent();

            MinecraftImage = IMG;
            VersionTextBlock = VersionTextBlockO;
            VersionStateTextBlock = VersionStateTextBlockO;
        }

        public void UpdateImage()
        {
            IMG = MinecraftImage;
        }

        private void SetPointerNormalState(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
            VisualStateManager.GoToState(this, "IMGNormal", true);
        }

        private void SetPointerOverState(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PointerOver", true);
            VisualStateManager.GoToState(this, "IMGPointerOver", true);
        }

        private void SetPointerPressedState(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Pressed", true);
            VisualStateManager.GoToState(this, "IMGPressed", true);
        }
    }
}
