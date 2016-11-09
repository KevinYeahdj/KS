using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HRMS.WEB.Utils;

namespace HRMS
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();
            RegisterCacheEntry();
        }

        // 注册一缓存条目在10分钟内到期，到期后触发的调事件  
        private void RegisterCacheEntry()
        {
            if (null != HttpContext.Current.Cache["dummycachekey"]) return;
            HttpContext.Current.Cache.Add("dummycachekey", "cachevalue", null, DateTime.MaxValue,
                TimeSpan.FromMinutes(10), CacheItemPriority.NotRemovable,
                new CacheItemRemovedCallback(CacheItemRemovedCallback));
        }

        // 缓存项过期时程序模拟点击页面，阻止应用程序结束  
        public void CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            HitPage();
        }

        // 模拟点击网站网页  
        private void HitPage()
        {
            System.Net.WebClient client = new System.Net.WebClient();
            client.DownloadData(SystemStatic.serverUrlFull + "Account/Login");
        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //第一次访问网站时记录根地址，为之后程序池释放后自动刷新页面来保持程序池活力做准备
            if (string.IsNullOrEmpty(SystemStatic.serverUrl) || string.IsNullOrEmpty(SystemStatic.serverUrlFull))
            {
                UrlHelper url = new UrlHelper(Request.RequestContext);
                SystemStatic.serverUrl = url.Content("~");
                SystemStatic.serverUrlFull = "http://" + Request.Url.Authority + SystemStatic.serverUrl;

            }
            if (HttpContext.Current.Request.Url.ToString() == SystemStatic.serverUrlFull + "Account/Login")
            {
                RegisterCacheEntry();
            }
        }  
    }
}