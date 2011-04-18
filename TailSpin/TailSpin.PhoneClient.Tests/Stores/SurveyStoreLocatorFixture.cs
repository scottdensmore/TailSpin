//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Tests.Stores
{
    using Microsoft.Silverlight.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TailSpin.PhoneClient.Services.Stores;
    using TailSpin.PhoneClient.Tests.ViewModels.Mocks;

    [Tag("Store")]
    [TestClass]
    public class SurveyStoreLocatorFixture
    {
        [TestMethod]
        public void ReturnEmptySurveyStoreWhenUsernameIsEmpty()
        {
            ISettingsStore settingsStore = new MockSettingsStore();
            ISurveyStoreLocator surveyStoreLocator = new SurveyStoreLocator(settingsStore, storeName => new SurveyStoreMock());

            settingsStore.UserName = string.Empty;
            var surveyStore = surveyStoreLocator.GetStore();

            Assert.IsInstanceOfType(surveyStore, typeof(NullSurveyStore));
        }

        [TestMethod]
        public void ReturnSameStoreWhenUsernameDidNotChange()
        {
            ISettingsStore settingsStore = new MockSettingsStore();
            ISurveyStoreLocator surveyStoreLocator = new SurveyStoreLocator(settingsStore, storeName => new SurveyStoreMock());

            settingsStore.UserName = "username";
            var surveyStore = surveyStoreLocator.GetStore();
            var storeWithoutSettingsChanged = surveyStoreLocator.GetStore();

            Assert.AreSame(surveyStore, storeWithoutSettingsChanged);
        }

        [TestMethod]
        public void ReturnNewStoreWhenUsernameChanged()
        {
            ISettingsStore settingsStore = new MockSettingsStore();
            ISurveyStoreLocator surveyStoreLocator = new SurveyStoreLocator(settingsStore, storeName => new SurveyStoreMock());

            settingsStore.UserName = "username";
            var storeBeforeSettingsChanged = surveyStoreLocator.GetStore();
            settingsStore.UserName = "username changed";
            var storeAfterSettingsChanged = surveyStoreLocator.GetStore();

            Assert.AreNotSame(storeBeforeSettingsChanged, storeAfterSettingsChanged);
        }
    }
}
