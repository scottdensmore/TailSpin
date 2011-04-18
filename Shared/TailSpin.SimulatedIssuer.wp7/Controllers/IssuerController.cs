




 




namespace TailSpin.SimulatedIssuer.Controllers
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Util;
    using Microsoft.IdentityModel.Protocols.WSFederation;
    using Microsoft.IdentityModel.SecurityTokenService;
    using Microsoft.IdentityModel.Web;
    using TailSpin.SimulatedIssuer;

    public class IssuerController : Controller
    {
        public ActionResult Index()
        {
            string action = this.Request.QueryString[WSFederationConstants.Parameters.Action] ?? this.Request.Form[WSFederationConstants.Parameters.Action];

            try
            {
                if (action == WSFederationConstants.Actions.SignIn)
                {
                    return this.HandleSignInRequest();
                }
                
                if (action == WSFederationConstants.Actions.SignOut)
                {
                    // Process signout request.
                    SignOutRequestMessage requestMessage = (SignOutRequestMessage)WSFederationMessage.CreateFromUri(Request.Url);
                    FederatedPassiveSecurityTokenServiceOperations.ProcessSignOutRequest(requestMessage, User, null, this.HttpContext.ApplicationInstance.Response);
                    this.ViewData["ActionExplanation"] = "Sign out from the issuer has been requested.";
                    this.ViewData["ReturnUrl"] = Url.Encode(this.Request.QueryString["wreply"]);
                    return this.View();
                }
                
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "The action '{0}' (Request.QueryString['{1}']) is unexpected. Expected actions are: '{2}' or '{3}'.",
                        String.IsNullOrEmpty(action) ? "<EMPTY>" : action,
                        WSFederationConstants.Parameters.Action,
                        WSFederationConstants.Actions.SignIn,
                        WSFederationConstants.Actions.SignOut));
            }
            catch (Exception exception)
            {
                throw new Exception("An unexpected error occurred when processing the request. See inner exception for details.", exception);
            }
        }

        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SignInResponse()
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var responseMessage =
                    WSFederationMessage.CreateFromFormPost(this.HttpContext.ApplicationInstance.Request);
                return this.HandleSignInResponse(responseMessage.Context);
            }

            throw new UnauthorizedAccessException();
        }

        // This method decides which role this issuer will be acting as when a sign-in request is received
        private ActionResult HandleSignInRequest()
        {
            var homeRealm = this.Request["whr"];

            // The issuer will act as an Identity Provider when the home realm is empty
            if (string.IsNullOrEmpty(homeRealm))
            {
                throw new ArgumentException("This issuer only acts as a Federation Provider. The whr parameter should be set to the identifier of the issuer you want to use.");
            }

            // The issuer will act as a Federation Provider when the home realm has a value
            return this.HandleFederatedSignInRequest();
        }

        private ActionResult HandleFederatedSignInRequest()
        {
            var homeRealm = this.Request["whr"];
            string issuer;
            string realm;

            if (homeRealm == ConfigurationManager.AppSettings["AdatumIssuerIdentifier"])
            {
                issuer = ConfigurationManager.AppSettings["AdatumIssuerLocation"];
                realm = FederatedAuthentication.WSFederationAuthenticationModule.Realm;
            }
            else if (homeRealm == ConfigurationManager.AppSettings["FabrikamIssuerIdentifier"])
            {
                issuer = ConfigurationManager.AppSettings["FabrikamIssuerLocation"];
                realm = FederatedAuthentication.WSFederationAuthenticationModule.Realm;
            }
            else
            {
                throw new InvalidOperationException("The home realm is not trusted for federation.");
            }

            var contextId = Guid.NewGuid().ToString();
            this.CreateContextCookie(contextId, this.Request.Url.ToString());

            var message = new SignInRequestMessage(new Uri(issuer), realm)
            {
                CurrentTime = DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture) + "Z",
                Reply = realm,
                Context = contextId
            };

            return this.Redirect(message.RequestUrl);
        }

        private ActionResult HandleSignInResponse(string contextId)
        {
            var ctxCookie = this.Request.Cookies[contextId];
            if (ctxCookie == null)
            {
                throw new InvalidOperationException("Context cookie not found");
            }

            var originalRequestUri = new Uri(ctxCookie.Value);
            this.DeleteContextCookie(contextId);

            SignInRequestMessage requestMessage = (SignInRequestMessage)WSFederationMessage.CreateFromUri(originalRequestUri);

            SecurityTokenService sts =
                new FederationSecurityTokenService(FederationSecurityTokenServiceConfiguration.Current);
            SignInResponseMessage responseMessage =
                FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(requestMessage, User, sts);
            FederatedPassiveSecurityTokenServiceOperations.ProcessSignInResponse(responseMessage, this.HttpContext.ApplicationInstance.Response);

            return this.Content(responseMessage.WriteFormPost());
        }

        private void CreateContextCookie(string contextId, string context)
        {
            var contextCookie = new HttpCookie(contextId, context);
            contextCookie.Secure = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.RequireSsl;
            contextCookie.Path = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.Path;
            contextCookie.Domain = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.Domain;
            contextCookie.HttpOnly = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.HideFromClientScript;

            TimeSpan? lifetime = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.PersistentSessionLifetime;
            if (lifetime.HasValue)
            {
                contextCookie.Expires = DateTime.UtcNow.Add(lifetime.Value);
            }

            Response.Cookies.Add(contextCookie);
        }

        private void DeleteContextCookie(string contextId)
        {
            var contextCookie = new HttpCookie(contextId);
            contextCookie.Secure = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.RequireSsl;
            contextCookie.Path = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.Path;
            contextCookie.Domain = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.Domain;
            contextCookie.HttpOnly = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.HideFromClientScript;

            contextCookie.Expires = DateTime.UtcNow.AddDays(-1);

            Response.Cookies.Add(contextCookie);
        }
    }
}
