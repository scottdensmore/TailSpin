namespace TailSpin.Workers.Notifications.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Notifications;
    using TailSpin.Web.Survey.Shared.QueueMessages;
    using TailSpin.Web.Survey.Shared.Stores;
    using TailSpin.Web.Survey.Shared.SurveysFiltering;

    [TestClass]
    public class NewSurveyNotificationCommandFixture
    {
        [TestMethod]
        public void RunGetsSurveyFromStore()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var command = new NewSurveyNotificationCommand(mockSurveyStore.Object, new Mock<IFilteringService>().Object, null, null);
            var message = new NewSurveyMessage { SlugName = "slug-name", Tenant = "tenant" };

            command.Run(message);

            mockSurveyStore.Verify(s => s.GetSurveyByTenantAndSlugName("tenant", "slug-name", false), Times.Once());
        }

        [TestMethod]
        public void RunGetsUsersForTheSurvey()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var mockFilteringService = new Mock<IFilteringService>();
            var command = new NewSurveyNotificationCommand(mockSurveyStore.Object, mockFilteringService.Object, null, null);
            var survey = new Survey();
            mockSurveyStore.Setup(s => s.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(survey);

            command.Run(new NewSurveyMessage());

            mockFilteringService.Verify(s => s.GetUsersForSurvey(survey), Times.Once());
        }

        [TestMethod]
        public void RunGetsTheDevicesForEachUser()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var mockFilteringService = new Mock<IFilteringService>();
            var mockUserDeviceStore = new Mock<IUserDeviceStore>();
            var command = new NewSurveyNotificationCommand(mockSurveyStore.Object, mockFilteringService.Object, mockUserDeviceStore.Object, null);
            mockSurveyStore.Setup(s => s.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new Survey());
            mockFilteringService.Setup(s => s.GetUsersForSurvey(It.IsAny<Survey>())).Returns(new[] { "user1", "user2" });

            command.Run(new NewSurveyMessage());

            mockUserDeviceStore.Verify(s => s.GetDevices("user1"), Times.Once());
            mockUserDeviceStore.Verify(s => s.GetDevices("user2"), Times.Once());
        }

        [TestMethod]
        public void RunSendsNotificationsToDevices()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var mockFilteringService = new Mock<IFilteringService>();
            var mockUserDeviceStore = new Mock<IUserDeviceStore>();
            var mockPushNotification = new Mock<IPushNotification>();
            var command = new NewSurveyNotificationCommand(mockSurveyStore.Object, mockFilteringService.Object, mockUserDeviceStore.Object, mockPushNotification.Object);
            mockSurveyStore.Setup(s => s.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new Survey());
            mockFilteringService.Setup(s => s.GetUsersForSurvey(It.IsAny<Survey>())).Returns(new[] { "user" });
            mockUserDeviceStore.Setup(s => s.GetDevices(It.IsAny<string>())).Returns(new[] { new Uri("http://device-uri/") });

            command.Run(new NewSurveyMessage());

            mockPushNotification.Verify(n => n.PushTileNotification("http://device-uri/", "New Surveys", null, 0, It.IsAny<DeviceNotFoundInMpns>()));
        }
    }
}