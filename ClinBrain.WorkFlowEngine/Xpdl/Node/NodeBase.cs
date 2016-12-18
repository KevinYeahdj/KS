using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClinBrain.WorkFlowEngine.Utility;
using ClinBrain.Data;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.WorkFlowEngine.Business.Entity;

namespace ClinBrain.WorkFlowEngine.Xpdl.Node
{
    /// <summary>
    /// 节点的基类
    /// </summary>
    public abstract class NodeBase
    {
        #region 属性和构造函数

        
        internal ProcessModel _processModel;
        internal ProcessModel ProcessModel
        {
            get
            {
                if (_processModel == null)
                {
                    _processModel = new ProcessModel(this.Activity.ProcessGUID);
                }
                return _processModel;
            }
        }

        /// <summary>
        /// 节点定义属性
        /// </summary>
        public ActivityEntity Activity
        {
            get;
            set;
        }

        /// <summary>
        /// 节点实例
        /// </summary>
        public ActivityInstanceEntity ActivityInstance
        {
            get;
            set;
        }
        

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentActivity"></param>
        public NodeBase(ActivityEntity currentActivity)
        {
            Activity = currentActivity;
        }
        #endregion
    }
}
