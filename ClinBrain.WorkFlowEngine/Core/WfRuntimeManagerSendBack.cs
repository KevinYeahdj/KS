﻿
using System;
using System.Threading;
using System.Data.Linq;
using System.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.WorkFlowEngine.Utility;
using ClinBrain.Data;
using ClinBrain.WorkFlowEngine.Core.Result;
using ClinBrain.WorkFlowEngine.Business.Entity;
using ClinBrain.WorkFlowEngine.Business.Manager;
using ClinBrain.WorkFlowEngine.Xpdl;
using ClinBrain.WorkFlowEngine.Core.Event;
using ClinBrain.WorkFlowEngine.Core.Pattern;

namespace ClinBrain.WorkFlowEngine.Core
{
    /// <summary>
    /// 退回流程运行时
    /// </summary>
    internal class WfRuntimeManagerSendBack : WfRuntimeManager
    {
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            //创建新任务节点
            var backMostPreviouslyActivityInstanceID = GetBackwardMostPreviouslyActivityInstanceID();
            var nodeMediatorBackward = new NodeMediatorBackward(base.BackwardContext, session);
            nodeMediatorBackward.CreateBackwardActivityTaskTransitionInstance(base.BackwardContext.ProcessInstance,
                base.BackwardContext.BackwardFromActivityInstance,
                BackwardTypeEnum.Sendback,
                backMostPreviouslyActivityInstanceID,
                base.BackwardContext.BackwardToTargetTransitionGUID,
                TransitionTypeEnum.Sendback,
                TransitionFlyingTypeEnum.NotFlying,
                base.ActivityResource,
                session);

            //更新当前办理节点的状态（从准备或运行状态更新为退回状态）
            var aim = new ActivityInstanceManager();
            aim.SendBack(base.BackwardContext.BackwardFromActivityInstance.ID, 
                base.ActivityResource.AppRunner, 
                session);

            //构造回调函数需要的数据
            WfExecutedResult result = base.WfExecutedResult;
            result.BackwardTaskReciever = base.BackwardContext.BackwardTaskReciever;
            result.Status = WfExecutedStatus.Success;
        }
    }
}
