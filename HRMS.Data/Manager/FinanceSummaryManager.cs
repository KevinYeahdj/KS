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
                dic.Add("派遣工工资(收)", "iSalaryIn");
                dic.Add("临时工工资(收)", "iTemporarySalaryIn");
                dic.Add("外包工工资(收)", "iOutSourceSalaryIn");
                dic.Add("管理费", "iManageFee");
                //dic.Add("社保(收)", "iSocialSecurityIn");
                dic.Add("个人社保(收)", "iSocialSecurityInP");
                dic.Add("公司社保(收)", "iSocialSecurityInC");
                dic.Add("公积金(收)", "iProvidentFundIn");
                dic.Add("残保(收)", "iDisabilityBenefitsIn");
                dic.Add("宿舍费(收)", "iDormitoryFeeIn");
                dic.Add("其它(收)", "iOtherIn");
                dic.Add("收款备注", "iInNote");
                dic.Add("派遣工工资(付)", "iSalaryOut");
                dic.Add("临时工工资(付)", "iTemporarySalaryOut");
                dic.Add("外包工工资(付)", "iOutSourceSalaryOut");
                dic.Add("社保公司(付)", "iSocialSecurityCompanyPay");
                dic.Add("社保个人(付)", "iSocialSecurityPersonalPay");
                dic.Add("公积金个人(付) ", "iProvidentFundPersonalPay");
                dic.Add("公积金公司(付)", "iProvidentFundCompanyPay");
                dic.Add("社保补缴", "iSocialSecurityAdditional");
                dic.Add("公积金补缴", "iProvidentFundAdditional");
                dic.Add("返费", "iReturnFee");
                dic.Add("临时工费用", "iTemporaryFee");
                dic.Add("个人社保(退)", "iDisabilityBenefitsPay");
                //dic.Add("班车费(付)", "iBusFee");
                dic.Add("餐费(付)", "iMealFee");
                dic.Add("宿舍费(付)", "iDormitoryFeePay");
                dic.Add("商保费(付)", "iCommercialInsuranceFeePay");
                //dic.Add("奖金类(付)", "iBonusPay");
                //dic.Add("赔付款(付)", "iCompensationPay");
                dic.Add("税金", "iTaxFee");
                //dic.Add("代理费", "iAgentFee");
                //dic.Add("劳保费支出", "iLaborInsuranceFee");
                //dic.Add("租金", "iRentFee");
                dic.Add("个调税", "iPersonalTax");
                //dic.Add("水电物管费", "iUtilitiesFee");
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

                dic.Add("派遣工工资(收)", "iSalaryIn");
                dic.Add("临时工工资(收)", "iTemporarySalaryIn");
                dic.Add("外包工工资(收)", "iOutSourceSalaryIn");
                dic.Add("管理费", "iManageFee");
                //dic.Add("社保(收)", "iSocialSecurityIn");
                dic.Add("个人社保(收)", "iSocialSecurityInP");
                dic.Add("公司社保(收)", "iSocialSecurityInC");
                dic.Add("公积金(收)", "iProvidentFundIn");
                dic.Add("残保(收)", "iDisabilityBenefitsIn");
                dic.Add("宿舍费(收)", "iDormitoryFeeIn");
                dic.Add("其它(收)", "iOtherIn");
                dic.Add("收款备注", "iInNote");
                dic.Add("派遣工工资(付)", "iSalaryOut");
                dic.Add("临时工工资(付)", "iTemporarySalaryOut");
                dic.Add("外包工工资(付)", "iOutSourceSalaryOut");
                dic.Add("社保公司(付)", "iSocialSecurityCompanyPay");
                dic.Add("社保个人(付)", "iSocialSecurityPersonalPay");
                dic.Add("公积金个人(付) ", "iProvidentFundPersonalPay");
                dic.Add("公积金公司(付)", "iProvidentFundCompanyPay");
                dic.Add("社保补缴", "iSocialSecurityAdditional");
                dic.Add("公积金补缴", "iProvidentFundAdditional");
                dic.Add("返费", "iReturnFee");
                dic.Add("个人社保(退)", "iDisabilityBenefitsPay");
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
            string insertTmp = "insert into FinanceSummary (iGuid, iCompanyId, iProjectId, iDate, [iSocialSecurityCompanyPay],[iSocialSecurityPersonalPay],[iProvidentFundPersonalPay],[iProvidentFundCompanyPay],[iSocialSecurityAdditional] ,[iReturnFee], [iOfficePay],[iTemporaryFee], [iCreatedOn],[iCreatedBy] ,[iUpdatedOn] ,[iUpdatedBy],[iStatus],[iIsDeleted]) values(newid(),'{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10},getdate(),'system',getdate(),'system',1,0 )";


            string ssSql = "SELECT b.iCompany, b.iItemName, sum([iIndividualAmount]),sum([iCompanyAmount]) ,sum([iAdditionalAmount]) FROM [SocialSecurityDetail] a inner join hrinfo b on a.iHRInfoGuid = b.iGuid and a.iisdeleted=0 and b.iisdeleted=0 where a.iPayMonth=" + month + " group by b.iCompany, b.iItemName";
            string pfSql = "SELECT b.iCompany, b.iItemName, sum([iIndividualAmount]),sum([iCompanyAmount]) ,sum([iAdditionalAmount]) FROM [ProvidentFundDetail] a inner join hrinfo b on a.iHRInfoGuid = b.iGuid and a.iisdeleted=0 and b.iisdeleted=0 where a.iPayMonth=" + month + " group by b.iCompany, b.iItemName";
            string rfSql = "select hr.iCompany, hr.iItemName, sum(cast(rh.iReturnFeeAmount as decimal(6,2))) FROM [ReturnFeeHistory] rh inner join dbo.HRInfo hr on rh.iHRInfoGuid=hr.iGuid and rh.iisdeleted=0 and hr.iisdeleted=0  where left(CONVERT(varchar(100), rh.iReturnFeeActualPayDate, 112),6) ='" + month + "' and iReturnFeePayment='已付' group by hr.iCompany, hr.iItemName";
            string jrSql = "SELECT [iCompanyId] ,[iProjectId] ,sum([iAmount]) FROM [Journal] where left(CONVERT(varchar(100), iPaidDate, 112),6) ='" + month + "' and iChecked='是' and iisdeleted=0 and iType<>'临时工费用' group by iCompanyId, iProjectId";
            string jrTmpSql = "SELECT [iCompanyId] ,[iProjectId] ,sum([iAmount]) FROM [Journal] where left(CONVERT(varchar(100), iPaidDate, 112),6) ='" + month + "' and iChecked='是' and iisdeleted=0 and iType='临时工费用' group by iCompanyId, iProjectId";
            DataTable ssdt = DbHelperSQL.Query(ssSql).Tables[0];
            DataTable pfdt = DbHelperSQL.Query(pfSql).Tables[0];
            DataTable rfdt = DbHelperSQL.Query(rfSql).Tables[0];
            DataTable jrdt = DbHelperSQL.Query(jrSql).Tables[0];
            DataTable jrtmpdt = DbHelperSQL.Query(jrTmpSql).Tables[0];
            Dictionary<string, string> ssCompanyDic = new Dictionary<string, string>();
            ssCompanyDic = ssdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[3].ToString());
            Dictionary<string, string> ssPersonalDic = new Dictionary<string, string>();
            ssPersonalDic = ssdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[2].ToString());
            Dictionary<string, string> ssAditionalDic = new Dictionary<string, string>();
            ssAditionalDic = ssdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[4].ToString());


            Dictionary<string, string> pfPersonallDic = new Dictionary<string, string>();
            pfPersonallDic = pfdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[2].ToString());
            Dictionary<string, string> pfCompanyDic = new Dictionary<string, string>();
            pfCompanyDic = pfdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[3].ToString());


            Dictionary<string, string> rfDic = new Dictionary<string, string>();
            rfDic = rfdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[2].ToString());
            Dictionary<string, string> jrDic = new Dictionary<string, string>();
            jrDic = jrdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[2].ToString());
            Dictionary<string, string> jrtmpDic = new Dictionary<string, string>();
            jrtmpDic = jrtmpdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[2].ToString());

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string company = dr[0].ToString();
                string project = dr[1].ToString();
                string ssCompanyPay = ssCompanyDic.ContainsKey(company + "|" + project) ? ssCompanyDic[company + "|" + project] : "0";
                string ssPersonalPay = ssPersonalDic.ContainsKey(company + "|" + project) ? ssPersonalDic[company + "|" + project] : "0";
                string pfPersonalPay = pfPersonallDic.ContainsKey(company + "|" + project) ? pfPersonallDic[company + "|" + project] : "0";
                string pfCompanyPay = pfCompanyDic.ContainsKey(company + "|" + project) ? pfCompanyDic[company + "|" + project] : "0";
                string ssAdditional = ssAditionalDic.ContainsKey(company + "|" + project) ? ssAditionalDic[company + "|" + project] : "0";
                string returnFee = rfDic.ContainsKey(company + "|" + project) ? rfDic[company + "|" + project] : "0";
                string officePay = jrDic.ContainsKey(company + "|" + project) ? jrDic[company + "|" + project] : "0";
                string tempPay = jrtmpDic.ContainsKey(company + "|" + project) ? jrtmpDic[company + "|" + project] : "0";
                insertSqls.Add(string.Format(insertTmp, company, project, month, ssCompanyPay, ssPersonalPay, pfPersonalPay, pfCompanyPay, ssAdditional, returnFee, officePay, tempPay));
            }
            DbHelperSQL.ExecuteSql("delete from FinanceSummary where iDate =" + month.ToString());
            affectedRowCount = DbHelperSQL.ExecuteSqlTran(insertSqls);
            return affectedRowCount;
        }
        public int UpdateFinanceSummaryByMonth(int month)
        {
            int affectedRowCount = 0;
            List<string> exeSqls = new List<string>();

            string insertTmp = "insert into FinanceSummary (iGuid, iCompanyId, iProjectId, iDate, [iSocialSecurityCompanyPay],[iSocialSecurityPersonalPay],[iProvidentFundPersonalPay],[iProvidentFundCompanyPay],[iSocialSecurityAdditional] ,[iReturnFee], [iOfficePay],[iTemporaryFee], [iSalaryOut],[iOutSourceSalaryOut],[iTemporarySalaryOut],[iProvidentFundAdditional] ,[iCreatedOn],[iCreatedBy] ,[iUpdatedOn] ,[iUpdatedBy],[iStatus],[iIsDeleted]) values(newid(),'{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},getdate(),'system',getdate(),'system',1,0 )";

            string updateTmp = "update FinanceSummary set [iSocialSecurityCompanyPay]={3},[iSocialSecurityPersonalPay]={4},[iProvidentFundPersonalPay]={5},[iProvidentFundCompanyPay]={6},[iSocialSecurityAdditional]={7} ,[iReturnFee]={8}, [iOfficePay]={9},[iTemporaryFee]={10},[iSalaryOut]={11},[iOutSourceSalaryOut]={12},[iTemporarySalaryOut]={13},[iProvidentFundAdditional] ={14} where iCompanyId='{1}' and iProjectId= '{2}' and iDate={0}";


            string sql = @"SELECT distinct [iCompanyId] ,[iProjectId] FROM [SysCompanyProjectRelation] m inner join SysCompany a on a.iGuid=m.iCompanyId and a.iIsDeleted=0
              inner join SysProject b on b.iGuid=m.iProjectId and b.iIsDeleted=0
              where m.iIsDeleted=0 and m.iStatus=1";
            DataSet ds = DbHelperSQL.Query(sql);
            if (month.ToString() == DateTime.Now.ToString("yyyyMM")) //当前月，生成空的新数据
            {
                DataSet dsOld = DbHelperSQL.Query("select iCompanyId, iProjectId  from FinanceSummary where iIsDeleted=0 and iStatus=1 and iDate=" + month.ToString());
                Dictionary<string, string> dicOld = dsOld.Tables[0].Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[0].ToString());
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (!dicOld.ContainsKey(dr[0].ToString() + "|" + dr[1].ToString()))
                    {
                        exeSqls.Add(string.Format(insertTmp, dr[0].ToString(), dr[1].ToString(), month.ToString(), "Null", "Null", "Null", "Null", "Null", "Null", "Null", "Null","Null","Null","Null","Null"));
                    }
                }
                if (exeSqls.Count == 0)
                {
                    return 1;
                }
            }
            else  //非当前月，更新历史数据
            {
                //如果从来没跑过空的，需要先生成空的再更新，如果跑过，不完整，需要先添加完整
                DataSet dsOld = DbHelperSQL.Query("select iCompanyId, iProjectId  from FinanceSummary where iIsDeleted=0 and iStatus=1 and iDate=" + month.ToString());
                Dictionary<string, string> dicOld = dsOld.Tables[0].Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[0].ToString());
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (!dicOld.ContainsKey(dr[0].ToString() + "|" + dr[1].ToString()))
                    {
                        exeSqls.Add(string.Format(insertTmp, dr[0].ToString(), dr[1].ToString(), month.ToString(), "Null", "Null", "Null", "Null", "Null", "Null", "Null", "Null", "Null", "Null", "Null", "Null"));
                    }
                }
                string ssSql = "SELECT b.iCompany, b.iItemName, sum([iIndividualAmount]),sum([iCompanyAmount]) ,sum([iAdditionalAmount]) FROM [SocialSecurityDetail] a inner join hrinfo b on a.iHRInfoGuid = b.iGuid and a.iisdeleted=0 and b.iisdeleted=0 where a.iPayMonth=" + month + " group by b.iCompany, b.iItemName";
                string pfSql = "SELECT b.iCompany, b.iItemName, sum([iIndividualAmount]),sum([iCompanyAmount]) ,sum([iAdditionalAmount]) FROM [ProvidentFundDetail] a inner join hrinfo b on a.iHRInfoGuid = b.iGuid and a.iisdeleted=0 and b.iisdeleted=0 where a.iPayMonth=" + month + " group by b.iCompany, b.iItemName";
                string rfSql = "select hr.iCompany, hr.iItemName, sum(cast(rh.iReturnFeeAmount as decimal(6,2))) FROM [ReturnFeeHistory] rh inner join dbo.HRInfo hr on rh.iHRInfoGuid=hr.iGuid and rh.iisdeleted=0 and hr.iisdeleted=0  where left(CONVERT(varchar(100), rh.iReturnFeeActualPayDate, 112),6) ='" + month + "' and iReturnFeePayment='已付' group by hr.iCompany, hr.iItemName";
                string jrSql = "SELECT [iCompanyId] ,[iProjectId] ,sum([iAmount]) FROM [Journal] where left(CONVERT(varchar(100), iPaidDate, 112),6) ='" + month + "' and iChecked='是' and iisdeleted=0 and iType<>'临时工费用' group by iCompanyId, iProjectId";
                string jrTmpSql = "SELECT [iCompanyId] ,[iProjectId] ,sum([iAmount]) FROM [Journal] where left(CONVERT(varchar(100), iPaidDate, 112),6) ='" + month + "' and iChecked='是' and iisdeleted=0 and iType='临时工费用' group by iCompanyId, iProjectId";
                string salaryOutSql = "SELECT [iCompanyId] ,[iProjectId] ,sum([iTotal]) FROM  [Salary] where left(CONVERT(varchar(100), iYearMonth, 112),6) ='" + month + "' and iApproveStatus='已审核' and iisdeleted=0 and iCategory='派遣工' group by iCompanyId, iProjectId";
                string osSalaryOutSql = "SELECT [iCompanyId] ,[iProjectId] ,sum([iTotal]) FROM  [Salary] where left(CONVERT(varchar(100), iYearMonth, 112),6) ='" + month + "' and iApproveStatus='已审核' and iisdeleted=0 and iCategory='外包工' group by iCompanyId, iProjectId";
                string temSalaryOutSql = "SELECT [iCompanyId] ,[iProjectId] ,sum([iTotal]) FROM  [Salary] where left(CONVERT(varchar(100), iYearMonth, 112),6) ='" + month + "' and iApproveStatus='已审核' and iisdeleted=0 and iCategory='临时工' group by iCompanyId, iProjectId";
                DataTable ssdt = DbHelperSQL.Query(ssSql).Tables[0];
                DataTable pfdt = DbHelperSQL.Query(pfSql).Tables[0];
                DataTable rfdt = DbHelperSQL.Query(rfSql).Tables[0];
                DataTable jrdt = DbHelperSQL.Query(jrSql).Tables[0];
                DataTable jrtmpdt = DbHelperSQL.Query(jrTmpSql).Tables[0];
                DataTable salarydt = DbHelperSQL.Query(salaryOutSql).Tables[0];
                DataTable osSalarydt = DbHelperSQL.Query(osSalaryOutSql).Tables[0];
                DataTable temSalarydt = DbHelperSQL.Query(temSalaryOutSql).Tables[0];
                Dictionary<string, string> ssCompanyDic = new Dictionary<string, string>();
                ssCompanyDic = ssdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[3].ToString());
                Dictionary<string, string> ssPersonalDic = new Dictionary<string, string>();
                ssPersonalDic = ssdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[2].ToString());
                Dictionary<string, string> ssAditionalDic = new Dictionary<string, string>();
                ssAditionalDic = ssdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[4].ToString());


                Dictionary<string, string> pfPersonallDic = new Dictionary<string, string>();
                pfPersonallDic = pfdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[2].ToString());
                Dictionary<string, string> pfCompanyDic = new Dictionary<string, string>();
                pfCompanyDic = pfdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[3].ToString());
                Dictionary<string, string> pfAditionalDic = new Dictionary<string, string>();
                pfAditionalDic = pfdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[4].ToString());


                Dictionary<string, string> rfDic = new Dictionary<string, string>();
                rfDic = rfdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[2].ToString());
                Dictionary<string, string> jrDic = new Dictionary<string, string>();
                jrDic = jrdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[2].ToString());
                Dictionary<string, string> jrtmpDic = new Dictionary<string, string>();
                jrtmpDic = jrtmpdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[2].ToString());
                Dictionary<string, string> salaryDic = new Dictionary<string, string>();
                salaryDic = salarydt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[2].ToString());
                Dictionary<string, string> osSalaryDic = new Dictionary<string, string>();
                osSalaryDic = osSalarydt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[2].ToString());
                Dictionary<string, string> temSalaryDic = new Dictionary<string, string>();
                temSalaryDic = temSalarydt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[2].ToString());

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string company = dr[0].ToString();
                    string project = dr[1].ToString();
                    string ssCompanyPay = ssCompanyDic.ContainsKey(company + "|" + project) ? ssCompanyDic[company + "|" + project] : "0";
                    string ssPersonalPay = ssPersonalDic.ContainsKey(company + "|" + project) ? ssPersonalDic[company + "|" + project] : "0";
                    string pfPersonalPay = pfPersonallDic.ContainsKey(company + "|" + project) ? pfPersonallDic[company + "|" + project] : "0";
                    string pfCompanyPay = pfCompanyDic.ContainsKey(company + "|" + project) ? pfCompanyDic[company + "|" + project] : "0";
                    string ssAdditional = ssAditionalDic.ContainsKey(company + "|" + project) ? ssAditionalDic[company + "|" + project] : "0";
                    string pfAdditional = pfAditionalDic.ContainsKey(company + "|" + project) ? pfAditionalDic[company + "|" + project] : "0";
                    string returnFee = rfDic.ContainsKey(company + "|" + project) ? rfDic[company + "|" + project] : "0";
                    string officePay = jrDic.ContainsKey(company + "|" + project) ? jrDic[company + "|" + project] : "0";
                    string tempPay = jrtmpDic.ContainsKey(company + "|" + project) ? jrtmpDic[company + "|" + project] : "0";

                    string salaryPay = salaryDic.ContainsKey(company + "|" + project) ? salaryDic[company + "|" + project] : "0";
                    string osSalaryPay = osSalaryDic.ContainsKey(company + "|" + project) ? osSalaryDic[company + "|" + project] : "0";
                    string temSalaryPay = temSalaryDic.ContainsKey(company + "|" + project) ? temSalaryDic[company + "|" + project] : "0";

                    exeSqls.Add(string.Format(updateTmp, month.ToString(), company, project, string.IsNullOrEmpty(ssCompanyPay) ? "0" : ssCompanyPay, string.IsNullOrEmpty(ssPersonalPay) ? "0" : ssPersonalPay, string.IsNullOrEmpty(pfPersonalPay) ? "0" : pfPersonalPay, string.IsNullOrEmpty(pfCompanyPay) ? "0" : pfCompanyPay, string.IsNullOrEmpty(ssAdditional) ? "0" : ssAdditional, string.IsNullOrEmpty(returnFee) ? "0" : returnFee, string.IsNullOrEmpty(officePay) ? "0" : officePay, string.IsNullOrEmpty(tempPay) ? "0" : tempPay, string.IsNullOrEmpty(salaryPay) ? "0" : salaryPay, string.IsNullOrEmpty(osSalaryPay) ? "0" : osSalaryPay, string.IsNullOrEmpty(temSalaryPay) ? "0" : temSalaryPay, string.IsNullOrEmpty(pfAdditional) ? "0" : pfAdditional));
                }
            }
            affectedRowCount = DbHelperSQL.ExecuteSqlTran(exeSqls);
            return affectedRowCount;

        }
        public int UpdateFinanceSummaryByMonthOld(int month)
        {
            int affectedRowCount = 0;
            string sql = @"SELECT distinct [iCompanyId] ,[iProjectId] FROM [SysCompanyProjectRelation] where iIsDeleted=0 and iStatus=1";
            DataSet ds = DbHelperSQL.Query(sql);
            List<string> updateSqls = new List<string>();
            string updateTmp = "update FinanceSummary set [iSocialSecurityCompanyPay]={0},[iSocialSecurityPersonalPay]={1},[iProvidentFundPersonalPay]={2},[iProvidentFundCompanyPay]={3},[iSocialSecurityAdditional]={4} ,[iReturnFee]={5}, [iOfficePay]={6} where iCompanyId='{7}' and iProjectId= '{8}' and iDate=" + month;


            string ssSql = "SELECT b.iCompany, b.iItemName, sum([iIndividualAmount]),sum([iCompanyAmount]) ,sum([iAdditionalAmount]) FROM [SocialSecurityDetail] a inner join hrinfo b on a.iHRInfoGuid = b.iGuid and a.iisdeleted=0 and b.iisdeleted=0 where a.iPayMonth=" + month + " group by b.iCompany, b.iItemName";
            string pfSql = "SELECT b.iCompany, b.iItemName, sum([iIndividualAmount]),sum([iCompanyAmount]) ,sum([iAdditionalAmount]) FROM [ProvidentFundDetail] a inner join hrinfo b on a.iHRInfoGuid = b.iGuid and a.iisdeleted=0 and b.iisdeleted=0 where a.iPayMonth=" + month + " group by b.iCompany, b.iItemName";
            string rfSql = "select hr.iCompany, hr.iItemName, sum(cast(rh.iReturnFeeAmount as decimal(6,2))) FROM [ReturnFeeHistory] rh inner join dbo.HRInfo hr on rh.iHRInfoGuid=hr.iGuid and rh.iisdeleted=0 and hr.iisdeleted=0  where left(CONVERT(varchar(100), rh.iReturnFeeActualPayDate, 112),6) ='" + month + "' and iReturnFeePayment='已付' group by hr.iCompany, hr.iItemName";
            string jrSql = "SELECT [iCompanyId] ,[iProjectId] ,sum([iAmount]) FROM [Journal] where left(CONVERT(varchar(100), iPaidDate, 112),6) ='" + month + "' and iChecked='是' and iisdeleted=0 group by iCompanyId, iProjectId";
            DataTable ssdt = DbHelperSQL.Query(ssSql).Tables[0];
            DataTable pfdt = DbHelperSQL.Query(pfSql).Tables[0];
            DataTable rfdt = DbHelperSQL.Query(rfSql).Tables[0];
            DataTable jrdt = DbHelperSQL.Query(jrSql).Tables[0];
            Dictionary<string, string> ssCompanyDic = new Dictionary<string, string>();
            ssCompanyDic = ssdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[3].ToString());
            Dictionary<string, string> ssPersonalDic = new Dictionary<string, string>();
            ssPersonalDic = ssdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[2].ToString());
            Dictionary<string, string> ssAditionalDic = new Dictionary<string, string>();
            ssAditionalDic = ssdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[4].ToString());


            Dictionary<string, string> pfPersonallDic = new Dictionary<string, string>();
            pfPersonallDic = pfdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[2].ToString());
            Dictionary<string, string> pfCompanyDic = new Dictionary<string, string>();
            pfCompanyDic = pfdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[3].ToString());


            Dictionary<string, string> rfDic = new Dictionary<string, string>();
            rfDic = rfdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[2].ToString());
            Dictionary<string, string> jrDic = new Dictionary<string, string>();
            jrDic = jrdt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString() + "|" + x[1].ToString(), x => x[2].ToString());

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string company = dr[0].ToString();
                string project = dr[1].ToString();
                string ssCompanyPay = ssCompanyDic.ContainsKey(company + "|" + project) ? ssCompanyDic[company + "|" + project] : "0";
                string ssPersonalPay = ssPersonalDic.ContainsKey(company + "|" + project) ? ssPersonalDic[company + "|" + project] : "0";
                string pfPersonalPay = pfPersonallDic.ContainsKey(company + "|" + project) ? pfPersonallDic[company + "|" + project] : "0";
                string pfCompanyPay = pfCompanyDic.ContainsKey(company + "|" + project) ? pfCompanyDic[company + "|" + project] : "0";
                string ssAdditional = ssAditionalDic.ContainsKey(company + "|" + project) ? ssAditionalDic[company + "|" + project] : "0";
                string returnFee = rfDic.ContainsKey(company + "|" + project) ? rfDic[company + "|" + project] : "0";
                string officePay = jrDic.ContainsKey(company + "|" + project) ? jrDic[company + "|" + project] : "0";
                updateSqls.Add(string.Format(updateTmp, ssCompanyPay, ssPersonalPay, pfPersonalPay, pfCompanyPay, ssAdditional, returnFee, officePay, company, project));
            }
            affectedRowCount = DbHelperSQL.ExecuteSqlTran(updateSqls);
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