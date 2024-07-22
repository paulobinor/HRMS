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
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;
using System.Text.Json;

namespace hrms_be_backend_data.Repository
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

        public async Task<LeaveRequestLineItem> CreateLeaveRequestLineItem(LeaveRequestLineItem leaveRequestLineItem)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", leaveRequestLineItem.EmployeeId);
                param.Add("@LeaveTypeId", leaveRequestLineItem.LeaveTypeId);
                param.Add("@endDate", leaveRequestLineItem.endDate);
                param.Add("@HandoverNotes", leaveRequestLineItem.HandoverNotes);
                param.Add("@UploadFilePath", leaveRequestLineItem.UploadFilePath);
                param.Add("@LeaveLength", leaveRequestLineItem.LeaveLength);
                param.Add("@LeaveRequestId", leaveRequestLineItem.LeaveRequestId);
                param.Add("@RelieverUserId", leaveRequestLineItem.RelieverUserId);
                param.Add("@AnnualLeaveId", leaveRequestLineItem.AnnualLeaveId);
                param.Add("@startDate", leaveRequestLineItem.startDate);
              //  param.Add("@IsRescheduled", "0");
                param.Add("@ResumptionDate", leaveRequestLineItem.ResumptionDate);

                var res = await _dapperGeneric.Get<LeaveRequestLineItem>(ApplicationConstant.Sp_CreateEmpLeaveRequestLineItem1, param, commandType: CommandType.StoredProcedure);
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex}");
                throw;
            }
        }
        public async Task<List<LeaveRequestLineItem>> RescheduleAnnualLeaveRequest(List<LeaveRequestLineItem> leaveRequestLineItems)
        {
            List<LeaveRequestLineItem> responseItems = new List<LeaveRequestLineItem>(); 
            foreach (var leaveRequestLineItem in leaveRequestLineItems)
            {
                try
                {
                    using (SqlConnection _dapper = new SqlConnection(_connectionString))
                    {
                        var param = new DynamicParameters();
                        //  param.Add("@Status", LeaveRequestEnum.UPDATE);
                        param.Add("@LeaveRequestLineItemId", leaveRequestLineItem.LeaveRequestLineItemId);
                        param.Add("@LeaveTypeID", leaveRequestLineItem.LeaveTypeId);
                        param.Add("@startDate", leaveRequestLineItem.startDate);
                        param.Add("@endDate", leaveRequestLineItem.endDate);
                        param.Add("@HandoverNotes", leaveRequestLineItem.HandoverNotes);
                        param.Add("@UploadFilePath", leaveRequestLineItem.UploadFilePath);
                        param.Add("@RescheduleReason", leaveRequestLineItem.RescheduleReason);
                        param.Add("@ResumptionDate", leaveRequestLineItem.ResumptionDate);
                        param.Add("@RelieverUserId", leaveRequestLineItem.RelieverUserId);
                        param.Add("@LeaveLength", leaveRequestLineItem.LeaveLength);
                        param.Add("@IsRescheduled", leaveRequestLineItem.IsRescheduled);

                        var res = await _dapperGeneric.Get<LeaveRequestLineItem>(ApplicationConstant.Sp_RescheduleLeaveRequestLineItem, param, commandType: CommandType.StoredProcedure);
                        responseItems.Add(res);
                    }
                }
                catch (Exception ex)
                {
                    var err = ex.Message;
                    _logger.LogError($"MethodName: RescheduleLeaveRequest(RescheduleLeaveRequest update) ===>{ex.Message}");
                    throw;
                }
            }
            return responseItems;
        }
        public async Task<LeaveRequestLineItem> RescheduleLeaveRequest(LeaveRequestLineItem leaveRequestLineItem)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    //  param.Add("@Status", LeaveRequestEnum.UPDATE);
                    param.Add("@LeaveRequestLineItemId", leaveRequestLineItem.LeaveRequestLineItemId);
                    param.Add("@LeaveTypeID", leaveRequestLineItem.LeaveTypeId);
                    param.Add("@startDate", leaveRequestLineItem.startDate);
                    param.Add("@endDate", leaveRequestLineItem.endDate);
                    param.Add("@HandoverNotes", leaveRequestLineItem.HandoverNotes);
                    param.Add("@UploadFilePath", leaveRequestLineItem.UploadFilePath);
                    param.Add("@RescheduleReason", leaveRequestLineItem.RescheduleReason);
                    param.Add("@ResumptionDate", leaveRequestLineItem.ResumptionDate);
                    param.Add("@RelieverUserId", leaveRequestLineItem.RelieverUserId);
                    param.Add("@LeaveLength", leaveRequestLineItem.LeaveLength);
                    param.Add("@IsRescheduled", true);

                    var res = await _dapperGeneric.Get<LeaveRequestLineItem>(ApplicationConstant.Sp_RescheduleLeaveRequestLineItem, param, commandType: CommandType.StoredProcedure);

                    return res;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: RescheduleLeaveRequest(RescheduleLeaveRequest update) ===>{ex.Message}");
                throw;
            }
        }
        public async Task<LeaveApprovalLineItemDto> UpdateLeaveApprovalLineItem(LeaveApprovalLineItemDto leaveApprovalLineItem)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@LeaveApprovalLineItemId", leaveApprovalLineItem.LeaveApprovalLineItemId);
                //param.Add("@LeaveApprovalId", leaveApprovalLineItem.LeaveApprovalId);
                param.Add("@IsApproved", leaveApprovalLineItem.IsApproved);
                param.Add("@ApprovalEmployeeId", leaveApprovalLineItem.ApprovalEmployeeId);
                param.Add("@Comments", leaveApprovalLineItem.Comments);
                param.Add("@EntryDate", leaveApprovalLineItem.EntryDate);

                var res = await _dapperGeneric.Get<LeaveApprovalLineItemDto>(ApplicationConstant.Sp_UpdateLeaveApprovalLineItem, param, commandType: CommandType.StoredProcedure);
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
        public async Task<EmpLeaveRequestInfo> CreateEmpLeaveInfo(long employeeId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", employeeId);
               // param.Add("@CompanyId", CompanyId);
                // param.Add("@LeaveStatus", LeaveStatus);

                var res = await _dapperGeneric.Get<EmpLeaveRequestInfo>(ApplicationConstant.Sp_CreateEmpLeaveInfo, param, commandType: CommandType.StoredProcedure);
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
        public async Task<List<EmpLeaveRequestInfo>> GetEmpLeaveInfoHistory(BigInteger employeeId, BigInteger companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.GETALL);

                    var LeaveDetails = (await _dapper.QueryAsync<EmpLeaveRequestInfo>(ApplicationConstant.Sp_GetEmpLeaveInfoHistory, param: param, commandType: CommandType.StoredProcedure)).AsList();

                    return LeaveDetails;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllLeaveRequest() ===>{ex.Message}");
                throw;
            }
        }
        public async Task<LeaveRequestLineItem> GetLeaveRequestLineItem(long leaveRequestLineItemId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@leaveRequestLineItemId", leaveRequestLineItemId);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.Get<LeaveRequestLineItem>(ApplicationConstant.Sp_GetLeaveRequestLineItem, param, commandType: CommandType.StoredProcedure);
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
        public async Task<List<LeaveRequestLineItem>> GetLeaveRequestLineItems(long leaveRequestId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@leaveRequestId", leaveRequestId);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.GetAll<LeaveRequestLineItem>(ApplicationConstant.Sp_GetLeaveRequestLineItems, param, commandType: CommandType.StoredProcedure);
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

        public async Task<List<LeaveRequestLineItemDto>> GetAllLeaveRequestLineItems(long CompanyID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyID", CompanyID);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.GetAll<LeaveRequestLineItemDto>(ApplicationConstant.Sp_GetAllLeaveRequestLineItems, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    foreach (var item in res)
                    {
                        var leaveAprovalId = (await GetLeaveApprovalInfoByEmployeeId(item.EmployeeId)).LeaveApprovalId;
                        if (leaveAprovalId > 0)
                        {
                            item.leaveApprovalLineItems = await GetLeaveApprovalLineItems(leaveAprovalId);
                            //var lineitems = await GetLeaveApprovalLineItems(leaveAprovalId);
                            //if (lineitems.Count > 0)
                            //{
                            //    foreach (var itelitem in lineitems)
                            //    {
                            //        item.leaveApprovalLineItemList.Add(itelitem);
                            //    }
                            //}
                        }
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

        public async Task<List<LeaveRequestLineItemDto>> GetAllAnnualLeaveRequestLineItems(long CompanyID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyID", CompanyID);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.GetAll<LeaveRequestLineItemDto>(ApplicationConstant.Sp_GetAllAnnualLeaveRequestLineItems, param, commandType: CommandType.StoredProcedure);
               // res = res.FindAll(x=>x.LeaveTypeName.Contains("Annual"))
                if (res != null)
                {
                    foreach (var item in res)
                    {
                        var leaveAprovalId = (await GetLeaveApprovalInfoByEmployeeId(item.EmployeeId)).LeaveApprovalId;
                        if (leaveAprovalId > 0)
                        {
                            item.leaveApprovalLineItems = await GetLeaveApprovalLineItems(leaveAprovalId);
                            
                            //var lineitems = await GetLeaveApprovalLineItems(leaveAprovalId);
                            //if (lineitems.Count > 0)
                            //{
                            //    foreach (var itelitem in lineitems)
                            //    {
                            //        item.leaveApprovalLineItemList.Add(itelitem);
                            //    }
                            //}
                        }
                    }
                    res = res.OrderByDescending(x=>x.startDate).ToList();
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
        public async Task<List<LeaveRequestLineItemDto>> GetEmployeeLeaveRequests(long CompanyID, long EmployeeID)
        {
            var param = new DynamicParameters();
            param.Add("@EmployeeID", EmployeeID);
            param.Add("@CompanyID", CompanyID);
            try
            {
                var res = await _dapperGeneric.GetAll<LeaveRequestLineItemDto>(ApplicationConstant.Sp_GetEmployeeLeaveRequests, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    foreach (var item in res)
                    {
                        var leaveAprovalId = (await GetLeaveApprovalInfoByEmployeeId(item.EmployeeId)).LeaveApprovalId;
                        if (leaveAprovalId > 0)
                        {
                            item.leaveApprovalLineItems = await GetLeaveApprovalLineItems(leaveAprovalId);
                            //var lineitems = await GetLeaveApprovalLineItems(leaveAprovalId);
                            //if (lineitems.Count > 0)
                            //{
                            //    foreach (var itelitem in lineitems)
                            //    {
                            //        item.leaveApprovalLineItemList.Add(itelitem);
                            //    }
                            //}
                        }
                    }
                    return res;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
            return null;
        }
        private async Task<List<LeaveRequestLineItemDto>> GetAllLeaveEmpRequestLineItems(long EmployeeID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeID", EmployeeID);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.GetAll<LeaveRequestLineItemDto>(ApplicationConstant.Sp_GetAllEmpLeaveRequestLineItems, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    res = res.FindAll(x => !x.LeaveTypeName.Contains("Annual", StringComparison.OrdinalIgnoreCase));
                    foreach (var item in res)
                    {
                        var leaveAprovalId = (await GetLeaveApprovalInfoByEmployeeId(item.EmployeeId)).LeaveApprovalId;
                        if (leaveAprovalId > 0)
                        {
                            item.leaveApprovalLineItems = await GetLeaveApprovalLineItems(leaveAprovalId);
                        }
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


        public async Task<LeaveApprovalLineItem> UpdateLeaveRequestApprovalID(LeaveRequestLineItem leaveRequestLineItem)
        {
            var param = new DynamicParameters();
            param.Add("@ApproalID", leaveRequestLineItem.ApproalID);
            param.Add("@LeaveRequestLineItemId", leaveRequestLineItem.LeaveRequestLineItemId);

            try
            {
                
                var res = await _dapperGeneric.Get<LeaveApprovalLineItem>(ApplicationConstant.Sp_UpdateLeaveRequestApprovalID, param, commandType: CommandType.StoredProcedure);
                _logger.LogInformation($"Update Leave request ApprovalID ===> Response payload: {JsonConvert.SerializeObject(res)}");
                if (res != null)
                {
                    //  res.leaveRequestLineItems = await GetAnnualLeaveRequestLineItems(employeeId);
                    //  res.NoOfDaysTaken = res.leaveRequestLineItems.Sum(x => x.LeaveLength);
                    return res;
                }
                return default;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: Update Leave request ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }
        public async Task<AnnualLeave> UpdateAnnualLeave(AnnualLeave annualLeave)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@NoOfDaysTaken", annualLeave.NoOfDaysTaken);
                param.Add("@TotalNoOfDays", annualLeave.TotalNoOfDays);
                param.Add("@ApprovalStatus", annualLeave.ApprovalStatus);
                param.Add("@AnnualLeaveId", annualLeave.AnnualLeaveId);

                var res = await _dapperGeneric.Get<AnnualLeave>(ApplicationConstant.Sp_UpdateEmpAnnualLeave, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    //  res.leaveRequestLineItems = await GetAnnualLeaveRequestLineItems(employeeId);
                    //  res.NoOfDaysTaken = res.leaveRequestLineItems.Sum(x => x.LeaveLength);
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

        public async Task<AnnualLeave> CreateAnnualLeaveRequest(AnnualLeave annualLeave, List<LeaveRequestLineItem> requestLineItems)
        {
            var param = new DynamicParameters();
            param.Add("@LeaveRequestId", annualLeave.LeaveRequestId);
            param.Add("@CompanyID", annualLeave.CompanyID);
            param.Add("@DateCreated", annualLeave.DateCreated);
            param.Add("@EmployeeId", annualLeave.EmployeeId);
            param.Add("@LeavePeriod", annualLeave.LeavePeriod);
            param.Add("@NoOfDaysTaken", annualLeave.NoOfDaysTaken);
            param.Add("@TotalNoOfDays", annualLeave.TotalNoOfDays);
            param.Add("@SplitCount", annualLeave.SplitCount);
            param.Add("@ApprovalStatus", "Pending");

            try
            {
                var res = await _dapperGeneric.Get<AnnualLeave>(ApplicationConstant.Sp_CreateEmpAnnualLeave, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    foreach (var leaveRequestLineItem in requestLineItems)
                    {
                        leaveRequestLineItem.LeaveRequestId = annualLeave.LeaveRequestId;
                        leaveRequestLineItem.AnnualLeaveId = res.AnnualLeaveId;

                        _logger.LogInformation($"About to post leave request item to db. Paylaod:  {JsonConvert.SerializeObject(leaveRequestLineItem)}");

                        var lineItem = await CreateLeaveRequestLineItem(leaveRequestLineItem);

                        _logger.LogInformation($"Response after posting leave request to db:  {JsonConvert.SerializeObject(lineItem)}");

                        if (lineItem != null)
                        {
                            LeaveRequestLineItemDto leaveRequestLineItemDto = new LeaveRequestLineItemDto()
                            {
                                LeaveRequestLineItemId = lineItem.LeaveRequestLineItemId,
                                LeaveLength = lineItem.LeaveLength,
                                LeaveTypeId = lineItem.LeaveTypeId,
                                LeaveRequestId = lineItem.LeaveRequestId,
                                RelieverUserId = leaveRequestLineItem.RelieverUserId,
                                ResumptionDate = lineItem.ResumptionDate,
                                CompanyId = leaveRequestLineItem.CompanyId,
                                EmployeeId = lineItem.EmployeeId,
                                HandoverNotes = lineItem.HandoverNotes,
                                startDate = lineItem.startDate,
                                endDate = lineItem.endDate,
                                UploadFilePath = lineItem.UploadFilePath,
                                AnnualLeaveId = res.AnnualLeaveId
                            };
                            res.leaveRequestLineItems.Add(leaveRequestLineItemDto);
                        }
                    }
                    return res;
                }
              
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateLeaveRequest ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return null;
            }
        }

        public async Task<AnnualLeave> CheckAnnualLeaveInfo(LeaveRequestLineItem leaveRequestLineItem)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@LeavePeriod", leaveRequestLineItem.startDate.Year);
                param.Add("@CompanyId", leaveRequestLineItem.CompanyId);
                param.Add("@EmployeeId", leaveRequestLineItem.EmployeeId);

                var res = await _dapperGeneric.Get<AnnualLeave>(ApplicationConstant.Sp_CheckEmpAnnualLeave, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    res.leaveRequestLineItems = await GetAnnualLeaveRequestLineItems(res.AnnualLeaveId);
                    //res.NoOfDaysTaken = res.leaveRequestLineItems.Sum(x=>x.LeaveLength);
                    res.Comments = res.leaveRequestLineItems.FirstOrDefault().Comments;
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

        public async Task<AnnualLeave> GetAnnualLeaveInfo(int AnnualLeaveId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@AnnualLeaveId", AnnualLeaveId);

                var res = await _dapperGeneric.Get<AnnualLeave>(ApplicationConstant.Sp_GetEmpAnnualLeave, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    res.leaveRequestLineItems = await GetAnnualLeaveRequestLineItems(res.AnnualLeaveId);
                    //res.NoOfDaysTaken = res.leaveRequestLineItems.Sum(x=>x.LeaveLength);
                    res.Comments = res.leaveRequestLineItems.FirstOrDefault().Comments;
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
        public async Task<List<AnnualLeave>> GetAnnualLeaveInfo(int employeeId, int companyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", employeeId);
                param.Add("@CompanyId", companyId);

                var res = await _dapperGeneric.GetAll<AnnualLeave>(ApplicationConstant.Sp_GetEmpAnnualLeaveInfo, param, commandType: CommandType.StoredProcedure);
                if (res != null && res.Count > 0)
                {
                    foreach (var item in res)
                    {
                        item.leaveRequestLineItems = await GetAnnualLeaveRequestLineItems(item.AnnualLeaveId);
                        //res.NoOfDaysTaken = res.leaveRequestLineItems.Sum(x=>x.LeaveLength);
                       // res.Comments = res.leaveRequestLineItems.FirstOrDefault().Comments;
                    }
                    
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAnnualLeaveInfo ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }

        private async Task<List<LeaveRequestLineItemDto>> GetAnnualLeaveRequestLineItems(int AnnualLeaveId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@AnnualLeaveId", AnnualLeaveId);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.GetAll<LeaveRequestLineItemDto>(ApplicationConstant.Sp_GetEmpAnnualLeaveRequestLineItems, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    foreach (var item in res)
                    {
                        var leaveAprovalId = (await GetLeaveApprovalInfoByEmployeeId(item.EmployeeId)).LeaveApprovalId;
                        if (leaveAprovalId > 0)
                        {
                            item.leaveApprovalLineItems = await GetLeaveApprovalLineItems(leaveAprovalId);
                        }
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

        public async Task<GradeLeave> GetEmployeeGradeLeave(long employeeId, long leaveTypeId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@employeeId", employeeId);
                param.Add("@LeaveTypeId", leaveTypeId);
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
                return null;
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
                   // res.LeaveApprovalLineItems = await GetLeaveApprovalLineItems(res.LeaveApprovalId);
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
        
        public async Task<LeaveApprovalLineItemDto> GetLeaveApprovalLineItem(long leaveApprovalLineItemId, int approvalStep = 0)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@LeaveApprovalLineItemId", leaveApprovalLineItemId);
                param.Add("@ApprovalStep", approvalStep);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.Get<LeaveApprovalLineItemDto>(ApplicationConstant.Sp_GetLeaveApprovalLineItem, param, commandType: CommandType.StoredProcedure);
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
        public async Task<List<LeaveApprovalLineItemDto>> GetLeaveApprovalLineItems(long leaveApprovalId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@leaveApprovalId", leaveApprovalId);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.GetAll<LeaveApprovalLineItemDto>(ApplicationConstant.Sp_GetLeaveApprovalLineItems, param, commandType: CommandType.StoredProcedure);
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

                var res = await _dapperGeneric.GetAll<LeaveApprovalLineItemDto>(ApplicationConstant.Sp_UpdateLeaveRequestLineItemApproval, param, commandType: CommandType.StoredProcedure);

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
        public async Task<EmpLeaveRequestInfo> GetEmpLeaveInfo(long employeeId, long companyId, string LeaveStatus)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", employeeId);
               // param.Add("@CompanyId", companyId);
                param.Add("@LeaveStatus", LeaveStatus);

                var res = await _dapperGeneric.Get<EmpLeaveRequestInfo>(ApplicationConstant.Sp_GetEmpLeaveInfo, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    res.leaveRequestLineItems = (await GetAllLeaveEmpRequestLineItems(employeeId)).OrderByDescending(x=>x.startDate).ToList();
                    
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
        public async Task<string> ApproveLeaveRequest(long LeaveRequestID, long ApprovedByUserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", 10);
                param.Add("@LeaveRequestID", LeaveRequestID);
                param.Add("@ApprovedByUserId", ApprovedByUserId);
                param.Add("@DateApproved", DateTime.Now);
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
        public async Task<string> CreateLeaveRequest(LeaveRequestCreate Leave)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", LeaveRequestEnum.CREATE);
                param.Add("@EmployeeId", Leave.EmployeeId);
                param.Add("@RequestYear", Leave.RequestYear);
                param.Add("@LeaveTypeId", Leave.LeaveTypeId);
                param.Add("@NoOfDays", Leave.NoOfDays);
                param.Add("@StartDate", Leave.StartDate);
                param.Add("@EndDate", Leave.EndDate);
                param.Add("@RelieverUserID", Leave.RelieverUserID);
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
        public async Task<LeaveRequestDTO> GetLeaveRequestById(long LeaveRequestID)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.GETBYID);
                    param.Add("@LeaveRequestIDGet", LeaveRequestID);

                    var LeaveDetails = await con.QueryFirstAsync<LeaveRequestDTO>(ApplicationConstant.Sp_LeaveRequest, param: param, commandType: CommandType.StoredProcedure);

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
   
        
        public async Task<dynamic> DeleteLeaveRequest(LeaveRequestDelete delete, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.DELETE);
                    param.Add("@LeaveRequestIDDelete", Convert.ToInt32(delete.LeaveRequestID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Delete", delete.Reasons_For_Delete == null ? "" : delete.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await conn.ExecuteAsync(ApplicationConstant.Sp_LeaveRequest, param: param, commandType: CommandType.StoredProcedure);

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
        public async Task<dynamic> RescheduleLeaveRequest(RescheduleLeaveRequest update, string requesterUserEmail)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.UPDATE);
                    param.Add("@LeaveRequestIDUpd", update.LeaveRequestID);
                    param.Add("@EmployeeId", update.EmployeeId);
                    param.Add("@RequestYearUpd", update.RequestYear);
                    param.Add("@LeaveTypeIdUpd", update.LeaveTypeId);
                    param.Add("@NoOfDaysUpd", update.NoOfDays);
                    param.Add("@StartDateUpd", update.StartDate);
                    param.Add("@EndDateUpd", update.EndDate);
                    param.Add("@ReliverUserIDUpd", update.ReliverUserID);
                    param.Add("@LeaveEvidenceUpd", update.LeaveEvidence.Trim());
                    param.Add("@NotesUpd", update.Notes.Trim());
                    param.Add("@ReasonForReschedulingUpd", update.ReasonForRescheduling.Trim());
                    param.Add("@CompanyIDUpd", update.CompanyID);

                    param.Add("@Updated_By_User_Email", requesterUserEmail.Trim());

                    dynamic response = await conn.ExecuteAsync(ApplicationConstant.Sp_LeaveRequest, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: RescheduleLeaveRequest(RescheduleLeaveRequest update) ===>{ex.Message}");
                throw;
            }
        }
        public async Task<IEnumerable<EmpLeaveRequestInfo>> GetAllLeaveRequest(string CompanyId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@CompanyId", CompanyId);

                    var LeaveDetails = await conn.QueryAsync<EmpLeaveRequestInfo>(ApplicationConstant.Sp_GetLeaveRequestByCompanyId, param: param, commandType: CommandType.StoredProcedure);
                    if (LeaveDetails.Count() > 0)
                    {
                        foreach (var item in LeaveDetails)
                        {
                            item.leaveRequestLineItems = await GetAllLeaveEmpRequestLineItems(item.LeaveRequestId);
                        }
                      
                    }
                    return LeaveDetails;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllLeaveRequest() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<LeaveRequestDTO>> GetLeaveRequestByUserId(long UserId, long CompanyId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.GETleaveRequestByUerId);
                    param.Add("@UserIdGet", UserId);
                    param.Add("@CompanyIdGet", CompanyId);


                    var LeaveDetails = await conn.QueryAsync<LeaveRequestDTO>(ApplicationConstant.Sp_LeaveRequest, param: param, commandType: CommandType.StoredProcedure);

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
        public async Task<LeaveRequestDTO> GetLeaveRequestByYear(string RequestYear, long CompanyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.GETBYEMAIL);
                    param.Add("@RequestYearGet", RequestYear);
                    param.Add("@CompanyIdGet", CompanyId);

                    var LeaveDetails = await _dapper.QueryFirstOrDefaultAsync<LeaveRequestDTO>(ApplicationConstant.Sp_LeaveRequest, param: param, commandType: CommandType.StoredProcedure);

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
        public async Task<IEnumerable<LeaveRequestDTO>> GetLeaveRequestByCompany(string RequestYear, long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.GETBYCOMPANYID);
                    param.Add("@RequestYear", RequestYear);
                    param.Add("@CompanyIdGet", companyId);

                    var LeaveDetails = await _dapper.QueryAsync<LeaveRequestDTO>(ApplicationConstant.Sp_LeaveRequest, param: param, commandType: CommandType.StoredProcedure);

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
        public async Task<IEnumerable<LeaveRequestDTO>> GetLeaveRequestPendingApproval(long UserIdGet)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", LeaveRequestEnum.GETPENDINGAPPROVAL);
                    param.Add("@UserIdGet", UserIdGet);

                    var userDetails = await _dapper.QueryAsync<LeaveRequestDTO>(ApplicationConstant.Sp_LeaveRequest, param: param, commandType: CommandType.StoredProcedure);

                    return userDetails;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetLeaveRequestPendingApproval() ===>{ex.Message}");
                throw;
            }
        }
        public async Task<LeaveRequestDTO> GetEmpLeaveRequest(long employeeId, string leavePeriod, string companyId = null)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@EmployeeId", employeeId);
                    param.Add("@CompanyId", companyId);
                    param.Add("@LeavePeriod", leavePeriod);

                    var userDetails = await _dapper.QueryFirstOrDefaultAsync<LeaveRequestDTO>(ApplicationConstant.sp_GetLeaveRequest, param: param, commandType: CommandType.StoredProcedure);

                    return userDetails;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetLeaveRequestPendingApproval() ===>{ex.Message}");
                throw;
            }
        }      
        public async Task<EmpLeaveRequestInfo> UpdateLeaveRequestInfoStatus(EmpLeaveRequestInfo empLeaveRequestInfo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@LeaveRequestId", empLeaveRequestInfo.LeaveRequestId);
                param.Add("@LeaveStatus", empLeaveRequestInfo.LeaveStatus);
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _dapperGeneric.Get<EmpLeaveRequestInfo>(ApplicationConstant.Sp_UpdateLeaveRequestInfoStatus, param, commandType: CommandType.StoredProcedure);
                if (res != null)
                {
                    _logger.LogInformation($"Successfully update LeaveRequest status. Response: {JsonConvert.SerializeObject(res)}");
                    return res;
                }

                _logger.LogError($"Could not update leaverequest status");
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: UpdateLeaveRequestInfoStatus ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }



        //public async Task<IEnumerable<LeaveRequestDTO>> GetUnitedHeadPendingApproval()
        //{
        //    try
        //    {
        //        using (SqlConnection _dapper = new SqlConnection(_connectionString))
        //        {
        //            var param = new DynamicParameters();
        //            param.Add("@Status", LeaveRequestEnum.GETUNITHEADAPPROVAL);

        //            var userDetails = await _dapper.QueryAsync<LeaveRequestDTO>(ApplicationConstant.Sp_LeaveRequest, param: param, commandType: CommandType.StoredProcedure);

        //            return userDetails;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = ex.Message;
        //        _logger.LogError($"MethodName: GetUnitedHeadPendingApproval() ===>{ex.Message}");
        //        throw;
        //    }
        //}

        //public async Task<IEnumerable<LeaveRequestDTO>> GetHODPendingApproval()
        //{
        //    try
        //    {
        //        using (SqlConnection _dapper = new SqlConnection(_connectionString))
        //        {
        //            var param = new DynamicParameters();
        //            param.Add("@Status", LeaveRequestEnum.GETHODAPPROVAL);

        //            var userDetails = await _dapper.QueryAsync<LeaveRequestDTO>(ApplicationConstant.Sp_LeaveRequest, param: param, commandType: CommandType.StoredProcedure);

        //            return userDetails;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = ex.Message;
        //        _logger.LogError($"MethodName: GetHODPendingApproval() ===>{ex.Message}");
        //        throw;
        //    }
        //}


        //public async Task<IEnumerable<LeaveRequestDTO>> GetHRPendingApproval()
        //{
        //    try
        //    {
        //        using (SqlConnection _dapper = new SqlConnection(_connectionString))
        //        {
        //            var param = new DynamicParameters();
        //            param.Add("@Status", LeaveRequestEnum.GETHRAPPROVAL);

        //            var userDetails = await _dapper.QueryAsync<LeaveRequestDTO>(ApplicationConstant.Sp_LeaveRequest, param: param, commandType: CommandType.StoredProcedure);

        //            return userDetails;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = ex.Message;
        //        _logger.LogError($"MethodName: GetHRPendingApproval() ===>{ex.Message}");
        //        throw;
        //    }
        //}
    }
}
