using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HRMS.Data.Entity;

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


        public List<ModifyLogEntity> GetSearch(Dictionary<string,string> dic, string sort, string order, int offset, int pageSize, out int total)
        {
            string commonSql = "from SysModifyLog where iTableName='{0}' and iId='{1}' ";
            string querySql = "select * " + commonSql + "order by {2} {3} offset {4} row fetch next {5} rows only";
            querySql = string.Format(querySql, dic["tableName"], dic["iGuid"], sort, order, offset, pageSize);
            string totalSql = string.Format("select cast(count(1) as varchar(8)) " + commonSql, dic["tableName"], dic["iGuid"]);
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<ModifyLogEntity>(querySql).ToList();

        }
    }
}