namespace TailSpin.PhoneClient.Services.Stores
{
    using System.Collections.Generic;
    using Models;

    public interface ISurveyStore
    {
        string LastSyncDate { get; set; }
        IEnumerable<SurveyTemplate> GetSurveyTemplates();
        IEnumerable<SurveyAnswer> GetCompleteSurveyAnswers();
        void SaveSurveyTemplates(IEnumerable<SurveyTemplate> surveys);
        void SaveSurveyAnswer(SurveyAnswer answer);
        SurveyAnswer GetCurrentAnswerForTemplate(SurveyTemplate template);
        void DeleteSurveyAnswers(IEnumerable<SurveyAnswer> surveyAnswers);
        void SaveStore();
    }
}
