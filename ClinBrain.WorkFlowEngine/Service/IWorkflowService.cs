
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.WorkFlowEngine.Business.Entity;
using ClinBrain.WorkFlowEngine.Core;
using ClinBrain.WorkFlowEngine.Core.Result;
using ClinBrain.WorkFlowEngine.Xpdl;
using ClinBrain.WorkFlowEngine.Xpdl.Node;
using ClinBrain.WorkFlowEngine.Parser;

namespace ClinBrain.WorkFlowEngine.Service
{
    /// <summary>
    /// 工作流服务接口
    /// </summary>
    public interface IWorkflowService
    {
        ActivityEntity GetFirstActivity(string processGUID);
        List<ActivityEntity> GetTaskActivityList(string processGUID);
        ActivityEntity GetNextActivity(string processGUID, string activityGUID);
        IList<NodeView> GetNextActivity(String processGUID, String activityGUID, IDictionary<string, string> condition = null);
        NodeView GetNextActivity(int taskID, IDictionary<string, string> condition = null, IUserRoleService roleService = null);
        NodeView GetNextActivity(WfAppRunner runner, IDictionary<string, string> condition = null, IUserRoleService roleService = null);
        IList<NodeView> GetNextActivityTree(int taskID, IDictionary<string, string> condition = null, IUserRoleService roleService = null);
        IList<NodeView> GetNextActivityTree(WfAppRunner runner, IDictionary<string, string> condition = null, IUserRoleService roleService = null);
        ActivityEntity GetActivityEntity(string processGUID, string actvityGUID);
        IList<NodeImage> GetActivityInstanceCompleted(int taskID);
        IList<NodeImage> GetActivityInstanceCompleted(WfAppRunner runner);
        Performer GetProcessInitiator(int processInstanceID);

        WfExecutedResult StartProcess(WfAppRunner runner);
        WfExecutedResult StartProcess(IDbConnection conn, WfAppRunner starter, IDbTransaction trans);
        WfExecutedResult RunProcessApp(WfAppRunner runner);
        WfExecutedResult RunProcessApp(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        WfExecutedResult JumpProcess(WfAppRunner runner);
        WfExecutedResult JumpProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        WfExecutedResult WithdrawProcess(WfAppRunner runner);
        WfExecutedResult WithdrawProcess(IDbConnection conn, WfAppRunner withdrawer, IDbTransaction trans);
        WfExecutedResult SendBackProcess(WfAppRunner runner);
        WfExecutedResult SendBackProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans);
        WfExecutedResult ReverseProcess(WfAppRunner runner);
        WfExecutedResult ReverseProcess(IDbConnection conn, WfAppRunner starter, IDbTransaction trans);

        bool CancelProcess(WfAppRunner canceler);
        bool DiscardProcess(WfAppRunner discarder);
        bool SetTaskRead(WfAppRunner runner);
        IList<TaskViewEntity> GetRunningTasks(TaskQueryEntity query);
        IList<TaskViewEntity> GetReadyTasks(TaskQueryEntity query);

        IList<TaskViewEntity> GetMyCompleteTasks(TaskQueryEntity query);

        IList<ProcessInstanceEntity> GetMyApplyProcessInstances(string emplooyeeCode);

        void EntrustTask(TaskEntrustedEntity entrusted);
        ActivityInstanceEntity GetRunningNode(WfAppRunner runner);
        ProcessInstanceEntity GetRunningProcessInstance(WfAppRunner runner);
        IList<Performer> GetTaskPerformers(WfAppRunner runner);
        IList<ActivityInstanceEntity> GetActivityInstances(int processInstanceID);
        ProcessInstanceEntity GetProcessInstance(int processInstanceID);
        ActivityInstanceEntity GetActivityInstance(int activityInstanceID);
        ProcessEntity GetProcessById(string processGUID);
        List<ProcessEntity> GetProcess();
        ProcessFileEntity GetProcessFile(string processGUID);
        void SaveProcessFile(ProcessFileEntity entity);

        void CreateProcess(ProcessEntity entity);
        void UpdateProcess(ProcessEntity entity);
        void DeleteProcess(string processGUID);

        /// <summary>
        /// 流程封装提交申请 Kevin
        /// </summary>
        /// <param name="initiator"></param>
        /// <returns></returns>
        string StartApplication(WfAppRunner initiator);
        /// <summary>
        /// 流程封装提交审批 Kevin
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        string StartApproval(WfAppRunner runner);

        /// <summary>
        /// 流程封装撤销申请 Kevin
        /// </summary>
        /// <param name="initiator"></param>
        /// <returns></returns>
        string CancelApplication(WfAppRunner initiator);
    }
}
