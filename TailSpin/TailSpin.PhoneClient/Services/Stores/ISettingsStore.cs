//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Services.Stores
{
    public interface ISettingsStore
    {
        string Password { get; set; }
        string UserName { get; set; }
        bool SubscribeToPushNotifications { get; set; }
        bool LocationServiceAllowed { get; set; }
    }
}