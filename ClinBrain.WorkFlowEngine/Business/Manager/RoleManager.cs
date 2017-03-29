using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinBrain.Data;
using ClinBrain.WorkFlowEngine.Business.Entity;

namespace ClinBrain.WorkFlowEngine.Business.Manager
{
    /// <summary>
    /// 角色管理类
    /// </summary>
    public class RoleManager : ManagerBase
    {
        /// <summary>
        /// 根据角色编码查询用户
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        public List<UserEntity> GetByRoleCode(string roleCode)
        {
            var strSQL = @"SELECT 
                                e.employee_code as ID, e.name as UserName
                           FROM dbo.HPM_LBR_EMPLOYEE e
                           INNER JOIN SysRoleUser RU
                                ON e.Employee_code = RU.UserID
                           INNER JOIN SysRole R
                                ON R.ID = RU.RoleID
                           WHERE R.ID = @roleCode
                           ORDER BY e.employee_code";
            List<UserEntity> list = Repository.Query<UserEntity>(strSQL, new { roleCode = roleCode }).ToList();
            return list;
        }

        public List<UserEntity> GetByRoleCode(string roleCode, string companyId)
        {
            var strSQL = "SELECT iUsers from [HRMS].[dbo].[SysRoleUsers] where iRoleGuid ='{0}' and iCompanyId='{1}' and istatus=1 and iisdeleted =0";
            DataSet ds = DbHelperSQL.Query(strSQL);
            List<string> ids = ds.Tables[0].Rows[0][0].ToString().Split(';').ToList();
            StringBuilder sb = new StringBuilder();
            foreach (string id in ids)
            {
                sb.Append("'" + id + "',");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 2, 1);
            }
            List<UserEntity> list = Repository.Query<UserEntity>("SELECT employee_code as ID, name as UserName FROM dbo.HPM_LBR_EMPLOYEE where  employee_code in(" + sb.ToString() + ")").ToList();
            return list;
        }

        /// <summary>
        /// 获取所有角色数据
        /// </summary>
        /// <returns></returns>
        public List<RoleEntity> GetAll()
        {
            var strSQL = @"SELECT 
                                R.*
                           FROM SysRole R
                           ORDER BY R.RoleName";
            List<RoleEntity> list = Repository.Query<RoleEntity>(strSQL, null).ToList();
            return list;
        }
    }
}