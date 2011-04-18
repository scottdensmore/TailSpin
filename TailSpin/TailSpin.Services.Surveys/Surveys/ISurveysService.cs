




 




namespace TailSpin.Services.Surveys.Surveys
{
    using System.IO;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public interface ISurveysService
    {
        [OperationContract]
        [WebGet(UriTemplate = "Surveys?lastSyncUtcDate={lastSyncUtcDate}", ResponseFormat = WebMessageFormat.Json)]
        SurveyDto[] GetSurveys(string lastSyncUtcDate);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SurveyAnswers", RequestFormat = WebMessageFormat.Json)]
        void AddSurveyAnswers(SurveyAnswerDto[] surveyAnswers);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "MediaAnswer?type={type}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string AddMediaAnswer(Stream media, string type);
    }
}