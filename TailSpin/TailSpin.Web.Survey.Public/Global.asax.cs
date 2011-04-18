namespace TailSpin.Web.Survey.Public
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.Practices.Unity;
    using TailSpin.Web.Survey.Public.Controllers;
    using TailSpin.Web.Survey.Public.Extensions;
    using TailSpin.Web.Survey.Shared;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var container = new UnityContainer();
            ComponentRegistration.RegisterSurveyStore(container);
            ComponentRegistration.RegisterSurveyAnswerStore(container);
            ControllerBuilder.Current.SetControllerFactory(new UnityControllerFactory(container));

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new MobileCapableWebFormViewEngine());

            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            AppRoutes.RegisterRoutes(RouteTable.Routes);
        }
    }
}