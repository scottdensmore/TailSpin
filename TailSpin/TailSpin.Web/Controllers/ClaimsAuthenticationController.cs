




 




namespace TailSpin.Web.Controllers
{
    using System;
    using System.Text;
    using System.Web.Mvc;
    using Microsoft.IdentityModel.Protocols.WSFederation;
    using Microsoft.IdentityModel.Web;

    [RequireHttps]
    public class ClaimsAuthenticationController : Controller
    {
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult FederationResult()
        {
            var fam = FederatedAuthentication.WSFederationAuthenticationModule;
            if (fam.CanReadSignInResponse(System.Web.HttpContext.Current.Request, true))
            {
                string returnUrl = GetReturnUrlFromCtx();

                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "OnBoarding");
        }

        [HttpGet]
        public ActionResult Signout()
        {
            if (User.Identity.IsAuthenticated)
            {
                FederatedAuthentication.WSFederationAuthenticationModule.SignOut(false);

                string issuer = FederatedAuthentication.WSFederationAuthenticationModule.Issuer;
                var signOut = new SignOutRequestMessage(new Uri(issuer));

                // In the Windows Azure environment, build a wreply parameter for  the SignIn request
                // that reflects the real address of the application.
                var request = this.Request;
                Uri requestUrl = request.Url;

                StringBuilder wreply = new StringBuilder();
                wreply.Append(requestUrl.Scheme);     // e.g. "http" or "https"
                wreply.Append("://");
                wreply.Append(request.Headers["Host"] ?? requestUrl.Authority);
                wreply.Append(request.ApplicationPath);

                if (!request.ApplicationPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    wreply.Append("/");
                }

                signOut.Reply = wreply.ToString();

                return this.Redirect(signOut.WriteQueryString());
            }

            return this.RedirectToAction("Index", "OnBoarding");
        }

        private static string GetReturnUrlFromCtx()
        {
            // this is the same as doing HttpContext.Request.Form["wctx"];
            var response = FederatedAuthentication.WSFederationAuthenticationModule.GetSignInResponseMessage(System.Web.HttpContext.Current.Request);
            return response.Context;
        }
    }
}
