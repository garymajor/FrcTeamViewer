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
    public sealed partial class EventTeamsPage : Page
    {
        private EventTeamsViewModel etvm;

        public EventTeamsPage()
        {
            this.InitializeComponent();
            etvm = new EventTeamsViewModel();
            this.DataContext = etvm;
            etvm.CurrentPage = this;
        }

        private void pageEventTeamsPage_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            etvm.PageWidth = ((EventTeamsPage)sender).Frame.ActualWidth;
        }
    }
}
