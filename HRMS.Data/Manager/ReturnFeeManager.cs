using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HRMS.Data.Entity;
using HRMS.Common;

namespace HRMS.Data.Manager
{
    /// <summary>
    /// 返费信息管理类
    /// </summary>
    public class ReturnFeeManager : ManagerBase
    {
        public static Dictionary<string, string> ReturnFeeDic
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("iGuid", "iGuid");

                dic.Add("项目名称", "iItemName");
                dic.Add("所在公司", "iCompany");
                dic.Add("工号", "iEmpNo");
                dic.Add("姓名", "iName");
                dic.Add("身份证号", "iIdCard");
                dic.Add("入职时间", "iEmployeeDate");
                dic.Add("离职日期", "iResignDate");
                dic.Add("在职天数", "iOnJobDays");

                dic.Add("返费建议", "iAdvice");
                dic.Add("劳务名称", "iLaborName");
                dic.Add("劳务所银行支行", "iLaborCampBank");
                dic.Add("劳务所账号", "iLaborCampBankAccount");
                dic.Add("劳务所人姓名", "iLaborCampBankPerson");
                dic.Add("面试日期", "iInterviewDate");


                dic.Add("一级返费金额", "iFirstReturnFeeAmount");
                dic.Add("一级返费天数", "iFirstReturnFeeDays");
                dic.Add("一级返费日期", "iFirstReturnFeeDate");
                dic.Add("一级付款情况", "iFirstReturnFeePayment");
                dic.Add("一级实际支付日期", "iFirstReturnFeeActualPayDate");

                dic.Add("二级返费金额", "iSecondReturnFeeAmount");
                dic.Add("二级返费天数", "iSecondReturnFeeDays");
                dic.Add("二级返费日期", "iSecondReturnFeeDate");
                dic.Add("二级付款情况", "iSecondReturnFeePayment");
                dic.Add("二级实际支付日期", "iSecondReturnFeeActualPayDate");

                dic.Add("三级返费金额", "iThirdReturnFeeAmount");
                dic.Add("三级返费天数", "iThirdReturnFeeDays");
                dic.Add("三级返费日期", "iThirdReturnFeeDate");
                dic.Add("三级付款情况", "iThirdReturnFeePayment");
                dic.Add("三级实际支付日期", "iThirdReturnFeeActualPayDate");

                dic.Add("四级返费金额", "iFourthReturnFeeAmount");
                dic.Add("四级返费天数", "iFourthReturnFeeDays");
                dic.Add("四级返费日期", "iFourthReturnFeeDate");
                dic.Add("四级付款情况", "iFourthReturnFeePayment");
                dic.Add("四级实际支付日期", "iFourthReturnFeeActualPayDate");

                dic.Add("五级返费金额", "iFifthReturnFeeAmount");
                dic.Add("五级返费天数", "iFifthReturnFeeDays");
                dic.Add("五级返费日期", "iFifthReturnFeeDate");
                dic.Add("五级付款情况", "iFifthReturnFeePayment");
                dic.Add("五级实际支付日期", "iFifthReturnFeeActualPayDate");

                dic.Add("返费信息备注", "iReturnFeeNote");
                return dic;
            }
        }

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Insert(ReturnFeeEntity entity)
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
                Repository.Insert<ReturnFeeEntity>(session.Connection, entity, session.Transaction);
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
        public void Update(ReturnFeeEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            var record = ModifyRecord(entity);
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<ReturnFeeEntity>(session.Connection, entity, session.Transaction);
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
        public ReturnFeeModel GetFirstOrDefault(string hrGuid)
        {
            string sql = @"select DATEDIFF(day, hr.iEmployeeDate, hr.iResignDate)  as iOnJobDays, fee.*, hr.iItemName, hr.iCompany, hr.iEmpNo, hr.iName, hr.iIdCard,hr.iEmployeeDate, hr.iResignDate, hr.iGuid as iHRInfoGuid2  from returnfee fee right join hrinfo hr on fee.ihrinfoguid = hr.iguid and fee.iIsDeleted =0 and fee.iStatus =1 where hr.iIsReturnFee = '是' and hr.iisdeleted=0 and hr.istatus=1 and hr.iguid=@id ";
            return Repository.Query<ReturnFeeModel>(sql, new { id = hrGuid }).FirstOrDefault();
        }

        public ReturnFeeEntity FirstOrDefault(string hrGuid)
        {
            string sql = @"select * from returnFee where iHRInfoGuid=@hrid and iIsDeleted =0 and iStatus =1";
            return Repository.Query<ReturnFeeEntity>(sql, new { hrid = hrGuid }).FirstOrDefault();
        }

        public List<ReturnFeeModel> GetSearch(string userType, Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            if (userType != "普通用户")
            {
                para["iCompany"] = para["iCompany"] == "-" ? "" : para["iCompany"];
                para["iItemName"] = para["iItemName"] == "-" ? "" : para["iItemName"];
            }
            StringBuilder commandsb = new StringBuilder("from ReturnFee fee right join hrinfo hr on fee.iHRInfoGuid = hr.iguid and fee.iIsDeleted =0 and fee.iStatus =1  where hr.iIsReturnFee = '是' and hr.iisdeleted=0 and hr.istatus=1 ");

            if (para["editType"] == "已编辑")
            {
                commandsb.Append("and fee.iGuid is not null ");
            }
            else if (para["editType"] == "未编辑")
            {
                commandsb.Append("and fee.iGuid is null ");
            }
            para.Remove("editType");

            string searchKey = para["search"];
            para.Remove("search");

            foreach (KeyValuePair<string, string> item in para)
            {
                if (!string.IsNullOrEmpty(item.Value) && item.Value != "§")
                {
                    if (item.Key.StartsWith("iFirst") && item.Key != "iFirstReturnFeePayment")
                    {
                        commandsb.Append(" and (");
                        if (item.Key.EndsWith("[d]"))
                        {
                            commandsb.Append(item.Key.Replace("[d]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "1900-01-01" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.Value.Split('§')[1]) + "' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iSecond").Replace("[d]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "1900-01-01" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.Value.Split('§')[1]) + "' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iThird").Replace("[d]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "1900-01-01" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.Value.Split('§')[1]) + "' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iFourth").Replace("[d]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "1900-01-01" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.Value.Split('§')[1]) + "' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iFifth").Replace("[d]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "1900-01-01" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.Value.Split('§')[1]) + "' ");
                        }
                        else
                        {
                            commandsb.Append(item.Key + " like '%" + item.Value + "%' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iSecond") + " like '%" + item.Value + "%' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iThird") + " like '%" + item.Value + "%' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iFourth") + " like '%" + item.Value + "%' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iFifth") + " like '%" + item.Value + "%' ");
                        }
                        commandsb.Append(" ) ");
                    }
                    else
                    {
                        if (item.Key.EndsWith("[d]"))
                        {
                            commandsb.Append(" and " + item.Key.Replace("[d]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "1900-01-01" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.Value.Split('§')[1]) + "' ");
                        }
                        else
                        {
                            if (item.Key == "iFirstReturnFeePayment" && item.Value == "正常待返")
                            {
                                commandsb.Append(" and fee.iFirstReturnFeePayment <> '已付' and ((hr.iEmployeeStatus = '在职' and fee.iFirstReturnFeeDate < getdate()) or (hr.iEmployeeStatus = '离职' and fee.iFirstReturnFeeDate < hr.iResignDate)) ");

                            }
                            else if (item.Key == "iSecondReturnFeePayment" && item.Value == "正常待返")
                            {
                                commandsb.Append(" and fee.iSecondReturnFeePayment <> '已付' and ((hr.iEmployeeStatus = '在职' and fee.iSecondReturnFeeDate < getdate()) or (hr.iEmployeeStatus = '离职' and fee.iSecondReturnFeeDate < hr.iResignDate)) ");
                            }
                            else if (item.Key == "iThirdReturnFeePayment" && item.Value == "正常待返")
                            {
                                commandsb.Append(" and fee.iThirdReturnFeePayment <> '已付' and ((hr.iEmployeeStatus = '在职' and fee.iThirdReturnFeeDate < getdate()) or (hr.iEmployeeStatus = '离职' and fee.iThirdReturnFeeDate < hr.iResignDate)) ");
                            }
                            else if (item.Key == "iFourthReturnFeePayment" && item.Value == "正常待返")
                            {
                                commandsb.Append(" and fee.iFourthReturnFeePayment <> '已付' and ((hr.iEmployeeStatus = '在职' and fee.iFourthReturnFeeDate < getdate()) or (hr.iEmployeeStatus = '离职' and fee.iFourthReturnFeeDate < hr.iResignDate)) ");
                            }
                            else if (item.Key == "iFifthReturnFeePayment" && item.Value == "正常待返")
                            {
                                commandsb.Append(" and fee.iFifthReturnFeePayment <> '已付' and ((hr.iEmployeeStatus = '在职' and fee.iFifthReturnFeeDate < getdate()) or (hr.iEmployeeStatus = '离职' and fee.iFifthReturnFeeDate < hr.iResignDate)) ");
                            }
                            else
                            {
                                commandsb.Append(" and " + item.Key + " like '%" + item.Value + "%'");
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(searchKey))
            {
                commandsb.Append(" and (");
                foreach (var item in Common.ConvertHelper.DicConvert(ReturnFeeDic))
                {
                    if (item.Key == "iGuid" || item.Key == "iOnJobDays") continue;
                    commandsb.Append(item.Key + " like '%" + searchKey + "%' or ");
                }
                commandsb.Remove(commandsb.Length - 3, 3);
                commandsb.Append(")");
            }

            string commonSql = commandsb.ToString();
            string querySql = "select DATEDIFF(day, hr.iEmployeeDate, hr.iResignDate)  as iOnJobDays, fee.*, hr.iItemName, hr.iCompany, hr.iEmpNo, hr.iName, hr.iIdCard,hr.iEmployeeDate, hr.iResignDate, hr.iGuid as iHRInfoGuid2, hr.iEmployeeStatus  " + commonSql + "order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<ReturnFeeModel>(querySql).ToList();

        }

        public List<ReturnFeeModel> GetSearchAll(string userType, Dictionary<string, string> para)
        {
            if (userType != "普通用户")
            {
                para["iCompany"] = para["iCompany"] == "-" ? "" : para["iCompany"];
                para["iItemName"] = para["iItemName"] == "-" ? "" : para["iItemName"];
            }
            StringBuilder commandsb = new StringBuilder("from ReturnFee fee right join hrinfo hr on fee.iHRInfoGuid = hr.iguid and fee.iIsDeleted =0 and fee.iStatus =1  where hr.iIsReturnFee = '是' and hr.iisdeleted=0 and hr.istatus=1 ");
            if (para["editType"] == "已编辑")
            {
                commandsb.Append("and fee.iGuid is not null ");
            }
            else if (para["editType"] == "未编辑")
            {
                commandsb.Append("and fee.iGuid is null ");
            }
            para.Remove("editType");
            string searchKey = para["search"];
            para.Remove("search");

            foreach (KeyValuePair<string, string> item in para)
            {
                if (!string.IsNullOrEmpty(item.Value) && item.Value != "§")
                {
                    if (item.Key.StartsWith("iFirst") && item.Key != "iFirstReturnFeePayment")
                    {
                        commandsb.Append(" and (");
                        if (item.Key.EndsWith("[d]"))
                        {
                            commandsb.Append(item.Key.Replace("[d]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "1900-01-01" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.Value.Split('§')[1]) + "' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iSecond").Replace("[d]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "1900-01-01" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.Value.Split('§')[1]) + "' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iThird").Replace("[d]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "1900-01-01" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.Value.Split('§')[1]) + "' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iFourth").Replace("[d]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "1900-01-01" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.Value.Split('§')[1]) + "' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iFifth").Replace("[d]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "1900-01-01" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.Value.Split('§')[1]) + "' ");
                        }
                        else
                        {
                            commandsb.Append(item.Key + " like '%" + item.Value + "%' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iSecond") + " like '%" + item.Value + "%' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iThird") + " like '%" + item.Value + "%' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iFourth") + " like '%" + item.Value + "%' or ");
                            commandsb.Append(item.Key.Replace("iFirst", "iFifth") + " like '%" + item.Value + "%' ");
                        }
                        commandsb.Append(" ) ");
                    }
                    else
                    {
                        if (item.Key.EndsWith("[d]"))
                        {
                            commandsb.Append(" and " + item.Key.Replace("[d]", "") + " between '" + (string.IsNullOrEmpty(item.Value.Split('§')[0]) ? "1900-01-01" : item.Value.Split('§')[0]) + "' and '" + (string.IsNullOrEmpty(item.Value.Split('§')[1]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : item.Value.Split('§')[1]) + "' ");
                        }
                        else
                        {
                            if (item.Key == "iFirstReturnFeePayment" && item.Value == "正常待返")
                            {
                                commandsb.Append(" and fee.iFirstReturnFeePayment <> '已付' and ((hr.iEmployeeStatus = '在职' and fee.iFirstReturnFeeDate < getdate()) or (hr.iEmployeeStatus = '离职' and fee.iFirstReturnFeeDate < hr.iResignDate)) ");

                            }
                            else if (item.Key == "iSecondReturnFeePayment" && item.Value == "正常待返")
                            {
                                commandsb.Append(" and fee.iSecondReturnFeePayment <> '已付' and ((hr.iEmployeeStatus = '在职' and fee.iSecondReturnFeeDate < getdate()) or (hr.iEmployeeStatus = '离职' and fee.iSecondReturnFeeDate < hr.iResignDate)) ");
                            }
                            else if (item.Key == "iThirdReturnFeePayment" && item.Value == "正常待返")
                            {
                                commandsb.Append(" and fee.iThirdReturnFeePayment <> '已付' and ((hr.iEmployeeStatus = '在职' and fee.iThirdReturnFeeDate < getdate()) or (hr.iEmployeeStatus = '离职' and fee.iThirdReturnFeeDate < hr.iResignDate)) ");
                            }
                            else if (item.Key == "iFourthReturnFeePayment" && item.Value == "正常待返")
                            {
                                commandsb.Append(" and fee.iFourthReturnFeePayment <> '已付' and ((hr.iEmployeeStatus = '在职' and fee.iFourthReturnFeeDate < getdate()) or (hr.iEmployeeStatus = '离职' and fee.iFourthReturnFeeDate < hr.iResignDate)) ");
                            }
                            else if (item.Key == "iFifthReturnFeePayment" && item.Value == "正常待返")
                            {
                                commandsb.Append(" and fee.iFifthReturnFeePayment <> '已付' and ((hr.iEmployeeStatus = '在职' and fee.iFifthReturnFeeDate < getdate()) or (hr.iEmployeeStatus = '离职' and fee.iFifthReturnFeeDate < hr.iResignDate)) ");
                            }
                            else
                            {
                                commandsb.Append(" and " + item.Key + " like '%" + item.Value + "%'");
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(searchKey))
            {
                commandsb.Append(" and (");
                foreach (var item in Common.ConvertHelper.DicConvert(ReturnFeeDic))
                {
                    if (item.Key == "iGuid" || item.Key == "iOnJobDays") continue;
                    commandsb.Append(item.Key + " like '%" + searchKey + "%' or ");
                }
                commandsb.Remove(commandsb.Length - 3, 3);
                commandsb.Append(")");
            }

            string commonSql = commandsb.ToString();
            string querySql = "select DATEDIFF(day, hr.iEmployeeDate, hr.iResignDate)  as iOnJobDays, fee.*, hr.iItemName, hr.iCompany, hr.iEmpNo, hr.iName, hr.iIdCard,hr.iEmployeeDate, hr.iResignDate, hr.iGuid as iHRInfoGuid2, hr.iEmployeeStatus " + commonSql + "order by fee.iUpdatedOn desc, hr.iUpdatedOn desc";
            return Repository.Query<ReturnFeeModel>(querySql).ToList();
        }

        public string GetValidReturnFeeHrId(string company, string empcode, string idcard, string itemName)
        {
            string sql = "select iGuid from HRInfo where iCompany='" + company + "' and iEmpNo = '" + empcode + "' and iIdCard ='" + idcard + "' and iIsReturnFee='是' and iIsDeleted =0 and iStatus=1 and iItemName = '" + itemName + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0][0].ToString();
            }
        }
        public ModifyLogEntity ModifyRecord(ReturnFeeEntity entity)
        {
            var oldEntity = FirstOrDefault(entity.iHRInfoGuid);
            if (oldEntity == null)
            {
                return null;
            }
            else
            {
                string modifiedContent = string.Empty;
                System.Reflection.PropertyInfo[] properties = entity.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                Dictionary<string, string> dicConvertTmp = ConvertHelper.DicConvert(ReturnFeeDic);

                foreach (System.Reflection.PropertyInfo item in properties)
                {
                    string name = item.Name;
                    if (dicConvertTmp.ContainsKey(name) && !dicConvertTmp[name].StartsWith("i"))
                    {
                        object value = item.GetValue(entity, null);
                        if (value == null) value = "";
                        object valueOld = item.GetValue(oldEntity, null);
                        if (valueOld == null) valueOld = "";
                        if (value.ToString() != valueOld.ToString() && (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String")))
                        {
                            modifiedContent += string.Format("{0}:[{1}]->[{2}] ;", dicConvertTmp[name], valueOld, value);
                        }
                    }
                }
                if (string.IsNullOrEmpty(modifiedContent))
                {
                    return null;
                }
                ModifyLogEntity en = new ModifyLogEntity();
                en.iId = entity.iGuid;
                en.iModifiedBy = entity.iUpdatedBy;
                en.iModifiedOn = DateTime.Now;
                en.iModifiedContent = modifiedContent;
                en.iTableName = "ReturnFee";
                return en;
            }
        }

        public List<ReturnFeeHistoryViewModel> GetValidReturnFeeList(string companyId, string projectId)
        {
            string sql = @"select fee.[iGuid]+'1' as iGuid
      ,fee.[iGuid] as iReturnFeeGuid
      ,[iHRInfoGuid]
      ,[iLaborName]
      ,[iLaborCampBank]
      ,[iLaborCampBankAccount]
      ,[iLaborCampBankPerson]
      ,'一级' as [iReturnFeeLevel]
      ,[iFirstReturnFeeAmount] as iReturnFeeAmount
      ,[iFirstReturnFeeDays]as iReturnFeeDays
      ,[iFirstReturnFeeDate]as iReturnFeeDate
      ,[iFirstReturnFeePayment]as iReturnFeePayment
      ,[iFirstReturnFeeActualPayDate]as iReturnFeeActualPayDate,hr.iCompany as iCompanyId, hr.iItemName as iProjectId, hr.iName as iName, hr.iIdCard as iIdCard
      ,fee.iInterviewDate, hr.iEmployeeDate, hr.iResignDate, hr.iEmployeeStatus
  from ReturnFee fee right join hrinfo hr on fee.iHRInfoGuid = hr.iguid and fee.iIsDeleted =0 and fee.iStatus =1  
  where hr.iIsReturnFee = '是' and hr.iisdeleted=0 and hr.istatus=1 and hr.iCompany = @companyid and hr.iItemName= @projectid
  and fee.iFirstReturnFeePayment <> '已付' and fee.iAdvice <> '暂不返费' and fee.iFirstAppNo is null
  and ((hr.iEmployeeStatus = '在职' and fee.iFirstReturnFeeDate < getdate()) or (hr.iEmployeeStatus = '离职' and fee.iFirstReturnFeeDate < hr.iResignDate))
  union all
  select fee.[iGuid]+'2' as iGuid
      ,fee.[iGuid] as iReturnFeeGuid
      ,[iHRInfoGuid]
      ,[iLaborName]
      ,[iLaborCampBank]
      ,[iLaborCampBankAccount]
      ,[iLaborCampBankPerson]
      ,'二级' as [iReturnFeeLevel]
      ,[iSecondReturnFeeAmount] as iReturnFeeAmount
      ,[iSecondReturnFeeDays]as iReturnFeeDays
      ,[iSecondReturnFeeDate]as iReturnFeeDate
      ,[iSecondReturnFeePayment]as iReturnFeePayment
      ,[iSecondReturnFeeActualPayDate]as iReturnFeeActualPayDate,hr.iCompany as iCompanyId, hr.iItemName as iProjectId, hr.iName as iName, hr.iIdCard as iIdCard
      ,fee.iInterviewDate, hr.iEmployeeDate, hr.iResignDate, hr.iEmployeeStatus
  from ReturnFee fee right join hrinfo hr on fee.iHRInfoGuid = hr.iguid and fee.iIsDeleted =0 and fee.iStatus =1  
  where hr.iIsReturnFee = '是' and hr.iisdeleted=0 and hr.istatus=1 and hr.iCompany = @companyid and hr.iItemName= @projectid
  and fee.iSecondReturnFeePayment <> '已付' and fee.iAdvice <> '暂不返费' and fee.iSecondAppNo is null
  and ((hr.iEmployeeStatus = '在职' and fee.iSecondReturnFeeDate < getdate()) or (hr.iEmployeeStatus = '离职' and fee.iSecondReturnFeeDate < hr.iResignDate))
  union all
  select fee.[iGuid]+'3' as iGuid
      ,fee.[iGuid] as iReturnFeeGuid
      ,[iHRInfoGuid]
      ,[iLaborName]
      ,[iLaborCampBank]
      ,[iLaborCampBankAccount]
      ,[iLaborCampBankPerson]
      ,'三级' as [iReturnFeeLevel]
      ,[iThirdReturnFeeAmount] as iReturnFeeAmount
      ,[iThirdReturnFeeDays]as iReturnFeeDays
      ,[iThirdReturnFeeDate]as iReturnFeeDate
      ,[iThirdReturnFeePayment]as iReturnFeePayment
      ,[iThirdReturnFeeActualPayDate]as iReturnFeeActualPayDate,hr.iCompany as iCompanyId, hr.iItemName as iProjectId, hr.iName as iName, hr.iIdCard as iIdCard
      ,fee.iInterviewDate, hr.iEmployeeDate, hr.iResignDate, hr.iEmployeeStatus
  from ReturnFee fee right join hrinfo hr on fee.iHRInfoGuid = hr.iguid and fee.iIsDeleted =0 and fee.iStatus =1  
  where hr.iIsReturnFee = '是' and hr.iisdeleted=0 and hr.istatus=1 and hr.iCompany = @companyid and hr.iItemName= @projectid
  and fee.iThirdReturnFeePayment <> '已付' and fee.iAdvice <> '暂不返费' and fee.iThirdAppNo is null
  and ((hr.iEmployeeStatus = '在职' and fee.iThirdReturnFeeDate < getdate()) or (hr.iEmployeeStatus = '离职' and fee.iThirdReturnFeeDate < hr.iResignDate))
  union all
  select fee.[iGuid]+'4' as iGuid
      ,fee.[iGuid] as iReturnFeeGuid
      ,[iHRInfoGuid]
      ,[iLaborName]
      ,[iLaborCampBank]
      ,[iLaborCampBankAccount]
      ,[iLaborCampBankPerson]
      ,'四级' as [iReturnFeeLevel]
      ,[iFourthReturnFeeAmount] as iReturnFeeAmount
      ,[iFourthReturnFeeDays]as iReturnFeeDays
      ,[iFourthReturnFeeDate]as iReturnFeeDate
      ,[iFourthReturnFeePayment]as iReturnFeePayment
      ,[iFourthReturnFeeActualPayDate]as iReturnFeeActualPayDate,hr.iCompany as iCompanyId, hr.iItemName as iProjectId, hr.iName as iName, hr.iIdCard as iIdCard
      ,fee.iInterviewDate, hr.iEmployeeDate, hr.iResignDate, hr.iEmployeeStatus
  from ReturnFee fee right join hrinfo hr on fee.iHRInfoGuid = hr.iguid and fee.iIsDeleted =0 and fee.iStatus =1  
  where hr.iIsReturnFee = '是' and hr.iisdeleted=0 and hr.istatus=1 and hr.iCompany = @companyid and hr.iItemName= @projectid
  and fee.iFourthReturnFeePayment <> '已付' and fee.iAdvice <> '暂不返费' and fee.iFourthAppNo is null
  and ((hr.iEmployeeStatus = '在职' and fee.iFourthReturnFeeDate < getdate()) or (hr.iEmployeeStatus = '离职' and fee.iFourthReturnFeeDate < hr.iResignDate))
  union all
  select fee.[iGuid]+'5' as iGuid
      ,fee.[iGuid] as iReturnFeeGuid
      ,[iHRInfoGuid]
      ,[iLaborName]
      ,[iLaborCampBank]
      ,[iLaborCampBankAccount]
      ,[iLaborCampBankPerson]
      ,'五级' as [iReturnFeeLevel]
      ,[iFifthReturnFeeAmount] as iReturnFeeAmount
      ,[iFifthReturnFeeDays]as iReturnFeeDays
      ,[iFifthReturnFeeDate]as iReturnFeeDate
      ,[iFifthReturnFeePayment]as iReturnFeePayment
      ,[iFifthReturnFeeActualPayDate]as iReturnFeeActualPayDate,hr.iCompany as iCompanyId, hr.iItemName as iProjectId, hr.iName as iName, hr.iIdCard as iIdCard
      ,fee.iInterviewDate, hr.iEmployeeDate, hr.iResignDate, hr.iEmployeeStatus
  from ReturnFee fee right join hrinfo hr on fee.iHRInfoGuid = hr.iguid and fee.iIsDeleted =0 and fee.iStatus =1  
  where hr.iIsReturnFee = '是' and hr.iisdeleted=0 and hr.istatus=1 and hr.iCompany = @companyid and hr.iItemName= @projectid
  and fee.iFifthReturnFeePayment <> '已付' and fee.iAdvice <> '暂不返费' and fee.iFifthAppNo is null
  and ((hr.iEmployeeStatus = '在职' and fee.iFifthReturnFeeDate < getdate()) or (hr.iEmployeeStatus = '离职' and fee.iFifthReturnFeeDate < hr.iResignDate))";

            return Repository.Query<ReturnFeeHistoryViewModel>(sql, new { companyid = companyId, projectid = projectId }).ToList();


        }

        public List<ReturnFeeHistoryViewModel> GetFlowReturnFeeHistory(string appNo)
        {
            string sql = @"select a.*, hr.iEmployeeDate, hr.iResignDate, hr.iEmployeeStatus, hr.iCompany as iCompanyId, hr.iItemName as iProjectId, hr.iName as iName, hr.iIdCard as iIdCard from returnfeeHistory a inner join hrinfo hr on a.iHRInfoGuid = hr.iGuid where iReturnFeeAppNo=@appno order by iLaborCampBankAccount asc";
            return Repository.Query<ReturnFeeHistoryViewModel>(sql, new { appno = appNo }).ToList();
        }

        //清掉所有当前流程标识的返费，以重新获取
        public bool ResetValidReturnFeeList(string appNo)
        {
            string sql1 = "update returnFee set iFirstAppNo = null where iFirstAppNo='" + appNo + "'";
            string sql2 = "update returnFee set iSecondAppNo=null where iSecondAppNo='" + appNo + "'";
            string sql3 = "update returnFee set iThirdAppNo=null where iThirdAppNo='" + appNo + "'";
            string sql4 = "update returnFee set iFourthAppNo=null where iFourthAppNo='" + appNo + "'";
            string sql5 = "update returnFee set iFifthAppNo=null where iFifthAppNo='" + appNo + "'";
            List<string> clearSqls = new List<string>();
            clearSqls.Add(sql1);
            clearSqls.Add(sql2);
            clearSqls.Add(sql3);
            clearSqls.Add(sql4);
            clearSqls.Add(sql5);
            DbHelperSQL.ExecuteSqlTran(clearSqls);
            return true;

        }

        //同一流程所有数据操作  发起流程
        public void BatchInsertReturnFeeHistoryApplication(List<ReturnFeeHistoryEntity> entities)
        {
            if (entities == null || entities.Count == 0)
                return;
            string appNo = entities[0].iReturnFeeAppNo;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                //var old = Repository.Query<ReturnFeeHistoryEntity>("select * from ReturnFeeHistory where iisdeleted=0 and istatus=1 and iReturnFeeAppNo='" + appNo + "'");
                //foreach (var item in old)
                //{
                //    Repository.Delete<ReturnFeeHistoryEntity>(session.Connection, item, session.Transaction);
                //}
                string removeSql = "delete from ReturnFeeHistory where iisdeleted=0 and istatus=1 and iReturnFeeAppNo='" + appNo + "'";
                Repository.Execute(session.Connection, removeSql, null, session.Transaction);
                foreach (var entity in entities)
                {
                    if (string.IsNullOrEmpty(entity.iGuid))
                        entity.iGuid = Guid.NewGuid().ToString();
                    entity.iCreatedOn = DateTime.Now;
                    entity.iUpdatedOn = DateTime.Now;
                    entity.iIsDeleted = 0;
                    entity.iStatus = 1;
                    Repository.Insert<ReturnFeeHistoryEntity>(session.Connection, entity, session.Transaction);
                    string updateSql = "";
                    if (entity.iReturnFeeLevel == "一级")
                    {
                        updateSql = "update returnfee set iFirstAppNo = '" + appNo + "' where iguid='" + entity.iReturnFeeGuid + "'";
                    }
                    else if (entity.iReturnFeeLevel == "二级")
                    {
                        updateSql = "update returnfee set iSecondAppNo = '" + appNo + "' where iguid='" + entity.iReturnFeeGuid + "'";
                    }
                    else if (entity.iReturnFeeLevel == "三级")
                    {
                        updateSql = "update returnfee set iThirdAppNo = '" + appNo + "' where iguid='" + entity.iReturnFeeGuid + "'";
                    }
                    else if (entity.iReturnFeeLevel == "四级")
                    {
                        updateSql = "update returnfee set iFourthAppNo = '" + appNo + "' where iguid='" + entity.iReturnFeeGuid + "'";
                    }
                    else if (entity.iReturnFeeLevel == "五级")
                    {
                        updateSql = "update returnfee set iFifthAppNo = '" + appNo + "' where iguid='" + entity.iReturnFeeGuid + "'";
                    }

                    Repository.Execute(session.Connection, updateSql, null, session.Transaction);
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

    }
}