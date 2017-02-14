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
    /// 账务汇总管理类
    /// </summary>
    public class FinanceSummaryManager : ManagerBase
    {
        //建表dic
        public static Dictionary<string, string> FinanceSummaryDic
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("iGuid", "iGuid");
                dic.Add("公司", "iCompanyId");
                dic.Add("项目", "iProjectId");
                dic.Add("日期", "iDate");
                dic.Add("收款情况", "iReceivable");
                dic.Add("开票金额", "iInvoice");
                dic.Add("工资(收)", "iSalaryIn");
                dic.Add("管理费", "iManageFee");
                dic.Add("社保(收)", "iSocialSecurityIn");
                dic.Add("公积金(收)", "iProvidentFundIn");
                dic.Add("残保(收)", "iDisabilityBenefitsIn");
                dic.Add("宿舍费(收)", "iDormitoryFeeIn");
                dic.Add("其它(收)", "iOtherIn");
                dic.Add("收款备注", "iInNote");
                dic.Add("工资(付)", "iSalaryOut");
                dic.Add("社保公司(付)", "iSocialSecurityCompanyPay");
                dic.Add("社保个人(付)", "iSocialSecurityPersonalPay");
                dic.Add("公积金个人(付) ", "iProvidentFundPersonalPay");
                dic.Add("公积金公司(付)", "iProvidentFundCompanyPay");
                dic.Add("社保补缴", "iSocialSecurityAdditional");
                dic.Add("返费", "iReturnFee");
                dic.Add("残保金(付)", "iDisabilityBenefitsPay");
                dic.Add("班车费(付)", "iBusFee");
                dic.Add("餐费(付)", "iMealFee");
                dic.Add("宿舍费(付)", "iDormitoryFeePay");
                dic.Add("商保费(付)", "iCommercialInsuranceFeePay");
                dic.Add("奖金类(付)", "iBonusPay");
                dic.Add("赔付款(付)", "iCompensationPay");
                dic.Add("税金", "iTaxFee");
                dic.Add("代理费", "iAgentFee");
                dic.Add("劳保费支出", "iLaborInsuranceFee");
                dic.Add("租金", "iRentFee");
                dic.Add("水电物管费", "iUtilitiesFee");
                dic.Add("办公费", "iOfficePay");
                dic.Add("公司人员工资", "iCompanySalaryPay");
                dic.Add("公司人员社保", "iCompanySocialSecurityPay");
                dic.Add("公司人员公积金", "iCompanyProvidentFundPay");
                dic.Add("工伤费用", "iInjuryPay");
                dic.Add("其它付", "iOtherPay");
                dic.Add("支出备注", "iPayNote");

                dic.Add("iCreatedOn", "iCreatedOn");
                dic.Add("iCreatedBy", "iCreatedBy");
                dic.Add("iUpdatedOn", "iUpdatedOn");
                dic.Add("iUpdatedBy", "iUpdatedBy");
                dic.Add("iStatus", "iStatus");
                dic.Add("iIsDeleted", "iIsDeleted");
                return dic;
            }
        }

        public static Dictionary<string, string> FinanceSummaryViewDic
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("公司", "iCompanyId");
                dic.Add("项目", "iProjectId");
                dic.Add("日期", "iDate");
                dic.Add("收款情况", "iReceivable");
                dic.Add("开票金额", "iInvoice");
                dic.Add("差异", "iDiffer");  //

                dic.Add("工资(收)", "iSalaryIn");
                dic.Add("管理费", "iManageFee");
                dic.Add("社保(收)", "iSocialSecurityIn");
                dic.Add("公积金(收)", "iProvidentFundIn");
                dic.Add("残保(收)", "iDisabilityBenefitsIn");
                dic.Add("宿舍费(收)", "iDormitoryFeeIn");
                dic.Add("其它(收)", "iOtherIn");
                dic.Add("收款备注", "iInNote");
                dic.Add("工资(付)", "iSalaryOut");
                dic.Add("社保公司(付)", "iSocialSecurityCompanyPay");
                dic.Add("社保个人(付)", "iSocialSecurityPersonalPay");
                dic.Add("公积金个人(付) ", "iProvidentFundPersonalPay");
                dic.Add("公积金公司(付)", "iProvidentFundCompanyPay");
                dic.Add("社保补缴", "iSocialSecurityAdditional");
                dic.Add("返费", "iReturnFee");
                dic.Add("残保金(付)", "iDisabilityBenefitsPay");
                dic.Add("班车费(付)", "iBusFee");
                dic.Add("餐费(付)", "iMealFee");
                dic.Add("宿舍费(付)", "iDormitoryFeePay");
                dic.Add("商保费(付)", "iCommercialInsuranceFeePay");
                dic.Add("奖金类(付)", "iBonusPay");
                dic.Add("赔付款(付)", "iCompensationPay");
                dic.Add("税金", "iTaxFee");
                dic.Add("代理费", "iAgentFee");
                dic.Add("劳保费支出", "iLaborInsuranceFee");
                dic.Add("租金", "iRentFee");
                dic.Add("水电物管费", "iUtilitiesFee");
                dic.Add("办公费", "iOfficePay");
                dic.Add("公司人员工资", "iCompanySalaryPay");
                dic.Add("公司人员社保", "iCompanySocialSecurityPay");
                dic.Add("公司人员公积金", "iCompanyProvidentFundPay");
                dic.Add("工伤费用", "iInjuryPay");
                dic.Add("其它付", "iOtherPay");
                dic.Add("支出备注", "iPayNote");
                dic.Add("利润", "iProfit");
                return dic;
            }
        }

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Insert(FinanceSummaryEntity entity)
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
                Repository.Insert<FinanceSummaryEntity>(session.Connection, entity, session.Transaction);
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
        public void Update(FinanceSummaryEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            var record = ModifyRecord(entity);
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<FinanceSummaryEntity>(session.Connection, entity, session.Transaction);
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
        public FinanceSummaryEntity FirstOrDefault(string iguid)
        {
            string sql = @"select * from FinanceSummary where iisdeleted=0 and istatus=1 and iguid=@id ";
            return Repository.Query<FinanceSummaryEntity>(sql, new { id = iguid }).FirstOrDefault();
        }

        public int GenerateFinanceSummaryMonthly(int month)
        {
            int affectedRowCount = 0;
            string sql = @"SELECT distinct [iCompanyId] ,[iProjectId] FROM [SysCompanyProjectRelation] where iIsDeleted=0 and iStatus=1";
            DataSet ds = DbHelperSQL.Query(sql);
            List<string> insertSqls = new List<string>();
            string insertTmp = "insert into FinanceSummary (iGuid, iCompanyId, iProjectId, iDate, [iSocialSecurityCompanyPay],[iSocialSecurityPersonalPay][iProvidentFundPersonalPay],[iProvidentFundCompanyPay],[iSocialSecurityAdditional] ,[iReturnFee], [iOfficePay], ,[iCreatedOn],[iCreatedBy] ,[iUpdatedOn] ,[iUpdatedBy],[iStatus],[iIsDeleted]) values(newid(), )";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

            }


            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HRMSDBConnectionString"].ConnectionString))
            {
                Repository.Execute(conn, "delete from FinanceSummary where iPayMonth =" + month.ToString());
                affectedRowCount = Repository.Execute(conn, sql);
            }
            return affectedRowCount;
        }

        public List<FinanceSummaryEntity> GetSearch(string userType, Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            string commonSql = GenerateQuerySql(userType, para);
            string querySql = "select * " + commonSql + "order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<FinanceSummaryEntity>(querySql).ToList();

        }

        public List<FinanceSummaryEntity> GetSearchAll(string userType, Dictionary<string, string> para)
        {
            string commonSql = GenerateQuerySql(userType, para);
            string querySql = "select * " + commonSql + "order by iUpdatedOn desc";
            return Repository.Query<FinanceSummaryEntity>(querySql).ToList();
        }

        //生成通用查询前面部分
        public string GenerateQuerySql(string userType, Dictionary<string, string> para)
        {
            if (userType != "普通用户")
            {
                para["iCompanyId"] = para["iCompanyId"] == "-" ? "" : para["iCompanyId"];
                para["iProjectId"] = para["iProjectId"] == "-" ? "" : para["iProjectId"];
            }
            StringBuilder commandsb = new StringBuilder("from FinanceSummary  where iisdeleted=0 and istatus=1 ");

            string searchKey = para["search"];
            para.Remove("search");

            foreach (KeyValuePair<string, string> item in para)
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
            if (!string.IsNullOrEmpty(searchKey))
            {
                commandsb.Append(" and (");
                foreach (var item in Common.ConvertHelper.DicConvert(FinanceSummaryDic))
                {
                    if (item.Key == "iGuid" || item.Key == "iTotal") continue;
                    commandsb.Append(item.Key + " like '%" + searchKey + "%' or ");
                }
                commandsb.Remove(commandsb.Length - 3, 3);
                commandsb.Append(")");
            }
            return commandsb.ToString();
        }

        public ModifyLogEntity ModifyRecord(FinanceSummaryEntity entity)
        {
            var oldEntity = FirstOrDefault(entity.iGuid);
            if (oldEntity == null)
            {
                return null;
            }
            else
            {
                string modifiedContent = string.Empty;
                System.Reflection.PropertyInfo[] properties = entity.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                Dictionary<string, string> dicConvertTmp = ConvertHelper.DicConvert(FinanceSummaryDic);

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
                en.iTableName = "FinanceSummary";
                return en;
            }
        }

    }
}