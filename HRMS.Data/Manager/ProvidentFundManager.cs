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
    /// 公积金信息管理类
    /// </summary>
    public class ProvidentFundManager : ManagerBase
    {
        public static Dictionary<string, string> ProvidentFundDic
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("iGuid", "iGuid");
                dic.Add("人事信息标识", "iHRInfoGuid");
                dic.Add("缴纳地", "iPayPlace");
                dic.Add("是否缴纳", "iIsPaid");
                dic.Add("缴费基数", "iPayBase");
                dic.Add("转入日期", "iEntryDate");
                dic.Add("封存日期", "iSealDate");
                dic.Add("个人缴费金额", "iIndividualAmount");
                dic.Add("公司缴费金额", "iCompanyAmount");
                dic.Add("补缴金额", "iAdditionalAmount");
                dic.Add("补缴月数", "iAdditionalMonths");
                dic.Add("商业保险缴纳公司", "iCommercialInsurancePaidCompany");
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
        public static Dictionary<string, string> ProvidentFundDetailDic
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

        public static Dictionary<string, string> ProvidentFundViewDic
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
                dic.Add("员工意愿", "iProvidentFundPaidWilling");
                dic.Add("是否缴纳", "iIsPaid");
                dic.Add("缴费基数", "iPayBase");
                dic.Add("转入日期", "iEntryDate");
                dic.Add("封存日期", "iSealDate");
                dic.Add("个人缴费金额", "iIndividualAmount");
                dic.Add("公司缴费金额", "iCompanyAmount");
                dic.Add("补缴金额", "iAdditionalAmount");
                dic.Add("补缴月数", "iAdditionalMonths");
                dic.Add("公积金总计", "iTotal");
                dic.Add("是否缴纳商业保险", "iIsCommercialInsurancePaid");
                dic.Add("商业保险缴纳公司", "iCommercialInsurancePaidCompany");
                dic.Add("备注1", "iNote1");
                dic.Add("备注2", "iNote2");
                return dic;
            }
        }

        public static Dictionary<string, string> ProvidentFundDetailViewDic
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
                dic.Add("入职日期", "iEmployeeDate");
                dic.Add("离职日期", "iResignDate");
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
        public void Insert(ProvidentFundEntity entity)
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
                Repository.Insert<ProvidentFundEntity>(session.Connection, entity, session.Transaction);
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
        public void Update(ProvidentFundEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            var record = ModifyRecord(entity);
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<ProvidentFundEntity>(session.Connection, entity, session.Transaction);
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

        public void UpdateDetail(ProvidentFundDetailEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<ProvidentFundDetailEntity>(session.Connection, entity, session.Transaction);
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
        public ProvidentFundModel GetFirstOrDefault(string hrGuid)
        {
            string sql = @"select pf.iIndividualAmount + pf.iCompanyAmount + pf.iAdditionalAmount  as iTotal, pf.*, hr.iItemName, hr.iCompany, hr.iName, hr.iIdCard, hr.iEmpNo, hr.iEmployeeDate,  hr.iResignDate, hr.iEmployeeStatus, hr.iResidenceProperty, hr.iProvidentFundPaidWilling, hr.iIsCommercialInsurancePaid, hr.iGuid as iHRInfoGuid2  from ProvidentFund pf right join hrinfo hr on pf.iHRInfoGuid = hr.iguid and pf.iIsDeleted =0 and pf.iStatus =1  where hr.iisdeleted=0 and hr.istatus=1 and hr.iguid=@id ";
            return Repository.Query<ProvidentFundModel>(sql, new { id = hrGuid }).FirstOrDefault();
        }
        public int GenerateProvidentFundDetailMonthly(int payMonth)
        {
            int affectedRowCount = 0;
            string sql = @"insert into ProvidentFundDetail select newid()," + payMonth.ToString() + ",iHRInfoGuid, iPayPlace, iPayBase, iIndividualAmount, iCompanyAmount, isnull(iAdditionalAmount,0), iAdditionalMonths,GETDATE(),'系统',GETDATE(),'系统',1,0 from ProvidentFund where iIsPaid='是' and iIsDeleted =0 and iStatus =1 ";
            string clearsql = "update ProvidentFund set iAdditionalAmount = null, iAdditionalMonths = null ";
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HRMSDBConnectionString"].ConnectionString))
            {
                Repository.Execute(conn, "delete from ProvidentFundDetail where iPayMonth =" + payMonth.ToString());
                affectedRowCount = Repository.Execute(conn, sql);
                Repository.Execute(conn, clearsql);
            }
            return affectedRowCount;
        }
        public int GenerateProvidentFundDetail(int payMonth, string iguid)
        {
            int affectedRowCount = 0;
            string sql = @"insert into ProvidentFundDetail select newid()," + payMonth.ToString() + ",iHRInfoGuid, iPayPlace, iPayBase, iIndividualAmount, iCompanyAmount, isnull(iAdditionalAmount,0), iAdditionalMonths,GETDATE(),'系统',GETDATE(),'系统',1,0 from ProvidentFund where iGuid='" + iguid + "'";
            string clearsql = "update ProvidentFund set iAdditionalAmount = null, iAdditionalMonths = null where iGuid='" + iguid + "'";
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HRMSDBConnectionString"].ConnectionString))
            {
                affectedRowCount = Repository.Execute(conn, sql);
                Repository.Execute(conn, clearsql);
            }
            return affectedRowCount;
        }
        public ProvidentFundDetailModel GetDetailFirstOrDefault(string iguid)
        {
            string sql = @"select pf.iIndividualAmount + pf.iCompanyAmount + pf.iAdditionalAmount  as iTotal, pf.*, hr.iItemName, hr.iCompany, hr.iName, hr.iIdCard, hr.iEmpNo, hr.iEmployeeStatus from ProvidentFundDetail pf inner join hrinfo hr on pf.iHRInfoGuid = hr.iguid and pf.iIsDeleted =0 and pf.iStatus =1  where hr.iisdeleted=0 and hr.istatus=1 and pf.iguid=@id ";
            return Repository.Query<ProvidentFundDetailModel>(sql, new { id = iguid }).FirstOrDefault();
        }
        public ProvidentFundEntity FirstOrDefault(string hrGuid)
        {
            string sql = @"select * from ProvidentFund where iHRInfoGuid=@hrid and iIsDeleted =0 and iStatus =1";
            return Repository.Query<ProvidentFundEntity>(sql, new { hrid = hrGuid }).FirstOrDefault();
        }

        public List<ProvidentFundModel> GetSearch(string userType, Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            string commonSql = GenerateQuerySql(userType, para);
            string querySql = "select  Isnull(pf.iIndividualAmount,0) + Isnull(pf.iCompanyAmount,0) + Isnull(pf.iAdditionalAmount,0) as iTotal, pf.*, hr.iItemName, hr.iCompany, hr.iName, hr.iIdCard, hr.iEmpNo, hr.iEmployeeDate,  hr.iResignDate, hr.iEmployeeStatus, hr.iResidenceProperty, hr.iProvidentFundPaidWilling, hr.iIsCommercialInsurancePaid, hr.iGuid as iHRInfoGuid2 " + commonSql + "order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<ProvidentFundModel>(querySql).ToList();

        }

        public List<ProvidentFundDetailModel> GetDetailSearch(string userType, Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            string commonSql = GenerateDetailQuerySql(userType, para);
            string querySql = "select pf.iIndividualAmount + pf.iCompanyAmount + pf.iAdditionalAmount  as iTotal, pf.*, hr.iItemName, hr.iCompany, hr.iName, hr.iIdCard, hr.iEmpNo, hr.iEmployeeStatus, hr.iEmployeeDate, hr.iResignDate " + commonSql + "order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            string sumSql = "select sum(iIndividualAmount) as iIndividualAmount, sum(iCompanyAmount) as iCompanyAmount, sum(iIndividualAmount+iCompanyAmount+iAdditionalAmount) as iTotal " + commonSql;
            ProvidentFundDetailModel sumRecord = Repository.Query<ProvidentFundDetailModel>(sumSql).ToList().FirstOrDefault();
            List<ProvidentFundDetailModel> result = Repository.Query<ProvidentFundDetailModel>(querySql).ToList();
            result.Add(new ProvidentFundDetailModel { iIndividualAmount = sumRecord.iIndividualAmount, iCompanyAmount = sumRecord.iCompanyAmount, iTotal = sumRecord.iTotal });  //多加一列传递总值，记得删除
            return result;
        }

        public List<ProvidentFundModel> GetSearchAll(string userType, Dictionary<string, string> para)
        {
            string commonSql = GenerateQuerySql(userType, para);
            string querySql = "select pf.iIndividualAmount + pf.iCompanyAmount + pf.iAdditionalAmount  as iTotal, pf.*, hr.iItemName, hr.iCompany, hr.iName, hr.iIdCard, hr.iEmpNo, hr.iEmployeeDate,  hr.iResignDate, hr.iEmployeeStatus, hr.iResidenceProperty, hr.iProvidentFundPaidWilling, hr.iIsCommercialInsurancePaid, hr.iGuid as iHRInfoGuid2 " + commonSql + "order by pf.iUpdatedOn desc, hr.iUpdatedOn desc";
            return Repository.Query<ProvidentFundModel>(querySql).ToList();
        }
        public List<ProvidentFundDetailModel> GetDetailSearchAll(string userType, Dictionary<string, string> para)
        {
            string commonSql = GenerateDetailQuerySql(userType, para);
            string querySql = "select pf.iIndividualAmount + pf.iCompanyAmount + pf.iAdditionalAmount  as iTotal, pf.*, hr.iItemName, hr.iCompany, hr.iName, hr.iIdCard, hr.iEmpNo, hr.iEmployeeStatus, hr.iEmployeeDate, hr.iResignDate " + commonSql + "order by pf.iUpdatedOn desc";
            return Repository.Query<ProvidentFundDetailModel>(querySql).ToList();

        }

        //生成通用查询前面部分
        public string GenerateQuerySql(string userType, Dictionary<string, string> para)
        {
            if (userType != "普通用户")
            {
                para["iCompany"] = para["iCompany"] == "-" ? "" : para["iCompany"];
                para["iItemName"] = para["iItemName"] == "-" ? "" : para["iItemName"];
            }
            StringBuilder commandsb = new StringBuilder("from ProvidentFund pf right join hrinfo hr on pf.iHRInfoGuid = hr.iguid and pf.iIsDeleted =0 and pf.iStatus =1 where hr.iisdeleted=0 and hr.istatus=1 ");

            if (para["editType"] == "已编辑")
            {
                commandsb.Append("and pf.iGuid is not null ");
            }
            else if (para["editType"] == "未编辑")
            {
                commandsb.Append("and pf.iGuid is null ");
            }
            para.Remove("editType");

            string searchKey = para["search"];
            para.Remove("search");

            foreach (KeyValuePair<string, string> item in para)
            {
                if (!string.IsNullOrEmpty(item.Value) && item.Value != "§")
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
            if (!string.IsNullOrEmpty(searchKey))
            {
                commandsb.Append(" and (");
                foreach (var item in Common.ConvertHelper.DicConvert(ProvidentFundViewDic))
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
            StringBuilder commandsb = new StringBuilder("from ProvidentFundDetail pf inner join hrinfo hr on pf.iHRInfoGuid = hr.iguid and pf.iIsDeleted =0 and pf.iStatus =1  where hr.iisdeleted=0 and hr.istatus=1 ");

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
                foreach (var item in Common.ConvertHelper.DicConvert(ProvidentFundDetailViewDic))
                {
                    if (item.Key == "iGuid" || item.Key == "iTotal") continue;
                    commandsb.Append(item.Key + " like '%" + searchKey + "%' or ");
                }
                commandsb.Remove(commandsb.Length - 3, 3);
                commandsb.Append(")");
            }
            return commandsb.ToString();
        }

        public string GetValidProvidentFundHrId(string company, string empcode, string idcard, string itemName)
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

        public ModifyLogEntity ModifyRecord(ProvidentFundEntity entity)
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

                Dictionary<string, string> dicConvertTmp = ConvertHelper.DicConvert(ProvidentFundDic);

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
                en.iTableName = "ProvidentFund";
                return en;
            }
        }


    }
}