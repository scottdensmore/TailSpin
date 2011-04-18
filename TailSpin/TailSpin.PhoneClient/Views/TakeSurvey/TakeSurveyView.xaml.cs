//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Views.TakeSurvey
{
    using TailSpin.PhoneClient.ViewModels;

    public partial class TakeSurveyView
    {
        private bool loaded;

        // Constructor
        public TakeSurveyView()
        {
            InitializeComponent();
        }

        private static string ProcessTitle(string title)
        {
            // Trim the title if it's too long and add ellipsis in that case.
            if (title.Length > 20)
            {
                title = title.Substring(0, 20) + "...";
            }
            return title;
        }

        private void PanoramaSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.loaded)
            {
                ((TakeSurveyViewModel)this.DataContext).SelectedPanoramaIndex = this.panorama.SelectedIndex;
            }
        }

        private void PanoramaLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.panorama.DefaultItem = this.panorama.Items[((TakeSurveyViewModel)this.DataContext).SelectedPanoramaIndex];
            this.loaded = true;
            this.panorama.Title = ProcessTitle(((TakeSurveyViewModel)this.DataContext).TemplateViewModel.Title);
        }
    }
}
