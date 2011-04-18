




 




namespace TailSpin.Web.Areas.Survey
{
    using System.Web.Mvc;

    public class SurveyAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Survey";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MySurveys",
                "survey/{tenant}",
                new { controller = "Surveys", action = "Index" });

            context.MapRoute(
                "NewSurvey",
                "survey/{tenant}/newsurvey",
                new { controller = "Surveys", action = "New" });

            context.MapRoute(
                "NewQuestion",
                "survey/{tenant}/newquestion",
                new { controller = "Surveys", action = "NewQuestion" });

            context.MapRoute(
                "AddQuestion",
                "survey/{tenant}/newquestion/add",
                new { controller = "Surveys", action = "AddQuestion" });

            context.MapRoute(
                "CancelNewQuestion",
                "survey/{tenant}/newquestion/cancel",
                new { controller = "Surveys", action = "CancelNewQuestion" });

            context.MapRoute(
                "AnalyzeSurvey",
                "survey/{tenant}/{surveySlug}/analyze",
                new { controller = "Surveys", action = "Analyze" });

            context.MapRoute(
                "BrowseResponses",
                "survey/{tenant}/{surveySlug}/analyze/browse/{answerId}",
                new { controller = "Surveys", action = "BrowseResponses", answerId = string.Empty });

            context.MapRoute(
                "ExportResponses",
                "survey/{tenant}/{surveySlug}/analyze/export",
                new { controller = "Surveys", action = "ExportResponses" });

            context.MapRoute(
                "DeleteSurvey",
                "survey/{tenant}/{surveySlug}/delete",
                new { controller = "Surveys", action = "Delete" });
        }
    }
}
