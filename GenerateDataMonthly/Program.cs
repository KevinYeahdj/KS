using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.Data.Manager;
using Kevin.Common;

namespace GenerateDataMonthly
{
    class Program
    {
        static void Main(string[] args)
        {
            //社保分地区生成，公积金以上海为准
            //上海 每月最后一天11点生成
            //苏州 每月9号晚11点生成
            //无锡 每月25号晚11点生成
            if (args.Length == 0)
                return;
            int paidMonth = int.Parse(DateTime.Now.ToString("yyyyMM"));
            string iPayPlace = args[0];
            SocialSecurityManager serviceSocialSecurity = new SocialSecurityManager();
            int count = serviceSocialSecurity.GenerateSocialSecurityDetailMonthly(paidMonth, iPayPlace);
            LogFileHelper.WriteLog("社保【" + paidMonth.ToString() + "】生成成功，生成条数【" + count.ToString() + "】");

            if (iPayPlace == "上海")
            {
                ProvidentFundManager serviceProvidentFund = new ProvidentFundManager();
                count = serviceProvidentFund.GenerateProvidentFundDetailMonthly(paidMonth);
                LogFileHelper.WriteLog("公积金【" + paidMonth.ToString() + "】生成成功，生成条数【" + count.ToString() + "】");
            }
        }
    }
}
