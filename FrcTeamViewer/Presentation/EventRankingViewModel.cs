using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TbaApiClient.DataModel;

namespace FrcTeamViewer.Presentation
{
    public class EventRankingViewModel : ViewModelBase
    {
        /// <summary>
        /// The EventInformation object that will hold the event information, using the given eventkey
        /// </summary>
        public NotifyTaskCompletion<EventInformation> EventData { get; private set; }

        /// <summary>
        /// The EventRankingInformation list that hold the list of awards to display
        /// </summary>
        public NotifyTaskCompletion<ObservableCollection<EventRankingInformation>> EventRankingData
        {
            get
            {
                return eventRankingData;
            }
            private set
            {
                eventRankingData = value;
                OnPropertyChanged("EventRankingData");
            }
        }

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
        /// Constructor - Note: Initiates async loading of data for the view
        /// </summary>
        public EventRankingViewModel()
        {
            // reset this because we overrode it.
            refreshCommand = new DelegateCommand(RefreshList);

            // Load the data
            EventData = new NotifyTaskCompletion<EventInformation>(LoadEventData(svm.EventKey));
            EventRankingData = new NotifyTaskCompletion<ObservableCollection<EventRankingInformation>>(LoadEventRankingData(svm.EventKey));
        }

        /// <summary>
        /// Internal event ranking data
        /// </summary>
        private NotifyTaskCompletion<ObservableCollection<EventRankingInformation>> eventRankingData { get; set; }

        /// <summary>
        /// Internal Page Width member
        /// </summary>
        private double pageWidth { get; set; }

        /// <summary>
        /// Code for the Refresh Command
        /// </summary>
        /// <param name="p"></param>
        protected override void RefreshList(object p)
        {
            EventRankingData = new NotifyTaskCompletion<ObservableCollection<EventRankingInformation>>(LoadEventRankingData(svm.EventKey));
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
        /// Function to load event ranking data into the viewmodel.
        /// </summary>
        /// <param name="s">The team number to load.</param>
        private async Task<ObservableCollection<EventRankingInformation>> LoadEventRankingData(string eventkey)
        {
            List<EventRankingInformation> eai = await apiClient.EventApi.GetEventRankingList(eventkey);

            // Don't need to sort this one - the api returns the data in the correct order.
            return new ObservableCollection<EventRankingInformation>(eai);
        }
    }
}
