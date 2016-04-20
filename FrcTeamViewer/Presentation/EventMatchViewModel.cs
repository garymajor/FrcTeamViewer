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
    public class EventMatchViewModel : NotifyPropertyChanged
    {
        /// <summary>
        /// The EventInformation object that will hold the event information, using the given eventkey
        /// </summary>
        public NotifyTaskCompletion<EventInformation> EventData { get; private set; }

        /// <summary>
        /// The EventRankingInformation list that hold the list of awards to display
        /// </summary>
        public NotifyTaskCompletion<ObservableCollection<MatchInformation>> EventMatchData
        {
            get
            {
                return eventMatchData;
            }
            set
            {
                eventMatchData = value;
                OnPropertyChanged("EventMatchData");
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
        /// Refresh List Command
        /// </summary>
        public ICommand RefreshCommand
        {
            get
            {
                return refreshCommand;
            }
        }

        /// <summary>
        /// Sort List Command
        /// </summary>
        public ICommand SortListCommand
        {
            get
            {
                return sortListCommand;
            }
        }

        /// <summary>
        /// Constructor - Note: Initiates async loading of data for the view
        /// </summary>
        public EventMatchViewModel()
        {
            changeEventCommand = new DelegateCommand(ChangeEvent);
            changeTeamCommand = new DelegateCommand(ChangeTeam);
            refreshCommand = new DelegateCommand(RefreshList);
            sortListCommand = new DelegateCommand(SortList);
            svm = new SettingsViewModel();
            apiClient = new ApiClient();
            EventData = new NotifyTaskCompletion<EventInformation>(LoadEventData(svm.EventKey));
            EventMatchData = new NotifyTaskCompletion<ObservableCollection<MatchInformation>>(LoadEventMatchData(svm.EventKey));
        }

        /// <summary>
        /// Internal change event command to use as a DelegateCommand.
        /// </summary>
        private ICommand changeEventCommand;

        /// <summary>
        /// Internal change team command to use as a DelegateCommand.
        /// </summary>
        private ICommand changeTeamCommand;

        /// <summary>
        /// internal refresh list command to use as a DelegateCommand.
        /// </summary>
        private ICommand refreshCommand;

        /// <summary>
        /// Internal sort list command to use as a DelegateCommand.
        /// </summary>
        private ICommand sortListCommand;

        /// <summary>
        /// Internal Event Match Data member
        /// </summary>
        private NotifyTaskCompletion<ObservableCollection<MatchInformation>> eventMatchData { get; set; }

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
        /// Code for the Change Team Command
        /// </summary>
        /// <param name="p">The object passed from the view</param>
        private void ChangeTeam(object p)
        {
            svm.TeamNumber = (string)p;
            CurrentPage.Frame.Navigate(typeof(TeamInfoPage));
        }

        /// <summary>
        /// Refresh List Command Execute
        /// </summary>
        /// <param name="p"></param>
        private void RefreshList(object p)
        {
            EventMatchData = new NotifyTaskCompletion<ObservableCollection<MatchInformation>>(LoadEventMatchData(svm.EventKey));
        }

        /// <summary>
        /// Sort List Command Execute
        /// </summary>
        /// <param name="p"></param>
        private void SortList(object p)
        {
            ChangeSortOrder();
            EventMatchData = new NotifyTaskCompletion<ObservableCollection<MatchInformation>>(SortMatchListAsync(EventMatchData.Result));
        }

        /// <summary>
        /// Change the sort order for the list -- it toggles between ascending and descending every time it is called. Values are persisted in settings.
        /// </summary>
        private void ChangeSortOrder()
        {
            if (svm.EventMatchSortOrder == (int)SortOrder.Descending)
            {
                svm.EventMatchSortOrder = (int)SortOrder.Ascending;
            }
            else
            {
                svm.EventMatchSortOrder = (int)SortOrder.Descending;
            }
        }

        /// <summary>
        /// Sort List work
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private IEnumerable<MatchInformation> SortMatchList(IEnumerable<MatchInformation> list)
        {
            IEnumerable<MatchInformation> sortedlist;

            if (svm.EventMatchSortOrder == (int)SortOrder.Descending)
            {
                // sort descending (competition level, then by match number)
                sortedlist = list.OrderByDescending(m => m.CompetitionLevelSortOrder).ThenByDescending(m => m.match_number).Select(m => m);
            }
            else
            {
                // sort ascending
                sortedlist = list.OrderBy(m => m.CompetitionLevelSortOrder).ThenBy(m => m.match_number).Select(m => m);
            }

            return sortedlist;
        }

        /// <summary>
        /// Function to sort existing event data that can reload the viewmodel... has to be async so that we get a Task in return, just like we do with the Load Data calls
        /// </summary>
        /// <param name="list">The list to sort.</param>
        private async Task<ObservableCollection<MatchInformation>> SortMatchListAsync(IEnumerable<MatchInformation> list)
        {
            // artificially make this function awaitable by waiting .001 seconds.
            await Task.Delay(System.TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

            IEnumerable<MatchInformation> sortedresult = SortMatchList(list);

            return new ObservableCollection<MatchInformation>(sortedresult);
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
        /// <param name="eventkey">The team number to load.</param>
        private async Task<ObservableCollection<MatchInformation>> LoadEventMatchData(string eventkey)
        {
            IEnumerable<MatchInformation> list = await apiClient.EventApi.GetEventMatchList(eventkey);
            IEnumerable<MatchInformation> sortedresult = SortMatchList(list);
            return new ObservableCollection<MatchInformation>(sortedresult);
        }
    }
}
