




 




namespace TailSpin.Web.Survey.Shared.Models
{
    using System;

    [Serializable]
    public class Tenant
    {
        public string ClaimType { get; set; }
        
        public string ClaimValue { get; set; }
        
        public string HostGeoLocation { get; set; }
        
        public string IssuerThumbPrint { get; set; }
        
        public string IssuerUrl { get; set; }
        
        public string Logo { get; set; }

        public string PhoneLogo { get; set; }

        public string Name { get; set; }
        
        public string SqlAzureConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string DatabaseUserName { get; set; }

        public string DatabasePassword { get; set; }
        
        public string SqlAzureFirewallIpStart { get; set; }

        public string SqlAzureFirewallIpEnd { get; set; }
    }
}