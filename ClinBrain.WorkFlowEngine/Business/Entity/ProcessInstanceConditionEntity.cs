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
    [Table("WfProcessInstanceCondition")]
    public class ProcessInstanceConditionEntity
    {
        public int ID { get; set; }
        public int ProcessInstanceID { get; set; }
        public string ConditionsJson { get; set; }
    }
}
