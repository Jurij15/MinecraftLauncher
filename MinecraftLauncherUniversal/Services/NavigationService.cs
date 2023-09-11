using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncherUniversal.Services
{
    public class NavigationService
    {
        #region Classes
        //thanks to https://github.com/microsoft/devhome/blob/main/settings/DevHome.Settings/Models/Breadcrumb.cs#L10
        public class Breadcrumb
        {
            public Breadcrumb(string label, Type page)
            {
                Label = label;
                Page = page;
            }
            public string Label { get; }

            public Type Page { get; }

            public override string ToString() => Label;

            public void NavigateToFromBreadcrumb(int BreadcrumbItemIndex)
            {
                NavigationService.NavigateInternal(Page, BreadcrumbItemIndex);
            }
        }
        #endregion

        #region Properties
        public static NavigationView MainNavigation { get; private set; }
        public static BreadcrumbBar MainBreadcrumb { get; private set; }

        public static Frame MainFrame { get; private set; }

        public static ObservableCollection<Breadcrumb> BreadCrumbs = new ObservableCollection<Breadcrumb>();
        #endregion

        #region Constructor
        public static void InitializeNavigationService(NavigationView navigationView, BreadcrumbBar breadcrumbBar, Frame frame)
        {
            MainNavigation = navigationView;
            MainBreadcrumb = breadcrumbBar;
            MainFrame = frame;
        }
        #endregion

        #region Private Functions
        private static void UpdateBreadcrumb()
        {
            MainBreadcrumb.ItemsSource = BreadCrumbs;
        }
        private static void NavigateInternal(Type page, int BreadcrumbBarIndex)
        {
            SlideNavigationTransitionInfo info = new SlideNavigationTransitionInfo();
            info.Effect = SlideNavigationTransitionEffect.FromLeft;
            MainFrame.Navigate(page, null, info);

            int indexToRemoveAfter = BreadcrumbBarIndex;

            if (indexToRemoveAfter < BreadCrumbs.Count - 1)
            {
                int itemsToRemove = BreadCrumbs.Count - indexToRemoveAfter - 1;

                for (int i = 0; i < itemsToRemove; i++)
                {
                    BreadCrumbs.RemoveAt(indexToRemoveAfter + 1);
                }
            }
        }
        #endregion

        #region Public Functions
        public static void Navigate(Type TargetPageType, string BreadcrumbItemLabel, bool ClearNavigation)
        {
            Log.Verbose("Navigate started");
            if (ClearNavigation)
            {
                BreadCrumbs.Clear();
                MainFrame.BackStack.Clear();
                Log.Verbose("Cleaned navigation breadcrumb and backstack");
            }
            BreadCrumbs.Add(new Breadcrumb(BreadcrumbItemLabel, TargetPageType));
            Log.Verbose($"Added a new breadcrumb with page {TargetPageType.ToString()}, and label {BreadcrumbItemLabel}");

            if (ClearNavigation)
            {
                MainFrame.Navigate(TargetPageType, null, new EntranceNavigationTransitionInfo());
            }
            else
            {
                SlideNavigationTransitionInfo info = new SlideNavigationTransitionInfo();
                (info as SlideNavigationTransitionInfo).Effect = SlideNavigationTransitionEffect.FromRight;
                MainFrame.Navigate(TargetPageType, null, info);
            }
            Log.Verbose($"Navigated to {TargetPageType.ToString()}");

            UpdateBreadcrumb();
            ChangeBreadcrumbVisibility(true);
        }
        public static void Navigate(Type TargetPageType, string BreadcrumbItemLabel, bool ClearNavigation, bool BreadcrumbVisibility)
        {
            Log.Verbose("Navigate started");
            if (ClearNavigation)
            {
                BreadCrumbs.Clear();
                MainFrame.BackStack.Clear();
                Log.Verbose("Cleaned navigation breadcrumb and backstack");
            }
            BreadCrumbs.Add(new Breadcrumb(BreadcrumbItemLabel, TargetPageType));
            Log.Verbose($"Added a new breadcrumb with page {TargetPageType.ToString()}, and label {BreadcrumbItemLabel}");

            ChangeBreadcrumbVisibility(BreadcrumbVisibility);

            if (ClearNavigation)
            {
                MainFrame.Navigate(TargetPageType, null, new EntranceNavigationTransitionInfo());
            }
            else
            {
                SlideNavigationTransitionInfo info = new SlideNavigationTransitionInfo();
                (info as SlideNavigationTransitionInfo).Effect = SlideNavigationTransitionEffect.FromRight;
                MainFrame.Navigate(TargetPageType, null, info);
            }
            Log.Verbose($"Navigated to {TargetPageType.ToString()}");

            UpdateBreadcrumb();
        }
        public static void Navigate(Type TargetPageType, string BreadcrumbItemLabel, bool ClearNavigation, SlideNavigationTransitionEffect TransitionEffect)
        {
            if (ClearNavigation)
            {
                BreadCrumbs.Clear();
                MainFrame.BackStack.Clear();
            }
            BreadCrumbs.Add(new Breadcrumb(BreadcrumbItemLabel, TargetPageType));

            SlideNavigationTransitionInfo info = new SlideNavigationTransitionInfo();
            info.Effect = (SlideNavigationTransitionEffect)TransitionEffect;

            UpdateBreadcrumb();
            MainFrame.Navigate(TargetPageType, null, info);
            Log.Verbose($"Navigated to {TargetPageType.ToString()}, with effect {TransitionEffect.ToString()}");
        }

        public static void NavigateSuppressedAnim(Type TargetPageType, string BreadcrumbItemLabel, bool ClearNavigation)
        {
            if (ClearNavigation)
            {
                BreadCrumbs.Clear();
                MainFrame.BackStack.Clear();
            }
            BreadCrumbs.Add(new Breadcrumb(BreadcrumbItemLabel, TargetPageType));

            UpdateBreadcrumb();
            MainFrame.Navigate(TargetPageType, null, new SuppressNavigationTransitionInfo());
            Log.Verbose($"Navigated to {TargetPageType.ToString()}, with no effect");
        }

        public static void NavigateSuppressedAnim(Type TargetPageType, string BreadcrumbItemLabel, bool ClearNavigation, bool BreadcrumbVisibility)
        {
            if (ClearNavigation)
            {
                BreadCrumbs.Clear();
                MainFrame.BackStack.Clear();
            }
            BreadCrumbs.Add(new Breadcrumb(BreadcrumbItemLabel, TargetPageType));

            UpdateBreadcrumb();
            MainFrame.Navigate(TargetPageType, null, new SuppressNavigationTransitionInfo());
            ChangeBreadcrumbVisibility(BreadcrumbVisibility);
            Log.Verbose($"Navigated to {TargetPageType.ToString()}, with no effect");
        }

        public static void ChangeBreadcrumbVisibility(bool IsBreadcrumbVisible)
        {
            if (IsBreadcrumbVisible)
            {
                MainBreadcrumb.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                MainNavigation.AlwaysShowHeader = true;
            }
            else
            {
                MainBreadcrumb.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                MainNavigation.AlwaysShowHeader = false;
            }

            Log.Verbose($"Changed breadcurmbbar visibility to {IsBreadcrumbVisible.ToString()}");
        }
        #endregion
    }
}