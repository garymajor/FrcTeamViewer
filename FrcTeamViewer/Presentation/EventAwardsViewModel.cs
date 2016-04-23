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
    public class EventAwardsViewModel : ViewModelBase
    {
        /// <summary>
        /// The EventInformation object that will hold the event information, using the given eventkey
        /// </summary>
        public NotifyTaskCompletion<EventInformation> EventData { get; private set; }

        /// <summary>
        /// The EventAwardInformation list that hold the list of awards to display
        /// </summary>
        public NotifyTaskCompletion<ObservableCollection<EventAwardInformation>> EventAwardData
        {
            get
            {
                return eventAwardData;
            }
            private set
            {
                eventAwardData = value;
                OnPropertyChanged("EventAwardData");
            }
        }

        /// <summary>
        /// Constructor - Note: Initiates async loading of data for the view
        /// </summary>
        public EventAwardsViewModel()
        {
            // reset this since we overrode it.
            refreshCommand = new DelegateCommand(RefreshList);

            // Load the data
            EventData = new NotifyTaskCompletion<EventInformation>(LoadEventData(svm.EventKey));
            EventAwardData = new NotifyTaskCompletion<ObservableCollection<EventAwardInformation>>(LoadEventAwardData(svm.EventKey));
        }

        /// <summary>
        /// The internal EventAwardInformation list that hold the list of awards to display
        /// </summary>
        private NotifyTaskCompletion<ObservableCollection<EventAwardInformation>> eventAwardData { get; set; }

        /// <summary>
        /// Code for the Refresh Command
        /// </summary>
        /// <param name="p"></param>
        protected override void RefreshList(object p)
        {
            EventAwardData = new NotifyTaskCompletion<ObservableCollection<EventAwardInformation>>(LoadEventAwardData(svm.EventKey));
        }

        /// <summary>
        /// Function to load event data into the viewmodel.
        /// </summary>
        /// <param name="eventkey">The event key to load.</param>
        private async Task<EventInformation> LoadEventData(string eventkey)
        {
            EventInformation ei = await apiClient.EventApi.GetEventInfo(eventkey);
            return ei;
        }

        /// <summary>
        /// Function to load event team data into the viewmodel.
        /// </summary>
        /// <param name="s">The team number to load.</param>
        private async Task<ObservableCollection<EventAwardInformation>> LoadEventAwardData(string eventkey)
        {
            List<EventAwardInformation> eai = await apiClient.EventApi.GetEventAwardList(eventkey);

            // Sort the events before we return them.
            var sortedresult = eai.OrderBy(award => award.award_type).Select(award => award);
            return new ObservableCollection<EventAwardInformation>(sortedresult);
        }
    }
}
