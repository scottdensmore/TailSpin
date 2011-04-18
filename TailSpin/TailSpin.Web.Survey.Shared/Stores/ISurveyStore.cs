




 




namespace TailSpin.Web.Survey.Shared.Stores
{
    using System;
    using System.Collections.Generic;
    using TailSpin.Web.Survey.Shared.Models;

    public interface ISurveyStore
    {
        void Initialize();
        void SaveSurvey(Survey survey);
        void DeleteSurveyByTenantAndSlugName(string tenant, string slugName);
        Survey GetSurveyByTenantAndSlugName(string tenant, string slugName, bool getQuestions);
        IEnumerable<Survey> GetSurveysByTenant(string tenant);
        IEnumerable<Survey> GetRecentSurveys();
        IEnumerable<Survey> GetSurveys(string tenant, DateTime fromDate);
    }
}