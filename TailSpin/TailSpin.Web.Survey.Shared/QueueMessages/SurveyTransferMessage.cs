




 




namespace TailSpin.Web.Survey.Shared.QueueMessages
{
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    public class SurveyTransferMessage : AzureQueueMessage
    {
        public string Tenant { get; set; }
        public string SlugName { get; set; }
    }
}