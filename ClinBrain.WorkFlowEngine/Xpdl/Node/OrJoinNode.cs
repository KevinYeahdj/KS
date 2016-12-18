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
    internal class OrJoinNode : NodeBase
    {
        internal OrJoinNode(ActivityEntity activity) :
            base(activity)
        {

        }
    }
}
