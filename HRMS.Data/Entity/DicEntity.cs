using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Data.Entity
{
    /// <summary>
    /// 类型对象
    /// </summary>
    [Table("SysDic")]
    public class DicEntity
    {
        public string iId { get; set; }

        public string iCompanyCode { get; set; }
        public string iType { get; set; }
        public string iKey { get; set; }
        public string iValue { get; set; }

        public DateTime iCreatedOn { get; set; }
        public string iCreatedBy { get; set; }
        public DateTime iUpdatedOn { get; set; }

        public string iUpdatedBy { get; set; }
        public int iStatus { get; set; }
        public int iIsDeleted { get; set; }
    }
    /// <summary>
    /// 公司对象
    /// </summary>
    [Table("SysCompany")]
    public class CompanyEntity
    {
        public string iGuid { get; set; }
        public string iName { get; set; }
        public DateTime? iCreatedOn { get; set; }
        public string iCreatedBy { get; set; }
        public DateTime? iUpdatedOn { get; set; }
        public string iUpdatedBy { get; set; }
        public int iStatus { get; set; }
        public int iIsDeleted { get; set; }
    }
    /// <summary>
    /// 项目对象
    /// </summary>
    [Table("SysProject")]
    public class ProjectEntity
    {
        public string iGuid { get; set; }
        public string iName { get; set; }
        public DateTime? iCreatedOn { get; set; }
        public string iCreatedBy { get; set; }
        public DateTime? iUpdatedOn { get; set; }
        public string iUpdatedBy { get; set; }
        public int iStatus { get; set; }
        public int iIsDeleted { get; set; }
    }

    /// <summary>
    /// 项目对应公司关系对象
    /// </summary>
    [Table("SysCompanyProjectRelation")]
    public class CompanyProjectRelationEntity
    {
        public string iGuid { get; set; }
        public string iCompanyId { get; set; }
        public string iProjectId { get; set; }
        public DateTime? iCreatedOn { get; set; }
        public string iCreatedBy { get; set; }
        public DateTime? iUpdatedOn { get; set; }
        public string iUpdatedBy { get; set; }
        public int iStatus { get; set; }
        public int iIsDeleted { get; set; }
    }

    /// <summary>
    /// 流程人员对象--废弃
    /// </summary>
    [Table("BpmUser")]
    public class BpmUserEntity
    {
        public string iGuid { get; set; }
        public string iCompanyId { get; set; }
        public string iProjectId { get; set; }
        public string iFlowSign { get; set; }
        public string iRoleName { get; set; }
        public string iUsers { get; set; }
        public string iNote { get; set; }
        public DateTime? iCreatedOn { get; set; }
        public string iCreatedBy { get; set; }
        public DateTime? iUpdatedOn { get; set; }
        public string iUpdatedBy { get; set; }
        public int iStatus { get; set; }
        public int iIsDeleted { get; set; }
    }

    public class BpmUserView
    {
        public string iGuid { get; set; }
        public string iCompanyId { get; set; }
        public string iCompanyName { get; set; }
        public string iFlowSign { get; set; }
        public string iRoleName { get; set; }
        public string iUsers { get; set; }
        public string iNote { get; set; }
        public DateTime? iUpdatedOn { get; set; }
    }


    //流程公司角色定义
    /// <summary>
    /// 角色对象
    /// </summary>
    [Table("SysRole")]
    public class RoleEntity
    {
        public string iGuid { get; set; }
        public string iName { get; set; }       
        public string iNote { get; set; }
        public DateTime? iCreatedOn { get; set; }
        public string iCreatedBy { get; set; }
        public DateTime? iUpdatedOn { get; set; }
        public string iUpdatedBy { get; set; }
        public int iStatus { get; set; }
        public int iIsDeleted { get; set; }
    }

    /// <summary>
    /// 角色对象
    /// </summary>
    [Table("SysRoleUsers")]
    public class RoleUsersEntity
    {
        public string iGuid { get; set; }
        public string iRoleGuid { get; set; }
        public string iCompanyId { get; set; }
        public string iUsers { get; set; }
        public DateTime? iCreatedOn { get; set; }
        public string iCreatedBy { get; set; }
        public DateTime? iUpdatedOn { get; set; }
        public string iUpdatedBy { get; set; }
        public int iStatus { get; set; }
        public int iIsDeleted { get; set; }
    }
}
