using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DapperGeneric;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.LeaveModuleRepository.LeaveRequestRepo
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private string _connectionString;
        private readonly ILogger<LeaveRequestRepository> _logger;
        private readonly IDapperGenericRepository _dapperGeneric;
        private readonly IConfiguration _configuration;

        public LeaveRequestRepository(IConfiguration configuration, ILogger<LeaveRequestRepository> logger, IDapperGenericRepository dapperGeneric)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
            _dapperGeneric = dapperGeneric;
        }

        public async Task<string> CreateLeaveRequest(LeaveRequestCreate Leave)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", LeaveRequestEnum.CREATE);
                param.Add("@UserId", Leave.UserId);
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

                param.Add("@Created_By_User_Email", Leave.Created_By_User_Email.Trim());

                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_LeaveRequest, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {               
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex}");
                throw;
            }
        }
        public async Task<string> ApproveLeaveRequest(long LeaveRequestID, long ApprovedByUserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", 10);
                param.Add("@LeaveRequestID", LeaveRequestID);
                param.Add("@ApprovedByUserId", ApprovedByUserId);               
                param.Add("@DateApproved",DateTime.Now);
                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_LeaveRequest, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {               
                _logger.LogError($"MethodName: ApproveLeaveRequest ===>{ex}");
                throw;
            }
        }
        public async Task<string> DisaproveLeaveRequest(long LeaveRequestID, long DisapprovedByUserId, string DisapprovedComment)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", 11);
                param.Add("@LeaveRequestID", LeaveRequestID);
                param.Add("@DisapprovedByUserId", DisapprovedByUserId);
                param.Add("@DisapprovedComment", DisapprovedComment);
                param.Add("@DateDisapproved", DateTime.Now);
                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_LeaveRequest, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: DisaproveLeaveRequest ===>{ex}");
                throw;
            }
        }

        public async Task<dynamic> DeleteLeaveRequest(LeaveRequestDelete delete, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.DELETE);
                    param.Add("@LeaveRequestIDDelete", Convert.ToInt32(delete.LeaveRequestID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Delete", delete.Reasons_For_Delete == null ? "" : delete.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_LeaveRequest, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<dynamic> DeleteLeaveRequest(LeaveRequestDelete delete, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<LeaveRequestDTO>> GetAllLeaveRequest()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.GETALL);

                    var LeaveDetails = await _dapper.QueryAsync<LeaveRequestDTO>(ApplicationConstant.Sp_LeaveRequest, param: param, commandType: CommandType.StoredProcedure);

                    return LeaveDetails;
                }
            }
            catch (Exception ex)
            {              
                _logger.LogError($"MethodName: GetAllLeaveRequest() ===>{ex.Message}");
                throw;
            }
        }


        public async Task<LeaveRequestDTO> GetLeaveRequestById(long LeaveRequestID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.GETBYID);
                    param.Add("@LeaveRequestIDGet", LeaveRequestID);

                    var LeaveDetails = await _dapper.QueryFirstOrDefaultAsync<LeaveRequestDTO>(ApplicationConstant.Sp_LeaveRequest, param: param, commandType: CommandType.StoredProcedure);

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

        public async Task<DepartmentsDTO> GetLeaveRequestByName(string RequestYear)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.GETBYEMAIL);
                    param.Add("@RequestYearGet", RequestYear);

                    var DepartmentDetails = await _dapper.QueryFirstOrDefaultAsync<DepartmentsDTO>(ApplicationConstant.Sp_Departments, param: param, commandType: CommandType.StoredProcedure);

                    return DepartmentDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<DepartmentsDTO> GetLeaveRequestByName(string RequestYear) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<LeaveRequestDTO> GetLeaveRequestByCompany(string RequestYear, int companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.GETBYCOMPANY);
                    param.Add("@RequestYearGet", RequestYear);
                    param.Add("@CompanyIdGet", companyId);

                    var LeaveDetails = await _dapper.QueryFirstOrDefaultAsync<LeaveRequestDTO>(ApplicationConstant.Sp_LeaveRequest, param: param, commandType: CommandType.StoredProcedure);

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
    }
}
