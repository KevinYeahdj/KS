using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinBrain.WorkFlowEngine.Business.Entity
{
    /// <summary>
    /// 流程单号生成
    /// </summary>
    [Table("WfProcessNoCheck")]
    public class ProcessNoCheckEntity
    {
        public int ID { get; set; }
        public string KeyName { get; set; }
        public int NumberValue { get; set; }
    }
}
