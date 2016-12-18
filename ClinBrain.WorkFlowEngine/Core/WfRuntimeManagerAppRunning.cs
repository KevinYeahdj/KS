
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
    /// 应用执行运行时
    /// </summary>
    internal class WfRuntimeManagerAppRunning : WfRuntimeManager
    {
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            try
            {
                var runningExecutionContext = ActivityForwardContext.CreateRunningContext(base.TaskView,
                    base.ProcessModel,
                    base.ActivityResource);

                //执行节点
                NodeMediator mediator = NodeMediatorFactory.CreateNodeMediator(runningExecutionContext, session);
                mediator.Linker.FromActivityInstance = RunningActivityInstance;
                mediator.ExecuteWorkItem();

                //构造回调函数需要的数据
                var result = base.WfExecutedResult;
                result.Status = WfExecutedStatus.Success;
                result.Message = mediator.GetNodeMediatedMessage();
            }
            catch (WfRuntimeException rx)
            {
                var result = base.WfExecutedResult;
                result.Status = WfExecutedStatus.Failed;
                result.ExceptionType = WfExceptionType.RunApp_RuntimeError;
                result.Message = rx.Message;
                throw rx;
            }
            catch (System.Exception ex)
            {
                var result = base.WfExecutedResult;
                result.Status = WfExecutedStatus.Failed;
                result.ExceptionType = WfExceptionType.RunApp_RuntimeError;
                result.Message = ex.Message;
                throw ex;
            }
        }
    }
}
