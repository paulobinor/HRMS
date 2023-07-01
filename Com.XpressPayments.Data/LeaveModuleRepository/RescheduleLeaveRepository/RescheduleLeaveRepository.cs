using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
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
using Com.XpressPayments.Data.DapperGeneric;

namespace Com.XpressPayments.Data.LeaveModuleRepository.LeaveRequestRepo
{
    public  class RescheduleLeaveRepository : IRescheduleLeaveRepository
    {
        private string _connectionString;
        private readonly ILogger<RescheduleLeaveRepository> _logger;
        private readonly IDapperGenericRepository _dapperGeneric;
        private readonly IConfiguration _configuration;

        public RescheduleLeaveRepository(IConfiguration configuration, ILogger<RescheduleLeaveRepository> logger, IDapperGenericRepository dapperGeneric)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _dapperGeneric = dapperGeneric;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateRescheduleLeave(RescheduleLeaveRequestCreateDTO RescheduleLeave, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", RescheduleLeaveRequestEnum.CREATE);
                    param.Add("@LeaveRequestID", RescheduleLeave.LeaveRequestID);
                    param.Add("@UserId", RescheduleLeave.UserId);
                    param.Add("@RequestYear", RescheduleLeave.RequestYear.Trim());
                    param.Add("@LeaveTypeId", RescheduleLeave.LeaveTypeId);
                    param.Add("@NoOfDays", RescheduleLeave.NoOfDays);
                    param.Add("@StartDate", RescheduleLeave.StartDate);
                    param.Add("@EndDate", RescheduleLeave.EndDate);
                    param.Add("@ReliverUserID", RescheduleLeave.ReliverUserID);
                    param.Add("@LeaveEvidence", RescheduleLeave.LeaveEvidence.Trim());
                    param.Add("@Notes", RescheduleLeave.Notes.Trim());
                    param.Add("@ReasonForRescheduling", RescheduleLeave.ReasonForRescheduling.Trim());
                    param.Add("@CompanyID", RescheduleLeave.CompanyID);

                    param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_RescheduleLeave, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateRescheduleLeave(RescheduleLeaveRequestCreateDTO RescheduleLeave, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<string> ApproveRescheduleLeave(long RescheduleLeaveID, long ApprovedByUserId)
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
                _logger.LogError($"MethodName: ApproveRescheduleLeave ===>{ex}");
                throw;
            }
        }

        public async Task<string> DisaproveRescheduleLeave(long RescheduleLeaveID, long DisapprovedByUserId, string DisapprovedComment)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", RescheduleLeaveRequestEnum.disapproval);
                param.Add("@RescheduleLeaveID", RescheduleLeaveID);
                param.Add("@DisapprovedByUserId", DisapprovedByUserId);
                param.Add("@DisapprovedComment", DisapprovedComment);
                param.Add("@DateDisapproved", DateTime.Now);
                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_RescheduleLeave, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: DisaproveLeaveRequest ===>{ex}");
                throw;
            }
        }

        public async Task<IEnumerable<RescheduleLeaveRequestDTO>> GetAllRescheduleLeave()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", RescheduleLeaveRequestEnum.GETALL);

                    var LeaveDetails = await _dapper.QueryAsync<RescheduleLeaveRequestDTO>(ApplicationConstant.Sp_RescheduleLeave, param: param, commandType: CommandType.StoredProcedure);

                    return LeaveDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllRescheduleLeave() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<RescheduleLeaveRequestDTO> GetRescheduleLeaveById(long RescheduleLeaveID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", RescheduleLeaveRequestEnum.GETBYID);
                    param.Add("@RescheduleLeaveIDGet", RescheduleLeaveID);

                    var LeaveDetails = await _dapper.QueryFirstOrDefaultAsync<RescheduleLeaveRequestDTO>(ApplicationConstant.Sp_RescheduleLeave, param: param, commandType: CommandType.StoredProcedure);

                    return LeaveDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<LeaveRequestDTO> GetRescheduleLeaveById(long RescheduleLeaveID) ===>{ex.Message}");
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
                    param.Add("@Status", RescheduleLeaveRequestEnum.GETBYYear);
                    param.Add("@RequestYearGet", RequestYear);

                    var LeaveDetails = await _dapper.QueryFirstOrDefaultAsync<RescheduleLeaveRequestDTO>(ApplicationConstant.Sp_RescheduleLeave, param: param, commandType: CommandType.StoredProcedure);

                    return LeaveDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<RescheduleLeaveRequestDTO> GetRescheduleLeaveRequestByYear(string RequestYear) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<RescheduleLeaveRequestDTO> GetRescheduleLeaveRequestByCompany(string RequestYear, long companyId)
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
                _logger.LogError($"MethodName: GetRescheduleLeaveRequestByCompany(string RequestYear, long companyId) ===>{ex.Message}");
                throw;
            }
        }

    }
}
