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
    /// 类型管理类
    /// </summary>
    public class ModifyLogManager : ManagerBase
    {
        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Insert(ModifyLogEntity entity)
        {
            entity.iId = Guid.NewGuid().ToString();
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Insert<ModifyLogEntity>(session.Connection, entity, session.Transaction);
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


        public List<ModifyLogEntity> GetSearch(string keyString, string sort, string order, int offset, int pageSize, out int total)
        {
            string commonSql = "from SysModifyLog where iTableName='HRInfo' and iId='{0}' ";
            string querySql = "select * " + commonSql + "order by {1} {2} offset {3} row fetch next {4} rows only";
            querySql = string.Format(querySql, keyString, sort, order, offset, pageSize);
            string totalSql = string.Format("select cast(count(1) as varchar(8)) " + commonSql, keyString);
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<ModifyLogEntity>(querySql).ToList();

        }
    }
}