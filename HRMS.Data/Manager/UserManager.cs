using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClinBrain.Data.Entity;

namespace HRMS.Data.Manager
{
    /* 旧的代码
    /// <summary>
    /// 用户管理类
    /// </summary>
    public class User_oldManager : ManagerBase
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
            var user = Repository.Query<UserEntity>(sql, new { empcode = employeeCode }).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            if (user.iUserType == "普通用户")  //为普通用户指定默认所在项目，目前普通用户项目存于 companycode, 超级用户及管理员都是-
            {
                HRMS.Data.Service.ManageService ms = new HRMS.Data.Service.ManageService();
                user.iCompanyCode = ms.GetUserDefaultProject(employeeCode);
            }
            else
            {
                user.iCompanyCode = "-";
            }
            return user;
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
    */

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
            var user = Repository.Query<UserEntity>(sql, new { empcode = employeeCode }).FirstOrDefault();
            return user;
        }

        public bool CheckUserValid(string iEmployeeCodeId)
        {
            var result = Repository.Query<ProjectEntity>("select * from SysUser where iIsdeleted = 0 and iStatus = 1 and iEmployeeCodeId =@iempcode", new { iempcode = iEmployeeCodeId });
            if (result != null && result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public List<UserEntity> GetSearch(string keyString, string sort, string order, int offset, int pageSize, out int total)
        {
            string commonSql = "from SysUser where iIsDeleted =0 and iStatus =1 ";
            if (!string.IsNullOrEmpty(keyString))
            {
                commonSql = "from SysUser where iIsDeleted =0 and iStatus =1 and (iUserName like '%" + keyString + "%' or iEmployeeCodeId like'%" + keyString + "%') ";
            }
            string querySql = "select * " + commonSql + "order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<UserEntity>(querySql).ToList();

        }
    }
}