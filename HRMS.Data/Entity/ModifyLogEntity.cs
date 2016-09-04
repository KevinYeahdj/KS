using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinBrain.Data.Entity
{
    /// <summary>
    /// 类型对象
    /// </summary>
    [Table("SysModifyLog")]
    public class ModifyLogEntity
    {
        public string iId { get; set; }
        public string iTableName { get; set; }
        public string iModifiedBy { get; set; }
        public DateTime iModifiedOn { get; set; }
        public string iModifiedContent { get; set; }
    }
}
