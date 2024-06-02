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

        public async Task<LeaveApprovalInfo> GetAnnualLeaveApprovalInfo(long leaveApprovalId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@leaveApprovalId", leaveApprovalId);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.Get<LeaveApprovalInfo>(ApplicationConstant.Sp_GetAnnualLeaveApproval, param, commandType: CommandType.StoredProcedure);
                
                if (res != null)
                {
                    res.LeaveApprovalLineItems = await GetLeaveApprovalLineItems(res.LeaveApprovalId);
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAnnualLeaveApprovalInfo ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
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
                    res = res.OrderBy(x=>x.ApprovalStep).ToList();
                    bool ispresent = false;
                    List<PendingLeaveApprovalItemsDto> res1 = new List<PendingLeaveApprovalItemsDto>();
                    foreach (var item in res)
                    {
                        if (item.EmployeeID == approvalEmployeeID && !item.ApprovalStatus.Contains("Approved", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!ispresent)
                            {
                                res1.Add(item);
                                ispresent = true;
                            }
                        }
                        else
                        {
                            res1.Add(item);
                        }
                    }
                    return res1;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }

        public async Task<List<LeaveApproval>> GetLeaveApprovals(long approvalEmployeeId, long employeeID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ApprovalEmployeeID", approvalEmployeeId);
                param.Add("@EmployeeID", employeeID);
                //if (!string.IsNullOrEmpty(v))
                //{
                //    param.Add("@ApprovalStatus", "All");
                //}
                //else
                //{
                //    param.Add("@ApprovalStatus", "Pending");
                //}

                var res = await _dapperGeneric.GetAll<LeaveApproval>(ApplicationConstant.Sp_GetLeaveApprovals, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetPendingAnnualLeaveApprovals ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }
        public async Task<List<PendingAnnualLeaveApprovalItemDto>> GetPendingAnnualLeaveApprovals(long approvalEmployeeID, string v)
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

                var res = await _dapperGeneric.GetAll<PendingLeaveApprovalItemsDto>(ApplicationConstant.Sp_GetPendingAnnualLeaveApprovals, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    res = res.OrderBy(x => x.ApprovalStep).ToList();
                    bool ispresent = false;
                    bool ispresent1 = false;
                    List<PendingLeaveApprovalItemsDto> res1 = new List<PendingLeaveApprovalItemsDto>();
                    List<PendingAnnualLeaveApprovalItemDto> pendingRes = new List<PendingAnnualLeaveApprovalItemDto>();
                    foreach (var item in res)
                    {
                        var count = res1.FirstOrDefault(x=>x.EmployeeID == item.EmployeeID);
                        if (count == null)
                        {
                            res1.Add(item);
                        }
                         
                        //if (item.EmployeeID == approvalEmployeeID && !item.ApprovalStatus.Contains("Approved", StringComparison.OrdinalIgnoreCase))
                        //{
                        //    if (!ispresent)
                        //    {
                        //        res1.Add(item);
                        //        ispresent = true;
                        //    }
                        //}
                        //else if(item.EmployeeID != approvalEmployeeID && !item.ApprovalStatus.Contains("Approved", StringComparison.OrdinalIgnoreCase))
                        //{
                        //    if (!ispresent1)
                        //    {
                        //        res1.Add(item);
                        //        ispresent1 = true;
                        //    }
                        //}

                    }

                    PendingAnnualLeaveApprovalItemDto pendingAnnualLeaveApprovalItemDto = null;
                    foreach (var item in res1)
                    {
                        pendingAnnualLeaveApprovalItemDto = new()
                        {
                            FullName = item.FullName,
                            EmployeeID = item.EmployeeID,
                            LeaveCount = res.FindAll(x => x.EmployeeID == item.EmployeeID).Count(),
                            LeaveTypeName = item.LeaveTypeName,
                            leaveRequestLineItems = await GetLeaveApprovalLineItems(item.LeaveApprovalId),
                            Status = item.Comments,
                            TotalNoOfDays = res.FindAll(x => x.EmployeeID == item.EmployeeID).Sum(x => x.LeaveLength)
                        };
                        pendingRes.Add(pendingAnnualLeaveApprovalItemDto);
                    }
                    return pendingRes;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetPendingAnnualLeaveApprovals ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
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
