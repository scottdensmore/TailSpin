




 




namespace TailSpin.Web.Survey.Shared.SurveysFiltering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TailSpin.Web.Survey.Shared.Models;

    public class FilteringService : IFilteringService
    {
        private readonly ISurveyFilter[] filters;

        public FilteringService(params ISurveyFilter[] filters)
        {
            this.filters = filters;
        }

        public IEnumerable<Survey> GetSurveysForUser(string username, DateTime fromDate)
        {
            return
                (from filter in this.filters
                 from survey in filter.GetSurveys(username, fromDate)
                 select survey).Distinct(new SurveysComparer());
        }

        public IEnumerable<string> GetUsersForSurvey(Survey survey)
        {
            return
                (from filter in this.filters
                 from user in filter.GetUsers(survey)
                 select user).Distinct();
        }
    }
}