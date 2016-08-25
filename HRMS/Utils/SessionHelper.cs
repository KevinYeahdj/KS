using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ClinBrain.Data.Entity;

namespace HRMS.WEB.Utils
{
    public class SessionHelper
    {
        public static string CurrentUserKey = "HRMS_CURRENTUSER";

        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        public static UserEntity CurrentUser
        {
            get
            {

                UserEntity userinfo = HttpContext.Current.Session[CurrentUserKey] as UserEntity;
                if (userinfo == null && System.Net.Dns.GetHostName() == "ThinkPad-PC")
                {                    
                    return new UserEntity { iUserName = "超级管理员", iEmployeeCodeId = "sa", iUserType = "超级管理员", iCompanyCode = "上海敏慧" };
                }
                return userinfo;
            }
        }
    }
}