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
    [Table("SysDic")]
    public class DicEntity
    {
        public string iId { get; set; }
        public string iType { get; set; }
        public string iKey { get; set; }
        public string iValue { get; set; }

        public DateTime iCreatedOn { get; set; }
        public string iCreatedBy { get; set; }
        public DateTime iUpdatedOn { get; set; }

        public string iUpdatedBy { get; set; }
        public int iStatus { get; set; }
        public int iIsDeleted { get; set; }
    }
}
