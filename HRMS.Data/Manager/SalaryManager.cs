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
    /// 工资信息管理类
    /// </summary>
    public class SalaryManager : ManagerBase
    {
        //建表dic
        public static Dictionary<string, string> SalaryDic
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("iGuid", "iGuid");
                dic.Add("公司", "iCompanyId");
                dic.Add("项目", "iProjectId");
                dic.Add("工资发放年月", "iYearMonth");
                dic.Add("文件名称", "iFileName");
                dic.Add("下载地址", "iUrl");
                dic.Add("内容", "iContent");
                dic.Add("备注", "iNote");
                dic.Add("单号", "iAppNo");
                dic.Add("审核状态", "iApproveStatus");
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
        public void Insert(SalaryEntity entity)
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
                Repository.Insert<SalaryEntity>(session.Connection, entity, session.Transaction);
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

        public void Update(SalaryEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<SalaryEntity>(session.Connection, entity, session.Transaction);
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

        //同一流程所有数据操作
        public void BatchUpdate4Flow(List<SalaryEntity> entities, string appNo)
        {
            if (entities == null || entities.Count == 0)
            {
                DbHelperSQL.ExecuteSql("update Salary set iRecordStatus = '草稿', iAppNo = '' where iisdeleted=0 and istatus=1 and iAppNo='" + appNo + "'");
                return;

            }
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                List<SalaryEntity> upds = new List<SalaryEntity>();
                foreach (var entity in entities)
                {
                    var item = Repository.GetById<SalaryEntity>(entity.iGuid);
                    item.iApproveStatus = "待审核";
                    item.iAppNo = appNo;
                    upds.Add(item);
                }
                Repository.UpdateBatch<SalaryEntity>(session.Connection, upds, session.Transaction);
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

        public SalaryEntity FirstOrDefault(string guid)
        {
            string sql = @"select * from Salary where iGuid=@id and iIsDeleted =0 and iStatus =1";
            return Repository.Query<SalaryEntity>(sql, new { id = guid }).FirstOrDefault();
        }

        public List<SalaryEntity> GetSearch(string userType, Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            if (userType != "普通用户")
            {
                para["iCompanyId"] = para["iCompanyId"] == "-" ? "" : para["iCompanyId"];
                para["iProjectId"] = para["iProjectId"] == "-" ? "" : para["iProjectId"];
            }
            StringBuilder commandsb = new StringBuilder("from Salary where iisdeleted=0 and istatus=1 ");

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
                foreach (var item in Common.ConvertHelper.DicConvert(SalaryDic))
                {
                    if (item.Value.StartsWith("i")) continue;  //去掉不必要的比对
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
            List<SalaryEntity> result = Repository.Query<SalaryEntity>(querySql).ToList();
            return result;
        }

    }
}