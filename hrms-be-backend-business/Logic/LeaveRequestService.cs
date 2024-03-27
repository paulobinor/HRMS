using Dapper;
using hrms_be_backend_business.AppCode;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;
using System.Text;

namespace hrms_be_backend_business.Logic
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IAuditLog _audit;
        private readonly ILogger<LeaveRequestService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMailService _mailService;

        public LeaveRequestService(IAccountRepository accountRepository, ILogger<LeaveRequestService> logger,
            ILeaveRequestRepository leaveRequestRepository, IAuditLog audit, ICompanyRepository companyrepository, IMailService mailService)
        {
            _audit = audit;
            _mailService = mailService;
            _logger = logger;
            _accountRepository = accountRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _companyrepository = companyrepository;
        }

        public async Task<BaseResponse> CreateLeaveRequest(LeaveRequestLineItem leaveRequestLineItem)
        {
            //check if us
            _logger.LogInformation($"About to create leave for EmployeId: {leaveRequestLineItem.EmployeeId} and CompanyId: {leaveRequestLineItem.CompanyId}");
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            EmpLeaveRequestInfo empLeaveRequestInfo = null;
            try
            {
                //if (string.IsNullOrEmpty(leaveRequestLineItem.HandoverNotes))
                //{
                //    response.ResponseCode = "08";
                //    response.ResponseMessage = "Note is required"; return response;
                //}
                //if (leaveRequestLineItem.RequestYear < 1)
                //{
                //    response.ResponseCode = "08";
                //    response.ResponseMessage = "Request Year is required";
                //    return response;
                //}
                empLeaveRequestInfo = (await _leaveRequestRepository.GetEmpLeaveInfo(leaveRequestLineItem.EmployeeId, "Active")).FirstOrDefault();
                if (empLeaveRequestInfo == null)
                {
                    empLeaveRequestInfo = await _leaveRequestRepository.CreateEmpLeaveInfo(leaveRequestLineItem.EmployeeId);
                    if (empLeaveRequestInfo == null)
                    {
                        throw new Exception("Could not create leave request");
                    }
                }


                //Check maximum split count

                //Check number of days left

                //check if any pending leave approvals

                leaveRequestLineItem.LeaveRequestId = empLeaveRequestInfo.LeaveRequestId;
                var res = await _leaveRequestRepository.CreateLeaveRequestLineItem(leaveRequestLineItem);

                //var repoResponse = await _leaveRequestRepository.CreateLeaveRequest(payload);
                //if (!repoResponse.Contains("Success"))
                //{
                //    response.ResponseCode = "08";
                //    response.ResponseMessage = repoResponse;
                //    return response;
                //}
                //  var userDetails = await _employeeRepository.GetEmployeeById(leaveRequestLineItem.EmployeeId, leaveRequestLineItem.CompanyId);

                //Send mail to reliever
               // _mailService.SendLeaveMailToReliever(leaveRequestLineItem.RelieverUserId, leaveRequestLineItem.EmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);

                //Send mail to approval
                //if (userDetails.Employee.UnitHeadEmployeeId == null)
                //{
                //    _mailService.SendLeaveApproveMailToApprover(userDetails.Employee.HodEmployeeId, leaveRequestLineItem.EmployeeId, leaveRequestLineItem.StartDate, payload.EndDate);
                //}
                //else
                //{
                //    _mailService.SendLeaveApproveMailToApprover(userDetails.Employee.UnitHeadEmployeeId, payload.EmployeeId, payload.StartDate, payload.EndDate);
                //}


                response.Data = res;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "leaveRequest created successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Exception occured Contact Admin";
                response.Data = null;



                return response;
            }
        }
        public async Task<List<EmpLeaveRequestInfo>> GetEmpLeaveInfo(long employeeId, string LeaveStatus, string companyId = null)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", employeeId);
                if (!string.IsNullOrEmpty(LeaveStatus))
                {
                    param.Add("@LeaveStatus", LeaveStatus);
                }
                //param.Add("@LeavePeriod", LeavePeriod);

                var res = await _leaveRequestRepository.GetEmpLeaveInfo(employeeId, LeaveStatus, companyId);
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
        public async Task<LeaveRequestLineItem> GetLeaveRequestLineItem(long leaveRequestLineItemId)
        {
            try
            {
                var leaveRequestLineItem = await _leaveRequestRepository.GetLeaveRequestLineItem(leaveRequestLineItemId);
                return leaveRequestLineItem;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<BaseResponse> RescheduleLeaveRequest(LeaveRequestLineItem leaveRequestLineItem)
        {
            var response = new BaseResponse();
            try
            {

                var empLeaveRequestInfo = await _leaveRequestRepository.GetEmpLeaveInfo(leaveRequestLineItem.EmployeeId, "Active");
                if (empLeaveRequestInfo == null)
                {
                    return new BaseResponse { ResponseCode = "404", ResponseMessage = "No record found" };
                }
                var lineItem = await _leaveRequestRepository.RescheduleLeaveRequest(leaveRequestLineItem);
                response.Data = lineItem;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Reschedule Leave was successful.";
                return response;

                //response.ResponseCode = ResponseCode.Exception.ToString();
                //response.ResponseMessage = "An error occurred while updating Reschedule Leave Request.";
                //response.Data = null;
                //return response;
            }


            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: RescheduleLeaveRequest ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: RescheduleLeaveRequest ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }    
        public async Task<BaseResponse> ApproveLeaveRequest(LeaveApprovalLineItem leaveApprovalLineItem)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            bool sendMail = false;
            var response = new BaseResponse();
            try
            {
                var repoResponse = await _leaveRequestRepository.UpdateLeaveApprovalLineItem(leaveApprovalLineItem);
                if (repoResponse == null)
                {
                    response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                    response.ResponseMessage = "an error occured while processing your request. Please contact your administrator for further assistance";
                    response.Data = repoResponse;
                    return response;
                }
                //var approvalStep = repoResponse.ApprovalStep += 1;

                //if (nextApprovalLineItem == null)
                //{

                //}
                LeaveApprovalLineItem nextApprovalLineItem = null;
                var currentLeaveApprovalInfo = await _leaveRequestRepository.GetLeaveApprovalInfo(repoResponse.LeaveApprovalId);
                if (currentLeaveApprovalInfo == null)
                {
                    response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                    response.ResponseMessage = "an error occured while processing your request. Please contact your administrator for further assistance";
                    response.Data = repoResponse;
                    return response;
                }

                if (repoResponse.IsApproved)
                {
                    if (currentLeaveApprovalInfo.RequiredApprovalCount == repoResponse.ApprovalStep)
                    {
                        currentLeaveApprovalInfo.ApprovalStatus = "Completed";
                    }

                    if (currentLeaveApprovalInfo.RequiredApprovalCount < repoResponse.ApprovalStep)
                    {
                         repoResponse.ApprovalStep +=  1;
                        currentLeaveApprovalInfo.ApprovalStatus = $"Pending on Approval level: {repoResponse.ApprovalStep}";

                        nextApprovalLineItem = await _leaveRequestRepository.GetLeaveApprovalLineItem(repoResponse.LeaveApprovalLineItemId, repoResponse.ApprovalStep);
                        sendMail = true;
                    }
                    
                    var updateLeaveApproval = await _leaveRequestRepository.UpdateLeaveApprovalInfo(currentLeaveApprovalInfo);

                    if (sendMail)
                    {
                        var leaveRequestLineItem = await _leaveRequestRepository.GetLeaveRequestLineItem(currentLeaveApprovalInfo.LeaveRequestLineItemId);

                        var userDetails = await _accountRepository.FindUser(leaveRequestLineItem.EmployeeId);
                        ////Send email to next level approver
                        _mailService.SendLeaveApproveMailToApprover(nextApprovalLineItem.ApprovalEmployeeId, leaveRequestLineItem.EmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);
                    }
                }
                else if(!repoResponse.IsApproved)
                {
                    currentLeaveApprovalInfo.ApprovalStatus = "Completed";
                    currentLeaveApprovalInfo.ApprovalStatus = $"Denied on Approval level: {repoResponse.ApprovalStep}";
                    currentLeaveApprovalInfo.Comments = repoResponse.Comments;


                    var leaveRequestLineItem = await _leaveRequestRepository.GetLeaveRequestLineItem(currentLeaveApprovalInfo.LeaveRequestLineItemId);

                    _mailService.SendLeaveDisapproveConfirmationMail(leaveRequestLineItem.EmployeeId, repoResponse.ApprovalEmployeeId);
                }
               
                
                response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                response.ResponseMessage = ResponseCode.Ok.ToString();
                response.Data = repoResponse;
                return response;


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Exception occured";
                response.Data = null;



                return response;
            }
        }
        public async Task<LeaveApprovalInfo> GetLeaveApprovalInfo(long leaveApprovalId, long leaveReqestLineItemId)
        {
            LeaveApprovalInfo leaveApproval = null;
            try
            {
                if (leaveApprovalId != 0)
                {
                    leaveApproval = await _leaveRequestRepository.GetLeaveApprovalInfo(leaveApprovalId);
                }
                else if (leaveReqestLineItemId != 0)
                {
                    leaveApproval = await _leaveRequestRepository.GetLeaveApprovalInfoByRequestLineItem(leaveReqestLineItemId);
                }
                return leaveApproval;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<LeaveApprovalInfo> GetLeaveApprovalByLineItem(long leaveRequestLineitemId)
        {
            try
            {
                var leaveApproval = await _leaveRequestRepository.GetLeaveApprovalInfoByRequestLineItem(leaveRequestLineitemId);
                return leaveApproval;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<LeaveApprovalInfo> UpdateLeaveApprovalInfo(LeaveApprovalInfo leaveApproval)
        {
            try
            {
                var UpdateLeaveApproval = await _leaveRequestRepository.UpdateLeaveApprovalInfo(leaveApproval);
                return UpdateLeaveApproval;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LeaveApprovalLineItem> GetLeaveApprovalLineItem(long leaveApprovalLineItemId, int approvalStep = 0)
        {
            try
            {
                var leaveApprovalLineitem = await _leaveRequestRepository.GetLeaveApprovalLineItem(leaveApprovalLineItemId, approvalStep);
                return leaveApprovalLineitem;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LeaveApprovalInfo> GetLeaveApprovalInfoByRequestLineItem(long leaveRequestLineItemId)
        {
            try
            {
                var LeaveApprovalInfo = await _leaveRequestRepository.GetLeaveApprovalInfoByRequestLineItem(leaveRequestLineItemId);
                return LeaveApprovalInfo;
            }
            catch (Exception)
            {
                throw;
            }
        }






        #region Depricated
        public async Task<BaseResponse> DisaproveLeaveRequest(LeaveRequestDisapproved payload, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {


                var repoResponse = await _leaveRequestRepository.DisaproveLeaveRequest(payload.LeaveRequestID, requester.UserId, payload.Reasons_For_Disapprove);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                var leaveRequestDetail = await _leaveRequestRepository.GetLeaveRequestById(payload.LeaveRequestID);

                var userDetails = await _employeeRepository.GetEmployeeByUserId(leaveRequestDetail.UserId);

                //Send mail to reliever
                _mailService.SendLeaveApproveConfirmationMail(leaveRequestDetail.UserId, requester.UserId, leaveRequestDetail.StartDate, leaveRequestDetail.EndDate);

                //Send mail to approval
                if (!leaveRequestDetail.IsHodApproved)
                {
                    _mailService.SendLeaveApproveMailToApprover(userDetails.HodEmployeeId, leaveRequestDetail.UserId, leaveRequestDetail.StartDate, leaveRequestDetail.EndDate);
                }

                response.ResponseCode = "00";
                response.ResponseMessage = "Record inserted successfully";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Exception occured";
                response.Data = null;



                return response;
            }
        }
        public async Task<BaseResponse> GetAllLeaveRquest(RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(null, requesterUserEmail, null);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                //if (Convert.ToInt32(RoleId) != 2)
                //{
                //    if (Convert.ToInt32(RoleId) != 4)
                //    {
                //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //        return response;

                //    }

                //}

                //update action performed into audit log here

                var leave = await _leaveRequestRepository.GetAllLeaveRequest();

                if (leave.Any())
                {
                    response.Data = leave;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "LeaveRequest fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No LeaveRequest found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: LeaveRequest() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: LeaveRequest() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
        public async Task<BaseResponse> GetLeaveRequsetById(long LeaveRequestID, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(null, requesterUserEmail, null);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }


                //if (Convert.ToInt32(RoleId) != 2)
                //{
                //    if (Convert.ToInt32(RoleId) != 4)
                //    {
                //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //        return response;

                //    }

                //}

                var LeaveRequest = await _leaveRequestRepository.GetLeaveRequestById(LeaveRequestID);

                if (LeaveRequest == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "LeaveRequest not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = LeaveRequest;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "LeaveRequest fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetLeaveRequestById(long LeaveRequestID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured:  GetLeaveRequestById(long LeaveRequestID  ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
        public async Task<BaseResponse> GetLeaveRequsetByUerId(long UserId, long CompanyId, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                // var ipAddress = requester.IpAddress;
                // var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(null, requesterUserEmail, null);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }



                var LeaveRequest = await _leaveRequestRepository.GetLeaveRequestByUserId(UserId, CompanyId);

                if (LeaveRequest == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "User LeaveRequest not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = LeaveRequest;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "User LeaveRequest fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetLeaveRequestByUserId(long UserId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured:  GetLeaveRequestByUserId(long UserId  ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
        public async Task<BaseResponse> GetLeaveRquestbyCompanyId(string RequestYear, long companyId, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(null, requesterUserEmail, null);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }



                var LeaveRquest = await _leaveRequestRepository.GetLeaveRequestByCompany(RequestYear, companyId);

                if (LeaveRquest == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "LeaveRequest not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = LeaveRquest;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "LeaveRequest fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetLeaveRequestByCompanyID(string RequestYear,long companyId) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetLeaveRequestByCompanyID(string RequestYear, long companyId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
        public async Task<BaseResponse> GetLeaveRequestPendingApproval(RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(null, requesterUserEmail, null);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                var leave = await _leaveRequestRepository.GetLeaveRequestPendingApproval(requester.UserId);

                if (leave.Any())
                {
                    response.Data = leave;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "LeaveRequest fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No record found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetLeaveRequestPendingApproval() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetLeaveRequestPendingApproval() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
        public async Task<BaseResponse> CreateLeaveRequest(LeaveRequestCreate payload, RequesterInfo requester)
        {
            //check if us

            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {
                if (string.IsNullOrEmpty(payload.Notes))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Note is required"; return response;
                }
                if (payload.RequestYear < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Request Year is required";
                    return response;
                }

                var repoResponse = await _leaveRequestRepository.CreateLeaveRequest(payload);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }
                var userDetails = await _employeeRepository.GetEmployeeById(payload.EmployeeId, payload.CompanyID);

                //Send mail to reliever
                _mailService.SendLeaveMailToReliever(payload.RelieverUserID, payload.EmployeeId, payload.StartDate, payload.EndDate);

                //Send mail to approval
                if (userDetails.Employee.UnitHeadEmployeeId == null)
                {
                    _mailService.SendLeaveApproveMailToApprover(userDetails.Employee.HodEmployeeId, payload.EmployeeId, payload.StartDate, payload.EndDate);
                }
                else
                {
                    _mailService.SendLeaveApproveMailToApprover(userDetails.Employee.UnitHeadEmployeeId, payload.EmployeeId, payload.StartDate, payload.EndDate);
                }


                response.Data = payload;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "leaveRequest created successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Exception occured Contact Admin";
                response.Data = null;



                return response;
            }
        }
        public async Task<BaseResponse> RescheduleLeaveRequest(RescheduleLeaveRequest updateDto, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(null, requesterUserEmail, null);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }


                //validate DepartmentDto payload here 
                if (/*String.IsNullOrEmpty(updateDto.) ||*/ updateDto.CompanyID <= 0)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Please ensure all required fields are entered.";
                    return response;
                }

                var LeaveRequest = await _leaveRequestRepository.GetLeaveRequestById(updateDto.LeaveRequestID);
                if (null == LeaveRequest)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "No record found for the specified LeaveRequest";
                    response.Data = null;
                    return response;
                }

                dynamic resp = await _leaveRequestRepository.RescheduleLeaveRequest(updateDto, requesterUserEmail);
                if (resp > 0)
                {
                    //update action performed into audit log here

                    var updatedLeaveType = await _leaveRequestRepository.GetLeaveRequestById(updateDto.LeaveRequestID);

                    _logger.LogInformation("Reschedule Leave Request updated successfully.");
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Reschedule Leave Request updated successfully.";
                    response.Data = updatedLeaveType;
                    return response;

                }

                var userDetails = await _employeeRepository.GetEmployeeById(updateDto.EmployeeId, updateDto.CompanyID);

                //Send mail to reliever
                _mailService.SendLeaveMailToReliever(updateDto.ReliverUserID, updateDto.EmployeeId, updateDto.StartDate, updateDto.EndDate);

                //Send mail to approval
                if (userDetails.Employee.UnitHeadEmployeeId == null)
                {
                    _mailService.SendLeaveApproveMailToApprover(userDetails.Employee.HodEmployeeId, updateDto.EmployeeId, updateDto.StartDate, updateDto.EndDate);
                }
                else
                {
                    _mailService.SendLeaveApproveMailToApprover(userDetails.Employee.UnitHeadEmployeeId, userDetails.Employee.EmployeeID, updateDto.StartDate, updateDto.EndDate);
                }

                response.Data = updateDto;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Reschedule Leave Request created successfully.";
                return response;

                //response.ResponseCode = ResponseCode.Exception.ToString();
                //response.ResponseMessage = "An error occurred while updating Reschedule Leave Request.";
                //response.Data = null;
                //return response;
            }


            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: RescheduleLeaveRequest ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: RescheduleLeaveRequest ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
        public async Task<BaseResponse> ApproveLeaveRequest(long LeaveRequestID, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {


                var repoResponse = await _leaveRequestRepository.ApproveLeaveRequest(LeaveRequestID, requester.UserId);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                var leaveRequestDetail = await _leaveRequestRepository.GetLeaveRequestById(LeaveRequestID);

                var userDetails = await _accountRepository.FindUser(leaveRequestDetail.UserId);

                //Send mail to reliever
                _mailService.SendLeaveApproveConfirmationMail(leaveRequestDetail.UserId, requester.UserId, leaveRequestDetail.StartDate, leaveRequestDetail.EndDate);

                //Send mail to approval
                if (leaveRequestDetail.UnitHeadUserID == null)
                {
                    _mailService.SendLeaveApproveMailToApprover(leaveRequestDetail.HRUserId, leaveRequestDetail.UserId, leaveRequestDetail.StartDate, leaveRequestDetail.EndDate);
                }
                else
                {
                    if (!leaveRequestDetail.IsHodApproved)
                    {
                        _mailService.SendLeaveApproveMailToApprover(leaveRequestDetail.HodUserID, leaveRequestDetail.UserId, leaveRequestDetail.StartDate, leaveRequestDetail.EndDate);
                    }
                    else
                    {
                        _mailService.SendLeaveApproveMailToApprover(leaveRequestDetail.HRUserId, leaveRequestDetail.UserId, leaveRequestDetail.StartDate, leaveRequestDetail.EndDate);
                    }

                }


                response.ResponseCode = "00";
                response.ResponseMessage = "Record inserted successfully";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Exception occured";
                response.Data = null;



                return response;
            }
        }

       
        #endregion
    }
}
