using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Data.Entity
{
    //合同 信息
    [Table("Contract")]
    public class ContractEntity
    {
        public string iGuid { get; set; } //惟一标识
        public string iCompanyId { get; set; } //公司
        public string iProjectId { get; set; } //项目
        public DateTime? iStartDate { get; set; } //合同起始日期   
        public DateTime? iEndDate { get; set; } //合同终止日期
        public string iProperty { get; set; } //合同性质
        public string iEnterpriseContact { get; set; } //企业联系人
        public string iEnterpriseTel { get; set; } //企业联系电话
        public string iEnterprisePhone { get; set; } //企业联系手机
        public string iMailAddress { get; set; } //邮寄地址
        public string iEmailAddress { get; set; } //邮箱地址
        public string iPersonInChargeOnSpot { get; set; } //现场负责人
        public string iPersonInChargeOnSpotTel { get; set; } //现场负责人电话
        public string iNote { get; set; } //备注 
        public DateTime? iCreatedOn { get; set; }  //创建时间
        public string iCreatedBy { get; set; } //创建人
        public DateTime? iUpdatedOn { get; set; } //修改时间
        public string iUpdatedBy { get; set; } //修改人
        public int iStatus { get; set; }  //状态
        public int iIsDeleted { get; set; } //假删

    }
}
