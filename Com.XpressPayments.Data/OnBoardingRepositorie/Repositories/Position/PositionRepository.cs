using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.Repositories.Grade;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.Position
{
    public  class PositionRepository : IPositionRepository
    {
        private string _connectionString;
        private readonly ILogger<PositionRepository> _logger;
        private readonly IConfiguration _configuration;

        public PositionRepository(IConfiguration configuration, ILogger<PositionRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreatePosition(CreatePositionDTO create, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", PositionEnum.CREATE);
                    param.Add("@PositionName", create.PositionName.Trim());
                    param.Add("@CompanyID", create.CompanyID);

                    param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());
                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Position, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreatePosition(CreatePositionDTO create, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdatePosition(UpadtePositionDTO update, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", PositionEnum.UPDATE);
                    param.Add("@PositionIDUpd", update.PositionID);
                    param.Add("@PositionNameUpd", update.PositionName);
                    param.Add("@CompanyIdUpd", update.CompanyID);

                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Position, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdatePosition(UpadtePositionDTO update, string updatedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> Deleteposition(DeletePositionDTO delete, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", PositionEnum.DELETE);
                    param.Add("@PositionIDDelete", Convert.ToInt32(delete.PositionID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Deleting", delete.Reasons_For_Delete == null ? "" : delete.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Position, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Deleteposition(DeletePositionDTO delete, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PositionDTO>> GetAllActivePosition()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", PositionEnum.GETALLACTIVE);

                    var PositionDetails = await _dapper.QueryAsync<PositionDTO>(ApplicationConstant.Sp_Position, param: param, commandType: CommandType.StoredProcedure);

                    return PositionDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActivePosition()===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PositionDTO>> GetAllPosition()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", PositionEnum.GETALL);

                    var PositionDetails = await _dapper.QueryAsync<PositionDTO>(ApplicationConstant.Sp_Position, param: param, commandType: CommandType.StoredProcedure);

                    return PositionDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:GetAllPosition() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<PositionDTO> GetPositionById(long PositionID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", PositionEnum.GETBYID);
                    param.Add("@PositionIDGet", PositionID);

                    var PositionDetails = await _dapper.QueryFirstOrDefaultAsync<PositionDTO>(ApplicationConstant.Sp_Position, param: param, commandType: CommandType.StoredProcedure);

                    return PositionDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetPositionById(long PositionID)===>{ex.Message}");
                throw;
            }
        }

        public async Task<PositionDTO> GetPositionByName(string PositionName)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", PositionEnum.GETBYEMAIL);
                    param.Add("@PositionNameGet", PositionName);

                    var PositionDetails = await _dapper.QueryFirstOrDefaultAsync<PositionDTO>(ApplicationConstant.Sp_Position, param: param, commandType: CommandType.StoredProcedure);

                    return PositionDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetPositionByName(string PositionName) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PositionDTO>> GetAllPositionCompanyId(long PositionID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", 8);
                    param.Add("@CompanyIdGet", PositionID);

                    var PositionDetails = await _dapper.QueryAsync<PositionDTO>(ApplicationConstant.Sp_Position, param: param, commandType: CommandType.StoredProcedure);

                    return PositionDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllPositionCompanyId(long PositionID) ===>{ex.Message}");
                throw;
            }
        }


    }
}
