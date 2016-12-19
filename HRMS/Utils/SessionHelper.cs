using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HRMS.Data.Entity;

namespace HRMS.WEB.Utils
{
    public class SessionHelper
    {
        public static string CurrentUserKey = "HRMS_CURRENTUSER";

        /// <summary>
        /// 获取当前登录用户
        /// </summary>

        public static LoginUserInfo CurrentUser
        {
            get
            {

                LoginUserInfo userinfo = HttpContext.Current.Session[CurrentUserKey] as LoginUserInfo;
                if (userinfo == null && System.Net.Dns.GetHostName() == "ThinkPad-PC")
                {
                    LoginUserInfo sa = new LoginUserInfo { UserId = "sa", UserName = "超级管理员", UserType = "超级管理员", CurrentCompany = "-", CurrentProject = "-", PassWord = "password" };
                    return sa;
                }
                return userinfo;
            }
            set
            {
                HttpContext.Current.Session[SessionHelper.CurrentUserKey] = value;
            }

        }
    }

    public class LoginUserInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CurrentCompany { get; set; }
        public string CurrentProject { get; set; }
        public string UserType { get; set; }
        public string PassWord { get; set; }
    }
}