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
    using TailSpin.PhoneClient.Services.Stores;

    public class MockSettingsStore : ISettingsStore
    {
        public string Password { get; set; }
        public string PushNotificationUri { get; set; }
        public bool SubscribeToPushNotifications { get; set; }
        public string UserName { get; set; }
        public bool LocationServiceAllowed { get; set; }
    }
}