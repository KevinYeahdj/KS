using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.WEB.Utils
{
    public static class SystemStatic
    {
        //服务器地址带虚拟目录 如http://192.168.0.2:6001/formanager/
        private static string _serverUrl;

        public static string serverUrl
        {
            get
            {
                return _serverUrl;
            }
            set
            {
                _serverUrl = value;
            }

        }
        private static string _serverUrlFull;

        public static string serverUrlFull
        {
            get
            {
                return _serverUrlFull;
            }
            set
            {
                _serverUrlFull = value;
            }
        }

        private static string _appCode;
        public static string appCode
        {
            get
            {
                return _appCode;
            }
            set
            {
                _appCode = value;
            }
        }

    }
}
