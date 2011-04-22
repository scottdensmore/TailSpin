namespace TailSpin.PhoneClient.Services.Stores
{
    using System;

    public class SurveyStoreLocator : ISurveyStoreLocator
    {
        private readonly ISettingsStore settingsStore;
        private readonly Func<string, ISurveyStore> surveyStoreFactory;
        private string username;
        private ISurveyStore surveyStore;

        public SurveyStoreLocator(ISettingsStore settingsStore, Func<string, ISurveyStore> surveyStoreFactory)
        {
            this.settingsStore = settingsStore;
            this.surveyStoreFactory = surveyStoreFactory;
        }

        public ISurveyStore GetStore()
        {
            if (string.IsNullOrEmpty(this.settingsStore.UserName))
            {
                return new NullSurveyStore();
            }

            if (this.settingsStore.UserName != this.username)
            {
                this.username = this.settingsStore.UserName;
                var storeName = string.Format("{0}.store", this.username);
                this.surveyStore = this.surveyStoreFactory.Invoke(storeName);
            }

            return this.surveyStore;
        }
    }
}