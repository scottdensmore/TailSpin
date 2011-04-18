//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Tests.ViewModels.Mocks
{
    using System;
    using TailSpin.PhoneClient.Services;

    public class MockNavigationService : INavigationService
    {
        public MockNavigationService()
        {
            this.CanGoBack = true;
            this.CurrentSource = null;
            this.NavigateCallback = s => true;
            this.GoBackCallback = () => { };
        }

        public Func<Uri, bool> NavigateCallback { get; set; }

        public Action GoBackCallback { get; set; }

        public bool CanGoBack { get; set; }

        public Uri CurrentSource { get; set; }

        public bool Navigate(Uri source)
        {
            return this.NavigateCallback(source);
        }

        public void GoBack()
        {
            this.GoBackCallback();
        }
    }
}
