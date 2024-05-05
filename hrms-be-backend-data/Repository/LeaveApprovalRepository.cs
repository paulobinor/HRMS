using Dapper;
using hrms_be_backend_common.DTO;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;
using System.Text.Json;

namespace hrms_be_backend_data.Repository
{
    public class LeaveApprovalRepository : ILeaveApprovalRepository
    {
        private string _connectionString;
        private readonly ILogger<LeaveApprovalRepository> _logger;
        private readonly IDapperGenericRepository _dapperGeneric;
        private readonly IConfiguration _configuration;

        public LeaveApprovalRepository(IConfiguration configuration, ILogger<LeaveApprovalRepository> logger, IDapperGenericRepository dapperGeneric)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
            _dapperGeneric = dapperGeneric;
        }

        public async Task<LeaveApprovalLineItem> UpdateLeaveApprovalLineItem(LeaveApprovalLineItem leaveApprovalLineItem)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@LeaveApprovalLineItemId", leaveApprovalLineItem.LeaveApprovalLineItemId);
                param.Add("@ApprovalStatus", leaveApprovalLineItem.ApprovalStatus);
                param.Add("@IsApproved", leaveApprovalLineItem.IsApproved);
                param.Add("@ApprovalEmployeeId", leaveApprovalLineItem.ApprovalEmployeeId);
                param.Add("@Comments", leaveApprovalLineItem.Comments);
                param.Add("@EntryDate", leaveApprovalLineItem.EntryDate);

                var res = await _dapperGeneric.Get<LeaveApprovalLineItem>(ApplicationConstant.Sp_UpdateLeaveApprovalLineItem, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return res;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: ApproveLeaveRequest ===>{ex}");
                throw;
            }
        }
        public async Task<GradeLeave> GetEmployeeGradeLeave(long employeeId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@employeeId", employeeId);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.Get<GradeLeave>(ApplicationConstant.Sp_GetEmployeeGradeLeave, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }
        public async Task<LeaveRequestLineItem> GetLeaveApprovalLineItem(long leaveApprovalLineItemId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@leaveApprovalLineItemId", leaveApprovalLineItemId);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.Get<LeaveRequestLineItem>(ApplicationConstant.Sp_GetLeaveApprovalLineItem, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }
        public async Task<LeaveApprovalInfo> GetLeaveApprovalInfo(long leaveApprovalId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@leaveApprovalId", leaveApprovalId);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.Get<LeaveApprovalInfo>(ApplicationConstant.Sp_GetLeaveApproval, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }
        public async Task<LeaveApprovalInfo> GetExistingLeaveApproval(long EmployeeId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", EmployeeId);
              //  param.Add("@CompanyID", CompanyID);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.Get<LeaveApprovalInfo>(ApplicationConstant.Sp_GetExistingLeaveApproval, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }
        public async Task<LeaveApprovalInfo> GetLeaveApprovalInfoByEmployeeId(long EmployeeId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", EmployeeId);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.Get<LeaveApprovalInfo>(ApplicationConstant.Sp_GetLeaveApprovalByEmployeeId, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }

        public async Task<LeaveApprovalInfo> GetLeaveApprovalInfoByRequestLineItemId(long leaveRequestLineItemId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@leaveRequestLineItemId", leaveRequestLineItemId);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.Get<LeaveApprovalInfo>(ApplicationConstant.Sp_GetLeaveApprovalByRequestItem, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }
        
        public async Task<LeaveApprovalLineItem> GetLeaveApprovalLineItem(long LeaveApprovalId, int approvalStep = 0)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@LeaveApprovalId", LeaveApprovalId);
                param.Add("@ApprovalStep", approvalStep);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.Get<LeaveApprovalLineItem>(ApplicationConstant.Sp_GetLeaveApprovalLineItem, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }

        public async Task<List<PendingLeaveApprovalItemsDto>> GetPendingLeaveApprovals(long approvalEmployeeID, string v)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ApprovalEmployeeID", approvalEmployeeID);
                param.Add("@ApprovalStatus", v);
                //if (!string.IsNullOrEmpty(v))
                //{
                //    param.Add("@ApprovalStatus", "All");
                //}
                //else
                //{
                //    param.Add("@ApprovalStatus", "Pending");
                //}

                var res = await _dapperGeneric.GetAll<PendingLeaveApprovalItemsDto>(ApplicationConstant.Sp_GetPendingLeaveApprovals, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }

        public async Task<List<LeaveApprovalLineItem>> GetLeaveApprovalLineItems(long leaveApprovalId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@leaveApprovalId", leaveApprovalId);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.GetAll<LeaveApprovalLineItem>(ApplicationConstant.Sp_GetLeaveApprovalLineItems, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }

        public async Task UpdateLeaveRequestLineItemApproval(LeaveRequestLineItem leaveRequestLineItem)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IsApproved", leaveRequestLineItem.IsApproved);
                param.Add("@LeaveRequestLineItemId", leaveRequestLineItem.LeaveRequestLineItemId);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.GetAll<LeaveApprovalLineItem>(ApplicationConstant.Sp_UpdateLeaveRequestLineItemApproval, param, commandType: CommandType.StoredProcedure);

                _logger.LogInformation($"Result of Sp_UpdateLeaveRequestLineItemApproval {JsonConvert.SerializeObject(res)}");
                //if (res != null)
                //{
                //    return res;
                //}
                //return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
               // return default;
            }
        }
        public async Task<LeaveApprovalInfo> UpdateLeaveApprovalInfo(LeaveApprovalInfo leaveApproval)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ApprovalStatus", leaveApproval.ApprovalStatus);
                param.Add("@Comments", leaveApproval.Comments);
                param.Add("@CurrentApprovalCount", leaveApproval.CurrentApprovalCount);
                param.Add("@LeaveApprovalId", leaveApproval.LeaveApprovalId);
                param.Add("@IsApproved", leaveApproval.IsApproved);
                param.Add("@LastApprovalEmployeeID", leaveApproval.CurrentApprovalID);

                var res = await _dapperGeneric.Get<LeaveApprovalInfo>(ApplicationConstant.Sp_UpdateLeaveApproval, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }

        public async Task<List<LeaveApprovalInfoDto>> GetLeaveApprovalInfoByCompanyID(long CompanyID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyID", CompanyID);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.GetAll<LeaveApprovalInfoDto>(ApplicationConstant.Sp_GetLeaveApprovalIfoByCompanyID, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    foreach (var item in res)
                    {
                        item.LeaveApprovalLineItems = await GetLeaveApprovalLineItems(item.LeaveApprovalId);
                    }
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }
    }
}
