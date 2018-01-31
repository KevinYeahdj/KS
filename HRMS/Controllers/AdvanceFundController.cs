using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HRMS.Controllers;
using HRMS.Data.Entity;
using HRMS.Data.Manager;
using HRMS.WEB.Models;
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

        public void GetUndoBillByUser()
        {
            try
            {
                PageModels5<AdvanceFundEntity> model = new PageModels5<AdvanceFundEntity>();
                //用于序列化实体类的对象  
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                AdvanceFundManager service = new AdvanceFundManager();
                string appno = Request.Params["appno"];
                string applicant = Request.Params["applicant"];
                if (!string.IsNullOrEmpty(appno))
                {
                    var entity = service.FirstOrDefault(Request.Params["appno"]);
                    applicant = entity.iApplicant;
                    model.appAmount = (decimal)entity.iAmount;
                }
                model.sum = service.GetTotalBill(applicant);
                if (string.IsNullOrEmpty(appno))
                {
                    model.appAmount = model.sum;
                }
                List<AdvanceFundEntity> list = service.GetUndoBillByUser(applicant);
                model.total = 10;
                model.page = 1;
                model.rows = list;

                //将查询结果返回  
                HttpContext.Response.Write(jss.Serialize(model));

            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ex);
            }
        }

        public JsonResult GetUndoBillByAppno(string appno)
        {
            try
            {
                AdvanceFundManager service = new AdvanceFundManager();
                var record = service.FirstOrDefault(appno);
                List<AdvanceFundEntity> list = service.GetUndoBillByUser(record.iApplicant);
                return new JsonResult { Data = new { success = false, data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet } };

            }
            catch (Exception ex)
            {

                return new JsonResult { Data = new { success = false, msg = ex.ToString() } };
            }
        }

        public void SearchAdvanceFund()
        {
            try
            {
                //用于序列化实体类的对象  
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                //请求中携带的条件  
                string order = HttpContext.Request.Params["order"];
                string sort = HttpContext.Request.Params["sort"];
                string searchKey = HttpContext.Request.Params["search"];
                int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
                int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);

                Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
                bizParaDic.Add("search", searchKey);
                Dictionary<string, string> bizParaDicTemp = new Dictionary<string, string>();

                foreach (string para in HttpContext.Request.Params.Keys)
                {
                    if (para.StartsWith("s") && (JournalManager.JournalDic.ContainsValue("i" + para.Substring(1, para.Length - 1)) || (para.Length > 2 && JournalManager.JournalDic.ContainsValue("i" + para.Substring(1, para.Length - 2)))))
                    {
                        bizParaDicTemp.Add("i" + para.Substring(1, para.Length - 1), HttpContext.Request.Params[para]);
                    }
                }
                foreach (var item in bizParaDicTemp)
                {
                    if (item.Key.EndsWith("2"))
                        continue;
                    if (bizParaDicTemp.ContainsKey(item.Key + "2"))
                    {
                        bizParaDic.Add(item.Key + "[d]", item.Value + "§" + bizParaDicTemp[item.Key + "2"]);
                    }
                    else
                    {
                        bizParaDic.Add(item.Key, item.Value);
                    }
                }

                int total = 0;
                AdvanceFundManager service = new AdvanceFundManager();
                List<AdvanceFundEntity> list = service.GetSearch(SessionHelper.CurrentUser.UserType, bizParaDic, sort, order, offset, pageSize, out total);
                DicManager dm = new DicManager();
                var companies = dm.GetAllCompanies();
                var projects = dm.GetAllProjects();
                Dictionary<string, string> comDic = companies.ToDictionary(i => i.iGuid, i => i.iName);
                Dictionary<string, string> proDic = projects.ToDictionary(i => i.iGuid, i => i.iName);
                foreach (var item in list)
                {
                    item.iCompanyId = comDic[item.iCompanyId];
                    item.iProjectId = proDic[item.iProjectId];
                }

                //给分页实体赋值  
                PageModels2<AdvanceFundEntity> model = new PageModels2<AdvanceFundEntity>();
                model.total = total;
                if (total % pageSize == 0)
                    model.page = total / pageSize;
                else
                    model.page = (total / pageSize) + 1;

                model.rows = list;
                model.sum = service.GetTotalBill(HttpContext.Request.Params["sApplicant"]);

                //将查询结果返回  
                HttpContext.Response.Write(jss.Serialize(model));
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ex);
            }
        }
        
    }
}
