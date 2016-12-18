
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
    /// 流程启动运行时
    /// </summary>
    internal class WfRuntimeManagerStartup : WfRuntimeManager
    {
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            //构造流程实例
            var processInstance = new ProcessInstanceManager()
                .CreateNewProcessInstanceObject(base.AppRunner, base.ProcessModel.ProcessEntity, 
                base.ParentProcessInstance, base.InvokedSubProcessNode == null? null : base.InvokedSubProcessNode.ActivityInstance);

            //构造活动实例
            //1. 获取开始节点活动
            var startActivity = base.ProcessModel.GetStartActivity();

            var startExecutionContext = ActivityForwardContext.CreateStartupContext(base.ProcessModel,
                processInstance,
                startActivity,
                base.ActivityResource);

            //base.RunWorkItemIteraly(startExecutionContext, session);
            NodeMediator mediator = NodeMediatorFactory.CreateNodeMediator(startExecutionContext, session);
            mediator.Linker.FromActivityInstance = RunningActivityInstance;
            mediator.ExecuteWorkItem();

            //构造回调函数需要的数据
            WfExecutedResult result = base.WfExecutedResult;
            result.ProcessInstanceIDStarted = processInstance.ID;
            result.Status = WfExecutedStatus.Success;
        }
    }
}
