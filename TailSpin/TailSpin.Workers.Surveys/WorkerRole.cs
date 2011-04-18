namespace TailSpin.Workers.Surveys
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using Microsoft.Practices.Unity;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using TailSpin.Web.Survey.Shared;
    using TailSpin.Web.Survey.Shared.Handlers;
    using TailSpin.Web.Survey.Shared.QueueMessages;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    public class WorkerRole : RoleEntryPoint
    {
        private IUnityContainer container;

        public override bool OnStart()
        {
            // The number of connections depends on the particular usage in each application
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += (sender, e) =>
                                            {
                                                if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
                                                {
                                                    // Set e.Cancel to true to restart this role instance
                                                    e.Cancel = true;
                                                }
                                            };

            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
                                                                 configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)));

            container = new UnityContainer();

            ComponentRegistration.RegisterSurveyStore(container);
            ComponentRegistration.RegisterSurveyAnswerStore(container);
            ComponentRegistration.RegisterSurveyAnswersSummaryStore(container);
            ComponentRegistration.RegisterSurveyAnswerTransferStore(container);
            ComponentRegistration.RegisterTenantStore(container);
            ComponentRegistration.RegisterUpdatingSurveyResultsSummaryCommand(container);
            ComponentRegistration.RegisterTransferSurveysToSqlAzureCommand(container);

            return base.OnStart();
        }

        public override void OnStop()
        {
            container.Dispose();
            base.OnStop();
        }

        public override void Run()
        {
            //// The time interval for checking the queues have to be tuned depending on the scenario and the expected workload
            var updatingSurveyResultsSummaryCommand = container.Resolve<UpdatingSurveyResultsSummaryCommand>();
            var surveyAnswerStoredQueue = container.Resolve<IAzureQueue<SurveyAnswerStoredMessage>>();
            BatchProcessingQueueHandler.For(surveyAnswerStoredQueue).Every(TimeSpan.FromSeconds(10))
                .Do(updatingSurveyResultsSummaryCommand);

            var transferCommand = container.Resolve<TransferSurveysToSqlAzureCommand>();
            var transferQueue = container.Resolve<IAzureQueue<SurveyTransferMessage>>();
            QueueHandler.For(transferQueue).Every(TimeSpan.FromSeconds(5)).Do(transferCommand);

            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
    }
}