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
    using Microsoft.Phone.Reactive;
    using PhoneClient.Services;

    public class SurveysSynchronizationServiceMock : ISurveysSynchronizationService
    {
        public IObservable<TaskCompletedSummary[]> StartSynchronization()
        {
            return Observable.Empty<TaskCompletedSummary[]>();
        }
    }
}