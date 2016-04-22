using FrcTeamViewer.Pages;
using Intense.Presentation;
using TbaApiClient;
using TbaApiClient.DataModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace FrcTeamViewer.Presentation
{
    public class EventRankingViewModel : NotifyPropertyChanged
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
        /// The current page (set in the code behind constructor).
        /// </summary>
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

        /// <summary>
        /// Change Team Command
        /// </summary>
        public ICommand ChangeTeamCommand
        {
            get
            {
                return changeTeamCommand;
            }
        }

        /// <summary>
        /// Change Event Command
        /// </summary>
        public ICommand ChangeEventCommand
        {
            get
            {
                return changeEventCommand;
            }
        }

        /// <summary>
        /// Refresh Command
        /// </summary>
        public ICommand RefreshCommand
        {
            get
            {
                return refreshCommand;
            }
        }

        /// <summary>
        /// Constructor - Note: Initiates async loading of data for the view
        /// </summary>
        public EventRankingViewModel()
        {
            changeEventCommand = new DelegateCommand(ChangeEvent);
            changeTeamCommand = new DelegateCommand(ChangeTeam);
            refreshCommand = new DelegateCommand(RefreshList);
            svm = new SettingsViewModel();
            apiClient = new ApiClient(svm.LocalStorage);
            EventData = new NotifyTaskCompletion<EventInformation>(LoadEventData(svm.EventKey));
            EventRankingData = new NotifyTaskCompletion<ObservableCollection<EventRankingInformation>>(LoadEventRankingData(svm.EventKey));
        }

        /// <summary>
        /// Internal change event command to use as a DelegateCommand.
        /// </summary>
        private ICommand changeEventCommand;

        /// <summary>
        /// Internal change event command to use as a DelegateCommand.
        /// </summary>
        private ICommand changeTeamCommand;

        /// <summary>
        /// Internal refresh command to use as a DelegateCommand.
        /// </summary>
        private ICommand refreshCommand;
        
        /// <summary>
        /// Internal event ranking data
        /// </summary>
        private NotifyTaskCompletion<ObservableCollection<EventRankingInformation>> eventRankingData { get; set; }

        /// <summary>
        /// Internal copy of the app settings
        /// </summary>
        private SettingsViewModel svm { get; set; }

        /// <summary>
        /// Internal TBA API Client
        /// </summary>
        private ApiClient apiClient { get; set; }

        /// <summary>
        /// Internal Page Width member
        /// </summary>
        private double pageWidth { get; set; }

        /// <summary>
        /// Code for the Change Event Command
        /// </summary>
        /// <param name="p">The object passed from the view</param>
        private void ChangeEvent(object p)
        {
            CurrentPage.Frame.Navigate(typeof(TeamInfoPage));
        }

        /// <summary>
        /// Code for the Change Event Command
        /// </summary>
        /// <param name="p">The object passed from the view</param>
        private void ChangeTeam(object p)
        {
            svm.TeamNumber = (string)p;
            CurrentPage.Frame.Navigate(typeof(TeamInfoPage));
        }

        /// <summary>
        /// Code for the Refresh Command
        /// </summary>
        /// <param name="p"></param>
        private void RefreshList(object p)
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
