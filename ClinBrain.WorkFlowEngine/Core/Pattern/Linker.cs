using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ClinBrain.Data;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.WorkFlowEngine.Utility;
using ClinBrain.WorkFlowEngine.Xpdl;
using ClinBrain.WorkFlowEngine.Xpdl.Node;
using ClinBrain.WorkFlowEngine.Business.Entity;
using ClinBrain.WorkFlowEngine.Business.Manager;
using ClinBrain.WorkFlowEngine.Core.Result;

namespace ClinBrain.WorkFlowEngine.Core.Pattern
{
    /// <summary>
    /// 节点连接线
    /// </summary>
    public class Linker
    {
        /// <summary>
        /// 起始节点定义
        /// </summary>
        public ActivityEntity FromActivity { get; set; }
        
        /// <summary>
        /// 起始节点实例
        /// </summary>
        public ActivityInstanceEntity FromActivityInstance { get; set; }

        /// <summary>
        /// 到达节点定义
        /// </summary>
        public ActivityEntity ToActivity { get; set; }

        /// <summary>
        /// 到达节点实例
        /// </summary>
        public ActivityInstanceEntity ToActivityInstance { get; set; } 
    }
}
