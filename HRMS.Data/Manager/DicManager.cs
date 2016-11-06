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
    public class DicManager : ManagerBase
    {
        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Insert(DicEntity entity)
        {
            entity.iCreatedOn = DateTime.Now;
            entity.iUpdatedOn = DateTime.Now;
            entity.iStatus = 1;
            entity.iIsDeleted = 0;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Insert<DicEntity>(session.Connection, entity, session.Transaction);
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

        public void Update(DicEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<DicEntity>(session.Connection, entity, session.Transaction);
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
        public DicEntity GetDic(string id)
        {
            string sql = @"select * from SysDic where iid=@id and iIsDeleted =0 and iStatus =1";
            return Repository.Query<DicEntity>(sql, new { id = id }).FirstOrDefault();
        }

        public List<DicEntity> GetDicByType(string type)
        {
            string sql = @"select * from SysDic where itype=@type and iIsDeleted =0 and iStatus =1";
            return Repository.Query<DicEntity>(sql, new { type = type }).ToList();
        }

        public List<DicEntity> GetSearch(Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            StringBuilder commandsb = new StringBuilder("from SysDic where iIsDeleted =0 and iStatus =1 ");

            foreach (KeyValuePair<string, string> item in para)
            {
                if (!string.IsNullOrEmpty(item.Value) && item.Value != "§")
                {
                    if (item.Key == "search")
                    {
                        commandsb.Append(" and iValue like '%" + item.Value + "%'");
                    }
                    else
                    {
                        commandsb.Append(" and " + item.Key + " like '%" + item.Value + "%'");
                    }
                }
            }

            string commonSql = commandsb.ToString();
            string querySql = "select * " + commonSql + "order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<DicEntity>(querySql).ToList();

        }
    }
}