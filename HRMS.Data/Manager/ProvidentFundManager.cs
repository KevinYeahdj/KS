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
    /// 公积金信息管理类
    /// </summary>
    public class ProvidentFundManager : ManagerBase
    {
        //建表dic
        public static Dictionary<string, string> ProvidentFundDic
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("iGuid", "iGuid");
                dic.Add("人事信息标识", "iHRInfoGuid");
                dic.Add("缴纳地", "iPayPlace");
                dic.Add("员工意愿", "iEmployeeWilling");
                dic.Add("缴费基数", "iPayBase");
                dic.Add("转入日期", "iEntryDate");
                dic.Add("封存日期", "iSealDate");
                dic.Add("个人缴费金额", "iIndividualAmount");
                dic.Add("公司缴费金额", "iCompanyAmount");
                dic.Add("补缴金额", "iAdditionalAmount");
                dic.Add("补缴月数", "iAdditionalMonths");
                dic.Add("商业保险缴纳公司", "iSocialInsurancePaidCompany");
                
                dic.Add("备注1", "iNote1");
                dic.Add("备注2", "iNote2");

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