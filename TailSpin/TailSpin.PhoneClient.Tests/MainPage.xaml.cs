//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


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