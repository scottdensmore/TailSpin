




 




namespace TailSpin.Web.Survey.Shared.Stores
{
    using AzureStorage;
    using TailSpin.Web.Survey.Shared.Models;

    public interface ISurveyAnswerContainerFactory
    {
        IAzureBlobContainer<SurveyAnswer> Create(string tenant, string surveySlug);
    }
}