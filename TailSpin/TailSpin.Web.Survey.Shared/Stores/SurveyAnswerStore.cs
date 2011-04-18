




 




namespace TailSpin.Web.Survey.Shared.Stores
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using AzureStorage;
    using QueueMessages;
    using TailSpin.Web.Survey.Shared.Models;

    public class SurveyAnswerStore : ISurveyAnswerStore
    {
        private readonly ISurveyAnswerContainerFactory surveyAnswerContainerFactory;
        private readonly IAzureQueue<SurveyAnswerStoredMessage> surveyAnswerStoredQueue;
        private readonly IAzureBlobContainer<List<string>> surveyAnswerIdsListContainer;
        private readonly IMediaAnswerStore mediaAnswerStore;

        public SurveyAnswerStore(
            ISurveyAnswerContainerFactory surveyAnswerContainerFactory, 
            IAzureQueue<SurveyAnswerStoredMessage> surveyAnswerStoredQueue, 
            IAzureBlobContainer<List<string>> surveyAnswerIdsListContainer,
            IMediaAnswerStore mediaAnswerStore)
        {
            this.surveyAnswerContainerFactory = surveyAnswerContainerFactory;
            this.surveyAnswerStoredQueue = surveyAnswerStoredQueue;
            this.surveyAnswerIdsListContainer = surveyAnswerIdsListContainer;
            this.mediaAnswerStore = mediaAnswerStore;
        }

        public void Initialize()
        {
            this.surveyAnswerStoredQueue.EnsureExist();
            this.surveyAnswerIdsListContainer.EnsureExist();
        }

        public void SaveSurveyAnswer(SurveyAnswer surveyAnswer)
        {
            var surveyAnswerBlobContainer = this.surveyAnswerContainerFactory.Create(surveyAnswer.Tenant, surveyAnswer.SlugName);
            surveyAnswerBlobContainer.EnsureExist();
            DateTime now = DateTime.UtcNow;
            surveyAnswer.CreatedOn = now;
            var blobId = now.GetFormatedTicks();
            surveyAnswerBlobContainer.Save(blobId, surveyAnswer);
            this.surveyAnswerStoredQueue.AddMessage(
                new SurveyAnswerStoredMessage
                    {
                        SurveyAnswerBlobId = blobId,
                        Tenant = surveyAnswer.Tenant,
                        SurveySlugName = surveyAnswer.SlugName
                    });
        }

        public SurveyAnswer GetSurveyAnswer(string tenant, string slugName, string surveyAnswerId)
        {
            var surveyBlobContainer = this.surveyAnswerContainerFactory.Create(tenant, slugName);
            surveyBlobContainer.EnsureExist();
            return surveyBlobContainer.Get(surveyAnswerId);
        }

        public string GetFirstSurveyAnswerId(string tenant, string slugName)
        {
            string id = string.Format(CultureInfo.InvariantCulture, "{0}-{1}", tenant, slugName);
            var answerIdList = this.surveyAnswerIdsListContainer.Get(id);

            if (answerIdList != null)
            {
                return answerIdList[0];
            }

            return string.Empty;
        }

        public void AppendSurveyAnswerIdToAnswersList(string tenant, string slugName, string surveyAnswerId)
        {
            string id = string.Format(CultureInfo.InvariantCulture, "{0}-{1}", tenant, slugName);
            var answerIdList = this.surveyAnswerIdsListContainer.Get(id) ?? new List<string>(1);
            answerIdList.Add(surveyAnswerId);
            this.surveyAnswerIdsListContainer.Save(id, answerIdList);
        }

        public SurveyAnswerBrowsingContext GetSurveyAnswerBrowsingContext(string tenant, string slugName, string answerId)
        {
            string id = string.Format(CultureInfo.InvariantCulture, "{0}-{1}", tenant, slugName);
            var answerIdsList = this.surveyAnswerIdsListContainer.Get(id);

            string previousId = null;
            string nextId = null;
            if (answerIdsList != null)
            {
                var currentAnswerIndex = answerIdsList.FindIndex(i => i == answerId);

                if (currentAnswerIndex - 1 >= 0)
                {
                    previousId = answerIdsList[currentAnswerIndex - 1];
                }

                if (currentAnswerIndex + 1 <= answerIdsList.Count - 1)
                {
                    nextId = answerIdsList[currentAnswerIndex + 1];
                }
            }

            return new SurveyAnswerBrowsingContext
                       {
                           PreviousId = previousId,
                           NextId = nextId
                       };
        }

        public IEnumerable<string> GetSurveyAnswerIds(string tenant, string slugName)
        {
            string id = string.Format(CultureInfo.InvariantCulture, "{0}-{1}", tenant, slugName);
            return this.surveyAnswerIdsListContainer.Get(id);
        }

        public void DeleteSurveyAnswers(string tenant, string slugName)
        {
            var surveyAnswersBlobContainer = this.surveyAnswerContainerFactory.Create(tenant, slugName);
            surveyAnswersBlobContainer.EnsureExist();

            var mediaAnswerIds =
                from answers in surveyAnswersBlobContainer.GetAll()
                from answer in answers.QuestionAnswers
                where
                    answer.QuestionType == QuestionType.Picture ||
                    answer.QuestionType == QuestionType.Voice
                select answer;

            foreach (var answer in mediaAnswerIds)
            {
                if (!string.IsNullOrEmpty(answer.Answer))
                {
                    this.mediaAnswerStore.DeleteMediaAnswer(
                        answer.Answer.Split('/').Last(),
                        answer.QuestionType);
                }
            }

            surveyAnswersBlobContainer.DeleteContainer();

            string id = string.Format(CultureInfo.InvariantCulture, "{0}-{1}", tenant, slugName);
            this.surveyAnswerIdsListContainer.Delete(id);
        }
    }
}