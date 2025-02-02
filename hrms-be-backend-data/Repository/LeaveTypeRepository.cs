﻿using Dapper;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private string _connectionString;
        private readonly ILogger<LeaveTypeRepository> _logger;
        private readonly IDapperGenericRepository _dapperGeneric;
        private readonly IConfiguration _configuration;

        public LeaveTypeRepository(IConfiguration configuration, ILogger<LeaveTypeRepository> logger, IDapperGenericRepository dapperGeneric)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
            _dapperGeneric = dapperGeneric;
        }

        public async Task<CreateLeaveTypeDTO> CreateLeaveType(CreateLeaveTypeDTO create)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@LeaveTypeName", create.LeaveTypeName.Trim());
                param.Add("@CompanyID", create.CompanyID);
                param.Add("@GenderID", create.GenderID);
                param.Add("@CreatedByUserId", create.UserId);

                var res = await _dapperGeneric.Get<CreateLeaveTypeDTO>(ApplicationConstant.Sp_CreateLeaveType, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return res;
                }
                return default;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateLeaveType(CreateLeaveTypeDTO create, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<LeaveTypeDTO> UpdateLeaveType(UpdateLeaveTypeDTO update)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                   // param.Add("@Status", LeaveTypeEnum.UPDATE);
                    param.Add("@LeaveTypeId", update.LeaveTypeId);
                    param.Add("@LeaveTypeName", update.LeaveTypeName);
                  //  param.Add("@MaximumLeaveDurationDaysUpd", update.MaximumLeaveDurationDays);
                    param.Add("@GenderID", update.GenderID);
                    //param.Add("@IsPaidLeaveUpd", update.IsPaidLeave);
                    param.Add("@CompanyId", update.CompanyID);
                    param.Add("@LastUpdatedUserId", update.LastUpdatedUserId);

                    var response = await _dapper.QueryFirstOrDefaultAsync<LeaveTypeDTO>(ApplicationConstant.Sp_UpdateLeaveTypeById, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateLeaveType(UpdateLeaveTypeDTO update, string updatedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<LeaveTypeDTO> DeleteLeaveType(DeleteLeaveTypeDTO delete)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                  //  param.Add("@Status", LeaveTypeEnum.DELETE);
                    param.Add("@LeaveTypeId", delete.LeaveTypeId);
                    param.Add("@DeletedByUserId", delete.DeletedByUserId);
                    param.Add("@Reasons_For_Delete", delete.Reasons_For_Delete);

                    var response = await _dapper.QueryFirstOrDefaultAsync<LeaveTypeDTO>(ApplicationConstant.Sp_DeleteLeaveTypeById, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteLeave(DeleteLeaveTypeDTO delete, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<LeaveTypeDTO>> GetAllActiveLeaveType()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                   // param.Add("@Status", LeaveTypeEnum.GETALLACTIVE);

                    var LeaveTypeDetails = await _dapper.QueryAsync<LeaveTypeDTO>(ApplicationConstant.Sp_GetActiveLeaveTypes, param: param, commandType: CommandType.StoredProcedure);

                    return LeaveTypeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActiveLeaveType()===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<LeaveTypeDTO>> GetAllLeaveType()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveTypeEnum.GETALL);

                    var LeaveTypeDetails = await _dapper.QueryAsync<LeaveTypeDTO>(ApplicationConstant.Sp_LeaveType, param: param, commandType: CommandType.StoredProcedure);

                    return LeaveTypeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:GetAllLeaveType() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<LeaveTypeDTO> GetLeaveTypeById(long LeaveTypeId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                  //  param.Add("@Status", LeaveTypeEnum.GETBYID);
                    param.Add("@LeaveTypeId", LeaveTypeId);

                    var LeaveTypeDetails = await _dapper.QueryFirstOrDefaultAsync<LeaveTypeDTO>(ApplicationConstant.Sp_GetLeaveTypeById, param: param, commandType: CommandType.StoredProcedure);

                    return LeaveTypeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetLeaveTypeById(long LeaveTypeId)===>{ex.Message}");
                throw;
            }
        }

        public async Task<LeaveTypeDTO> GetLeaveTypeByName(string LeaveTypeName)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveTypeEnum.GETBYEMAIL);
                    param.Add("@LeaveTypeNameGet", LeaveTypeName);

                    var LeaveTypeDetails = await _dapper.QueryFirstOrDefaultAsync<LeaveTypeDTO>(ApplicationConstant.Sp_LeaveType, param: param, commandType: CommandType.StoredProcedure);

                    return LeaveTypeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetLeaveTypeByName(string LeaveTypeName) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<LeaveTypeDTO> GetLeaveTypeByCompany(string LeaveTypeName, long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                   // param.Add("@Status", LeaveTypeEnum.GETBYCOMPANY);
                    param.Add("@LeaveTypeName", LeaveTypeName);
                    param.Add("@CompanyID", companyId);

                    var LeaveTypeDetails = await _dapper.QueryFirstOrDefaultAsync<LeaveTypeDTO>(ApplicationConstant.Sp_GetLeaveTypeByName, param: param, commandType: CommandType.StoredProcedure);

                    return LeaveTypeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetLeaveTypeByCompany(string LeaveTypeName, int companyId) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<LeaveTypeDTO>> GetAllLeaveTypeCompanyId(long CompanyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@CompanyId", CompanyId);

                    var LeaveTypeDetails = await _dapper.QueryAsync<LeaveTypeDTO>(ApplicationConstant.Sp_GetLeaveType, param: param, commandType: CommandType.StoredProcedure);

                    return LeaveTypeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllGradeCompanyId(long GradeID) ===>{ex.Message}");
                throw;
            }
        }

    }
}
