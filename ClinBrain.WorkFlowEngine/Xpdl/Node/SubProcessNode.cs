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
    /// 子流程节点
    /// </summary>
    internal class SubProcessNode : NodeBase
    {
        public string SubProcessGUID { get; set; }

        internal SubProcessNode(ActivityEntity activity) :
            base(activity)
        {

        }
    }
}
