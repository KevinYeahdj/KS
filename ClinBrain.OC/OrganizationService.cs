using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ClinBrain.Data;

namespace ClinBrain.OC
{
    public class OrganizationService : ManagerBase , IOC 
    {
        //暂不使用域账号登录
        //private static string domian = System.Configuration.ConfigurationManager.AppSettings["domain"] + "/";

        private static string domian = "/";
        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoByLoginName(string loginName)
        {
            if (loginName == null)
            {
                return null;
            }
            //loginName = loginName.ToString().PadLeft(8, '0');
            loginName = loginName.Replace('\\', '/');
            string employeeCode = loginName.Replace(domian, string.Empty);
            employeeCode = employeeCode.Replace("CIMC/", string.Empty);
            loginName = "CIMC.COM/" + employeeCode;
            string sqlString = @"
            select e.*,u.unit_name,u.unit_id ,u.company_unit_id,u.company_unit_name,u.branch_unit_id,u.branch_unit_name
             from dbo.hpm_lbr_employee e 
             inner join dbo.hpm_org_unit u on e.unit_id=u.unit_id 
             where is_primary_position=1 and e.employee_code=@employee_code";
            SqlParameter[] selpara = {
                              new SqlParameter("@employee_code", SqlDbType.NVarChar)};
            selpara[0].Value = employeeCode;
            DataSet ds = Repository.Query(sqlString, selpara);
            if ((ds.Tables[0].Rows.Count > 0))
            {
                string companycode = ds.Tables[0].Rows[0].Field<Object>("company_unit_id").ToString();
                CompanyInfo companyInfo = GetCompanyInfoByCode(companycode);


                UserInfo userInfo =
                    new UserInfo
                    {
                        ID = ds.Tables[0].Rows[0]["used_name"].ToString().Trim(),
                        LoginName = loginName,
                        EmployeeCode = ds.Tables[0].Rows[0]["employee_code"].ToString().Trim(),
                        Name = ds.Tables[0].Rows[0]["Name"].ToString().Trim(),
                        DepartmentCode = ds.Tables[0].Rows[0]["unit_id"].ToString().Trim(),
                        DepartmentName = ds.Tables[0].Rows[0]["unit_name"].ToString().Trim(),
                        CompanyCode = companyInfo.CompanyCode,
                        CompanyName = companyInfo.CompanyName,
                        AppCompanyCode = "",
                        AppCompanyName = "",
                        OriginalCompanyCode = companyInfo.CompanyCode,
                        OriginalCompanyName = companyInfo.CompanyName,
                        BranchCode = (ds.Tables[0].Rows[0].Field<object>("branch_unit_id") == null || ds.Tables[0].Rows[0].Field<object>("branch_unit_id") == DBNull.Value) ? string.Empty : ds.Tables[0].Rows[0].Field<object>("branch_unit_id").ToString(),
                        BranchName = (ds.Tables[0].Rows[0].Field<object>("branch_unit_name") == null || ds.Tables[0].Rows[0].Field<object>("branch_unit_name") == DBNull.Value) ? string.Empty : ds.Tables[0].Rows[0].Field<object>("branch_unit_name").ToString(),
                        PositionID = ds.Tables[0].Rows[0].Field<Object>("position_id").ToString(),
                        PositionName = ds.Tables[0].Rows[0].Field<string>("position_name"),
                        PositionLevel = (ds.Tables[0].Rows[0].Field<Object>("resume_id") == null || ds.Tables[0].Rows[0].Field<Object>("resume_id") == DBNull.Value) ? string.Empty : ds.Tables[0].Rows[0].Field<Object>("resume_id").ToString(),
                        EMail = (ds.Tables[0].Rows[0].Field<Object>("EMAIL") == null || ds.Tables[0].Rows[0].Field<Object>("EMAIL") == DBNull.Value) ? string.Empty : ds.Tables[0].Rows[0].Field<Object>("EMAIL").ToString(),
                        Phone = (ds.Tables[0].Rows[0].Field<Object>("PHONE") == null || ds.Tables[0].Rows[0].Field<Object>("PHONE") == DBNull.Value) ? string.Empty : ds.Tables[0].Rows[0].Field<Object>("PHONE").ToString(),
                        Tel = (ds.Tables[0].Rows[0].Field<Object>("MOBIL") == null || ds.Tables[0].Rows[0].Field<Object>("MOBIL") == DBNull.Value) ? string.Empty : ds.Tables[0].Rows[0].Field<Object>("MOBIL").ToString(),
                        Subordinate_Industry = (ds.Tables[0].Rows[0].Field<Object>("SUBORDINATE_INDUSTRY") == null || ds.Tables[0].Rows[0].Field<Object>("SUBORDINATE_INDUSTRY") == DBNull.Value) ? string.Empty : ds.Tables[0].Rows[0].Field<Object>("SUBORDINATE_INDUSTRY").ToString(),
                        Gender = (ds.Tables[0].Rows[0].Field<Object>("GENDER") == null || ds.Tables[0].Rows[0].Field<Object>("GENDER") == DBNull.Value) ? string.Empty : ds.Tables[0].Rows[0].Field<Object>("GENDER").ToString(),
                        PoliticsStatus = (ds.Tables[0].Rows[0].Field<Object>("POLITICS_STATUS") == null || ds.Tables[0].Rows[0].Field<Object>("POLITICS_STATUS") == DBNull.Value) ? string.Empty : ds.Tables[0].Rows[0].Field<Object>("POLITICS_STATUS").ToString(),
                        BornDate = (ds.Tables[0].Rows[0].Field<Object>("BORN_DATE") == null || ds.Tables[0].Rows[0].Field<Object>("BORN_DATE") == DBNull.Value) ? string.Empty : ds.Tables[0].Rows[0].Field<Object>("BORN_DATE").ToString(),
                        Graduate = (ds.Tables[0].Rows[0].Field<Object>("GRADUATE") == null || ds.Tables[0].Rows[0].Field<Object>("GRADUATE") == DBNull.Value) ? string.Empty : ds.Tables[0].Rows[0].Field<Object>("GRADUATE").ToString(),
                        Province = (ds.Tables[0].Rows[0].Field<Object>("PROVINCE") == null || ds.Tables[0].Rows[0].Field<Object>("PROVINCE") == DBNull.Value) ? string.Empty : ds.Tables[0].Rows[0].Field<Object>("PROVINCE").ToString(),
                        CertificateID = (ds.Tables[0].Rows[0].Field<Object>("CERTIFICATE_ID") == null || ds.Tables[0].Rows[0].Field<Object>("CERTIFICATE_ID") == DBNull.Value) ? string.Empty : ds.Tables[0].Rows[0].Field<Object>("CERTIFICATE_ID").ToString(),
                        CityAddress = (ds.Tables[0].Rows[0].Field<Object>("CITY_ADDRESS") == null || ds.Tables[0].Rows[0].Field<Object>("CITY_ADDRESS") == DBNull.Value) ? string.Empty : ds.Tables[0].Rows[0].Field<Object>("CITY_ADDRESS").ToString(),
                        UsedName = (ds.Tables[0].Rows[0].Field<Object>("USED_NAME") == null || ds.Tables[0].Rows[0].Field<Object>("USED_NAME") == DBNull.Value) ? string.Empty : ds.Tables[0].Rows[0].Field<Object>("USED_NAME").ToString()
                    };
                return userInfo;
            }
            return null;
        }

        /// <summary>
        /// 根据用户关键信息获取用户信息
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public List<UserInfo> GetUserInfoByKeyWord(string keyWord)
        {
            List<UserInfo> serachResult = new List<UserInfo>();

            string sqlString = @"
            select top 10 e.*,u.unit_name,u.unit_id ,u.company_unit_id,u.company_unit_name,u.branch_unit_id,u.branch_unit_name
             from dbo.hpm_lbr_employee e 
             inner join dbo.hpm_org_unit u on e.unit_id=u.unit_id and is_primary_position=1
             where e.employee_code like'%" + keyWord + "%' or e.name like'%" + keyWord + "%' or e.email like'%" + keyWord + "%'";

            DataSet ds = Repository.Query(sqlString);
            if ((ds.Tables[0].Rows.Count > 0))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string companycode = dr.Field<Object>("company_unit_id").ToString();
                    //AppMappingInfo appMappingInfo = GetAppMapping(companycode);
                    CompanyInfo companyInfo = GetCompanyInfoByCode(companycode);

                    UserInfo userInfo =
                        new UserInfo
                        {
                            ID = dr["EMPLOYEE_CODE"].ToString().Trim(),
                            LoginName = "CIMC.COM/" + dr["employee_code"].ToString().Trim(),
                            EmployeeCode = dr["employee_code"].ToString().Trim(),
                            Name = dr["Name"].ToString().Trim(),
                            DepartmentCode = dr["unit_id"].ToString().Trim(),
                            DepartmentName = dr["unit_name"].ToString().Trim(),
                            //CompanyCode = appMappingInfo.CompanyCode,
                            //CompanyName = appMappingInfo.CompanyName,
                            //AppCompanyCode = appMappingInfo.CustomCode,
                            //AppCompanyName = appMappingInfo.CustomName,
                            //OriginalCompanyCode = appMappingInfo.CompanyCode,
                            //OriginalCompanyName = appMappingInfo.CompanyName,
                            CompanyCode = companyInfo.CompanyCode,
                            CompanyName = companyInfo.CompanyName,
                            AppCompanyCode = "",
                            AppCompanyName = "",
                            OriginalCompanyCode = companyInfo.CompanyCode,
                            OriginalCompanyName = companyInfo.CompanyName,
                            BranchCode = dr["branch_unit_id"].ToString(),
                            BranchName = dr["branch_unit_name"].ToString(),
                            PositionID = dr["position_id"].ToString(),
                            PositionName = dr["position_name"].ToString(),
                            PositionLevel = (dr["resume_id"] == DBNull.Value) ? string.Empty : dr.Field<Object>("resume_id").ToString(),
                            EMail = (dr["EMAIL"] == DBNull.Value) ? string.Empty : dr.Field<Object>("EMAIL").ToString(),
                            Phone = (dr["PHONE"] == DBNull.Value) ? string.Empty : dr.Field<Object>("PHONE").ToString(),
                            Tel = (dr["MOBIL"] == DBNull.Value) ? string.Empty : dr.Field<Object>("MOBIL").ToString(),
                            Subordinate_Industry = (dr["SUBORDINATE_INDUSTRY"] == DBNull.Value) ? string.Empty : dr.Field<Object>("SUBORDINATE_INDUSTRY").ToString(),
                            Gender = (dr["GENDER"] == DBNull.Value) ? string.Empty : dr.Field<Object>("GENDER").ToString(),
                            PoliticsStatus = (dr["POLITICS_STATUS"] == DBNull.Value) ? string.Empty : dr.Field<Object>("POLITICS_STATUS").ToString(),
                            BornDate = (dr["BORN_DATE"] == DBNull.Value) ? string.Empty : dr.Field<Object>("BORN_DATE").ToString(),
                            Graduate = (dr["GRADUATE"] == DBNull.Value) ? string.Empty : dr.Field<Object>("GRADUATE").ToString(),
                            Province = (dr["PROVINCE"] == DBNull.Value) ? string.Empty : dr.Field<Object>("PROVINCE").ToString(),
                            CertificateID = (dr["CERTIFICATE_ID"] == DBNull.Value) ? string.Empty : dr.Field<Object>("CERTIFICATE_ID").ToString(),
                            CityAddress = (dr["CITY_ADDRESS"] == DBNull.Value) ? string.Empty : dr.Field<Object>("CITY_ADDRESS").ToString(),
                            UsedName = (dr["USED_NAME"] == DBNull.Value) ? string.Empty : dr.Field<Object>("USED_NAME").ToString()
                        };
                    serachResult.Add(userInfo);
                }
                return serachResult;
            }
            return null;
        }

        /// <summary>
        /// 根据用户邮箱获取用户名
        /// </summary>
        /// <param name="emails"></param>
        /// <returns></returns>
        public string[] GetEmployeeCodeByEmails(string[] emails)
        {
            List<string> result = new List<string>();
            string whereExp = "('";
            foreach (string email in emails)
            {
                whereExp += email + "','";
            }
            whereExp = whereExp.Substring(0, whereExp.Length - 2) + ")";
            string sqlString = @"
             select EMPLOYEE_CODE 
             from dbo.hpm_lbr_employee  where EMAIL in " + whereExp;
            DataSet ds = Repository.Query(sqlString);
            if ((ds.Tables[0].Rows.Count > 0))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    result.Add(dr[0].ToString());
                }
                return result.ToArray();
            }
            else
            {
                return null;
            }




        }
        
        /// <summary>
        /// 根据公司id获取公司信息
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public CompanyInfo GetCompanyInfoByCode(string companyCode)
        {
            string sqlString = @"
             select u.unit_id,u.COMPANY_ABBR_CN as unit_name
             from dbo.hpm_org_unit u where u.level_id=15 and u.unit_id=@company_code";
            SqlParameter[] selpara = {
                              new SqlParameter("@company_code", SqlDbType.NVarChar)};
            selpara[0].Value = companyCode;
            DataSet ds = Repository.Query(sqlString, selpara);
            if ((ds.Tables[0].Rows.Count > 0))
            {
                CompanyInfo companyInfo =
                       new CompanyInfo
                       {
                           ID = "ID",
                           CompanyCode = ds.Tables[0].Rows[0].Field<Object>("unit_id").ToString(),
                           CompanyName = ds.Tables[0].Rows[0].Field<string>("unit_name")
                       };
                return companyInfo;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据部门编号获取部门全路径和部门经理
        /// </summary>
        /// <param name="deptId">部门编号</param>
        /// <returns>部门全路径</returns>
        public string[] GetDepartmentPathAndManager(string deptId)
        {
            //递归树获取部门全路径
            string sqlString = @"with Dep as 
            (select level_id,unit_name,UNIT_ID,PARENT_ID from dbo.hpm_org_unit where UNIT_ID=@unitid
            union all 
            select d.level_id,d.unit_name,d.UNIT_ID,d.PARENT_ID from Dep 
            inner join dbo.hpm_org_unit d on dep.PARENT_ID = d.UNIT_ID 
            )select * from Dep";
            SqlParameter[] selpara = {
                              new SqlParameter("@unitid", SqlDbType.NVarChar)};
            selpara[0].Value = deptId;
            DataSet ds = Repository.Query(sqlString, selpara);
            string deptPath = string.Empty;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //排除第一级(中集集团)
                if (Convert.ToInt32(dr["level_id"].ToString()) > 15)
                    deptPath = dr["unit_name"].ToString() + "/" + deptPath;
            }

            deptPath = deptPath.TrimEnd("/".ToCharArray());
            string[] DeptInfo = { deptPath };
            return DeptInfo;
        }
        /*
        public UserInfo GetDepartmentManager(string loginname)
        {

            if (loginname == null)
            {
                return null;
            }
            OrgChart chart = new OrgChart();

            OracleDBA oDBA = new OracleDBA(connection_string);
            DBAParameter[] paraArray = new DBAParameter[2];
            string employeeCode = loginname.Replace(domian, string.Empty);
            employeeCode = employeeCode.Replace("CIMC/", string.Empty);
            // kevin0729 add row 128 ' and is_primary_position=1'
            string sqlString = @"select a.employee_code from  hr_bpm_lbr_employee a where a.position_id in (select  u2.manager_position  from  Hr_Bpm_Org_Unit u2 where u2.unit_id in
             ( select BRANCH_UNIT_ID from  Hr_Bpm_Org_Unit u where u.unit_id in
            (
             select t.unit_id
             from  HR_BPM_LBR_EMPLOYEE t 
             where t.employee_code=:employee_code
             and is_primary_position=1
             )
             )
             )";
            OracleParameter[] opList = new OracleParameter[1];
            opList[0] = new OracleParameter("@employee_code", OracleDbType.Varchar2);
            opList[0].Value = employeeCode;
            string str = oDBA.ExecuteScalarQuery(sqlString, opList) as string;
            if (str == null)
            {
                return null;
            }
            else
            {
                return GetUserInfoByLoginName(domian + str);
            }
        }


        /// <summary>
        /// 根据部门编号获取部门全路径和部门经理
        /// </summary>
        /// <param name="deptId">部门编号</param>
        /// <returns>部门全路径</returns>
        public static string[] GetDepartmentPathAndManager(string deptId)
        {
            OracleDBA oDBA = new OracleDBA(connection_string);
            //递归树获取部门全路径
            string sqlString = "select level_id,unit_name from hr_bpm_org_unit start with unit_id = :unitid "
                                + "connect by prior parent_id=unit_id";
            OracleParameter[] opList = new OracleParameter[1];
            opList[0] = new OracleParameter("@unitid", OracleDbType.Int32);
            opList[0].Value = deptId;
            DataSet ds = oDBA.ExecuteDataSetQuery(sqlString, opList);
            string deptPath = string.Empty;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //排除第一级(中集集团)
                if (Convert.ToInt32(dr["level_id"].ToString()) > 15)
                    deptPath = dr["unit_name"].ToString() + "/" + deptPath;
            }

            deptPath = deptPath.TrimEnd("/".ToCharArray());

            sqlString = @"select a.employee_code from  hr_bpm_lbr_employee a where a.position_id = (select t.manager_position from HR_BPM_ORG_UNIT t where t.unit_id=:deptid
             )";
            opList = new OracleParameter[1];
            opList[0] = new OracleParameter("@deptid", OracleDbType.Varchar2);
            opList[0].Value = deptId;
            string Manager = oDBA.ExecuteScalarQuery(sqlString, opList) as string;
            string[] DeptInfo = { deptPath, domian + Manager };
            return DeptInfo;
        }
        /// <summary>
        /// 根据部门编号获取部门全路径
        /// </summary>
        /// <param name="deptId">部门编号</param>
        /// <returns>部门全路径</returns>
        public static string[] GetDepartmentPathAndManager(string deptId, int levelid)
        {
            OracleDBA oDBA = new OracleDBA(connection_string);
            //递归树获取部门全路径
            string sqlString = "select level_id,unit_name from hr_bpm_org_unit start with unit_id = :unitid "
                                + "connect by prior parent_id=unit_id";
            OracleParameter[] opList = new OracleParameter[1];
            opList[0] = new OracleParameter("@unitid", OracleDbType.Int32);
            opList[0].Value = deptId;
            DataSet ds = oDBA.ExecuteDataSetQuery(sqlString, opList);
            string deptPath = string.Empty;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //排除第一级(中集集团)
                if (Convert.ToInt32(dr["level_id"].ToString()) > levelid)
                    deptPath = dr["unit_name"].ToString() + "/" + deptPath;
            }

            deptPath = deptPath.TrimEnd("/".ToCharArray());

            sqlString = @"select a.employee_code from  hr_bpm_lbr_employee a where a.position_id = (select t.manager_position from HR_BPM_ORG_UNIT t where t.unit_id=:deptid
             )";
            opList = new OracleParameter[1];
            opList[0] = new OracleParameter("@deptid", OracleDbType.Varchar2);
            opList[0].Value = deptId;
            string Manager = oDBA.ExecuteScalarQuery(sqlString, opList) as string;
            string[] DeptInfo = { deptPath, domian + Manager };
            return DeptInfo;
        }




        public IList<CompanyInfo> GetCompanyList()
        {
            IList<CompanyInfo> companyList = new List<CompanyInfo>();
            OracleDBA oDBA = new OracleDBA(connection_string);

            string sqlString = @"SELECT parent_id,parent_name,unit_id,COMPANY_ABBR_CN as unit_name
                                FROM hr_bpm_org_unit unit_ WHERE unit_.level_id=15 and unit_.COMPANY_ABBR_CN is not null
                                ORDER BY unit_.parent_id,nlssort(unit_.parent_name,'NLS_SORT=SCHINESE_PINYIN_M'), nlssort(unit_.COMPANY_ABBR_CN,'NLS_SORT=SCHINESE_PINYIN_M')";

            DataSet ds = oDBA.ExecuteDataSetQuery(sqlString);
            DataRowCollection datarows = ds.Tables[0].Rows;
            if (datarows.Count > 0)
            {
                foreach (DataRow item in datarows)
                {
                    CompanyInfo companyInfo =
                          new CompanyInfo
                          {
                              ParentCode = item.Field<Object>("parent_id").ToString(),
                              ParentName = item.Field<string>("parent_name"),
                              CompanyCode = item.Field<Object>("unit_id").ToString(),
                              CompanyName = item.Field<string>("unit_name")
                          };
                    companyList.Add(companyInfo);
                }
            }
            return companyList;
        }

        public CompanyInfo GetCompanyInfoByCode(string companyCode)
        {

            OracleDBA oDBA = new OracleDBA(connection_string);

            string sqlString = @"
             select u.unit_id,u.COMPANY_ABBR_CN as unit_name
             from hr_bpm_org_unit u where u.level_id=15 and u.unit_id=:company_code";

            OracleParameter[] opList = new OracleParameter[1];
            opList[0] = new OracleParameter("@company_code", OracleDbType.Varchar2);
            opList[0].Value = companyCode;
            DataSet ds = oDBA.ExecuteDataSetQuery(sqlString, opList);
            if ((ds.Tables[0].Rows.Count > 0))
            {
                CompanyInfo companyInfo =
                       new CompanyInfo
                       {
                           ID = "ID",
                           CompanyCode = ds.Tables[0].Rows[0].Field<Object>("unit_id").ToString(),
                           CompanyName = ds.Tables[0].Rows[0].Field<string>("unit_name")
                       };
                return companyInfo;
            }
            else
            {
                return null;
            }
        }

        public UserInfo GetManager(string loginName)
        {
            OrgChart orc = new OrgChart();
            User user = new User();
            if (orc.FindUser(loginName, null, 0, out user))
            {
                string Manager = string.Empty;
                if (user.GetManager(out Manager))
                {
                    return GetUserInfoByLoginName(Manager);
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }

        }

        public UserInfo GetSupervisor(string loginName)
        {
            OrgChart orc = new OrgChart();
            User user = new User();
            if (orc.FindUser(loginName, null, 0, out user))
            {
                string Supervisor = string.Empty;
                if (user.GetSupervisor(out Supervisor))
                {
                    return GetUserInfoByLoginName(Supervisor);
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 获取指定用户所属二级部门经理
        /// </summary>
        /// <param name="employeeCode">用户工号</param>
        /// <returns>二级部门经理</returns>
        public string GetSubManager(string employeeCode)
        {
            string sqlString = "select e.employee_code from hr_bpm_lbr_employee e "
                            + "inner join hr_bpm_org_unit u on e.position_id=u.manager_position "
                            + "inner join hr_bpm_org_unit u1 on u.unit_id=u1.plant_unit_id "
                            + "inner join hr_bpm_lbr_employee e1 on u1.unit_id=e1.unit_id "
                            + "where u.manager_position is not null and e1.is_primary_position=1 "
                            + "and e1.employee_code=:employeecode";
            employeeCode = employeeCode.Replace(domian, string.Empty);
            List<OracleParameter> opList = new List<OracleParameter>();
            opList.Add(new OracleParameter("@employeecode", employeeCode));
            OracleDBA oDBA = new OracleDBA();
            object oManagerName = oDBA.ExecuteScalarQuery(sqlString, opList.ToArray());
            if (oManagerName == null) return string.Empty;
            string managerName = oManagerName.ToString().Trim();
            if (string.IsNullOrEmpty(managerName)) return string.Empty;
            return domian + managerName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetMangerUser(string loginName, string tableName)
        {

            OracleDBA oDBA = new OracleDBA(connection_string);
            DBAParameter[] paraArray = new DBAParameter[2];
            loginName = loginName.Replace('\\', '/');
            string employeeCode = loginName.Replace(domian, string.Empty);
            employeeCode = employeeCode.Replace("CIMC/", string.Empty);
            loginName = "CIMC.COM/" + employeeCode;

            string sqlString = " select t.manager_code   from " + tableName + "t  inner join hr_bpm_org_unit u on t.unit_id=u.branch_unit_id  "
                              + "inner join hr_bpm_lbr_employee e  on e.unit_id=u.unit_id   where   e.employee_code=:employee_code ";




            OracleParameter[] opList = new OracleParameter[1];
            opList[0] = new OracleParameter("@employee_code", OracleDbType.Varchar2);
            opList[0].Value = employeeCode;

            string manager = oDBA.ExecuteScalarQuery(sqlString, opList) as string;

            if (string.IsNullOrEmpty(manager))
            {

                sqlString = "select e.employee_code from hr_bpm_lbr_employee e inner join hr_bpm_org_position p "
                                   + "on e.position_id=p.RESPONSE_TO inner join hr_bpm_lbr_employee e1  "
                                   + "on e1.position_id=p.POSITION_ID "
                                   + "where e1.employee_code=:employee_code";
                manager = oDBA.ExecuteScalarQuery(sqlString, opList) as string;
            }
            if (manager.IndexOf("/") == -1)
            {

                manager = domian + manager;
            }
            return manager;
        }
         */
        /// <summary>
        /// 上级
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public List<UserInfo> GetSupervisor(string loginName)
        {
            List<UserInfo> result = new List<UserInfo>();
            if (loginName == null)
            {
                return null;
            }
            string employeeCode = loginName.Replace(domian, string.Empty);
            employeeCode = employeeCode.Replace("CIMC/", string.Empty);
            string sqlString = "select e.employee_code from dbo.hpm_lbr_employee e inner join dbo.hpm_org_position p on e.position_id=p.response_to inner join dbo.hpm_lbr_employee e1 on p.position_id=e1.position_id where e1.is_primary_position=1 and e1.employee_code=@employee_code";
            SqlParameter[] selpara = {
                              new SqlParameter("@employee_code", SqlDbType.NVarChar)};
            selpara[0].Value = employeeCode;
            DataSet ds = Repository.Query(sqlString, selpara);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    result.Add(GetUserInfoByLoginName(domian + item[0].ToString().Trim()));
                }
            }
            return result;
        }

        /// <summary>
        /// 部门经理
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public List<UserInfo> GetManager(string loginName)
        {
            // strQuery = "select e.employee_code from hr_bpm_lbr_employee e inner join hr_bpm_org_unit b on e.position_id=b.manager_position inner join hr_bpm_org_unit u on b.unit_id=u.branch_unit_id inner join hr_bpm_lbr_employee e1 on u.unit_id=e1.unit_id and e1.is_primary_position=1 and e1.employee_code='" + str3 + "'";

            List<UserInfo> result = new List<UserInfo>();
            if (loginName == null)
            {
                return null;
            }
            string employeeCode = loginName.Replace(domian, string.Empty);
            employeeCode = employeeCode.Replace("CIMC/", string.Empty);
            string sqlString = @"select a.employee_code from dbo.hpm_lbr_employee a where a.position_id in (select  u2.manager_position  from  dbo.hpm_org_unit u2 where u2.unit_id in
             ( select BRANCH_UNIT_ID from dbo.hpm_org_unit u where u.unit_id in
            (
             select t.unit_id
             from dbo.hpm_lbr_employee t 
             where t.employee_code=@employee_code
             and is_primary_position=1
             )
             )
             )";
            SqlParameter[] selpara = {
                              new SqlParameter("@employee_code", SqlDbType.NVarChar)};
            selpara[0].Value = employeeCode;
            DataSet ds = Repository.Query(sqlString, selpara);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    result.Add(GetUserInfoByLoginName(domian + item[0].ToString().Trim()));
                }
            }
            return result;
        }

        /// <summary>
        /// 二级部门经理
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns></returns>
        public UserInfo GetSubManager(string employeeCode)
        {
            if (employeeCode == null)
            {
                return null;
            }
            string EmployeeCode = employeeCode.Replace(domian, string.Empty);
            EmployeeCode = EmployeeCode.Replace("CIMC/", string.Empty);
            string sqlString = "select e.employee_code from dbo.hpm_lbr_employee e "
                            + "inner join dbo.hpm_org_unit u on e.position_id=u.manager_position "
                            + "inner join dbo.hpm_org_unit u1 on u.unit_id=u1.plant_unit_id "
                            + "inner join dbo.hpm_lbr_employee e1 on u1.unit_id=e1.unit_id "
                            + "where u.manager_position is not null and e1.is_primary_position=1 "
                            + "and e1.employee_code=@employee_code";
            SqlParameter[] selpara = {
                              new SqlParameter("@employee_code", SqlDbType.NVarChar)};
            selpara[0].Value = employeeCode;
            DataSet ds = Repository.Query(sqlString, selpara);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return GetUserInfoByLoginName(domian + ds.Tables[0].Rows[0][0].ToString().Trim());
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 总经理
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns></returns>
        public UserInfo GetGeneralManager(string employeeCode)
        {
            if (employeeCode == null)
            {
                return null;
            }
            string EmployeeCode = employeeCode.Replace(domian, string.Empty);
            EmployeeCode = EmployeeCode.Replace("CIMC/", string.Empty);
            string sqlString = @"  select e1.EMPLOYEE_CODE,e1.NAME from dbo.hpm_lbr_employee e1
            inner join dbo.hpm_org_unit u1 on e1.unit_id=u1.unit_id
            inner join dbo.hpm_org_position p on
            p.NAME_TL='总经理' and e1.POSITION_ID=p.POSITION_ID
            inner join dbo.hpm_lbr_employee e2
            inner join dbo.hpm_org_unit u2 on e2.unit_id=u2.unit_id 
            on e2.EMPLOYEE_CODE=@employee_code and e2.IS_PRIMARY_POSITION =1 
            where  u2.COMPANY_UNIT_ID= u1.COMPANY_UNIT_ID";
            SqlParameter[] selpara = {
                              new SqlParameter("@employee_code", SqlDbType.NVarChar)};
            selpara[0].Value = employeeCode;
            DataSet ds = Repository.Query(sqlString, selpara);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return GetUserInfoByLoginName(domian + ds.Tables[0].Rows[0][0].ToString().Trim());
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据流程配置，获取流程任务接收人的OC架构人员信息
        /// </summary>
        /// <param name="OCType">1表示流程发起人，2表示发起人上级，3表示发起人经理，11表示当前人，12表示当前人上级，13表示当前人经理</param>
        /// <returns></returns>
        public List<UserInfo> GetOCTypeParticipants(int OCType, string flowInitiator, string currentUser)
        {
            List<UserInfo> users = new List<UserInfo>();
            switch(OCType)
            {
                case 1:
                    {
                        users.Add(GetUserInfoByLoginName(flowInitiator));
                        break;
                    }
                case 2:
                    {
                        users = GetSupervisor(flowInitiator);
                        break;
                    }
                case 3:
                    {
                        users = GetManager(flowInitiator);
                        break;
                    }
                case 11:
                    {
                        users.Add(GetUserInfoByLoginName(currentUser));
                        break;
                    }
                case 12:
                    {
                        users = GetSupervisor(currentUser);
                        break;
                    }
                case 13:
                    {
                        users = GetManager(currentUser);
                        break;
                    }
            }
            return users;
        }
    }
}