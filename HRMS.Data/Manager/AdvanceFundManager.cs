using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HRMS.Data.Entity;
using HRMS.Common;
using Dapper;

namespace HRMS.Data.Manager
{
    /// <summary>
    /// 备用金信息管理类
    /// </summary>
    public class AdvanceFundManager : ManagerBase
    {
        //建表dic
        public static Dictionary<string, string> AdvanceFundDic
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("iGuid", "iGuid");
                dic.Add("公司", "iCompanyId");
                dic.Add("项目", "iProjectId");
                dic.Add("提报人", "iApplicant");
                dic.Add("申请事由", "iReason");
                dic.Add("金额", "iAmount");
                dic.Add("是否核销", "iChecked");
                dic.Add("核销人", "iCheckedBy");
                dic.Add("支付日期", "iPaidDate");
                dic.Add("备注", "iNote");
                dic.Add("流程单号", "iAppNo");
                dic.Add("系统备注", "iRecordNote");

                //dic.Add("iCreatedOn", "iCreatedOn");
                //dic.Add("iCreatedBy", "iCreatedBy");
                //dic.Add("iUpdatedOn", "iUpdatedOn");
                //dic.Add("iUpdatedBy", "iUpdatedBy");
                //dic.Add("iStatus", "iStatus");
                //dic.Add("iIsDeleted", "iIsDeleted");
                return dic;
            }
        }

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Insert(AdvanceFundEntity entity)
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
                Repository.Insert<AdvanceFundEntity>(session.Connection, entity, session.Transaction);
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

        public void Update(AdvanceFundEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<AdvanceFundEntity>(session.Connection, entity, session.Transaction);
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

        public AdvanceFundEntity FirstOrDefault(string appno)
        {
            string sql = @"select * from AdvanceFund where iAppno=@appno and iIsDeleted =0 and iStatus =1";
            return Repository.Query<AdvanceFundEntity>(sql, new { appno = appno }).FirstOrDefault();
        }


        public List<AdvanceFundEntity> GetSearch(string userType, Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {

            StringBuilder commandsb = new StringBuilder("from AdvanceFund where iisdeleted=0 and istatus=1 ");

            string searchKey = para["search"];
            para.Remove("search");

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
            if (!string.IsNullOrEmpty(searchKey))
            {
                commandsb.Append(" and (");
                foreach (var item in Common.ConvertHelper.DicConvert(AdvanceFundDic))
                {
                    if (item.Value.StartsWith("i")) continue;  //去年不必要的比对
                    commandsb.Append(item.Key + " like '%" + searchKey + "%' or ");
                }
                commandsb.Remove(commandsb.Length - 3, 3);
                commandsb.Append(")");
            }

            string commonSql = commandsb.ToString();
            string querySql = "select * " + commonSql + "order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            List<AdvanceFundEntity> result = Repository.Query<AdvanceFundEntity>(querySql).ToList();
            return result;
        }

        public decimal GetTotalBill(string applicant)
        {
            string queryTotal = "select sum(iAmount) from AdvanceFund where iPaidDate is not null and iApplicant = '" + applicant + "' and iIsDeleted = 0 and iStatus =1";
            string total = DbHelperSQL.Query(queryTotal).Tables[0].Rows[0][0].ToString();
            if (string.IsNullOrEmpty(total))
                total = "0";
            return decimal.Parse(total);
        }
        public decimal GetTotalBillWithDate(string applicant, string untilDtStr)
        {
            string queryTotal = "select sum(iAmount) from AdvanceFund where iPaidDate is not null and iApplicant = '" + applicant + "' and iIsDeleted = 0 and iStatus =1";
            if (string.IsNullOrEmpty(applicant))
            {
                return 0;   //如果不是针对某个人的查询，没办法汇总，直接返回0吧
            }
            if (untilDtStr != null)
            {
                queryTotal += " and ISNULL(iPaidDate, iCreatedOn) <= '" + untilDtStr + "'";
            }
            string total = DbHelperSQL.Query(queryTotal).Tables[0].Rows[0][0].ToString();
            if (string.IsNullOrEmpty(total))
                total = "0";
            return decimal.Parse(total);
        }
    }
}