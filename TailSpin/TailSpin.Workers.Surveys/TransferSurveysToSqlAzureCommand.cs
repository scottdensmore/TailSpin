




 




namespace TailSpin.Workers.Surveys
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TailSpin.Web.Survey.Shared.Commands;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.QueueMessages;
    using TailSpin.Web.Survey.Shared.Stores;

    public class TransferSurveysToSqlAzureCommand : ICommand<SurveyTransferMessage>
    {
        private readonly ISurveyAnswerStore surveyAnswerStore;
        private readonly ISurveyStore surveyStore;
        private readonly ISurveySqlStore surveySqlStore;
        private readonly ITenantStore tenantStore;

        public TransferSurveysToSqlAzureCommand(
            ISurveyAnswerStore surveyAnswerStore, 
            ISurveyStore surveyStore, 
            ITenantStore tenantStore, 
            ISurveySqlStore surveySqlStore)
        {
            this.surveyAnswerStore = surveyAnswerStore;
            this.surveyStore = surveyStore;
            this.tenantStore = tenantStore;
            this.surveySqlStore = surveySqlStore;
        }

        public void Run(SurveyTransferMessage message)
        {
            Tenant tenant = this.tenantStore.GetTenant(message.Tenant);
            this.surveySqlStore.Reset(tenant.SqlAzureConnectionString, message.Tenant, message.SlugName);

            Survey surveyWithQuestions = this.surveyStore.GetSurveyByTenantAndSlugName(message.Tenant, message.SlugName, true);
            
            IEnumerable<string> answerIds = this.surveyAnswerStore.GetSurveyAnswerIds(message.Tenant, surveyWithQuestions.SlugName);

            SurveyData surveyData = surveyWithQuestions.ToDataModel();

            foreach (var answerId in answerIds)
            {
                SurveyAnswer surveyAnswer = this.surveyAnswerStore.GetSurveyAnswer(surveyWithQuestions.Tenant, surveyWithQuestions.SlugName, answerId);
                
                var responseData = new ResponseData { Id = Guid.NewGuid().ToString(), CreatedOn = surveyAnswer.CreatedOn };
                foreach (var answer in surveyAnswer.QuestionAnswers)
                {
                    QuestionAnswer answerCopy = answer;
                    var questionResponseData = new QuestionResponseData
                                                    {
                                                        QuestionId = (from question in surveyData.QuestionDatas
                                                                        where question.QuestionText == answerCopy.QuestionText
                                                                        select question.Id).FirstOrDefault(),
                                                        Answer = answer.Answer
                                                    };

                    responseData.QuestionResponseDatas.Add(questionResponseData);
                }

                if (responseData.QuestionResponseDatas.Count > 0)
                {
                    surveyData.ResponseDatas.Add(responseData);
                }
            }

            this.surveySqlStore.SaveSurvey(tenant.SqlAzureConnectionString, surveyData);
        }
    }
}