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


                dic.Add("劳务名称", "iLaborName");
                dic.Add("劳务所银行支行", "iLaborCampBank");
                dic.Add("劳务所账号", "iLaborCampBankAccount");
                dic.Add("劳务所人姓名", "iLaborCampBankPerson");
                dic.Add("面试日期", "iInterviewDate");
                dic.Add("入职时间", "iEmployeeDate");
                dic.Add("离职日期", "iResignDate");
                dic.Add("在职天数", "iOnJobDays");

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

                dic.Add("iCreatedOn", "iCreatedOn");
                dic.Add("iCreatedBy", "iCreatedBy");
                dic.Add("iUpdatedOn", "iUpdatedOn");
                dic.Add("iUpdatedBy", "iUpdatedBy");
                dic.Add("iStatus", "iStatus");
                dic.Add("iIsDeleted", "iIsDeleted");
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
        public ReturnFeeEntity GetFirstOrDefault(string id)
        {
            string sql = @"select * from returnfee where iguid=@id and iIsDeleted =0 and iStatus =1";
            return Repository.Query<ReturnFeeEntity>(sql, new { id = id }).FirstOrDefault();
        }

        public List<ReturnFeeModel> GetSearch(string companyCode, Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            //StringBuilder commandsb = new StringBuilder("from HRInfo where icompany='");
            //commandsb.Append(companyCode);
            //commandsb.Append("' ");
            StringBuilder commandsb = new StringBuilder("from ReturnFee fee right join hrinfo hr on fee.iHRInfoGuid = hr.iguid and hr.iIsReturnFee = '是' where 1=1 ");
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
            string querySql = "select fee.*, hr.* " + commonSql + "order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            return Repository.Query<ReturnFeeModel>(querySql).ToList();

        }

        public List<ReturnFeeModel> GetSearchAll(string companyCode, Dictionary<string, string> para)
        {
            //StringBuilder commandsb = new StringBuilder("from HRInfo where icompany='");
            //commandsb.Append(companyCode);
            //commandsb.Append("' ");
            StringBuilder commandsb = new StringBuilder("from ReturnFee fee right join hrinfo hr on fee.iHRInfoGuid = hr.iguid and hr.iIsReturnFee = '是' where 1=1 ");
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
            return Repository.Query<ReturnFeeModel>(querySql).ToList();
        }

    }
}