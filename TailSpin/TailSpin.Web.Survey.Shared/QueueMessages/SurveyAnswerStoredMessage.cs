




 




namespace TailSpin.Web.Survey.Shared.QueueMessages
{
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    public class SurveyAnswerStoredMessage : AzureQueueMessage
    {
        public string SurveyAnswerBlobId { get; set; }

        public string Tenant { get; set; }

        public string SurveySlugName { get; set; }
    }
}