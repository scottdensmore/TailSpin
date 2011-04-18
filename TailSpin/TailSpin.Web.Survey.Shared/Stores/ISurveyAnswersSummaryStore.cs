




 




namespace TailSpin.Web.Survey.Shared.Stores
{
    using Models;

    public interface ISurveyAnswersSummaryStore
    {
        void Initialize();
        SurveyAnswersSummary GetSurveyAnswersSummary(string tenant, string slugName);
        void SaveSurveyAnswersSummary(SurveyAnswersSummary surveyAnswersSummary);
        void DeleteSurveyAnswersSummary(string tenant, string slugName);
    }
}
