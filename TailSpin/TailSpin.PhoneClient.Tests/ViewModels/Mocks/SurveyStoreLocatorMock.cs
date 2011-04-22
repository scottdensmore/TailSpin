namespace TailSpin.PhoneClient.Tests.ViewModels.Mocks
{
    using TailSpin.PhoneClient.Services.Stores;

    public class SurveyStoreLocatorMock : ISurveyStoreLocator
    {
        private readonly ISurveyStore surveyStoreToReturn;

        public SurveyStoreLocatorMock(ISurveyStore surveyStoreToReturn)
        {
            this.surveyStoreToReturn = surveyStoreToReturn;
        }

        public ISurveyStore GetStore()
        {
            return this.surveyStoreToReturn;
        }
    }
}