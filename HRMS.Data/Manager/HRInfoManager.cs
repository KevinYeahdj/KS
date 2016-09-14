using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClinBrain.Data.Entity;

namespace HRMS.Data.Manager
{
    /// <summary>
    /// HR信息管理类
    /// </summary>
    public class HRInfoManager : ManagerBase
    {

        public static Dictionary<string, string> hrDicOld
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("序号", "iNo");
                dic.Add("项目名称", "iItemName");
                dic.Add("所在公司", "iCompany");
                dic.Add("姓名", "iName");
                dic.Add("工号", "iEmpNo");
                dic.Add("电话", "iPhone");
                dic.Add("紧急联系人", "iEmeContact");
                dic.Add("紧急联系人电话", "iEmeContactPhone");
                dic.Add("性别", "iSex");
                dic.Add("出生日期", "iBirthday");
                dic.Add("户籍", "iRegistry");
                dic.Add("民族", "iNation");
                dic.Add("身份证号", "iIdCard");
                dic.Add("户口性质", "iResidenceProperty");
                dic.Add("户籍地址", "iRegistryAddress");
                dic.Add("签发机关", "iIssuedBy");
                dic.Add("身份证有效期", "iIdCardValidate");
                dic.Add("现住地", "iLivedIn");
                dic.Add("员工状态", "iEmployeeStatus");
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
                return dic;
            }

        }
        public static Dictionary<string, string> hrDic
        {
            get
            {
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

                dic.Add("基本信息备注", "iBasicInfoNote");
                dic.Add("账户信息备注", "iAccountInfoNote");
                dic.Add("职位信息备注", "iPositionInfoNote");

                dic.Add("iCreatedOn", "iCreatedOn");
                dic.Add("iCreatedBy", "iCreatedBy");
                dic.Add("iUpdatedOn", "iUpdatedOn");
                dic.Add("iUpdatedBy", "iUpdatedBy");
                dic.Add("iStatus", "iStatus");
                dic.Add("iIsDeleted", "iIsDeleted");
                return dic;
            }
        }

        public static Dictionary<string, string> hrBasicDic
        {
            get
            {
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
                dic.Add("基本信息备注", "iBasicInfoNote");
                return dic;
            }
        }

        public static Dictionary<string, string> hrAccountDic
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("iGuid", "iGuid");
                dic.Add("项目名称", "iItemName");
                dic.Add("所在公司", "iCompany");
                dic.Add("所在工作地", "iWorkPlace");
                dic.Add("工号", "iEmpNo");
                dic.Add("姓名", "iName");
                dic.Add("身份证号", "iIdCard");
                dic.Add("开户行", "iWageBank");
                dic.Add("开户行名称", "iWageBankName");
                dic.Add("工资帐号", "iWageAccount");
                dic.Add("社保账号", "iSocialSecurityAccount");
                dic.Add("公积金账号", "iProvidentAccount");
                dic.Add("账户信息备注", "iAccountInfoNote");
                return dic;
            }
        }
        public static Dictionary<string, string> hrPositionDic
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("iGuid", "iGuid");
                dic.Add("项目名称", "iItemName");
                dic.Add("所在公司", "iCompany");
                dic.Add("所在工作地", "iWorkPlace");
                dic.Add("工号", "iEmpNo");
                dic.Add("姓名", "iName");
                dic.Add("身份证号", "iIdCard");
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
                dic.Add("职位信息备注", "iPositionInfoNote");
                return dic;
            }
        }

        public static Dictionary<string, string> DicConvert(Dictionary<string, string> dicOri)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in dicOri)
            {
                dic.Add(item.Value, item.Key);
            }
            return dic;
        }
        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Insert(HRInfoEntity entity)
        {
            if (string.IsNullOrEmpty(entity.iGuid))
                entity.iGuid = Guid.NewGuid().ToString();
            entity.iCreatedOn = DateTime.Now;
            entity.iUpdatedOn = DateTime.Now;
            entity.iIsDeleted = 0;
            entity.iStatus = 1;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Insert<HRInfoEntity>(session.Connection, entity, session.Transaction);
                session.Commit();
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }
        public void Update(HRInfoEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            var record = ModifyRecord(entity);
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<HRInfoEntity>(session.Connection, entity, session.Transaction);
                if (record != null)
                {
                    Repository.Insert<ModifyLogEntity>(session.Connection, record, session.Transaction);
                }
                session.Commit();
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }
        public HRInfoEntity GetFirstOrDefault(string id)
        {
            string sql = @"select * from hrinfo where iguid=@id and iIsDeleted =0 and iStatus =1";
            return Repository.Query<HRInfoEntity>(sql, new { id = id }).FirstOrDefault();
        }

        public HRInfoEntity GetUniqueFirstOrDefault(string company, string empcode, string idcard)
        {
            string sql = @"select * from hrinfo where icompany=@icompany and iempno = @iempno and iidcard=@iidcard and iIsDeleted =0 and iStatus =1";
            return Repository.Query<HRInfoEntity>(sql, new { icompany = company, iempno = empcode, iidcard = idcard }).FirstOrDefault();
        }
        public List<HRInfoEntity> GetSearch(string companyCode, Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            StringBuilder commandsb = new StringBuilder("from HRInfo where icompany='");
            commandsb.Append(companyCode);
            commandsb.Append("' ");
            foreach (KeyValuePair<string, string> item in para)
            {
                if (!string.IsNullOrEmpty(item.Value) && item.Value != "§")
                {
                    if (item.Key.EndsWith("[d]"))
                    {
                        commandsb.Append(" and " + item.Key.Replace("[d]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "1900-01-01" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.Value.Split('§')[1]) + "' ");
                    }
                    else
                    {
                        commandsb.Append(" and " + item.Key + " like '%" + item.Value + "%'");
                    }
                }
            }


            string commonSql = commandsb.ToString();
            string querySql = "select * " + commonSql + "order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<HRInfoEntity>(querySql).ToList();

        }

        public List<HRInfoEntity> GetSearchAll(string companyCode, Dictionary<string, string> para)
        {
            StringBuilder commandsb = new StringBuilder("from HRInfo where icompany='");
            commandsb.Append(companyCode);
            commandsb.Append("' ");
            foreach (KeyValuePair<string, string> item in para)
            {
                if (!string.IsNullOrEmpty(item.Value) && item.Value != "§")
                {
                    if (item.Key.EndsWith("[d]"))
                    {
                        commandsb.Append(" and " + item.Key.Replace("[d]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "1900-01-01" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.Value.Split('§')[1]) + "' ");
                    }
                    else
                    {
                        commandsb.Append(" and " + item.Key + " like '%" + item.Value + "%'");
                    }
                }
            }
            string commonSql = commandsb.ToString();
            string querySql = "select * " + commonSql + "order by iUpdatedOn desc";
            return Repository.Query<HRInfoEntity>(querySql).ToList();
        }

        public ModifyLogEntity ModifyRecord(HRInfoEntity entity)
        {
            var oldEntity = GetFirstOrDefault(entity.iGuid);
            if (oldEntity == null)
            {
                return null;
            }
            else
            {
                string modifiedContent = string.Empty;
                System.Reflection.PropertyInfo[] properties = entity.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                Dictionary<string, string> hrDicConvertTmp = DicConvert(hrDic);

                foreach (System.Reflection.PropertyInfo item in properties)
                {
                    string name = item.Name;
                    if (!hrDicConvertTmp[name].StartsWith("i"))
                    {
                        object value = item.GetValue(entity, null);
                        if (value == null) value = "";
                        object valueOld = item.GetValue(oldEntity, null);
                        if (valueOld == null) valueOld = "";
                        if (value.ToString() != valueOld.ToString() && (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String")))
                        {
                            modifiedContent += string.Format("{0}:[{1}]->[{2}] ;", hrDicConvertTmp[name], valueOld, value);
                        }
                    }
                }
                if (string.IsNullOrEmpty(modifiedContent))
                {
                    return null;
                }
                modifiedContent = "基本信息:[" + entity.iName + "," + entity.iIdCard + "]" + modifiedContent;
                ModifyLogEntity en = new ModifyLogEntity();
                en.iId = entity.iGuid;
                en.iModifiedBy = entity.iUpdatedBy;
                en.iModifiedOn = DateTime.Now;
                en.iModifiedContent = modifiedContent;
                en.iTableName = "HRInfo";
                return en;
            }
        }
    }
}