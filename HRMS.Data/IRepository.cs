﻿using System;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using System.Data.SqlClient;

namespace HRMS.Data
{
    /// <summary>
    /// Data Repository
    /// Implement Select, Insert, Update, Delete
    /// </summary>
    public interface IRepository
    {
        //select
        T GetById<T>(dynamic primaryId) where T : class;
        T GetById<T>(IDbConnection conn, dynamic primaryId, IDbTransaction trans) where T : class;
        T GetDefaultByName<T>(string colName, string value) where T : class;
        IEnumerable<T> GetByIds<T>(IList<dynamic> ids) where T : class;
        IEnumerable<T> GetAll<T>() where T : class;
        IEnumerable<T> Query<T>(string sql, dynamic param = null, bool buffered = true) where T : class;
        IEnumerable<T> Query<T>(IDbConnection conn, string sql, dynamic param = null, IDbTransaction trans = null, bool buffered = true) where T : class;
        IEnumerable<dynamic> Query(IDbConnection conn, string sql, dynamic param = null, IDbTransaction trans = null, bool buffered = true);
        IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(IDbConnection conn, string sql, Func<TFirst, TSecond, TReturn> map,
             dynamic param = null, IDbTransaction transaction = null, bool buffered = true,
            string splitOn = "Id", int? commandTimeout = null);
        //SqlMapper.GridReader GetMultiple(string sql, IDbConnection conn, dynamic param = null, 
        //    IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        //count
        int Count<T>(IDbConnection conn, IPredicate predicate, bool buffered = false) where T : class;
        int Count<T>(IDbConnection conn, string sql, bool buffered = false) where T : class;
        int Count(string sql, DynamicParameters parameters = null);

        //lsit
        IEnumerable<T> GetList<T>(IDbConnection conn, IPredicate predicate = null,
            IList<ISort> sort = null, bool buffered = false) where T : class;

        //paged select
        //IEnumerable<T> GetPage<T>(IDbConnection conn, int pageIndex, int pageSize, out int allRowsCount,  
        //    string sql, ISort sort = null, bool buffered = true) where T : class;
        //IEnumerable<T> GetPage<T>(IDbConnection conn, int pageIndex, int pageSize, out int allRowsCount, 
        //    string sql, IList<ISort> sort = null, bool buffered = true) where T : class;

        //execute
        Int32 Execute(IDbConnection conn, string sql, dynamic param = null, IDbTransaction transaction = null);
        Int32 ExecuteCommand(IDbCommand cmd);

        //insert, update, delete
        dynamic Insert<T>(IDbConnection conn, T entity, IDbTransaction transaction = null) where T : class;
        void InsertBatch<T>(IDbConnection conn, IEnumerable<T> entityList, IDbTransaction transaction = null, string tableName = null) where T : class;
        bool Update<T>(IDbConnection conn, T entity, IDbTransaction transaction = null) where T : class;
        bool UpdateBatch<T>(IDbConnection conn, IEnumerable<T> entityList, IDbTransaction transaction = null) where T : class;
        bool Delete<T>(IDbConnection conn, dynamic primaryId, IDbTransaction transaction = null) where T : class;
        bool Delete<T>(IDbConnection conn, IPredicate predicate, IDbTransaction transaction = null) where T : class;
        bool DeleteBatch<T>(IDbConnection conn, IEnumerable<dynamic> ids, IDbTransaction transaction = null) where T : class;

        DataSet Query(string SQLString, params SqlParameter[] cmdParms);
    }
}
