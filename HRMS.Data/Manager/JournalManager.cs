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
                dic.Add("公司", "iCompanyId");
                dic.Add("项目", "iProjectId");
                dic.Add("日期", "iDate");
                dic.Add("事项", "iEvent");
                dic.Add("科目", "iType");
                dic.Add("提报人", "iApplicant");
                dic.Add("金额", "iAmount");
                dic.Add("是否核销", "iChecked");
                dic.Add("核销人", "iCheckedBy");
                dic.Add("支付日期", "iPaidDate");
                dic.Add("备注", "iNote");
                dic.Add("流程单号", "iAppNo");
                dic.Add("流水账性质", "iRecordStatus");

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
        public void Insert(JournalEntity entity)
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
                Repository.Insert<JournalEntity>(session.Connection, entity, session.Transaction);
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
        public void BatchInsert(List<JournalEntity> entities)
        {
            if (entities == null || entities.Count == 0)
                return;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                var old = Repository.Query<JournalEntity>("select * from Journal where iisdeleted=0 and istatus=1 and iAppNo='" + entities[0].iAppNo + "'");
                foreach (var item in old)
                {
                    Repository.Delete<JournalEntity>(session.Connection, item, session.Transaction);
                }
                foreach (var entity in entities)
                {
                    if (string.IsNullOrEmpty(entity.iGuid))
                        entity.iGuid = Guid.NewGuid().ToString();
                    entity.iCreatedOn = DateTime.Now;
                    entity.iUpdatedOn = DateTime.Now;
                    entity.iIsDeleted = 0;
                    entity.iStatus = 1;
                    Repository.Insert<JournalEntity>(session.Connection, entity, session.Transaction);
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

        //同一流程所有数据操作
        public void BatchUpdate(List<JournalEntity> entities, string appNo)
        {
            DbHelperSQL.ExecuteSql("update Journal set iRecordStatus = '草稿', iAppNo = '' where iisdeleted=0 and istatus=1 and iAppNo='" + appNo + "'");
            if (entities == null || entities.Count == 0)
                return;
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                List<JournalEntity> upds = new List<JournalEntity>();
                foreach (var entity in entities)
                {
                    var item = Repository.GetById<JournalEntity>(entity.iGuid);
                    item.iRecordStatus = "正式";
                    item.iAppNo = appNo;
                    upds.Add(item);
                }
                Repository.UpdateBatch<JournalEntity>(session.Connection, upds, session.Transaction);
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
        public void Update(JournalEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            var record = ModifyRecord(entity);
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<JournalEntity>(session.Connection, entity, session.Transaction);
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

        public List<JournalEntity> GetSearch(string userType, Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            if (userType != "普通用户")
            {
                para["iCompanyId"] = para["iCompanyId"] == "-" ? "" : para["iCompanyId"];
                para["iProjectId"] = para["iProjectId"] == "-" ? "" : para["iProjectId"];
            }
            StringBuilder commandsb = new StringBuilder("from Journal where iisdeleted=0 and istatus=1 ");

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
                foreach (var item in Common.ConvertHelper.DicConvert(JournalDic))
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
            string sumSql = "select CONVERT(varchar(100),sum(iAmount)) " + commonSql;
            decimal sum = 0;
            decimal.TryParse(Repository.Query<string>(sumSql).ToList()[0], out sum);
            List<JournalEntity> result = Repository.Query<JournalEntity>(querySql).ToList();
            result.Add(new JournalEntity { iAmount = sum });  //多加一列传递总值，记得删除
            return result;
        }

        public List<JournalEntity> GetMyJournalDraft(string userType, Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            //StringBuilder commandsb = new StringBuilder(" from Journal where iChecked<>'是' and iisdeleted=0 and istatus=1 and iRecordStatus='草稿' and iCreatedBy ='" + para["currentUserId"] + "' and iCompanyId = '" + para["iCompanyId"] + "' and iProjectId='" + para["iProjectId"] + "'");
            StringBuilder commandsb = new StringBuilder(" from Journal where iChecked<>'是' and iisdeleted=0 and istatus=1 and iRecordStatus='草稿' and iCreatedBy ='" + para["currentUserId"] + "' and iCompanyId = '" + para["iCompanyId"] + "'");


            string commonSql = commandsb.ToString();
            string querySql = "select * " + commonSql + "order by {0} {1} offset {2} row fetch next {3} rows only";
            querySql = string.Format(querySql, sort, order, offset, pageSize);
            string totalSql = "select cast(count(1) as varchar(8)) " + commonSql;
            total = int.Parse(Repository.Query<string>(totalSql).ToList()[0]);
            string sumSql = "select CONVERT(varchar(100),sum(iAmount)) " + commonSql;
            decimal sum = 0;
            decimal.TryParse(Repository.Query<string>(sumSql).ToList()[0], out sum);
            List<JournalEntity> result = Repository.Query<JournalEntity>(querySql).ToList();
            result.Add(new JournalEntity { iAmount = sum });  //多加一列传递总值，记得删除
            return result;
        }

        public JournalEntity FirstOrDefault(string guid)
        {
            string sql = @"select * from Journal where iGuid=@id and iIsDeleted =0 and iStatus =1";
            return Repository.Query<JournalEntity>(sql, new { id = guid }).FirstOrDefault();
        }

        //清掉所有当前流程标识的流水账，以重新获取
        public bool ResetFlowJournal(string appNo)
        {
            string sql = "update Journal set iAppNo = null, iRecordStatus ='草稿' where iAppNo='" + appNo + "'";
            DbHelperSQL.ExecuteSql(sql);
            return true;

        }
        public ModifyLogEntity ModifyRecord(JournalEntity entity)
        {
            var oldEntity = FirstOrDefault(entity.iGuid);
            if (oldEntity == null)
            {
                return null;
            }
            else
            {
                string modifiedContent = string.Empty;
                System.Reflection.PropertyInfo[] properties = entity.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                Dictionary<string, string> dicConvertTmp = ConvertHelper.DicConvert(JournalDic);

                foreach (System.Reflection.PropertyInfo item in properties)
                {
                    string name = item.Name;
                    if (!dicConvertTmp[name].StartsWith("i"))
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
                en.iTableName = "Journal";
                return en;
            }
        }

        public bool UpdateNoteByFlow(string note, string appno)
        {
            string sql = "update Journal set iNote = iNote+'【" + note + "】' where iAppNo='" + appno + "' ";
            if (DbHelperSQL.ExecuteSql(sql) > 0)
                return true;
            else return false;
        }
    }
}