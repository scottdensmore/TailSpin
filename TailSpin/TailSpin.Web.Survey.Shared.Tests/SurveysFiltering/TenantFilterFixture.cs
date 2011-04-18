




 




namespace TailSpin.Web.Survey.Shared.Tests.SurveysFiltering
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Stores;
    using TailSpin.Web.Survey.Shared.SurveysFiltering;

    [TestClass]
    public class TenantFilterFixture
    {
        [TestMethod]
        public void GetSurveysGetsTenantsToFilterFromStore()
        {
            var mockTenantFilterStore = new Mock<ITenantFilterStore>();
            var filter = new TenantFilter(new Mock<ISurveyStore>().Object, mockTenantFilterStore.Object, null);
            mockTenantFilterStore.Setup(s => s.GetTenants(It.IsAny<string>())).Returns(new[] { "tenant" });

            filter.GetSurveys("username", new DateTime());

            mockTenantFilterStore.Verify(s => s.GetTenants("username"), Times.Once());
        }

        [TestMethod]
        public void GetSurveysGetsSurveysFromStoreForEachTenant()
        {
            var mockTenantStore = new Mock<ISurveyStore>();
            var mockTenantFilterStore = new Mock<ITenantFilterStore>();
            var filter = new TenantFilter(mockTenantStore.Object, mockTenantFilterStore.Object, null);
            mockTenantFilterStore.Setup(s => s.GetTenants(It.IsAny<string>())).Returns(new[] { "tenant1", "tenant2" });
            var fromDate = new DateTime();

            filter.GetSurveys("username", fromDate).ToList();

            mockTenantStore.Verify(s => s.GetSurveys("tenant1", fromDate), Times.Once());
            mockTenantStore.Verify(s => s.GetSurveys("tenant2", fromDate), Times.Once());
        }

        [TestMethod]
        public void GetSurveysReturnsDistinctSurveys()
        {
            var mockTenantStore = new Mock<ISurveyStore>();
            var mockTenantFilterStore = new Mock<ITenantFilterStore>();
            var filter = new TenantFilter(mockTenantStore.Object, mockTenantFilterStore.Object, null);
            mockTenantFilterStore.Setup(s => s.GetTenants(It.IsAny<string>())).Returns(new[] { "tenant1", "tenant2" });
            var survey = new Survey();
            mockTenantStore.Setup(s => s.GetSurveys(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(new[] { survey });
            
            var surveys = filter.GetSurveys("username", new DateTime());

            Assert.AreSame(survey, surveys.Single());
        }

        [TestMethod]
        public void GetUsersGetsUsersFromStoreWithTenant()
        {
            var mockTenantFilterStore = new Mock<ITenantFilterStore>();
            var filter = new TenantFilter(new Mock<ISurveyStore>().Object, mockTenantFilterStore.Object, null);

            filter.GetUsers(new Survey { Tenant = "tenant" });

            mockTenantFilterStore.Verify(s => s.GetUsers("tenant"), Times.Once());
        }

        [TestMethod]
        public void GetUsersReturnsUsersFromStore()
        {
            var mockTenantFilterStore = new Mock<ITenantFilterStore>();
            var filter = new TenantFilter(new Mock<ISurveyStore>().Object, mockTenantFilterStore.Object, null);
            mockTenantFilterStore.Setup(s => s.GetUsers(It.IsAny<string>())).Returns(new[] { "username" });

            var users = filter.GetUsers(new Survey());

            Assert.AreEqual("username", users.Single());
        }
    }
}