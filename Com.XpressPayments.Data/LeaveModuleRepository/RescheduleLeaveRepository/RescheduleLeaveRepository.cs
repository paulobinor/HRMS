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

namespace Com.XpressPayments.Data.LeaveModuleRepository.LeaveRequestRepo
{
    public  class RescheduleLeaveRepository : IRescheduleLeaveRepository
    {
        private string _connectionString;
        private readonly ILogger<RescheduleLeaveRepository> _logger;
        private readonly IConfiguration _configuration;

        public RescheduleLeaveRepository(IConfiguration configuration, ILogger<RescheduleLeaveRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateRescheduleLeave(RescheduleLeaveRequestCreateDTO RescheduleLeave, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.CREATE);
                    param.Add("@LeaveRequestID", RescheduleLeave.LeaveRequestID);
                    param.Add("@StaffID", RescheduleLeave.StaffID.Trim());
                    param.Add("@RequestYear", RescheduleLeave.RequestYear.Trim());
                    param.Add("@LeaveTypeId", RescheduleLeave.LeaveTypeId);
                    param.Add("@NoOfDays", RescheduleLeave.NoOfDays);
                    param.Add("@StartDate", RescheduleLeave.StartDate);
                    param.Add("@EndDate", RescheduleLeave.EndDate);
                    param.Add("@ReliverStaffID", RescheduleLeave.ReliverStaffID.Trim());
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
    }
}
