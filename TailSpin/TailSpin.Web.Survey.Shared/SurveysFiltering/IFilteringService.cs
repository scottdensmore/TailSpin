




 




namespace TailSpin.Web.Survey.Shared.SurveysFiltering
{
    using System;
    using System.Collections.Generic;
    using TailSpin.Web.Survey.Shared.Models;

    public interface IFilteringService
    {
        IEnumerable<Survey> GetSurveysForUser(string username, DateTime fromDate);
        IEnumerable<string> GetUsersForSurvey(Survey survey);
    }
}