




 




namespace TailSpin.Services.Surveys
{
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using Microsoft.Practices.Unity;

    public class CustomServiceBehavior : IServiceBehavior
    {
        private readonly IUnityContainer container;

        public CustomServiceBehavior(IUnityContainer container)
        {
            this.container = container;
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var dispatcher in serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = (ChannelDispatcher)dispatcher;

                foreach (var endpointDispatcher in channelDispatcher.Endpoints)
                {
                    endpointDispatcher.DispatchRuntime.InstanceProvider = new CustomInstanceProvider(this.container, serviceDescription.ServiceType);
                    endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new NoCacheMessageInspector());
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }
    }
}