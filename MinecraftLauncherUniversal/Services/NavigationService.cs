using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherUniversal.Services
{
    public class NavigationService
    {
        public static void UpdateBreadcrumb(string AddText, bool RemovePrevious)
        {
            if (RemovePrevious)
            {
                Globals.Breadcrumbs.Clear();
            }
            Globals.Breadcrumbs.Add(AddText);

            Globals.UpdateBreadcrumb();
        }

        public static void RemoveBreadcrumbSpecificElement(string SpecificElementName)
        {
            Globals.Breadcrumbs.Remove(SpecificElementName);

            Globals.UpdateBreadcrumb();
        }

        public static void NavigateHiearchical(Type TargetPageType,string BreadcrumbText , bool RemovePreviousText)
        {
            if (TargetPageType == null) { return; }
            UpdateBreadcrumb(BreadcrumbText, RemovePreviousText);

            SlideNavigationTransitionInfo info = new SlideNavigationTransitionInfo();
            info.Effect = SlideNavigationTransitionEffect.FromRight;
            Globals.MainFrame.Navigate(TargetPageType, null, info);

            ShowBreadcrumb();
        }

        public static bool CanGoBack()
        {
            return Globals.MainFrame.CanGoBack;
        }
        
        public static void FrameGoBack()
        {
            if (Globals.Breadcrumbs.Count  > 1)
            {
                Globals.Breadcrumbs.Remove(Globals.Breadcrumbs[1]);
            }
            Globals.UpdateBreadcrumb();
            Globals.MainFrame.GoBack();
        }

        public static void HideBreadcrumb()
        {
            Globals.MainNavigationBreadcrumb.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
            Globals.MainNavigation.AlwaysShowHeader = false;
        }
        public static void ShowBreadcrumb()
        {
            Globals.MainNavigationBreadcrumb.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            Globals.MainNavigation.AlwaysShowHeader = true;
        }
    }
}
