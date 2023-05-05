using MinecraftLauncher.Helpers;
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

namespace MinecraftLauncher.Pages
{
    /// <summary>
    /// Interaction logic for PlayPage.xaml
    /// </summary>
    public partial class PlayPage : Page
    {
        public PlayPage()
        {
            InitializeComponent();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            AsyncLaunch();
        }

        async void AsyncLaunch()
        {
            int memooryinmb = Convert.ToInt32(RamAmountBox.Value) * 1024;

            await PlayHelper.Launch(Globals.CurrentBuild, memooryinmb);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            VersionNameBlock.Text = "Minecraft " + Globals.CurrentBuild;
        }
    }
}
