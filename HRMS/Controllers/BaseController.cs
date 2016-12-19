using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMS.Data.Entity;
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
            string projectPara = SessionHelper.CurrentUser.CurrentProject;
            string companyPara = SessionHelper.CurrentUser.CurrentCompany;
            if (SessionHelper.CurrentUser.UserType == "超级用户")
            {
                projectPara = "-";
                companyPara = "-";

            }
            string sql = "SELECT top 1 [iMenuRights] FROM [SysUserMenuTree] a inner join [SysMenu] b on a.iMenuId = b.iguid and a.iEmployeeCodeId = '{0}' and iCompanyId= '{1}' and iProjectId='{2}' and b.iUrl = '{3}'";
            DataSet ds = DbHelperSQL.Query(string.Format(sql, SessionHelper.CurrentUser.UserId, companyPara, projectPara, url));

            if (ds.Tables[0].Rows.Count == 0)
                return "deny";
            else return ds.Tables[0].Rows[0][0].ToString();

        }
    }
}
