using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Data.Entity
{
    //物资领用 信息
    [Table("MaterialBorrow")]
    public class MaterialBorrowEntity
    {
        public string iGuid { get; set; } //惟一标识
        public string iCompanyId { get; set; } //公司
        public string iProjectId { get; set; } //项目
        public string iBorrower { get; set; } //领用人 
        public DateTime? iBorrowedDate { get; set; } //领用日期
        public string iBrand { get; set; } //品牌
        public string iModelNo { get; set; } //型号
        public string iSerialNo { get; set; } //序列号
        public decimal? iQuantity { get; set; } //数量
        public decimal? iPrice { get; set; } //购买金额
        public DateTime? iBoughtDate { get; set; } //购买日期
        public DateTime? iReturnedDate { get; set; } //归还日期
        public string iNote { get; set; } // 备注
        public DateTime? iCreatedOn { get; set; }  //创建时间
        public string iCreatedBy { get; set; } //创建人
        public DateTime? iUpdatedOn { get; set; } //修改时间
        public string iUpdatedBy { get; set; } //修改人
        public int iStatus { get; set; }  //状态
        public int iIsDeleted { get; set; } //假删

    }

}

