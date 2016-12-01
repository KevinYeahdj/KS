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
    /// 流水账信息管理类
    /// </summary>
    public class JournalManager : ManagerBase
    {
        //建表dic
        public static Dictionary<string, string> JournalDic
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("iGuid", "iGuid");
                dic.Add("项目", "iProjectId");
                dic.Add("日期", "iDate");
                dic.Add("事项", "iEvent");
                dic.Add("科目", "iType");
                dic.Add("金额", "iAmount");
                dic.Add("是否核销", "iChecked");
                dic.Add("支付日期", "iPaidDate");
                dic.Add("备注", "iNote");

                dic.Add("iCreatedOn", "iCreatedOn");
                dic.Add("iCreatedBy", "iCreatedBy");
                dic.Add("iUpdatedOn", "iUpdatedOn");
                dic.Add("iUpdatedBy", "iUpdatedBy");
                dic.Add("iStatus", "iStatus");
                dic.Add("iIsDeleted", "iIsDeleted");
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

    }
}