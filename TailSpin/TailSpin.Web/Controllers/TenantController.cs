namespace TailSpin.Web.Controllers
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;
    using TailSpin.Web.Models;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Stores;

    public abstract class TenantController : AsyncController
    {
        private readonly ITenantStore tenantStore;
        private string tenantName;

        protected TenantController(ITenantStore tenantStore)
        {
            this.tenantStore = tenantStore;
        }

        public Tenant Tenant { get; set; }

        public string TenantName
        {
            get { return tenantName; }

            set
            {
                tenantName = value;
                ViewData["tenant"] = value;
            }
        }

        public ITenantStore TenantStore
        {
            get { return tenantStore; }
        }

        public TenantPageViewData<T> CreateTenantPageViewData<T>(T contentModel)
        {
            var tenantPageViewData = new TenantPageViewData<T>(contentModel)
                                         {
                                             LogoUrl = Tenant == null ? string.Empty : Tenant.Logo
                                         };
            return tenantPageViewData;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.Values["tenant"] != null)
            {
                TenantName = (string) filterContext.RouteData.Values["tenant"];
            }

            if (Tenant == null)
            {
                var tenant = TenantStore.GetTenant(tenantName);
                if (tenant == null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, "'{0}' is not a valid tenant.", tenantName));
                }

                Tenant = tenant;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}