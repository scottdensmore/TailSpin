namespace TailSpin.PhoneClient.Tests
{
    using Microsoft.Phone.Controls;
    using Microsoft.Silverlight.Testing;

    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            this.InitializeComponent();
            Content = UnitTestSystem.CreateTestPage();
            BackKeyPress += (x, xe) => xe.Cancel = (Content as IMobileTestPage).NavigateBack();
        }
    }
}