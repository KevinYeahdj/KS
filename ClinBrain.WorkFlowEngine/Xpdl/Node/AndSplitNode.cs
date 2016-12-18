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
    /// 与分支节点
    /// </summary>
    internal class AndSplitNode : NodeBase
    {
        internal AndSplitNode(ActivityEntity activity)
            : base(activity)
        {
        }
    }
}
