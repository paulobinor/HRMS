using Dapper;
using System.Data;
using System.Data.Common;
using static Dapper.SqlMapper;

namespace hrms_be_backend_data.IRepository
{
    public interface IDapperGenericRepository
    {
        DbConnection GetDbconnection();
        Task<T> Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<List<T>> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<T> Execute<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<T> Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<T> Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<T> BulkInsert<T>(object parameter, string procedurename);
        Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> GetMultiple<T1, T2>(string sql, DynamicParameters parameters,
                                        Func<GridReader, IEnumerable<T1>> func1,
                                        Func<GridReader, IEnumerable<T2>> func2, CommandType commandType = CommandType.StoredProcedure);
        Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>> GetMultiple<T1, T2, T3>(string sql, DynamicParameters parameters,
                                        Func<GridReader, IEnumerable<T1>> func1,
                                        Func<GridReader, IEnumerable<T2>> func2,
                                        Func<GridReader, IEnumerable<T3>> func3,
                                        CommandType commandType = CommandType.StoredProcedure);

        Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>>> GetMultiple<T1, T2, T3, T4>(string sql, DynamicParameters parameters,
                                        Func<GridReader, IEnumerable<T1>> func1,
                                        Func<GridReader, IEnumerable<T2>> func2,
                                        Func<GridReader, IEnumerable<T3>> func3,
                                         Func<GridReader, IEnumerable<T4>> func4,
                                        CommandType commandType = CommandType.StoredProcedure);
        Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>>> GetMultiple<T1, T2, T3, T4, T5>(string sql, DynamicParameters parameters,
                                     Func<GridReader, IEnumerable<T1>> func1,
                                     Func<GridReader, IEnumerable<T2>> func2,
                                     Func<GridReader, IEnumerable<T3>> func3,
                                      Func<GridReader, IEnumerable<T4>> func4,
                                      Func<GridReader, IEnumerable<T5>> func5,
                                     CommandType commandType = CommandType.StoredProcedure);

        Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>>> GetMultiple<T1, T2, T3, T4, T5, T6>(string sql, DynamicParameters parameters,
                                     Func<GridReader, IEnumerable<T1>> func1,
                                     Func<GridReader, IEnumerable<T2>> func2,
                                     Func<GridReader, IEnumerable<T3>> func3,
                                      Func<GridReader, IEnumerable<T4>> func4,
                                      Func<GridReader, IEnumerable<T5>> func5,
                                        Func<GridReader, IEnumerable<T6>> func6,
                                     CommandType commandType = CommandType.StoredProcedure);
    }
}
