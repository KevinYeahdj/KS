using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HRMS.Data.Entity;
using System.Data.SqlClient;
using System.Configuration;

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
                if (!string.IsNullOrEmpty(item.Value))
                {
                    if (item.Key == "search")
                    {
                        commandsb.Append(" and iValue like '%" + item.Value + "%'");
                    }
                    else if (item.Key == "iType")
                    {
                        commandsb.Append(" and iType ='" + item.Value + "'");
                    }
                    else if (item.Key == "iCompanyCode")
                    {
                        commandsb.Append(" and iCompanyCode like '%''" + item.Value + "''%'");
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

        //company
        public void Insert(CompanyEntity entity)
        {
            entity.iCreatedOn = DateTime.Now;
            entity.iUpdatedOn = DateTime.Now;
            entity.iStatus = 1;
            entity.iIsDeleted = 0;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Insert<CompanyEntity>(session.Connection, entity, session.Transaction);
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
        public void Update(CompanyEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<CompanyEntity>(session.Connection, entity, session.Transaction);
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
        public CompanyEntity CompanyFirstOrDefault(string iguid)
        {
            return Repository.GetById<CompanyEntity>(iguid);
        }
        public bool CheckCompanyValid(string companyName)
        {
            var result = Repository.Query<CompanyEntity>("select * from SysCompany where iIsdeleted = 0 and iStatus = 1 and iName =@iname", new { iname = companyName });
            if (result != null && result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public List<CompanyEntity> CompanyGetSearch(Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            string commonSql = string.Format(" from SysCompany where iIsDeleted =0 and iStatus =1 {0} ", string.IsNullOrEmpty(para["search"]) ? "" : " and iName like '%" + para["search"] + "%' ");
            string querySql = "select * " + commonSql + " order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<CompanyEntity>(querySql).ToList();
        }

        public List<CompanyEntity> GetAllCompanies()
        {
            return Repository.GetAll<CompanyEntity>().Where(i => i.iStatus == 1).ToList();
        }
        public List<CompanyEntity> GetAllValidCompanies()
        {
            return Repository.GetAll<CompanyEntity>().Where(i => i.iStatus == 1 && i.iIsDeleted == 0).ToList();
        }

        //project
        public void Insert(ProjectEntity entity)
        {
            entity.iCreatedOn = DateTime.Now;
            entity.iUpdatedOn = DateTime.Now;
            entity.iStatus = 1;
            entity.iIsDeleted = 0;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Insert<ProjectEntity>(session.Connection, entity, session.Transaction);
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
        public void Update(ProjectEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<ProjectEntity>(session.Connection, entity, session.Transaction);
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
        public ProjectEntity ProjectFirstOrDefault(string iguid)
        {
            return Repository.GetById<ProjectEntity>(iguid);
        }
        public bool CheckProjectValid(string projectName)
        {
            var result = Repository.Query<ProjectEntity>("select * from SysProject where iIsdeleted = 0 and iStatus = 1 and iName =@iname", new { iname = projectName });
            if (result != null && result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public List<ProjectEntity> GetAllProjects()
        {
            return Repository.GetAll<ProjectEntity>().Where(i => i.iStatus == 1).ToList();
        }

        public List<ProjectEntity> GetAllValidProjects()
        {
            return Repository.GetAll<ProjectEntity>().Where(i => i.iStatus == 1 && i.iIsDeleted == 0).ToList();
        }

        public List<ProjectEntity> ProjectGetSearch(Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            string commonSql = string.Format(" from SysProject where iIsDeleted =0 and iStatus =1 {0} ", string.IsNullOrEmpty(para["search"]) ? "" : " and iName like '%" + para["search"] + "%' ");
            string querySql = "select * " + commonSql + " order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<ProjectEntity>(querySql).ToList();
        }

        //companyProjectRelation
        public List<string> GetCompaniesOfProject(string projectId)
        {
            List<string> result = Repository.Query<string>("select iCompanyId from SysCompanyProjectRelation where iIsDeleted = 0 and iStatus = 1 and iProjectId ='" + projectId + "'").ToList();
            return result;
        }

        public int SaveCompanyProjectRelation(string projectId, List<string> companyIdList, string currentUserName)
        {
            int affectedRowCount = 0;
            List<CompanyProjectRelationEntity> list = new List<CompanyProjectRelationEntity>();
            foreach (string item in companyIdList)
            {
                list.Add(new CompanyProjectRelationEntity() { iGuid = Guid.NewGuid().ToString(), iProjectId = projectId, iCompanyId = item, iCreatedBy = currentUserName, iUpdatedBy = currentUserName, iIsDeleted = 0, iStatus = 1, iCreatedOn = DateTime.Now, iUpdatedOn = DateTime.Now });
            }
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HRMSDBConnectionString"].ConnectionString))
            {
                Repository.Execute(conn, "delete from SysCompanyProjectRelation where iProjectId ='" + projectId + "'");
                Repository.InsertBatch<CompanyProjectRelationEntity>(conn, list.ToArray(), null, "SysCompanyProjectRelation");
                affectedRowCount = list.Count();
            }
            return affectedRowCount;
        }

        //授权相关
        public Dictionary<string, string> GetAuthorisedCompanyDic(string empNo, string userType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (userType == "普通用户")
            {
                string querySql = "SELECT distinct ipid, iName FROM [SysUserCompanyProjectTree] a inner join [SysCompany] b on a.iPid= b.iGuid and b.iIsDeleted =0 and b.iStatus=1 where iEmployeeCodeId='" + empNo + "'";
                DataSet ds = DbHelperSQL.Query(querySql);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dic.Add(dr[0].ToString(), dr[1].ToString());
                }
            }
            else
            {
                dic = GetAllValidCompanies().ToDictionary(i => i.iGuid, i => i.iName);
            }
            return dic;
        }

        public Dictionary<string, string> GetAuthorisedProjectDic(string empNo, string userType, string companyId)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (userType == "普通用户")
            {
                string querySql = "SELECT distinct iGuid, iName FROM [SysUserCompanyProjectTree] a inner join [SysProject] b on REPLACE(a.iId,a.iPid+'|','')= b.iGuid and b.iIsDeleted =0 and b.iStatus=1 and a.iId<>a.iPid where iEmployeeCodeId='" + empNo + "' and a.iPid ='" + companyId + "'";
                DataSet ds = DbHelperSQL.Query(querySql);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dic.Add(dr[0].ToString(), dr[1].ToString());
                }
            }
            else
            {
                string querySql = "SELECT b.iguid,b.iname FROM [SysCompanyProjectRelation] a inner join [SysProject] b on a.iProjectId= b.iGuid and a.iStatus=1 and a.iIsDeleted=0 and b.iStatus=1 and b.iIsDeleted=0 and a.iCompanyId='" + companyId + "'";
                DataSet ds = DbHelperSQL.Query(querySql);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dic.Add(dr[0].ToString(), dr[1].ToString());
                }
            }
            return dic;
        }

        //流程人员相关
        public void InsertBpmUser(BpmUserEntity entity)
        {
            entity.iCreatedOn = DateTime.Now;
            entity.iUpdatedOn = DateTime.Now;
            entity.iStatus = 1;
            entity.iIsDeleted = 0;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Insert<BpmUserEntity>(session.Connection, entity, session.Transaction);
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
        public void UpdateBpmUser(BpmUserEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<BpmUserEntity>(session.Connection, entity, session.Transaction);
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
        public BpmUserEntity BpmUserFirstOrDefault(string iguid)
        {
            return Repository.GetById<BpmUserEntity>(iguid);
        }
        public List<BpmUserView> JournalBpmUserGetSearch(Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            string commonSql = @"from (
  select b.iGuid, a.iguid as icompanyId, a.iName as iCompanyName, '流水账申请' as iFlowSign, '确认' as iRoleName, b.iUsers, b.iNote, b.iUpdatedOn from [dbo].[SysCompany] a
  left join [dbo].[BpmUser] b
  on a.iguid = b.iCompanyId and b.iFlowSign='流水账申请' and b.iRoleName='确认'
  and b.iIsDeleted =0 and b.iStatus =1  where a.iisdeleted=0 and a.istatus=1
union all  
   select b.iGuid, a.iguid as icompanyId, a.iName as iCompanyName, '流水账申请' as iFlowSign, '经理' as iRoleName, b.iUsers, b.iNote , b.iUpdatedOn from [dbo].[SysCompany] a
  left join [dbo].[BpmUser] b
  on a.iguid = b.iCompanyId and b.iFlowSign='流水账申请' and b.iRoleName='经理'
  and b.iIsDeleted =0 and b.iStatus =1  where a.iisdeleted=0 and a.istatus=1
  union all  
   select b.iGuid, a.iguid as icompanyId, a.iName as iCompanyName, '流水账申请' as iFlowSign, '高管' as iRoleName, b.iUsers, b.iNote , b.iUpdatedOn from [dbo].[SysCompany] a
  left join [dbo].[BpmUser] b
  on a.iguid = b.iCompanyId and b.iFlowSign='流水账申请' and b.iRoleName='高管'
  and b.iIsDeleted =0 and b.iStatus =1  where a.iisdeleted=0 and a.istatus=1
union all  
   select b.iGuid, a.iguid as icompanyId, a.iName as iCompanyName, '流水账申请' as iFlowSign, '出纳' as iRoleName, b.iUsers, b.iNote , b.iUpdatedOn from [dbo].[SysCompany] a
  left join [dbo].[BpmUser] b
  on a.iguid = b.iCompanyId and b.iFlowSign='流水账申请' and b.iRoleName='出纳'
  and b.iIsDeleted =0 and b.iStatus =1  where a.iisdeleted=0 and a.istatus=1

) t";

            //          union all  
            // select b.iGuid, a.iguid as icompanyId, a.iName as iCompanyName, '流水账申请' as iFlowSign, '登记' as iRoleName, b.iUsers, b.iNote , b.iUpdatedOn from [dbo].[SysCompany] a
            //left join [dbo].[BpmUser] b
            //on a.iguid = b.iCompanyId and b.iFlowSign='流水账申请' and b.iRoleName='登记'
            //and b.iIsDeleted =0 and b.iStatus =1  where a.iisdeleted=0 and a.istatus=1

            string querySql = "select * " + commonSql + " order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<BpmUserView>(querySql).ToList();
        }

        public string GetUsersByFlowAndRole(string companyId, string flowSign, string roleName)
        {
            string sql = "select iUsers from [dbo].[BpmUser] where iisdeleted=0 and istatus=1 and iCompanyId='" + companyId + "' and iFlowSign='" + flowSign + "' and iRoleName='" + roleName + "'";
            List<string> result = Repository.Query<string>(sql).ToList();
            return (result == null || result.Count == 0) ? "sa" : result[0];
        }

        //流程公司角色人员相关
        public void InsertRole(RoleEntity entity)
        {
            entity.iCreatedOn = DateTime.Now;
            entity.iUpdatedOn = DateTime.Now;
            entity.iStatus = 1;
            entity.iIsDeleted = 0;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Insert<RoleEntity>(session.Connection, entity, session.Transaction);
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
        public void UpdateRole(RoleEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<RoleEntity>(session.Connection, entity, session.Transaction);
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
        public RoleEntity RoleFirstOrDefault(string iguid)
        {
            return Repository.GetById<RoleEntity>(iguid);
        }
        public bool CheckRoleValid(string name)
        {
            var result = Repository.Query<RoleEntity>("select * from SysRole where iIsdeleted = 0 and iStatus = 1 and iName =@iname", new { iname = name });
            if (result != null && result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public List<RoleEntity> RoleGetSearch(Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            string commonSql = string.Format(" from SysRole where iIsDeleted =0 and iStatus =1 {0} ", string.IsNullOrEmpty(para["search"]) ? "" : " and iName like '%" + para["search"] + "%' or iNote like '%" + para["search"] + "%' ");
            string querySql = "select * " + commonSql + " order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<RoleEntity>(querySql).ToList();
        }

        public List<RoleUsersEntity> GetRoleUsers(string roleid)
        {
            return Repository.Query<RoleUsersEntity>("select * from SysRoleUsers where iRoleGuid ='" + roleid + "'").ToList();
        }
    }
}