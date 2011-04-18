




 




namespace TailSpin.SimulatedIssuer
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("*.svc");
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("*.htm");

            routes.MapRoute(
                "SignInResponse",
                "SignInResponse",
                new { controller = "Issuer", action = "SignInResponse" });
            
            routes.MapRoute(
                "Default",
                string.Empty,
                new { controller = "Issuer", action = "Index" });
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}