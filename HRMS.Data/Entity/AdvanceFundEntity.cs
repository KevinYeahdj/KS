using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Data.Entity
{
    //备用金申请 信息
    [Table("AdvanceFund")]
    public class AdvanceFundEntity
    {
        public string iGuid { get; set; } //惟一标识
        public string iCompanyId { get; set; } //公司
        public string iProjectId { get; set; } //项目  
        public string iReason { get; set; } //事由
        public decimal? iAmount { get; set; } //金额
        public string iChecked { get; set; } //是否核销
        public string iCheckedBy { get; set; } //核销人
        public DateTime? iPaidDate { get; set; } //支付日期
        public string iNote { get; set; } //备注 
        public string iAppNo { get; set; } //流程单号 

        public string iRecordNote { get; set; } //数据说明  --备用金还款 /返费抵冲/流水帐抵冲
        public string iApplicant { get; set; } //申请人工号

        public DateTime? iCreatedOn { get; set; }  //创建时间
        public string iCreatedBy { get; set; } //创建人
        public DateTime? iUpdatedOn { get; set; } //修改时间
        public string iUpdatedBy { get; set; } //修改人
        public int iStatus { get; set; }  //状态
        public int iIsDeleted { get; set; } //假删

    }
}
