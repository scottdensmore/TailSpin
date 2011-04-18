




 




namespace TailSpin.Web.AcceptanceTests.Stores.AzureStorage
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Survey.Shared;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    [TestClass]
    public class EntitiesBlobContainerFixture
    {
        private const string SurveyAnswersContainer = "surveyanswersfortest";

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            var surveyAnswerStorage = new EntitiesBlobContainer<SurveyAnswer>(account, SurveyAnswersContainer);
            surveyAnswerStorage.EnsureExist();
        }

        [TestMethod]
        public void SaveAndGet()
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            var surveyAnswerStorage = new EntitiesBlobContainer<SurveyAnswer>(account, SurveyAnswersContainer);
            var surveyAnswerId = new Guid("{DB0298D2-432B-41A7-B80F-7E7A025FA267}").ToString();
            var expectedSurveyAnswer = new SurveyAnswer { Tenant = "tenant", Title = "title", SlugName = "slugname" };
            var question1 = new QuestionAnswer { QuestionText = "text 1", QuestionType = QuestionType.SimpleText, PossibleAnswers = string.Empty };
            var question2 = new QuestionAnswer { QuestionText = "text 2", QuestionType = QuestionType.MultipleChoice, PossibleAnswers = "answer 1\nanswer2" };
            var question3 = new QuestionAnswer { QuestionText = "text 3", QuestionType = QuestionType.FiveStars, PossibleAnswers = string.Empty };
            expectedSurveyAnswer.QuestionAnswers.AddRange(new[] { question1, question2, question3 });

            surveyAnswerStorage.Save(surveyAnswerId, expectedSurveyAnswer);
            SurveyAnswer actualSurveyAnswer = surveyAnswerStorage.Get(surveyAnswerId);

            Assert.AreEqual(expectedSurveyAnswer.Tenant, actualSurveyAnswer.Tenant);
            Assert.AreEqual(expectedSurveyAnswer.Title, actualSurveyAnswer.Title);
            Assert.AreEqual(expectedSurveyAnswer.SlugName, actualSurveyAnswer.SlugName);
            Assert.AreEqual(3, actualSurveyAnswer.QuestionAnswers.Count);
            var actualQuestionAnswer1 = actualSurveyAnswer.QuestionAnswers.SingleOrDefault(q =>
                q.QuestionText == "text 1" &&
                q.QuestionType == QuestionType.SimpleText &&
                q.PossibleAnswers == string.Empty);
            Assert.IsNotNull(actualQuestionAnswer1);
            var actualQuestionAnswer2 = actualSurveyAnswer.QuestionAnswers.SingleOrDefault(q =>
                q.QuestionText == "text 2" &&
                q.QuestionType == QuestionType.MultipleChoice &&
                q.PossibleAnswers == "answer 1\nanswer2");
            Assert.IsNotNull(actualQuestionAnswer2);
            var actualQuestionAnswer3 = actualSurveyAnswer.QuestionAnswers.SingleOrDefault(q =>
                q.QuestionText == "text 3" &&
                q.QuestionType == QuestionType.FiveStars &&
                q.PossibleAnswers == string.Empty);
            Assert.IsNotNull(actualQuestionAnswer3);
        }

        [TestMethod]
        public void SaveAndDelete()
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            var surveyAnswerStorage = new EntitiesBlobContainer<SurveyAnswer>(account, SurveyAnswersContainer);
            var surveyAnswerId = new Guid("{A0E27D4B-4CD9-43B0-B29E-FE61096A529A}").ToString();
            var expectedSurveyAnswer = new SurveyAnswer { Tenant = "tenant", Title = "title", SlugName = "slugname" };

            surveyAnswerStorage.Save(surveyAnswerId, expectedSurveyAnswer);
            SurveyAnswer savedSurveyAnswer = surveyAnswerStorage.Get(surveyAnswerId);
            Assert.IsNotNull(savedSurveyAnswer);

            surveyAnswerStorage.Delete(surveyAnswerId);
            SurveyAnswer deletedSurveyAnswer = surveyAnswerStorage.Get(surveyAnswerId);
            Assert.IsNull(deletedSurveyAnswer);
        }

        [TestMethod]
        public void GetReturnsNullWhenItDoesNotExist()
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            var storage = new EntitiesBlobContainer<object>(account);
            var nonExistingObject = "id-for-non-existing-object";

            object actualObject = storage.Get(nonExistingObject);

            Assert.IsNull(actualObject);
        }
    }
}
