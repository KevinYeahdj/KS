using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.WorkFlowEngine.Utility;
using ClinBrain.Data;
using ClinBrain.WorkFlowEngine.Business.Entity;

namespace ClinBrain.WorkFlowEngine.Xpdl.Node
{
    /// <summary>
    /// 互斥合并类型的节点
    /// </summary>
    internal class XOrJoinNode : NodeBase
    {
        internal XOrJoinNode(ActivityEntity activity)
            : base(activity)
        {

        }
    }
}
