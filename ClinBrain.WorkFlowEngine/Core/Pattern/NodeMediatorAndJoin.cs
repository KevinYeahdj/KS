

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
    internal class NodeMediatorAndJoin : NodeMediatorGateway, ICompleteAutomaticlly
    {
        internal NodeMediatorAndJoin(ActivityEntity activity, ProcessModel processModel, IDbSession session)
            : base(activity, processModel, session)
        {

        }

        internal int GetTokensRequired(string processGuid, int processInstanceID, IDbSession session)
        {
            //此处通过配置信息获取所有前续结点   为了适应orsplit 的合并情况，应取前续被激活的结点数目
            IList<TransitionEntity> transitions = this.ProcessModel.GetBackwardTransitionList(GatewayActivity.ActivityGUID);
            
            List<ActivityInstanceEntity> acts = this.ActivityInstanceManager.GetActivityByProcessInstance(processGuid, processInstanceID, session);
            int tokensRequiredCount = 0;
            foreach (var item in transitions)
            {
                if (acts.FirstOrDefault(a => a.ActivityGUID == item.FromActivityGUID) != null)
                    tokensRequiredCount ++ ;
            }
            return tokensRequiredCount;

            int tokensRequired = this.ProcessModel.GetBackwardTransitionListCount(GatewayActivity.ActivityGUID);
            return tokensRequired;
        }

        #region ICompleteAutomaticlly 成员

        public GatewayExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionGUID,
            ActivityInstanceEntity fromActivityInstance,
            ActivityResource activityResource,
            IDbSession session)
        {
            //检查是否有运行中的合并节点实例
            ActivityInstanceEntity joinNode = base.ActivityInstanceManager.GetActivityRunning(
                processInstance.ID,
                base.GatewayActivity.ActivityGUID,
                session);

            if (joinNode == null)
            {
                var joinActivityInstance = base.CreateActivityInstanceObject(base.GatewayActivity, 
                    processInstance, activityResource.AppRunner);

                //计算总需要的Token数目
                joinActivityInstance.TokensRequired = GetTokensRequired(activityResource.AppRunner.ProcessGUID, processInstance.ID, session);
                joinActivityInstance.TokensHad = 1;

                //第一次就达到所需Token数目
                if(joinActivityInstance.TokensRequired == 1)
                {
                    joinActivityInstance.ActivityState = (short)ActivityStateEnum.Running;
                    base.GatewayActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
                }
                else
                {
                    //进入运行状态
                    joinActivityInstance.ActivityState = (short)ActivityStateEnum.Running;
                }
                
                joinActivityInstance.GatewayDirectionTypeID = (short)GatewayDirectionEnum.AndJoin;

                base.InsertActivityInstance(joinActivityInstance,
                    session);
            }
            else
            {
                //更新节点的活动实例属性
                base.GatewayActivityInstance = joinNode;
                int tokensRequired = base.GatewayActivityInstance.TokensRequired;
                int tokensHad = base.GatewayActivityInstance.TokensHad;

                //更新Token数目
                base.ActivityInstanceManager.IncreaseTokensHad(base.GatewayActivityInstance.ID,
                    activityResource.AppRunner,
                    session);

                if ((tokensHad + 1) == tokensRequired)
                {
                    //如果达到完成节点的Token数，则设置该节点状态为完成
                    base.CompleteActivityInstance(base.GatewayActivityInstance.ID,
                        activityResource,
                        session);
                    base.GatewayActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
                }
            }

            GatewayExecutedResult result = GatewayExecutedResult.CreateGatewayExecutedResult(
                GatewayExecutedStatus.Successed);
            return result;
        }

        #endregion
    }
}
