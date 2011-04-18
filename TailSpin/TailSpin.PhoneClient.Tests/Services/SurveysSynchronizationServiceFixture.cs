//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Tests.Services
{
    using System;
    using Microsoft.Phone.Reactive;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using PhoneClient.Models;
    using PhoneClient.Services;
    using ViewModels.Mocks;

    [TestClass]
    public class SurveysSynchronizationServiceFixture
    {
        [TestMethod]
        public void StartSynchronizationCallsGetNewSurveys()
        {
            var surveysServiceClientMock = new SurveysServiceClientMock();
            var syncService = new SurveysSynchronizationService(() => surveysServiceClientMock, new SurveyStoreLocatorMock(new SurveyStoreMock()));

            syncService.StartSynchronization();

            Assert.IsTrue(surveysServiceClientMock.GetNewSurveysWasCalled);
        }

        [TestMethod]
        public void StartSynchronizationCallsGetNewSurveysWithLastSyncDate()
        {
            var surveysServiceClientMock = new SurveysServiceClientMock();
            var surveyStoreMock = new SurveyStoreMock();
            var syncService = new SurveysSynchronizationService(() => surveysServiceClientMock, new SurveyStoreLocatorMock(surveyStoreMock));
            surveyStoreMock.LastSyncDate = "last sync date";

            syncService.StartSynchronization();

            Assert.AreEqual("last sync date", surveysServiceClientMock.GetNewSurveysLastSyncDate);
        }

        [TestMethod]
        public void StartSynchronizationAddsSurveys()
        {
            var surveysServiceClientMock = new SurveysServiceClientMock();
            surveysServiceClientMock.SurveysToReturn = new[] { new SurveyTemplate() };
            var surveyStoreMock = new SurveyStoreMock();
            var syncService = new SurveysSynchronizationService(() => surveysServiceClientMock, new SurveyStoreLocatorMock(surveyStoreMock));

            syncService.StartSynchronization()
                .Subscribe(
                    s =>
                        Assert.AreSame(surveysServiceClientMock.SurveysToReturn, surveyStoreMock.SavedSurveys));
        }

        [TestMethod]
        public void StartSynchronizationUpdatesLastSyncDate()
        {
            var surveysServiceClientMock = new SurveysServiceClientMock();
            var maxCreatedOnDate = new DateTime(2010, 1, 1);
            var otherCreatedOnDate = maxCreatedOnDate.AddDays(-1);
            surveysServiceClientMock.SurveysToReturn = new[]
                                                           {
                                                               new SurveyTemplate { CreatedOn = maxCreatedOnDate },
                                                               new SurveyTemplate { CreatedOn = otherCreatedOnDate }
                                                           };
            var surveyStoreMock = new SurveyStoreMock();
            var syncService = new SurveysSynchronizationService(() => surveysServiceClientMock, new SurveyStoreLocatorMock(surveyStoreMock));

            syncService.StartSynchronization()
                .Subscribe(
                    s =>
                        Assert.AreEqual(maxCreatedOnDate.ToString("s"), surveyStoreMock.LastSyncDate));
        }

        [TestMethod]
        public void StartSynchronizationSendsSurveyAnswers()
        {
            var surveysServiceClientMock = new SurveysServiceClientMock();
            var surveyStoreMock = new SurveyStoreMock();
            surveyStoreMock.SurveyAnswers = new[] { new SurveyAnswer() };
            var syncService = new SurveysSynchronizationService(() => surveysServiceClientMock, new SurveyStoreLocatorMock(surveyStoreMock));
            
            syncService.StartSynchronization();

            Assert.AreSame(surveyStoreMock.SurveyAnswers, surveysServiceClientMock.SentSurveyAnswers);
        }
    }
}