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
    public sealed partial class EventAwardsPage : Page
    {
        private EventAwardsViewModel eavm;

        public EventAwardsPage()
        {
            this.InitializeComponent();
            eavm = new EventAwardsViewModel();
            this.DataContext = eavm;
            eavm.CurrentPage = this;
        }

        private void pageEventAwardsPage_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            eavm.PageWidth = ((EventAwardsPage)sender).Frame.ActualWidth;
        }
    }
}
