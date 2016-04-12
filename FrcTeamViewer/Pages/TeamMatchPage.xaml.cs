using TbaApiClient.DataModel;
using FrcTeamViewer;
using FrcTeamViewer.Pages;
using FrcTeamViewer.Presentation;
using System;
using Windows.UI.Xaml.Controls;

namespace FrcTeamViewer.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TeamMatchPage : Page
    {
        private TeamMatchViewModel tmvm;

        public TeamMatchPage()
        {
            this.InitializeComponent();
            tmvm = new TeamMatchViewModel();
            this.DataContext = tmvm;
            tmvm.CurrentPage = this.pageTeamMatchPage;
        }

        private void pageTeamMatchPage_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            if (e.PreviousSize != e.NewSize)
            {
                tmvm.PageWidth = ((TeamMatchPage)sender).Frame.ActualWidth;
            }
        }
    }
}
