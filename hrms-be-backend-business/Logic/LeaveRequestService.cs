using hrms_be_backend_business.AppCode;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        private int CountWeekdays(DateTime startDate, DateTime endDate)
        {
            int count = 0;
            TimeSpan span = endDate - startDate;
            int days = span.Days + 1; // Including both start and end dates

            for (int i = 0; i < days; i++)
            {
                DateTime currentDate = startDate.AddDays(i);
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    count++;
                }
            }

            return count;
        }
        private bool IsValidWeekeday(DateTime dateTime)
        {
            if(  dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                return false;
            }
            return true;
        }
        public async Task<BaseResponse> CreateLeaveRequestLineItem(LeaveRequestLineItem leaveRequestLineItem)
        {

            _logger.LogInformation($"About to create leave request for EmployeeId: {leaveRequestLineItem.EmployeeId} and CompanyId: {leaveRequestLineItem.CompanyId}");
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            GradeLeave gradeLeave = null;
            int noOfDaysTaken = 0;

            #region Validate Leave Request
            //check if any pending leave approvals
            var leaveAproval = await _leaveRequestRepository.GetLeaveApprovalInfoByEmployeeId(leaveRequestLineItem.EmployeeId);
            if (leaveAproval != null)
            {
                _logger.LogInformation($"There is already a pending leave approval for EmployeeId: {leaveRequestLineItem.EmployeeId} and CompanyId: {leaveRequestLineItem.CompanyId}, payload: {JsonConvert.SerializeObject(leaveAproval)}");
                response.ResponseCode = "08";
                response.ResponseMessage = "pending leave detected";
                response.Data = leaveAproval;
                return response;
            }

            if (!IsValidWeekeday(leaveRequestLineItem.startDate) || !IsValidWeekeday(leaveRequestLineItem.endDate))
            {
                _logger.LogWarning($"Invalid startdate specified. {leaveRequestLineItem.startDate.DayOfWeek} does not fall within a weekday");
                response.ResponseCode = "08";
                response.ResponseMessage = $"Invalid startdate specified. {leaveRequestLineItem.startDate.DayOfWeek} does not fall within a weekday";
                return response;
            }
            if (!IsValidWeekeday(leaveRequestLineItem.endDate))
            {
                _logger.LogWarning($"Invalid endDate specified. {leaveRequestLineItem.endDate.DayOfWeek} does not fall within a weekday");
                response.ResponseCode = "08";
                response.ResponseMessage = $"Invalid endDate specified. {leaveRequestLineItem.endDate.DayOfWeek} does not fall within a weekday";
                return response;
            }

            //startdate must be less than end date
            if (leaveRequestLineItem.startDate > leaveRequestLineItem.endDate)
            {
                _logger.LogError("Invalid date range specified. start date must come before the end date");
                response.ResponseCode = "08";
                response.ResponseMessage = "Invalid date range specified. start date must come before the the end date";
                return response;
            }

            //You cannot select a date in the past
            if (DateTime.Now.Date > leaveRequestLineItem.startDate)
            {
                _logger.LogError("Invalid date range specified. You cannot select a date in the past");
                response.ResponseCode = "08";
                response.ResponseMessage = $"Invalid date range specified. You cannot select a date in the past. Selected date {leaveRequestLineItem.startDate}, Current date {DateTime.Now}";
                return response;
            }

            //resumption date must be greater or equal to end date
            if (leaveRequestLineItem.ResumptionDate < leaveRequestLineItem.endDate)
            {
                _logger.LogError("Invalid resumption date specified.");
                response.ResponseCode = "08";
                response.ResponseMessage = "Invalid resumption specified.";
                return response;
            }

            //Validate leave length
            int totaldays = CountWeekdays(leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);

            if (totaldays != leaveRequestLineItem.LeaveLength)
            {
                _logger.LogError($"Invalid leave length specified! The leave length is {leaveRequestLineItem.LeaveLength} but the total weekdays between {leaveRequestLineItem.startDate.ToShortDateString()} and {leaveRequestLineItem.endDate.ToShortDateString()} is {totaldays}");
                response.ResponseCode = "08";
                response.ResponseMessage = $"Invalid leave length specified! The leave length is {leaveRequestLineItem.LeaveLength} but the total weekdays between {leaveRequestLineItem.startDate.ToShortDateString()} and {leaveRequestLineItem.endDate.ToShortDateString()} is {totaldays}";
                return response;
            }

            #endregion


            EmpLeaveRequestInfo empLeaveRequestInfo = null;
            try
            {
                empLeaveRequestInfo = await _leaveRequestRepository.GetEmpLeaveInfo(leaveRequestLineItem.EmployeeId, leaveRequestLineItem.EmployeeId);
                if (empLeaveRequestInfo == null)
                {
                    empLeaveRequestInfo = await _leaveRequestRepository.CreateEmpLeaveInfo(leaveRequestLineItem.EmployeeId);
                    if (empLeaveRequestInfo == null)
                    {
                        _logger.LogError($"Could not create leave request in database.");
                        response.ResponseCode = "08";
                        response.ResponseMessage = $"Could not create leave request. Please try again later of contact support for assistance";
                        return response;
                        //throw new Exception("Could not create leave request");
                    }    
                }

                var leaveRequestLineItems = await _leaveRequestRepository.GetLeaveRequestLineItems(empLeaveRequestInfo.LeaveRequestId);
                if (leaveRequestLineItems != null)
                {
                    if (leaveRequestLineItems.Count > 0)
                    {
                        gradeLeave = await _leaveRequestRepository.GetEmployeeGradeLeave(leaveRequestLineItem.EmployeeId);

                        //Check number of days left
                        //Only sum up the days of the items that were approved
                        noOfDaysTaken = leaveRequestLineItems.Where(x => x.IsApproved == true).Sum(x => x.LeaveLength); 

                        if ((noOfDaysTaken + leaveRequestLineItem.LeaveLength) > gradeLeave.NumbersOfDays)
                        {
                            response.ResponseCode = "08";
                            response.ResponseMessage = "Leave length exceeded";
                            return response;
                        }

                        //Check split count
                        //Only count the items that were approved
                        //include proposed leave (+1)
                        var noOfApprovedSplit = leaveRequestLineItems.Where(x => x.IsApproved == true).Count();
                        if ((noOfApprovedSplit + 1) > gradeLeave.NumberOfVacationSplit ) 
                        {
                            response.ResponseCode = "08";
                            response.ResponseMessage = "Vacation split count exceeded";
                            return response;
                        }
                    }
                }
                
                leaveRequestLineItem.LeaveRequestId = empLeaveRequestInfo.LeaveRequestId;
                var res = await _leaveRequestRepository.CreateLeaveRequestLineItem(leaveRequestLineItem);
                if (res != null)
                {
                    //Update active leave info for employee if maximum days or split count reached.

                    //if ((gradeLeave.NumbersOfDays == (noOfDaysTaken + leaveRequestLineItem.LeaveLength)) ||
                    //    gradeLeave.NumberOfVacationSplit == (leaveRequestLineItems.Count + 1))
                    //{
                    //    empLeaveRequestInfo.LeaveStatus = "Completed";
                    //    _leaveRequestRepository.UpdateLeaveRequestInfoStatus(empLeaveRequestInfo);
                    //}

                }

                var currentLeaveApprovalInfo = await _leaveRequestRepository.GetLeaveApprovalInfoByEmployeeId(leaveRequestLineItem.EmployeeId);
                if (currentLeaveApprovalInfo == null)
                {
                    response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                    response.ResponseMessage = "an error occured while processing your request. Please contact your administrator for further assistance";
                  //  response.Data = repoResponse;
                    return response;
                }


                currentLeaveApprovalInfo.ApprovalStatus = $"Pending on Approval count: {1}";

                var nextApprovalLineItem = (await _leaveRequestRepository.GetLeaveApprovalLineItems(currentLeaveApprovalInfo.LeaveApprovalId)).FirstOrDefault(x=>x.ApprovalStep == 1);
                if (nextApprovalLineItem == null) 
                {
                    response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                    response.ResponseMessage = "an error occured while processing your request. Please contact your administrator for further assistance";
                    //  response.Data = repoResponse;
                    return response;
                }

                _mailService.SendLeaveApproveMailToApprover(nextApprovalLineItem.ApprovalEmployeeId, leaveRequestLineItem.EmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);

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
        public async Task<EmpLeaveRequestInfo> GetEmpLeaveInfo(long employeeId, long companyId, string LeaveStatus)
        {
            try
            {
                //var param = new DynamicParameters();
                //param.Add("@EmployeeId", employeeId);
                //if (!string.IsNullOrEmpty(LeaveStatus))
                //{
                //    param.Add("@LeaveStatus", LeaveStatus);
                //}
                ////param.Add("@LeavePeriod", LeavePeriod);

                var res = await _leaveRequestRepository.GetEmpLeaveInfo(employeeId, companyId, LeaveStatus);
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

                var empLeaveRequestInfo = await _leaveRequestRepository.GetEmpLeaveInfo(leaveRequestLineItem.EmployeeId, leaveRequestLineItem.CompanyId);
                if (empLeaveRequestInfo == null)
                {
                    return new BaseResponse { ResponseCode = "404", ResponseMessage = "No record found" };
                }

                //Check that leave start date is not in the past
                if (leaveRequestLineItem.startDate.Date < DateTime.Today.Date)
                {
                    return new BaseResponse { ResponseCode = "400", ResponseMessage = "Leave start date cannot be in the past" };
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
        public async Task<BaseResponse> UpdateLeaveApproveLineItem(LeaveApprovalLineItem leaveApprovalLineItem)
        {
            StringBuilder errorOutput = new StringBuilder();
            bool sendMail = false;
            bool sendMailToReliever = false;
            var response = new BaseResponse();
            LeaveApprovalLineItem nextApprovalLineItem = null;
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
               
                var currentLeaveApprovalInfo = await _leaveRequestRepository.GetLeaveApprovalInfo(repoResponse.LeaveApprovalId);
                var leaveRequestLineItem = await _leaveRequestRepository.GetLeaveRequestLineItem(currentLeaveApprovalInfo.LeaveRequestLineItemId);
                if (currentLeaveApprovalInfo == null)
                {
                    response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                    response.ResponseMessage = "an error occured while processing your request. Please contact your administrator for further assistance";
                    response.Data = repoResponse;
                    return response;
                }

                if (repoResponse.IsApproved)
                {
                    if (currentLeaveApprovalInfo.RequiredApprovalCount == repoResponse.ApprovalStep) //all approvals is complete
                    {
                       
                        currentLeaveApprovalInfo.ApprovalStatus = "Completed";
                        leaveRequestLineItem.IsApproved = true;
                        sendMailToReliever = true;

                        //Update active leave info for employee if maximum days or split count reached.

                        var empLeaveRequestInfo = await _leaveRequestRepository.GetEmpLeaveInfo(leaveRequestLineItem.EmployeeId, leaveRequestLineItem.EmployeeId);
                        var gradeLeave = await _leaveRequestRepository.GetEmployeeGradeLeave(leaveRequestLineItem.EmployeeId);
                        var leaveRequestLineItems = await _leaveRequestRepository.GetLeaveRequestLineItems(empLeaveRequestInfo.LeaveRequestId);
                        int noOfDaysTaken = leaveRequestLineItems.Sum(x => x.LeaveLength);
                        if (gradeLeave.NumbersOfDays == noOfDaysTaken || gradeLeave.NumberOfVacationSplit == leaveRequestLineItems.Count())
                        {
                            empLeaveRequestInfo.LeaveStatus = "Completed";
                            _leaveRequestRepository.UpdateLeaveRequestInfoStatus(empLeaveRequestInfo);
                        }

                        //update Leaverequestlineitem
                        _leaveRequestRepository.UpdateLeaveRequestLineItemApproval(leaveRequestLineItem);
                    }

                    if (currentLeaveApprovalInfo.RequiredApprovalCount > repoResponse.ApprovalStep)
                    {
                         repoResponse.ApprovalStep +=  1;
                        currentLeaveApprovalInfo.ApprovalStatus = $"Pending on Approval count: {repoResponse.ApprovalStep}";

                        nextApprovalLineItem = await _leaveRequestRepository.GetLeaveApprovalLineItem(repoResponse.LeaveApprovalLineItemId, repoResponse.ApprovalStep);
                        sendMail = true;
                    }
                    
                    var updateLeaveApproval = await _leaveRequestRepository.UpdateLeaveApprovalInfo(currentLeaveApprovalInfo);

                    if (sendMail)
                    {
                        //Send mail to next approver
                        _mailService.SendLeaveApproveMailToApprover(nextApprovalLineItem.ApprovalEmployeeId, leaveRequestLineItem.EmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);
                    }

                    if (sendMailToReliever)
                    {
                        //Send mail to reliever
                         _mailService.SendLeaveMailToReliever(leaveRequestLineItem.RelieverUserId, leaveRequestLineItem.EmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);
                    }
                }
                else if(!repoResponse.IsApproved) // Leave approval is denied
                {
                    currentLeaveApprovalInfo.ApprovalStatus = "Completed";
                   // currentLeaveApprovalInfo.ApprovalStatus = $"Denied on Approval count: {repoResponse.ApprovalStep}";
                    currentLeaveApprovalInfo.Comments = repoResponse.Comments;

                    // leaveRequestLineItem = await _leaveRequestRepository.GetLeaveRequestLineItem(currentLeaveApprovalInfo.LeaveRequestLineItemId);

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
                if (leaveApprovalId > 0)
                {
                    leaveApproval = await _leaveRequestRepository.GetLeaveApprovalInfo(leaveApprovalId);
                }
                else if (leaveReqestLineItemId > 0)
                {
                    leaveApproval = await _leaveRequestRepository.GetLeaveApprovalInfoByRequestLineItem(leaveReqestLineItemId);
                }
                if (leaveApproval != null) {

                    leaveApproval.LeaveApprovalLineItems = await GetleaveApprovalLineItems(leaveApproval.LeaveApprovalId);
                    
                }
                return leaveApproval;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<LeaveApprovalLineItem>> GetleaveApprovalLineItems(long leaveApprovalId)
        {
            try
            {
                var leaveApproval = await _leaveRequestRepository.GetLeaveApprovalLineItems(leaveApprovalId);

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
