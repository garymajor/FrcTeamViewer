using Intense.Presentation;
using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using FrcTeamViewer.Pages;
using FrcTeamViewer.Presentation;

namespace FrcTeamViewer
{
    public sealed partial class Shell : UserControl
    {
        public Shell()
        {
            this.InitializeComponent();

            var vm = new ShellViewModel();
            //vm.TopItems.Add(new NavigationItem { Icon = "", DisplayName = "Welcome", PageType = typeof(WelcomePage) });
            vm.TopItems.Add(new NavigationItem { Icon = "\uE8D4", DisplayName = "Team Info", PageType = typeof(TeamInfoPage) });
            vm.TopItems.Add(new NavigationItem { Icon = "\uE133", DisplayName = "Event Matches", PageType = typeof(EventMatchPage) });
            vm.TopItems.Add(new NavigationItem { Icon = "\ue779", DisplayName = "Team Matches", PageType = typeof(TeamMatchPage) });
            vm.TopItems.Add(new NavigationItem { Icon = "\uE716", DisplayName = "Event Teams", PageType = typeof(EventTeamsPage) });
            vm.TopItems.Add(new NavigationItem { Icon = "\uE7AC", DisplayName = "Event Ranking", PageType = typeof(EventRankingPage) });
            vm.TopItems.Add(new NavigationItem { Icon = "\uE734", DisplayName = "Event Awards", PageType = typeof(EventAwardsPage) });

            vm.BottomItems.Add(new NavigationItem { Icon = "\uE713", DisplayName = "Settings", PageType = typeof(SettingsPage) });
            vm.BottomItems.Add(new NavigationItem { Icon = "\uE897", DisplayName = "About", PageType = typeof(AboutPage) });

            // select the item we should navigate to... If we don't have a team number, go to settings, otherwise, go to team info
            if (vm.CurrentTeamNumber == null || vm.CurrentTeamNumber.CompareTo(string.Empty) == 0)
            {
                vm.SelectedItem = vm.BottomItems.Where(item => item.PageType == typeof(SettingsPage)).Select(item => item).First();
            }
            else
            {
                vm.SelectedItem = vm.TopItems.Where(item => item.PageType == typeof(TeamInfoPage)).Select(item => item).First();
            }

            this.ViewModel = vm;
        }

        public ShellViewModel ViewModel { get; private set; }

        public Frame RootFrame
        {
            get
            {
                return this.Frame;
            }
        }
    }
}
