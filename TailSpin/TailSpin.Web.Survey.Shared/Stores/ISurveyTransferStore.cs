




 




namespace TailSpin.Web.Survey.Shared.Stores
{
    public interface ISurveyTransferStore
    {
        void Initialize();
        void Transfer(string tenant, string slugName);
    }
}