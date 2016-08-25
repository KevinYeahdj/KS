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

        public void GetAllBasicInfo()
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
            UserManager um = new UserManager();
            List<UserEntity> list = um.GetSearch(searchKey, sort, order, offset, pageSize, out total);

            DicManager dm = new DicManager();
            List<DicEntity> companyDicE = dm.GetDicByType("公司");
            Dictionary<string, string> companyDic = new Dictionary<string, string>();
            foreach (var item in companyDicE)
            {
                companyDic.Add(item.iKey, item.iValue);
            }
            List<UserViewModel> listView = new List<UserViewModel>();
            foreach (var item in list)
            {
                listView.Add(new UserViewModel { iCompanyCode = item.iCompanyCode, iCompanyName = companyDic[item.iCompanyCode], iEmployeeCodeId = item.iEmployeeCodeId, iPassWord = item.iPassWord, iUserName = item.iUserName, iUserType = item.iUserType, iUpdatedOn = item.iUpdatedOn.ToString("yyyyMMdd HH:mm") });
            }

            //给分页实体赋值  
            PageModels<UserViewModel> model = new PageModels<UserViewModel>();
            model.total = total;
            if (total % pageSize == 0)
                model.page = total / pageSize;
            else
                model.page = (total / pageSize) + 1;

            model.rows = listView;

            //将查询结果返回  
            HttpContext.Response.Write(jss.Serialize(model));
        }

        public string UsersSaveChanges(string jsonString, string action)
        {

            try
            {
                UserEntity ue = JsonConvert.DeserializeObject<UserEntity>(jsonString);
                UserManager pm = new UserManager();
                if (action == "add")
                {
                    ue.iCreatedBy = SessionHelper.CurrentUser.iUserName;
                    ue.iUpdatedBy = SessionHelper.CurrentUser.iUserName;
                    pm.Insert(ue);
                }
                else
                {
                    UserEntity ueOld = pm.GetUser(ue.iEmployeeCodeId);
                    ue.iUpdatedBy = SessionHelper.CurrentUser.iUserName;
                    ue.iCreatedBy = ueOld.iCreatedBy;
                    ue.iCreatedOn = ueOld.iCreatedOn;
                    pm.Update(ue);
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
