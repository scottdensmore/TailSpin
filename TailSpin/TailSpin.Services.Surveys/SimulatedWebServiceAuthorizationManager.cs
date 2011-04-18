




 




namespace TailSpin.Services.Surveys
{
    using System;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Text.RegularExpressions;
    using Microsoft.IdentityModel.Claims;

    public class SimulatedWebServiceAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            try
            {
                if (WebOperationContext.Current != null)
                {
                    var headers = WebOperationContext.Current.IncomingRequest.Headers;
                    if (headers != null)
                    {
                        var authorizationHeader = headers[HttpRequestHeader.Authorization];
                        if (!string.IsNullOrEmpty(authorizationHeader))
                        {
                            if (authorizationHeader.StartsWith("user", StringComparison.OrdinalIgnoreCase))
                            {
                                var userRegex = new Regex(@"(\w+):([^\s]+)", RegexOptions.Singleline);
                                var username = userRegex.Match(authorizationHeader).Groups[1].Value;
                                var password = userRegex.Match(authorizationHeader).Groups[2].Value;
                                if (ValidateUserAndPassword(username, password))
                                {
                                    var identity = new ClaimsIdentity(
                                        new[]
                                            {
                                                new Claim(System.IdentityModel.Claims.ClaimTypes.Name, username)
                                            },
                                        "TailSpin");

                                    var principal = ClaimsPrincipal.CreateFromIdentity(identity);
                                    operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] = principal;

                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (WebOperationContext.Current != null)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                }

                return false;
            }

            if (WebOperationContext.Current != null)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
            }

            return false;
        }

        private static bool ValidateUserAndPassword(string username, string password)
        {
            switch (username.ToLowerInvariant())
            {
                case "fred":
                case "joe":
                case "scott":
                    return !string.IsNullOrEmpty(password);
                default:
                    return false;
            }
        }
    }
}