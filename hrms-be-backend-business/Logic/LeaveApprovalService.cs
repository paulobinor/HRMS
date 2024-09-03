using GTB.Common;
using hrms_be_backend_business.AppCode;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.DTO;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics.Metrics;
using System.Text;

namespace hrms_be_backend_business.Logic
{
    public class LeaveApprovalService : ILeaveApprovalService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<LeaveApprovalService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly ILeaveApprovalRepository _leaveApprovalRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMailService _mailService;
        private readonly ILeaveTypeRepository _leaveTypeService;
        private bool sendApprovalMailToInitiator;

        public LeaveApprovalService(IAccountRepository accountRepository, ILogger<LeaveApprovalService> logger,
            ILeaveApprovalRepository leaveApprovalRepository, IAuditLog audit, ICompanyRepository companyrepository, IMailService mailService, ILeaveRequestRepository leaveRequestRepository, ILeaveTypeRepository leaveTypeService)
        {
            _audit = audit;
            _mailService = mailService;
            _logger = logger;
            _accountRepository = accountRepository;
            _leaveApprovalRepository = leaveApprovalRepository;
            _companyrepository = companyrepository;
            _leaveRequestRepository = leaveRequestRepository;
            _leaveTypeService = leaveTypeService;
        }

        public async Task<Approvals> CreateApproval(Approvals approvals)
        {
            var response = new Approvals();

            try
            {
                response = await _leaveApprovalRepository.CreateApproval(approvals);               
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured ==> {ex.Message}");
               // response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
               // response.ResponseMessage = "Exception occured";
               // response.Data = null;

            }
            return response;
        }
        public async Task<BaseResponse> UpdateLeaveApproveLineItem(LeaveApprovalLineItem leaveApprovalLineItem)
        {
            
            StringBuilder errorOutput = new StringBuilder();
            var updateLeaveRequestLineItem = new LeaveRequestLineItem();
            bool sendMailToApprover = false;
            bool sendMailToReliever = false;
            var response = new BaseResponse();
            LeaveApprovalLineItem nextApprovalLineItem = null;
            bool isEmilokan = false;
            try
            {

                var leaveApproval = await GetLeaveApprovalInfo(leaveApprovalLineItem.LeaveApprovalId, 0); 
                if (leaveApproval == null )
                {
                    _logger.LogInformation($"No information was returned for the LeaveApprovalID:{leaveApprovalLineItem.LeaveApprovalId}");
                    response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                    response.ResponseMessage = $"No information was returned for the LeaveApprovalID:{leaveApprovalLineItem.LeaveApprovalId}";
                    return response;
                }

                if (leaveApproval.ApprovalKey == leaveApprovalLineItem.LeaveApprovalLineItemId)
                {
                    isEmilokan = true;
                }

                //var leaveApprovalLineItem1 = leaveApproval.LeaveApprovalLineItems.FirstOrDefault(x=>x.LeaveApprovalLineItemId == leaveApprovalLineItem.LeaveApprovalLineItemId);
                //if (leaveApproval.LastApprovalEmployeeID == leaveApprovalLineItem.ApprovalEmployeeId && leaveApproval.Comments.Equals(leaveApprovalLineItem1.Comments, StringComparison.OrdinalIgnoreCase))
                //{
                //    isEmilokan=true;
                //}

                if (!isEmilokan)
                {
                    _logger.LogInformation($"Approval mismatch. ApprovalKey provided: {leaveApprovalLineItem.LeaveApprovalLineItemId}, ApprovalKey returned: {leaveApproval.ApprovalKey}");

                    //Not your turn to approve at the moment
                    response.ResponseCode = "401";
                    response.ResponseMessage = "You cannot peform this action at this time";
                    response.Data = null;
                    return response;
                }

                var repoResponse = await _leaveApprovalRepository.UpdateLeaveApprovalLineItem(leaveApprovalLineItem);
                if (repoResponse == null)
                {
                    response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                    response.ResponseMessage = "an error occured while processing your request. Please contact your administrator for further assistance";
                    response.Data = repoResponse;
                    return response;
                }

                var currentLeaveApprovalInfo = await _leaveApprovalRepository.GetLeaveApprovalInfo(leaveApprovalLineItem.LeaveApprovalId);

                if (currentLeaveApprovalInfo == null)
                {
                    response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                    response.ResponseMessage = "an error occured while processing your request. Please contact your administrator for further assistance";
                    return response;
                }

                var leaveRequestLineItem = await _leaveRequestRepository.GetLeaveRequestLineItem(currentLeaveApprovalInfo.LeaveRequestLineItemId);
              
                if (leaveRequestLineItem == null)
                {
                    response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                    _logger.LogError($"Failed to get current LeaveRequestLine item while trying to procces the request for: {JsonConvert.SerializeObject(leaveApprovalLineItem)}");
                    response.ResponseMessage = "an error occured while processing current LeaveRequestLine request. Please contact your administrator for further assistance";
                    return response;
                }

                if (repoResponse.IsApproved || repoResponse.ApprovalStatus == "Approved")
                {
                    if (currentLeaveApprovalInfo.RequiredApprovalCount == currentLeaveApprovalInfo.CurrentApprovalCount) //all approvals is complete
                    {
                        currentLeaveApprovalInfo.ApprovalStatus = "Completed";
                        currentLeaveApprovalInfo.Comments = "Approved";
                        if (!string.IsNullOrEmpty(repoResponse.Comments))
                        {
                            currentLeaveApprovalInfo.Comments += "," + repoResponse.Comments;
                        }
                        currentLeaveApprovalInfo.IsApproved = true;
                        sendMailToReliever = true;
                        sendApprovalMailToInitiator = true;
                    }

                    if (currentLeaveApprovalInfo.RequiredApprovalCount > currentLeaveApprovalInfo.CurrentApprovalCount)
                    {
                        currentLeaveApprovalInfo.CurrentApprovalCount += 1;

                        // currentLeaveApprovalInfo.ApprovalStatus = $"Pending on Approval count: {repoResponse.ApprovalStep}";
                        nextApprovalLineItem = await _leaveApprovalRepository.GetLeaveApprovalLineItem(repoResponse.LeaveApprovalId, currentLeaveApprovalInfo.CurrentApprovalCount);
                        currentLeaveApprovalInfo.CurrentApprovalID = (int)nextApprovalLineItem.ApprovalEmployeeId;

                           currentLeaveApprovalInfo.Comments = $"Pending on {nextApprovalLineItem.ApprovalPosition}";
                           currentLeaveApprovalInfo.ApprovalKey = nextApprovalLineItem.LeaveApprovalLineItemId;
                           sendMailToApprover = true;
                    }

                    if (sendMailToApprover)
                    {
                        //Send mail to next approver
                        _mailService.SendLeaveApproveMailToApprover(nextApprovalLineItem.ApprovalEmployeeId, leaveRequestLineItem.EmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);
                        _mailService.SendLeaveApproveConfirmationMail(leaveRequestLineItem.EmployeeId, leaveApprovalLineItem.ApprovalEmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);

                       
                        //Notify Leave request Initiator of approval progress
                        var userDetails = await _accountRepository.GetUserByEmployeeId(leaveRequestLineItem.EmployeeId);
                        var app = await _accountRepository.GetUserByEmployeeId(leaveApprovalLineItem.ApprovalEmployeeId);
                        StringBuilder mailBody = new StringBuilder();

                        var leaveType = await _leaveTypeService.GetLeaveTypeById(leaveRequestLineItem.LeaveTypeId);
                        leaveType.LeaveTypeName = leaveType.LeaveTypeName.Replace("leave", "", StringComparison.OrdinalIgnoreCase);

                        mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName} <br/> <br/>");
                        mailBody.Append($"Kindly note that the next approval is currently pending on  {app.FirstName} {app.LastName} {leaveApprovalLineItem.ApprovalPosition},  <br/> <br/>");
                        mailBody.Append($"<b>Start Date : <b/> {leaveRequestLineItem.startDate}  <br/> ");
                        mailBody.Append($"<b>End Date : <b/> {leaveRequestLineItem.endDate}   <br/> ");

                        var mailPayload = new MailRequest
                        {
                            Body = mailBody.ToString(),
                            Subject = $"{leaveType.LeaveTypeName} leave Approval",
                            ToEmail = userDetails.OfficialMail,
                        };

                        _logger.LogError($"Email payload to send: {mailPayload}.");
                        _mailService.SendEmailAsync(mailPayload, null);

                    }

                    if (sendMailToReliever)
                    {
                        //Send mail to reliever
                        _mailService.SendLeaveMailToReliever(leaveRequestLineItem.RelieverUserId, leaveRequestLineItem.EmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);
                    }

                    if (sendApprovalMailToInitiator)
                    {
                        var userDetails = await _accountRepository.GetUserByEmployeeId(leaveRequestLineItem.EmployeeId);
                        //  var app = await _accountRepository.GetUserByEmployeeId(leaveApprovalLineItem.ApprovalEmployeeId);
                        StringBuilder mailBody = new StringBuilder();
                        var leaveType = await _leaveTypeService.GetLeaveTypeById(leaveRequestLineItem.LeaveTypeId);
                        leaveType.LeaveTypeName = leaveType.LeaveTypeName.Replace("leave", "", StringComparison.OrdinalIgnoreCase);

                        mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} <br/> <br/>");
                        mailBody.Append($"Kindly note that your request for {leaveType.LeaveTypeName} leave has successfully passed the final stage for approval. Enjoy your leave<br/> <br/>");
                        mailBody.Append($"<b>Start Date : <b/> {leaveRequestLineItem.startDate}  <br/> ");
                        mailBody.Append($"<b>End Date : <b/> {leaveRequestLineItem.endDate}   <br/> ");

                        var mailPayload = new MailRequest
                        {
                            Body = mailBody.ToString(),
                            Subject = $"{leaveType.LeaveTypeName} Leave Request",
                            ToEmail = userDetails.OfficialMail,
                        };

                        _logger.LogError($"Email payload to send: {mailPayload}.");
                        _mailService.SendEmailAsync(mailPayload);
                    }

                    response.ResponseMessage = $"Approved Successfully";
                }
                else if (!repoResponse.IsApproved || repoResponse.ApprovalStatus == "Disapproved") // Leave approval is denied
                {
                    currentLeaveApprovalInfo.ApprovalStatus = "Completed";
                    currentLeaveApprovalInfo.Comments = $"Disapproved by {repoResponse.ApprovalPosition}";
                    currentLeaveApprovalInfo.CurrentApprovalID = (int)leaveApprovalLineItem.ApprovalEmployeeId;
                    if (!string.IsNullOrEmpty(repoResponse.Comments))
                    {
                        currentLeaveApprovalInfo.Comments += "," + repoResponse.Comments;
                    }
                   
                    _mailService.SendLeaveDisapproveConfirmationMail(leaveRequestLineItem.EmployeeId, repoResponse.ApprovalEmployeeId);
                    response.ResponseMessage = "Disapproved Successfully";
                }
                _ = _leaveApprovalRepository.UpdateLeaveApprovalInfo(currentLeaveApprovalInfo);

                response.ResponseCode = ((int)ResponseCode.Ok).ToString();
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

        public async Task<BaseResponse> UpdateAnnualLeaveApproval(LeaveApprovalLineItem leaveApprovalLineItem)
        {
         
            try
            {
                
                _logger.LogInformation($"About to get leave approval details for update");


                int count1 = 0;
                var updateLineItem = new LeaveApprovalLineItem(); // info.LeaveApprovalLineItems.FirstOrDefault(x=>x.LeaveApprovalLineItemId == leaveApprovalLineItem.LeaveApprovalLineItemId);
                _logger.LogInformation($"Item to update: {JsonConvert.SerializeObject(leaveApprovalLineItem)}");
                updateLineItem.ApprovalStatus = leaveApprovalLineItem.ApprovalStatus;
                updateLineItem.IsApproved = leaveApprovalLineItem.IsApproved;
                updateLineItem.DateCompleted = DateTime.Now; // leaveApprovalLineItem.EntryDate;
                updateLineItem.Comments = leaveApprovalLineItem.Comments;
                if (updateLineItem != null)
                {
                    var res = await UpdateAnnualLeaveApproveLineItem(leaveApprovalLineItem);
                    return res;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured: {ex.Message}, Stack trace: {ex.StackTrace}");

                throw;
            }
        }
        private async Task<BaseResponse> UpdateAnnualLeaveApproveLineItem(LeaveApprovalLineItem leaveApprovalLineItem)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var updateLeaveRequestLineItem = new LeaveRequestLineItem();
            bool sendMailToApprover = false;
            bool sendMailToReliever = false;
            var response = new BaseResponse();
            LeaveApprovalLineItem nextApprovalLineItem = null;
            bool isEmilokan = false;
            try
            {

                var leaveApproval = await GetLeaveApprovalInfo(leaveApprovalLineItem.LeaveApprovalId, 0);
                if (leaveApproval == null)
                {
                    _logger.LogInformation($"No information was returned for the LeaveApprovalID:{leaveApprovalLineItem.LeaveApprovalId}");
                    response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                    response.ResponseMessage = $"No information was returned for the LeaveApprovalID:{leaveApprovalLineItem.LeaveApprovalId}";
                    // response.Data = repoResponse;
                    return response;
                }
                if (leaveApproval.ApprovalKey == leaveApprovalLineItem.LeaveApprovalLineItemId)
                {
                    isEmilokan = true;

                }
               
                //var leaveApprovalLineItem1 = leaveApproval.LeaveApprovalLineItems.FirstOrDefault(x => x.LeaveApprovalLineItemId == leaveApprovalLineItem.LeaveApprovalLineItemId);
               
                //if (leaveApproval.LastApprovalEmployeeID == leaveApprovalLineItem.ApprovalEmployeeId && leaveApproval.Comments.Equals(leaveApprovalLineItem1.Comments, StringComparison.OrdinalIgnoreCase))
                //{
                //    isEmilokan = true;
                //}

                if (!isEmilokan)
                {
                    //Not your turn to approve
                    response.ResponseCode = "401";
                    response.ResponseMessage = "You cannot peform this action at this time";
                    response.Data = null;
                    return response;
                }

                var repoResponse = await _leaveApprovalRepository.UpdateLeaveApprovalLineItem(leaveApprovalLineItem);
                if (repoResponse == null)
                {
                    response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                    response.ResponseMessage = "an error occured while processing your request. Please contact your administrator for further assistance";
                    response.Data = repoResponse;
                    return response;
                }

               // _ = ProcessLeaveApproval(repoResponse.LeaveApprovalId, repoResponse.IsApproved);
                //Get the eapproval information of the current leave request
                var currentLeaveApprovalInfo = await _leaveApprovalRepository.GetLeaveApprovalInfo(leaveApprovalLineItem.LeaveApprovalId);


                if (currentLeaveApprovalInfo == null)
                {
                    response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                    response.ResponseMessage = "an error occured while processing your request. Please contact your administrator for further assistance";
                    //  response.Data = repoResponse;
                    return response;
                }

                //Get the current request being approved/denied
                AnnualLeave annualLeave = await _leaveApprovalRepository.GetAnnualLeaveInfo(currentLeaveApprovalInfo.LeaveApprovalId);

                var leaveRequestLineItem = annualLeave.leaveRequestLineItems.FirstOrDefault(); // await _leaveRequestRepository.GetLeaveRequestLineItem(currentLeaveApprovalInfo.LeaveRequestLineItemId);

                if (leaveRequestLineItem == null)
                {
                    response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                    _logger.LogError($"Failed to get current LeaveRequestLine item while trying to procces the request for: {JsonConvert.SerializeObject(leaveApprovalLineItem)}");
                    response.ResponseMessage = "an error occured while processing current LeaveRequestLine request. Please contact your administrator for further assistance";
                    //  response.Data = repoResponse;
                    return response;
                }

                if (repoResponse.IsApproved || repoResponse.ApprovalStatus == "Approved")
                {
                    if (currentLeaveApprovalInfo.RequiredApprovalCount == currentLeaveApprovalInfo.CurrentApprovalCount) //all approvals is complete
                    {

                        currentLeaveApprovalInfo.ApprovalStatus = "Completed";
                        annualLeave.ApprovalStatus = "Completed";

                        currentLeaveApprovalInfo.Comments = "Approved";
                        annualLeave.Comments = "Approved";
                        if (!string.IsNullOrEmpty(repoResponse.Comments))
                        {
                            currentLeaveApprovalInfo.Comments += "," + repoResponse.Comments;
                            annualLeave.Comments += "," + repoResponse.Comments;

                        }
                        currentLeaveApprovalInfo.IsApproved = true;
                        //   leaveRequestLineItem.IsApproved = true;//update Leaverequestlineitem
                        sendMailToReliever = true;
                        sendApprovalMailToInitiator = true;
                    }

                    if (currentLeaveApprovalInfo.RequiredApprovalCount > currentLeaveApprovalInfo.CurrentApprovalCount)
                    {
                        currentLeaveApprovalInfo.CurrentApprovalCount += 1;

                        // currentLeaveApprovalInfo.ApprovalStatus = $"Pending on Approval count: {repoResponse.ApprovalStep}";
                        nextApprovalLineItem = await _leaveApprovalRepository.GetLeaveApprovalLineItem(repoResponse.LeaveApprovalId, currentLeaveApprovalInfo.CurrentApprovalCount);
                        currentLeaveApprovalInfo.CurrentApprovalID = (int)nextApprovalLineItem.ApprovalEmployeeId;

                        currentLeaveApprovalInfo.Comments = $"Pending on {nextApprovalLineItem.ApprovalPosition}";
                        currentLeaveApprovalInfo.ApprovalKey = nextApprovalLineItem.LeaveApprovalLineItemId;


                        annualLeave.Comments = $"Pending on {nextApprovalLineItem.ApprovalPosition}";
                        sendMailToApprover = true;
                    }

                    if (sendMailToApprover)
                    {
                        //Send mail to next approver
                        _mailService.SendLeaveApproveMailToApprover(nextApprovalLineItem.ApprovalEmployeeId, leaveRequestLineItem.EmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);
                        _mailService.SendLeaveApproveConfirmationMail(leaveRequestLineItem.EmployeeId, leaveApprovalLineItem.ApprovalEmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);


                        //Notify Leave request Initiator of approval progress
                        var userDetails = await _accountRepository.GetUserByEmployeeId(leaveRequestLineItem.EmployeeId);
                        var app = await _accountRepository.GetUserByEmployeeId(leaveApprovalLineItem.ApprovalEmployeeId);
                        StringBuilder mailBody = new StringBuilder();

                        var leaveType = await _leaveTypeService.GetLeaveTypeById(leaveRequestLineItem.LeaveTypeId);
                        leaveType.LeaveTypeName = leaveType.LeaveTypeName.Replace("leave", "", StringComparison.OrdinalIgnoreCase);

                        mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName} <br/> <br/>");
                        mailBody.Append($"Kindly note that the next approval is currently pending on  {app.FirstName} {app.LastName} {leaveApprovalLineItem.ApprovalPosition},  <br/> <br/>");
                        mailBody.Append($"<b>Start Date : <b/> {leaveRequestLineItem.startDate}  <br/> ");
                        mailBody.Append($"<b>End Date : <b/> {leaveRequestLineItem.endDate}   <br/> ");

                        var mailPayload = new MailRequest
                        {
                            Body = mailBody.ToString(),
                            Subject = $"{leaveType.LeaveTypeName} leave Approval",
                            ToEmail = userDetails.OfficialMail,
                        };

                        _logger.LogError($"Email payload to send: {mailPayload}.");
                        _mailService.SendEmailAsync(mailPayload, null);

                    }

                    if (sendMailToReliever)
                    {
                        //Send mail to reliever
                        _mailService.SendLeaveMailToReliever(leaveRequestLineItem.RelieverUserId, leaveRequestLineItem.EmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);
                    }

                    if (sendApprovalMailToInitiator)
                    {
                        var userDetails = await _accountRepository.GetUserByEmployeeId(leaveRequestLineItem.EmployeeId);
                        //  var app = await _accountRepository.GetUserByEmployeeId(leaveApprovalLineItem.ApprovalEmployeeId);
                        StringBuilder mailBody = new StringBuilder();
                        var leaveType = await _leaveTypeService.GetLeaveTypeById(leaveRequestLineItem.LeaveTypeId);
                        leaveType.LeaveTypeName = leaveType.LeaveTypeName.Replace("leave", "", StringComparison.OrdinalIgnoreCase);

                        mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} <br/> <br/>");
                        mailBody.Append($"Kindly note that your request for {leaveType.LeaveTypeName} leave has successfully passed the final stage for approval. Enjoy your leave<br/> <br/>");
                        mailBody.Append($"<b>Start Date : <b/> {leaveRequestLineItem.startDate}  <br/> ");
                        mailBody.Append($"<b>End Date : <b/> {leaveRequestLineItem.endDate}   <br/> ");

                        var mailPayload = new MailRequest
                        {
                            Body = mailBody.ToString(),
                            Subject = $"{leaveType.LeaveTypeName} Leave Request",
                            ToEmail = userDetails.OfficialMail,
                        };

                        _logger.LogError($"Email payload to send: {mailPayload}.");
                        _mailService.SendEmailAsync(mailPayload);
                    }

                    response.ResponseMessage = $"Approved Successfully";
                }
                else if (!repoResponse.IsApproved || repoResponse.ApprovalStatus == "Disapproved") // Leave approval is denied
                {
                    currentLeaveApprovalInfo.ApprovalStatus = "Completed";
                    currentLeaveApprovalInfo.Comments = $"Disapproved by {repoResponse.ApprovalPosition}";
                    currentLeaveApprovalInfo.CurrentApprovalID = (int)leaveApprovalLineItem.ApprovalEmployeeId;
                    if (!string.IsNullOrEmpty(repoResponse.Comments))
                    {
                        currentLeaveApprovalInfo.Comments += "," + repoResponse.Comments;
                    }
                    annualLeave.ApprovalStatus = currentLeaveApprovalInfo.ApprovalStatus;
                    annualLeave.Comments = currentLeaveApprovalInfo.Comments;

                    //else
                    //{
                    //    currentLeaveApprovalInfo.Comments = "Completed";
                    //}

                    // leaveRequestLineItem = await _leaveRequestRepository.GetLeaveRequestLineItem(currentLeaveApprovalInfo.LeaveRequestLineItemId);

                    _mailService.SendLeaveDisapproveConfirmationMail(leaveRequestLineItem.EmployeeId, repoResponse.ApprovalEmployeeId);
                    response.ResponseMessage = "Disapproved Successfully";
                }

                _ = _leaveApprovalRepository.UpdateLeaveApprovalInfo(currentLeaveApprovalInfo);

                response.ResponseCode = ((int)ResponseCode.Ok).ToString();
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

        private async Task<AnnualLeave> GetEmpAnnualLeaveInfo(long LeaveApprovalId)
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

                var leaveApproval = await _leaveApprovalRepository.GetLeaveApprovalInfo(LeaveApprovalId);
                var leaveRequestLineItem = await _leaveRequestRepository.GetLeaveRequestLineItem(leaveApproval.LeaveRequestLineItemId);
                var res = await _leaveRequestRepository.GetAnnualLeaveInfo(leaveRequestLineItem.AnnualLeaveId.Value);
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
        public async Task<BaseResponse> UpdateLeaveApproveLineItems(List<LeaveApprovalLineItem> leaveApprovalLineItems, string approvalStatus, string comments = "N/A")
        {
            //check if us
            //  StringBuilder errorOutput = new StringBuilder();

            var updateLeaveRequestLineItem = new LeaveRequestLineItem();
            bool sendMailToApprover = false;
            bool sendMailToReliever = false;
            var response = new BaseResponse();
            LeaveApprovalLineItem nextApprovalLineItem = null;
            var singleItem = leaveApprovalLineItems.FirstOrDefault();
            var anualLeave = await GetEmpAnnualLeaveInfo(singleItem.LeaveApprovalId);
            var approvalItems = new List<LeaveApprovalLineItem>();
            //  var leaveApprovalItems = _leaveApprovalRepository.GetPendingLeaveApprovals(noOfDaysTaken);

            _logger.LogInformation($"About to update approval Items to approval status: {approvalStatus}");
            _logger.LogInformation($"No of items to set status to{approvalStatus} are {leaveApprovalLineItems.Count}");
            int approvalCount = 1;
            foreach (var leaveApprovalLineItem in leaveApprovalLineItems)
            {
                _logger.LogInformation($"Now treating {approvalCount} of {leaveApprovalLineItems.Count}  leave Approval with LeaveApprovalLineItemId:{leaveApprovalLineItem.LeaveApprovalLineItemId}");
                try
                {

                    //// Check if it is the Approver's turn to approve
                    //_logger.LogInformation($"About to Check if it is the Approver's turn to approve: LeaveApprovalLineItemId:{leaveApprovalLineItem.LeaveApprovalLineItemId}");
                    //var Emilokan = (await _leaveApprovalRepository.GetPendingLeaveApprovals(leaveApprovalLineItem.ApprovalEmployeeId, "")).FirstOrDefault(x => x.LeaveApprovalLineItemId == leaveApprovalLineItem.LeaveApprovalLineItemId);
                    //if (Emilokan != null)
                    //{
                    //    if (Emilokan.LastApprovalEmployeeID != leaveApprovalLineItem.ApprovalEmployeeId)
                    //    {
                    //        //Not your turn to approve at this time

                    //        _logger.LogError($"It is not the approver's turn to approve at this time");
                    //        response.ResponseCode = "401";
                    //        response.ResponseMessage = "You cannot peform this action at this time";
                    //        response.Data = null;
                    //        return response;
                    //    }
                    //}
                    leaveApprovalLineItem.ApprovalStatus = approvalStatus;
                    leaveApprovalLineItem.Comments = comments;
                    _logger.LogInformation($"About to update approval Item with payload:  to approval status: {JsonConvert.SerializeObject(leaveApprovalLineItem)}");

                    var repoResponse = await _leaveApprovalRepository.UpdateLeaveApprovalLineItem(leaveApprovalLineItem);
                    approvalItems.Add(repoResponse);

                    _logger.LogInformation($"Response from UpdateLeaveApprovalLineItem: {JsonConvert.SerializeObject(repoResponse)}");
                    if (repoResponse == null)
                    {
                        response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                        response.ResponseMessage = "an error occured while processing your request. Please contact your administrator for further assistance";
                        response.Data = repoResponse;
                        return response;
                    }
                    //if (approvalStatus == "Approved")

                    _logger.LogInformation($"About to fetch leave approval info for {leaveApprovalLineItem.LeaveApprovalId}");
                    var currentLeaveApprovalInfo = await _leaveApprovalRepository.GetLeaveApprovalInfo(leaveApprovalLineItem.LeaveApprovalId);

                    _logger.LogInformation($"Response from GetLeaveApprovalInfo: {JsonConvert.SerializeObject(currentLeaveApprovalInfo)}");

                    if (currentLeaveApprovalInfo == null)
                    {
                        _logger.LogError($"We could not fetch the current leave approval info at this time");
                        response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                        response.ResponseMessage = "An error occured while processing your request. Please contact your administrator for further assistance";
                        //  response.Data = repoResponse;
                        return response;
                    }

                    _logger.LogInformation($"Get the current request being approved/denied");
                    //Get the current request being approved/denied
                    var leaveRequestLineItem = await _leaveRequestRepository.GetLeaveRequestLineItem(currentLeaveApprovalInfo.LeaveRequestLineItemId);


                    _logger.LogInformation($"Response from GetLeaveRequestLineItem: {JsonConvert.SerializeObject(leaveRequestLineItem)}");

                    if (leaveRequestLineItem == null)
                    {
                        response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                        _logger.LogError($"Failed to get current LeaveRequestLine item while trying to procces the request for: {JsonConvert.SerializeObject(leaveApprovalLineItem)}");
                        response.ResponseMessage = "an error occured while processing current LeaveRequestLine request. Please contact your administrator for further assistance";
                        //  response.Data = repoResponse;
                        return response;
                    }

                    if (repoResponse.IsApproved || repoResponse.ApprovalStatus == "Approved")
                    {
                        if (currentLeaveApprovalInfo.RequiredApprovalCount == currentLeaveApprovalInfo.CurrentApprovalCount) //all approvals is complete
                        {
                            _logger.LogInformation($"Approval has been completed for the leave");
                            currentLeaveApprovalInfo.ApprovalStatus = "Completed";

                            currentLeaveApprovalInfo.Comments = "Approved";
                            anualLeave.ApprovalStatus = "Approved";
                            if (!string.IsNullOrEmpty(repoResponse.Comments))
                            {
                                currentLeaveApprovalInfo.Comments += "," + repoResponse.Comments;

                            }
                            currentLeaveApprovalInfo.IsApproved = true;
                            anualLeave.Comments = currentLeaveApprovalInfo.Comments;
                            //   leaveRequestLineItem.IsApproved = true;//update Leaverequestlineitem
                            sendMailToReliever = true;
                            sendApprovalMailToInitiator = true;
                        }

                        if (currentLeaveApprovalInfo.RequiredApprovalCount > currentLeaveApprovalInfo.CurrentApprovalCount)
                        {
                            currentLeaveApprovalInfo.CurrentApprovalCount += 1;

                            // currentLeaveApprovalInfo.ApprovalStatus = $"Pending on Approval count: {repoResponse.ApprovalStep}";

                            _logger.LogInformation($"About to fetch next approval for LeaveApprovalId: {repoResponse.LeaveApprovalId}");

                            nextApprovalLineItem = await _leaveApprovalRepository.GetLeaveApprovalLineItem(repoResponse.LeaveApprovalId, currentLeaveApprovalInfo.CurrentApprovalCount);

                            _logger.LogInformation($"Response from GetLeaveRequestLineItem: {JsonConvert.SerializeObject(leaveRequestLineItem)}");

                            if (nextApprovalLineItem == null)
                            {
                                response.ResponseCode = ((int)ResponseCode.NotFound).ToString();
                                _logger.LogError($"Failed to get current Next Approval item while trying to procces the request for: {JsonConvert.SerializeObject(leaveApprovalLineItem)}");
                                response.ResponseMessage = "an error occured while processing current Approval. We could not get the next approver from the db. Please contact your administrator for further assistance";
                               
                                return response;
                            }

                            currentLeaveApprovalInfo.CurrentApprovalID = (int)nextApprovalLineItem.ApprovalEmployeeId;

                            currentLeaveApprovalInfo.Comments = $"Pending on {nextApprovalLineItem.ApprovalPosition}";
                            sendMailToApprover = true;
                        }

                        if (sendMailToApprover)
                        {
                            _logger.LogInformation($"About to send mail to next approver with payload: {JsonConvert.SerializeObject(new { nextApprovalLineItem.ApprovalEmployeeId, leaveRequestLineItem.EmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate})}");

                            //Send mail to next approver
                            var nextApprover = await _accountRepository.GetUserByEmployeeId(nextApprovalLineItem.ApprovalEmployeeId);
                            var requester = await _accountRepository.GetUserByEmployeeId(leaveRequestLineItem.EmployeeId);

                            var leaveType = await _leaveTypeService.GetLeaveTypeById(leaveRequestLineItem.LeaveTypeId);
                            leaveType.LeaveTypeName = leaveType.LeaveTypeName.Replace("leave", "", StringComparison.OrdinalIgnoreCase);

                            StringBuilder mailBody = new StringBuilder();
                            mailBody.Append($"Dear {nextApprover.FirstName} {nextApprover.LastName} {nextApprover.MiddleName} <br/> <br/>");
                            mailBody.Append($"Kindly login to approve a leave request by {requester.FirstName} {requester.LastName} {requester.MiddleName} <br/> <br/>");
                            mailBody.Append($"<b>Start Date : <b/> {leaveRequestLineItem.startDate}  <br/> ");
                            mailBody.Append($"<b>End Date : <b/> {leaveRequestLineItem.endDate}   <br/> ");

                            var mailPayload = new MailRequest
                            {
                                Body = mailBody.ToString(),
                                Subject = "Leave Approval",
                                ToEmail = nextApprover.OfficialMail,
                                DisplayName = "HRMS Leave Approval",
                                EmailTitle = "Leave Approval"

                            };

                            _logger.LogInformation($"Email payload to send: {JsonConvert.SerializeObject(mailPayload)}.");
                            _mailService.SendEmailAsync(mailPayload, null);

                            //_mailService.SendLeaveApproveMailToApprover(nextApprovalLineItem.ApprovalEmployeeId, leaveRequestLineItem.EmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);

                            _logger.LogInformation($"About to send confirmation mail to requester with payload: {JsonConvert.SerializeObject(new { leaveRequestLineItem.EmployeeId, leaveApprovalLineItem.ApprovalEmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate })}");

                            _mailService.SendLeaveApproveConfirmationMail(leaveRequestLineItem.EmployeeId, leaveApprovalLineItem.ApprovalEmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);


                            //Notify Leave request Initiator of approval progress
                            mailBody = new StringBuilder();
                            mailBody.Append($"Dear {requester.FirstName} {requester.LastName} {requester.MiddleName} <br/> <br/>");
                            mailBody.Append($"Kindly note that the next approval for {leaveType.LeaveTypeName} leave is currently pending on  {nextApprover.FirstName} {nextApprover.LastName} {leaveApprovalLineItem.ApprovalPosition},  <br/> <br/>");
                            mailBody.Append($"<b>Start Date : <b/> {leaveRequestLineItem.startDate}  <br/> ");
                            mailBody.Append($"<b>End Date : <b/> {leaveRequestLineItem.endDate}   <br/> ");

                            mailPayload = new MailRequest
                            {
                                Body = mailBody.ToString(),
                                Subject = "Leave Request",
                                ToEmail = requester.OfficialMail,
                                DisplayName = "HRMS",
                                EmailTitle = "Leave Request"
                            };

                            _logger.LogError($"Email payload to send: {JsonConvert.SerializeObject(mailPayload)}.");
                            _mailService.SendEmailAsync(mailPayload, null);
                            sendMailToApprover = false;
                        }

                        if (sendMailToReliever)
                        {
                            //Send mail to reliever
                            _mailService.SendLeaveMailToReliever(leaveRequestLineItem.RelieverUserId, leaveRequestLineItem.EmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);
                        }

                        if (sendApprovalMailToInitiator)
                        {
                            var userDetails = await _accountRepository.GetUserByEmployeeId(leaveRequestLineItem.EmployeeId);
                            //  var app = await _accountRepository.GetUserByEmployeeId(leaveApprovalLineItem.ApprovalEmployeeId);
                            StringBuilder mailBody = new StringBuilder();
                            mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} <br/> <br/>");
                            mailBody.Append($"Kindly note that your request for leave has successfully passed the final stage for approval. Enjoy your leave<br/> <br/>");
                            mailBody.Append($"<b>Start Date : <b/> {leaveRequestLineItem.startDate}  <br/> ");
                            mailBody.Append($"<b>End Date : <b/> {leaveRequestLineItem.endDate}   <br/> ");

                            var mailPayload = new MailRequest
                            {
                                Body = mailBody.ToString(),
                                Subject = "Leave Request",
                                ToEmail = userDetails.OfficialMail,
                                DisplayName = "HRMS",
                                EmailTitle = "Leave Approval"
                            };

                            _logger.LogError($"Email payload to send: {JsonConvert.SerializeObject(mailPayload)}.");
                            _mailService.SendEmailAsync(mailPayload);
                            sendApprovalMailToInitiator = false;
                        }

                        response.ResponseMessage = "Approved Successfully";
                    }
                    else if (!repoResponse.IsApproved || repoResponse.ApprovalStatus == "Disapproved") // Leave approval is denied
                    {
                        _logger.LogInformation($"Approval has been completed for the leave");
                        currentLeaveApprovalInfo.ApprovalStatus = "Completed";

                        currentLeaveApprovalInfo.Comments = $"Disapproved by {repoResponse.ApprovalPosition}";
                        if (!string.IsNullOrEmpty(repoResponse.Comments))
                        {
                            currentLeaveApprovalInfo.Comments += "," + repoResponse.Comments;

                        }
                        anualLeave.ApprovalStatus = "Disapproved";

                        _logger.LogInformation($"About to send disapproval mail to requester with payload: {JsonConvert.SerializeObject(new { leaveRequestLineItem.EmployeeId, repoResponse.ApprovalEmployeeId })}");

                        _mailService.SendLeaveDisapproveConfirmationMail(leaveRequestLineItem.EmployeeId, repoResponse.ApprovalEmployeeId);
                        response.ResponseMessage = "Disapproved Successfully";
                    }


                    //  _ = UpdateLeavePlanner(leaveRequestLineItem);
                    _logger.LogInformation($"About to update  UpdateLeaveApprovalInfo. Payload: {JsonConvert.SerializeObject(currentLeaveApprovalInfo)}");
                    _ = _leaveApprovalRepository.UpdateLeaveApprovalInfo(currentLeaveApprovalInfo);

                    response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                    // response.ResponseMessage = ResponseCode.Ok.ToString();
                    response.Data = approvalItems;
                   // return response;

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception Occured ==> {ex.Message}");
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Exception occured";
                    response.Data = null;

                    return response;
                }
                approvalCount++;
            }
            _leaveRequestRepository.UpdateAnnualLeave(anualLeave);
            return response;

        }
        private async Task Process_Email(LeaveRequestLineItem leaveRequestLineItem, LeaveApprovalLineItem leaveApprovalLineItem)
        {
            var userDetails = await _accountRepository.GetUserByEmployeeId(leaveRequestLineItem.EmployeeId);
          //  var app = await _accountRepository.GetUserByEmployeeId(leaveApprovalLineItem.ApprovalEmployeeId);
            StringBuilder mailBody = new StringBuilder();
            mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} <br/> <br/>");
            mailBody.Append($"Kindly note that your request for leave has successfully passed the final stage for approval. Enjoy your leave<br/> <br/>");
            mailBody.Append($"<b>Start Date : <b/> {leaveRequestLineItem.startDate}  <br/> ");
            mailBody.Append($"<b>End Date : <b/> {leaveRequestLineItem.endDate}   <br/> ");

            var mailPayload = new MailRequest
            {
                Body = mailBody.ToString(),
                Subject = "Leave Request",
                ToEmail = userDetails.OfficialMail,
            };

            _logger.LogError($"Email payload to send: {mailPayload}.");
            _mailService.SendEmailAsync(mailPayload, null);

        }
        private async Task ProcessLeaveApproval(long leaveApprovalId, bool isApproved)
        {
           
        }

     
        public async Task<LeaveApprovalInfo> GetAnnualLeaveApprovalInfo(long leaveApprovalId, long leaveReqestLineItemId)
        {
            LeaveApprovalInfo leaveApproval = null;
            try
            {
                if (leaveApprovalId > 0)
                {
                    leaveApproval = await _leaveApprovalRepository.GetLeaveApprovalInfo(leaveApprovalId);
                }
                else if (leaveReqestLineItemId > 0)
                {
                    leaveApproval = await _leaveApprovalRepository.GetLeaveApprovalInfoByRequestLineItemId(leaveReqestLineItemId);
                }
                if (leaveApproval != null)
                {

                    leaveApproval.LeaveApprovalLineItems = await GetleaveApprovalLineItems(leaveApproval.LeaveApprovalId);

                }
                return leaveApproval;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> GetLeaveApprovalInfoByApprovalKey(long ApprovalKey)
        {
            try
            {
                var leaveApproval = await _leaveApprovalRepository.GetLeaveApprovalInfoByApprovalKey(ApprovalKey);

                return leaveApproval;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<LeaveApprovalInfo> GetLeaveApprovalInfo(long leaveApprovalId, long leaveReqestLineItemId)
        {
            LeaveApprovalInfo leaveApproval = null;
            try
            {
                if (leaveApprovalId > 0)
                {
                    leaveApproval = await _leaveApprovalRepository.GetLeaveApprovalInfo(leaveApprovalId);
                }
                else if (leaveReqestLineItemId > 0)
                {
                    leaveApproval = await _leaveApprovalRepository.GetLeaveApprovalInfoByRequestLineItemId(leaveReqestLineItemId);
                }
                if (leaveApproval != null)
                {

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
                var leaveApproval = await _leaveApprovalRepository.GetLeaveApprovalLineItems(leaveApprovalId);

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
                var UpdateLeaveApproval = await _leaveApprovalRepository.UpdateLeaveApprovalInfo(leaveApproval);
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
                var leaveApprovalLineitem = await _leaveApprovalRepository.GetLeaveApprovalLineItem(leaveApprovalLineItemId, approvalStep);
                return leaveApprovalLineitem;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<List<PendingAnnualLeaveApprovalItemDto>> GetPendingAnnualLeaveApprovals(long approvalEmployeeID, string v = null)
        {
            try
            {
                var res = await _leaveApprovalRepository.GetPendingAnnualLeaveApprovals(approvalEmployeeID, v);
               
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PendingLeaveApprovalItemsDto>> GetPendingLeaveApprovals(long approvalEmployeeID, string v = null)
        {
            try
            {
                var res = await _leaveApprovalRepository.GetPendingLeaveApprovals(approvalEmployeeID, v);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<LeaveApprovalInfoDto>> GetLeaveApprovalInfoByCompanyID(long CompanyID)
        {
            try
            {
                var res = await _leaveApprovalRepository.GetLeaveApprovalInfoByCompanyID(CompanyID);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LeaveApprovalInfo> GetLeaveApprovalInfoByRequestLineItemId(long leaveRequestLineItemId)
        {
            try
            {
                var LeaveApprovalInfo = await _leaveApprovalRepository.GetLeaveApprovalInfoByRequestLineItemId(leaveRequestLineItemId);
                return LeaveApprovalInfo;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
