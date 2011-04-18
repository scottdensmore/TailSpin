




 




namespace TailSpin.Services.Surveys.Registration
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public interface IRegistrationService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SetFilters", RequestFormat = WebMessageFormat.Json)]
        void SetFilters(SurveyFiltersDto surveyFiltersDto);

        [OperationContract(Name = "GetFilters")]
        [WebInvoke(Method = "GET", UriTemplate = "GetFilters", ResponseFormat = WebMessageFormat.Json)]
        SurveyFiltersInformationDto GetFilters();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Notifications", RequestFormat = WebMessageFormat.Json)]
        void Notifications(DeviceDto device);
    }
}