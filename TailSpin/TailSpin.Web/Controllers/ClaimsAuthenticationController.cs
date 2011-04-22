namespace TailSpin.Web.Controllers
{
    using System;
    using System.Net;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.IdentityModel.Protocols.WSFederation;
    using Microsoft.IdentityModel.Web;

    [RequireHttps]
    public class ClaimsAuthenticationController : Controller
    {
        private const string IdentityProviderJsonEndpoint =
            "https://{0}.accesscontrol.windows.net:443/v2/metadata/IdentityProviders.js?protocol=wsfederation&realm={1}&reply_to=&context=&request_id=&version=1.0";

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult FederationResult()
        {
            var fam = FederatedAuthentication.WSFederationAuthenticationModule;
            if (fam.CanReadSignInResponse(System.Web.HttpContext.Current.Request, true))
            {
                string returnUrl = GetReturnUrlFromCtx();

                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "OnBoarding");
        }

        public ActionResult IdentityProviders(string serviceNamespace, string appId)
        {
            string idpsJsonEndpoint = string.Format(IdentityProviderJsonEndpoint, serviceNamespace, appId);
            
            var client = new WebClient();
            var data = client.DownloadData(idpsJsonEndpoint);
            return Content(Encoding.UTF8.GetString(data), "application/json");
        }

        public ActionResult LogOn(string returnUrl)
        {
            return LogOnCommon(returnUrl);
        }

        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogOn()
        {
            return LogOnCommon(null);
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
                var request = Request;
                Uri requestUrl = request.Url;

                StringBuilder wreply = new StringBuilder();
                wreply.Append(requestUrl.Scheme); // e.g. "http" or "https"
                wreply.Append("://");
                wreply.Append(request.Headers["Host"] ?? requestUrl.Authority);
                wreply.Append(request.ApplicationPath);

                if (!request.ApplicationPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    wreply.Append("/");
                }

                signOut.Reply = wreply.ToString();

                return Redirect(signOut.WriteQueryString());
            }

            return RedirectToAction("Index", "OnBoarding");
        }

        private static string GetReturnUrlFromCtx()
        {
            // this is the same as doing HttpContext.Request.Form["wctx"];
            var response = FederatedAuthentication.WSFederationAuthenticationModule.GetSignInResponseMessage(System.Web.HttpContext.Current.Request);
            return response.Context;
        }

        private string GetContextFromRequest()
        {
            Uri requestBaseUrl = WSFederationMessage.GetBaseUrl(Request.Url);
            WSFederationMessage message = WSFederationMessage.CreateFromNameValueCollection(requestBaseUrl, Request.Form);
            return message != null ? message.Context : String.Empty;
        }

        private string GetFederatedSignInRedirectUrl(string returnUrl)
        {
            // Create a sign in request based on the configured parameters.
            WSFederationAuthenticationModule fam = FederatedAuthentication.WSFederationAuthenticationModule;

            var signInRequest = new SignInRequestMessage(new Uri(fam.Issuer), fam.Realm)
                                    {
                                        AuthenticationType = fam.AuthenticationType,
                                        Context = GetReturnUrl(new RequestContext(HttpContext, RouteData)),
                                        Freshness = fam.Freshness,
                                        Reply = GetWReply(),
                                        HomeRealm = fam.HomeRealm
                                    };

            return signInRequest.WriteQueryString();
        }

        private string GetReturnUrl(RequestContext context)
        {
            var request = context.HttpContext.Request;
            var reqUrl = request.Url;
            var returnUrl = new StringBuilder();

            returnUrl.Append(reqUrl.Scheme);
            returnUrl.Append("://");
            returnUrl.Append(request.Headers["Host"] ?? reqUrl.Authority);
            returnUrl.Append(request.RawUrl);

            if (!request.ApplicationPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                returnUrl.Append("/");
            }

            return returnUrl.ToString();
        }

        private string GetWReply()
        {
            Uri reqUrl = Request.Url;
            var wreply = new StringBuilder();
            wreply.Append(reqUrl.Scheme); // e.g. "https"
            wreply.Append("://");
            wreply.Append(Request.Headers["Host"] ?? reqUrl.Authority);
            wreply.Append(Request.ApplicationPath);
            if (!Request.ApplicationPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                wreply.Append("/");
            }

            wreply.Append("Account/LogOn");

            return wreply.ToString();
        }

        private ActionResult LogOnCommon(string returnUrl)
        {
            // If the request is unauthenticated, Redirect to the STS with a protocol request.
            if (!Request.IsAuthenticated)
            {
                string federatedSignInRedirectUrl = GetFederatedSignInRedirectUrl(returnUrl);
                return Redirect(federatedSignInRedirectUrl);
            }

            // Request is already authenticated.
            // Redirect to the URL the user was trying to access before being authenticated.
            string effectiveReturnUrl = returnUrl;

            // If no return URL was specified, try to get it from the Request context.
            if (String.IsNullOrEmpty(effectiveReturnUrl))
            {
                effectiveReturnUrl = GetContextFromRequest();
            }

            // If there is a return URL, Redirect to it. Otherwise, Redirect to Home.
            if (!String.IsNullOrEmpty(effectiveReturnUrl))
            {
                return Redirect(effectiveReturnUrl);
            }
            else
            {
                return RedirectToAction("Index", "OnBoarding");
            }
        }
    }
}