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
    /// 用户管理类
    /// </summary>
    public class UserManager : ManagerBase
    {
        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Insert(UserEntity entity)
        {
            entity.iCreatedOn = DateTime.Now;
            entity.iUpdatedOn = DateTime.Now;
            entity.iStatus = 1;
            entity.iIsDeleted = 0;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Insert<UserEntity>(session.Connection, entity, session.Transaction);
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

        public void Update(UserEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            entity.iStatus = 1;
            entity.iIsDeleted = 0;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<UserEntity>(session.Connection, entity, session.Transaction);
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

        public UserEntity GetUser(string employeeCode)
        {
            string sql = @"select * from SysUser where iemployeecodeId=@empcode and iIsDeleted =0 and iStatus =1";
            return Repository.Query<UserEntity>(sql, new { empcode = employeeCode }).FirstOrDefault();
        }

        public List<UserEntity> GetSearch(string keyString, string sort, string order, int offset, int pageSize, out int total)
        {
            string commonSql = "from SysUser where iIsDeleted =0 and iStatus =1 and iUserName like '%{0}%' ";
            string querySql = "select * " + commonSql + "order by {1} {2} offset {3} row fetch next {4} rows only";
            querySql = string.Format(querySql, keyString, sort, order, offset, pageSize);
            string totalSql = string.Format("select cast(count(1) as varchar(8)) " + commonSql, keyString);
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<UserEntity>(querySql).ToList();

        }
    }
}