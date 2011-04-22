namespace TailSpin.PhoneClient.Services.Stores
{
    using System.Collections.Generic;
    using TailSpin.PhoneClient.Models;

    public class SurveysList
    {
        public SurveysList()
        {
            this.LastSyncDate = string.Empty;
        }

        public List<SurveyTemplate> Templates { get; set; }

        public List<SurveyAnswer> Answers { get; set; }

        public string LastSyncDate { get; set; }
    }
}
