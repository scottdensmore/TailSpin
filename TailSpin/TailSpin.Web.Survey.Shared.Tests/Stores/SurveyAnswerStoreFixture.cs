




 




namespace TailSpin.Web.Survey.Shared.Tests.Stores
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using QueueMessages;
    using Shared.Stores;
    using Shared.Stores.AzureStorage;
    using TailSpin.Web.Survey.Shared.Models;

    [TestClass]
    public class SurveyAnswerStoreFixture
    {
        [TestMethod]
        public void SaveSurveyAnswerCreatesBlobContainerForGivenTenantAndSurvey()
        {
            var mockSurveyAnswerContainerFactory = new Mock<ISurveyAnswerContainerFactory>();
            var store = new SurveyAnswerStore(mockSurveyAnswerContainerFactory.Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, new Mock<IAzureBlobContainer<List<string>>>().Object, null);
            var surveyAnswer = new SurveyAnswer { Tenant = "tenant", SlugName = "slug-name" };
            mockSurveyAnswerContainerFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(new Mock<IAzureBlobContainer<SurveyAnswer>>().Object);

            store.SaveSurveyAnswer(surveyAnswer);

            mockSurveyAnswerContainerFactory.Verify(f => f.Create("tenant", "slug-name"));
        }

        [TestMethod]
        public void SaveSurveyAnswerEnsuresBlobContainerExists()
        {
            var mockSurveyAnswerBlobContainer = new Mock<IAzureBlobContainer<SurveyAnswer>>();
            var mockSurveyAnswerContainerFactory = new Mock<ISurveyAnswerContainerFactory>();
            var store = new SurveyAnswerStore(mockSurveyAnswerContainerFactory.Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, new Mock<IAzureBlobContainer<List<string>>>().Object, null);
            mockSurveyAnswerContainerFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(mockSurveyAnswerBlobContainer.Object);

            store.SaveSurveyAnswer(new SurveyAnswer());

            mockSurveyAnswerBlobContainer.Verify(c => c.EnsureExist());
        }

        [TestMethod]
        public void SaveSurveyAnswerSavesAnswerInBlobContainer()
        {
            var mockSurveyAnswerBlobContainer = new Mock<IAzureBlobContainer<SurveyAnswer>>();
            var mockSurveyAnswerContainerFactory = new Mock<ISurveyAnswerContainerFactory>();
            var store = new SurveyAnswerStore(mockSurveyAnswerContainerFactory.Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, new Mock<IAzureBlobContainer<List<string>>>().Object, null);
            mockSurveyAnswerContainerFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(mockSurveyAnswerBlobContainer.Object);
            var surveyAnswer = new SurveyAnswer();

            store.SaveSurveyAnswer(surveyAnswer);

            mockSurveyAnswerBlobContainer.Verify(c => c.Save(It.IsAny<string>(), It.Is<SurveyAnswer>(a => a == surveyAnswer)));
        }

        [TestMethod]
        public void SaveSurveyAnswerSavesAnswerInBlobContainerWithId()
        {
            var mockSurveyAnswerBlobContainer = new Mock<IAzureBlobContainer<SurveyAnswer>>();
            var mockSurveyAnswerContainerFactory = new Mock<ISurveyAnswerContainerFactory>();
            var store = new SurveyAnswerStore(mockSurveyAnswerContainerFactory.Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, new Mock<IAzureBlobContainer<List<string>>>().Object, null);
            mockSurveyAnswerContainerFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(mockSurveyAnswerBlobContainer.Object);

            store.SaveSurveyAnswer(new SurveyAnswer());

            mockSurveyAnswerBlobContainer.Verify(c => c.Save(It.Is<string>(s => s.Length == 19), It.IsAny<SurveyAnswer>()));
        }

        [TestMethod]
        public void SaveSurveyAnswerAddMessageToQueueWithSavedBlobId()
        {
            var mockSurveyAnswerBlobContainer = new Mock<IAzureBlobContainer<SurveyAnswer>>();
            var mockSurveyAnswerContainerFactory = new Mock<ISurveyAnswerContainerFactory>();
            var mockSurveyAnswerStoredQueue = new Mock<IAzureQueue<SurveyAnswerStoredMessage>>();
            var store = new SurveyAnswerStore(mockSurveyAnswerContainerFactory.Object, mockSurveyAnswerStoredQueue.Object, new Mock<IAzureBlobContainer<List<string>>>().Object, null);
            mockSurveyAnswerContainerFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(mockSurveyAnswerBlobContainer.Object);
            string blobId = string.Empty;
            mockSurveyAnswerBlobContainer.Setup(c => c.Save(It.IsAny<string>(), It.IsAny<SurveyAnswer>()))
                                         .Callback((string id, SurveyAnswer sa) => blobId = id);

            store.SaveSurveyAnswer(new SurveyAnswer());

            mockSurveyAnswerStoredQueue.Verify(
                q => q.AddMessage(
                    It.Is<SurveyAnswerStoredMessage>(m => m.SurveyAnswerBlobId == blobId)));
        }

        [TestMethod]
        public void SaveSurveyAnswerAddMessageToQueueWithTenant()
        {
            var mockSurveyAnswerContainerFactory = new Mock<ISurveyAnswerContainerFactory>();
            var mockSurveyAnswerStoredQueue = new Mock<IAzureQueue<SurveyAnswerStoredMessage>>();
            var store = new SurveyAnswerStore(mockSurveyAnswerContainerFactory.Object, mockSurveyAnswerStoredQueue.Object, new Mock<IAzureBlobContainer<List<string>>>().Object, null);
            mockSurveyAnswerContainerFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(new Mock<IAzureBlobContainer<SurveyAnswer>>().Object);

            store.SaveSurveyAnswer(new SurveyAnswer { Tenant = "tenant" });

            mockSurveyAnswerStoredQueue.Verify(
                q => q.AddMessage(
                    It.Is<SurveyAnswerStoredMessage>(m => m.Tenant == "tenant")));
        }

        [TestMethod]
        public void SaveSurveyAnswerAddMessageToQueueWithSurveySlugName()
        {
            var mockSurveyAnswerContainerFactory = new Mock<ISurveyAnswerContainerFactory>();
            var mockSurveyAnswerStoredQueue = new Mock<IAzureQueue<SurveyAnswerStoredMessage>>();
            var store = new SurveyAnswerStore(mockSurveyAnswerContainerFactory.Object, mockSurveyAnswerStoredQueue.Object, new Mock<IAzureBlobContainer<List<string>>>().Object, null);
            mockSurveyAnswerContainerFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(new Mock<IAzureBlobContainer<SurveyAnswer>>().Object);

            store.SaveSurveyAnswer(new SurveyAnswer { SlugName = "slug-name" });

            mockSurveyAnswerStoredQueue.Verify(
                q => q.AddMessage(
                    It.Is<SurveyAnswerStoredMessage>(m => m.SurveySlugName == "slug-name")));
        }

        [TestMethod]
        public void AppendSurveyAnswerIdToAnswersListGetTheAnswersListBlob()
        {
            var mockSurveyAnswerIdsListContainer = new Mock<IAzureBlobContainer<List<string>>>();
            var store = new SurveyAnswerStore(new Mock<ISurveyAnswerContainerFactory>().Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, mockSurveyAnswerIdsListContainer.Object, null);

            store.AppendSurveyAnswerIdToAnswersList("tenant", "slug-name", string.Empty);
            
            mockSurveyAnswerIdsListContainer.Verify(c => c.Get("tenant-slug-name"), Times.Once());
        }

        [TestMethod]
        public void AppendSurveyAnswerIdToAnswersListSavesModifiedListToTheAnswersListBlob()
        {
            var mockSurveyAnswerIdsListContainer = new Mock<IAzureBlobContainer<List<string>>>();
            var store = new SurveyAnswerStore(new Mock<ISurveyAnswerContainerFactory>().Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, mockSurveyAnswerIdsListContainer.Object, null);
            var answerIdsList = new List<string> { "id 1", "id 2" };
            mockSurveyAnswerIdsListContainer.Setup(c => c.Get("tenant-slug-name")).Returns(answerIdsList);
            List<string> savedList = null;
            mockSurveyAnswerIdsListContainer.Setup(c => c.Save("tenant-slug-name", It.IsAny<List<string>>()))
                                            .Callback<string, List<string>>((id, l) => savedList = l);

            store.AppendSurveyAnswerIdToAnswersList("tenant", "slug-name", "new id");

            Assert.AreEqual(3, savedList.Count);
            Assert.AreEqual("new id", savedList.Last());
        }

        [TestMethod]
        public void AppendSurveyAnswerIdToAnswersListCreatesListWhenItDoesNotExistAndSavesIt()
        {
            var mockSurveyAnswerIdsListContainer = new Mock<IAzureBlobContainer<List<string>>>();
            var store = new SurveyAnswerStore(new Mock<ISurveyAnswerContainerFactory>().Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, mockSurveyAnswerIdsListContainer.Object, null);
            List<string> answerIdsList = null;
            mockSurveyAnswerIdsListContainer.Setup(c => c.Get("tenant-slug-name")).Returns(answerIdsList);
            List<string> savedList = null;
            mockSurveyAnswerIdsListContainer.Setup(c => c.Save("tenant-slug-name", It.IsAny<List<string>>()))
                                            .Callback<string, List<string>>((id, l) => savedList = l);

            store.AppendSurveyAnswerIdToAnswersList("tenant", "slug-name", "new id");

            Assert.AreEqual(1, savedList.Count);
            Assert.AreEqual("new id", savedList.Last());
        }

        [TestMethod]
        public void GetSurveyAnswerBrowsingContextGetTheAnswersListBlob()
        {
            var mockSurveyAnswerIdsListContainer = new Mock<IAzureBlobContainer<List<string>>>();
            var store = new SurveyAnswerStore(new Mock<ISurveyAnswerContainerFactory>().Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, mockSurveyAnswerIdsListContainer.Object, null);

            store.GetSurveyAnswerBrowsingContext("tenant", "slug-name", string.Empty);

            mockSurveyAnswerIdsListContainer.Verify(c => c.Get("tenant-slug-name"), Times.Once());
        }

        [TestMethod]
        public void GetSurveyAnswerBrowsingContextReturnsPreviousId()
        {
            var mockSurveyAnswerIdsListContainer = new Mock<IAzureBlobContainer<List<string>>>();
            var store = new SurveyAnswerStore(new Mock<ISurveyAnswerContainerFactory>().Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, mockSurveyAnswerIdsListContainer.Object, null);
            mockSurveyAnswerIdsListContainer.Setup(c => c.Get("tenant-slug-name"))
                                            .Returns(new List<string> { "id 1", "id 2", "id 3" });

            var surveyAnswerBrowsingContext = store.GetSurveyAnswerBrowsingContext("tenant", "slug-name", "id 2");

            Assert.AreEqual("id 1", surveyAnswerBrowsingContext.PreviousId);
        }

        [TestMethod]
        public void GetSurveyAnswerBrowsingContextReturnsNextId()
        {
            var mockSurveyAnswerIdsListContainer = new Mock<IAzureBlobContainer<List<string>>>();
            var store = new SurveyAnswerStore(new Mock<ISurveyAnswerContainerFactory>().Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, mockSurveyAnswerIdsListContainer.Object, null);
            mockSurveyAnswerIdsListContainer.Setup(c => c.Get("tenant-slug-name"))
                                            .Returns(new List<string> { "id 1", "id 2", "id 3" });

            var surveyAnswerBrowsingContext = store.GetSurveyAnswerBrowsingContext("tenant", "slug-name", "id 2");

            Assert.AreEqual("id 3", surveyAnswerBrowsingContext.NextId);
        }

        [TestMethod]
        public void GetSurveyAnswerBrowsingContextReturnsNullNextIdAndPreviousIdWhenListDoesNotExist()
        {
            var mockSurveyAnswerIdsListContainer = new Mock<IAzureBlobContainer<List<string>>>();
            var store = new SurveyAnswerStore(new Mock<ISurveyAnswerContainerFactory>().Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, mockSurveyAnswerIdsListContainer.Object, null);
            mockSurveyAnswerIdsListContainer.Setup(c => c.Get("tenant-slug-name"))
                                            .Returns(default(List<string>));

            var surveyAnswerBrowsingContext = store.GetSurveyAnswerBrowsingContext("tenant", "slug-name", "id");

            Assert.IsNull(surveyAnswerBrowsingContext.PreviousId);
            Assert.IsNull(surveyAnswerBrowsingContext.NextId);
        }

        [TestMethod]
        public void GetSurveyAnswerBrowsingContextReturnsNullNextIdWhenEndOfList()
        {
            var mockSurveyAnswerIdsListContainer = new Mock<IAzureBlobContainer<List<string>>>();
            var store = new SurveyAnswerStore(new Mock<ISurveyAnswerContainerFactory>().Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, mockSurveyAnswerIdsListContainer.Object, null);
            mockSurveyAnswerIdsListContainer.Setup(c => c.Get("tenant-slug-name"))
                                            .Returns(new List<string> { "id 1" });

            var surveyAnswerBrowsingContext = store.GetSurveyAnswerBrowsingContext("tenant", "slug-name", "id 1");

            Assert.IsNull(surveyAnswerBrowsingContext.NextId);
        }

        [TestMethod]
        public void GetSurveyAnswerBrowsingContextReturnsNullPreviousIdWhenInBeginingOfList()
        {
            var mockSurveyAnswerIdsListContainer = new Mock<IAzureBlobContainer<List<string>>>();
            var store = new SurveyAnswerStore(new Mock<ISurveyAnswerContainerFactory>().Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, mockSurveyAnswerIdsListContainer.Object, null);
            mockSurveyAnswerIdsListContainer.Setup(c => c.Get("tenant-slug-name"))
                                            .Returns(new List<string> { "id 1" });

            var surveyAnswerBrowsingContext = store.GetSurveyAnswerBrowsingContext("tenant", "slug-name", "id 1");

            Assert.IsNull(surveyAnswerBrowsingContext.PreviousId);
        }

        [TestMethod]
        public void GetFirstSurveyAnswerIdGetTheAnswersListBlob()
        {
            var mockSurveyAnswerIdsListContainer = new Mock<IAzureBlobContainer<List<string>>>();
            var store = new SurveyAnswerStore(new Mock<ISurveyAnswerContainerFactory>().Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, mockSurveyAnswerIdsListContainer.Object, null);

            store.GetFirstSurveyAnswerId("tenant", "slug-name");

            mockSurveyAnswerIdsListContainer.Verify(c => c.Get("tenant-slug-name"), Times.Once());
        }

        [TestMethod]
        public void GetFirstSurveyAnswerIdReturnsTheAnswerWhichAppearsFirstOnTheListWhenListIsNotEmpty()
        {
            var mockSurveyAnswerIdsListContainer = new Mock<IAzureBlobContainer<List<string>>>();
            var store = new SurveyAnswerStore(new Mock<ISurveyAnswerContainerFactory>().Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, mockSurveyAnswerIdsListContainer.Object, null);
            mockSurveyAnswerIdsListContainer.Setup(c => c.Get("tenant-slug-name"))
                                            .Returns(new List<string> { "id" });

            var id = store.GetFirstSurveyAnswerId("tenant", "slug-name");

            Assert.AreEqual("id", id);
        }

        [TestMethod]
        public void GetFirstSurveyAnswerIdReturnsEmprtyStringWhenListIsEmpty()
        {
            var mockSurveyAnswerIdsListContainer = new Mock<IAzureBlobContainer<List<string>>>();
            var store = new SurveyAnswerStore(new Mock<ISurveyAnswerContainerFactory>().Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, mockSurveyAnswerIdsListContainer.Object, null);
            mockSurveyAnswerIdsListContainer.Setup(c => c.Get("tenant-slug-name"))
                                            .Returns(default(List<string>));

            var id = store.GetFirstSurveyAnswerId("tenant", "slug-name");

            Assert.AreEqual(string.Empty, id);
        }

        [TestMethod]
        public void GetSurveyAnswerCreatesBlobContainerForGivenTenantAndSurvey()
        {
            var mockSurveyAnswerContainerFactory = new Mock<ISurveyAnswerContainerFactory>();
            var store = new SurveyAnswerStore(mockSurveyAnswerContainerFactory.Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, new Mock<IAzureBlobContainer<List<string>>>().Object, null);
            mockSurveyAnswerContainerFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(new Mock<IAzureBlobContainer<SurveyAnswer>>().Object);

            store.GetSurveyAnswer("tenant", "slug-name", string.Empty);

            mockSurveyAnswerContainerFactory.Verify(f => f.Create("tenant", "slug-name"));
        }

        [TestMethod]
        public void GetSurveyAnswerEnsuresBlobContainerExists()
        {
            var mockSurveyAnswerBlobContainer = new Mock<IAzureBlobContainer<SurveyAnswer>>();
            var mockSurveyAnswerContainerFactory = new Mock<ISurveyAnswerContainerFactory>();
            var store = new SurveyAnswerStore(mockSurveyAnswerContainerFactory.Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, new Mock<IAzureBlobContainer<List<string>>>().Object, null);
            mockSurveyAnswerContainerFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(mockSurveyAnswerBlobContainer.Object);

            store.GetSurveyAnswer("tenant", "slug-name", string.Empty);

            mockSurveyAnswerBlobContainer.Verify(c => c.EnsureExist());
        }

        [TestMethod]
        public void GetSurveyAnswerGetsAnswerFromBlobContainer()
        {
            var mockSurveyAnswerBlobContainer = new Mock<IAzureBlobContainer<SurveyAnswer>>();
            var mockSurveyAnswerContainerFactory = new Mock<ISurveyAnswerContainerFactory>();
            var store = new SurveyAnswerStore(mockSurveyAnswerContainerFactory.Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, new Mock<IAzureBlobContainer<List<string>>>().Object, null);
            mockSurveyAnswerContainerFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(mockSurveyAnswerBlobContainer.Object);

            store.GetSurveyAnswer("tenant", "slug-name", "id");

            mockSurveyAnswerBlobContainer.Verify(c => c.Get("id"));
        }

        [TestMethod]
        public void GetSurveyAnswerReturnsAnswerObtainedFromBlobContainer()
        {
            var mockSurveyAnswerBlobContainer = new Mock<IAzureBlobContainer<SurveyAnswer>>();
            var mockSurveyAnswerContainerFactory = new Mock<ISurveyAnswerContainerFactory>();
            var store = new SurveyAnswerStore(mockSurveyAnswerContainerFactory.Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, new Mock<IAzureBlobContainer<List<string>>>().Object, null);
            mockSurveyAnswerContainerFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(mockSurveyAnswerBlobContainer.Object);
            var surveyAnswer = new SurveyAnswer();
            mockSurveyAnswerBlobContainer.Setup(c => c.Get(It.IsAny<string>()))
                                         .Returns(surveyAnswer);

            var actualSurveyAnswer = store.GetSurveyAnswer("tenant", "slug-name", string.Empty);

            Assert.AreSame(surveyAnswer, actualSurveyAnswer);
        }

        [TestMethod]
        public void DeleteSurveyAnswersCreatesBlobContainer()
        {
            var mockSurveyAnswerContainerFactory = new Mock<ISurveyAnswerContainerFactory>();
            var store = new SurveyAnswerStore(mockSurveyAnswerContainerFactory.Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, new Mock<IAzureBlobContainer<List<string>>>().Object, null);
            mockSurveyAnswerContainerFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(new Mock<IAzureBlobContainer<SurveyAnswer>>().Object);

            store.DeleteSurveyAnswers("tenant", "slug-name");

            mockSurveyAnswerContainerFactory.Verify(f => f.Create("tenant", "slug-name"), Times.Once());
        }

        [TestMethod]
        public void DeleteSurveyAnswersCallsDeleteFromBlobContainer()
        {
            var mockSurveyAnswerBlobContainer = new Mock<IAzureBlobContainer<SurveyAnswer>>();
            var mockSurveyAnswerContainerFactory = new Mock<ISurveyAnswerContainerFactory>();
            var store = new SurveyAnswerStore(mockSurveyAnswerContainerFactory.Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, new Mock<IAzureBlobContainer<List<string>>>().Object, null);
            mockSurveyAnswerContainerFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(mockSurveyAnswerBlobContainer.Object);

            store.DeleteSurveyAnswers("tenant", "slug-name");

            mockSurveyAnswerBlobContainer.Verify(c => c.DeleteContainer(), Times.Once());
        }

        [TestMethod]
        public void DeleteSurveyAnswersDeletesAnswersList()
        {
            var mockSurveyAnswerContainerFactory = new Mock<ISurveyAnswerContainerFactory>();
            var mockSurveyAnswerIdsListContainer = new Mock<IAzureBlobContainer<List<string>>>();
            var store = new SurveyAnswerStore(mockSurveyAnswerContainerFactory.Object, new Mock<IAzureQueue<SurveyAnswerStoredMessage>>().Object, mockSurveyAnswerIdsListContainer.Object, null);
            mockSurveyAnswerContainerFactory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(new Mock<IAzureBlobContainer<SurveyAnswer>>().Object);

            store.DeleteSurveyAnswers("tenant", "slug-name");

            mockSurveyAnswerIdsListContainer.Verify(c => c.Delete("tenant-slug-name"), Times.Once());
        }
    }
}