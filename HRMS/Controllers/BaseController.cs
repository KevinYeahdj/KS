using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClinBrain.Data.Entity;
using HRMS.Data;
using HRMS.WEB.Utils;

namespace HRMS.Controllers
{
    [OutputCache(NoStore = true, Duration = 0)]
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //判定登录
            if (SessionHelper.CurrentUser == null)
            {
                filterContext.Result = RedirectToRoute(new { Controller = "Account", Action = "Login", returnUrl = Request.Url.ToString() });
            }
            else
            {
                ViewBag.CurrentPageRights = "unknown";
                if (!"Home".Equals(filterContext.RequestContext.RouteData.Values["controller"].ToString()))
                {            //判定访问权限
                    string result = HasVisitRights(Request.Url.LocalPath.ToLower());
                    if (result == "deny")
                    {
                        filterContext.Result = RedirectToRoute(new { Controller = "Home", Action = "Index" });
                    }
                    else
                    {
                        ViewBag.CurrentPageRights = result;
                    }

                }
            }

            base.OnActionExecuting(filterContext);
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
            log.Error(filterContext.Exception);
        }

        private string HasVisitRights(string url)
        {
            if (SessionHelper.CurrentUser.UserType == "超级管理员")
                return "w";
            //if (SessionHelper.CurrentUser.iUserType == "超级用户")
            //    SessionHelper.CurrentUser.iCompanyCode = "-";


            string sql = "SELECT top 1 [iMenuRights] FROM [SysUserMenu] a inner join [SysMenu] b on a.iMenuId = b.iguid and iEmployeeCode = '{0}' and iProjectCode='{1}' and b.iUrl = '{2}'";
            DataSet ds = DbHelperSQL.Query(string.Format(sql, SessionHelper.CurrentUser.UserId, SessionHelper.CurrentUser.CurrentProject, url));

            if (ds.Tables[0].Rows.Count == 0)
                return "deny";
            else return ds.Tables[0].Rows[0][0].ToString();

        }
    }
}
