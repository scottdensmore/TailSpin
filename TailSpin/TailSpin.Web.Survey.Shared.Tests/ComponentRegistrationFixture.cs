namespace TailSpin.Web.Survey.Shared.Tests
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TailSpin.Web.Survey.Shared.Notifications;
    using TailSpin.Web.Survey.Shared.Stores;
    using TailSpin.Web.Survey.Shared.SurveysFiltering;
    using TailSpin.Workers.Notifications;
    using TailSpin.Workers.Surveys;

    [TestClass]
    public class ComponentRegistrationFixture
    {
        [TestMethod]
        public void ResolveISurveyStore()
        {
            var container = new UnityContainer();

            ComponentRegistration.RegisterSurveyStore(container);
            var actualObject = container.Resolve<ISurveyStore>();

            Assert.IsInstanceOfType(actualObject, typeof(SurveyStore));
        }

        [TestMethod]
        public void ResolveISurveyAnswerStore()
        {
            var container = new UnityContainer();

            ComponentRegistration.RegisterSurveyAnswerStore(container);
            var actualObject = container.Resolve<ISurveyAnswerStore>();

            Assert.IsInstanceOfType(actualObject, typeof(SurveyAnswerStore));
        }

        [TestMethod]
        public void ResolveIMediaAnswerStore()
        {
            var container = new UnityContainer();

            ComponentRegistration.RegisterMediaAnswerStore(container);
            var actualObject = container.Resolve<IMediaAnswerStore>();

            Assert.IsInstanceOfType(actualObject, typeof(MediaAnswerStore));
        }

        [TestMethod]
        public void ResolveISurveyAnswersSummaryStore()
        {
            var container = new UnityContainer();

            ComponentRegistration.RegisterSurveyAnswersSummaryStore(container);
            var actualObject = container.Resolve<ISurveyAnswersSummaryStore>();

            Assert.IsInstanceOfType(actualObject, typeof(SurveyAnswersSummaryStore));
        }

        [TestMethod]
        public void ResolveITenantStore()
        {
            var container = new UnityContainer();

            ComponentRegistration.RegisterTenantStore(container);
            var actualObject = container.Resolve<ITenantStore>();

            Assert.IsInstanceOfType(actualObject, typeof(TenantStore));
        }

        [TestMethod]
        public void ResolveISurveyTransferStore()
        {
            var container = new UnityContainer();

            ComponentRegistration.RegisterSurveyAnswerTransferStore(container);
            var actualObject = container.Resolve<ISurveyTransferStore>();

            Assert.IsInstanceOfType(actualObject, typeof(SurveyTransferStore));
        }

        [TestMethod]
        public void ResolveIPushNotification()
        {
            var container = new UnityContainer();

            ComponentRegistration.RegisterPushNotification(container);
            var actualObject = container.Resolve<IPushNotification>();

            Assert.IsInstanceOfType(actualObject, typeof(PushNotification));
        }

        [TestMethod]
        public void ResolveUpdatingSurveyResultsSummaryCommand()
        {
            var container = new UnityContainer();

            ComponentRegistration.RegisterUpdatingSurveyResultsSummaryCommand(container);
            var actualObject = container.Resolve<UpdatingSurveyResultsSummaryCommand>();

            Assert.IsInstanceOfType(actualObject, typeof(UpdatingSurveyResultsSummaryCommand));
        }

        [TestMethod]
        public void ResolveNewSurveyNotificationCommand()
        {
            var container = new UnityContainer();

            ComponentRegistration.RegisterNewSurveyNotificationCommand(container);
            var actualObject = container.Resolve<NewSurveyNotificationCommand>();

            Assert.IsInstanceOfType(actualObject, typeof(NewSurveyNotificationCommand));
        }

        [TestMethod]
        public void ResolveTransferSurveysToSqlAzureCommand()
        {
            var container = new UnityContainer();

            ComponentRegistration.RegisterTransferSurveysToSqlAzureCommand(container);
            var actualObject = container.Resolve<TransferSurveysToSqlAzureCommand>();

            Assert.IsInstanceOfType(actualObject, typeof(TransferSurveysToSqlAzureCommand));
        }

        [TestMethod]
        public void ResolveISurveySqlStore()
        {
            var container = new UnityContainer();

            ComponentRegistration.RegisterSurveySqlStore(container);
            var actualObject = container.Resolve<ISurveySqlStore>();

            Assert.IsInstanceOfType(actualObject, typeof(SurveySqlStore));
        }

        [TestMethod]
        public void ResolveIFilteringService()
        {
            var container = new UnityContainer();

            ComponentRegistration.RegisterFilteringService(container);
            var actualObject = container.Resolve<IFilteringService>();

            Assert.IsInstanceOfType(actualObject, typeof(FilteringService));
        }

        [TestMethod]
        public void ResolveITenantFilterStore()
        {
            var container = new UnityContainer();

            ComponentRegistration.RegisterTenantFilterStore(container);
            var actualObject = container.Resolve<ITenantFilterStore>();

            Assert.IsInstanceOfType(actualObject, typeof(TenantFilterStore));
        }

        [TestMethod]
        public void ResolveIUserDeviceStore()
        {
            var container = new UnityContainer();

            ComponentRegistration.RegisterUserDeviceStore(container);
            var actualObject = container.Resolve<IUserDeviceStore>();

            Assert.IsInstanceOfType(actualObject, typeof(UserDeviceStore));
        }
    }
}