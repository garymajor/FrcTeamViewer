using FrcTeamViewer.Pages;
using Intense.Presentation;
using TbaApiClient;
using TbaApiClient.DataModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace FrcTeamViewer.Presentation
{
    public class EventTeamsViewModel : NotifyPropertyChanged
    {
        /// <summary>
        /// The EventInformation object that will hold the event information, using the given eventkey
        /// </summary>
        public NotifyTaskCompletion<EventInformation> EventData { get; private set; }
        public NotifyTaskCompletion<ObservableCollection<TeamInformation>> EventTeamData { get; private set; }
        public Page CurrentPage { get; set; }

        /// <summary>
        /// Width of the page.
        /// </summary>
        //TODO: figure out how to do this through MVVM, rather than code-behind (currently set through the Page.SizeChanged event in code-behind)
        public double PageWidth
        {
            get
            {
                return pageWidth;
            }
            set
            {
                pageWidth = value;
                OnPropertyChanged("PageWidth");
            }
        }

        private ICommand changeEventCommand;
        private ICommand changeTeamCommand;
        private SettingsViewModel svm { get; set; }
        private ApiClient apiClient { get; set; }
        private double pageWidth { get; set; }

        public ICommand ChangeEventCommand
        {
            get
            {
                return changeEventCommand;
            }
        }

        public ICommand ChangeTeamCommand
        {
            get
            {
                return changeTeamCommand;
            }
        }

        /// <summary>
        /// Constructor - pulls the team to look up from the local settings
        /// </summary>
        public EventTeamsViewModel()
        {
            changeEventCommand = new DelegateCommand(ChangeEvent);
            changeTeamCommand = new DelegateCommand(ChangeTeam);
            svm = new SettingsViewModel();
            apiClient = new ApiClient(svm.LocalStorage);
            EventData = new NotifyTaskCompletion<EventInformation>(LoadEventData(svm.EventKey));
            EventTeamData = new NotifyTaskCompletion<ObservableCollection<TeamInformation>>(LoadEventTeamData(svm.EventKey));
        }

        private void ChangeEvent(object p)
        {
            CurrentPage.Frame.Navigate(typeof(TeamInfoPage));
        }

        private void ChangeTeam(object p)
        {
            var competitionteam = ((TeamInformation)((HyperlinkButton)p).DataContext).team_number;
            svm.TeamNumber = competitionteam;
            CurrentPage.Frame.Navigate(typeof(TeamInfoPage));
        }

        /// <summary>
        /// Function to load event data into the View Model.
        /// </summary>
        /// <param name="eventkey">The event key to load.</param>
        private async Task<EventInformation> LoadEventData(string eventkey)
        {
            EventInformation ei = await apiClient.EventApi.GetEventInfo(eventkey);
            return ei;
        }

        /// <summary>
        /// Function to load event team data into the View Model.
        /// </summary>
        /// <param name="s">The team number to load.</param>
        private async Task<ObservableCollection<TeamInformation>> LoadEventTeamData(string eventkey)
        {
            List<TeamInformation> ti = await apiClient.EventApi.GetEventTeamList(eventkey);

            // Sort the events before we return them.
            var sortedresult = ti.OrderBy(team => team.teamnum).Select(team => team);
            return new ObservableCollection<TeamInformation>(sortedresult);
        }
    }
}
