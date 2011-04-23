namespace TailSpin.SimulatedIssuer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Web.Configuration;
    using Microsoft.IdentityModel.Claims;
    using Microsoft.IdentityModel.Configuration;
    using Microsoft.IdentityModel.Protocols.WSIdentity;
    using Microsoft.IdentityModel.Protocols.WSTrust;
    using Microsoft.IdentityModel.SecurityTokenService;
    using Samples.Web.ClaimsUtillities;

    public class FederationSecurityTokenService : SecurityTokenService
    {
        public FederationSecurityTokenService(SecurityTokenServiceConfiguration configuration)
            : base(configuration)
        {
        }

        protected override IClaimsIdentity GetOutputClaimsIdentity(IClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {
            var output = new ClaimsIdentity();

            if (null == principal)
            {
                throw new InvalidRequestException("The caller's principal is null.");
            }

            var input = (ClaimsIdentity)principal.Identity;
            var issuer = input.Claims.First().Issuer;

            switch (issuer.ToUpperInvariant())
            {
                case "ADATUM":
                    var adatumClaimTypesToCopy = new[]
                                                     {
                                                         WSIdentityConstants.ClaimTypes.Name
                                                     };
                    CopyClaims(input, adatumClaimTypesToCopy, output);
                    TransformClaims(input, AllOrganizations.ClaimTypes.Group, Adatum.Groups.MarketingManagers, ClaimTypes.Role, TailSpin.Roles.SurveyAdministrator, output);
                    output.Claims.Add(new Claim(TailSpin.ClaimTypes.Tenant, Adatum.OrganizationName));
                    break;
                case "FABRIKAM":
                    var fabrikamClaimTypesToCopy = new[]
                                                       {
                                                           WSIdentityConstants.ClaimTypes.Name
                                                       };
                    CopyClaims(input, fabrikamClaimTypesToCopy, output);
                    TransformClaims(input, AllOrganizations.ClaimTypes.Group, Fabrikam.Groups.MarketingManagers, ClaimTypes.Role, TailSpin.Roles.SurveyAdministrator, output);
                    output.Claims.Add(new Claim(TailSpin.ClaimTypes.Tenant, Fabrikam.OrganizationName));
                    break;
                default:
                    throw new InvalidOperationException("Issuer not trusted.");
            }

            return output;
        }

        protected override Scope GetScope(IClaimsPrincipal principal, RequestSecurityToken request)
        {
            Scope scope = new Scope(request.AppliesTo.Uri.AbsoluteUri, this.SecurityTokenServiceConfiguration.SigningCredentials);

            string encryptingCertificateName = WebConfigurationManager.AppSettings[ApplicationSettingsNames.EncryptingCertificateName];
            if (!string.IsNullOrEmpty(encryptingCertificateName))
            {
                scope.EncryptingCredentials = new X509EncryptingCredentials(CertificateUtilities.GetCertificate(StoreName.My, StoreLocation.LocalMachine, encryptingCertificateName));
            }
            else
            {
                scope.TokenEncryptionRequired = false;
            }

            if (!string.IsNullOrEmpty(request.ReplyTo))
            {
                scope.ReplyToAddress = request.ReplyTo;
            }
            else
            {
                scope.ReplyToAddress = scope.AppliesToAddress;
            }

            scope.TokenEncryptionRequired = false;

            return scope;
        }

        private static void CopyClaims(IClaimsIdentity input, IEnumerable<string> claimTypes, IClaimsIdentity output)
        {
            output.Claims.CopyRange(input.Claims.Where(c => claimTypes.Contains(c.ClaimType)));
        }

        private static void TransformClaims(IClaimsIdentity input, string inputClaimType, string inputClaimValue, string outputClaimType, string outputClaimValue, IClaimsIdentity output)
        {
            var inputClaims = input.Claims.Where(c => c.ClaimType == inputClaimType);

            if ((inputClaimValue == "*") && (outputClaimValue == "*"))
            {
                var claimsToAdd = inputClaims.Select(c => new Claim(outputClaimType, c.Value));
                output.Claims.AddRange(claimsToAdd);
            }
            else
            {
                if (inputClaims.Count(c => c.Value == inputClaimValue) > 0)
                {
                    output.Claims.Add(new Claim(outputClaimType, outputClaimValue));
                }
            }
        }
    }
}