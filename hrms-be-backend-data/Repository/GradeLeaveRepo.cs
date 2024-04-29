using Dapper;
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
                param.Add("@GenderID", create.GenderID);
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

        public async Task<GradeLeaveDTO> UpdateGradeLeave(UpdateGradeLeaveDTO update)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                   // param.Add("@Status", GradeLeaveEnum.UPDATE);
                    param.Add("@GradeLeaveID", update.GradeLeaveID);
                    param.Add("@LeaveTypeId", update.LeaveTypeId);
                    param.Add("@CompanyID", update.CompanyID);
                    param.Add("@GradeID", update.GradeID);
                    param.Add("@GenderID", update.GenderID);
                    param.Add("@NumbersOfDays", update.NumbersOfDays);
                    param.Add("@NumberOfVacationSplit", update.NumberOfVacationSplit);
                    param.Add("@MaximumNumberOfLeaveDays", update.MaximumNumberOfLeaveDays);
                    param.Add("@LastUpdatedByUserId", update.UserId);

                    var response = await _dapper.QueryFirstOrDefaultAsync<GradeLeaveDTO>(ApplicationConstant.Sp_UpdateGradeLeave, param: param, commandType: CommandType.StoredProcedure);

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

        public async Task<GradeLeaveDTO> DeleteGradeLeave(DeleteGradeLeaveDTO delete)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                   // param.Add("@Status", GradeLeaveEnum.DELETE);
                    param.Add("@GradeLeaveID", delete.GradeLeaveID);
                    param.Add("@DeletedByUserId", delete.UserID);
                    param.Add("@DeletedComment", delete.Reasons_For_Delete);

                    var response = await _dapper.QueryFirstOrDefaultAsync<GradeLeaveDTO>(ApplicationConstant.Sp_DeleteGradeLeave, param: param, commandType: CommandType.StoredProcedure);

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
                    //param.Add("@Status", GradeLeaveEnum.GETALLACTIVE);

                    var LeaveTypeDetails = await _dapper.QueryAsync<GradeLeaveDTO>(ApplicationConstant.Sp_GetAllActiveGradeLeave, param: param, commandType: CommandType.StoredProcedure);

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
                    param.Add("@GradeLeaveID", GradeLeaveID);

                    var LeaveTypeDetails = await _dapper.QueryFirstOrDefaultAsync<GradeLeaveDTO>(ApplicationConstant.Sp_GetGradeLeaveById, param: param, commandType: CommandType.StoredProcedure);

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

        public async Task<IEnumerable<GradeLeaveDTO>> GetEmployeeGradeLeaveTypes(long companyID, long employeeID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@CompanyID", companyID);
                    param.Add("@EmployeeID", employeeID);
                    var res = await _dapper.QueryAsync<GradeLeaveDTO>(ApplicationConstant.Sp_GetEmployeeGradeLeaveTypes, param, commandType: CommandType.StoredProcedure);
                   // var LeaveTypeDetails = await _dapper.QueryAsync<GradeLeaveDTO>("Sp_get_all_grade_leave_by_compnay", param: param, commandType: CommandType.StoredProcedure);

                    return res;
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
