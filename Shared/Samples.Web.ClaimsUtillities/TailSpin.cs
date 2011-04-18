




 




namespace Samples.Web.ClaimsUtillities
{
    public static class TailSpin
    {
        public static string TenantName
        {
            get
            {
                return "TailSpin";
            }
        }

        public static class ClaimTypes
        {
            public static readonly string Tenant = "http://schemas.tailspin.com/claims/2010/06/tenant";
        }

        public static class Roles
        {
            public static readonly string SurveyAdministrator = "Survey Administrator";
        }
    }
}