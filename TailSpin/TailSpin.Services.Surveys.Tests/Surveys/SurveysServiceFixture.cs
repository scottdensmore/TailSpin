




 




namespace TailSpin.Services.Surveys.Tests.Surveys
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TailSpin.Services.Surveys.Surveys;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Stores;
    using TailSpin.Web.Survey.Shared.SurveysFiltering;

    [TestClass]
    public class SurveysServiceFixture
    {
        [TestMethod]
        public void GetSurveysFiltersByUsername()
        {
            SetUsernameInCurrentThread("username");
            var mockFilteringService = new Mock<IFilteringService>();
            mockFilteringService.Setup(s => s.GetSurveysForUser("username", It.IsAny<DateTime>())).Returns(new Survey[] { }).Verifiable();
            var service = new SurveysService(mockFilteringService.Object, new Mock<ITenantStore>().Object, null, null);

            service.GetSurveys(null);

            mockFilteringService.Verify();
        }

        [TestMethod]
        public void GetSurveysFiltersByLastSyncUtcTime()
        {
            var mockFilteringService = new Mock<IFilteringService>();
            mockFilteringService.Setup(s => s.GetSurveysForUser(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(new Survey[] { });
            var service = new SurveysService(mockFilteringService.Object, new Mock<ITenantStore>().Object, null, null);
            var lasySyncDate = new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            service.GetSurveys(lasySyncDate.ToString("s"));

            mockFilteringService.Verify(s => s.GetSurveysForUser(It.IsAny<string>(), lasySyncDate), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void GetSurveysThrowsWhenLastSyncUtcTimeIsProvidedAndHasWrongFormat()
        {
            var mockFilteringService = new Mock<IFilteringService>();
            mockFilteringService.Setup(s => s.GetSurveysForUser(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(new Survey[] { });
            var service = new SurveysService(mockFilteringService.Object, new Mock<ITenantStore>().Object, null, null);

            service.GetSurveys("wromg date format");
        }

        [TestMethod]
        public void GetSurveysReturnsSurveySlugName()
        {
            var mockFilteringService = new Mock<IFilteringService>();
            var surveysToReturn = new[] { new Survey("slug-name"), };
            mockFilteringService.Setup(s => s.GetSurveysForUser(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(surveysToReturn);
            var service = new SurveysService(mockFilteringService.Object, new Mock<ITenantStore>().Object, null, null);

            var surveys = service.GetSurveys(null);

            Assert.AreEqual("slug-name", surveys.First().SlugName);
        }

        [TestMethod]
        public void GetSurveysReturnsSurveyTitle()
        {
            var mockFilteringService = new Mock<IFilteringService>();
            var surveysToReturn = new[] { new Survey { Title = "title" } };
            mockFilteringService.Setup(s => s.GetSurveysForUser(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(surveysToReturn);
            var service = new SurveysService(mockFilteringService.Object, new Mock<ITenantStore>().Object, null, null);

            var surveys = service.GetSurveys(null);

            Assert.AreEqual("title", surveys.First().Title);
        }

        [TestMethod]
        public void GetSurveysReturnsSurveyTenant()
        {
            var mockFilteringService = new Mock<IFilteringService>();
            var surveysToReturn = new[] { new Survey { Tenant = "tenant" } };
            mockFilteringService.Setup(s => s.GetSurveysForUser(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(surveysToReturn);
            var mockTenantStore = new Mock<ITenantStore>();
            mockTenantStore.Setup(s => s.GetTenant(It.IsAny<string>())).Returns(new Tenant());
            var service = new SurveysService(mockFilteringService.Object, mockTenantStore.Object, null, null);

            var surveys = service.GetSurveys(null);

            Assert.AreEqual("tenant", surveys.First().Tenant);
        }

        [TestMethod]
        public void GetSurveysReturnsSurveyLengthBasedOnQuestionsCount()
        {
            var mockFilteringService = new Mock<IFilteringService>();
            var surveysToReturn = new[] { new Survey { Questions = new List<Question> { new Question(), new Question() } } };
            mockFilteringService.Setup(s => s.GetSurveysForUser(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(surveysToReturn);
            var service = new SurveysService(mockFilteringService.Object, new Mock<ITenantStore>().Object, null, null);

            var surveys = service.GetSurveys(null);

            Assert.AreEqual(2 * 5, surveys.First().Length);
        }

        [TestMethod]
        public void GetSurveysReturnsSurveyIconUrlBasedOnTenant()
        {
            var mockFilteringService = new Mock<IFilteringService>();
            var surveysToReturn = new[] { new Survey { Tenant = "tenant" } };
            mockFilteringService.Setup(s => s.GetSurveysForUser(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(surveysToReturn);
            var mockTenantStore = new Mock<ITenantStore>();
            mockTenantStore.Setup(s => s.GetTenant("tenant")).Returns(new Tenant { PhoneLogo = "logo url" }).Verifiable();
            var service = new SurveysService(mockFilteringService.Object, mockTenantStore.Object, null, null);

            var surveys = service.GetSurveys(null);

            mockTenantStore.Verify();
            Assert.AreEqual("logo url", surveys.First().IconUrl);
        }

        [TestMethod]
        public void GetSurveysCachesTenantsReturnedFromStore()
        {
            var mockFilteringService = new Mock<IFilteringService>();
            var surveysToReturn = new[] { new Survey { Tenant = "tenant" }, new Survey { Tenant = "tenant" } };
            mockFilteringService.Setup(s => s.GetSurveysForUser(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(surveysToReturn);
            var mockTenantStore = new Mock<ITenantStore>();
            mockTenantStore.Setup(s => s.GetTenant("tenant")).Returns(new Tenant());
            var service = new SurveysService(mockFilteringService.Object, mockTenantStore.Object, null, null);

            service.GetSurveys(null);

            mockTenantStore.Verify(s => s.GetTenant("tenant"), Times.Once());
        }

        [TestMethod]
        public void GetSurveysReturnsQuestionPossibleAnswer()
        {
            var mockFilteringService = new Mock<IFilteringService>();
            var question = new Question { PossibleAnswers = "possible answers" };
            var surveysToReturn = new[] { new Survey { Questions = new List<Question> { question } } };
            mockFilteringService.Setup(s => s.GetSurveysForUser(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(surveysToReturn);
            var service = new SurveysService(mockFilteringService.Object, new Mock<ITenantStore>().Object, null, null);

            var surveys = service.GetSurveys(null);

            Assert.AreEqual("possible answers", surveys.First().Questions.First().PossibleAnswers);
        }

        [TestMethod]
        public void GetSurveysReturnsQuestionText()
        {
            var mockFilteringService = new Mock<IFilteringService>();
            var question = new Question { Text = "text" };
            var surveysToReturn = new[] { new Survey { Questions = new List<Question> { question } } };
            mockFilteringService.Setup(s => s.GetSurveysForUser(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(surveysToReturn);
            var service = new SurveysService(mockFilteringService.Object, new Mock<ITenantStore>().Object, null, null);

            var surveys = service.GetSurveys(null);

            Assert.AreEqual("text", surveys.First().Questions.First().Text);
        }

        [TestMethod]
        public void GetSurveysReturnsQuestionType()
        {
            var mockFilteringService = new Mock<IFilteringService>();
            var question = new Question { Type = QuestionType.SimpleText };
            var surveysToReturn = new[] { new Survey { Questions = new List<Question> { question } } };
            mockFilteringService.Setup(s => s.GetSurveysForUser(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(surveysToReturn);
            var service = new SurveysService(mockFilteringService.Object, new Mock<ITenantStore>().Object, null, null);

            var surveys = service.GetSurveys(null);

            Assert.AreEqual(Enum.GetName(typeof(QuestionType), QuestionType.SimpleText), surveys.First().Questions.First().Type);
        }

        [TestMethod]
        public void AddSurveyAnswersSavesTitle()
        {
            var surveyAnswerDto = new SurveyAnswerDto { Title = "title" };
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var service = new SurveysService(null, null, mockSurveyAnswerStore.Object, null);
            SurveyAnswer savedAnswer = null;
            mockSurveyAnswerStore.Setup(s => s.SaveSurveyAnswer(It.IsAny<SurveyAnswer>()))
                                 .Callback<SurveyAnswer>(a => savedAnswer = a);

            service.AddSurveyAnswers(new[] { surveyAnswerDto });

            Assert.AreEqual("title", savedAnswer.Title);
        }

        [TestMethod]
        public void AddSurveyAnswersSavesSlugName()
        {
            var surveyAnswerDto = new SurveyAnswerDto { SlugName = "slug-name" };
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var service = new SurveysService(null, null, mockSurveyAnswerStore.Object, null);
            SurveyAnswer savedAnswer = null;
            mockSurveyAnswerStore.Setup(s => s.SaveSurveyAnswer(It.IsAny<SurveyAnswer>()))
                                 .Callback<SurveyAnswer>(a => savedAnswer = a);

            service.AddSurveyAnswers(new[] { surveyAnswerDto });

            Assert.AreEqual("slug-name", savedAnswer.SlugName);
        }

        [TestMethod]
        public void AddSurveyAnswersSavesStartLocation()
        {
            var surveyAnswerDto = new SurveyAnswerDto { StartLocation = "0.000000,0.000000" };
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var service = new SurveysService(null, null, mockSurveyAnswerStore.Object, null);
            SurveyAnswer savedAnswer = null;
            mockSurveyAnswerStore.Setup(s => s.SaveSurveyAnswer(It.IsAny<SurveyAnswer>()))
                                 .Callback<SurveyAnswer>(a => savedAnswer = a);

            service.AddSurveyAnswers(new[] { surveyAnswerDto });

            Assert.AreEqual("0.000000,0.000000", savedAnswer.StartLocation);
        }

        [TestMethod]
        public void AddSurveyAnswersSavesCompleteLocation()
        {
            var surveyAnswerDto = new SurveyAnswerDto { CompleteLocation = "0.000000,0.000000" };
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var service = new SurveysService(null, null, mockSurveyAnswerStore.Object, null);
            SurveyAnswer savedAnswer = null;
            mockSurveyAnswerStore.Setup(s => s.SaveSurveyAnswer(It.IsAny<SurveyAnswer>()))
                                 .Callback<SurveyAnswer>(a => savedAnswer = a);

            service.AddSurveyAnswers(new[] { surveyAnswerDto });

            Assert.AreEqual("0.000000,0.000000", savedAnswer.CompleteLocation);
        }

        [TestMethod]
        public void AddSurveyAnswersSavesQuestionText()
        {
            var questionAnswerDto = new QuestionAnswerDto
                                        {
                                            QuestionText = "question text",
                                            QuestionType = Enum.GetName(typeof(QuestionType), QuestionType.SimpleText)
                                        };
            var surveyAnswerDto = new SurveyAnswerDto { QuestionAnswers = new[] { questionAnswerDto } };
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var service = new SurveysService(null, null, mockSurveyAnswerStore.Object, null);
            SurveyAnswer savedAnswer = null;
            mockSurveyAnswerStore.Setup(s => s.SaveSurveyAnswer(It.IsAny<SurveyAnswer>()))
                                 .Callback<SurveyAnswer>(a => savedAnswer = a);

            service.AddSurveyAnswers(new[] { surveyAnswerDto });

            Assert.AreEqual("question text", savedAnswer.QuestionAnswers.First().QuestionText);
        }

        [TestMethod]
        public void AddSurveyAnswersSavesPossibleAnswers()
        {
            var questionAnswerDto = new QuestionAnswerDto
                                        {
                                            PossibleAnswers = "possible answers",
                                            QuestionType = Enum.GetName(typeof(QuestionType), QuestionType.SimpleText)
                                        };
            var surveyAnswerDto = new SurveyAnswerDto { QuestionAnswers = new[] { questionAnswerDto } };
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var service = new SurveysService(null, null, mockSurveyAnswerStore.Object, null);
            SurveyAnswer savedAnswer = null;
            mockSurveyAnswerStore.Setup(s => s.SaveSurveyAnswer(It.IsAny<SurveyAnswer>()))
                                 .Callback<SurveyAnswer>(a => savedAnswer = a);

            service.AddSurveyAnswers(new[] { surveyAnswerDto });

            Assert.AreEqual("possible answers", savedAnswer.QuestionAnswers.First().PossibleAnswers);
        }

        [TestMethod]
        public void AddSurveyAnswersSavesQuestionType()
        {
            var questionAnswerDto = new QuestionAnswerDto { QuestionType = Enum.GetName(typeof(QuestionType), QuestionType.SimpleText) };
            var surveyAnswerDto = new SurveyAnswerDto { QuestionAnswers = new[] { questionAnswerDto } };
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var service = new SurveysService(null, null, mockSurveyAnswerStore.Object, null);
            SurveyAnswer savedAnswer = null;
            mockSurveyAnswerStore.Setup(s => s.SaveSurveyAnswer(It.IsAny<SurveyAnswer>()))
                                 .Callback<SurveyAnswer>(a => savedAnswer = a);

            service.AddSurveyAnswers(new[] { surveyAnswerDto });

            Assert.AreEqual(QuestionType.SimpleText, savedAnswer.QuestionAnswers.First().QuestionType);
        }

        [TestMethod]
        public void AddSurveyAnswersSavesAnswer()
        {
            var questionAnswerDto = new QuestionAnswerDto
                                        {
                                            Answer = "answer",
                                            QuestionType = Enum.GetName(typeof(QuestionType), QuestionType.SimpleText)
                                        };
            var surveyAnswerDto = new SurveyAnswerDto { QuestionAnswers = new[] { questionAnswerDto } };
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var service = new SurveysService(null, null, mockSurveyAnswerStore.Object, null);
            SurveyAnswer savedAnswer = null;
            mockSurveyAnswerStore.Setup(s => s.SaveSurveyAnswer(It.IsAny<SurveyAnswer>()))
                                 .Callback<SurveyAnswer>(a => savedAnswer = a);

            service.AddSurveyAnswers(new[] { surveyAnswerDto });

            Assert.AreEqual("answer", savedAnswer.QuestionAnswers.First().Answer);
        }

        [TestMethod]
        public void AddMediaAnswerAddsMediaToStore()
        {
            var mockMediaAnswerStore = new Mock<IMediaAnswerStore>();
            var service = new SurveysService(null, null, null, mockMediaAnswerStore.Object);
            var type = Enum.GetName(typeof(QuestionType), QuestionType.Voice);
            Stream media = null;

            service.AddMediaAnswer(media, type);

            mockMediaAnswerStore.Verify(s => s.SaveMediaAnswer(media, QuestionType.Voice), Times.Once());
        }

        [TestMethod]
        public void AddMediaAnswerReturnsStringFromStore()
        {
            var mockMediaAnswerStore = new Mock<IMediaAnswerStore>();
            mockMediaAnswerStore.Setup(s => s.SaveMediaAnswer(It.IsAny<Stream>(), It.IsAny<QuestionType>())).Returns("string to return");
            var service = new SurveysService(null, null, null, mockMediaAnswerStore.Object);
            var type = Enum.GetName(typeof(QuestionType), QuestionType.Voice);
            Stream media = null;

            var actual = service.AddMediaAnswer(media, type);

            Assert.AreEqual("string to return", actual);
        }

        private static void SetUsernameInCurrentThread(string username)
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), new string[] { });
        }
    }
}
