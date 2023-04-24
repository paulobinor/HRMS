using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.Repositories.Unit;
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

namespace Com.XpressPayments.Data.Repositories.UnitHead
{
    public class UnitHeadRepository : IUnitHeadRepository
    {
        private string _connectionString;
        private readonly ILogger<UnitHeadRepository> _logger;
        private readonly IConfiguration _configuration;

        public UnitHeadRepository(IConfiguration configuration, ILogger<UnitHeadRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateUnitHead(CreateUnitHeadDTO create, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", UnitHeadEnum.CREATE);
                    param.Add("@UnitHeadName", create.UnitHeadName.Trim());
                    param.Add("@HodID", create.HodID);
                    param.Add("@DepartmentID", create.DepartmentID);
                    param.Add("@CompanyId", create.CompanyID);

                    param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UnitHead, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateUnitHead(CreateUnitHeadDTO create, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateUnitHead(UpdateUnitHeadDTO update, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", UnitHeadEnum.UPDATE);
                    param.Add("@UnitHeadIDUpd", update.UnitHeadID);
                    param.Add("@UnitHeadNameUpd", update.UnitHeadName.Trim());
                    param.Add("@HodIDUpd", update.HodID);
                    param.Add("@DepartmentIDUpd", update.DepartmentID);
                    param.Add("@CompanyIdUpd", update.CompanyID);

                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UnitHead, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateUnitHead(UpdateUnitHeadDTO update, string updatedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteUnitHead(DeleteUnitHeadDTO delete, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", UnitHeadEnum.DELETE);
                    param.Add("@UnitHeadIDDelete", Convert.ToInt32(delete.UnitHeadID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Deleting", delete.Reasons_For_Delete == null ? "" : delete.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UnitHead, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteUnitHead(DeleteUnitHeadDTO delete, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<UnitHeadDTO>> GetAllActiveUnitHead()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", UnitHeadEnum.GETALLACTIVE);

                    var UnitHeadDetails = await _dapper.QueryAsync<UnitHeadDTO>(ApplicationConstant.Sp_UnitHead, param: param, commandType: CommandType.StoredProcedure);

                    return UnitHeadDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActiveUnitHead() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<UnitHeadDTO>> GetAllUnitHead()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", UnitHeadEnum.GETALL);

                    var UnitHeadDetails = await _dapper.QueryAsync<UnitHeadDTO>(ApplicationConstant.Sp_UnitHead, param: param, commandType: CommandType.StoredProcedure);

                    return UnitHeadDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:GetAllUnitHead() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<UnitHeadDTO> GetUnitHeadById(long UnitHeadID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", UnitHeadEnum.GETBYID);
                    param.Add("@UnitHeadIDGet", UnitHeadID);

                    var UnitHeadDetails = await _dapper.QueryFirstOrDefaultAsync<UnitHeadDTO>(ApplicationConstant.Sp_UnitHead, param: param, commandType: CommandType.StoredProcedure);

                    return UnitHeadDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetUnitHeadById(long UnitHeadID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<UnitHeadDTO> GetUnitHeadByName(string UnitHeadName)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HODenum.GETBYEMAIL);
                    param.Add("@UnitHeadNameGet", UnitHeadName);

                    var UnitHeadDetails = await _dapper.QueryFirstOrDefaultAsync<UnitHeadDTO>(ApplicationConstant.Sp_UnitHead, param: param, commandType: CommandType.StoredProcedure);

                    return UnitHeadDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetUnitHeadByName(string UnitHeadName) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<UnitHeadDTO>> GetAllUnitHeadCompanyId(long UnitHeadID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", 8);
                    param.Add("@CompanyIdGet", UnitHeadID);

                    var UnitHeadDetails = await _dapper.QueryAsync<UnitHeadDTO>(ApplicationConstant.Sp_UnitHead, param: param, commandType: CommandType.StoredProcedure);

                    return UnitHeadDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllUnitHeadCompanyId(long UnitHeadID) ===>{ex.Message}");
                throw;
            }
        }

    }
}
