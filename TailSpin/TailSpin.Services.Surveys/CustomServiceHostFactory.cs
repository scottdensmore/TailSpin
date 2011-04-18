




 




namespace TailSpin.Services.Surveys
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.ServiceModel.Description;
    using Microsoft.Practices.Unity;

    public class CustomServiceHostFactory : WebServiceHostFactory
    {
        private readonly IUnityContainer container;

        public CustomServiceHostFactory(IUnityContainer container)
        {
            this.container = container;
        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var host = new CustomServiceHost(serviceType, baseAddresses, this.container);

            host.Authorization.ServiceAuthorizationManager = new SimulatedWebServiceAuthorizationManager();
            host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;

            return host;
        }
    }
}