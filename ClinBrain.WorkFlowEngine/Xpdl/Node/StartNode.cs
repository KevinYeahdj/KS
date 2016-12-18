using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClinBrain.Data;
using ClinBrain.WorkFlowEngine.Business.Entity;
using ClinBrain.WorkFlowEngine.Common;

namespace ClinBrain.WorkFlowEngine.Xpdl.Node
{
    /// <summary>
    /// 开始节点
    /// </summary>
    internal class StartNode : NodeBase
    {
        internal StartNode(ActivityEntity activity)
            : base(activity)
        {

        }
    }
}
