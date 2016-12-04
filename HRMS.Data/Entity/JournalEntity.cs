using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinBrain.Data.Entity
{
    //流水账 信息
    [Table("Journal")]
    public class JournalEntity
    {
        public string iGuid { get; set; } //惟一标识
        public string iCompanyId { get; set; } //公司
        public string iProjectId { get; set; } //项目
        public DateTime? iDate { get; set; } //日期      
        public string iEvent { get; set; } //事项
        public string iType { get; set; } //科目
        public string iApplicant { get; set; } //提报人
        public decimal? iAmount { get; set; } //金额
        public string iChecked { get; set; } //是否核销
        public string iCheckedBy { get; set; } //核销人
        public DateTime? iPaidDate { get; set; } //支付日期
        public string iNote { get; set; } //备注 

        public DateTime? iCreatedOn { get; set; }  //创建时间
        public string iCreatedBy { get; set; } //创建人
        public DateTime? iUpdatedOn { get; set; } //修改时间
        public string iUpdatedBy { get; set; } //修改人
        public int iStatus { get; set; }  //状态
        public int iIsDeleted { get; set; } //假删

    }
}
