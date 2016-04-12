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
    public sealed partial class EventMatchPage : Page
    {
        private EventMatchViewModel emvm;

        public EventMatchPage()
        {
            this.InitializeComponent();
            emvm = new EventMatchViewModel();
            this.DataContext = emvm;
            emvm.CurrentPage = this.pageEventMatchPage;
        }

        private void pageEventMatchPage_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            if (e.PreviousSize != e.NewSize)
            {
                emvm.PageWidth = ((EventMatchPage)sender).Frame.ActualWidth;
            }
        }
    }
}
