




 




namespace TailSpin.Web.Survey.Shared
{
    public static class AzureConstants
    {
        public static class BlobContainers
        {
            public const string SurveyAnswers = "tempsurveyanswerswp7";
            public const string SurveyAnswersSummaries = "surveyanswerssummarieswp7";
            public const string SurveyAnswersLists = "surveyanswerslistswp7";
            public const string Tenants = "tenantswp7";
            public const string Logos = "logoswp7";
            public const string VoiceAnswers = "voiceanswerswp7";
            public const string PictureAnswers = "pictureanswerswp7";
        }

        public static class Queues
        {
            public const string SurveyAnswerStored = "surveyanswerstoredwp7";
            public const string SurveyTransferRequest = "surveytransferwp7";
            public const string NewSurveyStored = "newsurveystoredwp7";
        }

        public static class Tables
        {
            public const string Surveys = "SurveysWp7";
            public const string Questions = "QuestionsWp7";
            public const string UserDevice = "UserDeviceWp7";
            public const string TenantFilter = "TenantFilterWp7";
        }
    }
}
