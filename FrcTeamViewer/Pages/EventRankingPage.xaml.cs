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
    public sealed partial class EventRankingPage : Page
    {
        private EventRankingViewModel ervm;

        public EventRankingPage()
        {
            this.InitializeComponent();
            ervm = new EventRankingViewModel();
            this.DataContext = ervm;
            ervm.CurrentPage = this;
        }

        private void pageEventRankingPage_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            ervm.PageWidth = ((EventRankingPage)sender).Frame.ActualWidth;
        }
    }
}
