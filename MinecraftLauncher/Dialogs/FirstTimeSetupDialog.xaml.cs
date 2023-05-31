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
using Wpf.Ui.Controls.ContentDialogControl;

namespace MinecraftLauncher.Dialogs
{
    /// <summary>
    /// Interaction logic for FirstTimeSetupDialog.xaml
    /// </summary>
    public partial class FirstTimeSetupDialog : ContentDialog
    {
        bool Finished = false;
        public FirstTimeSetupDialog(ContentPresenter contentPresenter) : base(contentPresenter)
        {
            InitializeComponent();

            //Title = "Setup";

            this.TabStatusBox.Text = "Step 0 of 3";
        }

        protected override void OnButtonClick(ContentDialogButton button)
        {
            if (button == ContentDialogButton.Close)
            {
                if (Tabs.SelectedIndex < 4)
                {
                    Tabs.SelectedIndex = Tabs.SelectedIndex + 1;
                }
                TabStatusBox.Text = $"Step {Tabs.SelectedIndex.ToString()} of 2";

                if (Tabs.SelectedIndex == Tabs.Items.Count-1)
                {
                    CloseButtonText = "Finish";
                    if (CloseButtonText == "Finish" && Finished)
                    {
                        base.OnButtonClick(button);
                    }
                    Finished = true;
                }
            }
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(this.ActualHeight.ToString(), this.ActualWidth.ToString());
            //this.Height = 1240;
            //this.Width = 700;
        }

        private void UsernameSettingsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameSettingsBox.Text))
            {
                return;
            }
            else
            {
                Globals.Username = UsernameSettingsBox.Text;
                Settings.SaveNewUsername();
            }
        }

        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Tabs.SelectedItem == Tab2)
            {
                this.Title = "Setup";
            }
            else
            {
                this.Title = "";
            }
        }
    }
}
