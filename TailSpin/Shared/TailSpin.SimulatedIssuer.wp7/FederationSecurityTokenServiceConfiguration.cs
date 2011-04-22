




 




namespace TailSpin.SimulatedIssuer
{
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Permissions;
    using System.Web;
    using System.Web.Configuration;
    using Microsoft.IdentityModel.Configuration;
    using Microsoft.IdentityModel.SecurityTokenService;
    using Samples.Web.ClaimsUtillities;

    public class FederationSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
    {
        private static readonly object syncRoot = new object();
        private const string CustomSecurityTokenServiceConfigurationKey = "FederationSecurityTokenServiceConfigurationKey";

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public FederationSecurityTokenServiceConfiguration()
            : base(
                WebConfigurationManager.AppSettings[ApplicationSettingsNames.IssuerName],
                new X509SigningCredentials(
                    CertificateUtilities.GetCertificate(
                    StoreName.My,
                    StoreLocation.LocalMachine,
                    WebConfigurationManager.AppSettings[ApplicationSettingsNames.SigningCertificateName])))
        {
            this.SecurityTokenService = typeof(FederationSecurityTokenService);
        }

        public static FederationSecurityTokenServiceConfiguration Current
        {
            get
            {
                var httpAppState = HttpContext.Current.Application;

                var customConfiguration = httpAppState.Get(CustomSecurityTokenServiceConfigurationKey) as FederationSecurityTokenServiceConfiguration;

                if (customConfiguration == null)
                {
                    lock (syncRoot)
                    {
                        customConfiguration = httpAppState.Get(CustomSecurityTokenServiceConfigurationKey) as FederationSecurityTokenServiceConfiguration;

                        if (customConfiguration == null)
                        {
                            customConfiguration = new FederationSecurityTokenServiceConfiguration();
                            httpAppState.Add(CustomSecurityTokenServiceConfigurationKey, customConfiguration);
                        }
                    }
                }

                return customConfiguration;
            }
        }
    }
}