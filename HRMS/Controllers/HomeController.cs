using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HRMS.Data.Entity;
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
            //StringBuilder sb = new StringBuilder();
            //Dictionary<string, string> dic = FinanceSummaryManager.FinanceSummaryDic;
            //sb.AppendLine(CreateSql(dic, "FinanceSummary"));
            //sb.AppendLine(CreateClass(dic));
            //sb.AppendLine(CreateTableColumn(dic));
            //sb.AppendLine(CreateMvvmModel(dic));
            //sb.AppendLine(CreateMvvmBinding(dic));
            //sb.AppendLine(CreateInputs(dic));
            //ErrorLog(sb.ToString());
            return View();
        }

        //生成sql
        public string CreateSql(Dictionary<string, string> dic, string tableName)
        {
            StringBuilder sb = new StringBuilder("CREATE TABLE [dbo].[" + tableName + "]( \r\n");
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                sb.Append(kvp.Value);
                sb.Append(" [nvarchar](4000) NULL, \r\n");
            }
            return sb.ToString();
        }

        //生成class
        public string CreateClass(Dictionary<string, string> dic)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                sb.Append(string.Format("public string {0} {2} get; set; {3} //{1} \r\n", kvp.Value, kvp.Key, "{", "}"));
            }
            return sb.ToString();
        }

        //生成bootstraptable column
        public string CreateTableColumn(Dictionary<string, string> dic)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                sb.AppendFormat(",{0} field: \"{2}\", title: \"{3}\", sortable: true {1} \r\n", "{", "}", kvp.Value, kvp.Key);
            }
            return sb.ToString();
        }

        //生成Mvvm model
        public string CreateMvvmModel(Dictionary<string, string> dic)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                sb.AppendFormat("myViewModel.{0}(\"\"); \r\n", kvp.Value);
            }
            sb.AppendLine();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                sb.AppendFormat("myViewModel.{0}(result.data.{0}); \r\n", kvp.Value);
            }
            return sb.ToString(); 
        }

        //生成Mvvm modelbinding
        public string CreateMvvmBinding(Dictionary<string, string> dic)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                sb.AppendFormat("self.{0} = ko.observable(\"\"); \r\n", kvp.Value);
            }
            return sb.ToString();
        }

        //生成input
        public string CreateInputs(Dictionary<string, string> dic)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                sb.AppendFormat("<div class=\"form-group\"> {0}：<input type=\"text\" class=\"form-control\" id=\"{1}\" data-bind=\"value: {1}\" placeholder=\"请输入{0}\"></div> \r\n", kvp.Key, kvp.Value);
            }
            return sb.ToString();
        }

        public static void ErrorLog(string msg)
        {
            string logFolderPath = System.AppDomain.CurrentDomain.BaseDirectory + "/log";
            string FilePath = logFolderPath + "/ErrorLog_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            try
            {
                if (Directory.Exists(logFolderPath) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(logFolderPath);
                }

                if (System.IO.File.Exists(FilePath))
                {
                    using (StreamWriter tw = System.IO.File.AppendText(FilePath))
                    {
                        tw.WriteLine(DateTime.Now.ToString() + "> " + msg);
                    }  //END using

                }  //END if
                else
                {
                    FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                    using (StreamWriter tw = new StreamWriter(fs))
                    {
                        tw.WriteLine(DateTime.Now.ToString() + "> " + msg);
                    }  //END using
                    fs.Close();
                }
            }
            catch (Exception ex)
            { } //END Catch   

        }  // END ErrorLog

        /*测试代码
         
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

            //StringBuilder sb = new StringBuilder();
            //foreach (KeyValuePair<string, string> kvp in dic)
            //{
            //    sb.Append(string.Format("public string {0} {2} get; set; {3} //{1} \n",kvp.Value,kvp.Key,"{","}"));
            //}
            //string result = sb.ToString();

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                sb.Append(string.Format("public string {0} {2} get; set; {3} //{1} \n", kvp.Value, kvp.Key, "{", "}"));
            }
            string result = sb.ToString();
            
            
            
            int i = 0;

            //UserEntity user = new UserEntity();
            //user.EmployeeCode = Guid.NewGuid().ToString();
            //user.UserName = "Kevin";
            //user.PassWord = "123";
            //UserManager um = new UserManager();
            //um.Insert(user);
         * 
         * 
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("iGuid", "iGuid");

            dic.Add("项目名称", "iItemName");
            dic.Add("所在公司", "iCompany");
            dic.Add("所在工作地", "iWorkPlace");
            dic.Add("工号", "iEmpNo");
            dic.Add("姓名", "iName");
            dic.Add("身份证号", "iIdCard");
            dic.Add("性别", "iSex");
            dic.Add("出生日期", "iBirthday");
            dic.Add("户籍", "iRegistry");
            dic.Add("民族", "iNation");
            dic.Add("户口性质", "iResidenceProperty");
            dic.Add("户籍地址", "iRegistryAddress");
            dic.Add("签发机关", "iIssuedBy");
            dic.Add("身份证有效期", "iIdCardValidate");
            dic.Add("现住地", "iLivedIn");
            dic.Add("联系电话", "iPhone");
            dic.Add("紧急联系人", "iEmeContact");
            dic.Add("紧急联系人电话", "iEmeContactPhone");
            dic.Add("邮箱", "iEmail"); 
            dic.Add("邮编", "iPostCode");
            dic.Add("身高（cm）", "iHeight");
            dic.Add("体重", "iWeight");
            dic.Add("血型", "iBloodType");
            dic.Add("婚姻状况", "iMariage");
            dic.Add("年龄", "iAge");
            dic.Add("体检", "iHealthCheck");
            dic.Add("政治面貌", "iPolitical");
            dic.Add("文化水平", "iEducationLevel");
            dic.Add("专业", "iMajor");
            dic.Add("毕业学校", "iGraduatedSchool");
            dic.Add("毕业时间", "iGraduatedDate");
            dic.Add("工作经历", "iWorkExperience");

            dic.Add("开户行", "iWageBank");
            dic.Add("开户行名称", "iWageBankName");
            dic.Add("工资帐号", "iWageAccount"); 
            dic.Add("社保账号", "iSocialSecurityAccount");
            dic.Add("公积金账号", "iProvidentAccount");

            dic.Add("人员类别", "iEmpType");
            dic.Add("岗位", "iPosition");
            dic.Add("管理层级", "iManageLevel");
            dic.Add("所在项目组", "iProjectGroup");
            dic.Add("司龄", "iCompanyWorkYear");
            dic.Add("所属一级部门", "iFirstDep");
            dic.Add("所属二级部门", "iSecondDep");
            dic.Add("所属三级部门", "iThirdDep");
            dic.Add("所属四级部门", "iFourthDep");
            dic.Add("所属五级部门", "iFifthDep");
            dic.Add("员工状态", "iEmployeeStatus");
            dic.Add("入职时间", "iEmployeeDate");
            dic.Add("转正时间", "iPositiveDate");
            dic.Add("合同类型", "iContractType");
            dic.Add("合同签订情况", "iContractSignStatus");
            dic.Add("合同/协议期限", "iContractTerm");
            dic.Add("合同到期日期", "iContractDeadLine");
            dic.Add("离职类型", "iResignType");
            dic.Add("离职日期", "iResignDate");
            dic.Add("离职原因（公司）", "iResignReason");
            dic.Add("档案位置", "iFileLocation");
            dic.Add("是否返费", "iIsReturnFee");
            dic.Add("是否缴纳保险", "iIsSocialInsurancePaid");
            dic.Add("是否缴纳公积金", "iIsProvidentPaid");
            dic.Add("是否缴纳商业保险", "iIsCommercialInsurancePaid");

            dic.Add("iCreatedOn", "iCreatedOn");
            dic.Add("iCreatedBy", "iCreatedBy");
            dic.Add("iUpdatedOn", "iUpdatedOn");
            dic.Add("iUpdatedBy", "iUpdatedBy");
            dic.Add("iStatus", "iStatus");
            dic.Add("iIsDeleted", "iIsDeleted");


            string[] dateList = new string[] { "入职时间","合同到期日期","离职日期"};
            //StringBuilder sb = new StringBuilder("CREATE TABLE [dbo].[HRInfo]( \n");
            //foreach (KeyValuePair<string, string> kvp in dic)
            //{
            //    sb.Append(kvp.Value);
            //    if (dateList.Contains(kvp.Key))
            //        sb.Append(" [date] NULL, \n");
            //    else
            //        sb.Append(" [nvarchar](4000) NULL, \n");
            //}
            //string result = sb.ToString();

            //StringBuilder sb = new StringBuilder();
            //foreach (KeyValuePair<string, string> kvp in dic)
            //{
            //    if(dateList.Contains(kvp.Key))

            //        sb.Append(string.Format("public DateTime? {0} {2} get; set; {3} //{1} \n", kvp.Value, kvp.Key, "{", "}"));
            //    else
            //        sb.Append(string.Format("public string {0} {2} get; set; {3} //{1} \n", kvp.Value, kvp.Key, "{", "}"));
            //}
            //string result = sb.ToString();


            //StringBuilder sb = new StringBuilder();
            //foreach (KeyValuePair<string, string> kvp in dic)
            //{
            //    sb.AppendFormat("self.{0} = ko.observable(\"\"); \n", kvp.Value);
            //}
            //string result = sb.ToString();


            //StringBuilder sb = new StringBuilder();
            //foreach (KeyValuePair<string, string> kvp in dic)
            //{
            //    sb.AppendFormat("<div class=\"form-group\"> {0}：<input type=\"text\" class=\"form-control\" id=\"{1}\" data-bind=\"value: {1}\" placeholder=\"请输入{0}\"></div> \n", kvp.Key,kvp.Value);
            //}
            //string result = sb.ToString();

            //StringBuilder sb = new StringBuilder();
            //foreach (KeyValuePair<string, string> kvp in dic)
            //{
            //    sb.AppendFormat(",{0} field: \"{2}\", title: \"{3}\", sortable: true {1} \n","{","}", kvp.Value, kvp.Key);
            //}
            //string result = sb.ToString();

            //StringBuilder sb = new StringBuilder();
            //foreach (KeyValuePair<string, string> kvp in dic)
            //{
            //    sb.AppendFormat("myViewModel.{0}(result.data.{0}); \n", kvp.Value);
            //}
            //string result = sb.ToString(); 

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                sb.AppendFormat("myViewModel.{0}(\"\"); \n", kvp.Value);
            }
            string result = sb.ToString(); 

            int i = 0;
         */
    }
}
