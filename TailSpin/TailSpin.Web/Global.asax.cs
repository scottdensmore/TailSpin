namespace TailSpin.Web
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Web;
    using Microsoft.IdentityModel.Web.Configuration;
    using Microsoft.Practices.Unity;
    using TailSpin.Web.Controllers;
    using TailSpin.Web.Survey.Shared;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var container = new UnityContainer();

            ComponentRegistration.RegisterSurveyStore(container);
            ComponentRegistration.RegisterSurveyAnswerStore(container);
            ComponentRegistration.RegisterSurveyAnswersSummaryStore(container);
            ComponentRegistration.RegisterSurveyAnswerTransferStore(container);
            ComponentRegistration.RegisterTenantStore(container);

            ControllerBuilder.Current.SetControllerFactory(new UnityControllerFactory(container));

            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            AreaRegistration.RegisterAllAreas();
            AppRoutes.RegisterRoutes(RouteTable.Routes);

            FederatedAuthentication.ServiceConfigurationCreated += OnServiceConfigurationCreated;
        }

        private static void OnServiceConfigurationCreated(object sender, ServiceConfigurationCreatedEventArgs e)
        {
            // Use the <serviceCertificate> to protect the cookies that are
            // sent to the client.
            var sessionTransforms =
                new List<CookieTransform>(
                    new CookieTransform[]
                        {
                            new DeflateCookieTransform(),
                            new RsaEncryptionCookieTransform(e.ServiceConfiguration.ServiceCertificate),
                            new RsaSignatureCookieTransform(e.ServiceConfiguration.ServiceCertificate)
                        });
            var sessionHandler = new SessionSecurityTokenHandler(sessionTransforms.AsReadOnly());
            e.ServiceConfiguration.SecurityTokenHandlers.AddOrReplace(sessionHandler);
        }
    }
}