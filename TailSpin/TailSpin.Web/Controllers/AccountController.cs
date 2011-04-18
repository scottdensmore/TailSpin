namespace TailSpin.Web.Controllers
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using TailSpin.Web.Security;
    using TailSpin.Web.Survey.Shared.Stores;

    [RequireHttps]
    [AuthenticateAndAuthorize(Roles = "Survey Administrator")]
    public class AccountController : TenantController
    {
        public AccountController(ITenantStore tenantStore) : base(tenantStore)
        {
        }

        public ActionResult Index()
        {
            var model = CreateTenantPageViewData(Tenant);
            model.Title = "My Account";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadLogo(string tenant, HttpPostedFileBase newLogo)
        {
            if (newLogo != null && newLogo.ContentLength > 0)
            {
                TenantStore.UploadLogo(tenant, new BinaryReader(newLogo.InputStream).ReadBytes(Convert.ToInt32(newLogo.InputStream.Length)));
            }

            return RedirectToAction("Index");
        }
    }
}