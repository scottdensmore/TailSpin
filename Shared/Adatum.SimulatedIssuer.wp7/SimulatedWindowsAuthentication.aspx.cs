




 




namespace Adatum.SimulatedIssuer
{
    using System;
    using System.Globalization;
    using Microsoft.IdentityModel.Protocols.WSFederation;
    using Microsoft.IdentityModel.SecurityTokenService;
    using Microsoft.IdentityModel.Web;

    public partial class SimulatedWindowsAuthentication : System.Web.UI.Page
    {
        protected void Page_Load()
        {
            string action = Request.QueryString[WSFederationConstants.Parameters.Action];

            try
            {
                if (action == WSFederationConstants.Actions.SignIn)
                {
                    // Process signin request.
                    if (SimulatedWindowsAuthenticationOperations.TryToAuthenticateUser(this.Context, this.Request, this.Response))
                    {
                        SecurityTokenService sts = new IdentityProviderSecurityTokenService(IdentityProviderSecurityTokenServiceConfiguration.Current);
                        SignInRequestMessage requestMessage = (SignInRequestMessage)WSFederationMessage.CreateFromUri(Request.Url);
                        SignInResponseMessage responseMessage = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(requestMessage, User, sts);
                        FederatedPassiveSecurityTokenServiceOperations.ProcessSignInResponse(responseMessage, Response);
                    }
                }
                else if (action == WSFederationConstants.Actions.SignOut)
                {
                    // Process signout request in the default page.
                    this.Response.Redirect("~/?" + Request.QueryString, false);
                }
                else
                {
                    throw new InvalidOperationException(
                        String.Format(
                            CultureInfo.InvariantCulture,
                            "The action '{0}' (Request.QueryString['{1}']) is unexpected. Expected actions are: '{2}' or '{3}'.",
                            String.IsNullOrEmpty(action) ? "<EMPTY>" : action,
                            WSFederationConstants.Parameters.Action,
                            WSFederationConstants.Actions.SignIn,
                            WSFederationConstants.Actions.SignOut));
                }
            }
            catch (Exception exception)
            {
                throw new Exception("An unexpected error occurred when processing the request. See inner exception for details.", exception);
            }
        }

        protected void ContinueButtonClick(object sender, EventArgs e)
        {
            SimulatedWindowsAuthenticationOperations.LogOnUser(this.UserList.SelectedValue, this.Context, this.Request, this.Response);
            this.HandleSignInRequest();
        }

        private void HandleSignInRequest()
        {
            SignInRequestMessage requestMessage = (SignInRequestMessage)WSFederationMessage.CreateFromUri(Request.Url);
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                SecurityTokenService sts = new IdentityProviderSecurityTokenService(IdentityProviderSecurityTokenServiceConfiguration.Current);
                SignInResponseMessage responseMessage = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(requestMessage, User, sts);
                FederatedPassiveSecurityTokenServiceOperations.ProcessSignInResponse(responseMessage, Response);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}