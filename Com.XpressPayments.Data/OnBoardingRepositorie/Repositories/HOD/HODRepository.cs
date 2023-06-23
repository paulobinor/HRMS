﻿using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.Repositories.Departments.Repository;
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

namespace Com.XpressPayments.Data.Repositories.HOD
{
    internal class HODRepository : IHODRepository
    {
        private string _connectionString;
        private readonly ILogger<HODRepository> _logger;
        private readonly IConfiguration _configuration;

        public HODRepository(IConfiguration configuration, ILogger<HODRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateHOD(CreateHodDTO hod, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HODenum.CREATE);
                    param.Add("@UserId", hod.UserId);
                    //param.Add("@DeptId", hod.DeptId);
                    param.Add("@CompanyId", hod.CompanyID);

                    param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_HOD, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateHOD(CreateHodDTO hod, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateHOD(UpdateHodDTO hod, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HODenum.UPDATE);
                    param.Add("@HodIDUpd", hod.HodID);
                    param.Add("@UserIdUpd", hod.UserId);
                    //param.Add("@DeptIdUpd", hod.DeptId);
                    param.Add("@CompanyIdUpd", hod.CompanyID);

                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_HOD, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateHOD(UpdateHodDTO hod, string updatedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteHOD(DeleteHodDTO hod, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HODenum.DELETE);
                    param.Add("@HodIDDelete", Convert.ToInt32(hod.HodID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Deleting", hod.Reasons_For_Delete == null ? "" : hod.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_HOD, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteHOD(DeleteHodDTO hod, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<HodDTO>> GetAllActiveHODs()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HODenum.GETALLACTIVE);

                    var HODDetails = await _dapper.QueryAsync<HodDTO>(ApplicationConstant.Sp_HOD, param: param, commandType: CommandType.StoredProcedure);

                    return HODDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActiveHODs() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<HodDTO>> GetAllHOD()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HODenum.GETALL);

                    var HODDetails = await _dapper.QueryAsync<HodDTO>(ApplicationConstant.Sp_HOD, param: param, commandType: CommandType.StoredProcedure);

                    return HODDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:GetAllHOD() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<HodDTO> GetHODById(long HodID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HODenum.GETBYID);
                    param.Add("@HODIdGet", HodID);

                    var HODDetails = await _dapper.QueryFirstOrDefaultAsync<HodDTO>(ApplicationConstant.Sp_HOD, param: param, commandType: CommandType.StoredProcedure);

                    return HODDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetHODtById(int HodID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<HodDTO> GetHODByUserId(long UserId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HODenum.GETBYEMAIL);
                    param.Add("@UserIdGet", UserId);

                    var HODDetails = await _dapper.QueryFirstOrDefaultAsync<HodDTO>(ApplicationConstant.Sp_HOD, param: param, commandType: CommandType.StoredProcedure);

                    return HODDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetHODByName(string DepartmentName) ===>{ex.Message}");
                throw;
            }
        }
        public async Task<HodDTO> GetHODByName(string HODName)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", 10);
                    param.Add("@HODNameGet", HODName);

                    var HODDetails = await _dapper.QueryFirstOrDefaultAsync<HodDTO>(ApplicationConstant.Sp_HOD, param: param, commandType: CommandType.StoredProcedure);

                    return HODDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetHODByName(string DepartmentName) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<HodDTO> GetHODByCompany(long UserId, long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HODenum.GETCOMPANY);
                    param.Add("@@UserIdGet", UserId);
                    param.Add("@CompanyIdGet", companyId);

                    var HODDetails = await _dapper.QueryFirstOrDefaultAsync<HodDTO>(ApplicationConstant.Sp_HOD, param: param, commandType: CommandType.StoredProcedure);

                    return HODDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetHODByName(string DepartmentName) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<HodDTO>> GetAllHODCompanyId(long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HODenum.GETCOMPANYBYID);
                    param.Add("@CompanyIdGet", companyId);

                    var HODDetails = await _dapper.QueryAsync<HodDTO>(ApplicationConstant.Sp_HOD, param: param, commandType: CommandType.StoredProcedure);

                    return HODDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllDepartmentsbyCompanyId(long companyId) ===>{ex.Message}");
                throw;
            }
        }


    }
}
