using TbaApiClient.DataModel;
using FrcTeamViewer;
using FrcTeamViewer.Pages;
using FrcTeamViewer.Presentation;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FrcTeamViewer.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TeamInfoPage : Page
    {
        private TeamInfoViewModel tivm;

        public TeamInfoPage()
        {
            InitializeComponent();
            tivm = new TeamInfoViewModel();
            this.DataContext = tivm;
            tivm.CurrentPage = this;
        }
    }
}
                