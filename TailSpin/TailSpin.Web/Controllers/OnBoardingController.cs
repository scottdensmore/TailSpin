namespace TailSpin.Web.Controllers
{
    using System.Web.Mvc;
    using TailSpin.Web.Models;

    public class OnBoardingController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = new TenantMasterPageViewData {Title = "On boarding"};
            return View(model);
        }

        [HttpGet]
        public ActionResult Join()
        {
            var model = new TenantMasterPageViewData {Title = "Join Tailspin"};
            return View(model);
        }
    }
}