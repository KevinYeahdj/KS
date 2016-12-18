﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinBrain.WorkFlowEngine.Business.Entity
{
    [Table("WfProcessInstance")]
    public class ProcessInstanceEntity
    {
        public int ID { get; set; }
        public string ProcessGUID { get; set; }
        public string ProcessName { get; set; }
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }

        public string Summary { get; set; }
        public string AppInstanceCode { get; set; }
        public short ProcessState { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Nullable<int> ParentProcessInstanceID { get; set; }
        public string ParentProcessGUID { get; set; }
        public int InvokedActivityInstanceID { get; set; }
        public string InvokedActivityGUID { get; set; }
        public string CreatedByUserID { get; set; }
        public string CreatedByUserName { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
        public string LastUpdatedByUserID { get; set; }
        public string LastUpdatedByUserName { get; set; }
        public Nullable<System.DateTime> EndedDateTime { get; set; }
        public string EndedByUserID { get; set; }
        public string EndedByUserName { get; set; }
        public byte RecordStatusInvalid { get; set; }
    }
}
