using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Data.Entity
{
    //财务汇总 信息
    [Table("FinanceSummary")]
    public class FinanceSummaryEntity
    {
        public string iGuid { get; set; } //惟一标识
        public string iCompanyId { get; set; } //公司
        public string iProjectId { get; set; } //项目
        public int? iDate { get; set; } //日期     年月201701 
        public string iReceivable { get; set; } //收款情况
        public decimal? iInvoice { get; set; } //开票金额

        //public decimal? iDiffer { get; set; } //差异  差异=开票金额-工资（收）-管理费-社保（收）-公积金（收）-残保金（收）-宿舍费（收）-其它（收)
        public decimal? iSalaryIn { get; set; } //派遣工工资(收)  数据来源手填
        public decimal? iManageFee { get; set; } //管理费
        //public decimal? iSocialSecurityIn { get; set; } //社保(收)--拆分成 个人社保(收)，公司社保(收)--个人社保(收)
        public decimal? iSocialSecurityInP { get; set; } //个人社保(收)
        public decimal? iSocialSecurityInC { get; set; } //公司社保(收)
        public decimal? iProvidentFundIn { get; set; } //公积金(收)
        public decimal? iDisabilityBenefitsIn { get; set; } //残保(收)
        public decimal? iDormitoryFeeIn { get; set; } //宿舍费(收)
        public decimal? iOtherIn { get; set; } //其它(收)
        public string iInNote { get; set; } //收款备注 
        public decimal? iSalaryOut { get; set; } //工资(付)  数据来源工资流程
        public decimal? iSocialSecurityCompanyPay { get; set; } //社保公司(付) 
        public decimal? iSocialSecurityPersonalPay { get; set; } //社保个人(付) 
        public decimal? iProvidentFundPersonalPay { get; set; } //公积金个人(付) 
        public decimal? iProvidentFundCompanyPay { get; set; } //公积金公司(付) 
        public decimal? iSocialSecurityAdditional { get; set; } //社保补缴 
        public decimal? iProvidentFundAdditional { get; set; } //公积金补缴 
        public decimal? iReturnFee { get; set; } //返费 
        public decimal? iDisabilityBenefitsPay { get; set; } //残保金(付) --个人社保(退)
        //public decimal? iBusFee { get; set; } //班车费(付) 
        public decimal? iMealFee { get; set; } //餐费(付)
        public decimal? iDormitoryFeePay { get; set; } //宿舍费(付) 
        public decimal? iCommercialInsuranceFeePay { get; set; } //商保费(付)
        //public decimal? iBonusPay { get; set; } //奖金类(付)
        //public decimal? iCompensationPay { get; set; } //赔付款(付)
        public decimal? iTaxFee { get; set; } //税金
        //public decimal? iAgentFee { get; set; } //代理费
        //public decimal? iLaborInsuranceFee { get; set; } //劳保费支出
        //public decimal? iRentFee { get; set; } //租金        
        public decimal? iPersonalTax { get; set; } //个调税
        
        //public decimal? iUtilitiesFee { get; set; } //水电物管费
        public decimal? iOfficePay { get; set; } //办公费
        public decimal? iCompanySalaryPay { get; set; } //公司人员工资
        public decimal? iCompanySocialSecurityPay { get; set; } //公司人员社保
        public decimal? iCompanyProvidentFundPay { get; set; } //公司人员公积金
        public decimal? iInjuryPay { get; set; } //工伤费用
        public decimal? iOtherPay { get; set; } //其它付
        public string iPayNote { get; set; } //支出备注

        //public decimal? iProfit { get; set; } //利润 利润=开票金额-所有付（工资付到其它付）
        public DateTime? iCreatedOn { get; set; }  //创建时间
        public string iCreatedBy { get; set; } //创建人
        public DateTime? iUpdatedOn { get; set; } //修改时间
        public string iUpdatedBy { get; set; } //修改人
        public int iStatus { get; set; }  //状态
        public int iIsDeleted { get; set; } //假删

        public decimal? iTemporarySalaryIn { get; set; } //临时工工资收 数据来源手填
        public decimal? iTemporarySalaryOut { get; set; } //临时工工资付
        public decimal? iTemporaryFee { get; set; } //临时工支出  数据来源工资流程
        public decimal? iOutSourceSalaryIn { get; set; } //外包工工资收 数据来源手填
        public decimal? iOutSourceSalaryOut { get; set; } //外包工工资付 数据来源工资流程

    }
}
