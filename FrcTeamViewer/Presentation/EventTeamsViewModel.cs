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
        public NotifyTaskCompletion<ObservableCollection<TeamInformation>> EventTeamData { get; private set; }

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

        /// <summary>
        /// Constructor - pulls the team to look up from the local settings
        /// </summary>
        public EventTeamsViewModel()
        {
            EventData = new NotifyTaskCompletion<EventInformation>(LoadEventData(svm.EventKey));
            EventTeamData = new NotifyTaskCompletion<ObservableCollection<TeamInformation>>(LoadEventTeamData(svm.EventKey));
        }

        /// <summary>
        /// Internal Page Width member
        /// </summary>
        private double pageWidth { get; set; }

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
