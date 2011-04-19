namespace TailSpin.Web.Survey.Shared
{
    using System.Collections.Generic;
    using Microsoft.Practices.Unity;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Notifications;
    using TailSpin.Web.Survey.Shared.QueueMessages;
    using TailSpin.Web.Survey.Shared.Stores;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;
    using TailSpin.Web.Survey.Shared.SurveysFiltering;

    public static class ComponentRegistration
    {
        public static void RegisterFilteringService(IUnityContainer container)
        {
            container.RegisterType<IFilteringService, FilteringService>(
                new InjectionConstructor(
                    new ResolvedArrayParameter<ISurveyFilter>(new ResolvedParameter<ISurveyFilter>("tenantFilter"))));

            container.RegisterType<ISurveyFilter, TenantFilter>("tenantFilter");

            RegisterSurveyStore(container);
            RegisterTenantFilterStore(container);
            RegisterTenantStore(container);
        }

        public static void RegisterMediaAnswerStore(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<IMediaAnswerStore, MediaAnswerStore>(
                new InjectionConstructor(
                    new ResolvedParameter<IAzureBlobContainer<byte[]>>(AzureConstants.BlobContainers.VoiceAnswers),
                    new ResolvedParameter<IAzureBlobContainer<byte[]>>(AzureConstants.BlobContainers.PictureAnswers)));

            container.RegisterType<IAzureBlobContainer<byte[]>, FilesBlobContainer>(
                AzureConstants.BlobContainers.PictureAnswers,
                new InjectionConstructor(
                    typeof(CloudStorageAccount),
                    AzureConstants.BlobContainers.PictureAnswers,
                    "image/jpeg",
                    new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }));

            container.RegisterType<IAzureBlobContainer<byte[]>, FilesBlobContainer>(
                AzureConstants.BlobContainers.VoiceAnswers,
                new InjectionConstructor(
                    typeof(CloudStorageAccount),
                    AzureConstants.BlobContainers.VoiceAnswers,
                    "audio/x-wav",
                    new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }));

            container.Resolve<IMediaAnswerStore>().Initialize();
        }

        public static void RegisterNewSurveyNotificationCommand(IUnityContainer container)
        {
            RegisterSurveyStore(container);
            RegisterFilteringService(container);
            RegisterUserDeviceStore(container);
            RegisterPushNotification(container);
        }

        public static void RegisterPushNotification(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<IPushNotification, PushNotification>();
        }

        public static void RegisterSurveyAnswerStore(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<ISurveyAnswerStore, SurveyAnswerStore>();

            // Container for resolving the survey answer containers
            var surveyAnswerBlobContainerResolver = new UnityContainer();

            surveyAnswerBlobContainerResolver.RegisterInstance(account);

            surveyAnswerBlobContainerResolver.RegisterType<IAzureBlobContainer<SurveyAnswer>, EntitiesBlobContainer<SurveyAnswer>>(
                new InjectionConstructor(typeof(CloudStorageAccount), typeof(string)));

            container.RegisterType<ISurveyAnswerContainerFactory, SurveyAnswerContainerFactory>(
                new InjectionConstructor(surveyAnswerBlobContainerResolver));

            container.RegisterType<IAzureQueue<SurveyAnswerStoredMessage>, AzureQueue<SurveyAnswerStoredMessage>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.Queues.SurveyAnswerStored));

            container.RegisterType<IAzureBlobContainer<List<string>>, EntitiesBlobContainer<List<string>>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.BlobContainers.SurveyAnswersLists));

            RegisterMediaAnswerStore(container);

            container.Resolve<ISurveyAnswerStore>().Initialize();
        }

        public static void RegisterSurveyAnswerTransferStore(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<ISurveyTransferStore, SurveyTransferStore>();

            container.RegisterType<IAzureQueue<SurveyTransferMessage>, AzureQueue<SurveyTransferMessage>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.Queues.SurveyTransferRequest));

            container.Resolve<ISurveyTransferStore>().Initialize();
        }

        public static void RegisterSurveyAnswersSummaryStore(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<ISurveyAnswersSummaryStore, SurveyAnswersSummaryStore>();

            container.RegisterType<IAzureBlobContainer<SurveyAnswersSummary>, EntitiesBlobContainer<SurveyAnswersSummary>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.BlobContainers.SurveyAnswersSummaries));

            container.Resolve<ISurveyAnswersSummaryStore>().Initialize();
        }

        public static void RegisterSurveySqlStore(IUnityContainer container)
        {
            container.RegisterType<ISurveySqlStore, SurveySqlStore>();
        }

        public static void RegisterSurveyStore(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<ISurveyStore, SurveyStore>();

            container.RegisterType<IAzureTable<SurveyRow>, AzureTable<SurveyRow>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.Tables.Surveys));

            container.RegisterType<IAzureTable<QuestionRow>, AzureTable<QuestionRow>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.Tables.Questions));

            container.RegisterType<IAzureQueue<NewSurveyMessage>, AzureQueue<NewSurveyMessage>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.Queues.NewSurveyStored));

            container.Resolve<ISurveyStore>().Initialize();
        }

        public static void RegisterTenantFilterStore(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<ITenantFilterStore, TenantFilterStore>();

            container.RegisterType<IAzureTable<TenantFilterRow>, AzureTable<TenantFilterRow>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.Tables.TenantFilter));

            container.Resolve<ITenantFilterStore>().Initialize();
        }

        public static void RegisterTenantStore(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<ITenantStore, TenantStore>(
                new InjectionConstructor(
                    new ResolvedParameter<IAzureBlobContainer<Tenant>>(),
                    new ResolvedParameter<IAzureBlobContainer<byte[]>>(AzureConstants.BlobContainers.Logos)));

            container.RegisterType<IAzureBlobContainer<Tenant>, EntitiesBlobContainer<Tenant>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.BlobContainers.Tenants));

            container.RegisterType<IAzureBlobContainer<byte[]>, FilesBlobContainer>(
                AzureConstants.BlobContainers.Logos,
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.BlobContainers.Logos, "image/jpeg"));

            container.Resolve<ITenantStore>().Initialize();
        }

        public static void RegisterTransferSurveysToSqlAzureCommand(IUnityContainer container)
        {
            RegisterSurveyAnswerStore(container);
            RegisterSurveyStore(container);
            RegisterTenantStore(container);
            RegisterSurveySqlStore(container);
        }

        public static void RegisterUpdatingSurveyResultsSummaryCommand(IUnityContainer container)
        {
            container.RegisterType<IDictionary<string, SurveyAnswersSummary>, Dictionary<string, SurveyAnswersSummary>>(
                new InjectionConstructor());

            RegisterSurveyAnswerStore(container);
            RegisterSurveyAnswersSummaryStore(container);
        }

        public static void RegisterUserDeviceStore(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<IUserDeviceStore, UserDeviceStore>();

            container.RegisterType<IAzureTable<UserDeviceRow>, AzureTable<UserDeviceRow>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.Tables.UserDevice));

            container.Resolve<IUserDeviceStore>().Initialize();
        }
    }
}