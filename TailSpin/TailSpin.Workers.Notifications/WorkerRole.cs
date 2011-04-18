namespace TailSpin.Workers.Notifications
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using Microsoft.Practices.Unity;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Diagnostics;
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
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            DiagnosticMonitor.Start("DiagnosticsConnectionString");

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

            ComponentRegistration.RegisterNewSurveyNotificationCommand(container);

            return base.OnStart();
        }

        public override void OnStop()
        {
            container.Dispose();
            base.OnStop();
        }

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("TailSpin.Workers.Notifications entry point called", "Information");

            var newSurvenyCommand = container.Resolve<NewSurveyNotificationCommand>();
            var newSurveyQueue = container.Resolve<IAzureQueue<NewSurveyMessage>>();
            QueueHandler.For(newSurveyQueue).Every(TimeSpan.FromSeconds(5)).Do(newSurvenyCommand);

            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }
    }
}