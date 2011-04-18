




 




namespace TailSpin.Web.Survey.Shared.Stores.AzureStorage
{
    using System;
    using Microsoft.WindowsAzure.StorageClient;

    public class SurveyRow : TableServiceEntity
    {
        public string SlugName { get; set; }

        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}