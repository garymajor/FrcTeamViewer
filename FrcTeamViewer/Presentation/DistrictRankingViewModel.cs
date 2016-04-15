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
    public class DistrictRankingViewModel : NotifyPropertyChanged
    {
        /// <summary>
        /// The district name
        /// </summary>
        public NotifyTaskCompletion<string> DistrictName { get; private set; }

        /// <summary>
        /// The DistrictRankingInformation list
        /// </summary>
        public NotifyTaskCompletion<ObservableCollection<DistrictRankingInformation>> DistrictRankingData
        {
            get
            {
                return districtRankingData;
            }
            private set
            {
                districtRankingData = value;
                OnPropertyChanged("DistrictRankingData");
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
        public ICommand ChangeTeamCommand
        {
            get
            {
                return changeTeamCommand;
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
        public DistrictRankingViewModel()
        {
            changeTeamCommand = new DelegateCommand(ChangeTeam);
            refreshCommand = new DelegateCommand(RefreshList);
            svm = new SettingsViewModel();
            apiClient = new ApiClient();
            DistrictName = new NotifyTaskCompletion<string>(LoadDistrictName(svm.DistrictKey));
            DistrictRankingData = new NotifyTaskCompletion<ObservableCollection<DistrictRankingInformation>>(LoadDistrictRankingData(svm.DistrictKey));
        }

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
        private NotifyTaskCompletion<ObservableCollection<DistrictRankingInformation>> districtRankingData { get; set; }

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
            DistrictRankingData = new NotifyTaskCompletion<ObservableCollection<DistrictRankingInformation>>(LoadDistrictRankingData(svm.DistrictKey));
        }

        /// <summary>
        /// Function to load district name into the viewmodel.
        /// </summary>
        /// <param name="district">The district key (e.g., pnw)</param>
        private async Task<string> LoadDistrictName(string district)
        {
            var s = await apiClient.DistrictApi.GetDistrictName(district);
            return s;
        }

        /// <summary>
        /// Function to load district ranking data into the viewmodel.
        /// </summary>
        /// <param name="district">The district (e.g., pnw)</param>
        private async Task<ObservableCollection<DistrictRankingInformation>> LoadDistrictRankingData(string district)
        {
            List<DistrictRankingInformation> dri = await apiClient.DistrictApi.GetDistrictRankingList(district);

            // Don't need to sort this one - the api returns the data in the correct order.
            return new ObservableCollection<DistrictRankingInformation>(dri);
        }
    }
}
