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
    public sealed partial class DistrictRankingPage : Page
    {
        private DistrictRankingViewModel drvm;

        public DistrictRankingPage()
        {
            this.InitializeComponent();
            drvm = new DistrictRankingViewModel();
            this.DataContext = drvm;
            drvm.CurrentPage = this;
        }

        private void pageDistrictRankingPage_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            if (e.NewSize != e.PreviousSize)
            {
                drvm.PageWidth = ((DistrictRankingPage)sender).Frame.ActualWidth;
            }
        }
    }
}
