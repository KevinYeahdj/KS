﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinBrain.WorkFlowEngine.Common
{
    /// <summary>
    /// 任务状态类型
    /// </summary>
    public enum TaskStateEnum
    {
        /// <summary>
        /// 等待办理
        /// </summary>
        Waiting = 1,

        /// <summary>
        /// 办理状态
        /// </summary>
        Handling = 2,

        /// <summary>
        /// 正常完成
        /// </summary>
        Completed = 4,

        /// <summary>
        /// 撤销
        /// </summary>
        Withdrawed = 8,

        /// <summary>
        /// 退回
        /// </summary>
        Rejected = 16,

        /// <summary>
        /// 多人可以办理，当别人办理完后，置关闭状态
        /// </summary>
        Closed = 32,
    }
}
