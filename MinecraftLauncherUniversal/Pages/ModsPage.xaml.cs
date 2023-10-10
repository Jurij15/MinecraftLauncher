using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLauncherUniversal.Core;
using MinecraftLauncherUniversal.Interop;
using MinecraftLauncherUniversal.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncherUniversal.Pages
{
    public class Mod
    {
        public string Name;
        public string Version;

        //public bool IsModEnabled; //TODO: Make enabling/ disabling mods a feature (move them into a seperate folder i guess)

        public McMod ModClass;
    }

    public sealed partial class ModsPage : Page
    {
        List<Mod> ModList;

        public ModsPage()
        {
            this.InitializeComponent();

            this.DataContext = this;
        }

        private void List_Loaded(object sender, RoutedEventArgs e)
        {
            ModList = new List<Mod>();

            ModsManager manager = new ModsManager();
            foreach (var item in manager.GetAllMods())
            {
                Mod mod = new Mod();
                mod.Name = item.ModName;
                if (!item.ModVersion.Contains("jarVersion")) //mod has a defined version
                {
                    mod.Version = item.ModVersion;
                }
                else
                {
                    //mod does not have a defined version, weird
                    //we will leave it empty for now, but we can always put something there instead
                }

                mod.ModClass = item;

                ModList.Add(mod);
            }

            if (ModList.Count == 0)
            {
                // no mods installed

                List.Items.Add(new ListViewItem() { Content = "No Mods Found" });
            }

            List.ItemsSource = ModList;
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StartMessage.Visibility = Visibility.Collapsed;
            ServerDetails.Visibility = Visibility.Visible;

            McMod mod = (List.SelectedItem as Mod).ModClass;

            ModNameBlock.Text = mod.ModName;

            if (!mod.ModDisplayURL.Contains("Tommy.TomlLazy"))
            {
                displayURLBtn.Visibility = Visibility.Visible;
                displayURLBtn.NavigateUri = new Uri(mod.ModDisplayURL);
                ToolTipService.SetToolTip(displayURLBtn, mod.ModDisplayURL);
            }
            else
            {
                displayURLBtn.Visibility = Visibility.Collapsed;
            }
            if (!mod.ModIssueTrackerUrl.Contains("Tommy.TomlLazy"))
            {
                issueTrackerURLBtn.Visibility = Visibility.Visible;
                issueTrackerURLBtn.NavigateUri = new Uri(mod.ModIssueTrackerUrl);
                ToolTipService.SetToolTip(issueTrackerURLBtn, mod.ModIssueTrackerUrl);
            }
            else
            {
                issueTrackerURLBtn.Visibility = Visibility.Collapsed;
            }

            if (!mod.ModVersion.Contains("jarVersion"))
            {
                ModVersionBlock.Text = mod.ModVersion;
            }
            else
            {
                ModVersionBlock.Text = "No version specified";
            }

            ModIDBlock.Text = mod.ModID;
            ModAuthorsBlock.Text = mod.ModAuthors;
            ModLicenseBlock.Text = mod.ModLicense;
            ModDescriptionBlock.Text = mod.ModDescription;
        }
    }
}
