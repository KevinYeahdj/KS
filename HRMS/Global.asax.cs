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
        }

        // 注册一缓存条目在20分钟内到期，到期后触发的调事件  
        private void RegisterCacheEntry()
        {
            if (null != HttpContext.Current.Cache["dummycachekey"]) return;
            HttpContext.Current.Cache.Add("dummycachekey", "cachevalue", null, DateTime.MaxValue,
                TimeSpan.FromMinutes(20), CacheItemPriority.NotRemovable,
                new CacheItemRemovedCallback(CacheItemRemovedCallback));
        }

        // 缓存项过期时程序模拟点击页面，阻止应用程序结束  
        public void CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            // 模拟点击网站网页  
            log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
            try
            {
                System.Net.WebClient client = new System.Net.WebClient();
                foreach (var item in SystemStatic.serverRefreshUrlList)
                {
                    client.DownloadData(item);
                    log.Info("模拟点击成功：模拟地址" + item);
                }
            }
            catch (Exception ex)
            {
                log.Info("模拟点击失败");
            }
        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            RegisterCacheEntry();  //重新申请缓存
        }
    }
}