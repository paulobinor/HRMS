using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;
using XpressHRMS.Data.Enums;
using XpressHRMS.Data.IRepository;
using XpressHRMS.IRepository;

namespace XpressHRMS.Data.Repository
{
     public class GradeRepository : IGradeRepository
    {

        private readonly ILogger<GradeRepository> _logger;
        private readonly IDapperGeneric _dapper;
        private readonly string _connectionString;

        public GradeRepository(ILogger<GradeRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("HRMSConnectionString");


        }

        public async Task<int> CreateGrade(CreateGradeDTO createGrade)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.INSERT);
                    param.Add("@CompanyID", createGrade.CompanyID);
                    param.Add("@GradeName", createGrade.GradeName);
                    dynamic response = await _dapper.ExecuteAsync("sp_Grade", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<int> UpdateGrade(UpdateGradeDTO UpdateGrade)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.UPDATE);
                    param.Add("@CompanyIDUpd", UpdateGrade.CompanyID);
                    param.Add("@GradeIDUpd", UpdateGrade.GradeID);
                    param.Add("@GradeNameUpd", UpdateGrade.GradeName);
                

                    dynamic response = await _dapper.ExecuteAsync("sp_Grade", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<int> DeleteGrade(DelGradeDTO deleteGrade)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DELETE);
                    param.Add("@CompanyIDDel", deleteGrade.CompanyID);
                    param.Add("@GradeIDDel", deleteGrade.GradeID);
                    dynamic response = await _dapper.ExecuteAsync("sp_Grade", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<int> DisableGrade(int GradeID, int CompanyIDDis)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DISABLE);
                    param.Add("@GradeID", GradeID);
                    param.Add("@CompanyIDDis", CompanyIDDis);
                    dynamic response = await _dapper.ExecuteAsync("sp_Grade", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<int> ActivateGrade(int GradeID, int CompanyIDEna)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.ACTIVATE);
                    param.Add("@GradeID", GradeID);
                    param.Add("@CompanyIDEna", CompanyIDEna);
                    dynamic response = await _dapper.ExecuteAsync("sp_Grade", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<IEnumerable<GradeDTO>> GetAllGrades(int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    int d = (int)GetAllDefault.GetAll;
                    param.Add("@Status", ACTION.SELECTALL);
                    param.Add("@CompanyIDGet", CompanyID);
                    var response = await _dapper.QueryAsync<GradeDTO>("sp_Grade", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<IEnumerable<GradeDTO>> GetGradeByID(int CompanyID, int GradeID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    int d = (int)GetAllDefault.GetAll;
                    param.Add("@Status", ACTION.SELECTBYID);
                    param.Add("@CompanyIDGet", CompanyID);
                    param.Add("@GradeIDGet", GradeID);
                    var response = await _dapper.QueryAsync<GradeDTO>("sp_Grade", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }

        }

    }
}
