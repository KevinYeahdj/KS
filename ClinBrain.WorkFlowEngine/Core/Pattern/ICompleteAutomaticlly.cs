using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.Data;
using ClinBrain.WorkFlowEngine.Business.Entity;
using ClinBrain.WorkFlowEngine.Xpdl;

namespace ClinBrain.WorkFlowEngine.Core.Pattern
{
    /// <summary>
    /// 路由接口
    /// </summary>
    internal interface ICompleteAutomaticlly
    {
        GatewayExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionGUID,
            ActivityInstanceEntity fromActivityInstance,
            ActivityResource activityResource,
            IDbSession session);
    }
}
