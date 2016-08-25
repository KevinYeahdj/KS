using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ClinBrain.Data.Entity;
using HRMS.Data.Manager;
using HRMS.WEB.Models;
using HRMS.WEB.Utils;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace HRMS.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("序号","iNo");
            dic.Add("项目名称","iItemName");
            dic.Add("所在公司","iCompany");
            dic.Add("姓名","iName");
            dic.Add("工号","iEmpNo");
            dic.Add("电话","iPhone");
            dic.Add("紧急联系人","iEmeContact");
            dic.Add("紧急联系人电话","iEmeContactPhone");
            dic.Add("性别","iSex");
            dic.Add("出生日期","iBirthday");
            dic.Add("户籍","iRegistry");
            dic.Add("民族","iNation");
            dic.Add("身份证号","iIdCard");
            dic.Add("户口性质","iResidenceProperty");
            dic.Add("户籍地址","iRegistryAddress");
            dic.Add("签发机关","iIssuedBy");
            dic.Add("身份证有效期","iIdCardValidate");
            dic.Add("现住地","iLivedIn");
            dic.Add("员工状态","iEmployeeStatus");
            dic.Add("入职时间", "iEmployeeDate");
            dic.Add("转正时间", "iPositiveDate");
            dic.Add("到期日期", "iDeadLine");
            dic.Add("离职类型", "iResignType");
            dic.Add("离职日期", "iResignDate");
            dic.Add("工资截止日期", "iPayDeadLine");
            dic.Add("离职原因（公司）", "iResignReason");
            dic.Add("工资开户行", "iWageBank");
            dic.Add("工资帐号", "iWageAccount");  //工资账号
            dic.Add("所属一级部门", "iFirstDep");
            dic.Add("所属二级部门", "iSecondDep");
            dic.Add("所属三级部门", "iThirdDep");
            dic.Add("所属四级部门", "iFourthDep");
            dic.Add("所属五级部门", "iFifthDep");
            dic.Add("所在工作地", "iWorkPlace");
            dic.Add("人员类别", "iEmpType");
            dic.Add("岗位", "iPosition");
            dic.Add("管理层级", "iManageLevel");
            dic.Add("所在项目组", "iProjectGroup");
            dic.Add("司龄", "iCompanyWorkYear");
            dic.Add("婚姻状况", "iMariage");
            dic.Add("年龄", "iAge");
            dic.Add("健康", "iHealth");
            dic.Add("政治面貌", "iPolitical");
            dic.Add("通讯地址", "iPostalAddress");
            dic.Add("工作时间", "iWorkDate");
            dic.Add("工作年限", "iWorkYearSpan");
            dic.Add("文化水平", "iEducationLevel");
            dic.Add("毕业学校", "iGraduatedSchool");
            dic.Add("专业", "iMajor");
            dic.Add("毕业时间", "iGraduatedDate");
            dic.Add("所学外语", "iForeignLanguage");
            dic.Add("外语等级", "iForeignLanguageLevel");
            dic.Add("计算机掌握程度", "iComputerMastery");
            dic.Add("计算机等级", "iComputerLevel");
            dic.Add("所获证书", "iCertificate");
            dic.Add("教育简历", "iEducationResume");
            dic.Add("特长", "iSpecialty");
            dic.Add("爱好", "iHobby");
            dic.Add("邮箱", "iEmail");
            dic.Add("联系地址", "iContactAddress");
            dic.Add("联系电话", "iContactTel");
            dic.Add("邮编", "iPostCode");
            dic.Add("身高", "iHeight");
            dic.Add("体重", "iWeight");
            dic.Add("血型", "iBloodType");
            dic.Add("工作要求", "iJobRequire");
            dic.Add("社会关系", "iSocietyRelation");
            dic.Add("工作经历", "iWorkExperience");
            dic.Add("学习经历", "iStudyExperience");
            dic.Add("合同类型", "iContractType");
            dic.Add("参加基金状态", "iFundStatus");
            dic.Add("合同/协议期限", "iContractTerm");
            dic.Add("档案位置", "iFileLocation");
            dic.Add("社保账号", "iSocialSecurityAccount");
            dic.Add("社保公司缴纳金额", "iSocialSecurityCompanyPay");
            dic.Add("社保个人缴纳金额", "iSocialSecurityPersonalPay");
            dic.Add("社保缴纳总金额", "iSocialSecurityAmount");
            dic.Add("公积金账号", "iProvidentAccount");
            dic.Add("公积金公司缴费金额", "iProvidentCompanyPay");
            dic.Add("公积金个人缴费金额", "iProvidentPersonalPay");
            dic.Add("劳务公司", "iLaborCompany");
            dic.Add("一级返费金额", "iFirstReturn");
            dic.Add("一级返费日期", "iFirstReturnDate");
            dic.Add("一级返费付款情况", "iFirstReturnPayStatus");
            dic.Add("二级返费", "iSecondReturn");
            dic.Add("二级返费日期", "iSecondReturnDate");
            dic.Add("二级返费付款情况", "iSecondReturnPayStatus");
            dic.Add("三级返费", "iThirdReturn");
            dic.Add("三级返费到期日期", "iThirdReturnDate");
            dic.Add("三级返费付款情况", "iThirdReturnPayStatus");
            dic.Add("劳务所银行支行", "iLaborBank");
            dic.Add("劳务所账号", "iLaborAccount");
            dic.Add("账户所有人", "iAccountOwnner");
            dic.Add("备注1", "iNote1");
            dic.Add("备注2", "iNote2");

            //StringBuilder sb = new StringBuilder("CREATE TABLE [dbo].[LaborInfo]( \n");
            //foreach (KeyValuePair<string, string> kvp in dic)
            //{
            //    sb.Append(kvp.Value);
            //    sb.Append(" [nvarchar](4000) NULL, \n");
            //}
            //string result = sb.ToString();

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                sb.Append(string.Format("public string {0} {2} get; set; {3} //{1} \n",kvp.Value,kvp.Key,"{","}"));
            }
            string result = sb.ToString();
            int i = 0;







            //UserEntity user = new UserEntity();
            //user.EmployeeCode = Guid.NewGuid().ToString();
            //user.UserName = "Kevin";
            //user.PassWord = "123";
            //UserManager um = new UserManager();
            //um.Insert(user);

            return View();
        }

    }
}
