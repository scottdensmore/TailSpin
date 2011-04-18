




 




namespace TailSpin.Web.Survey.Shared.Tests.SurveysFiltering
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.SurveysFiltering;

    [TestClass]
    public class FilteringServiceFixture
    {
        [TestMethod]
        public void GetSurveysForUserRunsFilters()
        {
            var filter1 = new Mock<ISurveyFilter>();
            var filter2 = new Mock<ISurveyFilter>();
            var service = new FilteringService(new[] { filter1.Object, filter2.Object });
            var dateTime = new DateTime();

            service.GetSurveysForUser("username", dateTime).ToList();

            filter1.Verify(f => f.GetSurveys("username", dateTime), Times.Once());
            filter2.Verify(f => f.GetSurveys("username", dateTime), Times.Once());
        }

        [TestMethod]
        public void GetSurveysForUserReturnsSurveysFromFilter()
        {
            var filter = new Mock<ISurveyFilter>();
            var survey = new Survey();
            filter.Setup(f => f.GetSurveys(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(new[] { survey });
            var service = new FilteringService(new[] { filter.Object });

            var surveys = service.GetSurveysForUser("username", new DateTime());

            Assert.AreSame(survey, surveys.Single());
        }

        [TestMethod]
        public void GetSurveysForUserReturnsAllDistinctSurveys()
        {
            var filter1 = new Mock<ISurveyFilter>();
            var filter2 = new Mock<ISurveyFilter>();
            var survey = new Survey("slug-name") { Tenant = "tenant" };
            filter1.Setup(f => f.GetSurveys(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(new[] { survey });
            filter2.Setup(f => f.GetSurveys(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(new[] { survey });
            var service = new FilteringService(new[] { filter1.Object, filter2.Object });

            var surveys = service.GetSurveysForUser("username", new DateTime());

            Assert.AreEqual(1, surveys.Count());
        }

        [TestMethod]
        public void GetUsersForSurveyRunsFilters()
        {
            var filter1 = new Mock<ISurveyFilter>();
            var filter2 = new Mock<ISurveyFilter>();
            var service = new FilteringService(new[] { filter1.Object, filter2.Object });
            var survey = new Survey();

            service.GetUsersForSurvey(survey).ToList();

            filter1.Verify(f => f.GetUsers(survey), Times.Once());
            filter2.Verify(f => f.GetUsers(survey), Times.Once());
        }

        [TestMethod]
        public void GetUsersForSurveyReturnsUsersFromFilter()
        {
            var filter = new Mock<ISurveyFilter>();
            filter.Setup(f => f.GetUsers(It.IsAny<Survey>())).Returns(new[] { "username" });
            var service = new FilteringService(new[] { filter.Object });

            var users = service.GetUsersForSurvey(new Survey());

            Assert.AreSame("username", users.Single());
        }

        [TestMethod]
        public void GetUsersForSurveyReturnsAllDistinctUsers()
        {
            var filter1 = new Mock<ISurveyFilter>();
            var filter2 = new Mock<ISurveyFilter>();
            filter1.Setup(f => f.GetUsers(It.IsAny<Survey>())).Returns(new[] { "username" });
            filter2.Setup(f => f.GetUsers(It.IsAny<Survey>())).Returns(new[] { "username" });
            var service = new FilteringService(new[] { filter1.Object, filter2.Object });

            var users = service.GetUsersForSurvey(new Survey());

            Assert.AreEqual(1, users.Count());
        }
    }
}