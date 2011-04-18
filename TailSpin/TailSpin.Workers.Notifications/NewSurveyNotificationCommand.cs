




 




namespace TailSpin.Workers.Notifications
{
    using System;
    using System.Linq;
    using TailSpin.Web.Survey.Shared.Notifications;
    using TailSpin.Web.Survey.Shared.SurveysFiltering;
    using Web.Survey.Shared.Commands;
    using Web.Survey.Shared.QueueMessages;
    using Web.Survey.Shared.Stores;

    public class NewSurveyNotificationCommand : ICommand<NewSurveyMessage>
    {
        private readonly ISurveyStore surveyStore;
        private readonly IFilteringService filteringService;
        private readonly IUserDeviceStore userDeviceStore;
        private readonly IPushNotification pushNotification;

        public NewSurveyNotificationCommand(
                ISurveyStore surveyStore, 
                IFilteringService filteringService, 
                IUserDeviceStore userDeviceStore, 
                IPushNotification pushNotification)
        {
            this.surveyStore = surveyStore;
            this.filteringService = filteringService;
            this.userDeviceStore = userDeviceStore;
            this.pushNotification = pushNotification;
        }

        public void Run(NewSurveyMessage message)
        {
            var survey = this.surveyStore.GetSurveyByTenantAndSlugName(message.Tenant, message.SlugName, false);

            if (survey != null)
            {
                var deviceUris =
                    from user in this.filteringService.GetUsersForSurvey(survey)
                    from deviceUri in this.userDeviceStore.GetDevices(user)
                    select deviceUri;

                foreach (var deviceUri in deviceUris)
                {
                    this.pushNotification.PushTileNotification(
                                            deviceUri.ToString(), 
                                            "New Surveys", 
                                            null, 
                                            0,
                                            uri => this.userDeviceStore.RemoveUserDevice(new Uri(uri)));
                }
            }
        }
    }
}