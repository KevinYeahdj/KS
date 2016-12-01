using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinBrain.Data.Entity
{
    //公积金 信息
    [Table("ProvidentFund")]
    public class ProvidentFundEntity
    {
        public string iGuid { get; set; } //惟一标识
        public string iHRInfoGuid { get; set; } //人事信息标识
        public string iPayPlace { get; set; } //缴纳地      
        public string iEmployeeWilling { get; set; } //员工意愿
        //public string iIsPaid { get; set; } //是否缴纳
        public Decimal? iPayBase { get; set; } //缴费基数
        public DateTime? iEntryDate { get; set; } //转入日期
        public DateTime? iSealDate { get; set; } //封存日期

        public Decimal? iIndividualAmount { get; set; } //个人缴费金额
        public Decimal? iCompanyAmount { get; set; } //公司缴费金额 
        public Decimal? iAdditionalAmount { get; set; } //补缴金额

        public int? iAdditionalMonths { get; set; } //补缴月数
        //public string iIsSocialInsurancePaid { get; set; } //是否缴纳商业保险 保存在人事信息里面


        public string iSocialInsurancePaidCompany { get; set; } //商业保险缴纳公司
        public string iNote1 { get; set; } //备注1
        public string iNote2 { get; set; } //备注2

        public DateTime? iCreatedOn { get; set; }  //创建时间
        public string iCreatedBy { get; set; } //创建人
        public DateTime? iUpdatedOn { get; set; } //修改时间
        public string iUpdatedBy { get; set; } //修改人
        public int iStatus { get; set; }  //状态
        public int iIsDeleted { get; set; } //假删

    }

    public class ProvidentFundModel
    {

        public string iGuid { get; set; } //惟一标识

        public string iHRInfoGuid { get; set; } //人事标识外键

        public string iHRInfoGuid2 { get; set; } //人事标识
        public string iItemName { get; set; } //项目名称
        public string iCompany { get; set; } //所在公司 
        public string iEmpNo { get; set; } //工号 
        public string iName { get; set; } //姓名 
        public string iIdCard { get; set; } //身份证号 

        public string iLaborName { get; set; } //劳务名称 
        public string iLaborCampBank { get; set; } //劳务所银行支行
        public string iLaborCampBankAccount { get; set; } //劳务所账号
        public string iLaborCampBankPerson { get; set; } //劳务所人姓名
        public DateTime? iInterviewDate { get; set; } //面试日期

        public DateTime? iEmployeeDate { get; set; } //入职时间 

        public DateTime? iResignDate { get; set; } //离职日期 

        public string iOnJobDays { get; set; } //在职天数
        public string iFirstReturnFeeAmount { get; set; } //一级返费金额 
        public string iFirstReturnFeeDays { get; set; } //一级返费天数
        public DateTime? iFirstReturnFeeDate { get; set; } //一级返费日期
        public string iFirstReturnFeePayment { get; set; } //一级付款情况 
        public DateTime? iFirstReturnFeeActualPayDate { get; set; } //一级实际支付日期

        public string iSecondReturnFeeAmount { get; set; } //二级返费金额 
        public string iSecondReturnFeeDays { get; set; } //二级返费天数
        public DateTime? iSecondReturnFeeDate { get; set; } //二级返费日期
        public string iSecondReturnFeePayment { get; set; } //二级付款情况 
        public DateTime? iSecondReturnFeeActualPayDate { get; set; } //二级实际支付日期

        public string iThirdReturnFeeAmount { get; set; } //三级返费金额 
        public string iThirdReturnFeeDays { get; set; } //三级返费天数
        public DateTime? iThirdReturnFeeDate { get; set; } //三级返费日期
        public string iThirdReturnFeePayment { get; set; } //三级付款情况 
        public DateTime? iThirdReturnFeeActualPayDate { get; set; } //三级实际支付日期

        public string iFourthReturnFeeAmount { get; set; } //四级返费金额 
        public string iFourthReturnFeeDays { get; set; } //四级返费天数
        public DateTime? iFourthReturnFeeDate { get; set; } //四级返费日期
        public string iFourthReturnFeePayment { get; set; } //四级付款情况 
        public DateTime? iFourthReturnFeeActualPayDate { get; set; } //四级实际支付日期

        public string iFifthReturnFeeAmount { get; set; } //五级返费金额 
        public string iFifthReturnFeeDays { get; set; } //五级返费天数
        public DateTime? iFifthReturnFeeDate { get; set; } //五级返费日期
        public string iFifthReturnFeePayment { get; set; } //五级付款情况 
        public DateTime? iFifthReturnFeeActualPayDate { get; set; } //五级实际支付日期

        public string iReturnFeeNote { get; set; } // 返费信息备注
        public DateTime? iCreatedOn { get; set; }  //创建时间
        public string iCreatedBy { get; set; } //创建人
        public DateTime? iUpdatedOn { get; set; } //修改时间
        public string iUpdatedBy { get; set; } //修改人
        public int iStatus { get; set; }  //状态
        public int iIsDeleted { get; set; } //假删
        
    }
}
