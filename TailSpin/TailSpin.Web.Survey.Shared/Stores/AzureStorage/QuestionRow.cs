




 




namespace TailSpin.Web.Survey.Shared.Stores.AzureStorage
{
    using Microsoft.WindowsAzure.StorageClient;

    public class QuestionRow : TableServiceEntity
    {
        public string Text { get; set; }

        public string Type { get; set; }

        public string PossibleAnswers { get; set; }
    }
}