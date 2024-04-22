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
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private string _connectionString;
        private readonly ILogger<LeaveTypeRepository> _logger;
        private readonly IConfiguration _configuration;

        public LeaveTypeRepository(IConfiguration configuration, ILogger<LeaveTypeRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateLeaveType(CreateLeaveTypeDTO create, string Created_By_User_Email)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveTypeEnum.CREATE);
                    param.Add("@LeaveTypeName", create.LeaveTypeName.Trim());
                    param.Add("@MaximumLeaveDurationDays", create.MaximumLeaveDurationDays);
                    param.Add("@Gender", create.Gender.Trim());
                    //param.Add("@IsPaidLeave", create.IsPaidLeave);
                    param.Add("@CompanyID", create.CompanyID);
                    param.Add("@Created_By_User_Email", Created_By_User_Email.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_LeaveType, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateLeaveType(CreateLeaveTypeDTO create, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateLeaveType(UpdateLeaveTypeDTO update, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveTypeEnum.UPDATE);
                    param.Add("@LeaveTypeIdUpd", update.LeaveTypeId);
                    param.Add("@LeaveTypeNameUpd", update.LeaveTypeName);
                    param.Add("@MaximumLeaveDurationDaysUpd", update.MaximumLeaveDurationDays);
                    param.Add("@GenderUpd", update.Gender.Trim());
                    //param.Add("@IsPaidLeaveUpd", update.IsPaidLeave);
                    param.Add("@CompanyIdUpd", update.CompanyID);

                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_LeaveType, param: param, commandType: CommandType.StoredProcedure);

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

        public async Task<dynamic> DeleteLeaveType(DeleteLeaveTypeDTO delete, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveTypeEnum.DELETE);
                    param.Add("@LeaveTypeIdDelete", Convert.ToInt32(delete.LeaveTypeId));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Deleting", delete.Reasons_For_Delete == null ? "" : delete.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_LeaveType, param: param, commandType: CommandType.StoredProcedure);

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
                    param.Add("@Status", LeaveTypeEnum.GETALLACTIVE);

                    var LeaveTypeDetails = await _dapper.QueryAsync<LeaveTypeDTO>(ApplicationConstant.Sp_LeaveType, param: param, commandType: CommandType.StoredProcedure);

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

        public async Task<LeaveTypeDTO> GetLeaveTypeByCompany(string LeaveTypeName, int companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveTypeEnum.GETBYCOMPANY);
                    param.Add("@LeaveTypeNameGet", LeaveTypeName);
                    param.Add("@CompanyIdGet", companyId);

                    var LeaveTypeDetails = await _dapper.QueryFirstOrDefaultAsync<LeaveTypeDTO>(ApplicationConstant.Sp_LeaveType, param: param, commandType: CommandType.StoredProcedure);

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
