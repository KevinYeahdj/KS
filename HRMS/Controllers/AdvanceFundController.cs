using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMS.Controllers;
using HRMS.Data.Entity;
using HRMS.Data.Manager;
using HRMS.WEB.Utils;

namespace HRMS.WEB.Controllers
{
    public class AdvanceFundController : BaseController
    {
        //
        // GET: /AdvanceFund/

        public ActionResult Index()
        {
            return View();
        }

    }
    public class AdvanceFundAjaxController : Controller
    {
        //
        // GET: /AdvanceFund/

        public JsonResult GetApplicationRecord(string appno)
        {
            try
            {
                AdvanceFundManager service = new AdvanceFundManager();
                AdvanceFundEntity entity = service.FirstOrDefault(appno);
                return new JsonResult { Data = new { success = false, data = entity, JsonRequestBehavior = JsonRequestBehavior.AllowGet } };

            }
            catch (Exception ex)
            {

                return new JsonResult { Data = new { success = false, msg = ex.ToString() } };
            }
        }

        public JsonResult GetUndoBillByUser(string applicant)
        {
            try
            {
                AdvanceFundManager service = new AdvanceFundManager();
                List<AdvanceFundEntity> list = service.GetUndoBillByUser(applicant);
                return new JsonResult { Data = new { success = false, data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet } };

            }
            catch (Exception ex)
            {

                return new JsonResult { Data = new { success = false, msg = ex.ToString() } };
            }
        }

    }
}
