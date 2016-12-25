using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Data.Entity
{
    //工资 信息
    [Table("Salary")]
    public class SalaryEntity
    {
        public string iGuid { get; set; } //惟一标识
        public string iCompanyId { get; set; } //公司
        public string iProjectId { get; set; } //项目
        public DateTime? iYearMonth { get; set; } //工资发放年月      
        public string iFileName { get; set; } //文件名称
        public string iUrl { get; set; } //文件地址
        public string iContent { get; set; } //文件内容
        public string iNote { get; set; } //备注 
        public DateTime? iCreatedOn { get; set; }  //创建时间
        public string iCreatedBy { get; set; } //创建人
        public DateTime? iUpdatedOn { get; set; } //修改时间
        public string iUpdatedBy { get; set; } //修改人
        public int iStatus { get; set; }  //状态
        public int iIsDeleted { get; set; } //假删

    }
}
