




 




namespace TailSpin.Workers.Surveys.Tests
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.QueueMessages;
    using TailSpin.Web.Survey.Shared.Stores;

    [TestClass]
    public class TransferSurveysToSqlAzureCommandFixture
    {
        [TestMethod]
        public void RunGetsSurveyFromStore()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var mockTenantStore = new Mock<ITenantStore>();
            var mockSurveySqlStore = new Mock<ISurveySqlStore>();
            var command = new TransferSurveysToSqlAzureCommand(mockSurveyAnswerStore.Object, mockSurveyStore.Object, mockTenantStore.Object, mockSurveySqlStore.Object);
            var message = new SurveyTransferMessage { Tenant = "tenant", SlugName = "slugName" };
            var tenant = new Tenant { SqlAzureConnectionString = "connectionString" };
            mockTenantStore.Setup(r => r.GetTenant("tenant")).Returns(tenant);
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName("tenant", "slugName", true)).Returns(new Survey());

            command.Run(message);

            mockSurveyStore.Verify(r => r.GetSurveyByTenantAndSlugName(message.Tenant, message.SlugName, true));
        }

        [TestMethod]
        public void RunGetsSurveyAnswerIdsFromStore()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var mockTenantStore = new Mock<ITenantStore>();
            var mockSurveySqlStore = new Mock<ISurveySqlStore>();
            var command = new TransferSurveysToSqlAzureCommand(mockSurveyAnswerStore.Object, mockSurveyStore.Object, mockTenantStore.Object, mockSurveySqlStore.Object);
            var message = new SurveyTransferMessage { Tenant = "tenant", SlugName = "slugName" };
            var survey = new Survey("slugName")
                             {
                                 Tenant = "tenant",
                             };
            survey.Questions.Add(new Question
                                     {
                                         Text = "What is your favorite food?",
                                         PossibleAnswers = "Coffee\nPizza\nSalad",
                                         Type = QuestionType.MultipleChoice
                                     });
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName("tenant", "slugName", true)).Returns(survey);
            var tenant = new Tenant { SqlAzureConnectionString = "connectionString" };
            mockTenantStore.Setup(r => r.GetTenant("tenant")).Returns(tenant);

            command.Run(message);

            mockSurveyAnswerStore.Verify(r => r.GetSurveyAnswerIds(message.Tenant, "slugName"));
        }

        [TestMethod]
        public void RunGetsSurveyAnswersForIdFromStore()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var mockTenantStore = new Mock<ITenantStore>();
            var mockSurveySqlStore = new Mock<ISurveySqlStore>();
            var command = new TransferSurveysToSqlAzureCommand(mockSurveyAnswerStore.Object, mockSurveyStore.Object, mockTenantStore.Object, mockSurveySqlStore.Object);
            var message = new SurveyTransferMessage { Tenant = "tenant", SlugName = "slugName" };
            var survey = new Survey("slugName")
                             {
                                 Tenant = "tenant",
                             };
            survey.Questions.Add(new Question
                                     {
                                         Text = "What is your favorite food?",
                                         PossibleAnswers = "Coffee\nPizza\nSalad",
                                         Type = QuestionType.MultipleChoice
                                     });
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName("tenant", "slugName", true)).Returns(survey);
            mockSurveyStore.Setup(r => r.GetSurveysByTenant("tenant")).Returns(new List<Survey> { survey });
            mockSurveyAnswerStore.Setup(r => r.GetSurveyAnswerIds("tenant", "slugName")).Returns(new List<string> { "id" });
            mockSurveyAnswerStore.Setup(r => r.GetSurveyAnswer("tenant", "slugName", "id")).Returns(new SurveyAnswer());
            var tenant = new Tenant { SqlAzureConnectionString = "connectionString" };
            mockTenantStore.Setup(r => r.GetTenant("tenant")).Returns(tenant);

            command.Run(message);

            mockSurveyAnswerStore.Verify(r => r.GetSurveyAnswer(message.Tenant, "slugName", "id"));
        }

        [TestMethod]
        public void RunGetsTenantFromStore()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var mockTenantStore = new Mock<ITenantStore>();
            var mockSurveySqlStore = new Mock<ISurveySqlStore>();
            var command = new TransferSurveysToSqlAzureCommand(mockSurveyAnswerStore.Object, mockSurveyStore.Object, mockTenantStore.Object, mockSurveySqlStore.Object);
            var message = new SurveyTransferMessage { Tenant = "tenant", SlugName = "slugName" };
            var tenant = new Tenant { SqlAzureConnectionString = "connectionString" };
            mockTenantStore.Setup(r => r.GetTenant("tenant")).Returns(tenant);
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName("tenant", "slugName", true)).Returns(new Survey());

            command.Run(message);

            mockTenantStore.Verify(r => r.GetTenant(message.Tenant));
        }

        [TestMethod]
        public void RunResetsSqlStore()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var mockTenantStore = new Mock<ITenantStore>();
            var mockSurveySqlStore = new Mock<ISurveySqlStore>();
            var command = new TransferSurveysToSqlAzureCommand(mockSurveyAnswerStore.Object, mockSurveyStore.Object, mockTenantStore.Object, mockSurveySqlStore.Object);
            var message = new SurveyTransferMessage { Tenant = "tenant", SlugName = "slugName" };
            var survey = new Survey("slugName")
            {
                Tenant = "tenant",
            };
            survey.Questions.Add(new Question
            {
                Text = "What is your favorite food?",
                PossibleAnswers = "Coffee\nPizza\nSalad",
                Type = QuestionType.MultipleChoice
            });
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName("tenant", "slugName", true)).Returns(survey);
            mockSurveyAnswerStore.Setup(r => r.GetSurveyAnswerIds("tenant", "slugName")).Returns(new List<string> { "id" });
            mockSurveyAnswerStore.Setup(r => r.GetSurveyAnswer("tenant", "slugName", "id")).Returns(new SurveyAnswer());
            var tenant = new Tenant { SqlAzureConnectionString = "connectionString" };
            mockTenantStore.Setup(r => r.GetTenant("tenant")).Returns(tenant);

            command.Run(message);

            mockSurveySqlStore.Verify(r => r.Reset("connectionString", "tenant", "slugName"));
        }

        [TestMethod]
        public void RunSavesToSqlStore()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var mockTenantStore = new Mock<ITenantStore>();
            var mockSurveySqlStore = new Mock<ISurveySqlStore>();
            var command = new TransferSurveysToSqlAzureCommand(mockSurveyAnswerStore.Object, mockSurveyStore.Object, mockTenantStore.Object, mockSurveySqlStore.Object);
            var message = new SurveyTransferMessage { Tenant = "tenant", SlugName = "slugName" };
            var survey = new Survey("slugName")
                             {
                                 Tenant = "tenant",
                             };
            survey.Questions.Add(new Question
                                     {
                                         Text = "What is your favorite food?",
                                         PossibleAnswers = "Coffee\nPizza\nSalad",
                                         Type = QuestionType.MultipleChoice
                                     });
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName("tenant", "slugName", true)).Returns(survey);
            mockSurveyAnswerStore.Setup(r => r.GetSurveyAnswerIds("tenant", "slugName")).Returns(new List<string> { "id" });
            mockSurveyAnswerStore.Setup(r => r.GetSurveyAnswer("tenant", "slugName", "id")).Returns(new SurveyAnswer());
            var tenant = new Tenant { SqlAzureConnectionString = "connectionString" };
            mockTenantStore.Setup(r => r.GetTenant("tenant")).Returns(tenant);

            command.Run(message);

            mockSurveySqlStore.Verify(r => r.SaveSurvey("connectionString", It.IsAny<SurveyData>()));
        }
    }
}