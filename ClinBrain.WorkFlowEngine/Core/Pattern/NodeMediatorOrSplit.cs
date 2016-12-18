

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.Data;
using ClinBrain.WorkFlowEngine.Business.Entity;
using ClinBrain.WorkFlowEngine.Xpdl;
using ClinBrain.WorkFlowEngine.Xpdl.Node;

namespace ClinBrain.WorkFlowEngine.Core.Pattern
{
    internal class NodeMediatorOrSplit : NodeMediatorGateway, ICompleteAutomaticlly
    {
        internal NodeMediatorOrSplit(ActivityEntity activity, ProcessModel processModel, IDbSession session)
            : base(activity, processModel, session)
        {

        }

        #region ICompleteAutomaticlly 成员

        public GatewayExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionGUID,
            ActivityInstanceEntity fromActivityInstance,
            ActivityResource activityResource,
            IDbSession session)
        {
            var gatewayActivityInstance = base.CreateActivityInstanceObject(base.GatewayActivity, processInstance, activityResource.AppRunner);
            gatewayActivityInstance.GatewayDirectionTypeID = (short)GatewayDirectionEnum.OrSplit;

            base.InsertActivityInstance(gatewayActivityInstance,
                session);

            base.CompleteActivityInstance(gatewayActivityInstance.ID,
                activityResource,
                session);

            gatewayActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            base.GatewayActivityInstance = gatewayActivityInstance;

            //写节点转移实例数据
            base.InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                gatewayActivityInstance,
                TransitionTypeEnum.Forward,
                TransitionFlyingTypeEnum.NotFlying,
                activityResource.AppRunner,
                session);

            GatewayExecutedResult result = GatewayExecutedResult.CreateGatewayExecutedResult(GatewayExecutedStatus.Successed);
            return result;
        }
        #endregion
    }
}
