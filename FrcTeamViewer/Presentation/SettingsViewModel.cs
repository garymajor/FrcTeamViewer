using FrcTeamViewer.Pages;
using Intense.Presentation;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace FrcTeamViewer.Presentation
{
    public class SettingsViewModel : NotifyPropertyChanged
    {
        /// <summary>
        /// The team number to persist.
        /// </summary>
        public string TeamNumber
        {
            get
            {
                return teamNumber;
            }
            set
            {
                teamNumber = value;
                OnPropertyChanged("TeamNumber");
                StoreTeamNumber();
            }
        }

        /// <summary>
        /// The district key to persist.
        /// </summary>
        public string DistrictKey
        {
            get
            {
                return districtKey;
            }
            set
            {
                districtKey = value;
                OnPropertyChanged("DistrictKey");
                StoreDistrictKey();
            }
        }


        /// <summary>
        /// The event key to persist.
        /// </summary>
        public string EventKey
        {
            get
            {
                return eventKey;
            }
            set
            {
                eventKey = value;
                OnPropertyChanged("EventKey");
                StoreEventKey();
            }
        }

        /// <summary>
        /// The event match sort setting to persist.
        /// </summary>
        public int EventMatchSortOrder
        {
            get
            {
                return eventMatchSortOrder;
            }
            set
            {
                eventMatchSortOrder = value;
                StoreEventMatchSortOrder();
            }
        }

        /// <summary>
        /// The team match sort setting to persist.
        /// </summary>
        public int TeamMatchSortOrder
        {
            get
            {
                return teamMatchSortOrder;
            }
            set
            {
                teamMatchSortOrder = value;
                StoreTeamMatchSortOrder();
            }
        }

        private string teamNumber { get; set; }
        private string eventKey { get; set; }
        private string districtKey { get; set; }
        private int teamMatchSortOrder { get; set; }
        private int eventMatchSortOrder { get; set; }
        private ApplicationDataContainer localSettings;
        private ICommand viewTeamInfoCommand;
        public Page CurrentPage { get; set; } // gets set by the Page when it initializes

        public ICommand ViewTeamInfoCommand
        {
            get
            {
                return viewTeamInfoCommand;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SettingsViewModel()
        {
            viewTeamInfoCommand = new DelegateCommand(ViewTeamInfo);
            localSettings = ApplicationData.Current.LocalSettings;
            LoadState();
        }

        /// <summary>
        /// View Team Info Command Execute
        /// </summary>
        /// <param name="p"></param>
        private void ViewTeamInfo(object p)
        {
            CurrentPage.Frame.Navigate(typeof(TeamInfoPage));
        }

        /// <summary>
        /// Get the state settings from the local store.
        /// </summary>
        private void LoadState()
        {
            teamNumber = (string)localSettings.Values["TeamNumber"];
            eventKey = (string)localSettings.Values["EventKey"];
            districtKey = (string)localSettings.Values["DistrictKey"];

            if (localSettings.Values.ContainsKey("TeamMatchSortOrder"))
            {
                teamMatchSortOrder = (int)localSettings.Values["TeamMatchSortOrder"];
            }
            else
            {
                // go ahead and set the default sort order (ascending)
                TeamMatchSortOrder = int.MaxValue;
            }

            if (localSettings.Values.ContainsKey("EventMatchSortOrder"))
            {
                eventMatchSortOrder = (int)localSettings.Values["EventMatchSortOrder"];
            }
            else
            {
                // go ahead and set the default sort order (ascending)
                EventMatchSortOrder = int.MaxValue;
            }
        }

        /// <summary>
        /// Store the team number state setting to the local store.
        /// </summary>
        private void StoreTeamNumber()
        {
            localSettings.Values["TeamNumber"] = teamNumber;
        }

        /// <summary>
        /// Store the district key state setting to the local store.
        /// </summary>
        private void StoreDistrictKey()
        {
            localSettings.Values["DistrictKey"] = districtKey;
        }

        /// <summary>
        /// Store the event key state setting to the local store.
        /// </summary>
        private void StoreEventKey()
        {
            localSettings.Values["EventKey"] = eventKey;
        }

        /// <summary>
        /// Store the event match sort order state setting to the local store.
        /// </summary>
        private void StoreEventMatchSortOrder()
        {
            localSettings.Values["EventMatchSortOrder"] = eventMatchSortOrder;
        }

        /// <summary>
        /// Store the event match sort order state setting to the local store.
        /// </summary>
        private void StoreTeamMatchSortOrder()
        {
            localSettings.Values["TeamMatchSortOrder"] = teamMatchSortOrder;
        }
    }
}
