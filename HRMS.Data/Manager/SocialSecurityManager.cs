using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HRMS.Data.Entity;
using System.Data.SqlClient;
using System.Configuration;
using HRMS.Common;

namespace HRMS.Data.Manager
{
    /// <summary>
    /// 社保信息管理类
    /// </summary>
    public class SocialSecurityManager : ManagerBase
    {
        //建表dic
        public static Dictionary<string, string> SocialSecurityDic
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("iGuid", "iGuid");
                dic.Add("人事信息标识", "iHRInfoGuid");
                dic.Add("缴纳地", "iPayPlace");
                dic.Add("员工意愿", "iEmployeeWilling");
                dic.Add("缴费基数", "iPayBase");
                dic.Add("入保日期", "iFirstEntryDate");
                dic.Add("招工日期", "iFirstRecruitDate");
                dic.Add("退保日期", "iFirstExitDate");
                dic.Add("退工日期", "iFirstResignDate");
                dic.Add("二次入保日期", "iSecondEntryDate");
                dic.Add("二次招工日期", "iSecondRecruitDate");
                dic.Add("二次退保日期", "iSecondExitDate");
                dic.Add("二次退工日期", "iSecondResignDate");
                dic.Add("个人缴费金额", "iIndividualAmount");
                dic.Add("公司缴费金额", "iCompanyAmount");
                dic.Add("补缴金额", "iAdditionalAmount");
                dic.Add("补缴月数", "iAdditionalMonths");
                dic.Add("备注1", "iNote1");
                dic.Add("备注2", "iNote2");

                dic.Add("iCreatedOn", "iCreatedOn");
                dic.Add("iCreatedBy", "iCreatedBy");
                dic.Add("iUpdatedOn", "iUpdatedOn");
                dic.Add("iUpdatedBy", "iUpdatedBy");
                dic.Add("iStatus", "iStatus");
                dic.Add("iIsDeleted", "iIsDeleted");
                return dic;
            }
        }

        //建表dic
        public static Dictionary<string, string> SocialSecurityDetailDic
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("iGuid", "iGuid");
                dic.Add("缴纳月份", "iPayMonth");
                dic.Add("人事信息标识", "iHRInfoGuid");
                dic.Add("缴纳地", "iPayPlace");
                dic.Add("缴费基数", "iPayBase");
                dic.Add("个人缴费金额", "iIndividualAmount");
                dic.Add("公司缴费金额", "iCompanyAmount");
                dic.Add("补缴金额", "iAdditionalAmount");
                dic.Add("补缴月数", "iAdditionalMonths");

                dic.Add("iCreatedOn", "iCreatedOn");
                dic.Add("iCreatedBy", "iCreatedBy");
                dic.Add("iUpdatedOn", "iUpdatedOn");
                dic.Add("iUpdatedBy", "iUpdatedBy");
                dic.Add("iStatus", "iStatus");
                dic.Add("iIsDeleted", "iIsDeleted");
                return dic;
            }
        }

        public static Dictionary<string, string> SocialSecurityViewDic
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("项目名称", "iItemName");
                dic.Add("所在公司", "iCompany");
                dic.Add("工号", "iEmpNo");
                dic.Add("姓名", "iName");
                dic.Add("身份证号", "iIdCard");
                dic.Add("入职时间", "iEmployeeDate");
                dic.Add("离职日期", "iResignDate");
                dic.Add("员工状态", "iEmployeeStatus");
                dic.Add("户籍类型", "iResidenceProperty");
                dic.Add("缴纳地", "iPayPlace");
                dic.Add("员工意愿", "iSocialInsurancePaidWilling");
                dic.Add("是否缴纳", "iIsPaid");
                dic.Add("缴费基数", "iPayBase");
                dic.Add("入保日期", "iFirstEntryDate");
                dic.Add("招工日期", "iFirstRecruitDate");
                dic.Add("退保日期", "iFirstExitDate");
                dic.Add("退工日期", "iFirstResignDate");
                dic.Add("二次入保日期", "iSecondEntryDate");
                dic.Add("二次招工日期", "iSecondRecruitDate");
                dic.Add("二次退保日期", "iSecondExitDate");
                dic.Add("二次退工日期", "iSecondResignDate");
                dic.Add("个人缴费金额", "iIndividualAmount");
                dic.Add("公司缴费金额", "iCompanyAmount");
                dic.Add("补缴金额", "iAdditionalAmount");
                dic.Add("补缴月数", "iAdditionalMonths");
                dic.Add("社保总计", "iTotal");
                dic.Add("备注1", "iNote1");
                dic.Add("备注2", "iNote2");
                return dic;
            }
        }

        public static Dictionary<string, string> SocialSecurityDetailViewDic
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("月份", "iPayMonth");
                dic.Add("项目名称", "iItemName");
                dic.Add("所在公司", "iCompany");
                dic.Add("工号", "iEmpNo");
                dic.Add("姓名", "iName");
                dic.Add("身份证号", "iIdCard");
                dic.Add("缴纳地", "iPayPlace");
                dic.Add("缴费基数", "iPayBase");
                dic.Add("个人缴费金额", "iIndividualAmount");
                dic.Add("公司缴费金额", "iCompanyAmount");
                dic.Add("补缴金额", "iAdditionalAmount");
                dic.Add("补缴月数", "iAdditionalMonths");
                dic.Add("社保总计", "iTotal");
                return dic;
            }
        }

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Insert(SocialSecurityEntity entity)
        {
            if (string.IsNullOrEmpty(entity.iGuid))
                entity.iGuid = Guid.NewGuid().ToString();
            entity.iCreatedOn = DateTime.Now;
            entity.iUpdatedOn = DateTime.Now;
            entity.iIsDeleted = 0;
            entity.iStatus = 1;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Insert<SocialSecurityEntity>(session.Connection, entity, session.Transaction);
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
        public void Update(SocialSecurityEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            var record = ModifyRecord(entity);
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<SocialSecurityEntity>(session.Connection, entity, session.Transaction);
                if (record != null)
                {
                    Repository.Insert<ModifyLogEntity>(session.Connection, record, session.Transaction);
                }
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
        public void UpdateDetail(SocialSecurityDetailEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<SocialSecurityDetailEntity>(session.Connection, entity, session.Transaction);
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
        public SocialSecurityModel GetFirstOrDefault(string hrGuid)
        {
            string sql = @"select ss.iIndividualAmount + ss.iCompanyAmount + iAdditionalAmount  as iTotal, ss.*, hr.iItemName, hr.iCompany, hr.iName, hr.iIdCard, hr.iEmpNo, hr.iEmployeeDate,  hr.iResignDate, hr.iEmployeeStatus, hr.iResidenceProperty, hr.iSocialInsurancePaidWilling, hr.iGuid as iHRInfoGuid2  from SocialSecurity ss right join hrinfo hr on ss.iHRInfoGuid = hr.iguid and ss.iIsDeleted =0 and ss.iStatus =1  where hr.iisdeleted=0 and hr.istatus=1 and hr.iguid=@id ";
            return Repository.Query<SocialSecurityModel>(sql, new { id = hrGuid }).FirstOrDefault();
        }
        public SocialSecurityDetailModel GetDetailFirstOrDefault(string iguid)
        {
            string sql = @"select ss.iIndividualAmount + ss.iCompanyAmount + iAdditionalAmount  as iTotal, ss.*, hr.iItemName, hr.iCompany, hr.iName, hr.iIdCard, hr.iEmpNo from SocialSecurityDetail ss inner join hrinfo hr on ss.iHRInfoGuid = hr.iguid and ss.iIsDeleted =0 and ss.iStatus =1  where hr.iisdeleted=0 and hr.istatus=1 and ss.iguid=@id ";
            return Repository.Query<SocialSecurityDetailModel>(sql, new { id = iguid }).FirstOrDefault();
        }

        public SocialSecurityEntity FirstOrDefault(string hrGuid)
        {
            string sql = @"select * from SocialSecurity where iHRInfoGuid=@hrid and iIsDeleted =0 and iStatus =1";
            return Repository.Query<SocialSecurityEntity>(sql, new { hrid = hrGuid }).FirstOrDefault();
        }

        public int GenerateSocialSecurityDetailMonthly(int payMonth)
        {
            int affectedRowCount = 0;
            string sql = @"insert into SocialSecurityDetail select newid()," + payMonth.ToString() + ",iHRInfoGuid, iPayPlace, iPayBase, iIndividualAmount, iCompanyAmount, iAdditionalAmount, iAdditionalMonths,GETDATE(),'系统',GETDATE(),'系统',1,0 from SocialSecurity where iIsPaid='是' and iIsDeleted =0 and iStatus =1";
            string clearsql = "update SocialSecurity set iAdditionalAmount = null, iAdditionalMonths = null";
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HRMSDBConnectionString"].ConnectionString))
            {
                Repository.Execute(conn, "delete from SocialSecurityDetail where iPayMonth =" + payMonth.ToString());
                affectedRowCount = Repository.Execute(conn, sql);
                Repository.Execute(conn, clearsql);
            }
            return affectedRowCount;
        }

        public List<SocialSecurityModel> GetSearch(string userType, Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            string commonSql = GenerateQuerySql(userType, para);
            string querySql = "select ss.iIndividualAmount + ss.iCompanyAmount + iAdditionalAmount  as iTotal, ss.*, hr.iItemName, hr.iCompany, hr.iName, hr.iIdCard, hr.iEmpNo, hr.iEmployeeDate,  hr.iResignDate, hr.iEmployeeStatus, hr.iResidenceProperty, hr.iSocialInsurancePaidWilling, hr.iGuid as iHRInfoGuid2 " + commonSql + "order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<SocialSecurityModel>(querySql).ToList();

        }

        public List<SocialSecurityDetailModel> GetDetailSearch(string userType, Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            string commonSql = GenerateDetailQuerySql(userType, para);
            string querySql = "select ss.iIndividualAmount + ss.iCompanyAmount + iAdditionalAmount  as iTotal, ss.*, hr.iItemName, hr.iCompany, hr.iName, hr.iIdCard, hr.iEmpNo " + commonSql + "order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<SocialSecurityDetailModel>(querySql).ToList();

        }

        public List<SocialSecurityModel> GetSearchAll(string userType, Dictionary<string, string> para)
        {
            string commonSql = GenerateQuerySql(userType, para);
            string querySql = "select ss.iIndividualAmount + ss.iCompanyAmount + iAdditionalAmount  as iTotal, ss.*, hr.iItemName, hr.iCompany, hr.iName, hr.iIdCard, hr.iEmpNo, hr.iEmployeeDate,  hr.iResignDate, hr.iEmployeeStatus, hr.iResidenceProperty, hr.iSocialInsurancePaidWilling, hr.iGuid as iHRInfoGuid2 " + commonSql + "order by ss.iUpdatedOn desc, hr.iUpdatedOn desc";
            return Repository.Query<SocialSecurityModel>(querySql).ToList();
        }
        public List<SocialSecurityDetailModel> GetDetailSearchAll(string userType, Dictionary<string, string> para)
        {
            string commonSql = GenerateDetailQuerySql(userType, para);
            string querySql = "select ss.iIndividualAmount + ss.iCompanyAmount + iAdditionalAmount  as iTotal, ss.*, hr.iItemName, hr.iCompany, hr.iName, hr.iIdCard, hr.iEmpNo  " + commonSql + "order by ss.iUpdatedOn desc ";
            return Repository.Query<SocialSecurityDetailModel>(querySql).ToList();
        }

        //生成通用查询前面部分
        public string GenerateQuerySql(string userType, Dictionary<string, string> para)
        {
            if (userType != "普通用户")
            {
                para["iCompany"] = para["iCompany"] == "-" ? "" : para["iCompany"];
                para["iItemName"] = para["iItemName"] == "-" ? "" : para["iItemName"];
            }
            StringBuilder commandsb = new StringBuilder("from SocialSecurity ss right join hrinfo hr on ss.iHRInfoGuid = hr.iguid and ss.iIsDeleted =0 and ss.iStatus =1  where hr.iisdeleted=0 and hr.istatus=1 ");

            if (para["editType"] == "已编辑")
            {
                commandsb.Append("and ss.iGuid is not null ");
            }
            else if (para["editType"] == "未编辑")
            {
                commandsb.Append("and ss.iGuid is null ");
            }
            para.Remove("editType");

            string searchKey = para["search"];
            para.Remove("search");

            foreach (KeyValuePair<string, string> item in para)
            {
                if (!string.IsNullOrEmpty(item.Value) && item.Value != "§")
                {
                    if (item.Key.StartsWith("iFirst"))
                    {
                        commandsb.Append(" and (");
                        if (item.Key.EndsWith("[d]"))
                        {
                            commandsb.Append(item.Key.Replace("[d]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "1900-01-01" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.Value.Split('§')[1]) + "' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iSecond").Replace("[d]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "1900-01-01" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.Value.Split('§')[1]) + "' ");
                        }
                        else
                        {
                            commandsb.Append(item.Key + " like '%" + item.Value + "%' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iSecond") + " like '%" + item.Value + "%' ");
                        }
                        commandsb.Append(" ) ");
                    }
                    else
                    {
                        if (item.Key.EndsWith("[d]"))
                        {
                            commandsb.Append(" and " + item.Key.Replace("[d]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "1900-01-01" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.Value.Split('§')[1]) + "' ");
                        }
                        else
                        {
                            commandsb.Append(" and " + item.Key + " like '%" + item.Value + "%'");
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(searchKey))
            {
                commandsb.Append(" and (");
                foreach (var item in Common.ConvertHelper.DicConvert(SocialSecurityViewDic))
                {
                    if (item.Key == "iGuid" || item.Key == "iTotal") continue;
                    commandsb.Append(item.Key + " like '%" + searchKey + "%' or ");
                }
                commandsb.Remove(commandsb.Length - 3, 3);
                commandsb.Append(")");
            }
            return commandsb.ToString();
        }
        public string GenerateDetailQuerySql(string userType, Dictionary<string, string> para)
        {
            if (userType != "普通用户")
            {
                para["iCompany"] = para["iCompany"] == "-" ? "" : para["iCompany"];
                para["iItemName"] = para["iItemName"] == "-" ? "" : para["iItemName"];
            }
            StringBuilder commandsb = new StringBuilder("from SocialSecurityDetail ss inner join hrinfo hr on ss.iHRInfoGuid = hr.iguid and ss.iIsDeleted =0 and ss.iStatus =1  where hr.iisdeleted=0 and hr.istatus=1 ");

            string searchKey = para["search"];
            para.Remove("search");

            foreach (KeyValuePair<string, string> item in para)
            {
                if (!string.IsNullOrEmpty(item.Value) && item.Value != "§")
                {
                    if (item.Key.EndsWith("[i]"))
                    {
                        commandsb.Append(" and " + item.Key.Replace("[i]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "0" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? "9999" : item.Value.Split('§')[1]) + "' ");
                    }
                    else
                    {
                        commandsb.Append(" and " + item.Key + " like '%" + item.Value + "%'");
                    }
                }
            }
            if (!string.IsNullOrEmpty(searchKey))
            {
                commandsb.Append(" and (");
                foreach (var item in Common.ConvertHelper.DicConvert(SocialSecurityDetailViewDic))
                {
                    if (item.Key == "iGuid" || item.Key == "iTotal") continue;
                    commandsb.Append(item.Key + " like '%" + searchKey + "%' or ");
                }
                commandsb.Remove(commandsb.Length - 3, 3);
                commandsb.Append(")");
            }
            return commandsb.ToString();
        }

        public string GetValidSocialSecurityHrId(string company, string empcode, string idcard, string itemName)
        {
            string sql = "select iGuid from HRInfo where iIsDeleted =0 and iStatus=1 and iCompany='" + company + "' and iItemName = '" + itemName + "' and iEmpNo = '" + empcode + "' and iIdCard ='" + idcard + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0][0].ToString();
            }
        }
        public ModifyLogEntity ModifyRecord(SocialSecurityEntity entity)
        {
            var oldEntity = FirstOrDefault(entity.iHRInfoGuid);
            if (oldEntity == null)
            {
                return null;
            }
            else
            {
                string modifiedContent = string.Empty;
                System.Reflection.PropertyInfo[] properties = entity.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                Dictionary<string, string> dicConvertTmp = ConvertHelper.DicConvert(SocialSecurityDic);

                foreach (System.Reflection.PropertyInfo item in properties)
                {
                    string name = item.Name;
                    if (dicConvertTmp.ContainsKey(name) && !dicConvertTmp[name].StartsWith("i"))
                    {
                        object value = item.GetValue(entity, null);
                        if (value == null) value = "";
                        object valueOld = item.GetValue(oldEntity, null);
                        if (valueOld == null) valueOld = "";
                        if (value.ToString() != valueOld.ToString() && (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String")))
                        {
                            modifiedContent += string.Format("{0}:[{1}]->[{2}] ;", dicConvertTmp[name], valueOld, value);
                        }
                    }
                }
                if (string.IsNullOrEmpty(modifiedContent))
                {
                    return null;
                }
                ModifyLogEntity en = new ModifyLogEntity();
                en.iId = entity.iGuid;
                en.iModifiedBy = entity.iUpdatedBy;
                en.iModifiedOn = DateTime.Now;
                en.iModifiedContent = modifiedContent;
                en.iTableName = "SocialSecurity";
                return en;
            }
        }

    }
}