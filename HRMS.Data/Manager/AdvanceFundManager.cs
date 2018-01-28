using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HRMS.Data.Entity;
using HRMS.Common;
using Dapper;

namespace HRMS.Data.Manager
{
    /// <summary>
    /// 备用金信息管理类
    /// </summary>
    public class AdvanceFundManager : ManagerBase
    {
        //建表dic

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Insert(AdvanceFundEntity entity)
        {
            if (string.IsNullOrEmpty(entity.iGuid))
                entity.iGuid = Guid.NewGuid().ToString();
            entity.iCreatedOn = DateTime.Now;
            entity.iUpdatedOn = DateTime.Now;
            entity.iIsDeleted = 0;
            entity.iStatus = 1;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Insert<AdvanceFundEntity>(session.Connection, entity, session.Transaction);
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

        public void Update(AdvanceFundEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<AdvanceFundEntity>(session.Connection, entity, session.Transaction);
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

        public AdvanceFundEntity FirstOrDefault(string appno)
        {
            string sql = @"select * from AdvanceFund where iAppno=@appno and iIsDeleted =0 and iStatus =1";
            return Repository.Query<AdvanceFundEntity>(sql, new { appno = appno }).FirstOrDefault();
        }

        public List<AdvanceFundEntity> GetUndoBillByUser(string applicant)
        {
            string queryLast = "select top 1* from AdvanceFund where iPaidDate is not null and iApplicant = '" + applicant + "' and iIsDeleted = 0 and iStatus =1 order by iPaidDate desc";
            List<AdvanceFundEntity> last = Repository.Query<AdvanceFundEntity>(queryLast).ToList();
            DateTime dtm = DateTime.Parse("2018/01/01 12:00:00");
            if (last.Count == 1)
                dtm = (DateTime)last[1].iPaidDate;
            string querySql = "select * from AdvanceFund where iPaidDate is not null and iPaidDate>@dtm and  iApplicant = '" + applicant + "' and iIsDeleted = 0 and iStatus =1 order by iPaidDate desc";
            DynamicParameters pars = new DynamicParameters();
            pars.Add("dtm", dtm, System.Data.DbType.DateTime);
            return Repository.Query<AdvanceFundEntity>(querySql, pars).ToList();
        }

    }
}