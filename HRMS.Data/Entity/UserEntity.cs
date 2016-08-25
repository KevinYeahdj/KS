using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinBrain.Data.Entity
{
    /// <summary>
    /// 用户对象
    /// </summary>
    [Table("SysUser")]
    public class UserEntity
    {
        public string iEmployeeCodeId { get; set; }
        public string iCompanyCode { get; set; }
        public string iUserName { get; set; }
        public string iPassWord { get; set; }

        public string iUserType { get; set; }

        public DateTime iCreatedOn { get; set; }
        public string iCreatedBy { get; set; }
        public DateTime iUpdatedOn { get; set; }

        public string iUpdatedBy { get; set; }
        public int iStatus { get; set; }
        public int iIsDeleted { get; set; }
    }
}
