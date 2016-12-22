using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Data.Entity
{
    //公积金 信息
    [Table("ProvidentFund")]
    public class ProvidentFundEntity
    {
        public string iGuid { get; set; } //惟一标识
        public string iHRInfoGuid { get; set; } //人事信息标识
        public string iPayPlace { get; set; } //缴纳地      
        public string iEmployeeWilling { get; set; } //员工意愿  --和是否缴纳是一回事，暂时不使用
        //public string iIsPaid { get; set; } //是否缴纳
        public Decimal? iPayBase { get; set; } //缴费基数
        public DateTime? iEntryDate { get; set; } //转入日期
        public DateTime? iSealDate { get; set; } //封存日期

        public Decimal? iIndividualAmount { get; set; } //个人缴费金额
        public Decimal? iCompanyAmount { get; set; } //公司缴费金额 
        public Decimal? iAdditionalAmount { get; set; } //补缴金额

        public int? iAdditionalMonths { get; set; } //补缴月数
        //public string iIsCommercialInsurancePaid { get; set; } //是否缴纳商业保险 保存在人事信息里面


        public string iCommercialInsurancePaidCompany { get; set; } //商业保险缴纳公司
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

        public string iHRInfoGuid2 { get; set; } //人事标识(一定存在)
        public string iItemName { get; set; } //项目名称
        public string iCompany { get; set; } //所在公司 
        public string iEmpNo { get; set; } //工号 
        public string iName { get; set; } //姓名 
        public string iIdCard { get; set; } //身份证号 

        public DateTime? iEmployeeDate { get; set; } //入职日期
        public DateTime? iResignDate { get; set; }  //离职日期

        public string iEmployeeStatus { get; set; } //员工状态

        public string iResidenceProperty { get; set; } //户籍类型
        public string iPayPlace { get; set; } //缴纳地
        public string iEmployeeWilling { get; set; } //员工意愿

        public string iIsProvidentPaid { get; set; } //是否缴纳 
        public Decimal? iPayBase { get; set; } //缴费基数
        public DateTime? iEntryDate { get; set; } //转入日期
        public DateTime? iSealDate { get; set; } //封存日期
        public Decimal? iIndividualAmount { get; set; } //个人缴费金额
        public Decimal? iCompanyAmount { get; set; } //公司缴费金额 
        public Decimal? iAdditionalAmount { get; set; } //补缴金额

        public int? iAdditionalMonths { get; set; } //补缴月数

        public Decimal? iTotal { get; set; } //公积金总计
        public string iIsCommercialInsurancePaid { get; set; } //是否缴纳商业保险
        public string iCommercialInsurancePaidCompany { get; set; } //商业保险缴纳公司
        public string iNote1 { get; set; } //备注1
        public string iNote2 { get; set; } //备注2
        public DateTime? iCreatedOn { get; set; }  //创建时间
        public string iCreatedBy { get; set; } //创建人
        public DateTime? iUpdatedOn { get; set; } //修改时间
        public string iUpdatedBy { get; set; } //修改人
        public int iStatus { get; set; }  //状态
        public int iIsDeleted { get; set; } //假删

    }

    //公积金明细 信息
    [Table("ProvidentFundDetail")]
    public class ProvidentFundDetailEntity
    {
        public string iGuid { get; set; } //惟一标识
        public int? iPayMonth { get; set; } //缴纳月份
        public string iHRInfoGuid { get; set; } //人事信息标识
        public string iPayPlace { get; set; } //缴纳地
        public Decimal? iPayBase { get; set; } //缴费基数
        public Decimal? iIndividualAmount { get; set; } //个人缴费金额
        public Decimal? iCompanyAmount { get; set; } //公司缴费金额 
        public Decimal? iAdditionalAmount { get; set; } //补缴金额
        public int? iAdditionalMonths { get; set; } //补缴月数
        public DateTime? iCreatedOn { get; set; }  //创建时间
        public string iCreatedBy { get; set; } //创建人
        public DateTime? iUpdatedOn { get; set; } //修改时间
        public string iUpdatedBy { get; set; } //修改人
        public int iStatus { get; set; }  //状态
        public int iIsDeleted { get; set; } //假删

    }

    public class ProvidentFundDetailModel
    {
        public string iGuid { get; set; } //惟一标识
        public int? iPayMonth { get; set; } //缴纳月份
        public string iHRInfoGuid { get; set; } //人事标识外键
        public string iItemName { get; set; } //项目名称
        public string iCompany { get; set; } //所在公司 
        public string iEmpNo { get; set; } //工号 
        public string iName { get; set; } //姓名 
        public string iIdCard { get; set; } //身份证号 
        public string iPayPlace { get; set; } //缴纳地
        public Decimal? iPayBase { get; set; } //缴费基数
        public Decimal? iIndividualAmount { get; set; } //个人缴费金额
        public Decimal? iCompanyAmount { get; set; } //公司缴费金额 
        public Decimal? iAdditionalAmount { get; set; } //补缴金额
        public int? iAdditionalMonths { get; set; } //补缴月数
        public Decimal? iTotal { get; set; } //社保总计
        public DateTime? iCreatedOn { get; set; }  //创建时间
        public string iCreatedBy { get; set; } //创建人
        public DateTime? iUpdatedOn { get; set; } //修改时间
        public string iUpdatedBy { get; set; } //修改人
        public int iStatus { get; set; }  //状态
        public int iIsDeleted { get; set; } //假删

    }

}
