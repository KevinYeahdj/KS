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
            return View();
        }
        public ActionResult AccountIndex()
        {
            return View();
        }
        public ActionResult PositionIndex()
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

            int total = 0;
            HRInfoManager service = new HRInfoManager();
            List<HRInfoEntity> list = service.GetSearch(SessionHelper.CurrentUser.iCompanyCode, searchKey, sort, order, offset, pageSize, out total);

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
                HRInfoEntity entity = JsonConvert.DeserializeObject<HRInfoEntity>(jsonString);
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

    }
}
