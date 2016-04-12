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
    public class EventAwardsViewModel : NotifyPropertyChanged
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
        public EventAwardsViewModel()
        {
            changeEventCommand = new DelegateCommand(ChangeEvent);
            refreshCommand = new DelegateCommand(RefreshList);
            svm = new SettingsViewModel();
            competitionevent = new Event();
            EventData = new NotifyTaskCompletion<EventInformation>(LoadEventData(svm.EventKey));
            EventAwardData = new NotifyTaskCompletion<ObservableCollection<EventAwardInformation>>(LoadEventAwardData(svm.EventKey));
        }

        /// <summary>
        /// The internal EventAwardInformation list that hold the list of awards to display
        /// </summary>
        private NotifyTaskCompletion<ObservableCollection<EventAwardInformation>> eventAwardData { get; set; }

        /// <summary>
        /// Internal change event command to use as a DelegateCommand.
        /// </summary>
        private ICommand changeEventCommand;

        /// <summary>
        /// Internal refresh command to use as a DelegateCommand.
        /// </summary>
        private ICommand refreshCommand;

        /// <summary>
        /// Internal copy of the app settings
        /// </summary>
        private SettingsViewModel svm { get; set; }

        /// <summary>
        /// Internal event key member
        /// </summary>
        private Event competitionevent { get; set; }

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
        /// Code for the Refresh Command
        /// </summary>
        /// <param name="p"></param>
        private void RefreshList(object p)
        {
            EventAwardData = new NotifyTaskCompletion<ObservableCollection<EventAwardInformation>>(LoadEventAwardData(svm.EventKey));
        }

        /// <summary>
        /// Function to load event data into the viewmodel.
        /// </summary>
        /// <param name="eventkey">The event key to load.</param>
        private async Task<EventInformation> LoadEventData(string eventkey)
        {
            EventInformation ei = await competitionevent.GetEventInfo(eventkey);
            return ei;
        }

        /// <summary>
        /// Function to load event team data into the viewmodel.
        /// </summary>
        /// <param name="s">The team number to load.</param>
        private async Task<ObservableCollection<EventAwardInformation>> LoadEventAwardData(string eventkey)
        {
            List<EventAwardInformation> eai = await competitionevent.GetEventAwardList(eventkey);

            // Sort the events before we return them.
            var sortedresult = eai.OrderBy(award => award.award_type).Select(award => award);
            return new ObservableCollection<EventAwardInformation>(sortedresult);
        }
    }
}
