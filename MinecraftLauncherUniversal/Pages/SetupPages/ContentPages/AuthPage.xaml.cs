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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages.SetupPages.ContentPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AuthPage : Page
    {
        public AuthPage()
        {
            this.InitializeComponent();

            AuthSelectorCombo.SelectedIndex = 0;
        }

        private void AuthSelectorCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selecteditem = ((ComboBox)sender).SelectedIndex;
            if (selecteditem == 0)
            {
                DescriptionBox.Text = "Offline authentication in Minecraft refers to the process of verifying a player's identity without requiring an active internet connection. This is commonly used when playing the game without access to Minecraft's online services. Offline authentication allows players to access their saved worlds and progress locally on their device without needing to connect to the game's servers for authentication.";
            }
            if (selecteditem == 1)
            {
                DescriptionBox.Text = "\r\nMojang authentication in Minecraft is the system by which players prove their identity to access the game's online services. It involves sending login credentials to Mojang's servers, which then grant players access to multiplayer servers, online features, and their purchased content. This authentication process ensures secure and authorized interactions within the online Minecraft ecosystem.";
            }
        }
    }
}
