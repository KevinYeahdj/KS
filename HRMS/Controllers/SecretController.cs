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
using Newtonsoft.Json;

namespace HRMS.WEB.Controllers
{
    public class SecretController : BaseController
    {
        //
        // GET: /Secret/

        public ActionResult Contract()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllProjects();
            ViewBag.Projects = projects;
            return View();
        }

    }

    public class SecretAjaxController : Controller
    {
        public void GetAllContract()
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
                    if (para.StartsWith("s") && (ContractManager.ContractDic.ContainsValue("i" + para.Substring(1, para.Length - 1)) || (para.Length > 2 && ContractManager.ContractDic.ContainsValue("i" + para.Substring(1, para.Length - 2)))))
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
                ContractManager service = new ContractManager();
                List<ContractEntity> list = service.GetSearch(SessionHelper.CurrentUser.UserType, bizParaDic, sort, order, offset, pageSize, out total);

                //DicManager dm = new DicManager();
                //var companies = dm.GetAllCompanies();
                //var projects = dm.GetAllProjects();
                //Dictionary<string, string> comDic = companies.ToDictionary(i => i.iGuid, i => i.iName);
                //Dictionary<string, string> proDic = projects.ToDictionary(i => i.iGuid, i => i.iName);
                //foreach (var item in list)
                //{
                //    item.iCompanyId = comDic[item.iCompanyId];
                //    item.iProjectId = proDic[item.iProjectId];
                //}

                //给分页实体赋值  
                PageModels<ContractEntity> model = new PageModels<ContractEntity>();
                model.total = total;
                if (total % pageSize == 0)
                    model.page = total / pageSize;
                else
                    model.page = (total / pageSize) + 1;

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
        public JsonResult GetContract(string iGuid)
        {
            try
            {
                ContractManager service = new ContractManager();
                ContractEntity entity = service.FirstOrDefault(iGuid);
                return new JsonResult { Data = new { success = true, msg = "msg", data = entity }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ex);
                return new JsonResult { Data = new { success = false, msg = ex.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public string ContractSaveChanges(string jsonString)
        {

            try
            {
                JsonSerializerSettings st = new JsonSerializerSettings();
                st.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                ContractEntity entity = JsonConvert.DeserializeObject<ContractEntity>(jsonString, st);
                ContractManager service = new ContractManager();
                if (string.IsNullOrEmpty(entity.iGuid))
                {
                    entity.iCreatedBy = SessionHelper.CurrentUser.UserName;
                    entity.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    service.Insert(entity);
                }
                else
                {
                    entity.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    service.Update(entity);
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }
}
