using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HRMS.Data.Entity;
using HRMS.Data.Manager;
using HRMS.WEB.Models;

namespace HRMS.WEB.Controllers
{
    public class CommonAjaxController : Controller
    {
        //
        // GET: /CommonAjax/

        public ActionResult Index()
        {
            return View();
        }
        public void GetModifyRecords(string tableName)
        {
            //用于序列化实体类的对象  
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;

            //请求中携带的条件  
            string order = HttpContext.Request.Params["order"];
            string sort = HttpContext.Request.Params["sort"];
            string iguid = HttpContext.Request.Params["recordid"];
            int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
            int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("iGuid", iguid);
            dic.Add("tableName", tableName);  //传入的是表名
            int total = 0;
            ModifyLogManager service = new ModifyLogManager();
            List<ModifyLogEntity> list = service.GetSearch(dic, sort, order, offset, pageSize, out total);

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
