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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        void CreateCard(string VersionName)
        {
            Wpf.Ui.Controls.CardAction newCard = new Wpf.Ui.Controls.CardAction();
            StackPanel panel = new StackPanel();

            TextBlock name = new TextBlock();

            string CorrectVersionName = StringHelper.ReplaceDisallowedChars(VersionName);

            name.Text = VersionName;
            name.TextWrapping = TextWrapping.NoWrap;
            name.TextTrimming = TextTrimming.CharacterEllipsis;

            panel.Children.Add(name);

            newCard.ToolTip = VersionName;

            newCard.Content = panel;

            newCard.Name = CorrectVersionName;

            newCard.Width = 132;

            newCard.Margin = new Thickness(2);

            newCard.IsChevronVisible = false;

            newCard.Click += NewCard_Click;

            ItemsPanel.Children.Add(newCard);
        }

        private void NewCard_Click(object sender, RoutedEventArgs e)
        {
            string name = StringHelper.ReturnDisallowedChars(((Wpf.Ui.Controls.CardAction)sender).Name);
            Globals.CurrentBuild = name;

            Globals.PlayMenuItem.Content = "Play " + name;

            Globals.MainNavigation.NavigateWithHierarchy(typeof(PlayPage));
        }

        public HomePage()
        {
            InitializeComponent();

            InitRecents();
        }

        void InitRecents()
        {
            foreach (var version in Globals.Recents)
            {
                CreateCard(version);
            }
        }
    }
}
