using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinBrain.Data.Entity
{
    /// <summary>
    /// 人力对象
    /// </summary>
    [Table("LaborInfo")]
    public class LaborInfoEntity
    {
        public string iGuid { get; set; } //惟一主键
        public string iCompanyCode { get; set; } //信息所属公司
        public string iNo { get; set; } //序号 
        public string iItemName { get; set; } //项目名称 
        public string iCompany { get; set; } //所在公司 
        public string iName { get; set; } //姓名 
        public string iEmpNo { get; set; } //工号 
        public string iPhone { get; set; } //电话 
        public string iEmeContact { get; set; } //紧急联系人 
        public string iEmeContactPhone { get; set; } //紧急联系人电话 
        public string iSex { get; set; } //性别 
        public string iBirthday { get; set; } //出生日期 
        public string iRegistry { get; set; } //户籍 
        public string iNation { get; set; } //民族 
        public string iIdCard { get; set; } //身份证号 
        public string iResidenceProperty { get; set; } //户口性质 
        public string iRegistryAddress { get; set; } //户籍地址 
        public string iIssuedBy { get; set; } //签发机关 
        public string iIdCardValidate { get; set; } //身份证有效期 
        public string iLivedIn { get; set; } //现住地 
        public string iEmployeeStatus { get; set; } //员工状态 
        public string iEmployeeDate { get; set; } //入职时间 
        public string iPositiveDate { get; set; } //转正时间 
        public string iDeadLine { get; set; } //到期日期 
        public string iResignType { get; set; } //离职类型 
        public string iResignDate { get; set; } //离职日期 
        public string iPayDeadLine { get; set; } //工资截止日期 
        public string iResignReason { get; set; } //离职原因 
        public string iWageBank { get; set; } //工资开户行 
        public string iWageAccount { get; set; } //工资账号 
        public string iFirstDep { get; set; } //所属一级部门 
        public string iSecondDep { get; set; } //所属二级部门 
        public string iThirdDep { get; set; } //所属三级部门 
        public string iFourthDep { get; set; } //所属四级部门 
        public string iFifthDep { get; set; } //所属五级部门 
        public string iWorkPlace { get; set; } //所在工作地 
        public string iEmpType { get; set; } //人员类别 
        public string iPosition { get; set; } //岗位 
        public string iManageLevel { get; set; } //管理层级 
        public string iProjectGroup { get; set; } //所在项目组 
        public string iCompanyWorkYear { get; set; } //司龄 
        public string iMariage { get; set; } //婚姻状况 
        public string iAge { get; set; } //年龄 
        public string iHealth { get; set; } //健康 
        public string iPolitical { get; set; } //政治面貌 
        public string iPostalAddress { get; set; } //通讯地址 
        public string iWorkDate { get; set; } //工作时间 
        public string iWorkYearSpan { get; set; } //工作年限 
        public string iEducationLevel { get; set; } //文化水平 
        public string iGraduatedSchool { get; set; } //毕业学校 
        public string iMajor { get; set; } //专业 
        public string iGraduatedDate { get; set; } //毕业时间 
        public string iForeignLanguage { get; set; } //所学外语 
        public string iForeignLanguageLevel { get; set; } //外语等级 
        public string iComputerMastery { get; set; } //计算机掌握程度 
        public string iComputerLevel { get; set; } //计算机等级 
        public string iCertificate { get; set; } //所获证书 
        public string iEducationResume { get; set; } //教育简历 
        public string iSpecialty { get; set; } //特长 
        public string iHobby { get; set; } //爱好 
        public string iEmail { get; set; } //邮箱 
        public string iContactAddress { get; set; } //联系地址 
        public string iContactTel { get; set; } //联系电话 
        public string iPostCode { get; set; } //邮编 
        public string iHeight { get; set; } //身高 
        public string iWeight { get; set; } //体重 
        public string iBloodType { get; set; } //血型 
        public string iJobRequire { get; set; } //工作要求 
        public string iSocietyRelation { get; set; } //社会关系 
        public string iWorkExperience { get; set; } //工作经历 
        public string iStudyExperience { get; set; } //学习经历 
        public string iContractType { get; set; } //合同类型 
        public string iFundStatus { get; set; } //参加基金状态 
        public string iContractTerm { get; set; } //合同/协议期限 
        public string iFileLocation { get; set; } //档案位置 
        public string iSocialSecurityAccount { get; set; } //社保账号 
        public string iSocialSecurityCompanyPay { get; set; } //社保公司缴纳金额 
        public string iSocialSecurityPersonalPay { get; set; } //社保个人缴纳金额 
        public string iSocialSecurityAmount { get; set; } //社保缴纳总金额 
        public string iProvidentAccount { get; set; } //公积金账号 
        public string iProvidentCompanyPay { get; set; } //公积金公司缴费金额 
        public string iProvidentPersonalPay { get; set; } //公积金个人缴费金额 
        public string iLaborCompany { get; set; } //劳务公司 
        public string iFirstReturn { get; set; } //一级返费金额 
        public string iFirstReturnDate { get; set; } //一级返费日期 
        public string iFirstReturnPayStatus { get; set; } //一级返费付款情况 
        public string iSecondReturn { get; set; } //二级返费金额 
        public string iSecondReturnDate { get; set; } //二级返费日期 
        public string iSecondReturnPayStatus { get; set; } //二级返费付款情况 
        public string iThirdReturn { get; set; } //三级返费金额 
        public string iThirdReturnDate { get; set; } //三级返费日期 
        public string iThirdReturnPayStatus { get; set; } //三级返费付款情况 
        public string iLaborBank { get; set; } //劳务所银行支行 
        public string iLaborAccount { get; set; } //劳务所账号 
        public string iAccountOwnner { get; set; } //账户所有人 
        public string iNote1 { get; set; } //备注1 
        public string iNote2 { get; set; } //备注2 


    }
}
