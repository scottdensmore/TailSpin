namespace AcsMvcApplication.Controllers
{
    using System;
    using System.Net;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;
    using Microsoft.IdentityModel.Protocols.WSFederation;
    using Microsoft.IdentityModel.Web;

    [HandleError]
    [ValidateInput(false)]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class AccountController : Controller
    {
        private const string IdentityProviderJsonEndpoint =
            "https://{0}.accesscontrol.windows.net:443/v2/metadata/IdentityProviders.js?protocol=wsfederation&realm={1}&reply_to=&context=&request_id=&version=1.0";

        public ActionResult IdentityProviders(string serviceNamespace, string appId)
        {
            string idpsJsonEndpoint = string.Format(IdentityProviderJsonEndpoint, serviceNamespace, appId);
            var client = new WebClient();
            var data = client.DownloadData(idpsJsonEndpoint);

            return Content(Encoding.UTF8.GetString(data), "application/json");
        }

        /// <summary>
        ///   This method signs out the user from the current application and
        ///   issues a Federated sign out Redirect to the STS.
        /// </summary>
        /// <remarks>
        ///   When a protocol sign out cleanup message is received from the STS,
        ///   since the WSFederationAuthenticationModule is configured in the pipeline,
        ///   it will process the sign out cleanup message and return the appropriate
        ///   response back to the STS. Therefore, this LogOff() method does not 
        ///   need to handle sign out/sign out cleanup messages that are received.
        /// </remarks>
        /// <returns>Federated sign out Redirect to the STS</returns>
        public ActionResult LogOff()
        {
            WSFederationAuthenticationModule fam = FederatedAuthentication.WSFederationAuthenticationModule;

            // SignOut from both Authentications
            try
            {
                FormsAuthentication.SignOut();
            }
            finally
            {
                fam.SignOut(true);
            }

            // Initiate a Federated sign out request to the STS.
            SignOutRequestMessage signOutRequest = new SignOutRequestMessage(new Uri(fam.Issuer), fam.Realm);
            return Redirect(signOutRequest.WriteQueryString());
        }

        /// <summary>
        ///   This method signs in the user using Federated authentication.
        /// </summary>
        /// <remarks>
        ///   If the LogOn link was explicitly clicked, the return URL will be empty.
        ///   If the LogOn was triggered due to authorization requirements on a page,
        ///   a return URL will be present. It is expected that after authentication,
        ///   the user will be redirected back to this return URL.
        /// </remarks>
        /// <param name = "returnUrl">URL the user was trying to access before being authenticated. This parameter is optional.</param>
        /// <returns>
        ///   Redirect to return URL if the request is authenticated; 
        ///   otherwise Federated sign in Redirect to the STS.
        /// </returns>
        public ActionResult LogOn(string returnUrl)
        {
            return LogOnCommon(returnUrl);
        }

        /// <summary>
        ///   This method signs in the user using Federated authentication.
        /// </summary>
        /// <remarks>
        ///   If the POST is an unauthenticated POST, the request will be
        ///   redirected to the STS for Federated authentication.
        ///   If the protocol form POST is received with the token from 
        ///   the STS, since the WSFederationAuthenticationModule is configured in the pipeline, 
        ///   it will have authenticated the posted token by the time this method is invoked.
        ///   Therefore, this method turns off input validation.
        /// </remarks>
        /// <returns>
        ///   Redirect to return URL if the request is authenticated; 
        ///   otherwise Federated sign in Redirect to the STS.
        /// </returns>
        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogOn()
        {
            return LogOnCommon(null);
        }

        //public ActionResult FetchMetadata(string url)
        //{
        //    var client = new WebClient();
        //    var data = client.DownloadData("https://login.southworks.net/FederationMetadata/2007-06/FederationMetadata.xml");

        //    var request = WebRequest.Create(url);
        //    var response = request.GetResponse() as HttpWebResponse;
        //    return Content((int)response.StatusCode, "text/plain");
        //}

        /// <summary>
        ///   This method extracts the WS-Federation passive context from the current HTTP request,
        ///   if it is a valid protocol message.
        /// </summary>
        /// <returns>Context string if it exists; otherwise String.Empty</returns>
        private string GetContextFromRequest()
        {
            Uri requestBaseUrl = WSFederationMessage.GetBaseUrl(Request.Url);
            WSFederationMessage message = WSFederationMessage.CreateFromNameValueCollection(requestBaseUrl, Request.Form);
            return message != null ? message.Context : String.Empty;
        }

        /// <summary>
        ///   This method constructs a Federated sign in request to the STS
        ///   based on the current configuration of the WSFederationAuthenticationModule.
        /// </summary>
        /// <param name = "returnUrl">URL the user was trying to access before being authenticated. This parameter is optional.</param>
        /// <returns>A WS-Federation passive protocol request URL serialized to string</returns>
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

        /// <summary>
        ///   This LogOn method accepts both GET and POST requests.
        /// 
        ///   Case 1: Unauthenticated GET & POST - result in Federated sign in Redirect to the STS.
        ///         
        ///   Case 2: Authenticated GET - results in Redirect to the return URL.
        /// 
        ///   Case 3: Authenticated POST - When the protocol form POST is received with the token from 
        ///   the STS, since the WSFederationAuthenticationModule is configured in the pipeline, 
        ///   it will have authenticated the posted token by the time this method is invoked.
        ///   Therefore, this method will treat an authenticated POST request the same as an 
        ///   authenticated GET, and issues a Redirect to the return URL.
        /// </summary>
        /// <param name = "returnUrl">URL the user was trying to access before being authenticated. This parameter is optional.</param>
        /// <returns>
        ///   Redirect to return URL if the request is authenticated; 
        ///   otherwise Federated sign in Redirect to the STS.
        /// </returns>
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
                return RedirectToAction("Index", "Home");
            }
        }
    }
}