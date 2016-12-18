using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClinBrain.Data;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.WorkFlowEngine.Business.Entity;

namespace ClinBrain.WorkFlowEngine.Xpdl.Node
{
    /// <summary>
    /// 或分支节点
    /// </summary>
    internal class OrSplitNode : NodeBase
    {
        internal OrSplitNode(ActivityEntity activity) :
            base(activity)
        {
        }
        
    }
}
