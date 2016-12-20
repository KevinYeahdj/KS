using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ClinBrain.Data.Service;
using HRMS.Data.Manager;
using HRMS.WEB.Models;
using HRMS.WEB.Utils;

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

        public ActionResult JournalApplication()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllProjects();
            ViewBag.Projects = projects;
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
                int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
                int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);

                Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
                bizParaDic.Add("currentUserId", SessionHelper.CurrentUser.UserId);

                int total = 0;
                BPMManager service = new BPMManager();
                List<TodoViewModel> list = service.GetMyTodoList(bizParaDic, sort, order, offset, pageSize, out total);

                //给分页实体赋值  
                PageModels<TodoViewModel> model = new PageModels<TodoViewModel>();
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

        public void GetMyDoneList()
        {
            try
            {
                //用于序列化实体类的对象  
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                //请求中携带的条件  
                string order = HttpContext.Request.Params["order"];
                string sort = HttpContext.Request.Params["sort"];
                int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
                int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);

                Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
                bizParaDic.Add("currentUserId", SessionHelper.CurrentUser.UserId);

                int total = 0;
                BPMManager service = new BPMManager();
                List<DoneViewModel> list = service.GetMyDoneList(bizParaDic, sort, order, offset, pageSize, out total);

                //给分页实体赋值  
                PageModels<DoneViewModel> model = new PageModels<DoneViewModel>();
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

        public void GetMyApplicationList()
        {
            try
            {
                //用于序列化实体类的对象  
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                //请求中携带的条件  
                string order = HttpContext.Request.Params["order"];
                string sort = HttpContext.Request.Params["sort"];
                int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
                int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);

                Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
                bizParaDic.Add("currentUserId", SessionHelper.CurrentUser.UserId);

                int total = 0;
                BPMManager service = new BPMManager();
                List<ApplicationViewModel> list = service.GetMyApplicationList(bizParaDic, sort, order, offset, pageSize, out total);

                //给分页实体赋值  
                PageModels<ApplicationViewModel> model = new PageModels<ApplicationViewModel>();
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
