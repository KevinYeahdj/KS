using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClinBrain.Data.Entity;

namespace HRMS.Data.Manager
{
    /// <summary>
    /// 人力管理类
    /// </summary>
    public class LaborInfoManager : ManagerBase
    {
        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Insert(LaborInfoEntity entity)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Insert<LaborInfoEntity>(session.Connection, entity, session.Transaction);
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


        public List<LaborInfoEntity> GetSearch(string companyCode, string keyString, string sort, string order, int offset, int pageSize, out int total)
        {
            string commonSql = "from LaborInfo where icompanycode='{0}' and iname like '%{1}%' ";
            string querySql = "select * " + commonSql + "order by {2} {3} offset {4} row fetch next {5} rows only";
            querySql = string.Format(querySql, companyCode, keyString, sort, order, offset, pageSize);
            string totalSql = string.Format("select cast(count(1) as varchar(8)) " + commonSql, companyCode, keyString);
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<LaborInfoEntity>(querySql).ToList();

        }
    }
}