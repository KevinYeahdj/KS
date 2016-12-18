using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRMS.Controllers
{
    public class BPMController : BaseController
    {
        //
        // GET: /BPM/
        /// <summary>
        /// 待办
        /// </summary>
        /// <returns></returns>
        public ActionResult Todo()
        {
            return View();
        }
        /// <summary>
        /// 完成
        /// </summary>
        /// <returns></returns>
        public ActionResult Done()
        {
            return View();
        }
        /// <summary>
        /// 我的申请
        /// </summary>
        /// <returns></returns>
        public ActionResult Myapp()
        {
            return View();
        }

    }

    public class BPMAjaxController : Controller
    {
        public void GetMyTodoList()
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
                string editType = HttpContext.Request.Params["sEditType"];

                Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
                bizParaDic.Add("search", searchKey);
                bizParaDic.Add("editType", editType);
                Dictionary<string, string> bizParaDicTemp = new Dictionary<string, string>();

                foreach (string para in HttpContext.Request.Params.Keys)
                {
                    if (para.StartsWith("s") && (ReturnFeeManager.ReturnFeeDic.ContainsValue("i" + para.Substring(1, para.Length - 1)) || (para.Length > 2 && ReturnFeeManager.ReturnFeeDic.ContainsValue("i" + para.Substring(1, para.Length - 2)))))
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
                ReturnFeeManager service = new ReturnFeeManager();
                List<ReturnFeeModel> list = service.GetSearch(SessionHelper.CurrentUser.UserType, bizParaDic, sort, order, offset, pageSize, out total);

                DicManager dm = new DicManager();
                var companies = dm.GetAllCompanies();
                var projects = dm.GetAllProjects();
                Dictionary<string, string> comDic = companies.ToDictionary(i => i.iGuid, i => i.iName);
                Dictionary<string, string> proDic = projects.ToDictionary(i => i.iGuid, i => i.iName);
                foreach (var item in list)
                {
                    item.iCompany = comDic[item.iCompany];
                    item.iItemName = proDic[item.iItemName];
                }

                //给分页实体赋值  
                PageModels<ReturnFeeModel> model = new PageModels<ReturnFeeModel>();
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

    }
}
