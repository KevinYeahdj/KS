using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.WorkFlowEngine.Xpdl.Node;

namespace ClinBrain.WorkFlowEngine.Xpdl
{
    /// <summary>
    /// 节点类型详细信息
    /// </summary>
    public class ActivityTypeDetail
    {
        public ActivityTypeEnum ActivityType { get; set; }
        public MergeTypeEnum MergeType { get; set; }
        public ComplexTypeEnum ComplexType { get; set; }
        public Nullable<float> CompleteOrder { get; set; }
        public SkipInfo SkipInfo { get; set; }
    }

    /// <summary>
    /// 节点的其它附属类型
    /// </summary>
    public enum ComplexTypeEnum
    {
        /// <summary>
        /// 多实例节点
        /// </summary>
        MultipleInstance = 1
    }

    /// <summary>
    /// 会签节点合并类型
    /// </summary>
    public enum MergeTypeEnum
    {
        /// <summary>
        /// 串行
        /// </summary>
        Sequence = 1,

        /// <summary>
        /// 并行
        /// </summary>
        Parallel = 2
    }

    /// <summary>
    /// 节点类型上描述的跳转信息
    /// </summary>
    public class SkipInfo
    {
        public Boolean IsSkip { get; set; }
        public string Skipto { get; set; }
    }
}
