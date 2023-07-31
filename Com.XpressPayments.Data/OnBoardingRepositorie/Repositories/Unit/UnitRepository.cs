using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.Unit
{
    public  class UnitRepository : IUnitRepository
    {
        private string _connectionString;
        private readonly ILogger<UnitRepository> _logger;
        private readonly IConfiguration _configuration;

        public UnitRepository(IConfiguration configuration, ILogger<UnitRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateUnit(CreateUnitDTO unit, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", UnitEnum.CREATE);
                    param.Add("@UnitName", unit.UnitName.Trim());
                    param.Add("@UnitHeadUserId", unit.UnitHeadUserId);
                    param.Add("@DeptId", unit.DeptId);
                    param.Add("@CompanyId", unit.CompanyId);

                    param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Unit, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateUnit(CreateUnitDTO unit, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateUnit(UpdateUnitDTO unit, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", UnitEnum.UPDATE);
                    param.Add("@UnitIDUpd", unit.UnitID);
                    param.Add("@UnitHeadUserIdUpd", unit.UnitHeadUserId);
                    param.Add("@UnitNameUpd", unit.UnitName.Trim());
                    param.Add("@DeptIdUpd", unit.DeptId);
                    param.Add("@CompanyIdUpd", unit.CompanyId);
                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Unit, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateUnit(UpdateUnitDTO unit, string updatedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteUnit(DeleteUnitDTO unit, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", UnitEnum.DELETE);
                    param.Add("@UnitIDDelete", Convert.ToInt32(unit.UnitID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Deleting", unit.Reasons_For_Delete == null ? "" : unit.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Unit, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteUnit(DeleteUnitDTO unit, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<UnitDTO>> GetAllActiveUnit()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", UnitEnum.GETALLACTIVE);

                    var UnitDetails = await _dapper.QueryAsync<UnitDTO>(ApplicationConstant.Sp_Unit, param: param, commandType: CommandType.StoredProcedure);

                    return UnitDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActiveUnit() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<UnitDTO>> GetAllUnit()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", UnitEnum.GETALL);

                    var UnitDetails = await _dapper.QueryAsync<UnitDTO>(ApplicationConstant.Sp_Unit, param: param, commandType: CommandType.StoredProcedure);

                    return UnitDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:GetAllUniyt() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<UnitDTO> GetUnitById(long UnitID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", UnitEnum.GETBYID);
                    param.Add("@UnitGet", UnitID);

                    var UnitDetails = await _dapper.QueryFirstOrDefaultAsync<UnitDTO>(ApplicationConstant.Sp_Unit, param: param, commandType: CommandType.StoredProcedure);

                    return UnitDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetUnitById(long UnitID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<UnitDTO> GetUnitByName(string UnitName)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", UnitEnum.GETBYEMAIL);
                    param.Add("@UnitNameGet", UnitName);

                    var UnitDetails = await _dapper.QueryFirstOrDefaultAsync<UnitDTO>(ApplicationConstant.Sp_Unit, param: param, commandType: CommandType.StoredProcedure);

                    return UnitDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetUnitByName(string UnitName) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<UnitDTO> GetUnitByCompany(string UnitName, int companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", UnitEnum.GETCOMPANY);
                    param.Add("@UnitNameGet", UnitName);
                    param.Add("@CompanyIdGet", companyId);

                    var UnitDetails = await _dapper.QueryFirstOrDefaultAsync<UnitDTO>(ApplicationConstant.Sp_Unit, param: param, commandType: CommandType.StoredProcedure);

                    return UnitDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetUnitByName(string UnitName) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<UnitDTO>> GetAllUnitCompanyId(long CompanyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", UnitEnum.GETCOMPANYBYID);
                    param.Add("@CompanyIdGet", CompanyId);

                    var UnitDetails = await _dapper.QueryAsync<UnitDTO>(ApplicationConstant.Sp_Unit, param: param, commandType: CommandType.StoredProcedure);

                    return UnitDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllUnitCompanyId(long UnitID) ===>{ex.Message}");
                throw;
            }
        }
    }
}
