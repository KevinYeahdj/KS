using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.WorkFlowEngine.Core.Result;

namespace ClinBrain.WorkFlowEngine.Core.Event
{
    /// <summary>
    /// 工作流Event
    /// </summary>
    public class WfEventArgs : EventArgs
    {
        /// <summary>
        /// 工作项执行结果
        /// </summary>
        public WfExecutedResult WfExecutedResult { get; set; }

        public WfEventArgs(WfExecutedResult result)
            : base()
        {
            WfExecutedResult = result;
        }
    }
}
