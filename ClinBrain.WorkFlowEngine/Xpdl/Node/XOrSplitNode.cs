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
    /// 异或类型的节点
    /// </summary>
    internal class XOrSplitNode : NodeBase
    {
        internal XOrSplitNode(ActivityEntity activity)
            : base(activity)
        {
        }
        
    }
}
