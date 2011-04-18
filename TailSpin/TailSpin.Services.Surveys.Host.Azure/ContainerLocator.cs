




 




namespace TailSpin.Services.Surveys
{
    using Microsoft.Practices.Unity;

    public static class ContainerLocator
    {
        private static IUnityContainer container;

        public static IUnityContainer Container
        {
            get { return container ?? (container = new UnityContainer()); }
        }
    }
}