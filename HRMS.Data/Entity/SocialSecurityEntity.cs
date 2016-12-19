using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Data.Entity
{
    //社保 信息
    [Table("SocialSecurity")]
    public class SocialSecurityEntity
    {
        public string iGuid { get; set; } //惟一标识
        public string iHRInfoGuid { get; set; } //人事信息标识
        public string iPayPlace { get; set; } //缴纳地
        public string iEmployeeWilling { get; set; } //员工意愿

        //public string iIsPaid { get; set; } //是否缴纳 保存在人事信息里面 
        public Decimal? iPayBase { get; set; } //缴费基数
        public DateTime? iFirstEntryDate { get; set; } //入保日期
        public DateTime? iFirstRecruitDate { get; set; } //招工日期 
        public DateTime? iFirstExitDate { get; set; } //退保日期
        public DateTime? iFirstResignDate { get; set; } //退工日期
        public DateTime? iSecondRecruitDate { get; set; } //二次招工日期 
        public DateTime? iSecondResignDate { get; set; } //二次退工日期

        public DateTime? iSecondEntryDate { get; set; } //二次入保日期
        public DateTime? iSecondExitDate { get; set; } //二次退保日期
        public Decimal? iIndividualAmount { get; set; } //个人缴费金额
        public Decimal? iCompanyAmount { get; set; } //公司缴费金额 
        public Decimal? iAdditionalAmount { get; set; } //补缴金额

        public int? iAdditionalMonths { get; set; } //补缴月数
        public string iNote1 { get; set; } //备注1
        public string iNote2 { get; set; } //备注2
        public DateTime? iCreatedOn { get; set; }  //创建时间
        public string iCreatedBy { get; set; } //创建人
        public DateTime? iUpdatedOn { get; set; } //修改时间
        public string iUpdatedBy { get; set; } //修改人
        public int iStatus { get; set; }  //状态
        public int iIsDeleted { get; set; } //假删

    }

    public class SocialSecurityModel
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

        public string iIsSocialInsurancePaid { get; set; } //是否缴纳 
        public Decimal? iPayBase { get; set; } //缴费基数

        public DateTime? iFirstEntryDate { get; set; } //入保日期
        public DateTime? iFirstRecruitDate { get; set; } //招工日期 
        public DateTime? iFirstExitDate { get; set; } //退保日期
        public DateTime? iFirstResignDate { get; set; } //退工日期
        public DateTime? iSecondRecruitDate { get; set; } //二次招工日期 
        public DateTime? iSecondResignDate { get; set; } //二次退工日期

        public DateTime? iSecondEntryDate { get; set; } //二次入保日期
        public DateTime? iSecondExitDate { get; set; } //二次退保日期
        public Decimal? iIndividualAmount { get; set; } //个人缴费金额
        public Decimal? iCompanyAmount { get; set; } //公司缴费金额 
        public Decimal? iAdditionalAmount { get; set; } //补缴金额

        public int? iAdditionalMonths { get; set; } //补缴月数

        public Decimal? iTotal { get; set; } //社保总计
        public string iNote1 { get; set; } //备注1
        public string iNote2 { get; set; } //备注2
        public DateTime? iCreatedOn { get; set; }  //创建时间
        public string iCreatedBy { get; set; } //创建人
        public DateTime? iUpdatedOn { get; set; } //修改时间
        public string iUpdatedBy { get; set; } //修改人
        public int iStatus { get; set; }  //状态
        public int iIsDeleted { get; set; } //假删
        
    }

    //社保明细 信息
    [Table("SocialSecurityDetail")]
    public class SocialSecurityDetailEntity
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
    public class SocialSecurityDetailModel
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
