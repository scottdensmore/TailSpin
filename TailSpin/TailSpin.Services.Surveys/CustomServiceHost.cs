




 




namespace TailSpin.Services.Surveys
{
    using System;
    using System.ServiceModel.Web;
    using Microsoft.Practices.Unity;

    public class CustomServiceHost : WebServiceHost
    {
        private readonly IUnityContainer container;

        public CustomServiceHost(Type serviceType, Uri[] baseAddresses, IUnityContainer container) 
            : base(serviceType, baseAddresses)
        {
            this.container = container;
        }

        protected override void OnOpening()
        {
            if (Description.Behaviors.Find<CustomServiceBehavior>() == null)
            {
                Description.Behaviors.Add(new CustomServiceBehavior(this.container));
            }

            base.OnOpening();
        }
    }
}