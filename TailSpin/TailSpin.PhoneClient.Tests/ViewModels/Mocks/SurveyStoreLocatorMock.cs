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

    public class SurveyStoreLocatorMock : ISurveyStoreLocator
    {
        private readonly ISurveyStore surveyStoreToReturn;

        public SurveyStoreLocatorMock(ISurveyStore surveyStoreToReturn)
        {
            this.surveyStoreToReturn = surveyStoreToReturn;
        }

        public ISurveyStore GetStore()
        {
            return this.surveyStoreToReturn;
        }
    }
}