namespace TailSpin.PhoneClient.Services.Stores
{
    using System.Collections.Generic;
    using TailSpin.PhoneClient.Models;

    public class NullSurveyStore : ISurveyStore
    {
        public string LastSyncDate { get; set; }

        public void DeleteSurveyAnswers(IEnumerable<SurveyAnswer> surveyAnswers)
        {
        }

        public IEnumerable<SurveyAnswer> GetCompleteSurveyAnswers()
        {
            return new List<SurveyAnswer>();
        }

        public SurveyAnswer GetCurrentAnswerForTemplate(SurveyTemplate template)
        {
            return new SurveyAnswer();
        }

        public IEnumerable<SurveyTemplate> GetSurveyTemplates()
        {
            return new List<SurveyTemplate>();
        }

        public void SaveStore()
        {
        }

        public void SaveSurveyAnswer(SurveyAnswer answer)
        {
        }

        public void SaveSurveyTemplates(IEnumerable<SurveyTemplate> surveys)
        {
        }
    }
}