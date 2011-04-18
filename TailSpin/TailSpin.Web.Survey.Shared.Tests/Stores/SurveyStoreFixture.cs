




 




namespace TailSpin.Web.Survey.Shared.Tests.Stores
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.QueueMessages;
    using TailSpin.Web.Survey.Shared.Stores;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    [TestClass]
    public class SurveyStoreFixture
    {
        [TestMethod]
        public void GetSurveysByTenantFiltersByTenantInPartitionKey()
        {
            var surveyRow = new SurveyRow { PartitionKey = "tenant" };
            var otherSurveyRow = new SurveyRow { PartitionKey = "other tenant" };
            var surveyRowsForTheQuery = new[] { surveyRow, otherSurveyRow };
            var mock = new Mock<IAzureTable<SurveyRow>>();
            mock.SetupGet(t => t.Query).Returns(surveyRowsForTheQuery.AsQueryable());
            var store = new SurveyStore(mock.Object, default(IAzureTable<QuestionRow>), null);

            var surveysForTenant = store.GetSurveysByTenant("tenant");
            
            Assert.AreEqual(1, surveysForTenant.Count());
        }

        [TestMethod]
        public void GetSurveysByTenantReturnsTenantNameFromThePartitionKey()
        {
            var surveyRow = new SurveyRow { PartitionKey = "tenant" };
            var surveyRowsToReturn = new[] { surveyRow };
            var mock = new Mock<IAzureTable<SurveyRow>>();
            mock.SetupGet(t => t.Query).Returns(surveyRowsToReturn.AsQueryable());
            var store = new SurveyStore(mock.Object, default(IAzureTable<QuestionRow>), null);

            var actualSurveys = store.GetSurveysByTenant("tenant");

            Assert.AreEqual("tenant", actualSurveys.First().Tenant);
        }

        [TestMethod]
        public void GetSurveysByTenantReturnsTitle()
        {
            var surveyRow = new SurveyRow { PartitionKey = "tenant", Title = "title" };
            var surveyRowsToReturn = new[] { surveyRow };
            var mock = new Mock<IAzureTable<SurveyRow>>();
            mock.SetupGet(t => t.Query).Returns(surveyRowsToReturn.AsQueryable());
            var store = new SurveyStore(mock.Object, default(IAzureTable<QuestionRow>), null);

            var actualSurveys = store.GetSurveysByTenant("tenant");

            Assert.AreEqual("title", actualSurveys.First().Title);
        }

        [TestMethod]
        public void GetSurveysByTenantReturnsSlugName()
        {
            var surveyRow = new SurveyRow { PartitionKey = "tenant", SlugName = "slug" };
            var surveyRowsToReturn = new[] { surveyRow };
            var mock = new Mock<IAzureTable<SurveyRow>>();
            mock.SetupGet(t => t.Query).Returns(surveyRowsToReturn.AsQueryable());
            var store = new SurveyStore(mock.Object, default(IAzureTable<QuestionRow>), null);

            var actualSurveys = store.GetSurveysByTenant("tenant");

            Assert.AreEqual("slug", actualSurveys.First().SlugName);
        }

        [TestMethod]
        public void GetSurveysByTenantReturnsCreatedOn()
        {
            var expectedDate = new DateTime(2000, 1, 1);
            var surveyRow = new SurveyRow { PartitionKey = "tenant", CreatedOn = expectedDate };
            var surveyRowsToReturn = new[] { surveyRow };
            var mock = new Mock<IAzureTable<SurveyRow>>();
            mock.SetupGet(t => t.Query).Returns(surveyRowsToReturn.AsQueryable());
            var store = new SurveyStore(mock.Object, default(IAzureTable<QuestionRow>), null);

            var actualSurveys = store.GetSurveysByTenant("tenant");

            Assert.AreEqual(expectedDate, actualSurveys.First().CreatedOn);
        }

        [TestMethod]
        public void GetSurveyByTenantAndSlugNameFiltersByTenantAndSlugNameInRowKey()
        {
            string expectedRowKey = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", "tenant", "slug-name");
            var surveyRow = new SurveyRow { RowKey = expectedRowKey };
            var otherSurveyRow = new SurveyRow { RowKey = "other_row_key" };
            var surveyRowsForTheQuery = new[] { surveyRow, otherSurveyRow };
            var mockSurveyTable = new Mock<IAzureTable<SurveyRow>>();
            mockSurveyTable.SetupGet(t => t.Query).Returns(surveyRowsForTheQuery.AsQueryable()).Verifiable();
            var store = new SurveyStore(mockSurveyTable.Object, default(IAzureTable<QuestionRow>), null);

            var survey = store.GetSurveyByTenantAndSlugName("tenant", "slug-name", false);

            Assert.IsNotNull(survey);
            mockSurveyTable.Verify();
        }

        [TestMethod]
        public void GetSurveyByTenantAndSlugNameReturnsTenantNameFromPartitionKey()
        {
            string expectedRowKey = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", "tenant", "slug-name");
            var surveyRow = new SurveyRow { RowKey = expectedRowKey, PartitionKey = "tenant" };
            var surveyRowsForTheQuery = new[] { surveyRow };
            var mock = new Mock<IAzureTable<SurveyRow>>();
            mock.SetupGet(t => t.Query).Returns(surveyRowsForTheQuery.AsQueryable());
            var store = new SurveyStore(mock.Object, default(IAzureTable<QuestionRow>), null);

            var survey = store.GetSurveyByTenantAndSlugName("tenant", "slug-name", false);

            Assert.AreEqual("tenant", survey.Tenant);
        }

        [TestMethod]
        public void GetSurveyByTenantAndSlugNameReturnsTitle()
        {
            string expectedRowKey = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", "tenant", "slug-name");
            var surveyRow = new SurveyRow { RowKey = expectedRowKey, Title = "title" };
            var surveyRowsForTheQuery = new[] { surveyRow };
            var mock = new Mock<IAzureTable<SurveyRow>>();
            mock.SetupGet(t => t.Query).Returns(surveyRowsForTheQuery.AsQueryable());
            var store = new SurveyStore(mock.Object, default(IAzureTable<QuestionRow>), null);

            var survey = store.GetSurveyByTenantAndSlugName("tenant", "slug-name", false);

            Assert.AreEqual("title", survey.Title);
        }

        [TestMethod]
        public void GetSurveyByTenantAndSlugNameReturnsSlugName()
        {
            string expectedRowKey = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", "tenant", "slug-name");
            var surveyRow = new SurveyRow { RowKey = expectedRowKey, SlugName = "slug-name" };
            var surveyRowsForTheQuery = new[] { surveyRow };
            var mock = new Mock<IAzureTable<SurveyRow>>();
            mock.SetupGet(t => t.Query).Returns(surveyRowsForTheQuery.AsQueryable());
            var store = new SurveyStore(mock.Object, default(IAzureTable<QuestionRow>), null);

            var survey = store.GetSurveyByTenantAndSlugName("tenant", "slug-name", false);

            Assert.AreEqual("slug-name", survey.SlugName);
        }

        [TestMethod]
        public void GetSurveyByTenantAndSlugNameReturnsCreatedOn()
        {
            string expectedRowKey = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", "tenant", "slug-name");
            var expectedDate = new DateTime(2000, 1, 1);
            var surveyRow = new SurveyRow { RowKey = expectedRowKey, CreatedOn = expectedDate };
            var surveyRowsForTheQuery = new[] { surveyRow };
            var mock = new Mock<IAzureTable<SurveyRow>>();
            mock.SetupGet(t => t.Query).Returns(surveyRowsForTheQuery.AsQueryable());
            var store = new SurveyStore(mock.Object, default(IAzureTable<QuestionRow>), null);
            var survey = store.GetSurveyByTenantAndSlugName("tenant", "slug-name", false);

            Assert.AreEqual(expectedDate, survey.CreatedOn);
        }

        [TestMethod]
        public void GetSurveyByTenantAndSlugNameReturnsNullWhenNotFound()
        {
            var otherSurveyRow = new SurveyRow { RowKey = "other_row_key" };
            var surveyRowsForTheQuery = new[] { otherSurveyRow };
            var mock = new Mock<IAzureTable<SurveyRow>>();
            mock.SetupGet(t => t.Query).Returns(surveyRowsForTheQuery.AsQueryable());
            var store = new SurveyStore(mock.Object, default(IAzureTable<QuestionRow>), null);

            var survey = store.GetSurveyByTenantAndSlugName("tenant", "slug-name", false);

            Assert.IsNull(survey);
        }

        [TestMethod]
        public void GetSurveyByTenantAndSlugNameReturnsWithQuestionsFilteredByTenantAndSlugNameInPartitionKey()
        {
            string expectedKey = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", "tenant", "slug-name");
            var surveyRow = new SurveyRow { RowKey = expectedKey };
            var surveyRowsForTheQuery = new[] { surveyRow };
            var mockSurveyTable = new Mock<IAzureTable<SurveyRow>>();
            mockSurveyTable.SetupGet(t => t.Query).Returns(surveyRowsForTheQuery.AsQueryable());
            var questionRow = new QuestionRow { PartitionKey = expectedKey, Type = Enum.GetName(typeof(QuestionType), QuestionType.SimpleText) };
            var otherQuestionRow = new QuestionRow { PartitionKey = "other_partition_key", Type = Enum.GetName(typeof(QuestionType), QuestionType.SimpleText) };
            var questionsForTheQuery = new[] { questionRow, otherQuestionRow };
            var mockQuestionTable = new Mock<IAzureTable<QuestionRow>>();
            mockQuestionTable.SetupGet(t => t.Query).Returns(questionsForTheQuery.AsQueryable());
            var store = new SurveyStore(mockSurveyTable.Object, mockQuestionTable.Object, null);

            var survey = store.GetSurveyByTenantAndSlugName("tenant", "slug-name", true);

            Assert.AreEqual(1, survey.Questions.Count);
        }

        [TestMethod]
        public void GetSurveyByTenantAndSlugNameReturnsWithQuestionText()
        {
            string expectedKey = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", "tenant", "slug-name");
            var surveyRow = new SurveyRow { RowKey = expectedKey };
            var surveyRowsForTheQuery = new[] { surveyRow };
            var mockSurveyTable = new Mock<IAzureTable<SurveyRow>>();
            mockSurveyTable.SetupGet(t => t.Query).Returns(surveyRowsForTheQuery.AsQueryable());
            var questionRow = new QuestionRow { PartitionKey = expectedKey, Text = "text", Type = Enum.GetName(typeof(QuestionType), QuestionType.SimpleText) };
            var questionsForTheQuery = new[] { questionRow };
            var mockQuestionTable = new Mock<IAzureTable<QuestionRow>>();
            mockQuestionTable.SetupGet(t => t.Query).Returns(questionsForTheQuery.AsQueryable());
            var store = new SurveyStore(mockSurveyTable.Object, mockQuestionTable.Object, null);

            var survey = store.GetSurveyByTenantAndSlugName("tenant", "slug-name", true);

            Assert.AreEqual("text", survey.Questions.First().Text);
        }

        [TestMethod]
        public void GetSurveyByTenantAndSlugNameReturnsWithQuestionType()
        {
            string expectedKey = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", "tenant", "slug-name");
            var surveyRow = new SurveyRow { RowKey = expectedKey };
            var surveyRowsForTheQuery = new[] { surveyRow };
            var mockSurveyTable = new Mock<IAzureTable<SurveyRow>>();
            mockSurveyTable.SetupGet(t => t.Query).Returns(surveyRowsForTheQuery.AsQueryable());
            var questionRow = new QuestionRow { PartitionKey = expectedKey, Type = Enum.GetName(typeof(QuestionType), QuestionType.SimpleText) };
            var questionsForTheQuery = new[] { questionRow };
            var mockQuestionTable = new Mock<IAzureTable<QuestionRow>>();
            mockQuestionTable.SetupGet(t => t.Query).Returns(questionsForTheQuery.AsQueryable());
            var store = new SurveyStore(mockSurveyTable.Object, mockQuestionTable.Object, null);

            var survey = store.GetSurveyByTenantAndSlugName("tenant", "slug-name", true);

            Assert.AreEqual(QuestionType.SimpleText, survey.Questions.First().Type);
        }

        [TestMethod]
        public void GetSurveyByTenantAndSlugNameReturnsWithQuestionPossibleAnswers()
        {
            string expectedKey = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", "tenant", "slug-name");
            var surveyRow = new SurveyRow { RowKey = expectedKey };
            var surveyRowsForTheQuery = new[] { surveyRow };
            var mockSurveyTable = new Mock<IAzureTable<SurveyRow>>();
            mockSurveyTable.SetupGet(t => t.Query).Returns(surveyRowsForTheQuery.AsQueryable());
            var questionRow = new QuestionRow { PartitionKey = expectedKey, PossibleAnswers = "possible answers", Type = Enum.GetName(typeof(QuestionType), QuestionType.SimpleText) };
            var questionsForTheQuery = new[] { questionRow };
            var mockQuestionTable = new Mock<IAzureTable<QuestionRow>>();
            mockQuestionTable.SetupGet(t => t.Query).Returns(questionsForTheQuery.AsQueryable());
            var store = new SurveyStore(mockSurveyTable.Object, mockQuestionTable.Object, null);

            var survey = store.GetSurveyByTenantAndSlugName("tenant", "slug-name", true);

            Assert.AreEqual("possible answers", survey.Questions.First().PossibleAnswers);
        }

        [TestMethod]
        public void DeleteSurveyByTenantAndSlugNameDeletesSurveyWithTenantAndSlugNameInRowKeyFromSurveyTable()
        {
            string expectedRowKey = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", "tenant", "slug-name");
            var surveyRow = new SurveyRow { RowKey = expectedRowKey };
            var otherSurveyRow = new SurveyRow { RowKey = "other_row_key" };
            var surveyRowsForTheQuery = new[] { surveyRow, otherSurveyRow };
            var mockSurveyTable = new Mock<IAzureTable<SurveyRow>>();
            mockSurveyTable.SetupGet(t => t.Query).Returns(surveyRowsForTheQuery.AsQueryable());
            var mockQuestionTable = new Mock<IAzureTable<QuestionRow>>();
            mockQuestionTable.SetupGet(t => t.Query).Returns((new QuestionRow[] { }).AsQueryable());
            var store = new SurveyStore(mockSurveyTable.Object, mockQuestionTable.Object, null);

            store.DeleteSurveyByTenantAndSlugName("tenant", "slug-name");

            mockSurveyTable.Verify(t => t.Delete(It.Is<SurveyRow>(s => s.RowKey == expectedRowKey)));
        }

        [TestMethod]
        public void DeleteSurveyByTenantAndSlugNameDeleteQuestionsByTenantAndSlugNameInPartitionKeyFromQuestionTable()
        {
            string expectedKey = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", "tenant", "slug-name");
            var surveyRow = new SurveyRow { RowKey = expectedKey };
            var otherSurveyRow = new SurveyRow { RowKey = "other_row_key" };
            var surveyRowsForTheQuery = new[] { surveyRow, otherSurveyRow };
            var mockSurveyTable = new Mock<IAzureTable<SurveyRow>>();
            mockSurveyTable.SetupGet(t => t.Query).Returns(surveyRowsForTheQuery.AsQueryable());
            var questionRow = new QuestionRow { PartitionKey = expectedKey, PossibleAnswers = "possible answers", Type = Enum.GetName(typeof(QuestionType), QuestionType.SimpleText) };
            var questionsForTheQuery = new[] { questionRow };
            var mockQuestionTable = new Mock<IAzureTable<QuestionRow>>();
            mockQuestionTable.SetupGet(t => t.Query).Returns(questionsForTheQuery.AsQueryable());
            IEnumerable<QuestionRow> actualQuestionsToDelete = null;
            mockQuestionTable.Setup(t => t.Delete(It.IsAny<IEnumerable<QuestionRow>>()))
                             .Callback<IEnumerable<QuestionRow>>(q => actualQuestionsToDelete = q);

            var store = new SurveyStore(mockSurveyTable.Object, mockQuestionTable.Object, null);

            store.DeleteSurveyByTenantAndSlugName("tenant", "slug-name");

            Assert.AreEqual(1, actualQuestionsToDelete.Count());
            Assert.AreSame(questionRow, actualQuestionsToDelete.First());
        }

        [TestMethod]
        public void DeleteSurveyByTenantAndSlugNameDoesNotDeleteSurveyAndQuestionWhenSurveyDoesNotExist()
        {
            var otherSurveyRow = new SurveyRow { RowKey = "other_row_key" };
            var surveyRowsForTheQuery = new[] { otherSurveyRow };
            var mockSurveyTable = new Mock<IAzureTable<SurveyRow>>();
            mockSurveyTable.SetupGet(t => t.Query).Returns(surveyRowsForTheQuery.AsQueryable());
            var mockQuestionTable = new Mock<IAzureTable<QuestionRow>>();
            mockQuestionTable.SetupGet(t => t.Query).Returns((new QuestionRow[] { }).AsQueryable());
            var store = new SurveyStore(mockSurveyTable.Object, mockQuestionTable.Object, null);

            store.DeleteSurveyByTenantAndSlugName("tenant", "slug-name");

            mockSurveyTable.Verify(t => t.Delete(It.IsAny<SurveyRow>()), Times.Never());
            mockQuestionTable.Verify(t => t.Delete(It.IsAny<IEnumerable<QuestionRow>>()), Times.Never());
        }

        [TestMethod]
        public void SaveSurveyCallsAddFromSurveyTableWithTitle()
        {
            var mockSurveyTable = new Mock<IAzureTable<SurveyRow>>();
            var store = new SurveyStore(mockSurveyTable.Object, new Mock<IAzureTable<QuestionRow>>().Object, new Mock<IAzureQueue<NewSurveyMessage>>().Object);
            var survey = new Survey { Title = "title" };

            store.SaveSurvey(survey);

            mockSurveyTable.Verify(
                c => c.Add(
                    It.Is<SurveyRow>(s => s.Title == "title")),
                Times.Once());
        }

        [TestMethod]
        public void SaveSurveyCallsAddFromSurveyTableWithTenant()
        {
            var mockSurveyTable = new Mock<IAzureTable<SurveyRow>>();
            var store = new SurveyStore(mockSurveyTable.Object, new Mock<IAzureTable<QuestionRow>>().Object, new Mock<IAzureQueue<NewSurveyMessage>>().Object);
            var survey = new Survey { Title = "title", Tenant = "tenant" };

            store.SaveSurvey(survey);

            mockSurveyTable.Verify(
                c => c.Add(
                    It.Is<SurveyRow>(s => s.PartitionKey == "tenant")),
                Times.Once());
        }

        [TestMethod]
        public void SaveSurveyCallsAddFromSurveyTableGeneratingTheSlugName()
        {
            var mockSurveyTable = new Mock<IAzureTable<SurveyRow>>();
            var store = new SurveyStore(mockSurveyTable.Object, new Mock<IAzureTable<QuestionRow>>().Object, new Mock<IAzureQueue<NewSurveyMessage>>().Object);
            var survey = new Survey { Title = "title to slug" };

            store.SaveSurvey(survey);

            mockSurveyTable.Verify(
                c => c.Add(
                    It.Is<SurveyRow>(s => s.SlugName == "title-to-slug")),
                Times.Once());
        }

        [TestMethod]
        public void SaveSurveyCallsAddFromSurveyTableSettingTheTenantAsThePartitionKey()
        {
            var mockSurveyTable = new Mock<IAzureTable<SurveyRow>>();
            var store = new SurveyStore(mockSurveyTable.Object, new Mock<IAzureTable<QuestionRow>>().Object, new Mock<IAzureQueue<NewSurveyMessage>>().Object);
            var survey = new Survey { Title = "title", Tenant = "tenant" };

            store.SaveSurvey(survey);

            mockSurveyTable.Verify(
                c => c.Add(
                    It.Is<SurveyRow>(s => s.PartitionKey == "tenant")),
                Times.Once());
        }

        [TestMethod]
        public void SaveSurveyCallsAddFromSurveyTableSettingTheTenantAndSlugNameAsTheRowKey()
        {
            var mockSurveyTable = new Mock<IAzureTable<SurveyRow>>();
            var store = new SurveyStore(mockSurveyTable.Object, new Mock<IAzureTable<QuestionRow>>().Object, new Mock<IAzureQueue<NewSurveyMessage>>().Object);
            var survey = new Survey { Title = "title to slug", Tenant = "tenant" };

            store.SaveSurvey(survey);

            mockSurveyTable.Verify(
                c => c.Add(
                    It.Is<SurveyRow>(s => s.RowKey == "tenant_title-to-slug")),
                Times.Once());
        }

        [TestMethod]
        public void SaveSurveyCallsAddFromQuestionTableWithAllTheQuestions()
        {
            var mockQuestionTable = new Mock<IAzureTable<QuestionRow>>();
            var store = new SurveyStore(new Mock<IAzureTable<SurveyRow>>().Object, mockQuestionTable.Object, new Mock<IAzureQueue<NewSurveyMessage>>().Object);
            var survey = new Survey 
            { 
                Title = "title",
                Questions = new List<Question>(new[] { new Question(), new Question() })
            };
            IEnumerable<QuestionRow> actualQuestionsToAdd = null;
            mockQuestionTable.Setup(t => t.Add(It.IsAny<IEnumerable<QuestionRow>>()))
                             .Callback<IEnumerable<QuestionRow>>(q => actualQuestionsToAdd = q);
            
            store.SaveSurvey(survey);

            Assert.AreEqual(2, actualQuestionsToAdd.Count());
        }

        [TestMethod]
        public void SaveSurveyCallsAddFromQuestionTableWithQuestionText()
        {
            var mockQuestionTable = new Mock<IAzureTable<QuestionRow>>();
            var store = new SurveyStore(new Mock<IAzureTable<SurveyRow>>().Object, mockQuestionTable.Object, new Mock<IAzureQueue<NewSurveyMessage>>().Object);
            var question = new Question { Text = "text" };
            var survey = new Survey
            {
                Title = "title",
                Questions = new List<Question>(new[] { question })
            };
            IEnumerable<QuestionRow> actualQuestionsToAdd = null;
            mockQuestionTable.Setup(t => t.Add(It.IsAny<IEnumerable<QuestionRow>>()))
                             .Callback<IEnumerable<QuestionRow>>(q => actualQuestionsToAdd = q);

            store.SaveSurvey(survey);

            Assert.AreEqual("text", actualQuestionsToAdd.First().Text);
        }

        [TestMethod]
        public void SaveSurveyCallsAddFromQuestionTableWithQuestionType()
        {
            var mockQuestionTable = new Mock<IAzureTable<QuestionRow>>();
            var store = new SurveyStore(new Mock<IAzureTable<SurveyRow>>().Object, mockQuestionTable.Object, new Mock<IAzureQueue<NewSurveyMessage>>().Object);
            var question = new Question { Type = QuestionType.SimpleText };
            var survey = new Survey
            {
                Title = "title",
                Questions = new List<Question>(new[] { question })
            };
            IEnumerable<QuestionRow> actualQuestionsToAdd = null;
            mockQuestionTable.Setup(t => t.Add(It.IsAny<IEnumerable<QuestionRow>>()))
                             .Callback<IEnumerable<QuestionRow>>(q => actualQuestionsToAdd = q);

            store.SaveSurvey(survey);

            Assert.AreEqual(Enum.GetName(typeof(QuestionType), QuestionType.SimpleText), actualQuestionsToAdd.First().Type);
        }

        [TestMethod]
        public void SaveSurveyCallsAddFromQuestionTableWithQuestionPossibleAnswers()
        {
            var mockQuestionTable = new Mock<IAzureTable<QuestionRow>>();
            var store = new SurveyStore(new Mock<IAzureTable<SurveyRow>>().Object, mockQuestionTable.Object, new Mock<IAzureQueue<NewSurveyMessage>>().Object);
            var question = new Question { PossibleAnswers = "possible answers" };
            var survey = new Survey
            {
                Title = "title",
                Questions = new List<Question>(new[] { question })
            };
            IEnumerable<QuestionRow> actualQuestionsToAdd = null;
            mockQuestionTable.Setup(t => t.Add(It.IsAny<IEnumerable<QuestionRow>>()))
                             .Callback<IEnumerable<QuestionRow>>(q => actualQuestionsToAdd = q);

            store.SaveSurvey(survey);

            Assert.AreEqual("possible answers", actualQuestionsToAdd.First().PossibleAnswers);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SaveSurveyThrowsExceptionIfTitleAndSlugNameAreNullOrEmpty()
        {
            var store = new SurveyStore(new Mock<IAzureTable<SurveyRow>>().Object, new Mock<IAzureTable<QuestionRow>>().Object, null);
            var survey = new Survey();

            store.SaveSurvey(survey);
        }

        [TestMethod]
        public void SaveSurveyAddsMessageToQueueWithSlugNameWhenSlugNameIsEmpty()
        {
            var mockNewSurveyQueue = new Mock<IAzureQueue<NewSurveyMessage>>();
            var store = new SurveyStore(new Mock<IAzureTable<SurveyRow>>().Object, new Mock<IAzureTable<QuestionRow>>().Object, mockNewSurveyQueue.Object);
            var survey = new Survey(string.Empty) { Title = "title" };

            store.SaveSurvey(survey);

            mockNewSurveyQueue.Verify(q => q.AddMessage(It.Is<NewSurveyMessage>(m => m.SlugName == "title")));
        }

        [TestMethod]
        public void SaveSurveyAddsMessageToQueueWithTenantWhenSlugNameIsEmpty()
        {
            var mockNewSurveyQueue = new Mock<IAzureQueue<NewSurveyMessage>>();
            var store = new SurveyStore(new Mock<IAzureTable<SurveyRow>>().Object, new Mock<IAzureTable<QuestionRow>>().Object, mockNewSurveyQueue.Object);
            var survey = new Survey(string.Empty) { Tenant = "tenant", Title = "title" };

            store.SaveSurvey(survey);

            mockNewSurveyQueue.Verify(q => q.AddMessage(It.Is<NewSurveyMessage>(m => m.Tenant == "tenant")));
        }

        [TestMethod]
        public void GetRecentSurveysReturnsUpto10Surveys()
        {
            var surveyRowsToReturn = new List<SurveyRow>();
            for (int i = 1; i <= 11; i++)
            {
                surveyRowsToReturn.Add(new SurveyRow());
            }

            var mock = new Mock<IAzureTable<SurveyRow>>();
            mock.SetupGet(t => t.Query).Returns(surveyRowsToReturn.AsQueryable());
            var store = new SurveyStore(mock.Object, default(IAzureTable<QuestionRow>), null);

            var actualSurveys = store.GetRecentSurveys();

            Assert.AreEqual(10, actualSurveys.Count());
        }

        [TestMethod]
        public void GetRecentSurveysReturnsTenantNameFromThePartitionKey()
        {
            var surveyRow = new SurveyRow { PartitionKey = "tenant" };
            var surveyRowsToReturn = new[] { surveyRow };
            var mock = new Mock<IAzureTable<SurveyRow>>();
            mock.SetupGet(t => t.Query).Returns(surveyRowsToReturn.AsQueryable());
            var store = new SurveyStore(mock.Object, default(IAzureTable<QuestionRow>), null);

            var actualSurveys = store.GetRecentSurveys();

            Assert.AreEqual("tenant", actualSurveys.First().Tenant);
        }

        [TestMethod]
        public void GetRecentSurveysReturnsTitle()
        {
            var surveyRow = new SurveyRow { PartitionKey = "tenant", Title = "title" };
            var surveyRowsToReturn = new[] { surveyRow };
            var mock = new Mock<IAzureTable<SurveyRow>>();
            mock.SetupGet(t => t.Query).Returns(surveyRowsToReturn.AsQueryable());
            var store = new SurveyStore(mock.Object, default(IAzureTable<QuestionRow>), null);

            var actualSurveys = store.GetRecentSurveys();

            Assert.AreEqual("title", actualSurveys.First().Title);
        }

        [TestMethod]
        public void GetRecentSurveysReturnsSlugName()
        {
            var surveyRow = new SurveyRow { PartitionKey = "tenant", SlugName = "slug" };
            var surveyRowsToReturn = new[] { surveyRow };
            var mock = new Mock<IAzureTable<SurveyRow>>();
            mock.SetupGet(t => t.Query).Returns(surveyRowsToReturn.AsQueryable());
            var store = new SurveyStore(mock.Object, default(IAzureTable<QuestionRow>), null);

            var actualSurveys = store.GetRecentSurveys();

            Assert.AreEqual("slug", actualSurveys.First().SlugName);
        }
    }
}