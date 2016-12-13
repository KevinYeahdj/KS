using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.WEB.Utils
{
    public static class SystemStatic
    {
        private static string ReadAppConfig(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
        private static List<string> _serverRefreshUrlList;
        public static List<string> serverRefreshUrlList
        {
            get
            {
                if (_serverRefreshUrlList == null)
                {
                    var tmp = ReadAppConfig("serverRefreshUrlList");
                    if (string.IsNullOrEmpty(tmp))
                    {
                        _serverRefreshUrlList = new List<string>();
                    }
                    else
                        _serverRefreshUrlList = tmp.ToString().Split(';', '；').ToList();
                }
                return _serverRefreshUrlList;
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
