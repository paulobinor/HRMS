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
    public class RescheduleLeaveRepository : IRescheduleLeaveRepository
    {
        private string _connectionString;
        private readonly ILogger<RescheduleLeaveRepository> _logger;
        private readonly IDapperGenericRepository _dapperGeneric;
        private readonly IConfiguration _configuration;

        public RescheduleLeaveRepository(IConfiguration configuration, ILogger<RescheduleLeaveRepository> logger, IDapperGenericRepository dapperGeneric)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
            _dapperGeneric = dapperGeneric;
        }

        public async Task<string> CreateRescheduleLeaveRequest(RescheduleLeaveRequestCreate Leave)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", RescheduleLeaveRequestEnum.CREATE);
                param.Add("@LeaveRequestID", Leave.LeaveRequestID);
                param.Add("@UserID", Leave.UserId);
                param.Add("@RequestYear", Leave.RequestYear);
                param.Add("@LeaveTypeId", Leave.LeaveTypeId);
                param.Add("@NoOfDays", Leave.NoOfDays);
                param.Add("@StartDate", Leave.StartDate);
                param.Add("@EndDate", Leave.EndDate);
                param.Add("@ReliverUserID", Leave.ReliverUserID);
                param.Add("@LeaveEvidence", Leave.LeaveEvidence.Trim());
                param.Add("@Notes", Leave.Notes.Trim());
                param.Add("@ReasonForRescheduling", Leave.ReasonForRescheduling.Trim());
                param.Add("@CompanyID", Leave.CompanyID);


                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_RescheduleLeave, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateRescheduleLeaveRequest ===>{ex}");
                throw;
            }
        }
        public async Task<string> ApproveRescheduleLeaveRequest(long RescheduleLeaveID, long ApprovedByUserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", RescheduleLeaveRequestEnum.approval);
                param.Add("@RescheduleLeaveID", RescheduleLeaveID);
                param.Add("@ApprovedByUserId", ApprovedByUserId);
                param.Add("@DateApproved", DateTime.Now);
                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_RescheduleLeave, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: ApproveLeaveRequest ===>{ex}");
                throw;
            }
        }
        public async Task<string> DisaproveRescheduleLeaveRequest(long RescheduleLeaveID, long DisapprovedByUserId, string DisapprovedComment)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", 11);
                param.Add("@RescheduleLeaveID", RescheduleLeaveID);
                param.Add("@DisapprovedByUserId", DisapprovedByUserId);
                param.Add("@DisapprovedComment", DisapprovedComment);
                param.Add("@DateDisapproved", DateTime.Now);
                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_RescheduleLeave, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: DisaproveRescheduleLeaveRequest ===>{ex}");
                throw;
            }
        }

        //public async Task<dynamic> DeleteRescheduleLeaveRequest(LeaveRequestDelete delete, string deletedbyUserEmail)
        //{
        //    try
        //    {
        //        using (SqlConnection _dapper = new SqlConnection(_connectionString))
        //        {
        //            var param = new DynamicParameters();
        //            param.Add("@Status", LeaveRequestEnum.DELETE);
        //            param.Add("@LeaveRequestIDDelete", Convert.ToInt32(delete.LeaveRequestID));
        //            param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
        //            param.Add("@Reasons_For_Delete", delete.Reasons_For_Delete == null ? "" : delete.Reasons_For_Delete.ToString().Trim());

        //            dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_RescheduleLeave, param: param, commandType: CommandType.StoredProcedure);

        //            return response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = ex.Message;
        //        _logger.LogError($"MethodName: Task<dynamic> DeleteLeaveRequest(LeaveRequestDelete delete, string deletedbyUserEmail) ===>{ex.Message}");
        //        throw;
        //    }
        //}

        public async Task<IEnumerable<RescheduleLeaveRequestDTO>> GetAllRescheduleLeaveRequest()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.GETALL);

                    var LeaveDetails = await _dapper.QueryAsync<RescheduleLeaveRequestDTO>(ApplicationConstant.Sp_RescheduleLeave, param: param, commandType: CommandType.StoredProcedure);

                    return LeaveDetails;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllLeaveRequest() ===>{ex.Message}");
                throw;
            }
        }


        public async Task<RescheduleLeaveRequestDTO> GetRescheduleLeaveRequestById(long RescheduleLeaveID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.GETBYID);
                    param.Add("@RescheduleLeaveIDGet", RescheduleLeaveID);


                    var LeaveDetails = await _dapper.QueryFirstOrDefaultAsync<RescheduleLeaveRequestDTO>(ApplicationConstant.Sp_RescheduleLeave, param: param, commandType: CommandType.StoredProcedure);

                    return LeaveDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<LeaveRequestDTO> GetLeaveRequestById(long LeaveRequestID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<RescheduleLeaveRequestDTO>> GetRescheduleLeaveRequestByUserId(long UserId, long CompanyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.GETleaveRequestByUerId);
                    param.Add("@UserIdGet", UserId);
                    param.Add("@CompanyIdGet", CompanyId);

                    var LeaveDetails = await _dapper.QueryAsync<RescheduleLeaveRequestDTO>(ApplicationConstant.Sp_RescheduleLeave, param: param, commandType: CommandType.StoredProcedure);

                    return LeaveDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<LeaveRequestDTO> GetLeaveRequestById(long LeaveRequestID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<RescheduleLeaveRequestDTO> GetRescheduleLeaveRequestByYear(string RequestYear)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.GETBYEMAIL);
                    param.Add("@RequestYearGet", RequestYear);

                    var LeaveDetails = await _dapper.QueryFirstOrDefaultAsync<RescheduleLeaveRequestDTO>(ApplicationConstant.Sp_RescheduleLeave, param: param, commandType: CommandType.StoredProcedure);

                    return LeaveDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<DepartmentsDTO> GetLeaveRequestByName(string RequestYear) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<RescheduleLeaveRequestDTO> GetRescheduleLeaveRequestByCompanyId(string RequestYear, long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", RescheduleLeaveRequestEnum.GETBYCOMPANYID);
                    param.Add("@RequestYearGet", RequestYear);
                    param.Add("@CompanyIdGet", companyId);

                    var LeaveDetails = await _dapper.QueryFirstOrDefaultAsync<RescheduleLeaveRequestDTO>(ApplicationConstant.Sp_RescheduleLeave, param: param, commandType: CommandType.StoredProcedure);

                    return LeaveDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetLeaveRequestByCompany(string RequestYear, int companyId) ===>{ex.Message}");
                throw;
            }
        }
        public async Task<IEnumerable<RescheduleLeaveRequestDTO>> GetRescheduleLeaveRequestPendingApproval(long UserIdGet)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", RescheduleLeaveRequestEnum.approval);
                    param.Add("@UserIdGet", UserIdGet);
                    var userDetails = await _dapper.QueryAsync<RescheduleLeaveRequestDTO>(ApplicationConstant.Sp_RescheduleLeave, param: param, commandType: CommandType.StoredProcedure);

                    return userDetails;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetLeaveRequestPendingApproval() ===>{ex.Message}");
                throw;
            }
        }

    }
}
