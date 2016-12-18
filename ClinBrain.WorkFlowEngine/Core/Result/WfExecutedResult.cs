﻿

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinBrain.WorkFlowEngine.Common;

namespace ClinBrain.WorkFlowEngine.Core.Result
{
    /// <summary>
    /// 执行结果对象封装
    /// </summary>
    public class WfExecutedResult
    {
        public WfExecutedStatus Status { get; set; }
        public String Message { get; set; }
        public String ExceptionType { get; set; }
        public Int32 ProcessInstanceIDStarted { get; set; }     //流程启动返回的新实例ID
        public WfBackwardTaskReciever BackwardTaskReciever { get; set; }    //流程做回退处理时，返回的任务接收人信息

        public WfExecutedResult()
        {
            Status = WfExecutedStatus.Default;
        }
    }

    /// <summary>
    /// 状态执行枚举类型
    /// </summary>
    public enum WfExecutedStatus
    {
        /// <summary>
        /// 缺省状态
        /// </summary>
        Default = 0,

        /// <summary>
        /// 成功状态
        /// </summary>
        Success = 1,

        /// <summary>
        /// 执行失败状态
        /// </summary>
        Failed = 2,

        /// <summary>
        /// 异常状态
        /// </summary>
        Exception = 3
    }

    public class WfExceptionType
    {
        //流程启动异常信息
        public const string Started_IsRunningAlready = "Started_IsRunningAlready";

        //流程运行异常信息
        public const string RunApp_ErrorArguments = "RunApp_ErrorArguments";
        public const string RunApp_HasNoTask = "RunApp_HasNoTask";
        public const string RunApp_OverTasks = "RunApp_OverTasks";
        public const string RunApp_RuntimeError = "RunApp_RuntimeError";

        //流程跳转异常信息
        public const string Jump_ErrorArguments = "Jump_ErrorArguments";
        public const string Jump_OverOneStep = "Jump_OverOneStep";
        public const string Jump_NotActivityBackCompleted = "Jump_NotActivityBackCompleted";
        public const string Jump_OtherError = "Jump_OtherError";

        //流程撤销异常信息
        public const string Withdraw_NotInReady = "Withdraw_NotInReady";
        public const string Withdraw_NotCreatedByMine = "Withdraw_NotCreatedByMine";
        public const string Withdraw_HasTooMany = "Withdraw_HasTooMany";
        public const string Withdraw_PreviousIsEndNode = "Withdraw_PreviousIsEndNode";
        
        //流程退回异常信息
        public const string Sendback_NotTaskNode = "Sendback_NotTaskNode";
        public const string Sendback_IsLoopNode = "Sendback_IsLoopNode";
        public const string Sendback_NotInRunning = "Sendback_NotInRunning";
        public const string Sendback_NotMineTask = "NotMineTask";
        public const string Sendback_PreviousIsStartNode = "Sendback_PreviousIsStartNode";

        //流程返签异常信息
        public const string Reverse_NotInCompleted = "Reverse_NotInCompleted";
    }
}

