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
              
                List<PendingLeaveApprovalItemsDto> leaveApprovalItems = new List<PendingLeaveApprovalItemsDto>();
                List<PendingAnnualLeaveApprovalItemDto> pendingRes = new List<PendingAnnualLeaveApprovalItemDto>();
                PendingLeaveApprovalItemsDto pendingLeaveApproval = null;
            
                var leaveApprovalLineItems = await GetAllApprovalLineItems(approvalEmployeeID);
                bool isValidItem = false;
                foreach (var item in leaveApprovalLineItems)
                {
                    var leaveapproval = await GetLeaveApprovalInfo(item.LeaveApprovalId);
                    var leaverequestLineitem =  GetLeaveRequestLineItem(leaveapproval.LeaveRequestLineItemId).Result;
                    if (leaverequestLineitem.AnnualLeaveId > 0)
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
                            //if (leaveapproval.ApprovalStatus.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                            //{
                            //    if (leaveapproval.LastApprovalEmployeeID == item.ApprovalEmployeeId &&
                            //        leaveapproval.CurrentApprovalCount == item.ApprovalStep)
                            //    {
                            //        isValidItem = true;
                                    
                            //    }
                            //    //else if (item.ApprovalStep > leaveapproval.CurrentApprovalCount)
                            //    //{
                            //    //    isValidItem = true;
                            //    //}
                            //    leaveApprovalRequestItem.Comments = leaveapproval.Comments;
                            //}
                            //else if (leaveapproval.ApprovalStatus.Equals("Completed", StringComparison.OrdinalIgnoreCase))
                            //{
                            //    if (leaveApprovalRequestItem.LastApprovalEmployeeID == item.ApprovalEmployeeId &&
                            //        leaveapproval.CurrentApprovalCount == item.ApprovalStep)
                            //    {
                            //        isValidItem = true;

                            //    }
                              
                            //    leaveApprovalRequestItem.Comments = leaveapproval.Comments;
                            //    var approvalItemId = leaveApprovalItems.FirstOrDefault(x=>x.ApprovalEmployeeId ==  item.ApprovalEmployeeId && x.LeaveApprovalId == item.LeaveApprovalId);
                            //    if (approvalItemId != null)
                            //    {
                            //        //skip
                            //        isValidItem = false;

                            //    }
                            //    else
                            //    {
                            //        isValidItem = true;
                            //    }
                            //}
                            //else
                            //{
                            //    leaveApprovalRequestItem.Comments = item.ApprovalStatus + "," + item.Comments;
                            //    isValidItem = true;
                            //}

                            //if (ConfigSettings.leaveRequestConfig.EnableSingleApproval) // only one item of approval employeeid will show up in the list where the approver has more than one approval position.
                            //{
                            //    if (isValidItem)
                            //    {
                            //        leaveApprovalItems.Add(leaveApprovalRequestItem);
                            //        isValidItem = false;
                            //    }
                            //}
                            //else
                            //{
                            //}

                            leaveApprovalItems.Add(leaveApprovalRequestItem);
                        }
                      
                    }
                }

                return leaveApprovalItems;

                var res = await _dapperGeneric.GetAll<PendingLeaveApprovalItemsDto>(ApplicationConstant.Sp_GetPendingLeaveApprovals, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    bool ispresent = false;
                    bool ispresent1 = false;
                    List<PendingLeaveApprovalItemsDto> res1 = new List<PendingLeaveApprovalItemsDto>();
                    //List<PendingAnnualLeaveApprovalItemDto> pendingRes = new List<PendingAnnualLeaveApprovalItemDto>();


                    foreach (var item in res)
                    {
                        var approvalLineItems = await GetLeaveApprovalLineItems(item.LeaveApprovalId);
                        approvalLineItems = approvalLineItems.OrderBy(x => x.ApprovalStep).ToList();
                        var approvalLineItem = approvalLineItems.FirstOrDefault(x => x.ApprovalEmployeeId == approvalEmployeeID && x.ApprovalPosition == item.Comments.Split(" ").Last().Trim());
                        item.ApprovalStep = approvalLineItem.ApprovalStep;
                        item.ApprovalPosition = approvalLineItem.ApprovalPosition;
                        item.ApprovalStatus = approvalLineItem.ApprovalStatus;
                        item.LeaveApprovalLineItemId = approvalLineItem.LeaveApprovalLineItemId;
                        item.ApprovalEmployeeId = approvalLineItem.ApprovalEmployeeId;
                    }
                    res = res.OrderBy(x => x.ApprovalStep).ToList();
                    foreach (var item in res)
                    {

                        var pendingLeaveItem = res1.FirstOrDefault(x => x.EmployeeID == item.EmployeeID);
                        if (pendingLeaveItem == null)
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

        private async Task<LeaveRequestLineItem> GetLeaveRequestLineItem(long LeaveRequestLineItem)
        {
            var param = new DynamicParameters();
            param.Add("@@LeaveRequestLineItemId", LeaveRequestLineItem);

            var res = await _dapperGeneric.Get<LeaveRequestLineItem>(ApplicationConstant.Sp_GetLeaveRequestLineItem, param, commandType: CommandType.StoredProcedure);

            return res;
        }

        public async Task<List<PendingAnnualLeaveApprovalItemDto>> GetPendingAnnualLeaveApprovals(long approvalEmployeeID, string v)
        {
            List<PendingAnnualLeaveApprovalItemDto> pendingRes = new List<PendingAnnualLeaveApprovalItemDto>();

            PendingAnnualLeaveApprovalItemDto pendingAnnualLeaveApprovalItemDto = null;
            try
            {
                var leaveApprovalLineItems = await GetAllApprovalLineItems(approvalEmployeeID);

                foreach (var item in leaveApprovalLineItems)
                {
                    
                    var res1 = await GetAnnualLeaveInfo(item.LeaveApprovalId);
                    if (res1 != null)
                    {
                        string comments = string.Empty;
                        var leaveapproval = await GetLeaveApprovalInfo(item.LeaveApprovalId); 
                        
                        if (leaveapproval != null)
                        {
                            comments = leaveapproval.Comments;
                        }

                        foreach (var item1 in res1.leaveRequestLineItems)
                        {
                            item1.LeaveApprovalLineItemId = item.LeaveApprovalLineItemId;
                            item1.LeaveApprovalId = item.LeaveApprovalId;
                            item1.ApprovalStatus = item.ApprovalStatus;
                            item1.CompanyId = res1.CompanyID;
                            item1.Comments = comments; // + "," + item.Comments;
                        }
                       
                        pendingAnnualLeaveApprovalItemDto = new()
                        {
                            FullName = res1.leaveRequestLineItems.FirstOrDefault().FullName,
                            RelieverName = res1.leaveRequestLineItems.FirstOrDefault().RelieverName,
                            ApprovalStatus = item.ApprovalStatus,
                            //ApprovalEmployeeID = approvalEmployeeID,
                            Year = res1.leaveRequestLineItems.FirstOrDefault().startDate.Year.ToString(),
                            EmployeeID = item.EmployeeID,
                            LeaveCount = res1.leaveRequestLineItems.Count, // res.FindAll(x => x.EmployeeID == item.EmployeeID).Count(),
                            LeaveTypeName = res1.leaveRequestLineItems.FirstOrDefault().LeaveTypeName,
                            leaveRequestLineItems = res1.leaveRequestLineItems, // res.FindAll(x => x.EmployeeID == item.EmployeeID),
                            Status = comments, // item.Comments,
                            leaveApprovalId = leaveapproval.LeaveApprovalId,
                            ApprovalPosition = item.ApprovalPosition,
                            LastApprovalEmployeeId = leaveapproval.LastApprovalEmployeeID,
                            TotalNoOfDays = res1.leaveRequestLineItems.Sum(x => x.LeaveLength)  // res.FindAll(x => x.EmployeeID == item.EmployeeID).Sum(x => x.LeaveLength)
                        };


                        pendingRes.Add(pendingAnnualLeaveApprovalItemDto);
                        //if (ConfigSettings.leaveRequestConfig.EnableSingleApproval)
                        //{
                        //    if (leaveapproval.ApprovalStatus.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                        //    {
                        //        if (leaveapproval.LastApprovalEmployeeID == item.ApprovalEmployeeId && leaveapproval.CurrentApprovalCount == item.ApprovalStep)
                        //        {
                        //            pendingRes.Add(pendingAnnualLeaveApprovalItemDto);
                        //        }
                        //        else if(item.ApprovalStatus.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                        //        {
                        //            pendingRes.Add(pendingAnnualLeaveApprovalItemDto);
                        //        }
                        //    }
                        //    else if (leaveapproval.ApprovalStatus.Equals("Completed", StringComparison.OrdinalIgnoreCase))
                        //    {
                        //        if (leaveapproval.LastApprovalEmployeeID == item.ApprovalEmployeeId &&
                        //            leaveapproval.CurrentApprovalCount == item.ApprovalStep)
                        //        {
                        //            pendingRes.Add(pendingAnnualLeaveApprovalItemDto);

                        //        }

                        //        leaveapproval.Comments = leaveapproval.Comments;
                        //        var approvalItemId = pendingRes.FirstOrDefault(x=>x.LastApprovalEmployeeId ==  item.ApprovalEmployeeId && x.leaveApprovalId == item.LeaveApprovalId);
                        //        if (approvalItemId != null)
                        //        {
                        //            //skip
                        //           // isValidItem = false;

                        //        }
                        //        else
                        //        {
                        //        }

                        //    }
                        //}
                    }
                }

                return pendingRes;
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
                    bool ispresent = false;
                    bool ispresent1 = false;
                  //  List<PendingLeaveApprovalItemsDto> res1 = new List<PendingLeaveApprovalItemsDto>();
                //    List<PendingAnnualLeaveApprovalItemDto> pendingRes = new List<PendingAnnualLeaveApprovalItemDto>();



                  //  PendingAnnualLeaveApprovalItemDto pendingAnnualLeaveApprovalItemDto = null;
                    //foreach (var item in res)
                    //{
                    //    var leaverequests = await GetLeaveRequestLineItems(item.AnnualLeaveID);
                    //    foreach (var item1 in leaverequests)
                    //    {

                    //        var leaveApprovalItem = (await GetLeaveApprovalLineItems(item.LeaveApprovalId)).FirstOrDefault(x => x.ApprovalEmployeeId == item.LastApprovalEmployeeID && x.ApprovalStatus == "Pending");
                    //        item1.leaveApprovalLineItemId = leaveApprovalItem.LeaveApprovalLineItemId;
                    //        item1.leaveApprovalId = leaveApprovalItem.LeaveApprovalId;
                    //        var leaveApproval = await GetLeaveApprovalInfo(item.LeaveApprovalId);
                    //        item1.Comments = leaveApproval.Comments;
                    //        item1.ApprovalStatus = leaveApproval.ApprovalStatus;
                    //        item1.LeaveTypeName = item.LeaveTypeName;
                    //        item1.RelieverName = item.RelieverName;
                    //    }
                    //    pendingAnnualLeaveApprovalItemDto = new()
                    //    {
                    //        FullName = item.FullName,
                    //        RelieverName = item.RelieverName,
                    //        ApprovalStatus = item.ApprovalStatus,
                    //        //ApprovalEmployeeID = approvalEmployeeID,
                    //        Year = item.StartDate.Year.ToString(),
                    //        EmployeeID = item.EmployeeID,
                    //        LeaveCount = item.LeaveCount, // res.FindAll(x => x.EmployeeID == item.EmployeeID).Count(),
                    //        LeaveTypeName = item.LeaveTypeName,
                    //        leaveRequestLineItems = leaverequests, // res.FindAll(x => x.EmployeeID == item.EmployeeID),
                    //        Status = item.Comments,
                    //        TotalNoOfDays = item.TotalNoOfDays  // res.FindAll(x => x.EmployeeID == item.EmployeeID).Sum(x => x.LeaveLength)
                    //    };


                    //    pendingRes.Add(pendingAnnualLeaveApprovalItemDto);
                    //}
                   

                   // var approvalLineItems = await GetLeaveApprovalLineItems(res.LeaveApprovalId);
                   // approvalLineItems = approvalLineItems;
                  //  approvalLineItems = approvalLineItems.FindAll(x => x.ApprovalEmployeeId == approvalEmployeeID).OrderBy(x => x.ApprovalStep).ToList();

                    //List<PendingLeaveApprovalItemsDto> pendingLeaveApprovalItems = new List<PendingLeaveApprovalItemsDto>();
                    //foreach (var item in approvalLineItems)
                    //{
                    //    res.ApprovalStep = item.ApprovalStep;
                    //    res.ApprovalStatus = item.ApprovalStatus;
                    //    res.ApprovalPosition = item.ApprovalPosition;
                    //    res.LeaveApprovalId = item.LeaveApprovalId;
                    //    res.ApprovalEmployeeId = item.ApprovalEmployeeId;
                    //    res.Comments = item.Comments;
                    //    res.IsApproved = item.IsApproved;
                    //    res.LeaveApprovalLineItemId = item.LeaveApprovalLineItemId;
                    //    pendingLeaveApprovalItems.Add(res);
                    //}
                    //foreach (var item in leaverequests)
                    //{
                    //    res.StartDate = item.startDate;
                    //    res.EndDate = item.endDate;
                    //    res.LeaveLength = item.LeaveLength;
                    //    item.leaveApprovalLineItemId = res.LeaveApprovalLineItemId;
                    //    item.leaveApprovalId = res.LeaveApprovalId;
                    //}

                    //pendingAnnualLeaveApprovalItemDto = new()
                    //{
                    //    FullName = res.FullName,
                    //    //ApprovalEmployeeID = approvalEmployeeID,
                    //    Year = res.StartDate.Year.ToString(),
                    //    EmployeeID = res.EmployeeID,
                    //    LeaveCount = res.LeaveCount, //.FindAll(x => x.EmployeeID == item.EmployeeID).Count(),
                    //    LeaveTypeName = res.LeaveTypeName,
                    //    leaveRequestLineItems = leaverequests, // res.FindAll(x => x.EmployeeID == item.EmployeeID),
                    //    Status = item.Comments,
                    //    TotalNoOfDays = res.FindAll(x => x.EmployeeID == item.EmployeeID).Sum(x => x.LeaveLength)
                    //};
                    //item.ApprovalStep = approvalLineItem.ApprovalStep;
                    //item.ApprovalPosition = approvalLineItem.ApprovalPosition;
                    //item.ApprovalStatus = approvalLineItem.ApprovalStatus;
                    //item.LeaveApprovalLineItemId = approvalLineItem.LeaveApprovalLineItemId;
                    //item.ApprovalEmployeeId = approvalLineItem.ApprovalEmployeeId;

                    //foreach (var item in res)
                    //{

                    //}
                    //res = res.OrderBy(x => x.ApprovalStep).ToList();
                    //foreach (var item in res)
                    //{

                    //    var pendingLeaveItem = res1.FirstOrDefault(x=>x.EmployeeID == item.EmployeeID);
                    //    if (pendingLeaveItem == null)
                    //    {
                    //        res1.Add(item);
                    //    }

                    //}

                    //foreach (var item in res)
                    //{
                    //    pendingAnnualLeaveApprovalItemDto = new()
                    //    {
                    //        FullName = item.FullName,
                    //        //ApprovalEmployeeID = approvalEmployeeID,
                    //        Year = item.StartDate.Year.ToString(),
                    //        EmployeeID = item.EmployeeID,
                    //        LeaveCount = res.FindAll(x => x.EmployeeID == item.EmployeeID).Count(),
                    //        LeaveTypeName = item.LeaveTypeName,
                    //        leaveRequestLineItems = res.FindAll(x=>x.EmployeeID == item.EmployeeID),
                    //        Status = item.Comments,
                    //        TotalNoOfDays = res.FindAll(x => x.EmployeeID == item.EmployeeID).Sum(x => x.LeaveLength)
                    //    };


                    //    pendingRes.Add(pendingAnnualLeaveApprovalItemDto);
                    //}
                }
                return null;

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
        private async Task<AnnualLeave> GetAnnualLeaveInfo(long leaveApprovalId)
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

                throw;
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
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
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
