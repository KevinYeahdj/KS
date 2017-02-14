

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ClinBrain.Data;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.WorkFlowEngine.Utility;
using ClinBrain.WorkFlowEngine.Xpdl;
using ClinBrain.WorkFlowEngine.Xpdl.Node;
using ClinBrain.WorkFlowEngine.Business.Entity;
using ClinBrain.WorkFlowEngine.Business.Manager;
using ClinBrain.WorkFlowEngine.Core.Result;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Threading;
using System.IO;

namespace ClinBrain.WorkFlowEngine.Core.Pattern
{
    /// <summary>
    /// 节点执行器的抽象类
    /// </summary>
    internal abstract class NodeMediator
    {
        #region 属性列表
        internal ProcessModel _processModel;
        internal ProcessModel ProcessModel
        {
            get
            {
                return _processModel;
            }
        }

        private ActivityForwardContext _activityForwardContext;
        protected ActivityForwardContext ActivityForwardContext
        {
            get
            {
                return _activityForwardContext;
            }
        }

        private BackwardContext _backwardContext;
        protected BackwardContext BackwardContext
        {
            get
            {
                return _backwardContext;
            }
        }

        private IDbSession _session;
        protected IDbSession Session
        {
            get
            {
                return _session;
            }
        }

        private Linker _linker;
        internal Linker Linker
        {
            get
            {
                if (_linker == null)
                    _linker = new Linker();

                return _linker;
            }
        }

        /// <summary>
        /// 活动节点实例管理对象
        /// </summary>
        private ActivityInstanceManager activityInstanceManager;
        internal ActivityInstanceManager ActivityInstanceManager
        {
            get
            {
                if (activityInstanceManager == null)
                {
                    activityInstanceManager = new ActivityInstanceManager();
                }
                return activityInstanceManager;
            }
        }

        private TaskManager taskManager;
        internal TaskManager TaskManager
        {
            get
            {
                if (taskManager == null)
                {
                    taskManager = new TaskManager();
                }
                return taskManager;
            }
        }

        private ProcessInstanceManager processInstanceManager;
        internal ProcessInstanceManager ProcessInstanceManager
        {
            get
            {
                if (processInstanceManager == null)
                    processInstanceManager = new ProcessInstanceManager();
                return processInstanceManager;
            }
        }

        private WfNodeMediatedResult _wfNodeMediatedResult;
        internal WfNodeMediatedResult WfNodeMediatedResult
        {
            get
            {
                if (_wfNodeMediatedResult == null)
                {
                    _wfNodeMediatedResult = new WfNodeMediatedResult();
                }
                return _wfNodeMediatedResult;
            }
        }

        #endregion

        #region 抽象方法列表
        /// <summary>
        /// 执行节点方法
        /// </summary>
        internal abstract void ExecuteWorkItem();
        #endregion
        /// <summary>
        /// 向前流转时的NodeMediator的构造函数
        /// </summary>
        /// <param name="forwardContext"></param>
        /// <param name="session"></param>
        internal NodeMediator(ActivityForwardContext forwardContext, IDbSession session)
        {
            _activityForwardContext = forwardContext;
            _session = session;
            _processModel = forwardContext.ProcessModel;
            Linker.FromActivity = forwardContext.Activity;
        }

        /// <summary>
        /// 退回处理时的NodeMediator的构造函数
        /// </summary>
        /// <param name="backwardContext"></param>
        /// <param name="session"></param>
        internal NodeMediator(BackwardContext backwardContext, IDbSession session)
        {
            _session = session;
            _backwardContext = backwardContext;
            Linker.FromActivity = backwardContext.BackwardFromActivity;
        }

        internal NodeMediator(IDbSession session)
        {
            _session = session;
        }

        #region 流程执行逻辑
        /// <summary>
        /// 遍历执行当前节点后面的节点
        /// </summary>
        /// <param name="previousActivityInstance"></param>
        /// <param name="currentNode"></param>
        internal virtual void ContinueForwardCurrentNode(bool isJumpforward)
        {
            var activityResource = ActivityForwardContext.ActivityResource;

            if (isJumpforward == false)
            {
                //非跳转模式的正常运行
                var nextActivityMatchedResult = this.ProcessModel.GetNextActivityList(this.Linker.FromActivityInstance.ActivityGUID,
                    activityResource.ConditionKeyValuePair,
                    activityResource,
                    (a, b) => a.NextActivityPerformers.ContainsKey(b.ActivityGUID));

                if (nextActivityMatchedResult.MatchedType != NextActivityMatchedType.Successed
                    || nextActivityMatchedResult.Root.HasChildren == false)
                {
                    throw new WfRuntimeException("没有匹配的后续流转节点，流程虽然能处理当前节点，但是无法流转到下一步！");
                }

                ContinueForwardCurrentNodeRecurisivly(this.Linker.FromActivity,
                    this.Linker.FromActivityInstance,
                    nextActivityMatchedResult.Root,
                    activityResource.ConditionKeyValuePair,
                    isJumpforward);
            }
            else
            {
                //跳转模式运行
                var root = NextActivityComponentFactory.CreateNextActivityComponent();
                var nextActivityComponent = NextActivityComponentFactory.CreateNextActivityComponent(
                    this.Linker.FromActivity,
                    this.Linker.ToActivity);

                root.Add(nextActivityComponent);

                ContinueForwardCurrentNodeRecurisivly(this.Linker.FromActivity,
                    this.Linker.FromActivityInstance,
                    root,
                    activityResource.ConditionKeyValuePair,
                    isJumpforward);
            }
        }

        /// <summary>
        /// 递归执行节点
        /// </summary>
        /// <param name="root"></param>
        /// <param name="fromNode"></param>
        /// <param name="conditionKeyValuePair"></param>
        protected void ContinueForwardCurrentNodeRecurisivly(ActivityEntity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            NextActivityComponent root,
            IDictionary<string, string> conditionKeyValuePair,
            Boolean isJumpforward)
        {
            foreach (NextActivityComponent comp in root)
            {
                if (comp.HasChildren)
                {
                    //此节点类型为分支或合并节点类型：首先需要实例化当前节点(自动完成)
                    NodeMediatorGateway gatewayNodeMediator = NodeMediatorGatewayFactory.CreateGatewayNodeMediator(comp.Activity,
                        ProcessModel, Session);

                    ICompleteAutomaticlly autoGateway = (ICompleteAutomaticlly)gatewayNodeMediator;
                    GatewayExecutedResult gatewayResult = autoGateway.CompleteAutomaticlly(
                        ActivityForwardContext.ProcessInstance,
                        comp.Transition.TransitionGUID,
                        fromActivityInstance,
                        ActivityForwardContext.ActivityResource,
                        Session);


                    //原if (gatewayResult.Status == GatewayExecutedStatus.Successed)  更新是为了让andjoin合并节点正常走下去
                    if (gatewayResult.Status == GatewayExecutedStatus.Successed && gatewayNodeMediator.GatewayActivityInstance.ActivityState == (short)ActivityStateEnum.Completed)
                    {
                        //遍历后续子节点
                        ContinueForwardCurrentNodeRecurisivly(fromActivity,
                            gatewayNodeMediator.GatewayActivityInstance,
                            comp,
                            conditionKeyValuePair,
                            isJumpforward);
                    }
                    else
                    {
                        WfRuntimeException e = new WfRuntimeException("第一个满足条件的节点已经被成功执行，此后的节点被阻止在XOrJoin节点!");
                        LogManager.RecordLog("递归执行节点方法异常", LogEventType.Exception, LogPriority.Normal, null, e);
                    }
                }
                else if (comp.Activity.IsSimpleWorkItem)
                {
                    //此节点类型为任务节点：根据fromActivityInstance的类型判断是否可以创建任务
                    if (fromActivityInstance.ActivityState == (short)ActivityStateEnum.Completed)
                    {
                        //创建新任务节点
                        NodeMediator taskNodeMediator = new NodeMediatorTask(Session);
                        taskNodeMediator.CreateActivityTaskTransitionInstance(comp.Activity,
                            ActivityForwardContext.ProcessInstance,
                            fromActivityInstance,
                            comp.Transition.TransitionGUID,
                            comp.Transition.DirectionType == TransitionDirectionTypeEnum.Loop ?
                                TransitionTypeEnum.Loop : TransitionTypeEnum.Forward, //根据Direction方向确定是否是自身循环
                            isJumpforward == true ?
                                TransitionFlyingTypeEnum.ForwardFlying : TransitionFlyingTypeEnum.NotFlying,
                            ActivityForwardContext.ActivityResource,
                            Session);
                    }
                    else
                    {
                        //下一步的任务节点没有创建，需给出提示信息
                        if ((fromActivity.GatewayDirectionType | GatewayDirectionEnum.AllJoinType)
                            == GatewayDirectionEnum.AllJoinType)
                        {
                            WfRuntimeException e = new WfRuntimeException("等待其它需要合并的分支!");
                            LogManager.RecordLog("递归执行节点方法异常", LogEventType.Exception, LogPriority.Normal, null, e);
                        }
                    }
                }
                else if (comp.Activity.ActivityType == ActivityTypeEnum.SubProcessNode)
                {
                    //节点类型为subprocessnode
                    if (fromActivityInstance.ActivityState == (short)ActivityStateEnum.Completed)
                    {
                        //实例化subprocess节点数据
                        NodeMediator subNodeMediator = new NodeMediatorSubProcess(Session);
                        subNodeMediator.CreateActivityTaskTransitionInstance(comp.Activity,
                            ActivityForwardContext.ProcessInstance,
                            fromActivityInstance,
                            comp.Transition.TransitionGUID,
                            comp.Transition.DirectionType == TransitionDirectionTypeEnum.Loop ?
                                TransitionTypeEnum.Loop : TransitionTypeEnum.Forward,
                            TransitionFlyingTypeEnum.NotFlying,
                            ActivityForwardContext.ActivityResource,
                            Session);
                    }
                }
                else if (comp.Activity.ActivityType == ActivityTypeEnum.EndNode)
                {
                    //此节点为完成结束节点，结束流程
                    var endMediator = new NodeMediatorEnd(ActivityForwardContext, Session);
                    endMediator.Linker.ToActivity = comp.Activity;
                    endMediator.CompleteAutomaticlly(ActivityForwardContext.ProcessInstance,
                        comp.Transition.TransitionGUID,
                        fromActivityInstance,
                        ActivityForwardContext.ActivityResource,
                        Session);
                }
                else
                {
                    WfRuntimeException e = new WfRuntimeException(string.Format("XML文件定义了未知的节点类型，执行失败，节点类型信息：{0}",
                        comp.Activity.ActivityType.ToString()));
                    LogManager.RecordLog("递归执行节点方法异常", LogEventType.Exception, LogPriority.Normal, null, e);
                }
            }
        }

        /// <summary>
        /// 创建工作项及转移数据
        /// </summary>
        /// <param name="toActivity"></param>
        /// <param name="processInstance"></param>
        /// <param name="fromActivityInstance"></param>
        /// <param name="transitionGUID"></param>
        /// <param name="transitionType"></param>
        /// <param name="flyingType"></param>
        /// <param name="activityResource"></param>
        /// <param name="session"></param>
        internal virtual void CreateActivityTaskTransitionInstance(ActivityEntity toActivity,
            ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            String transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session) { }

        /// <summary>
        /// 创建任务的虚方法
        /// 1. 对于自动执行的工作项，无需重写该方法
        /// 2. 对于人工执行的工作项，需要重写该方法，插入待办的任务数据
        /// </summary>
        /// <param name="activityResource"></param>
        /// <param name="wfLinqDataContext"></param>
        internal virtual void CreateNewTask(ActivityInstanceEntity toActivityInstance,
            ActivityResource activityResource,
            IDbSession session)
        {
            if (activityResource.NextActivityPerformers == null)
            {
                throw new WorkflowException("无法创建任务，流程流转下一步的办理人员不能为空！");
            }

            TaskManager.Insert(toActivityInstance,
                activityResource.NextActivityPerformers[toActivityInstance.ActivityGUID],
                activityResource.AppRunner,
                session);
        }

        /// <summary>
        /// 创建节点对象
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        protected ActivityInstanceEntity CreateActivityInstanceObject(ActivityEntity activity,
            ProcessInstanceEntity processInstance,
            WfAppRunner runner)
        {
            ActivityInstanceEntity entity = ActivityInstanceManager.CreateActivityInstanceObject(processInstance.AppName,
                processInstance.AppInstanceID,
                processInstance.ID,
                activity,
                runner);

            return entity;
        }

        /// <summary>
        /// 创建退回类型的活动实例对象
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="backSrcActivityInstanceID">退回的活动实例ID</param>
        /// <param name="logonUser">登录用户</param>
        /// <returns></returns>
        protected ActivityInstanceEntity CreateBackwardToActivityInstanceObject(ProcessInstanceEntity processInstance,
            BackwardTypeEnum backwardType,
            int backSrcActivityInstanceID,
            WfAppRunner runner)
        {
            ActivityInstanceEntity entity = ActivityInstanceManager.CreateBackwardActivityInstanceObject(
                processInstance.AppName,
                processInstance.AppInstanceID,
                processInstance.ID,
                this.BackwardContext.BackwardToTaskActivity,
                backwardType,
                backSrcActivityInstanceID,
                runner);

            return entity;
        }

        /// <summary>
        /// 插入连线实例的方法
        /// </summary>
        /// <param name="processInstance"></param>
        /// <param name="fromToTransition"></param>
        /// <param name="fromActivityInstance"></param>
        /// <param name="toActivityInstance"></param>
        /// <param name="conditionParseResult"></param>
        /// <param name="wfLinqDataContext"></param>
        /// <returns></returns>
        internal virtual void InsertTransitionInstance(ProcessInstanceEntity processInstance,
            String transitionGUID,
            ActivityInstanceEntity fromActivityInstance,
            ActivityInstanceEntity toActivityInstance,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            WfAppRunner runner,
            IDbSession session)
        {
            var tim = new TransitionInstanceManager();
            var transitionInstanceObject = tim.CreateTransitionInstanceObject(processInstance,
                transitionGUID,
                fromActivityInstance,
                toActivityInstance,
                transitionType,
                flyingType,
                runner,
                (byte)ConditionParseResultEnum.Passed);

            tim.Insert(session.Connection, transitionInstanceObject, session.Transaction);

            //此处为插入流程跳转记录，即可以在记录跳转的时候执行跳转起点的结点完成事件
            //从配置中读取结点完成事件执行内容，结合流程变量执行sql或服务
            //Kevin 0630
            #region
            var processModel = new ProcessModel(processInstance.ProcessGUID);
            string[] eventString = processModel.GetAfterTransitionAction(transitionGUID);
            if (eventString != null)
            {
                //ProcessInstanceConditionEntity condition = ProcessInstanceManager.GetProcessInstanceCondition(processInstance.ID);
                //Thread newThread = new Thread(() => DoEvent(eventString[0], eventString[1], condition.ConditionsJson));
                Thread newThread = new Thread(() => DoEvent(eventString[0], eventString[1], runner.Conditions));
                newThread.IsBackground = true;
                newThread.Start();
            }

            #endregion




        }
        private void DoEventOld(string connstring, string sql, string conditionJson)
        {
            var activityResource = ActivityForwardContext.ActivityResource;
            var it = activityResource.ConditionKeyValuePair;
            IDictionary<string, string> conditions = (IDictionary<string, string>)JsonConvert.DeserializeObject(conditionJson, typeof(IDictionary<string, string>));
            foreach (var item in conditions)
            {
                sql = sql.Replace("{" + item.Key + "}", item.Value);
            }

            using (SqlConnection connection = new SqlConnection(connstring))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();
                    connection.Close();

                }
                catch (Exception ex)
                {
                    LogManager.RecordLog("执行流程结束事件方法异常", LogEventType.Exception, LogPriority.Normal, null, ex);
                }
            }

        }

        private void DoEvent(string connstring, string sql, IDictionary<string, string> conditions)
        {
            try
            {
                if (sql.StartsWith("$"))
                {
                    List<string> configs = sql.Split('$').ToList();
                    string assPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configs[1] + ".dll");
                    if (!File.Exists(assPath))
                        assPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", configs[1] + ".dll");
                    if (!File.Exists(assPath))
                        throw new Exception(string.Format("找不到{0}文件", assPath));
                    //获取程序集
                    var assembly = Assembly.LoadFrom(assPath);
                    //获取程序集中对应实体
                    var objectType = assembly.GetType(configs[2], false);
                    var objInstance = Activator.CreateInstance(objectType, true);
                    var T = objInstance.GetType();
                    var mi = T.GetMethod(configs[3]);
                    mi.Invoke(objInstance, new object[] { conditions });
                    return;
                }

                connstring = "Data Source=.;Initial Catalog=HRMS;Integrated Security=True;";

                if (conditions != null)
                {
                    foreach (var item in conditions)
                    {
                        sql = sql.Replace("{" + item.Key + "}", item.Value);
                    }
                }

                using (SqlConnection connection = new SqlConnection(connstring))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                LogManager.RecordLog("执行流程结束事件方法异常", LogEventType.Exception, LogPriority.Normal, null, ex);
                LogFileHelper.ErrorLog("执行流程结束事件方法异常" + ex.ToString());
            }
        }

        /// <summary>
        /// 生成任务办理人字符串列表
        /// </summary>
        /// <param name="performerList"></param>
        /// <returns></returns>
        protected string GenerateActivityAssignedUsers(PerformerList performerList)
        {
            StringBuilder strBuilder = new StringBuilder(1024);
            foreach (var performer in performerList)
            {
                if (strBuilder.ToString() != "")
                    strBuilder.Append(",");
                strBuilder.Append(performer.UserID);
            }
            return strBuilder.ToString();
        }

        #endregion

        /// <summary>
        /// 根据节点执行结果类型，生成消息
        /// </summary>
        /// <returns></returns>
        internal string GetNodeMediatedMessage()
        {
            var message = string.Empty;
            if (WfNodeMediatedResult.Feedback == WfNodeMediatedFeedback.ForwardToNextSequenceTask)
            {
                message = "串行会签，设置下一个执行节点的任务进入运行状态！";
            }
            else if (WfNodeMediatedResult.Feedback == WfNodeMediatedFeedback.WaitingForCompletedMore)
            {
                message = "并行会签，等待节点到达足够多的完成比例！";
            }

            return message;
        }
    }
}
