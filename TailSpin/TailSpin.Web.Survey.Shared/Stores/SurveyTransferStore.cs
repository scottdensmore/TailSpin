




 




namespace TailSpin.Web.Survey.Shared.Stores
{
    using AzureStorage;
    using QueueMessages;

    public class SurveyTransferStore : ISurveyTransferStore
    {
        private readonly IAzureQueue<SurveyTransferMessage> surveyTransferQueue;

        public SurveyTransferStore(IAzureQueue<SurveyTransferMessage> surveyTransferQueue)
        {
            this.surveyTransferQueue = surveyTransferQueue;
        }

        public void Initialize()
        {
            this.surveyTransferQueue.EnsureExist();
        }

        public void Transfer(string tenant, string slugName)
        {
            this.surveyTransferQueue.AddMessage(new SurveyTransferMessage { Tenant = tenant, SlugName = slugName });
        }
    }
}