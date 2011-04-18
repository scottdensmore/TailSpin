




 




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

        public ITenantStore TenantStore
        {
            get { return this.tenantStore; }
        }

        public string TenantName
        {
            get
            {
                return this.tenantName;
            }

            set
            {
                this.tenantName = value;
                this.ViewData["tenant"] = value;
            }
        }
        
        public Tenant Tenant { get; set; }

        public TenantPageViewData<T> CreateTenantPageViewData<T>(T contentModel)
        {
            var tenantPageViewData = new TenantPageViewData<T>(contentModel)
                                         {
                                             LogoUrl = this.Tenant == null ? string.Empty : this.Tenant.Logo
                                         };
            return tenantPageViewData;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.Values["tenant"] != null)
            {
                this.TenantName = (string)filterContext.RouteData.Values["tenant"];
            }

            if (this.Tenant == null)
            {
                var tenant = this.TenantStore.GetTenant(this.tenantName);
                if (tenant == null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, "'{0}' is not a valid tenant.", this.tenantName));
                }

                this.Tenant = tenant;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
