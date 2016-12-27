using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinBrain.Data;
using ClinBrain.WorkFlowEngine.Business.Entity;
using ClinBrain.WorkFlowEngine.Xpdl;
using System.Data;

namespace ClinBrain.WorkFlowEngine.Business.Manager
{
    /// <summary>
    /// 审批记录管理类
    /// </summary>
    public class ApproveInfoManager : ManagerBase
    {

        /// <summary>
        /// 获取所有审批日志
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="wfLinqDataContext"></param>
        /// <returns></returns>
        public List<ApproveInfo> GetAllList(string appNo)
        {
            var strSQL = @"SELECT 
                                *
                           FROM WfApproveInfo
                           where AppNo=@appNo
                           ORDER BY ID desc";
            List<ApproveInfo> list = Repository.Query<ApproveInfo>(strSQL, new { appNo = appNo }).ToList();
            if (list != null && list.Count > 0)
                return list;
            else return null;
        }

        /// <summary>
        /// 插入一条审批记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Insert(ApproveInfo entity)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Insert<ApproveInfo>(session.Connection, entity, session.Transaction);
                session.Commit();
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        public ApproveInfo GetActiveStep(string appNo)
        {
            var strSQL = @"SELECT top 1
                           ActivityName, AssignedToUserName,AssignedToUserID
                           FROM WfTasks
                           where TaskState =1 and AppInstanceID='" + appNo + "'";
            DataSet ds = Repository.Query(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ApproveInfo apif = new ApproveInfo { StepName = ds.Tables[0].Rows[0][0].ToString(), ApproverName = ds.Tables[0].Rows[0][1].ToString() + "(" + ds.Tables[0].Rows[0][2].ToString() + ")" };
                return apif;
            }
            else return null;

        }

        public List<ApproveInfo> GetActiveSteps(string appNo)
        {
            List<ApproveInfo> result = new List<ApproveInfo>();
            var strSQL = @"SELECT 
                           ActivityName, AssignedToUserName,AssignedToUserID
                           FROM WfTasks
                           where TaskState =1 and AppInstanceID='" + appNo + "'";
            DataSet ds = Repository.Query(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    result.Add(new ApproveInfo { StepName = dr[0].ToString(), ApproverName = dr[1].ToString(), ApproverId = dr[2].ToString(), AppNo="active"  });
                }
                return result;
            }
            else return null;
        }

    }
}