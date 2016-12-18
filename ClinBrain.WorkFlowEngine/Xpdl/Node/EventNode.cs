using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.WorkFlowEngine.Business.Entity;

namespace ClinBrain.WorkFlowEngine.Xpdl.Node
{
    internal class EventNode : NodeBase, IDynamicRunable
    {
        internal EventNode(ActivityEntity activity)
            : base(activity)
        {

        }

        #region IDynamicRunable Members

        public object InvokeMethod(TaskImplementDetail implementation, object[] userParameters)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
