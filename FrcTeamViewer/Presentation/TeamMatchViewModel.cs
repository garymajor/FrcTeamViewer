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
    public class TeamMatchViewModel : NotifyPropertyChanged
    {
        /// <summary>
        /// The EventInformation object that will hold the event information, using the given eventkey
        /// </summary>
        public NotifyTaskCompletion<EventInformation> EventData { get; private set; }

        /// <summary>
        /// The TeamMatchInformation list that hold the list of team matches to display
        /// </summary>
        public NotifyTaskCompletion<ObservableCollection<MatchInformation>> TeamMatchData
        {
            get
            {
                return teamMatchData;
            }
            set
            {
                teamMatchData = value;
                OnPropertyChanged("TeamMatchData");
            }
        }

        /// <summary>
        /// The current page (set in the code behind constructor).
        /// </summary>
        public Page CurrentPage { get; set; }

        /// <summary>
        /// The current team number.
        /// </summary>
        public string CurrentTeamNumber
        {
            get
            {
                return svm.TeamNumber;
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
        public ICommand RefreshListCommand
        {
            get
            {
                return refreshListCommand;
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
        public TeamMatchViewModel()
        {
            changeEventCommand = new DelegateCommand(ChangeEvent);
            changeTeamCommand = new DelegateCommand(ChangeTeam);
            refreshListCommand = new DelegateCommand(RefreshList);
            sortListCommand = new DelegateCommand(SortList);
            svm = new SettingsViewModel();
            competitionevent = new Event();
            competitionteam = new Team();
            EventData = new NotifyTaskCompletion<EventInformation>(LoadEventData(svm.EventKey));
            TeamMatchData = new NotifyTaskCompletion<ObservableCollection<MatchInformation>>(LoadEventMatchData(svm.TeamNumber, svm.EventKey));
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
        private ICommand refreshListCommand;

        /// <summary>
        /// Internal sort list command to use as a DelegateCommand.
        /// </summary>
        private ICommand sortListCommand;

        /// <summary>
        /// Internal Event Match Data member
        /// </summary>
        private NotifyTaskCompletion<ObservableCollection<MatchInformation>> teamMatchData { get; set; }

        /// <summary>
        /// Internal copy of the app settings
        /// </summary>
        private SettingsViewModel svm { get; set; }

        /// <summary>
        /// Internal event key member
        /// </summary>
        private Event competitionevent { get; set; }

        /// <summary>
        /// Internal team member
        /// </summary>
        private Team competitionteam { get; set; }

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
            TeamMatchData = new NotifyTaskCompletion<ObservableCollection<MatchInformation>>(LoadEventMatchData(svm.TeamNumber, svm.EventKey));
        }

        /// <summary>
        /// Sort List Command Execute
        /// </summary>
        /// <param name="p"></param>
        private void SortList(object p)
        {
            ChangeSortOrder();
            TeamMatchData = new NotifyTaskCompletion<ObservableCollection<MatchInformation>>(SortMatchListAsync(TeamMatchData.Result));
        }

        /// <summary>
        /// Change the sort order for the list -- it toggles between ascending and descending every time it is called.
        /// </summary>
        private void ChangeSortOrder()
        {
            if (svm.TeamMatchSortOrder == int.MinValue)
            {
                // we are sorted descending... sort ascending
                svm.TeamMatchSortOrder = int.MaxValue;
            }
            else
            {
                // we are sorted ascending... sort descending
                svm.TeamMatchSortOrder = int.MinValue;
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

            if (svm.TeamMatchSortOrder == int.MinValue)
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
            EventInformation ei = await competitionevent.GetEventInfo(eventkey);
            return ei;
        }

        /// <summary>
        /// Function to load team event match data into the viewmodel.
        /// </summary>
        /// <param name="teamnumber">The team number.</param>
        /// <param name="eventkey">The event.</param>
        private async Task<ObservableCollection<MatchInformation>> LoadEventMatchData(string teamnumber, string eventkey)
        {
            IEnumerable<MatchInformation> list = await competitionteam.GetTeamEventMatchList(teamnumber, eventkey);
            IEnumerable<MatchInformation> sortedresult = SortMatchList(list);
            return new ObservableCollection<MatchInformation>(sortedresult);
        }
    }
}
