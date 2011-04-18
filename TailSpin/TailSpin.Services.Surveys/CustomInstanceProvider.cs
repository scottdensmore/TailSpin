




 




namespace TailSpin.Services.Surveys
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using Microsoft.Practices.Unity;

    public class CustomInstanceProvider : IInstanceProvider
    {
        private readonly IUnityContainer container;
        private readonly Type serviceType;

        public CustomInstanceProvider(IUnityContainer container, Type serviceType)
        {
            this.container = container;
            this.serviceType = serviceType;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return this.GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return this.container.Resolve(this.serviceType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            this.container.Teardown(instance);
        }
    }
}