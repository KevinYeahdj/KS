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
            int paidMonth = int.Parse(DateTime.Now.ToString("yyyyMM"));

            SocialSecurityManager serviceSocialSecurity = new SocialSecurityManager();
            int count = serviceSocialSecurity.GenerateSocialSecurityDetailMonthly(paidMonth);
            LogFileHelper.WriteLog("社保【" + paidMonth.ToString() + "】生成成功，生成条数【" + count.ToString() + "】");

            ProvidentFundManager serviceProvidentFund = new ProvidentFundManager();
            count = serviceProvidentFund.GenerateSocialSecurityDetailMonthly(paidMonth);
            LogFileHelper.WriteLog("公积金【" + paidMonth.ToString() + "】生成成功，生成条数【" + count.ToString() + "】");
        }
    }
}
