using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ClinBrain.Data.Entity;
using HRMS.Data.Manager;
using HRMS.WEB.Models;
using HRMS.WEB.Utils;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace HRMS.Controllers
{
    public class HRController : BaseController
    {

        public ActionResult BasicIndex()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetDicByType("公司");
            ViewBag.Companies = companies;
            var projects = dm.GetDicByType("项目");
            ViewBag.Projects = projects;
            return View();
        }
        public ActionResult AccountIndex()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetDicByType("公司");
            ViewBag.Companies = companies;
            var projects = dm.GetDicByType("项目");
            ViewBag.Projects = projects;
            return View();
        }
        public ActionResult PositionIndex()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetDicByType("公司");
            ViewBag.Companies = companies;
            var projects = dm.GetDicByType("项目");
            ViewBag.Projects = projects;
            return View();
        }
        public ActionResult LogIndex()
        {
            return View();
        }

        public void GetAllHRInfo()
        {
            //用于序列化实体类的对象  
            JavaScriptSerializer jss = new JavaScriptSerializer();

            //请求中携带的条件  
            string order = HttpContext.Request.Params["order"];
            string sort = HttpContext.Request.Params["sort"];
            string searchKey = HttpContext.Request.Params["search"];
            int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
            int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);

            Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
            bizParaDic.Add("search", searchKey);

            string itemName = HttpContext.Request.Params["iItmeName"];
            string company = HttpContext.Request.Params["iCompany"];
            string idcard = HttpContext.Request.Params["iIdCard"];
            string tel = HttpContext.Request.Params["iPhone"];
            string empId = HttpContext.Request.Params["iEmpNo"];
            string status = HttpContext.Request.Params["iEmployeeStatus"];
            string employeedate1 = HttpContext.Request.Params["iEmployeeDateFrom"];
            string employeedate2 = HttpContext.Request.Params["iEmployeeDateTo"];

            string contractdeadline1 = HttpContext.Request.Params["iContractDeadLineFrom"];
            string contractdeadline2 = HttpContext.Request.Params["iContractDeadLineTo"];

            string resigndate1 = HttpContext.Request.Params["iResignDateFrom"];
            string resigndate2 = HttpContext.Request.Params["iResignDateTo"];

            string filelocation = HttpContext.Request.Params["iFileLocation"];

            string modifyon1 = HttpContext.Request.Params["iModifyOnForm"];
            string modifyon2 = HttpContext.Request.Params["iModifyOnTo"];

            bizParaDic.Add("iItmeName", itemName);
            bizParaDic.Add("iCompany", company);
            bizParaDic.Add("iIdCard", idcard);
            bizParaDic.Add("iPhone", tel);
            bizParaDic.Add("iEmpNo", empId);
            bizParaDic.Add("iEmployeeStatus", status);
            bizParaDic.Add("iEmployeeDate[d]", employeedate1 + "§" + employeedate2);
            bizParaDic.Add("iContractDeadLine[d]", contractdeadline1 + "§" + contractdeadline2);
            bizParaDic.Add("iResignDate[d]", resigndate1 + "§" + resigndate2);
            bizParaDic.Add("iFileLocation", filelocation);
            bizParaDic.Add("iUpdatedOn[d]", modifyon1 + "§" + modifyon2);

            int total = 0;
            HRInfoManager service = new HRInfoManager();
            List<HRInfoEntity> list = service.GetSearch(SessionHelper.CurrentUser.iCompanyCode, bizParaDic, sort, order, offset, pageSize, out total);

            DicManager dm = new DicManager();
            List<DicEntity> companyDicE = dm.GetDicByType("公司");
            Dictionary<string, string> companyDic = new Dictionary<string, string>();
            foreach (var item in companyDicE)
            {
                companyDic.Add(item.iKey, item.iValue);
            }
            foreach (var item in list)
            {
                item.iCompany = companyDic[item.iCompany];
            }

            //给分页实体赋值  
            PageModels<HRInfoEntity> model = new PageModels<HRInfoEntity>();
            model.total = total;
            if (total % pageSize == 0)
                model.page = total / pageSize;
            else
                model.page = (total / pageSize) + 1;

            model.rows = list;

            //将查询结果返回  
            HttpContext.Response.Write(jss.Serialize(model));
        }

        public JsonResult GetHRInfo(string guid)
        {
            try
            {
                HRInfoManager service = new HRInfoManager();
                HRInfoEntity entity = service.GetFirstOrDefault(guid);
                return new JsonResult { Data = new { success = true, msg = "msg", data = entity }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { success = false, msg = ex.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public string SaveChanges(string jsonString)
        {

            try
            {
                JsonSerializerSettings st = new JsonSerializerSettings();
                st.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                HRInfoEntity entity = JsonConvert.DeserializeObject<HRInfoEntity>(jsonString, st);
                HRInfoManager service = new HRInfoManager();
                if (entity.iGuid == "")
                {
                    entity.iCreatedBy = SessionHelper.CurrentUser.iUserName;
                    entity.iUpdatedBy = SessionHelper.CurrentUser.iUserName;
                    service.Insert(entity);
                }
                else
                {
                    entity.iUpdatedBy = SessionHelper.CurrentUser.iUserName;
                    service.Update(entity);
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public void GetHRInfoLog()
        {
            //用于序列化实体类的对象  
            JavaScriptSerializer jss = new JavaScriptSerializer();

            //请求中携带的条件  
            string order = HttpContext.Request.Params["order"];
            string sort = HttpContext.Request.Params["sort"];
            string searchKey = HttpContext.Request.Params["recordid"];
            int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
            int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);

            int total = 0;
            ModifyLogManager service = new ModifyLogManager();
            List<ModifyLogEntity> list = service.GetSearch(searchKey, sort, order, offset, pageSize, out total);

            //给分页实体赋值  
            PageModels<ModifyLogEntity> model = new PageModels<ModifyLogEntity>();
            model.total = total;
            if (total % pageSize == 0)
                model.page = total / pageSize;
            else
                model.page = (total / pageSize) + 1;

            model.rows = list;

            //将查询结果返回  
            HttpContext.Response.Write(jss.Serialize(model));
        }
    }
}
