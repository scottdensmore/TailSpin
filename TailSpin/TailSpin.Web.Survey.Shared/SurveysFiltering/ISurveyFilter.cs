




 




namespace TailSpin.Web.Survey.Shared.SurveysFiltering
{
    using System;
    using System.Collections.Generic;
    using TailSpin.Web.Survey.Shared.Models;

    public interface ISurveyFilter
    {
        IEnumerable<Survey> GetSurveys(string username, DateTime fromDate);
        IEnumerable<string> GetUsers(Survey survey);
    }
}