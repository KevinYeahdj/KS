﻿using System;
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
    /// 合同 管理类
    /// </summary>
    public class ContractManager : ManagerBase
    {
        //建表dic
        public static Dictionary<string, string> ContractDic
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("iGuid", "iGuid");
                dic.Add("公司ID", "iCompanyId");
                dic.Add("项目ID", "iProjectId");
                dic.Add("公司", "iCompanyName");
                dic.Add("项目", "iProjectName");
                dic.Add("合同起始日期", "iStartDate");
                dic.Add("合同终止日期", "iEndDate");
                dic.Add("合同性质", "iProperty");
                dic.Add("企业联系人", "iEnterpriseContact");
                dic.Add("企业联系电话", "iEnterpriseTel");
                dic.Add("企业联系手机", "iEnterprisePhone");
                dic.Add("邮寄地址", "iMailAddress");
                dic.Add("邮箱地址", "iEmailAddress");
                dic.Add("现场负责人", "iPersonInChargeOnSpot");
                dic.Add("现场负责人电话", "iPersonInChargeOnSpotTel");
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
        public void Insert(ContractEntity entity)
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
                Repository.Insert<ContractEntity>(session.Connection, entity, session.Transaction);
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
        public void Update(ContractEntity entity)
        {
            entity.iUpdatedOn = DateTime.Now;
            var record = ModifyRecord(entity);
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<ContractEntity>(session.Connection, entity, session.Transaction); 
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

        public List<ContractEntity> GetSearch(string userType, Dictionary<string, string> para, string sort, string order, int offset, int pageSize, out int total)
        {
            //if (userType != "普通用户")
            //{
            //    para["iCompanyId"] = para["iCompanyId"] == "-" ? "" : para["iCompanyId"];
            //    para["iProjectId"] = para["iProjectId"] == "-" ? "" : para["iProjectId"];
            //}
            StringBuilder commandsb = new StringBuilder("from Contract where iisdeleted=0 and istatus=1 ");

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
                foreach (var item in Common.ConvertHelper.DicConvert(ContractDic))
                {
                    if (item.Value.StartsWith("i")) continue;  
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
            List<ContractEntity> result = Repository.Query<ContractEntity>(querySql).ToList();
            return result;
        }

        public ContractEntity FirstOrDefault(string guid)
        {
            string sql = @"select * from Contract where iGuid=@id and iIsDeleted =0 and iStatus =1";
            return Repository.Query<ContractEntity>(sql, new { id = guid }).FirstOrDefault();
        }
        public ModifyLogEntity ModifyRecord(ContractEntity entity)
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

                Dictionary<string, string> dicConvertTmp = ConvertHelper.DicConvert(ContractDic);

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
                en.iTableName = "Contract";
                return en;
            }
        }
    }
}