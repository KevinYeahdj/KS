using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinBrain.Data;
using ClinBrain.WorkFlowEngine.Business.Entity;
using ClinBrain.WorkFlowEngine.Xpdl;

namespace ClinBrain.WorkFlowEngine.Business.Manager
{
    /// <summary>
    /// URL管理类
    /// </summary>
    public class URLManager : ManagerBase
    {
        /// <summary>
        /// 查询流程开始对应的url
        /// </summary>
        /// <returns></returns>
        public string GetFlowStartUrl(string processGUID)
        {
            var pmodel = new ProcessModel(processGUID);
            return pmodel.GetFlowStartUrl();
        }

        /// <summary>
        /// 查询流程步骤审批对应的url
        /// </summary>
        /// <returns></returns>
        public string GetFlowStepUrl(string processGUID, string activityGUID)
        {
            var pmodel = new ProcessModel(processGUID);
            return pmodel.GetFlowStepUrl(activityGUID);
        }

        /// <summary>
        /// 查询流程步骤查看对应的url
        /// </summary>
        /// <returns></returns>
        public string GetViewStepUrl(string processGUID, string activityGUID)
        {
            var pmodel = new ProcessModel(processGUID);
            return pmodel.GetViewStepUrl(activityGUID);
        }

    }
}