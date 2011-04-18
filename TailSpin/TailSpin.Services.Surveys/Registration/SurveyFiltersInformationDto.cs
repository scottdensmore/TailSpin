




 




namespace TailSpin.Services.Surveys.Registration
{
    public class SurveyFiltersInformationDto
    {
        public SurveyFiltersDto SurveyFilters { get; set; }

        public TenantDto[] AllTenants { get; set; }
    }
}