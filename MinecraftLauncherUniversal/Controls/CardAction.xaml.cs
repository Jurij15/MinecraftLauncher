using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Controls
{
    public sealed partial class CardAction : UserControl
    {
        Compositor _compositor = Microsoft.UI.Xaml.Media.CompositionTarget.GetCompositorForCurrentThread();
        public static new readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(UIElement), typeof(CardAction), new PropertyMetadata(null));
        public CardAction()
        {
            this.InitializeComponent();

            VisualStateManager.GoToState(this, "Normal", true);


            //animations stuff
            /*
            var expressionAnim = _compositor.CreateExpressionAnimation();

            // The ellipse's scale is inversely proportional to the rectangle's scale
            expressionAnim.Expression = "Vector3(1/scaleElement.Scale.X, 1/scaleElement.Scale.Y, 1)";
            expressionAnim.Target = "Scale";

            // Use SetExpressionReferenceParameter to alias a UIElement into the expression string
            expressionAnim.SetExpressionReferenceParameter("scaleElement", this);

            // Start the animation on the ellipse
            this.StartAnimation(expressionAnim);
            */
        }

        public new UIElement Content
        {
            get => (UIElement)GetValue(ContentProperty);
            set
            {
                SetValue(ContentProperty, value);
                PropertyChanged?.Invoke(value, new PropertyChangedEventArgs(nameof(Content)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SetPointerNormalState(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void SetPointerOverState(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PointerOver", true);
            //this.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(100,0,0,0));
        }

        private void SetPointerPressedState(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Pressed", true);
        }
    }
}
