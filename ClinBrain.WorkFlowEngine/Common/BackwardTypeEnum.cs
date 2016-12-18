using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinBrain.WorkFlowEngine.Common
{
    public enum BackwardTypeEnum
    {
        /// <summary>
        /// 撤销
        /// </summary>
        Withdrawed = 1,

        /// <summary>
        /// 退回
        /// </summary>
        Sendback = 2,

        /// <summary>
        /// 返签
        /// </summary>
        Reversed = 3
    }
}
