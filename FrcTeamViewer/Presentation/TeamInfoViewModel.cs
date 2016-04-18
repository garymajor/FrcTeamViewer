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
    public class TeamInfoViewModel : NotifyPropertyChanged
    {
        /// <summary>
        /// The Team Information, using the given team number -- loaded async
        /// </summary>
        public NotifyTaskCompletion<TeamInformation> TeamData { get; private set; }

        /// <summary>
        /// The Team Event list, using the given team number -- loaded async
        /// </summary>
        public NotifyTaskCompletion<ObservableCollection<EventInformation>> TeamEventData
        {
            get
            {
                return teamEventData;
            }
            set
            {
                teamEventData = value;
                OnPropertyChanged("TeamEventData");
            }
        }

        /// <summary>
        /// The Team District Key, using the given team number -- loaded async
        /// </summary>
        public NotifyTaskCompletion<string> DistrictKey
        {
            get
            {
                return districtKey;
            }
            private set
            {
                districtKey = value;
                OnPropertyChanged("DistrictKey");
            }
        }

        /// <summary>
        /// Returns whether team is in a district (will return false until data is returned)
        /// </summary>
        public bool IsTeamInDistrict
        {
            get
            {
                return isTeamInDistrict;
            }
            private set
            {
                isTeamInDistrict = value;
                OnPropertyChanged("IsTeamInDistrict");
            }
        }

        /// <summary>
        /// The current page
        /// </summary>
        public Page CurrentPage { get; set; } // gets set by the Page when it initializes

        /// <summary>
        /// Internal Team Event list
        /// </summary>
        private NotifyTaskCompletion<ObservableCollection<EventInformation>> teamEventData { get; set; }

        /// <summary>
        /// Internal change settings command to use as a DelegateCommand
        /// </summary>
        private ICommand changeSettingsCommand;

        /// <summary>
        /// Internal show district ranking command to use as a DelegateCommand
        /// </summary>
        private ICommand showDistrictRankingCommand;

        /// <summary>
        /// Internal show event awards command to use as a DelegateCommand
        /// </summary>
        private ICommand showEventAwardsCommand;

        /// <summary>
        /// Internal show event matches command to use as a DelegateCommand
        /// </summary>
        private ICommand showEventMatchesCommand;

        /// <summary>
        /// Internal show event teams command to use as a DelegateCommand
        /// </summary>
        private ICommand showEventTeamsCommand;

        /// <summary>
        /// Internal show event ranking command to use as a DelegateCommand
        /// </summary>
        private ICommand showEventRankingCommand;

        /// <summary>
        /// Internal show team matches command to use as a DelegateCommand
        /// </summary>
        private ICommand showTeamMatchesCommand;

        /// <summary>
        /// Internal sort list command to use as a DelegateCommand.
        /// </summary>
        private ICommand sortListCommand;

        /// <summary>
        /// Internal settings viewmodel to get/store app state that we want to persist from run-to-run
        /// </summary>
        private SettingsViewModel svm { get; set; }

        /// <summary>
        /// Internal TBA API Client
        /// </summary>
        private ApiClient apiClient { get; set; }

        /// <summary>
        /// Change Settings Command
        /// </summary>
        public ICommand ChangeSettingsCommand
        {
            get
            {
                return changeSettingsCommand;
            }
        }

        /// <summary>
        /// Show District Ranking Command
        /// </summary>
        public ICommand ShowDistrictRankingCommand
        {
            get
            {
                return showDistrictRankingCommand;
            }
        }

        /// <summary>
        /// Show Event Awards Command
        /// </summary>
        public ICommand ShowEventAwardsCommand
        {
            get
            {
                return showEventAwardsCommand;
            }
        }

        /// <summary>
        /// Show Event Matches Command
        /// </summary>
        public ICommand ShowEventMatchesCommand
        {
            get
            {
                return showEventMatchesCommand;
            }
        }

        /// <summary>
        /// Show Event Ranking Command
        /// </summary>
        public ICommand ShowEventRankingCommand
        {
            get
            {
                return showEventRankingCommand;
            }
        }

        /// <summary>
        /// Show Event Teams Command
        /// </summary>
        public ICommand ShowEventTeamsCommand
        {
            get
            {
                return showEventTeamsCommand;
            }
        }

        /// <summary>
        /// Show Team Matches Command
        /// </summary>
        public ICommand ShowTeamMatchesCommand
        {
            get
            {
                return showTeamMatchesCommand;
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
        /// Constructor
        /// </summary>
        public TeamInfoViewModel()
        {
            IsTeamInDistrict = false; // will be changed in LoadDistrictKey if the team is in a district
            changeSettingsCommand = new DelegateCommand(ChangeSettings);
            showDistrictRankingCommand = new DelegateCommand(ShowDistrictRanking);
            showEventAwardsCommand = new DelegateCommand(ShowEventAwards);
            showEventMatchesCommand = new DelegateCommand(ShowEventMatches);
            showEventRankingCommand = new DelegateCommand(ShowEventRanking);
            showEventTeamsCommand = new DelegateCommand(ShowEventTeams);
            showTeamMatchesCommand = new DelegateCommand(ShowTeamMatches);
            sortListCommand = new DelegateCommand(SortList);
            svm = new SettingsViewModel();
            apiClient = new ApiClient();
            TeamData = new NotifyTaskCompletion<TeamInformation>(LoadTeamData(svm.TeamNumber));
            TeamEventData = new NotifyTaskCompletion<ObservableCollection<EventInformation>>(LoadTeamEventData(svm.TeamNumber));
            DistrictKey = new NotifyTaskCompletion<string>(LoadDistrictKey(svm.TeamNumber));
        }

        /// <summary>
        /// Internal district key
        /// </summary>
        private NotifyTaskCompletion<string> districtKey { get; set; }

        /// <summary>
        /// Internal indicator - true if the team is in a district, false if they are not. Only set true by LoadDistrictKey.
        /// </summary>
        private bool isTeamInDistrict { get; set; }

        /// <summary>
        /// Change Settings Command implementation
        /// </summary>
        /// <param name="p">The Page</param>
        private void ChangeSettings(object p)
        {
            ((TeamInfoPage)p).Frame.Navigate(typeof(SettingsPage));
        }

        /// <summary>
        /// Show Event Ranking Command implementation
        /// </summary>
        /// <param name="p"></param>
        private void ShowDistrictRanking(object p)
        {
            CurrentPage.Frame.Navigate(typeof(DistrictRankingPage));
        }

        /// <summary>
        /// Show Event Awards Command implementation
        /// </summary>
        /// <param name="p"></param>
        private void ShowEventAwards(object p)
        {
            var competitionevent = ((EventInformation)((HyperlinkButton)p).DataContext).key;
            svm.EventKey = competitionevent;
            CurrentPage.Frame.Navigate(typeof(EventAwardsPage));
        }

        /// <summary>
        /// Show Event Matches Command implementation
        /// </summary>
        /// <param name="p"></param>
        private void ShowEventMatches(object p)
        {
            var competitionevent = ((EventInformation)((HyperlinkButton)p).DataContext).key;
            svm.EventKey = competitionevent;
            CurrentPage.Frame.Navigate(typeof(EventMatchPage));
        }

        /// <summary>
        /// Show Event Awards Command implementation
        /// </summary>
        /// <param name="p"></param>
        private void ShowEventTeams(object p)
        {
            // Store the eventkey from the context of the clicked button...
            var competitionevent = ((EventInformation)((HyperlinkButton)p).DataContext).key;
            svm.EventKey = competitionevent;
            CurrentPage.Frame.Navigate(typeof(EventTeamsPage));
        }

        /// <summary>
        /// Show Event Ranking Command implementation
        /// </summary>
        /// <param name="p"></param>
        private void ShowEventRanking(object p)
        {
            // Store the eventkey from the context of the clicked button...
            var competitionevent = ((EventInformation)((HyperlinkButton)p).DataContext).key;
            svm.EventKey = competitionevent;
            CurrentPage.Frame.Navigate(typeof(EventRankingPage));
        }

        /// <summary>
        /// Show Team Matches Command implementation
        /// </summary>
        /// <param name="p"></param>
        private void ShowTeamMatches(object p)
        {
            // Store the eventkey from the context of the clicked button...
            var competitionevent = ((EventInformation)((HyperlinkButton)p).DataContext).key;
            svm.EventKey = competitionevent;
            CurrentPage.Frame.Navigate(typeof(TeamMatchPage));
        }

        /// <summary>
        /// Sort List Command Execute
        /// </summary>
        /// <param name="p"></param>
        private void SortList(object p)
        {
            ChangeSortOrder();
            TeamEventData = new NotifyTaskCompletion<ObservableCollection<EventInformation>>(SortEventListAsync(TeamEventData.Result));
        }

        /// <summary>
        /// Change the sort order for the list -- it toggles between ascending and descending every time it is called.
        /// </summary>
        private void ChangeSortOrder()
        {
            if (svm.TeamEventSortOrder == int.MinValue)
            {
                // we are sorted descending... sort ascending
                svm.TeamEventSortOrder = int.MaxValue;
            }
            else
            {
                // we are sorted ascending... sort descending
                svm.TeamEventSortOrder = int.MinValue;
            }
        }

        /// <summary>
        /// Sort List work
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private IEnumerable<EventInformation> SortEventList(IEnumerable<EventInformation> list)
        {
            IEnumerable<EventInformation> sortedlist;

            if (svm.TeamEventSortOrder == int.MinValue)
            {
                // sort descending (competition level, then by match number)
                sortedlist = list.OrderByDescending(teamevent => teamevent.start_date).Select(teamevent => teamevent);
            }
            else
            {
                // sort ascending
                sortedlist = list.OrderBy(teamevent => teamevent.start_date).Select(teamevent => teamevent);
            }

            return sortedlist;
        }

        /// <summary>
        /// Function to sort existing event data that can reload the viewmodel... has to be async so that we get a Task in return, just like we do with the Load Data calls
        /// </summary>
        /// <param name="list">The list to sort.</param>
        private async Task<ObservableCollection<EventInformation>> SortEventListAsync(IEnumerable<EventInformation> list)
        {
            // artificially make this function awaitable by waiting .001 seconds.
            await Task.Delay(System.TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

            IEnumerable<EventInformation> sortedresult = SortEventList(list);

            return new ObservableCollection<EventInformation>(sortedresult);
        }

        /// <summary>
        /// Function to load the team data into the View Model.
        /// </summary>
        /// <param name="teamnumber">The team number to load.</param>
        private async Task<TeamInformation> LoadTeamData(string teamnumber)
        {
            TeamInformation ti = await apiClient.TeamApi.GetTeamInfo(teamnumber);
            return ti;
        }

        /// <summary>
        /// Function to load the event data into the View Model.
        /// </summary>
        /// <param name="teamnumber">The team number to load.</param>
        private async Task<ObservableCollection<EventInformation>> LoadTeamEventData(string teamnumber)
        {
            List<EventInformation> tei = await apiClient.TeamApi.GetTeamEventInfoList(teamnumber);

            // Sort the events before we return them.
            var sortedresult = SortEventList(tei);
            return new ObservableCollection<EventInformation>(sortedresult);
        }

        /// <summary>
        /// Function to load the district code (e.g., pnw) for the team into the view model.
        /// </summary>
        /// <param name="teamnumber">The team number</param>
        /// <returns>district code or string.Empty</returns>
        private async Task<string> LoadDistrictKey(string teamnumber)
        {
            var s = await apiClient.TeamApi.GetTeamDistrict(teamnumber);
            IsTeamInDistrict = (s.CompareTo(string.Empty) == 0) ? false : true;
            svm.DistrictKey = s;
            return s;
        }
    }
}
