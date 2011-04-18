




 




namespace TailSpin.Services.Surveys
{
    using System;
    using System.ServiceModel.Activation;
    using System.Web;
    using System.Web.Routing;
    using Registration;
    using TailSpin.Services.Surveys.Surveys;

    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes();
        }

        private static void RegisterRoutes()
        {
            var customServiceHostFactory = new CustomServiceHostFactory(ContainerLocator.Container);
            RouteTable.Routes.Add(new ServiceRoute("Registration", customServiceHostFactory, typeof(RegistrationService)));
            RouteTable.Routes.Add(new ServiceRoute("Survey", customServiceHostFactory, typeof(SurveysService)));
        }
    }
}