using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.WorkFlowEngine.Business.Entity;

namespace ClinBrain.WorkFlowEngine.Xpdl.Node
{
    internal class ScriptNode : NodeBase, IDynamicRunable
    {
        internal ScriptNode(ActivityEntity activity)
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
