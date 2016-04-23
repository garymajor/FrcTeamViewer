using FrcTeamViewer.Pages;
using Intense.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TbaApiClient;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FrcTeamViewer.Presentation
{
    abstract public class ViewModelBase : NotifyPropertyChanged
    {
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

        #region public ICommand objects
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
        /// View Team Command
        /// </summary>
        public ICommand ViewTeamCommand
        {
            get
            {
                return viewTeamCommand;
            }
        }
        #endregion

        public ViewModelBase()
        {
            changeEventCommand = new DelegateCommand(ChangeEvent);
            changeTeamCommand = new DelegateCommand(ChangeTeam);
            refreshCommand = new DelegateCommand(RefreshList);
            viewTeamCommand = new DelegateCommand(ViewTeam);
            svm = new SettingsViewModel();
            apiClient = new ApiClient(svm.LocalStorage);
        }

        /// <summary>
        /// Internal TBA API Client
        /// </summary>
        protected ApiClient apiClient { get; set; }

        /// <summary>
        /// Internal copy of the app settings
        /// </summary>
        protected SettingsViewModel svm { get; set; }

        /// <summary>
        /// Internal Page Width member
        /// </summary>
        protected double pageWidth { get; set; }

        #region protected ICommand objects
        /// <summary>
        /// Internal change event command to use as a DelegateCommand.
        /// </summary>
        protected ICommand changeEventCommand;

        /// <summary>
        /// Internal change team command to use as a DelegateCommand.
        /// </summary>
        protected ICommand changeTeamCommand;

        /// <summary>
        /// Internal refresh command to use as a DelegateCommand.
        /// </summary>
        protected ICommand refreshCommand;

        /// <summary>
        /// Internal view team command to use as a DelegateCommand.
        /// </summary>
        protected ICommand viewTeamCommand;
        #endregion

        #region virtual Command code
        /// <summary>
        /// Code for the Change Event Command
        /// </summary>
        /// <param name="p">The object passed from the view</param>
        protected virtual void ChangeEvent(object p)
        {
            CurrentPage.Frame.Navigate(typeof(TeamInfoPage));
        }

        /// <summary>
        /// Code for the Change Team Command
        /// </summary>
        /// <param name="p">The object passed from the view</param>
        protected virtual void ChangeTeam(object p)
        {
            svm.TeamNumber = (string)p;
            CurrentPage.Frame.Navigate(typeof(TeamInfoPage));
        }

        /// <summary>
        /// Code for the Refresh Command
        /// </summary>
        /// <param name="p"></param>
        protected virtual void RefreshList(object p)
        {
        }

        /// <summary>
        /// Code for the View Team Command
        /// </summary>
        /// <param name="p">The object passed from the view</param>
        protected virtual void ViewTeam(object p)
        {
            CurrentPage.Frame.Navigate(typeof(TeamInfoPage));
        }
        #endregion
    }
}
