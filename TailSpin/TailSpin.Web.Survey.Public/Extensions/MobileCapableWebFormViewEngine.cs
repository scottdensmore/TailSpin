




 




namespace TailSpin.Web.Survey.Public.Extensions
{
    using System;
    using System.Web.Mvc;

    public class MobileCapableWebFormViewEngine : WebFormViewEngine
    {
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            ViewEngineResult result = null;

            if (this.UserAgentIs(controllerContext, "IEMobile/7"))
            {
                result = new ViewEngineResult(new WebFormView("~/Views/MobileIE7/Index.aspx", string.Empty), this);
            }

            if (result == null || result.View == null)
            {
                result = base.FindView(controllerContext, viewName, masterName, useCache);
            }

            return result;
        }

        public bool UserAgentIs(ControllerContext controllerContext, string userAgentToTest)
        {
            return controllerContext.HttpContext.Request.UserAgent.IndexOf(userAgentToTest, StringComparison.OrdinalIgnoreCase) > 0;
        }
    }
}