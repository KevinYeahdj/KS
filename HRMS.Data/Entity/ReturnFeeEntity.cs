using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Data.Entity
{
    //返费 信息
    [Table("ReturnFee")]
    public class ReturnFeeEntity
    {
        public string iGuid { get; set; } //惟一标识
        public string iHRInfoGuid { get; set; } //人事信息标识
        public string iAdvice { get; set; } //返费建议
        public string iLaborName { get; set; } //劳务名称 
        public string iLaborCampBank { get; set; } //劳务所银行支行
        public string iLaborCampBankAccount { get; set; } //劳务所账号
        public string iLaborCampBankPerson { get; set; } //劳务所人姓名
        public DateTime? iInterviewDate { get; set; } //面试日期
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

        public string iFirstAppNo { get; set; } // 一级返费流程编号
        public string iSecondAppNo { get; set; } // 二级费流程编号
        public string iThirdAppNo { get; set; } // 三级返费流程编号
        public string iFourthAppNo { get; set; } // 四级返费流程编号
        public string iFifthAppNo { get; set; } // 五级返费流程编号

        public DateTime? iCreatedOn { get; set; }  //创建时间
        public string iCreatedBy { get; set; } //创建人
        public DateTime? iUpdatedOn { get; set; } //修改时间
        public string iUpdatedBy { get; set; } //修改人
        public int iStatus { get; set; }  //状态
        public int iIsDeleted { get; set; } //假删

    }

    public class ReturnFeeModel
    {

        public string iGuid { get; set; } //惟一标识

        public string iHRInfoGuid { get; set; } //人事标识外键

        public string iHRInfoGuid2 { get; set; } //人事标识
        public string iItemName { get; set; } //项目名称
        public string iCompany { get; set; } //所在公司 
        public string iEmpNo { get; set; } //工号 
        public string iName { get; set; } //姓名 
        public string iIdCard { get; set; } //身份证号 

        public string iEmployeeStatus { get; set; }
        public string iAdvice { get; set; } //返费建议
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

        public string iFirstAppNo { get; set; } // 一级返费流程编号
        public string iSecondAppNo { get; set; } // 二级费流程编号
        public string iThirdAppNo { get; set; } // 三级返费流程编号
        public string iFourthAppNo { get; set; } // 四级返费流程编号
        public string iFifthAppNo { get; set; } // 五级返费流程编号

        public DateTime? iCreatedOn { get; set; }  //创建时间
        public string iCreatedBy { get; set; } //创建人
        public DateTime? iUpdatedOn { get; set; } //修改时间
        public string iUpdatedBy { get; set; } //修改人
        public int iStatus { get; set; }  //状态
        public int iIsDeleted { get; set; } //假删

    }

    //返费历史 信息
    [Table("ReturnFeeHistory")]
    public class ReturnFeeHistoryEntity
    {
        public string iGuid { get; set; } //惟一标识
        public string iReturnFeeGuid { get; set; } //返费惟一标识
        public string iHRInfoGuid { get; set; } //人事信息标识
        public string iLaborName { get; set; } //劳务名称 
        public string iLaborCampBank { get; set; } //劳务所银行支行
        public string iLaborCampBankAccount { get; set; } //劳务所账号
        public string iLaborCampBankPerson { get; set; } //劳务所人姓名
        public string iReturnFeeLevel { get; set; } //返费级别
        public string iReturnFeeAmount { get; set; } //返费金额 
        public string iReturnFeeDays { get; set; } //返费天数
        public DateTime? iReturnFeeDate { get; set; } //返费日期
        public string iReturnFeePayment { get; set; } //付款情况 
        public DateTime? iReturnFeeActualPayDate { get; set; } //实际支付日期

        public string iReturnFeeAppNo { get; set; } //返费单号
        //public 
        public DateTime? iCreatedOn { get; set; }  //创建时间
        public string iCreatedBy { get; set; } //创建人
        public DateTime? iUpdatedOn { get; set; } //修改时间
        public string iUpdatedBy { get; set; } //修改人
        public int iStatus { get; set; }  //状态
        public int iIsDeleted { get; set; } //假删

    }

    public class ReturnFeeHistoryViewModel : ReturnFeeHistoryEntity
    {
        public string iCompanyId { get; set; }
        public string iProjectId { get; set; }
        public string iName { get; set; }
        public string iIdCard { get; set; }

        public string iEmployeeStatus { get; set; }

        public DateTime? iInterviewDate { get; set; }
        public DateTime? iEmployeeDate { get; set; }
        public DateTime? iResignDate { get; set; }
    }
}

