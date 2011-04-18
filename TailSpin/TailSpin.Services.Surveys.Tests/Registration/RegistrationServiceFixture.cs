




 




namespace TailSpin.Services.Surveys.Tests.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TailSpin.Services.Surveys.Registration;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Stores;
    using TailSpin.Web.Survey.Shared.SurveysFiltering;

    [TestClass]
    public class RegistrationServiceFixture
    {
        [TestMethod]
        public void SetFiltersSetsTenants()
        {
            SetUsernameInCurrentThread("username");
            var mockTenantFilterStore = new Mock<ITenantFilterStore>();
            var service = new RegistrationService(mockTenantFilterStore.Object, new Mock<IUserDeviceStore>().Object, null);
            var tenants = new[] { new TenantDto { Key = "key" } };
            var filters = new SurveyFiltersDto { Tenants = tenants };

            service.SetFilters(filters);

            mockTenantFilterStore.Verify(s => s.SetTenants("username", It.Is<IEnumerable<string>>(t => t.Single() == "key")), Times.Once());
        }

        [TestMethod]
        public void GetFiltersFiltersByUsername()
        {
            SetUsernameInCurrentThread("username");
            var mockTenantFilterStore = new Mock<ITenantFilterStore>();
            var service = new RegistrationService(mockTenantFilterStore.Object, new Mock<IUserDeviceStore>().Object, new Mock<ITenantStore>().Object);

            service.GetFilters();

            mockTenantFilterStore.Verify(s => s.GetTenants("username"), Times.Once());
        }

        [TestMethod]
        public void GetFiltersReturnsTenantsFromFilters()
        {
            var mockTenantFilterStore = new Mock<ITenantFilterStore>();
            var service = new RegistrationService(mockTenantFilterStore.Object, new Mock<IUserDeviceStore>().Object, new Mock<ITenantStore>().Object);
            var tenants = new[] { "tenant" };
            mockTenantFilterStore.Setup(s => s.GetTenants(It.IsAny<string>())).Returns(tenants);

            var filtersInformationDto = service.GetFilters();

            Assert.AreEqual("tenant", filtersInformationDto.SurveyFilters.Tenants.Single().Key);
            Assert.AreEqual("tenant", filtersInformationDto.SurveyFilters.Tenants.Single().Name);
        }

        [TestMethod]
        public void GetFiltersReturnsAllTenants()
        {
            var mockTenantStore = new Mock<ITenantStore>();
            var service = new RegistrationService(new Mock<ITenantFilterStore>().Object, new Mock<IUserDeviceStore>().Object, mockTenantStore.Object);
            var tenants = new[] { new Tenant { Name = "tenant" } };
            mockTenantStore.Setup(s => s.GetTenants()).Returns(tenants);

            var filtersInformationDto = service.GetFilters();

            Assert.AreEqual("tenant", filtersInformationDto.AllTenants.Single().Key);
            Assert.AreEqual("tenant", filtersInformationDto.AllTenants.Single().Name);
        }

        [TestMethod]
        public void NotificationsSetsUserDeviceWhenRecieveNotificationsAndUriIsWellFormed()
        {
            SetUsernameInCurrentThread("username");

            var mockUserDeviceStore = new Mock<IUserDeviceStore>();
            var service = new RegistrationService(new Mock<ITenantFilterStore>().Object, mockUserDeviceStore.Object, null);
            var deviceUri = new Uri("http://device-uri");
            var deviceDto = new DeviceDto
            {
                RecieveNotifications = true,
                Uri = deviceUri.ToString()
            };

            service.Notifications(deviceDto);

            mockUserDeviceStore.Verify(s => s.SetUserDevice("username", deviceUri), Times.Once());
        }
        
        [TestMethod]
        public void NotificationsRemovesUserDeviceWhenNotRecieveNotificationsAndUriIsWellFormed()
        {
            var mockUserDeviceStore = new Mock<IUserDeviceStore>();
            var service = new RegistrationService(new Mock<ITenantFilterStore>().Object, mockUserDeviceStore.Object, null);
            var deviceUri = new Uri("http://device-uri");
            var deviceDto = new DeviceDto
            {
                RecieveNotifications = false,
                Uri = deviceUri.ToString()
            };

            service.Notifications(deviceDto);

            mockUserDeviceStore.Verify(s => s.RemoveUserDevice(deviceUri), Times.Once());
        }

        private static void SetUsernameInCurrentThread(string username)
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), new string[] { });
        }
    }
}
