using Dapper;
using GTB.Common;
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
               // param.Add("@EntryDate", leaveApprovalLineItem.EntryDate);

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
        public async Task<bool> GetLeaveApprovalInfoByApprovalKey(long approvalKey)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ApprovalKey", approvalKey);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.Get<LeaveApprovalInfo>(ApplicationConstant.Sp_GetLeaveApprovalByApprovalKey, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    if (res.ApprovalKey == approvalKey)
                    {
                        return true;
                    }
                   // res.LeaveApprovalLineItems = await GetLeaveApprovalLineItems(res.LeaveApprovalId);
                   // return res;
                }
                return false;
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
                    res.LeaveApprovalLineItems = await GetLeaveApprovalLineItems(leaveApprovalId);
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
              //  param.Add("@LeavePeriod", LeavePeriod);

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
            int lastLeaveApprovalId = 0;
            try
            {
                var param = new DynamicParameters();
                param.Add("@ApprovalEmployeeID", approvalEmployeeID);
                param.Add("@ApprovalStatus", v);
              
                List<PendingLeaveApprovalItemsDto> leaveApprovalItems = new List<PendingLeaveApprovalItemsDto>();
                List<PendingAnnualLeaveApprovalItemDto> pendingRes = new List<PendingAnnualLeaveApprovalItemDto>();
                PendingLeaveApprovalItemsDto pendingLeaveApproval = null;
            
                var leaveApprovalLineItems = await GetAllApprovalLineItems(approvalEmployeeID);
                bool isValidItem = false;
                foreach (var item in leaveApprovalLineItems)
                {
                    var leaveapproval = await GetLeaveApprovalInfo(item.LeaveApprovalId);
                    var leaverequestLineitem =  GetLeaveRequestLineItem(leaveapproval.LeaveRequestLineItemId).Result;
                    if (leaverequestLineitem == null || leaverequestLineitem.AnnualLeaveId > 0)
                    {
                        //skip because we are not looking for annual leave requests
                    }
                    else
                    {
                        var param1 = new DynamicParameters();
                        param1.Add("@LeaveApprovalId", item.LeaveApprovalId);

                        var leaveApprovalRequestItem = await _dapperGeneric.Get<PendingLeaveApprovalItemsDto>(ApplicationConstant.Sp_GetLeaveApprovalItem, param1, commandType: CommandType.StoredProcedure);

                        if (leaveApprovalRequestItem != null)
                        {
                            leaveApprovalRequestItem.LeaveApprovalLineItemId = item.LeaveApprovalLineItemId;
                            leaveApprovalRequestItem.ApprovalEmployeeId = item.ApprovalEmployeeId;
                            leaveApprovalRequestItem.IsApproved = item.IsApproved;
                            leaveApprovalRequestItem.ApprovalStep = item.ApprovalStep;
                            // leaveApprovalRequestItem.ApprovalStatus = item.ApprovalStatus;
                            leaveApprovalRequestItem.LeaveApprovalId = item.LeaveApprovalId;
                            leaveApprovalRequestItem.ApprovalPosition = item.ApprovalPosition;
                            leaveApprovalRequestItem.DateCreated = item.EntryDate;
                            if (item.ApprovalStatus.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                            {
                                if (leaveapproval.ApprovalKey == item.LeaveApprovalLineItemId)
                                {
                                    isValidItem = true;
                                }
                            }
                            else
                            {
                                isValidItem = true;
                            }

                        
                            if (isValidItem)
                            {
                                leaveApprovalItems.Add(leaveApprovalRequestItem);
                                isValidItem = false;
                            }
                        }

                    }
                }
                if (leaveApprovalItems.Any())
                {
                    leaveApprovalItems = leaveApprovalItems.OrderByDescending(x => x.DateCreated).ToList();
                }
                return leaveApprovalItems;
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }

        private async Task<LeaveRequestLineItem> GetLeaveRequestLineItem(long LeaveRequestLineItemId)
        {
            var param = new DynamicParameters();
            param.Add("@LeaveRequestLineItemId", LeaveRequestLineItemId);

            var res = await _dapperGeneric.Get<LeaveRequestLineItem>(ApplicationConstant.Sp_GetLeaveRequestLineItem, param, commandType: CommandType.StoredProcedure);

            return res;
        }

        public async Task<List<PendingAnnualLeaveApprovalItemDto>> GetAnnualLeaveApprovals(long approvalEmployeeID)
        {
            List<PendingAnnualLeaveApprovalItemDto> pendingRes = new List<PendingAnnualLeaveApprovalItemDto>();

            PendingAnnualLeaveApprovalItemDto pendingAnnualLeaveApprovalItemDto = null;
            try
            {
                var leaveApprovalLineItems = await GetAllApprovalLineItems(approvalEmployeeID);
                if (leaveApprovalLineItems == null || leaveApprovalLineItems.Count <= 0)
                {
                    _logger.LogError($"MethodName: GetPendingAnnualLeaveApprovals ===> Could not get leaveApprovalLineItems info");
                    return default;
                }
               

                foreach (var item in leaveApprovalLineItems)
                {
                    var leaveapproval = await GetLeaveApprovalInfo(item.LeaveApprovalId);

                    if (leaveapproval == null)
                    {
                        _logger.LogError($"MethodName: GetPendingAnnualLeaveApprovals ===> Could not get leaveapproval info");
                        return default;
                    }
                    string comments = leaveapproval.Comments;
                    var annualLeaveInfo = await GetAnnualLeaveInfo(leaveapproval.LeaveApprovalId);
                    if (annualLeaveInfo == null)
                    {
                        //we are looking for only annual leave
                      //  _logger.LogError($"MethodName: GetPendingAnnualLeaveApprovals ===> Could not get annual leave info");
                      //  return default;
                    }
                    else
                    {
                        foreach (var AnnualLieaveRequestLineItem in annualLeaveInfo.leaveRequestLineItems)
                        {
                            AnnualLieaveRequestLineItem.LeaveApprovalLineItemId = item.LeaveApprovalLineItemId;
                            AnnualLieaveRequestLineItem.LeaveApprovalId = item.LeaveApprovalId;
                            AnnualLieaveRequestLineItem.ApprovalStatus = item.ApprovalStatus;
                            AnnualLieaveRequestLineItem.CompanyId = annualLeaveInfo.CompanyID;
                            AnnualLieaveRequestLineItem.Comments = comments; // + "," + item.Comments;
                        }
                        var reqLineItem = annualLeaveInfo.leaveRequestLineItems.FirstOrDefault();
                        pendingAnnualLeaveApprovalItemDto = new()
                        {
                            FullName = reqLineItem.FullName,
                            RelieverName = reqLineItem.RelieverName,
                            ApprovalStatus = item.ApprovalStatus,
                            IsApproved = leaveapproval.IsApproved,
                            Year = reqLineItem.startDate.Year.ToString(),
                            RequestDate = annualLeaveInfo.DateCreated, //.leaveRequestLineItems.FirstOrDefault().
                            EmployeeID = item.EmployeeID,
                            LeaveCount = annualLeaveInfo.leaveRequestLineItems.Count, // res.FindAll(x => x.EmployeeID == item.EmployeeID).Count(),
                            LeaveTypeName = reqLineItem.LeaveTypeName,
                            leaveRequestLineItems = annualLeaveInfo.leaveRequestLineItems, // res.FindAll(x => x.EmployeeID == item.EmployeeID),
                            Status = comments, // item.Comments,
                            leaveApprovalId = leaveapproval.LeaveApprovalId,
                            LeaveApprovalLineItemId = leaveapproval.LeaveApprovalLineItems.FirstOrDefault(x => x.ApprovalEmployeeId == approvalEmployeeID).LeaveApprovalLineItemId,
                            ApprovalPosition = item.ApprovalPosition,
                            LastApprovalEmployeeId = leaveapproval.LastApprovalEmployeeID,
                            TotalNoOfDays = annualLeaveInfo.leaveRequestLineItems.Sum(x => x.LeaveLength),
                            DateCreated = leaveapproval.EntryDate
                        };

                        if (item.ApprovalStatus.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                        {
                            if (leaveapproval.ApprovalKey == item.LeaveApprovalLineItemId)
                            {
                                pendingRes.Add(pendingAnnualLeaveApprovalItemDto);
                            }
                        }
                        else
                        {
                            pendingRes.Add(pendingAnnualLeaveApprovalItemDto);
                        }
                    }
                }
              //  pendingRes = pendingRes.OrderByDescending(x => x.RequestDate).ToList();
                return pendingRes;
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetPendingAnnualLeaveApprovals ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }

        public async Task<LeaveApproval> CreateLeaveApproval(LeaveApproval leaveApproval)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@LeaveRequestLineItemId", leaveApproval.LeaveRequestLineItemId);
                param.Add("@RequiredApprovalCount", leaveApproval.RequiredApprovalCount);
                param.Add("@EmployeeID", leaveApproval.EmployeeID);
                param.Add("@LastApprovalEmployeeID", leaveApproval.LastApprovalEmployeeID);
                param.Add("@Comments", leaveApproval.Comments);
                var res = await _dapperGeneric.Insert<LeaveApproval>(ApplicationConstant.Sp_CreatLeaveApproval, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    if (leaveApproval.leaveApprovalLineItems.Count > 0)
                    {
                        foreach (var item in leaveApproval.leaveApprovalLineItems)
                        {
                            var items = new DynamicParameters();
                            items.Add("@LeaveApprovalId", res.LeaveApprovalId);
                            items.Add("@ApprovalPosition", item.ApprovalPosition);
                            items.Add("@ApprovalEmployeeID", item.ApprovalEmployeeId);
                            // items.Add("@ApprovalStatus", item.ApprovalStatus);
                            items.Add("@Comments", item.Comments);
                            items.Add("@ApprovalStep", item.ApprovalStep);
                            //items.Add("@IsApproved", item.IsApproved);

                            var res1 = await _dapperGeneric.Insert<LeaveApprovalLineItem>(ApplicationConstant.Sp_CreateLeaveApprovalLineItem, items, commandType: CommandType.StoredProcedure);
                            if (res1 != null)
                            {
                                res.leaveApprovalLineItems.Add(res1);
                            }
                        }

                        //_ = UpdateLeaveApprovalRequestLineItemId(leaveApproval);
                    }

                    return res;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveApproval ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }

        private async Task UpdateLeaveApprovalRequestLineItemId(LeaveApproval leaveApproval)
        {
            var param = new DynamicParameters();
            param.Add("@LeaveRequestLineItemId", leaveApproval.LeaveRequestLineItemId);
            param.Add("@LeaveApprovalId", leaveApproval.LeaveApprovalId);

            var res = await _dapperGeneric.Insert<LeaveApproval>(ApplicationConstant.Sp_UpdateLeaveApprovalRequestLineItemID, param, commandType: CommandType.StoredProcedure);

        }

        public async Task<Approvals> CreateApproval(Approvals approvals)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ApprovalEmployeeID", approvals.ApprovalEmployeeID);
                // param.Add("@ApprovalStatus", approvals.ApprovalStatus);
                param.Add("@RequiredApprovalCount", approvals.RequiredApprovalCount);
                //  param.Add("@CurrentApprovalCount", approvals.CurrentApprovalCount);
                param.Add("@Comment", approvals.Comment);
                //  param.Add("@EntryDate", approvals.EntryDate);
                //  param.Add("@DateCompleted", approvals.DateCompleted);
                param.Add("@ApprovalDescription", approvals.ApprovalDescription);
                param.Add("@CompanyID", approvals.CompanyID);
                //  param.Add("@IsApproved", approvals.IsApproved);
                //if (!string.IsNullOrEmpty(v))
                //{
                //    param.Add("@ApprovalStatus", "All");
                //}
                //else
                //{
                //    param.Add("@ApprovalStatus", "Pending");
                //}

                var res = await _dapperGeneric.Insert<Approvals>(ApplicationConstant.Sp_CreateApprovals, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    if (approvals.ApprovalsLineItems.Count > 0)
                    {
                        foreach (var item in approvals.ApprovalsLineItems)
                        {
                            var items = new DynamicParameters();
                            items.Add("@ApprovalID", res.ApprovalID);
                            items.Add("@ApprovalPosition", item.ApprovalPosition);
                            items.Add("@ApprovalEmployeeID", item.ApprovalEmployeeID);
                            // items.Add("@ApprovalStatus", item.ApprovalStatus);
                            items.Add("@Comments", item.Comments);
                            items.Add("@ApprovalStep", item.ApprovalStep);
                            //items.Add("@IsApproved", item.IsApproved);

                            var res1 = await _dapperGeneric.Insert<ApprovalsLineItem>(ApplicationConstant.Sp_CreateApprovalsLineItem, items, commandType: CommandType.StoredProcedure);
                            if (res1 != null)
                            {
                                res.ApprovalsLineItems.Add(res1);
                            }
                        }
                    }
                    return res;
                }

                return default;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveApproval ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }

        //public async Task<AnnualLeave> GetAnnualLeaveInfo(long leaveApprovalId)
        //{
        //    try
        //    {
        //        var param = new DynamicParameters();
        //        param.Add("@ApprovalID", leaveApprovalId);

        //        var res = await _dapperGeneric.Get<AnnualLeave>(ApplicationConstant.Sp_GetAnnualLeaveInfo, param, commandType: CommandType.StoredProcedure);
        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"MethodName: GetAnnualLeaveInfo ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
        //        return null;
        //    }
        //}
        public async Task<LeaveApproval> CreateAnnualLeaveApproval(LeaveApproval leaveApproval)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@LeaveRequestLineItemId", leaveApproval.LeaveRequestLineItemId);
                param.Add("@RequiredApprovalCount", leaveApproval.RequiredApprovalCount);
                param.Add("@EmployeeID", leaveApproval.EmployeeID);
                param.Add("@LastApprovalEmployeeID", leaveApproval.LastApprovalEmployeeID);
                param.Add("@Comments", leaveApproval.Comments);
                //  param.Add("@IsApproved", approvals.IsApproved);
                //if (!string.IsNullOrEmpty(v))
                //{
                //    param.Add("@ApprovalStatus", "All");
                //}
                //else
                //{
                //    param.Add("@ApprovalStatus", "Pending");
                //}

                var res = await _dapperGeneric.Insert<LeaveApproval>(ApplicationConstant.Sp_CreatLeaveApproval, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    if (leaveApproval.leaveApprovalLineItems.Count > 0)
                    {
                        foreach (var item in leaveApproval.leaveApprovalLineItems)
                        {
                            var items = new DynamicParameters();
                            items.Add("@LeaveApprovalId", res.LeaveApprovalId);
                            items.Add("@ApprovalPosition", item.ApprovalPosition);
                            items.Add("@ApprovalEmployeeID", item.ApprovalEmployeeId);
                            // items.Add("@ApprovalStatus", item.ApprovalStatus);
                            items.Add("@Comments", item.Comments);
                            items.Add("@ApprovalStep", item.ApprovalStep);
                            //items.Add("@IsApproved", item.IsApproved);

                            var res1 = await _dapperGeneric.Insert<LeaveApprovalLineItem>(ApplicationConstant.Sp_CreateLeaveApprovalLineItem, items, commandType: CommandType.StoredProcedure);
                            if (res1 != null)
                            {
                                res.leaveApprovalLineItems.Add(res1);
                            }
                        }
                    }
                    return res;
                }

                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveApproval ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
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

                var res = await _dapperGeneric.GetAll<LeaveApproval>(ApplicationConstant.Sp_GetAnnualLeaveApprovals, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAnnualLeaveApprovals ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }

        public async Task<AnnualLeave> GetAnnualLeaveInfo(long leaveApprovalId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ApprovalID", leaveApprovalId);

                var res = await _dapperGeneric.Get<AnnualLeave>(ApplicationConstant.Sp_GetAnnualLeaveInfo, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    res.leaveRequestLineItems = await GetAnnualLeaveRequestLineItem(res.AnnualLeaveId);

                    return res;
                }
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAnnualLeaveInfo ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return null;
            }

        }

        private async Task<List<LeaveRequestLineItemDto>> GetAnnualLeaveRequestLineItem(int AnnualLeaveId)
        {
            var param = new DynamicParameters();
            param.Add("@AnnualLeaveId", AnnualLeaveId);

            var res = await _dapperGeneric.GetAll<LeaveRequestLineItemDto>(ApplicationConstant.Sp_GetAnnualLeaveRequestLineItems, param, commandType: CommandType.StoredProcedure);
            if (res != null && res.Count > 0)
            {
                return res;
            }
            return null;
        }

        private async Task<List<LeaveApprovalLineItem>> GetAllApprovalLineItems(long approvalEmployeeID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ApprovalEmployeeID", approvalEmployeeID);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.GetAll<LeaveApprovalLineItem>(ApplicationConstant.Sp_GetAllLeaveApprovalLineItems, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllApprovalLineItems ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }

        private async Task<List<LeaveRequestLineItem>> GetLeaveRequestLineItems(long AnnualLeaveID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@AnnualLeaveID", AnnualLeaveID);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.GetAll<LeaveRequestLineItem>(ApplicationConstant.Sp_GetAnnualLeaveRequestLineItems, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    //res = res.FindAll(x => x.LeaveTypeName == LeaveTypeName);

                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllLeaveEmpRequestLineItems ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }

        private async Task<List<LeaveRequestLineItemDto>> GetLeaveRequestLineItems(long EmployeeID, string LeaveTypeName)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeID", EmployeeID);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.GetAll<LeaveRequestLineItemDto>(ApplicationConstant.Sp_GetAllEmpLeaveRequestLineItems, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    res = res.FindAll(x => x.LeaveTypeName == LeaveTypeName);
                   
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllLeaveEmpRequestLineItems ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
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
                param.Add("@ApprovalKey", leaveApproval.ApprovalKey);


                var res = await _dapperGeneric.Get<LeaveApprovalInfo>(ApplicationConstant.Sp_UpdateLeaveApproval, param, commandType: CommandType.StoredProcedure);
                _logger.LogInformation($"Response from Sp_UpdateLeaveApproval: {JsonConvert.SerializeObject(res)}");

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
