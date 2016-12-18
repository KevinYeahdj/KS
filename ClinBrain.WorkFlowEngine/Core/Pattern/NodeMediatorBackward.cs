﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.WorkFlowEngine.Utility;
using ClinBrain.Data;
using ClinBrain.WorkFlowEngine.Xpdl;
using ClinBrain.WorkFlowEngine.Xpdl.Node;
using ClinBrain.WorkFlowEngine.Business.Entity;
using ClinBrain.WorkFlowEngine.Business.Manager;

namespace ClinBrain.WorkFlowEngine.Core.Pattern
{
    internal class NodeMediatorBackward : NodeMediator
    {
        internal NodeMediatorBackward(BackwardContext backwardContext, IDbSession session)
            : base(backwardContext, session)
        {
            
        }

        internal override void ExecuteWorkItem()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建退回时的流转节点对象、任务和转移数据
        /// </summary>
        /// <param name="processInstance"></param>
        /// <param name="fromActivityInstance"></param>
        /// <param name="backMostPreviouslyActivityInstanceID"></param>
        /// <param name="transitionGUID"></param>
        /// <param name="transitionType"></param>
        /// <param name="flyingType"></param>
        /// <param name="activityResource"></param>
        /// <param name="session"></param>
        internal void CreateBackwardActivityTaskTransitionInstance(ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            BackwardTypeEnum backwardType,
            int backMostPreviouslyActivityInstanceID,
            string transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            //实例化Activity
            var toActivityInstance = base.CreateBackwardToActivityInstanceObject(processInstance,
                backwardType,
                backMostPreviouslyActivityInstanceID,
                activityResource.AppRunner);

            //进入运行状态
            toActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
            toActivityInstance.AssignedToUsers = base.GenerateActivityAssignedUsers(
                activityResource.NextActivityPerformers[base.BackwardContext.BackwardToTaskActivity.ActivityGUID]);

            URLManager urlm = new URLManager();
            toActivityInstance.ApprovePageUrl = urlm.GetFlowStepUrl(processInstance.ProcessGUID, fromActivityInstance.ActivityGUID);
            toActivityInstance.ViewPageUrl = urlm.GetViewStepUrl(processInstance.ProcessGUID, fromActivityInstance.ActivityGUID);
            //插入活动实例数据
            base.ActivityInstanceManager.Insert(toActivityInstance,
                session);

            //插入任务数据
            base.CreateNewTask(toActivityInstance, activityResource, session);

            //插入转移数据
            base.InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                toActivityInstance,
                transitionType,
                flyingType,
                activityResource.AppRunner,
                session);
        }
    }
}
