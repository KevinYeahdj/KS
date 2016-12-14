using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.Data;

namespace HRMS.Data.Service
{
    public class ManageService
    {
        public Dictionary<string, string> GetUserProjects(string userCode)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string sql = "SELECT distinct n.ikey,n.ivalue FROM [dbo].[SysUserMenu] m inner join dbo.SysDic n on n.itype='项目' and m.iprojectcode = n.iKey and m.iEmployeeCode = '" + userCode + "'";
            DataSet ds = DbHelperSQL.Query(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                result.Add(dr[0].ToString(), dr[1].ToString());
            }
            return result;
        }

        public string GetUserDefaultProject(string userCode)
        {
            string sql = "SELECT top 1 n.ikey FROM [dbo].[SysUserMenu] m inner join dbo.SysDic n on n.itype='项目' and m.iprojectcode = n.iKey and m.iEmployeeCode = '" + userCode + "'";
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0].Rows.Count == 0)
                return "-";
            else return ds.Tables[0].Rows[0][0].ToString();
        }
    }
}
