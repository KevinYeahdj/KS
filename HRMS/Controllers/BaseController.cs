using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClinBrain.Data.Entity;
using HRMS.WEB.Utils;

namespace HRMS.Controllers
{
    [OutputCache(NoStore = true, Duration = 0)]
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionHelper.CurrentUser == null)
            {
                filterContext.Result = RedirectToRoute(new { Controller = "Account", Action = "Login", returnUrl = Request.Url.ToString() });
            }
            base.OnActionExecuting(filterContext);
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
            log.Error(filterContext.Exception);
        }
    }
}
