using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinBrain.WorkFlowEngine.Business.Entity
{
    /// <summary>
    /// 用户对象
    /// </summary>
    [Table("SysUser")]
    public class UserEntity
    {
        public string ID { get; set; }
        public string UserName { get; set; }
    }
}
