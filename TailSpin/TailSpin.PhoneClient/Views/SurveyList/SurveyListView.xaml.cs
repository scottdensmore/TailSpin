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
