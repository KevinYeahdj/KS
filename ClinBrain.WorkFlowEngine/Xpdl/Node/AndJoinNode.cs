using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.Data;
using ClinBrain.WorkFlowEngine.Business.Entity;

namespace ClinBrain.WorkFlowEngine.Xpdl.Node
{
    /// <summary>
    /// 并行合并节点
    /// </summary>
    internal class AndJoinNode : NodeBase
    {
        internal AndJoinNode(ActivityEntity activity)
            : base(activity)
        {

        }
    }
}
