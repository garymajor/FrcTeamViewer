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
        public NotifyTaskCompletion<ObservableCollection<EventInformation>> TeamEventData { get; private set; }

        /// <summary>
        /// The current page
        /// </summary>
        public Page CurrentPage { get; set; } // gets set by the Page when it initializes

        /// <summary>
        /// Internal change settings command to use as a DelegateCommand
        /// </summary>
        private ICommand changeSettingsCommand;

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
        /// Internal settings viewmodel to get/store app state that we want to persist from run-to-run
        /// </summary>
        private SettingsViewModel svm { get; set; }

        /// <summary>
        /// Internal Team object (from TbaApiClient)
        /// </summary>
        private Team team { get; set; }

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
        /// Constructor
        /// </summary>
        public TeamInfoViewModel()
        {
            changeSettingsCommand = new DelegateCommand(ChangeSettings);
            showEventAwardsCommand = new DelegateCommand(ShowEventAwards);
            showEventMatchesCommand = new DelegateCommand(ShowEventMatches);
            showEventRankingCommand = new DelegateCommand(ShowEventRanking);
            showEventTeamsCommand = new DelegateCommand(ShowEventTeams);
            showTeamMatchesCommand = new DelegateCommand(ShowTeamMatches);
            svm = new SettingsViewModel();
            team = new Team();
            TeamData = new NotifyTaskCompletion<TeamInformation>(LoadTeamData(svm.TeamNumber));
            TeamEventData = new NotifyTaskCompletion<ObservableCollection<EventInformation>>(LoadTeamEventData(svm.TeamNumber));
        }

        /// <summary>
        /// Change Settings Command implementation
        /// </summary>
        /// <param name="p">The Page</param>
        private void ChangeSettings(object p)
        {
            ((TeamInfoPage)p).Frame.Navigate(typeof(SettingsPage));
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
        /// Function to load the team data into the View Model.
        /// </summary>
        /// <param name="teamnumber">The team number to load.</param>
        private async Task<TeamInformation> LoadTeamData(string teamnumber)
        {
            TeamInformation ti = await team.GetTeamInfo(teamnumber);
            return ti;
        }

        /// <summary>
        /// Function to load the event data into the View Model.
        /// </summary>
        /// <param name="teamnumber">The team number to load.</param>
        private async Task<ObservableCollection<EventInformation>> LoadTeamEventData(string teamnumber)
        {
            List<EventInformation> tei = await team.GetTeamEventInfoList(teamnumber);

            // Sort the events before we return them.
            var sortedresult = tei.OrderBy(teamevent => teamevent.start_date).Select(teamevent => teamevent);
            return new ObservableCollection<EventInformation>(sortedresult);
        }
    }
}
