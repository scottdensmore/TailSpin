




 




namespace TailSpin.Web.Survey.Shared.QueueMessages
{
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    public class NewSurveyMessage : AzureQueueMessage
    {
        public string SlugName { get; set; }
        public string Tenant { get; set; }
    }
}