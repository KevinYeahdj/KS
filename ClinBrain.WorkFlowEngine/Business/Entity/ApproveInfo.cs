using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinBrain.WorkFlowEngine.Business.Entity
{
    /// <summary>
    /// 审批记录
    /// </summary>
    [Table("WfApproveInfo")]
    public class ApproveInfo
    {
        public int ID { get; set; }
        public int? TaskID { get; set; }
        public string ApproverId { get; set; }
        public string ApproverName { get; set; }
        public string AppNo { get; set; }
        public string ProcessName { get; set; }
        public string StepName { get; set; }
        public string ApproveType { get; set; }
        public string ApproveTypeName { get; set; }
        public string FeedBack { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
