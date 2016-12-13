using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinBrain.Data.Entity;
using HRMS.Data.Manager;

namespace GenerateSocialSecurityAndProvidentFundMonthly
{
    class Program
    {
        static void Main(string[] args)
        {
            int paidMonth = int.Parse(DateTime.Now.ToString("yyyyMM"));

            SocialSecurityManager serviceSocialSecurity = new SocialSecurityManager();
            serviceSocialSecurity.GenerateSocialSecurityDetailMonthly(paidMonth);

            ProvidentFundManager serviceProvidentFund = new ProvidentFundManager();
            serviceProvidentFund.GenerateSocialSecurityDetailMonthly(paidMonth);
        }
    }
}
