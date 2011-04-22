




 




namespace Adatum.SimulatedIssuer
{
    using System;
    using System.Security.Principal;
    using System.Web;

    public static class SimulatedWindowsAuthenticationOperations
    {
        public static void LogOnUser(string authenticatedUser, HttpContext context, HttpRequest request, HttpResponse response)
        {
            var genericIdentity = new GenericIdentity(authenticatedUser);
            context.User = new GenericPrincipal(genericIdentity, null);
            
            // The following line is commented out so that the issuer does not remember that the user has signed in.
            // This is useful because signing out from the TailSpin issuer will simulate a federated sign out.
            // CreateSimulatedWindowsAuthenticationCookie(authenticatedUser, request, response);
        }

        public static bool TryToAuthenticateUser(HttpContext context, HttpRequest request, HttpResponse response)
        {
            var simulatedWindowsAuthenticationCookie = request.Cookies[".WINAUTH"];

            if (simulatedWindowsAuthenticationCookie == null)
            {
                return false;
            }

            var authenticatedUser = simulatedWindowsAuthenticationCookie.Value;
            LogOnUser(authenticatedUser, context, request, response);

            return true;
        }

        public static void LogOutUser(HttpRequest request, HttpResponse response)
        {
            var simulatedWindowsAuthenticationCookie = new HttpCookie(".WINAUTH");
            simulatedWindowsAuthenticationCookie.Path = request.ApplicationPath;
            simulatedWindowsAuthenticationCookie.Expires = DateTime.Now.AddDays(-1);
            response.Cookies.Add(simulatedWindowsAuthenticationCookie);
        }

        private static void CreateSimulatedWindowsAuthenticationCookie(string authenticatedUser, HttpRequest request, HttpResponse response)
        {
            var simulatedWindowsAuthenticationCookie = new HttpCookie(".WINAUTH", authenticatedUser);
            simulatedWindowsAuthenticationCookie.Path = request.ApplicationPath;
            response.Cookies.Add(simulatedWindowsAuthenticationCookie);
        }
    }
}
