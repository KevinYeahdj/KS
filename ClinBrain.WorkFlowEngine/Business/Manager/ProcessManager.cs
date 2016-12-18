

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.WorkFlowEngine.Utility;
using ClinBrain.Data;
using ClinBrain.WorkFlowEngine.Business.Entity;
using ClinBrain.WorkFlowEngine.Business.Manager;
using ClinBrain.WorkFlowEngine.Xpdl;

namespace ClinBrain.WorkFlowEngine.Business.Manager
{
    /// <summary>
    /// 流程定义管理类
    /// </summary>
    public class ProcessManager : ManagerBase
    {
        #region 获取流程数据
        public ProcessEntity GetByGUID(string processGUID)
        {
            return Repository.GetDefaultByName<ProcessEntity>("ProcessGUID", processGUID);
        }

        public List<ProcessEntity> GetAll()
        {
            var list = Repository.GetAll<ProcessEntity>().ToList<ProcessEntity>();
            return list;
        }
        #endregion

        #region 新增、更新和删除流程数据
        public void Insert(ProcessEntity entity)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Insert<ProcessEntity>(session.Connection, entity, session.Transaction);
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

        public void Insert(IDbConnection conn, ProcessEntity entity, IDbTransaction trans)
        {
            Repository.Insert<ProcessEntity>(conn, entity, trans);
        }

        public void Update(ProcessEntity entity)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Repository.Update<ProcessEntity>(session.Connection, entity, session.Transaction);
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

        public void Delete(string processGUID)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                var entity = GetByGUID(processGUID);
                Repository.Delete<ProcessEntity>(session.Connection, entity, session.Transaction);
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

        public void Delete(IDbConnection conn, ProcessEntity entity, IDbTransaction trans)
        {
            Repository.Delete<ProcessEntity>(conn, entity, trans);
        }
        #endregion 
    }
}
