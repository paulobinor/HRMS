using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Com.XpressPayments.Data.DapperGeneric
{
    public class DapperGenericRepository : IDapperGenericRepository
    {
        private readonly IConfiguration _config;
        private string Connectionstring = "DefaultConnection";

        public DapperGenericRepository(IConfiguration config)
        {
            _config = config;
        }
        public void Dispose()
        {

        }

        public async Task<T> Execute<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var trans = db.BeginTransaction();
                try
                {
                    result = db.ExecuteScalar<T>(sp, parms, commandType: commandType, transaction: trans);
                    if (trans.Connection != null)
                        trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public async Task<T> Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {

            // using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            //return (await db.QueryAsync<T>(sp, parms, commandType: commandType)).FirstOrDefault();

            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            if (db.State == ConnectionState.Closed)
                db.Open();
            using (var tran = db.BeginTransaction())
            {
                try
                {
                    result = (await db.QueryAsync<T>(sp, parms, commandType: commandType, transaction: tran)).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        public async Task<List<T>> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            //using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            //return (await db.QueryAsync<T>(sp, parms, commandType: commandType)).ToList();


            List<T> result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));

            if (db.State == ConnectionState.Closed)
                db.Open();
            using (var tran = db.BeginTransaction())
            {
                try
                {
                    result = (await db.QueryAsync<T>(sp, parms, commandType: commandType, transaction: tran)).ToList();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }

            return result;
        }

        public async Task<T> BulkInsert<T>(DataTable dataTable, object parameter, string procedurename)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                using var tran = db.BeginTransaction();
                try
                {
                    result = await db.ExecuteScalarAsync<T>(procedurename, parameter, commandType: CommandType.StoredProcedure, transaction: tran);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return result;
        }
        public DbConnection GetDbconnection()
        {
            return new SqlConnection(_config.GetConnectionString(Connectionstring));
        }

        public async Task<T> Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public async Task<T> Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = (await db.QueryAsync<T>(sp, parms, commandType: commandType, transaction: tran)).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> GetMultiple<T1, T2>(string sql, DynamicParameters parameters,
                                        Func<GridReader, IEnumerable<T1>> func1,
                                        Func<GridReader, IEnumerable<T2>> func2, CommandType commandType = CommandType.StoredProcedure)
        {
            var objs = await getMultiple(sql, parameters, commandType, func1, func2);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>);
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>> GetMultiple<T1, T2, T3>(string sql, DynamicParameters parameters,
                                        Func<GridReader, IEnumerable<T1>> func1,
                                        Func<GridReader, IEnumerable<T2>> func2,
                                        Func<GridReader, IEnumerable<T3>> func3,
                                        CommandType commandType = CommandType.StoredProcedure)
        {
            var objs = await getMultiple(sql, parameters, commandType, func1, func2, func3);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>, objs[2] as IEnumerable<T3>);
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>>> GetMultiple<T1, T2, T3, T4>(string sql, DynamicParameters parameters,
                                       Func<GridReader, IEnumerable<T1>> func1,
                                       Func<GridReader, IEnumerable<T2>> func2,
                                       Func<GridReader, IEnumerable<T3>> func3,
                                        Func<GridReader, IEnumerable<T4>> func4,
                                       CommandType commandType = CommandType.StoredProcedure)
        {
            var objs = await getMultiple(sql, parameters, commandType, func1, func2, func3, func4);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>, objs[2] as IEnumerable<T3>, objs[3] as IEnumerable<T4>);
        }
        private async Task<List<object>> getMultiple(string sp, DynamicParameters parameters, CommandType commandType, params Func<GridReader, object>[] readerFuncs)
        {
            var returnResults = new List<object>();
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                List<dynamic> gridResult = new List<dynamic>();
                if (db.State == ConnectionState.Closed)
                    db.Open();

                //using var tran = db.BeginTransaction();
                try
                {
                    var gridReader = await db.QueryMultipleAsync(sp, parameters, commandType: commandType);
                    //tran.Commit();

                    foreach (var readerFunc in readerFuncs)
                    {
                        var obj = readerFunc(gridReader);
                        returnResults.Add(obj);
                    }
                }
                catch (Exception ex)
                {
                    //tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return returnResults;
        }
    }
}
