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
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<ReturnFeeEntity>(session.Connection, entity, session.Transaction);
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

            foreach (KeyValuePair<string, string> item in para)
            {
                if (!string.IsNullOrEmpty(item.Value) && item.Value != "§")
                {
                    if (item.Key.StartsWith("iFirst"))
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
                            commandsb.Append(" and " + item.Key + " like '%" + item.Value + "%'");
                        }
                    }
                }
            }


            string commonSql = commandsb.ToString();
            string querySql = "select DATEDIFF(day, hr.iEmployeeDate, hr.iResignDate)  as iOnJobDays, fee.*, hr.iItemName, hr.iCompany, hr.iEmpNo, hr.iName, hr.iIdCard,hr.iEmployeeDate, hr.iResignDate, hr.iGuid as iHRInfoGuid2 " + commonSql + "order by {0} {1} offset {2} row fetch next {3} rows only";
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

            foreach (KeyValuePair<string, string> item in para)
            {
                if (!string.IsNullOrEmpty(item.Value) && item.Value != "§")
                {
                    if (item.Key.StartsWith("iFirst"))
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
                            commandsb.Append(" and " + item.Key + " like '%" + item.Value + "%'");
                        }
                    }
                }
            }
            string commonSql = commandsb.ToString();
            string querySql = "select DATEDIFF(day, hr.iEmployeeDate, hr.iResignDate)  as iOnJobDays, fee.*, hr.iItemName, hr.iCompany, hr.iEmpNo, hr.iName, hr.iIdCard,hr.iEmployeeDate, hr.iResignDate, hr.iGuid as iHRInfoGuid2 " + commonSql + "order by fee.iUpdatedOn desc, hr.iUpdatedOn desc";
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

    }
}