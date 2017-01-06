using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ClinBrain.Data.Service
{
    /// <summary>
    /// BPM显示数据服务
    /// </summary>
    public class BPMManager : ManagerBase
    {
        public List<TodoViewModel> GetMyTodoList(Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            string commonSql = string.Format("FROM vwWfActivityInstanceTasks WHERE ActivityType=4 AND ActivityState=1 AND AssignedToUserID='{0}'  AND ProcessState=2 ", para["currentUserId"]);
            string querySql = "select applicantName, applyTime, AppName, AppInstanceID, CreatedDateTime, ActivityName, summary, ProcessInstanceID, ProcessGUID, TaskID, pageUrl " + commonSql + " order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<TodoViewModel>(querySql).ToList();
        }

        public int GetMyTodoCount(string userId)
        {
            string sql = string.Format("select cast(count(1) as varchar(8)) FROM vwWfActivityInstanceTasks WHERE ActivityType=4 AND ActivityState=1 AND AssignedToUserID='{0}'  AND ProcessState=2 ", userId);
            int total = int.Parse(Repository.Query<string>(sql).ToList()[0]);
            return total;
        }
        public List<DoneViewModel> GetMyDoneList(Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            string commonSql = string.Format("FROM vwWfActivityInstanceTasks WHERE ActivityType=4 AND ActivityState=4 AND AssignedToUserID='{0}'  AND DATEDIFF(s,CreatedDateTime,EndedDateTime) >5 and viewPageUrl is not null ", para["currentUserId"]);
            string querySql = "select applicantName, applyTime, AppName, CreatedDateTime, EndedDateTime, ActivityName, summary, AppInstanceID, viewPageUrl " + commonSql + " order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<DoneViewModel>(querySql).ToList();
        }

        public List<ApplicationViewModel> GetMyApplicationList(Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            string commonSql = string.Format("FROM vwWfActivityInstanceTasks WHERE applicantID='{0}' group by AppInstanceID ", para["currentUserId"]);
            string querySql = "select min(CreatedDateTime) CreatedDateTime , min(AppName) AppName, min(AppInstanceID) AppInstanceID, max(ProcessState) ProcessState, max(Summary) summary, max(viewPageUrl) viewPageUrl ";

            string totalSql = "select cast(count(1) as varchar(8)) from (" + querySql + commonSql + ")t";

            querySql+= commonSql + " order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<ApplicationViewModel>(querySql).ToList();
        }
    }
    public class TodoViewModel
    {
        public string applicantName { get; set; }
        public DateTime applyTime { get; set; }
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }  //单号
        public DateTime CreatedDateTime { get; set; }  //任务到达时间
        public string ActivityName { get; set; } //任务名称
        public string summary { get; set; }
        public string pageUrl { get; set; }
        public string ProcessInstanceID { get; set; }
        public string ProcessGUID { get; set; }
        public string TaskID { get; set; }
    }

    public class DoneViewModel
    {
        public string applicantName { get; set; }
        public DateTime applyTime { get; set; }
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }  //单号
        public DateTime CreatedDateTime { get; set; }  //任务到达时间
        public DateTime EndedDateTime { get; set; }  //任务审批时间
        public string ActivityName { get; set; } //任务名称
        public string summary { get; set; }
        public string viewPageUrl { get; set; }
    }

    public class ApplicationViewModel
    {
        public DateTime CreatedDateTime { get; set; }
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }  //单号
        public string ProcessState { get; set; }
        public string summary { get; set; }
        public string viewPageUrl { get; set; }
    }
}