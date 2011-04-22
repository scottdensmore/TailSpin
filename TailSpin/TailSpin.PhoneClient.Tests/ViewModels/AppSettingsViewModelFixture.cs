namespace TailSpin.PhoneClient.Tests.ViewModels
{
    using Microsoft.Silverlight.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TailSpin.PhoneClient.Tests.ViewModels.Mocks;
    using TailSpin.PhoneClient.ViewModels;

    [TestClass]
    [Tag("AppSettingsViewModelFixture")]
    public class AppSettingsViewModelFixture
    {
        [TestMethod]
        public void SettingUserNameAndPasswordDoesNotWriteToStorage()
        {
            var settingsStore = new MockSettingsStore();
            var registrationClient = new MockRegistrationService();
            var appSetttingsViewModel = new AppSettingsViewModel(settingsStore, registrationClient, new MockNavigationService());

            appSetttingsViewModel.UserName = "username";
            appSetttingsViewModel.Password = "password";

            Assert.AreEqual(null, settingsStore.UserName);
            Assert.AreEqual(null, settingsStore.Password);
        }

        [TestMethod]
        public void ExecutingSubmitCommandCommitsUserNameAndPasswordToStorage()
        {
            var settingsStore = new MockSettingsStore();
            var registrationClient = new MockRegistrationService();
            var appSetttingsViewModel = new AppSettingsViewModel(settingsStore, registrationClient, new MockNavigationService());
            appSetttingsViewModel.UserName = "username";
            appSetttingsViewModel.Password = "password";

            appSetttingsViewModel.Submit();

            Assert.AreEqual("username", settingsStore.UserName);
            Assert.AreEqual("password", settingsStore.Password);
        }

        [TestMethod]
        public void WhenCreatedViewModelLoadsFromStore()
        {
            var settingsStore = new MockSettingsStore();
            var registrationClient = new MockRegistrationService();
            settingsStore.UserName = "username";
            settingsStore.Password = "password";

            var appSetttingsViewModel = new AppSettingsViewModel(settingsStore, registrationClient, new MockNavigationService());

            Assert.AreEqual("username", appSetttingsViewModel.UserName);
            Assert.AreEqual("password", appSetttingsViewModel.Password);
        }

        [TestMethod]
        public void IfUserNameNotEnteredThenCanSubmitFalse()
        {
            var settingsStore = new MockSettingsStore();

            var appSetttingsViewModel = new AppSettingsViewModel(settingsStore, new MockRegistrationService(), new MockNavigationService());

            Assert.IsFalse(appSetttingsViewModel.CanSubmit);
        }

        [TestMethod]
        public void IfUserNameEnteredThenCanSubmitFalse()
        {
            var settingsStore = new MockSettingsStore();
            var appSetttingsViewModel = new AppSettingsViewModel(settingsStore, new MockRegistrationService(), new MockNavigationService());

            appSetttingsViewModel.UserName = "testUser";

            Assert.IsFalse(appSetttingsViewModel.CanSubmit);
        }

        [TestMethod]
        public void IfUserNameAndPasswordEnteredThenCanSubmit()
        {
            var settingsStore = new MockSettingsStore();
            var appSetttingsViewModel = new AppSettingsViewModel(settingsStore, new MockRegistrationService(), new MockNavigationService());

            appSetttingsViewModel.UserName = "testUser";
            appSetttingsViewModel.Password = "testPassword";

            Assert.IsTrue(appSetttingsViewModel.CanSubmit);
        }
    }
}