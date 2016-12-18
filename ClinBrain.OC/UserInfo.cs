using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinBrain.OC
{
    [Serializable]
    public class UserInfo
    {

        public string ID { get; set; }               //企业工号（如果是集团人员为集团工号）
        public string Name { get; set; }             //姓名
        public string LoginName { get; set; }        //登录名
        public string CompanyCode { get; set; }      //当前登录公司编号
        public string CompanyName { get; set; }      //当前登录公司名称
        public string AppCompanyCode { get; set; }    //员工所使用应用编号
        public string AppCompanyName { get; set; }    //员工所使用应用名称
        public string DepartmentCode { get; set; }   //二级部门编号
        public string DepartmentName { get; set; }   //二级部门名称
        public string BranchCode { get; set; }       //一级部门编号
        public string BranchName { get; set; }       //一级部门名称
        public string PositionID { get; set; }       //职位编号
        public string PositionName { get; set; }     //职位名称
        public string PositionLevel { get; set; }    //职级
        public string WorkGroupID { get; set; }      //班组编号
        public string WorkGroupName { get; set; }    //班组名称
        public string EmployeeCode { get; set; }     //集团工号
        public string EMail { get; set; }            //邮件
        public string OriginalCompanyCode { get; set; }   //员工所属公司编号
        public string OriginalCompanyName { get; set; }   //员工所属公司名称
        public string Phone { get; set; }                 //员工座机号
        public string Tel { get; set; }                   //员工手机号
        public string Subordinate_Industry { get; set; }  //板块
        public string Gender { get; set; }                //性别
        public string PoliticsStatus { get; set; }       //政治面貌
        public string BornDate { get; set; }             //出生年月
        public string Graduate { get; set; }              //文化程度
        public string Province { get; set; }              //出生地
        public string CertificateID { get; set; }        //身份证号
        public string CityAddress { get; set; }         //住址
        public string UsedName { get; set; }            //曾用名

        public override string ToString()
        {
            return string.Format("{0}-{1} {2}", Name, CompanyName, DepartmentName);
        }
    }
}
