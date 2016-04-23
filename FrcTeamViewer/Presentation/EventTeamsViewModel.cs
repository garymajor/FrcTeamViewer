using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TbaApiClient.DataModel;

namespace FrcTeamViewer.Presentation
{
    public class EventTeamsViewModel : ViewModelBase
    {
        /// <summary>
        /// The EventInformation object that will hold the event information, using the given eventkey
        /// </summary>
        public NotifyTaskCompletion<EventInformation> EventData { get; private set; }
        
        /// <summary>
        /// The TeamInformation object that will hold the event team list, using the given eventkey
        /// </summary>
        public NotifyTaskCompletion<ObservableCollection<TeamInformation>> EventTeamData
        {
            get
            {
                return eventTeamData;
            }
            private set
            {
                eventTeamData = value;
                OnPropertyChanged("EventTeamData");
            }
        }

        /// <summary>
        /// Constructor - pulls the team to look up from the local settings
        /// </summary>
        public EventTeamsViewModel()
        {
            // reset this because we overrode it
            refreshCommand = new DelegateCommand(RefreshList);

            // Load the data
            EventData = new NotifyTaskCompletion<EventInformation>(LoadEventData(svm.EventKey));
            EventTeamData = new NotifyTaskCompletion<ObservableCollection<TeamInformation>>(LoadEventTeamData(svm.EventKey));
        }

        /// <summary>
        /// Internal event team data
        /// </summary>
        private NotifyTaskCompletion<ObservableCollection<TeamInformation>> eventTeamData { get; set; }

        /// <summary>
        /// Code for the Refresh Command
        /// </summary>
        /// <param name="p"></param>
        protected override void RefreshList(object p)
        {
            EventTeamData = new NotifyTaskCompletion<ObservableCollection<TeamInformation>>(LoadEventTeamData(svm.EventKey));
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
