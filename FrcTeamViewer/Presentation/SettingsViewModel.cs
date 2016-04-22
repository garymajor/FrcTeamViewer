using FrcTeamViewer.Pages;
using Intense.Presentation;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml;
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
        public int TeamEventSortOrder
        {
            get
            {
                return teamEventSortOrder;
            }
            set
            {
                teamEventSortOrder = value;
                StoreTeamEventSortOrder();
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

        /// <summary>
        /// The application theme (dark/light) setting to persist. When changed, immediately sets the them on the app shell.
        /// </summary>
        public bool DarkMode
        {
            get
            {
                return darkMode;
            }
            set
            {
                darkMode = value;
                StoreDarkMode();
                SetShellTheme();
            }
        }

        /// <summary>
        /// Application Storage - where we can put the API cache.
        /// </summary>
        public StorageFolder LocalStorage
        {
            get
            {
                return localStorage;
            }
        }

        private string teamNumber { get; set; }
        private string eventKey { get; set; }
        private string districtKey { get; set; }
        private int teamEventSortOrder { get; set; }
        private int teamMatchSortOrder { get; set; }
        private int eventMatchSortOrder { get; set; }
        private bool darkMode { get; set; }
        private ApplicationDataContainer localSettings;
        private StorageFolder localStorage { get; set; }
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
            localStorage = ApplicationData.Current.LocalFolder;
            LoadState();
            SetShellTheme();
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
            TeamNumber = (string)localSettings.Values["TeamNumber"];
            EventKey = (string)localSettings.Values["EventKey"];
            DistrictKey = (string)localSettings.Values["DistrictKey"];

            // Dark Mode
            if (localSettings.Values.ContainsKey("DarkMode"))
            {
                DarkMode = (bool)localSettings.Values["DarkMode"];
            }
            else
            {
                // set the default (off)
                DarkMode = false;
            }

            // Team Event Sort Order
            if (localSettings.Values.ContainsKey("TeamEventSortOrder"))
            {
                TeamEventSortOrder = (int)localSettings.Values["TeamEventSortOrder"];
            }
            else
            {
                // go ahead and set the default sort order (ascending)
                TeamEventSortOrder = (int)SortOrder.Ascending;
            }

            // Team Match Sort Order
            if (localSettings.Values.ContainsKey("TeamMatchSortOrder"))
            {
                TeamMatchSortOrder = (int)localSettings.Values["TeamMatchSortOrder"];
            }
            else
            {
                // go ahead and set the default sort order (ascending)
                TeamMatchSortOrder = (int)SortOrder.Ascending;
            }

            // Event Match Sort Order
            if (localSettings.Values.ContainsKey("EventMatchSortOrder"))
            {
                EventMatchSortOrder = (int)localSettings.Values["EventMatchSortOrder"];
            }
            else
            {
                // go ahead and set the default sort order (ascending)
                EventMatchSortOrder = (int)SortOrder.Ascending;
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
        /// Store the team match sort order state setting to the local store.
        /// </summary>
        private void StoreTeamEventSortOrder()
        {
            localSettings.Values["TeamEventSortOrder"] = teamEventSortOrder;
        }

        /// <summary>
        /// Store the team match sort order state setting to the local store.
        /// </summary>
        private void StoreTeamMatchSortOrder()
        {
            localSettings.Values["TeamMatchSortOrder"] = teamMatchSortOrder;
        }

        /// <summary>
        /// Store the team match sort order state setting to the local store.
        /// </summary>
        private void StoreDarkMode()
        {
            localSettings.Values["DarkMode"] = darkMode;
        }

        /// <summary>
        /// Sets the Theme (dark/light) on the current shell based on the app setting.
        /// </summary>
        public void SetShellTheme()
        {
            var shell = Window.Current.Content as Shell;
            if (darkMode)
            {
                if (shell != null)
                {
                    shell.ShellContentHostObject.RequestedTheme = ElementTheme.Dark;
                }
            }
            else
            {
                if (shell != null)
                {
                    shell.ShellContentHostObject.RequestedTheme = ElementTheme.Light;
                }
            }
        }
    }
}
