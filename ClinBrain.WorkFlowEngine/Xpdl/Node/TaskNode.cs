using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.Data;
using ClinBrain.WorkFlowEngine.Business.Entity;

namespace ClinBrain.WorkFlowEngine.Xpdl.Node
{
    /// <summary>
    /// 任务节点
    /// </summary>
    internal class TaskNode : NodeBase
    {
        internal TaskNode(ActivityEntity activity) :
            base(activity)
        {

        }
    }
}
