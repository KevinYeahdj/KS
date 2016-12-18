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
    /// 结束节点
    /// </summary>
    internal class EndNode : NodeBase
    {
        internal EndNode(ActivityEntity activity)
            : base(activity)
        {

        }

        
    }
}
