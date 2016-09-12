using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinBrain.Data.Entity
{
    //HR 信息
    [Table("HRInfo")]
    public class HRInfoEntity
    {
        public string iGuid { get; set; } //惟一标识
        public string iItemName { get; set; } //项目名称 
        public string iCompany { get; set; } //所在公司 
        public string iWorkPlace { get; set; } //所在工作地 
        public string iEmpNo { get; set; } //工号 
        public string iName { get; set; } //姓名 
        public string iIdCard { get; set; } //身份证号 
        public string iSex { get; set; } //性别 
        public string iBirthday { get; set; } //出生日期 
        public string iRegistry { get; set; } //户籍 
        public string iNation { get; set; } //民族 
        public string iResidenceProperty { get; set; } //户口性质 
        public string iRegistryAddress { get; set; } //户籍地址 
        public string iIssuedBy { get; set; } //签发机关 
        public string iIdCardValidate { get; set; } //身份证有效期 
        public string iLivedIn { get; set; } //现住地 
        public string iPhone { get; set; } //联系电话 
        public string iEmeContact { get; set; } //紧急联系人 
        public string iEmeContactPhone { get; set; } //紧急联系人电话 
        public string iEmail { get; set; } //邮箱 
        public string iPostCode { get; set; } //邮编 
        public string iHeight { get; set; } //身高（cm） 
        public string iWeight { get; set; } //体重 
        public string iBloodType { get; set; } //血型 
        public string iMariage { get; set; } //婚姻状况 
        public string iAge { get; set; } //年龄 
        public string iHealthCheck { get; set; } //体检 
        public string iPolitical { get; set; } //政治面貌 
        public string iEducationLevel { get; set; } //文化水平 
        public string iMajor { get; set; } //专业 
        public string iGraduatedSchool { get; set; } //毕业学校 
        public string iGraduatedDate { get; set; } //毕业时间 
        public string iWorkExperience { get; set; } //工作经历 
        public string iWageBank { get; set; } //开户行 
        public string iWageBankName { get; set; } //开户行名称 
        public string iWageAccount { get; set; } //工资帐号 
        public string iSocialSecurityAccount { get; set; } //社保账号 
        public string iProvidentAccount { get; set; } //公积金账号 
        public string iEmpType { get; set; } //人员类别 
        public string iPosition { get; set; } //岗位 
        public string iManageLevel { get; set; } //管理层级 
        public string iProjectGroup { get; set; } //所在项目组 
        public string iCompanyWorkYear { get; set; } //司龄 
        public string iFirstDep { get; set; } //所属一级部门 
        public string iSecondDep { get; set; } //所属二级部门 
        public string iThirdDep { get; set; } //所属三级部门 
        public string iFourthDep { get; set; } //所属四级部门 
        public string iFifthDep { get; set; } //所属五级部门 
        public string iEmployeeStatus { get; set; } //员工状态 
        public DateTime? iEmployeeDate { get; set; } //入职时间 
        public string iPositiveDate { get; set; } //转正时间 
        public string iContractType { get; set; } //合同类型 
        public string iContractSignStatus { get; set; } //合同签订情况 
        public string iContractTerm { get; set; } //合同/协议期限 
        public DateTime? iContractDeadLine { get; set; } //合同到期日期 
        public string iResignType { get; set; } //离职类型 
        public DateTime? iResignDate { get; set; } //离职日期 
        public string iResignReason { get; set; } //离职原因（公司） 
        public string iFileLocation { get; set; } //档案位置 
        public string iIsReturnFee { get; set; } //是否返费 
        public string iIsSocialInsurancePaid { get; set; } //是否缴纳保险 
        public string iIsProvidentPaid { get; set; } //是否缴纳公积金 
        public string iIsCommercialInsurancePaid { get; set; } //是否缴纳商业保险 

        public string iBasicInfoNote { get; set; } // 基本信息备注
        public string iAccountInfoNote { get; set; } // 账户信息备注
        public string iPositionInfoNote { get; set; } // 职位信息备注

        public DateTime? iCreatedOn { get; set; }  //创建时间
        public string iCreatedBy { get; set; } //创建人
        public DateTime? iUpdatedOn { get; set; } //修改时间
        public string iUpdatedBy { get; set; } //修改人
        public int iStatus { get; set; }  //状态
        public int iIsDeleted { get; set; } //假删

    }
}
