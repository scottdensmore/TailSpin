




 




namespace TailSpin.Workers.Surveys.Tests
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.QueueMessages;
    using TailSpin.Web.Survey.Shared.Stores;

    [TestClass]
    public class UpdatingSurveyResultsSummaryCommandFixture
    {
        [TestMethod]
        public void PreRunClearsSurveyAnswersCache()
        {
            var mockSurveyAnswersSummaryCache = new Mock<IDictionary<string, SurveyAnswersSummary>>();
            var command = new UpdatingSurveyResultsSummaryCommand(mockSurveyAnswersSummaryCache.Object, new Mock<ISurveyAnswerStore>().Object, new Mock<ISurveyAnswersSummaryStore>().Object);

            command.PreRun();

            mockSurveyAnswersSummaryCache.Verify(c => c.Clear(), Times.Once());
        }

        [TestMethod]
        public void RunAddTheAnswerIdToTheListInTheStore()
        {
            var mockSurveyAnswersSummaryCache = new Mock<IDictionary<string, SurveyAnswersSummary>>();
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var command = new UpdatingSurveyResultsSummaryCommand(mockSurveyAnswersSummaryCache.Object, mockSurveyAnswerStore.Object, new Mock<ISurveyAnswersSummaryStore>().Object);
            var message = new SurveyAnswerStoredMessage
            {
                Tenant = "tenant",
                SurveySlugName = "slug-name",
                SurveyAnswerBlobId = "id"
            };

            command.Run(message);

            mockSurveyAnswerStore.Verify(r => r.AppendSurveyAnswerIdToAnswersList("tenant", "slug-name", "id"));
        }

        [TestMethod]
        public void RunGetsTheSurveyAnswerFromTheStore()
        {
            var mockSurveyAnswersSummaryCache = new Mock<IDictionary<string, SurveyAnswersSummary>>();
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var command = new UpdatingSurveyResultsSummaryCommand(mockSurveyAnswersSummaryCache.Object, mockSurveyAnswerStore.Object, new Mock<ISurveyAnswersSummaryStore>().Object);
            mockSurveyAnswersSummaryCache.Setup(c => c["tenant_slug-name"])
                                         .Returns(new SurveyAnswersSummary("tenant", "slug-name"));
            var message = new SurveyAnswerStoredMessage
            {
                Tenant = "tenant",
                SurveySlugName = "slug-name",
                SurveyAnswerBlobId = "id"
            };

            command.Run(message);

            mockSurveyAnswerStore.Verify(r => r.GetSurveyAnswer("tenant", "slug-name", "id"));
        }

        [TestMethod]
        public void RunGetsTheSurveyAnswersSummaryFromTheCache()
        {
            var mockSurveyAnswersSummaryCache = new Mock<IDictionary<string, SurveyAnswersSummary>>();
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var command = new UpdatingSurveyResultsSummaryCommand(mockSurveyAnswersSummaryCache.Object, mockSurveyAnswerStore.Object, new Mock<ISurveyAnswersSummaryStore>().Object);
            mockSurveyAnswersSummaryCache.Setup(c => c.ContainsKey("tenant_slug-name")).Returns(true);
            mockSurveyAnswersSummaryCache.Setup(c => c["tenant_slug-name"])
                                         .Returns(new SurveyAnswersSummary("tenant", "slug-name"));
            var message = new SurveyAnswerStoredMessage
            {
                Tenant = "tenant",
                SurveySlugName = "slug-name"
            };

            command.Run(message);

            mockSurveyAnswersSummaryCache.Verify(c => c["tenant_slug-name"], Times.Once());
        }

        [TestMethod]
        public void RunCreatesNewSummaryAndAddsItToCacheWhenNotFoundInCache()
        {
            var mockSurveyAnswersSummaryCache = new Mock<IDictionary<string, SurveyAnswersSummary>>();
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var command = new UpdatingSurveyResultsSummaryCommand(mockSurveyAnswersSummaryCache.Object, mockSurveyAnswerStore.Object, new Mock<ISurveyAnswersSummaryStore>().Object);
            var message = new SurveyAnswerStoredMessage
            {
                Tenant = "tenant",
                SurveySlugName = "slug-name"
            };
            mockSurveyAnswersSummaryCache.Setup(c => c["tenant_slug-name"])
                                         .Returns(default(SurveyAnswersSummary));

            command.Run(message);

            mockSurveyAnswersSummaryCache.VerifySet(
                c => c["tenant_slug-name"] = It.Is<SurveyAnswersSummary>(s => s.Tenant == "tenant" && s.SlugName == "slug-name"),
                Times.Once());
        }

        [TestMethod]
        public void RunAddsTheSurveyAnswerToTheSummary()
        {
            var mockSurveyAnswersSummaryCache = new Mock<IDictionary<string, SurveyAnswersSummary>>();
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var command = new UpdatingSurveyResultsSummaryCommand(mockSurveyAnswersSummaryCache.Object, mockSurveyAnswerStore.Object, new Mock<ISurveyAnswersSummaryStore>().Object);
            var mockSurveyAnswersSummary = new Mock<SurveyAnswersSummary>("tenant", "slug-name");
            mockSurveyAnswersSummaryCache.Setup(c => c.ContainsKey("tenant_slug-name")).Returns(true);
            mockSurveyAnswersSummaryCache.Setup(c => c["tenant_slug-name"])
                                         .Returns(mockSurveyAnswersSummary.Object);
            var surveyAnswer = new SurveyAnswer();
            mockSurveyAnswerStore.Setup(r => r.GetSurveyAnswer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(surveyAnswer);
            var message = new SurveyAnswerStoredMessage
            {
                Tenant = "tenant",
                SurveySlugName = "slug-name"
            };

            command.Run(message);

            mockSurveyAnswersSummary.Verify(s => s.AddNewAnswer(It.Is<SurveyAnswer>(sa => surveyAnswer == sa)), Times.Once());
        }

        [TestMethod]
        public void PostRunGetsTheSummaryFromTheReporitoryCorrespondingToTheSumamryInTheCache()
        {
            var mockSurveyAnswersSummaryCache = new Mock<IDictionary<string, SurveyAnswersSummary>>();
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var mockSurveyAnswersSummaryStore = new Mock<ISurveyAnswersSummaryStore>();
            var command = new UpdatingSurveyResultsSummaryCommand(mockSurveyAnswersSummaryCache.Object, mockSurveyAnswerStore.Object, mockSurveyAnswersSummaryStore.Object);
            var surveyAnswersSummary = new SurveyAnswersSummary("tenant", "slug-name");
            mockSurveyAnswersSummaryCache.Setup(c => c.Values)
                                         .Returns(new[] { surveyAnswersSummary });

            command.PostRun();

            mockSurveyAnswersSummaryStore.Verify(
                r => r.GetSurveyAnswersSummary("tenant", "slug-name"),
                Times.Once());
        }

        [TestMethod]
        public void PostRunMergesTheSummaryFromTheReporitoryWithTheSumamryInTheCache()
        {
            var mockSurveyAnswersSummaryCache = new Mock<IDictionary<string, SurveyAnswersSummary>>();
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var mockSurveyAnswersSummaryStore = new Mock<ISurveyAnswersSummaryStore>();
            var command = new UpdatingSurveyResultsSummaryCommand(mockSurveyAnswersSummaryCache.Object, mockSurveyAnswerStore.Object, mockSurveyAnswersSummaryStore.Object);
            var mockSurveyAnswersSummary = new Mock<SurveyAnswersSummary>("tenant", "slug-name");
            mockSurveyAnswersSummaryCache.Setup(c => c.Values)
                                         .Returns(new[] { mockSurveyAnswersSummary.Object });
            var surveyAnswersSummaryInStore = new SurveyAnswersSummary("tenant", "slug-name");
            mockSurveyAnswersSummaryStore.Setup(r => r.GetSurveyAnswersSummary("tenant", "slug-name"))
                                               .Returns(surveyAnswersSummaryInStore);

            command.PostRun();

            mockSurveyAnswersSummary.Verify(s => s.MergeWith(It.Is<SurveyAnswersSummary>(sa => sa == surveyAnswersSummaryInStore)));
        }

        [TestMethod]
        public void PostRunSavesTheSummaryToTheStore()
        {
            var mockSurveyAnswersSummaryCache = new Mock<IDictionary<string, SurveyAnswersSummary>>();
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var mockSurveyAnswersSummaryStore = new Mock<ISurveyAnswersSummaryStore>();
            var command = new UpdatingSurveyResultsSummaryCommand(mockSurveyAnswersSummaryCache.Object, mockSurveyAnswerStore.Object, mockSurveyAnswersSummaryStore.Object);
            var surveyAnswersSummary = new SurveyAnswersSummary("tenant", "slug-name");
            mockSurveyAnswersSummaryCache.Setup(c => c.Values)
                                         .Returns(new[] { surveyAnswersSummary });

            command.PostRun();

            mockSurveyAnswersSummaryStore.Verify(
                r => r.SaveSurveyAnswersSummary(It.Is<SurveyAnswersSummary>(s => s == surveyAnswersSummary)),
                Times.Once());
        }
    }
}