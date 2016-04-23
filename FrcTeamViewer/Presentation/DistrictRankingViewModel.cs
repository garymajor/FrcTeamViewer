using FrcTeamViewer.Pages;
using Intense.Presentation;
using TbaApiClient;
using TbaApiClient.DataModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using System.ComponentModel;

namespace FrcTeamViewer.Presentation
{
    public class DistrictRankingViewModel : ViewModelBase
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
        /// Constructor - Note: Initiates async loading of data for the view
        /// </summary>
        public DistrictRankingViewModel()
        {
            // reset this because we overrode it.
            refreshCommand = new DelegateCommand(RefreshList);

            // Load our data
            DistrictName = new NotifyTaskCompletion<string>(LoadDistrictName(svm.DistrictKey));
            DistrictRankingData = new NotifyTaskCompletion<ObservableCollection<DistrictRankingInformation>>(LoadDistrictRankingData(svm.DistrictKey));
        }

        /// <summary>
        /// Internal event ranking data
        /// </summary>
        private NotifyTaskCompletion<ObservableCollection<DistrictRankingInformation>> districtRankingData { get; set; }

        /// <summary>
        /// Code for the Refresh Command
        /// </summary>
        /// <param name="p"></param>
        protected override void RefreshList(object p)
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
