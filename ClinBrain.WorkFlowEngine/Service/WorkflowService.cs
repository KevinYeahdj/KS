
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Threading;
using ClinBrain.WorkFlowEngine.Core;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.WorkFlowEngine.Utility;
using ClinBrain.Data;
using ClinBrain.WorkFlowEngine.Xpdl;
using ClinBrain.WorkFlowEngine.Business.Entity;
using ClinBrain.WorkFlowEngine.Business.Manager;
using ClinBrain.WorkFlowEngine.Core.Result;
using ClinBrain.WorkFlowEngine.Core.Event;
using ClinBrain.WorkFlowEngine.Parser;
using ClinBrain.OC;
using Newtonsoft.Json;

namespace ClinBrain.WorkFlowEngine.Service
{
    /// <summary>
    /// 工作流服务(执行部分)
    /// </summary>
    public partial class WorkflowService : IWorkflowService
    {
        #region 构造方法
        public WorkflowService()
        {

        }

        #endregion

        #region 流程定义数据
        /// <summary>
        /// 流程定义数据读取
        /// </summary>
        /// <param name="processGUID">流程定义GUID</param>
        /// <returns></returns>
        public ProcessEntity GetProcessById(string processGUID)
        {
            var pm = new ProcessManager();
            var entity = pm.GetByGUID(processGUID);

            return entity;
        }

        /// <summary>
        /// 获取流程定义数据
        /// </summary>
        /// <returns></returns>
        public List<ProcessEntity> GetProcess()
        {
            var pm = new ProcessManager();
            var list = pm.GetAll();

            return list;
        }

        /// <summary>
        /// 流程定义的XML文件获取和保存
        /// </summary>
        /// <param name="processGUID"></param>
        /// <returns></returns>
        public ProcessFileEntity GetProcessFile(string processGUID)
        {
            var processModel = new ProcessModel(processGUID);
            var entity = processModel.GetProcessFile();

            return entity;
        }

        /// <summary>
        /// 保存流程定义的xml文件
        /// </summary>
        /// <param name="diagram"></param>
        public void SaveProcessFile(ProcessFileEntity entity)
        {
            var processModel = new ProcessModel(entity.ProcessGUID);
            processModel.SaveProcessFile(entity);

        }

        /// <summary>
        /// 创建流程定义记录
        /// </summary>
        /// <param name="entity"></param>
        public void CreateProcess(ProcessEntity entity)
        {
            var processManager = new ProcessManager();
            entity.ProcessGUID = Guid.NewGuid().ToString();
            entity.CreatedDateTime = DateTime.Now;
            entity.XmlFilePath = string.Format("{0}\\{1}", entity.AppType, entity.XmlFileName);

            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                processManager.Insert(session.Connection, entity, session.Transaction);

                //创建xml文件到本地目录
                var serverPath = ConfigHelper.GetAppSettingString("WorkflowFileServer");
                var physicalFileName = string.Format("{0}\\{1}", serverPath, entity.XmlFilePath);

                //判断目录是否存在，否则创建
                var pathName = Path.GetDirectoryName(physicalFileName);
                if (!Directory.Exists(pathName))
                    Directory.CreateDirectory(pathName);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml("<Package/>");
                XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

                //Add the new node to the document.
                XmlElement root = xmlDoc.DocumentElement;
                xmlDoc.InsertBefore(xmldecl, root);

                XmlElement workflowNode = xmlDoc.CreateElement("WorkflowProcess");
                XmlElement processNode = xmlDoc.CreateElement("Process");
                processNode.SetAttribute("name", entity.ProcessName);
                processNode.SetAttribute("id", entity.ProcessGUID);

                XmlElement descriptionNode = xmlDoc.CreateElement("Description");
                descriptionNode.InnerText = entity.Description;
                processNode.AppendChild(descriptionNode);

                workflowNode.AppendChild(processNode);
                root.AppendChild(workflowNode);

                xmlDoc.Save(physicalFileName);

                session.Commit();
            }
            catch (System.Exception ex)
            {
                session.Rollback();
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 更新流程定义记录
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateProcess(ProcessEntity entity)
        {
            var processManager = new ProcessManager();
            processManager.Update(entity);
        }

        /// <summary>
        /// 删除流程定义记录
        /// </summary>
        /// <param name="processGUID"></param>
        public void DeleteProcess(string processGUID)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                var processManager = new ProcessManager();
                var entity = processManager.GetByGUID(processGUID);
                processManager.Delete(session.Connection, entity, session.Transaction);

                //delete the xml file
                var serverPath = ConfigHelper.GetAppSettingString("WorkflowFileServer");
                var physicalFileName = string.Format("{0}\\{1}", serverPath, entity.XmlFilePath);
                File.Delete(physicalFileName);

                session.Commit();
            }
            catch (System.Exception ex)
            {
                session.Rollback();
            }
            finally
            {
                session.Dispose();
            }
        }
        #endregion

        #region 获取节点信息
        /// <summary>
        /// 获取流程的第一个可办理节点
        /// </summary>
        /// <returns></returns>
        public ActivityEntity GetFirstActivity(string processGUID)
        {
            var processModel = new ProcessModel(processGUID);
            var firstActivity = processModel.GetFirstActivity();
            return firstActivity;
        }

        /// <summary>
        /// 获取任务类型的节点列表
        /// </summary>
        /// <returns></returns>
        public List<ActivityEntity> GetTaskActivityList(string processGUID)
        {
            var processModel = new ProcessModel(processGUID);
            var activityList = processModel.GetTaskActivityList();

            return activityList;
        }

        /// <summary>
        /// 获取当前节点的下一个节点信息
        /// </summary>
        /// <param name="processGUID"></param>
        /// <param name="activityGUID"></param>
        /// <returns></returns>
        public ActivityEntity GetNextActivity(string processGUID, string activityGUID)
        {
            var processModel = new ProcessModel(processGUID);
            var nextActivity = processModel.GetNextActivity(activityGUID);
            return nextActivity;
        }

        /// <summary>
        /// 简单模式：根据应用获取流程下一步节点（不考虑有多个后续节点的情况）
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="condition"></param>
        /// <param name="roleService"></param>
        /// <returns></returns>
        public NodeView GetNextActivity(WfAppRunner runner,
            IDictionary<string, string> condition = null,
            IUserRoleService roleService = null)
        {
            NodeView nodeView = null;
            var list = GetNextActivityTree(runner, condition, roleService).ToList();
            if (list != null && list.Count() > 0)
            {
                nodeView = list[0];
            }
            return nodeView;
        }

        /// <summary>
        /// 根据应用获取流程下一步节点列表
        /// </summary>
        /// <param name="runner">应用执行人</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public IList<NodeView> GetNextActivityTree(WfAppRunner runner,
            IDictionary<string, string> condition = null,
            IUserRoleService roleService = null)
        {
            var tm = new TaskManager();
            var task = tm.GetTaskOfMine(runner.AppInstanceID, runner.ProcessGUID, runner.UserID);
            var processModel = new ProcessModel(runner.ProcessGUID);
            var nextSteps = processModel.GetNextActivityTree(task.ProcessInstanceID,
                task.ActivityGUID,
                condition,
                roleService);

            return nextSteps;
        }

        /// <summary>
        /// 获取下一步活动列表树
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public IList<NodeView> GetNextActivityTree(int taskID,
            IDictionary<string, string> condition = null,
            IUserRoleService roleService = null)
        {
            var task = (new TaskManager()).GetTaskView(taskID);
            var processModel = new ProcessModel(task.ProcessGUID);
            var nextSteps = processModel.GetNextActivityTree(task.ProcessInstanceID,
                task.ActivityGUID,
                condition,
                roleService);

            return nextSteps;
        }

        /// <summary>
        /// 简单模式：根据应用获取流程下一步节点（不考虑有多个后续节点的情况）
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="condition"></param>
        /// <param name="roleService"></param>
        /// <returns></returns>
        public NodeView GetNextActivity(int taskID,
            IDictionary<string, string> condition = null,
            IUserRoleService roleService = null)
        {
            NodeView nodeView = null;
            var list = GetNextActivityTree(taskID, condition, roleService).ToList();
            if (list != null && list.Count() > 0)
            {
                nodeView = list[0];
            }
            return nodeView;
        }

        /// <summary>
        /// 获取已经完成的节点
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public IList<NodeImage> GetActivityInstanceCompleted(int taskID)
        {
            IList<NodeImage> imageList = new List<NodeImage>();

            var tm = new TaskManager();
            var task = tm.GetTaskView(taskID);

            var am = new ActivityInstanceManager();
            var list = am.GetCompletedActivityInstanceList(task.AppInstanceID, task.ProcessGUID);

            foreach (ActivityInstanceEntity a in list)
            {
                imageList.Add(new NodeImage
                {
                    ID = a.ID,
                    ActivityName = a.ActivityName
                });
            }
            return imageList;
        }

        /// <summary>
        /// 获取已经完成的节点记录
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        public IList<NodeImage> GetActivityInstanceCompleted(WfAppRunner runner)
        {
            IList<NodeImage> imageList = new List<NodeImage>();
            var am = new ActivityInstanceManager();
            var list = am.GetCompletedActivityInstanceList(runner.AppInstanceID, runner.ProcessGUID);

            foreach (ActivityInstanceEntity a in list)
            {
                imageList.Add(new NodeImage
                {
                    ID = a.ID,
                    ActivityName = a.ActivityName
                });
            }
            return imageList;
        }

        /// <summary>
        /// 获取当前活动实体
        /// </summary>
        /// <param name="processGUID"></param>
        /// <param name="activityGUID"></param>
        /// <returns></returns>
        public ActivityEntity GetActivityEntity(string processGUID, string activityGUID)
        {
            var processModel = new ProcessModel(processGUID);
            return processModel.GetActivity(activityGUID);

        }

        /// <summary>
        /// 获取当前节点的下一个节点信息[]
        /// </summary>
        /// <param name="processGUID"></param>
        /// <param name="activityGUID"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IList<NodeView> GetNextActivity(String processGUID,
            String activityGUID,
            IDictionary<string, string> condition = null)
        {
            var processModel = new ProcessModel(processGUID);
            var nextSteps = processModel.GetNextActivityTree(activityGUID, condition);
            return nextSteps;
        }
        #endregion

        #region 流程启动
        private AutoResetEvent waitHandler = new AutoResetEvent(false);
        private WfExecutedResult _startedResult = null;
        private WfExecutedResult _runAppResult = null;
        private WfExecutedResult _withdrawedResult = null;
        private WfExecutedResult _sendbackResult = null;
        private WfExecutedResult _reversedResult = null;
        private WfExecutedResult _jumpResult = null;
        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="starter">启动人</param>
        /// <returns>启动结果</returns>
        public WfExecutedResult StartProcess(WfAppRunner starter)
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = StartProcess(conn, starter, trans);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="starter">启动人</param>
        /// <param name="trans">事务</param>
        /// <returns>启动结果</returns>
        public WfExecutedResult StartProcess(IDbConnection conn, WfAppRunner starter, IDbTransaction trans)
        {
            try
            {
                IDbSession session = SessionFactory.CreateSession(conn, trans);
                var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceStartup(starter, ref _startedResult);

                if (_startedResult.Status == WfExecutedStatus.Exception)
                {
                    return _startedResult;
                }

                runtimeInstance.OnWfProcessExecuted += runtimeInstance_OnWfProcessStarted;
                runtimeInstance.Execute(session);

                //do something else here...

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                _startedResult.Status = WfExecutedStatus.Failed;
                _startedResult.Message = string.Format("流程启动发生错误，内部异常:{0}", e.Message);
                LogManager.RecordLog(WfDefine.WF_PROCESS_ERROR, LogEventType.Error, LogPriority.High, starter, e);
            }
            return _startedResult;
        }

        private void runtimeInstance_OnWfProcessStarted(object sender, WfEventArgs args)
        {
            _startedResult = args.WfExecutedResult;
            waitHandler.Set();
        }
        #endregion

        #region 运行流程
        /// <summary>
        /// 运行流程(业务处理)
        /// </summary>
        /// <param name="runner">运行人</param>
        /// <returns>运行结果</returns>
        public WfExecutedResult RunProcessApp(WfAppRunner runner)
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = RunProcessApp(conn, runner, trans);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// 运行流程
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="runner">运行人</param>
        /// <param name="trans">事务</param>
        /// <returns>运行结果</returns>
        public WfExecutedResult RunProcessApp(IDbConnection conn,
            WfAppRunner runner,
            IDbTransaction trans)
        {
            try
            {
                IDbSession session = SessionFactory.CreateSession(conn, trans);
                var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceAppRunning(runner, ref _runAppResult);

                if (_runAppResult.Status == WfExecutedStatus.Exception)
                {
                    return _runAppResult;
                }

                runtimeInstance.OnWfProcessExecuted += runtimeInstance_OnWfProcessContinued;
                bool isRunning = runtimeInstance.Execute(session);

                waitHandler.WaitOne();

                return _runAppResult;
            }
            catch (System.Exception e)
            {
                _runAppResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                _runAppResult.Message = string.Format("流程运行时发生异常！，详细错误：{0}", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }

            return _runAppResult;
        }

        private void runtimeInstance_OnWfProcessContinued(object sender, WfEventArgs args)
        {
            _runAppResult = args.WfExecutedResult;
            waitHandler.Set();
        }
        #endregion

        #region 流程跳转
        /// <summary>
        /// 流程跳转
        /// </summary>
        /// <param name="runner">执行操作人</param>
        /// <returns>跳转结果</returns>
        public WfExecutedResult JumpProcess(WfAppRunner runner)
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = JumpProcess(conn, runner, trans);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// 流程跳转
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="runner">执行操作人</param>
        /// <param name="trans">事务</param>
        /// <returns>跳转结果</returns>
        public WfExecutedResult JumpProcess(IDbConnection conn,
            WfAppRunner runner,
            IDbTransaction trans)
        {
            try
            {
                IDbSession session = SessionFactory.CreateSession(conn, trans);
                var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceJump(runner, ref _jumpResult);

                if (_jumpResult.Status == WfExecutedStatus.Exception)
                {
                    return _jumpResult;
                }

                runtimeInstance.OnWfProcessExecuted += runtimeInstance_OnWfProcessJump;
                bool isJumping = runtimeInstance.Execute(session);

                waitHandler.WaitOne();

                return _jumpResult;
            }
            catch (System.Exception e)
            {
                _jumpResult.ExceptionType = WfExceptionType.Jump_OtherError;
                _jumpResult.Message = string.Format("流程跳转时发生异常！，详细错误：{0}", e.Message);
                LogManager.RecordLog(WfDefine.WF_PROCESS_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }

            return _jumpResult;
        }

        private void runtimeInstance_OnWfProcessJump(object sender, WfEventArgs args)
        {
            _jumpResult = args.WfExecutedResult;
            waitHandler.Set();
        }
        #endregion

        #region 流程撤销、回退和返签（已经结束的流程可以被复活）
        /// <summary>
        /// 流程撤销
        /// </summary>
        /// <param name="runner">撤销人</param>
        /// <returns>撤销结果</returns>
        public WfExecutedResult WithdrawProcess(WfAppRunner runner)
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = WithdrawProcess(conn, runner, trans);

                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// 撤销流程
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="withdrawer">撤销人</param>
        /// <param name="trans">事务</param>
        /// <returns>撤销结果</returns>
        public WfExecutedResult WithdrawProcess(IDbConnection conn,
            WfAppRunner withdrawer,
            IDbTransaction trans)
        {
            try
            {
                IDbSession session = SessionFactory.CreateSession(conn, trans);
                var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceWithdraw(withdrawer, ref _withdrawedResult);

                //不满足撤销操作，返回异常结果信息
                if (_withdrawedResult.Status == WfExecutedStatus.Exception)
                {
                    return _withdrawedResult;
                }

                runtimeInstance.OnWfProcessExecuted += runtimeInstance_OnWfProcessWithdrawed;
                bool isWithdrawed = runtimeInstance.Execute(session);

                waitHandler.WaitOne();

                return _withdrawedResult;
            }
            catch (System.Exception e)
            {
                _withdrawedResult.Status = WfExecutedStatus.Failed;
                _withdrawedResult.Message = string.Format("流程撤销发生异常！，详细错误：{0}", e.Message);
                LogManager.RecordLog(WfDefine.WF_PROCESS_ERROR, LogEventType.Error, LogPriority.High, withdrawer, e);
            }
            return _withdrawedResult;
        }

        private void runtimeInstance_OnWfProcessWithdrawed(object sender, WfEventArgs args)
        {
            _withdrawedResult = args.WfExecutedResult;
            waitHandler.Set();
        }

        /// <summary>
        /// 退回到上一步
        /// </summary>
        /// <param name="runner">退回操作人</param>
        /// <returns>退回结果</returns>
        public WfExecutedResult SendBackProcess(WfAppRunner runner)
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = SendBackProcess(conn, runner, trans);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// 退回到上一步
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="sender">退回人</param>
        /// <param name="trans">事务</param>
        /// <returns>退回结果</returns>
        public WfExecutedResult SendBackProcess(IDbConnection conn,
            WfAppRunner sender,
            IDbTransaction trans)
        {
            try
            {
                IDbSession session = SessionFactory.CreateSession(conn, trans);
                var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceSendBack(sender, ref _sendbackResult);

                if (_sendbackResult.Status == WfExecutedStatus.Exception)
                {
                    return _sendbackResult;
                }

                runtimeInstance.OnWfProcessExecuted += runtimeInstance_OnWfProcessSentBack;
                bool isRejected = runtimeInstance.Execute(session);

                waitHandler.WaitOne();

                return _sendbackResult;
            }
            catch (System.Exception e)
            {
                _sendbackResult.Status = WfExecutedStatus.Failed;
                _sendbackResult.Message = string.Format("流程退回发生异常！，详细错误：{0}", e.Message);
                LogManager.RecordLog(WfDefine.WF_PROCESS_ERROR, LogEventType.Error, LogPriority.High, sender, e);
            }
            return _sendbackResult;
        }

        private void runtimeInstance_OnWfProcessSentBack(object sender, WfEventArgs args)
        {
            _sendbackResult = args.WfExecutedResult;
            waitHandler.Set();
        }

        /// <summary>
        /// 流程返签
        /// </summary>
        /// <param name="ender">结束人</param>
        /// <returns>返签结果</returns>
        public WfExecutedResult ReverseProcess(WfAppRunner runner)
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = ReverseProcess(conn, runner, trans);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// 流程返签
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="ender">结束人</param>
        /// <param name="trans">事务</param>
        /// <returns>返签结果</returns>
        public WfExecutedResult ReverseProcess(IDbConnection conn,
            WfAppRunner ender,
            IDbTransaction trans)
        {
            try
            {
                IDbSession session = SessionFactory.CreateSession(conn, trans);
                var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceReverse(ender, ref _reversedResult);

                if (_reversedResult.Status == WfExecutedStatus.Exception)
                {
                    return _reversedResult;
                }

                runtimeInstance.OnWfProcessExecuted += runtimeInstance_OnWfProcessReversed;
                bool isReversed = runtimeInstance.Execute(session);

                waitHandler.WaitOne();

                //return _wfExecutedResult;
                return _reversedResult;
            }
            catch (System.Exception e)
            {
                _reversedResult.Status = WfExecutedStatus.Failed;
                _reversedResult.Message = string.Format("流程返签发生异常！，详细错误：{0}", e.Message);
                LogManager.RecordLog(WfDefine.WF_PROCESS_ERROR, LogEventType.Error, LogPriority.High, ender, e);
            }
            return _reversedResult;
        }

        private void runtimeInstance_OnWfProcessReversed(object sender, WfEventArgs args)
        {
            _reversedResult = args.WfExecutedResult;
            waitHandler.Set();
        }
        #endregion

        #region 取消（运行的）流程、废弃执行中或执行完的流程
        /// <summary>
        /// 取消流程
        /// </summary>
        /// <param name="canceller">执行操作的用户</param>
        /// <returns>执行结果的标志</returns>
        public bool CancelProcess(WfAppRunner runner)
        {
            var pim = new ProcessInstanceManager();
            return pim.Cancel(runner);
        }

        /// <summary>
        /// 废弃流程
        /// </summary>
        /// <param name="discarder">执行操作的用户</param>
        /// <returns>执行结果的标志</returns>
        public bool DiscardProcess(WfAppRunner runner)
        {
            var pim = new ProcessInstanceManager();
            return pim.Discard(runner);
        }
        #endregion

        #region 任务读取和处理
        /// <summary>
        /// 设置任务为已读状态(根据任务ID获取任务)
        /// </summary>
        /// <param name="runner">执行人</param>
        /// <returns>任务读取的标志</returns>
        public bool SetTaskRead(WfAppRunner taskRunner)
        {
            bool isRead = false;
            try
            {
                var taskManager = new TaskManager();
                taskManager.SetTaskRead(taskRunner);
                isRead = true;
            }
            catch (System.Exception)
            {
                throw;
            }

            return isRead;
        }

        /// <summary>
        /// 获取运行中的任务
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <returns>任务列表</returns>
        public IList<TaskViewEntity> GetRunningTasks(TaskQueryEntity query)
        {
            int allRowsCount = 0;
            var taskManager = new TaskManager();
            var taskList = taskManager.GetRunningTasks(query, out allRowsCount);
            if (taskList != null)
                return taskList.ToList();
            else
                return null;
        }

        /// <summary>
        /// 获取待办任务
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>任务列表</returns>
        public IList<TaskViewEntity> GetReadyTasks(TaskQueryEntity query)
        {
            int allRowsCount = 0;
            var taskManager = new TaskManager();
            var taskList = taskManager.GetReadyTasks(query, out allRowsCount);

            if (taskList != null)
                return taskList.ToList();
            else
                return null;
        }

        /// <summary>
        /// 获取完成任务
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>任务列表</returns>
        public IList<TaskViewEntity> GetMyCompleteTasks(TaskQueryEntity query)
        {
            int allRowsCount = 0;
            var taskManager = new TaskManager();
            var taskList = taskManager.GetMyCompleteTasks(query, out allRowsCount);

            if (taskList != null)
                return taskList.ToList();
            else
                return null;
        }


        public IList<ProcessInstanceEntity> GetMyApplyProcessInstances(string emplooyeeCode)
        {
            var piManager = new ProcessInstanceManager();
            IList<ProcessInstanceEntity> processInstances = piManager.GetMyProcessInstance(emplooyeeCode).ToList();
            return processInstances;

        }


        #endregion



        #region 流程外部封装 Kevin
        /// <summary>
        /// 开始流程申请操作
        /// </summary>
        /// <param name="initiator"></param>
        /// <returns></returns>
        public string StartApplication(WfAppRunner initiator)
        {
            ProcessEntity pe = GetProcessById(initiator.ProcessGUID);
            initiator.AppName = pe.ProcessName;  //初始化appName
            ApplicationNoHelper anh = new ApplicationNoHelper();
            string appNo = anh.GetFlowApplicationNo(initiator.UserID, pe.AppType);
            initiator.AppInstanceID = appNo;
            //初始化流程
            WfExecutedResult startedResult = StartProcess(initiator);
            if (startedResult.Status != WfExecutedStatus.Success)
            {
                return "fail";
                //给出提示
            }

            //流程开始节点
            ActivityEntity firstActivity = GetFirstActivity(initiator.ProcessGUID);
            initiator.CurrentStepName = firstActivity.ActivityName;   //kevin 0629

            initiator.Conditions.Add("sys_appno", appNo);
            //保存condition
            string conditionJson = JsonConvert.SerializeObject(initiator.Conditions);
            //IDictionary<string, string> obj = (IDictionary<string, string>)JsonConvert.DeserializeObject(conditionJson, typeof(IDictionary<string, string>));
            ProcessInstanceConditionEntity pice = new ProcessInstanceConditionEntity();
            pice.ProcessInstanceID = startedResult.ProcessInstanceIDStarted;
            pice.ConditionsJson = conditionJson;
            ProcessInstanceManager pim = new ProcessInstanceManager();
            pim.InsertConditionJson(pice);

            //流程第一步
            IList<NodeView> nextActivities = GetNextActivity(initiator.ProcessGUID, firstActivity.ActivityGUID, initiator.Conditions);
            IOC os = new OrganizationService();
            RoleManager rm = new RoleManager();
            if (nextActivities.Count() > 0)
            {
                //获取流程节点定义的接收人
                initiator.NextActivityPerformers = new Dictionary<String, PerformerList>();
                foreach (var node in nextActivities)
                {
                    PerformerList perList = new PerformerList();
                    foreach (var part in node.Participants)
                    {
                        string outerid = part.OuterID;
                        switch (part.Type)
                        {
                            case "User":
                                {
                                    foreach (string id in outerid.Split(';'))
                                    {
                                        UserInfo user = os.GetUserInfoByLoginName(id);
                                        perList.Add(new Performer(user.EmployeeCode, user.Name));
                                    }
                                    break;
                                }
                            case "Role":
                                {
                                    List<UserEntity> users = rm.GetByRoleCode(outerid);
                                    foreach (UserEntity ur in users)
                                    {
                                        //UserInfo user = os.GetUserInfoByLoginName(ur.ID.ToString());
                                        //perList.Add(new Performer(user.EmployeeCode, user.Name));
                                        perList.Add(new Performer(ur.ID, ur.UserName));
                                    }
                                    break;
                                }
                            case "OC":
                                {
                                    var ocUsers = os.GetOCTypeParticipants(int.Parse(outerid), initiator.UserID, initiator.UserID);
                                    foreach (var item in ocUsers)
                                    {
                                        perList.Add(new Performer(item.EmployeeCode.ToString(), item.Name));
                                    }
                                    break;
                                }
                            case "Var":
                                {
                                    if (initiator.Conditions.ContainsKey(outerid))
                                    {
                                        foreach (string userid in initiator.Conditions[outerid].Split(';'))
                                        {
                                            UserInfo user = os.GetUserInfoByLoginName(userid);
                                            perList.Add(new Performer(user.EmployeeCode, user.Name));
                                        }
                                    }
                                    break;
                                }
                        }
                    }
                    initiator.NextActivityPerformers.Add(node.ActivityGUID, perList);

                }
                //自动审批过第一个节点
                WfExecutedResult runAppResult = RunProcessApp(initiator);
                if (runAppResult.Status != WfExecutedStatus.Success)
                {
                    return "fail";
                }
                return appNo;
            }
            return "fail";
        }

        /// <summary>
        /// 开始流程审批
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        public string StartApproval(WfAppRunner runner)
        {
            ProcessEntity pe = GetProcessById(runner.ProcessGUID);
            runner.AppName = pe.ProcessName;  //初始化appName
            //更新condition
            ProcessInstanceManager pim = new ProcessInstanceManager();
            ProcessInstanceConditionEntity pice = pim.GetProcessInstanceCondition(int.Parse(runner.ProcessInstanceID));
            IDictionary<string, string> conditions = (IDictionary<string, string>)JsonConvert.DeserializeObject(pice.ConditionsJson, typeof(IDictionary<string, string>));
            if (conditions == null)
                conditions = new Dictionary<string, string>();
            foreach (var con in runner.Conditions)
            {
                if (conditions.ContainsKey(con.Key))
                    conditions[con.Key] = con.Value;
                else
                    conditions.Add(con.Key, con.Value);
            }
            string conditionJson = JsonConvert.SerializeObject(conditions);
            pice.ConditionsJson = conditionJson;
            pim.UpdateConditionJson(pice);
            runner.Conditions = conditions;


            PerformerList pList = new PerformerList();
            //获取流程申请人信息,因为流程审批时可能用到
            Performer flowInitator = GetProcessInitiator(int.Parse(runner.ProcessInstanceID));
            //获取下一步节点信息
            IList<NodeView> nextActivities = GetNextActivityTree(runner, runner.Conditions);
            IOC os = new OrganizationService();
            RoleManager rm = new RoleManager();
            if (nextActivities.Count() > 0)
            {
                runner.NextActivityPerformers = new Dictionary<String, PerformerList>();
                foreach (var node in nextActivities)
                {
                    PerformerList perList = new PerformerList();
                    foreach (var part in node.Participants)
                    {
                        string outerid = part.OuterID;
                        if (string.IsNullOrEmpty(outerid))
                            continue;  //无效
                        switch (part.Type)
                        {
                            case "User":
                                {
                                    foreach (string id in outerid.Split(';'))
                                    {
                                        UserInfo user = os.GetUserInfoByLoginName(id);
                                        perList.Add(new Performer(user.EmployeeCode, user.Name));
                                    }
                                    break;
                                }
                            case "Role":
                                {
                                    List<UserEntity> users = rm.GetByRoleCode(outerid);
                                    foreach (UserEntity ur in users)
                                    {
                                        //UserInfo user = os.GetUserInfoByLoginName(ur.ID.ToString());
                                        //perList.Add(new Performer(user.EmployeeCode, user.Name));
                                        perList.Add(new Performer(ur.ID, ur.UserName));
                                    }
                                    break;
                                }
                            case "OC":
                                {
                                    var ocUsers = os.GetOCTypeParticipants(int.Parse(outerid), flowInitator.UserID, runner.UserID);
                                    foreach (var item in ocUsers)
                                    {
                                        perList.Add(new Performer(item.EmployeeCode.ToString(), item.Name));
                                    }
                                    break;
                                }
                            case "Var":
                                {
                                    if (runner.Conditions.ContainsKey(outerid))
                                    {
                                        foreach (string userid in runner.Conditions[outerid].Split(';'))
                                        {
                                            UserInfo user = os.GetUserInfoByLoginName(userid);
                                            perList.Add(new Performer(user.EmployeeCode, user.Name));
                                        }
                                    }
                                    break;
                                }
                        }
                    }
                    runner.NextActivityPerformers.Add(node.ActivityGUID, perList);

                }
                WfExecutedResult runAppResult = RunProcessApp(runner);
                if (runAppResult.Status != WfExecutedStatus.Success)
                {
                    return "fail";
                }
                return "success";
            }
            else return "fail";

        }


        /// <summary>
        /// 撤销流程申请操作
        /// </summary>
        /// <param name="initiator"></param>
        /// <returns></returns>
        public string CancelApplication(WfAppRunner initiator)
        {
            initiator.UserName = "system";
            initiator.UserID = "system";
            if (DiscardProcess(initiator))
            {
                return "success";
            }
            else
            {
                return "撤销流程失败！";
            }
        }


        #endregion

    }
}