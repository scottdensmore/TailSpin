//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Views.SurveyList
{
    using TailSpin.PhoneClient.ViewModels;

    public partial class SurveyListView
    {
        // Constructor
        public SurveyListView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            ((SurveyListViewModel)this.DataContext).Refresh();
        }
    }
}
