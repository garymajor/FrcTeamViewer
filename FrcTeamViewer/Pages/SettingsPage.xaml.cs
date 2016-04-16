using FrcTeamViewer.Presentation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

namespace FrcTeamViewer.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private SettingsViewModel svm;

        public SettingsPage()
        {
            svm = new SettingsViewModel();
            InitializeComponent();
            svm.CurrentPage = this;
        }

        private void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                //We have to force the text to change before we call command.execute -- otherwise, we will get the old team value.
                svm.TeamNumber = ((TextBox)sender).Text;
                svm.ViewTeamInfoCommand.Execute(btnViewTeam);
            }
        }

        private void pageSettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            txtTeamNumber.Focus(FocusState.Keyboard);
            txtTeamNumber.SelectAll();
        }
    }
}
