using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.WEB.Models
{
    public class UserViewModel
    {
        public string iEmployeeCodeId { get; set; }
        public string iCompanyCode { get; set; }

        public string iCompanyName { get; set; }
        public string iUserName { get; set; }
        public string iPassWord { get; set; }
        public string iUserType { get; set; }
        public string iUpdatedOn { get; set; }
    }
}
