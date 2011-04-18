




 




namespace TailSpin.Web.Survey.Shared.Stores
{
    using System.Collections.Generic;
    using Models;

    public interface ISurveyAnswerStore
    {
        void Initialize();
        void SaveSurveyAnswer(SurveyAnswer surveyAnswer);
        SurveyAnswer GetSurveyAnswer(string tenant, string slugName, string surveyAnswerId);
        string GetFirstSurveyAnswerId(string tenant, string slugName);
        void AppendSurveyAnswerIdToAnswersList(string tenant, string slugName, string surveyAnswerId);
        SurveyAnswerBrowsingContext GetSurveyAnswerBrowsingContext(string tenant, string slugName, string answerId);
        IEnumerable<string> GetSurveyAnswerIds(string tenant, string slugName);
        void DeleteSurveyAnswers(string tenant, string slugName);
    }
}