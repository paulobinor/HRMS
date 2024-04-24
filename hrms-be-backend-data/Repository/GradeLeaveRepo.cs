﻿using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{
    public class GradeLeaveRepo : IGradeLeaveRepo
    {
        private string _connectionString;
        private readonly ILogger<GradeLeaveRepo> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDapperGenericRepository _dapper;

        public GradeLeaveRepo(IConfiguration configuration, ILogger<GradeLeaveRepo> logger, IDapperGenericRepository dapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
            _dapper = dapper;
        }

        public async Task<string> CreateGradeLeave(CreateGradeLeaveDTO create)
        {
            try
            {
                    var param = new DynamicParameters();
                    param.Add("@LeaveTypeId", create.LeaveTypeId);
                    param.Add("@GradeID", create.GradeID);
                    param.Add("@NumbersOfDays", create.NumbersOfDays);
                    param.Add("@NumberOfVacationSplit", create.NumberOfVacationSplit);
                    param.Add("@CompanyID", create.CompanyID);
                    param.Add("@CreatedByUserID", create.CreatedByUserID);

                    dynamic response = await _dapper.Get<string>("Sp_create_grade_leave", param, commandType: CommandType.StoredProcedure);

                    //dynamic response = await _dapper.ExecuteAsync("Sp_create_grade_leave", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateGradeLeave(CreateGradeLeaveDTO create, string Created_By_User_Email)===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateGradeLeave(UpdateGradeLeaveDTO update, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GradeLeaveEnum.UPDATE);
                    param.Add("@GradeLeaveIDUpd", update.GradeLeaveID);
                    param.Add("@LeaveTypeIdUpd", update.LeaveTypeId);
                    param.Add("@GradeIDUpd", update.GradeID);
                    param.Add("@NumbersOfDaysUpd", update.NumbersOfDays);
                    param.Add("@NumberOfVacationSplitUpd", update.NumberOfVacationSplit);
                    param.Add("@CompanyIdUpd", update.CompanyID);
                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_GradeLeave, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateGradeLeave(UpdateGradeLeaveDTO update, string updatedbyUserEmail)===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteGradeLeave(DeleteGradeLeaveDTO delete, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GradeLeaveEnum.DELETE);
                    param.Add("@GradeLeaveIDDelete", Convert.ToInt32(delete.GradeLeaveID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Deleting", delete.Reasons_For_Delete == null ? "" : delete.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_GradeLeave, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteGradeLeave(DeleteGradeLeaveDTO delete, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<GradeLeaveDTO>> GetAllActiveGradeLeave()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GradeLeaveEnum.GETALLACTIVE);

                    var LeaveTypeDetails = await _dapper.QueryAsync<GradeLeaveDTO>(ApplicationConstant.Sp_GradeLeave, param: param, commandType: CommandType.StoredProcedure);

                    return LeaveTypeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActiveGradeLeave()===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<GradeLeaveDTO>> GetAllGradeLeave()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    var LeaveTypeDetails = await _dapper.QueryAsync<GradeLeaveDTO>("Sp_get_all_grade_leave", param: param, commandType: CommandType.StoredProcedure);

                    return LeaveTypeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:GetAllActiveGradeLeave() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<GradeLeaveDTO> GetGradeLeaveById(long GradeLeaveID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GradeLeaveEnum.GETBYID);
                    param.Add("@GradeLeaveIDGet", GradeLeaveID);

                    var LeaveTypeDetails = await _dapper.QueryFirstOrDefaultAsync<GradeLeaveDTO>(ApplicationConstant.Sp_GradeLeave, param: param, commandType: CommandType.StoredProcedure);

                    return LeaveTypeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetGradeLeaveById(long GradeLeaveID)===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<GradeLeaveDTO>> GetAllGradeLeaveCompanyId(long CompanyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@CompanyId", CompanyId);

                    var LeaveTypeDetails = await _dapper.QueryAsync<GradeLeaveDTO>("Sp_get_all_grade_leave_by_compnay", param: param, commandType: CommandType.StoredProcedure);

                    return LeaveTypeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllGradeLeaveCompanyId(long CompanyId) ===>{ex.Message}");
                throw;
            }
        }

    }
}
