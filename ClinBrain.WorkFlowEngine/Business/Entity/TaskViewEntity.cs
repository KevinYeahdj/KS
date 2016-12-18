using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.WorkFlowEngine.Utility;
using ClinBrain.WorkFlowEngine.Business;

namespace ClinBrain.WorkFlowEngine.Business.Entity
{
    /// <summary>
    /// 任务视图类
    /// </summary>
    [Table("vwWfActivityInstanceTasks")]
    public class TaskViewEntity
    {
        public int TaskID { get; set; }
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }
        public string ProcessGUID { get; set; }
        public int ProcessInstanceID { get; set; }
        public string ActivityGUID { get; set; }
        public int ActivityInstanceID { get; set; }
        public string ActivityName { get; set; }
        public short ActivityType { get; set; }
        public short TaskType { get; set; }
        public string AssignedToUserID { get; set; }
        public string AssignedToUserName { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public Nullable<System.DateTime> EndedDateTime { get; set; }
        public string EndedByUserID { get; set; }
        public string EndedByUserName { get; set; }
        public short TaskState { get; set; }
        public short ActivityState { get; set; }
        public byte RecordStatusInvalid { get; set; }
        public short ProcessState { get; set; }
        public string applicantName { get; set; }
        public DateTime applyTime { get; set; }
        public string pageUrl { get; set; }
        public string viewPageUrl { get; set; }
        public string summary { get; set; }
    }
}
