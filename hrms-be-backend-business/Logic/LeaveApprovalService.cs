﻿using GTB.Common;
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


        public async Task<BaseResponse> UpdateLeaveApproveLineItem(LeaveApprovalLineItem leaveApprovalLineItem)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var updateLeaveRequestLineItem = new LeaveRequestLineItem();
            bool sendMailToApprover = false;
            bool sendMailToReliever = false;
            var response = new BaseResponse();
            LeaveApprovalLineItem nextApprovalLineItem = null;
            
            try
            {
                var Emilokan = (await _leaveApprovalRepository.GetPendingLeaveApprovals(leaveApprovalLineItem.ApprovalEmployeeId, "")).FirstOrDefault(x => x.LeaveApprovalLineItemId == leaveApprovalLineItem.LeaveApprovalLineItemId);
                if (Emilokan != null)
                {
                    if (Emilokan.LastApprovalEmployeeID != leaveApprovalLineItem.ApprovalEmployeeId) 
                    {
                        //Not your turn to approve
                        response.ResponseCode = "401";
                        response.ResponseMessage = "You cannot peform this action at this time";
                        response.Data = null;
                        return response;
                    }
                }

                var repoResponse = await _leaveApprovalRepository.UpdateLeaveApprovalLineItem(leaveApprovalLineItem);
                if (repoResponse == null)
                {
                    response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                    response.ResponseMessage = "an error occured while processing your request. Please contact your administrator for further assistance";
                    response.Data = repoResponse;
                    return response;
                }

                _ = ProcessLeaveApproval(repoResponse.LeaveApprovalId, repoResponse.IsApproved);
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
                var leaveRequestLineItem = await _leaveRequestRepository.GetLeaveRequestLineItem(currentLeaveApprovalInfo.LeaveRequestLineItemId);
              
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

                        currentLeaveApprovalInfo.Comments = "Approved";
                        if (!string.IsNullOrEmpty(repoResponse.Comments))
                        {
                            currentLeaveApprovalInfo.Comments += "," + repoResponse.Comments;

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
                        mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName} <br/> <br/>");
                        mailBody.Append($"Kindly note that the next approval is currently pending on  {app.FirstName} {app.LastName} {leaveApprovalLineItem.ApprovalPosition},  <br/> <br/>");
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
                        };

                        _logger.LogError($"Email payload to send: {mailPayload}.");
                        _mailService.SendEmailAsync(mailPayload);
                    }

                    response.ResponseMessage = "Approved Successfully";
                }
                else if (!repoResponse.IsApproved || repoResponse.ApprovalStatus == "Disapproved") // Leave approval is denied
                {
                    currentLeaveApprovalInfo.ApprovalStatus = "Completed";
                    currentLeaveApprovalInfo.Comments = $"Disapproved by {repoResponse.ApprovalPosition}";
                    if (!string.IsNullOrEmpty(repoResponse.Comments))
                    {
                        currentLeaveApprovalInfo.Comments += ","+repoResponse.Comments;

                    }
                    //else
                    //{
                    //    currentLeaveApprovalInfo.Comments = "Completed";
                    //}

                    // leaveRequestLineItem = await _leaveRequestRepository.GetLeaveRequestLineItem(currentLeaveApprovalInfo.LeaveRequestLineItemId);

                    _mailService.SendLeaveDisapproveConfirmationMail(leaveRequestLineItem.EmployeeId, repoResponse.ApprovalEmployeeId);
                    response.ResponseMessage = "Disapproved Successfully";
                }
                #region Depricated
                //await _leaveRequestRepository.UpdateLeaveRequestLineItemApproval(leaveRequestLineItem);
                #endregion

                _ = UpdateLeavePlanner(leaveRequestLineItem);
                _ = _leaveApprovalRepository.UpdateLeaveApprovalInfo(currentLeaveApprovalInfo);

                response.ResponseCode = ((int)ResponseCode.Ok).ToString();
               // response.ResponseMessage = ResponseCode.Ok.ToString();
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
                var leaveApprovals = await _leaveApprovalRepository.GetLeaveApprovals(leaveApprovalLineItem.ApprovalEmployeeId, leaveApprovalLineItem.EmployeeID);

                _logger.LogInformation($"Response from GetLeaveApprovals method:{JsonConvert.SerializeObject(leaveApprovals)}");
                if (leaveApprovals == null || leaveApprovals.Count <= 0)
                {
                    _logger.LogError($"We could not get any records for the the given leave ApprovalEmployeeId:{leaveApprovalLineItem.ApprovalEmployeeId}");
                    return new BaseResponse { Data = null, ResponseCode = "25", ResponseMessage = $"We could not get any records for the the given leave ApprovalEmployeeId:{leaveApprovalLineItem.ApprovalEmployeeId}" };
                }

                var leaveapprovalLineItems = new List<LeaveApprovalLineItem>();
                _logger.LogInformation($"Leave approval update will treat {leaveApprovals.Count} records");
                int count = 0;
                foreach (var item in leaveApprovals)
                {
                    count++;
                    _logger.LogInformation($"Now treating {count} of {leaveApprovals.Count}  leave Approval with LeaveApprovalLineItemId:{item.LeaveApprovalLineItemId}");

                    
                    _logger.LogInformation($"About to fetch Annual Leave info for LeaveApprovalId: {item.LeaveApprovalId}");
                    var info = await _leaveApprovalRepository.GetAnnualLeaveApprovalInfo(item.LeaveApprovalId);
                    _logger.LogInformation($"Response from GetAnnualLeaveApprovalInfo: {JsonConvert.SerializeObject(info)}");
                    if (info == null)
                    {

                        _logger.LogError($"We could not get any records for the the given leave LeaveApprovalId:{item.LeaveApprovalId}");
                       
                        return new BaseResponse { Data = null, ResponseCode = "25", ResponseMessage = "No Annual Leave Record found" };
                    }

                    int count1 = 0;
                    foreach (var LeaveApprovalLineItem1 in info.LeaveApprovalLineItems)
                    {
                        _logger.LogInformation($"Now treating {count1} of {info.LeaveApprovalLineItems} leave Approval item LeaveApprovalLineItemId:{LeaveApprovalLineItem1.LeaveApprovalLineItemId}");

                        if (LeaveApprovalLineItem1.ApprovalEmployeeId == leaveApprovalLineItem.ApprovalEmployeeId && LeaveApprovalLineItem1.ApprovalStep == leaveApprovalLineItem.ApprovalStep)
                        {

                            leaveapprovalLineItems.Add(LeaveApprovalLineItem1);
                            if (ConfigSettings.leaveRequestConfig.EnableSingleApproval)
                            {

                                break;
                            }
                        }
                    }
                }
                _logger.LogInformation($"Items to update: {JsonConvert.SerializeObject(leaveapprovalLineItems)}");
                var res = await UpdateLeaveApproveLineItems(leaveapprovalLineItems, leaveApprovalLineItem.ApprovalStatus, leaveApprovalLineItem.Comments);
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured: {ex.Message}, Stack trace: {ex.StackTrace}");

                throw;
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
                var res = await _leaveRequestRepository.GetAnnualLeaveInfo(leaveRequestLineItem.AnnualLeaveId);
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

                    // Check if it is the Approver's turn to approve
                    _logger.LogInformation($"About to Check if it is the Approver's turn to approve: LeaveApprovalLineItemId:{leaveApprovalLineItem.LeaveApprovalLineItemId}");
                    var Emilokan = (await _leaveApprovalRepository.GetPendingLeaveApprovals(leaveApprovalLineItem.ApprovalEmployeeId, "")).FirstOrDefault(x => x.LeaveApprovalLineItemId == leaveApprovalLineItem.LeaveApprovalLineItemId);
                    if (Emilokan != null)
                    {
                        if (Emilokan.LastApprovalEmployeeID != leaveApprovalLineItem.ApprovalEmployeeId)
                        {
                            //Not your turn to approve at this time

                            _logger.LogError($"It is not the approver's turn to approve at this time");
                            response.ResponseCode = "401";
                            response.ResponseMessage = "You cannot peform this action at this time";
                            response.Data = null;
                            return response;
                        }
                    }
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

        private async Task UpdateLeavePlanner(LeaveRequestLineItem leaveRequestLineItem)
        {
            //Update active leave info for employee if maximum days or split count reached.
            var empLeaveRequestInfo = await _leaveRequestRepository.GetEmpLeaveInfo(employeeId: leaveRequestLineItem.EmployeeId);
            var gradeLeave = await _leaveRequestRepository.GetEmployeeGradeLeave(leaveRequestLineItem.EmployeeId, leaveRequestLineItem.LeaveTypeId);
            var leaveRequestLineItems = await _leaveRequestRepository.GetLeaveRequestLineItems(empLeaveRequestInfo.LeaveRequestId);


           

            //int noOfDaysTaken = leaveRequestLineItems.Where(x => x.IsApproved == true).Sum(x => x.LeaveLength);
            //if (gradeLeave.NumbersOfDays >= noOfDaysTaken || gradeLeave.NumberOfVacationSplit == leaveRequestLineItems.Count())

            //if (gradeLeave != null)
            //{
            //    if (gradeLeave.NumbersOfDays >= noOfDaysTaken)
            //    {
            //        empLeaveRequestInfo.LeaveStatus = "Completed";
            //        _leaveRequestRepository.UpdateLeaveRequestInfoStatus(empLeaveRequestInfo);
            //    }
            //}
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

      

        #region Depricated
        //public async Task<BaseResponse> DisaproveLeaveRequest(LeaveRequestDisapproved payload, RequesterInfo requester)
        //{
        //    //check if us
        //    StringBuilder errorOutput = new StringBuilder();
        //    var response = new BaseResponse();
        //    try
        //    {


        //        var repoResponse = await _leaveRequestRepository.DisaproveLeaveRequest(payload.LeaveRequestID, requester.UserId, payload.Reasons_For_Disapprove);
        //        if (!repoResponse.Contains("Success"))
        //        {
        //            response.ResponseCode = "08";
        //            response.ResponseMessage = repoResponse;
        //            return response;
        //        }

        //        var leaveRequestDetail = await _leaveRequestRepository.GetLeaveRequestById(payload.LeaveRequestID);

        //        var userDetails = await _employeeRepository.GetEmployeeByUserId(leaveRequestDetail.UserId);

        //        //Send mail to reliever
        //        _mailService.SendLeaveApproveConfirmationMail(leaveRequestDetail.UserId, requester.UserId, leaveRequestDetail.StartDate, leaveRequestDetail.EndDate);

        //        //Send mail to approval
        //        if (!leaveRequestDetail.IsHodApproved)
        //        {
        //            _mailService.SendLeaveApproveMailToApprover(userDetails.HodEmployeeId, leaveRequestDetail.UserId, leaveRequestDetail.StartDate, leaveRequestDetail.EndDate);
        //        }

        //        response.ResponseCode = "00";
        //        response.ResponseMessage = "Record inserted successfully";
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = "Exception occured";
        //        response.Data = null;



        //        return response;
        //    }
        //}


        //public async Task<BaseResponse> GetLeaveRequsetById(long LeaveRequestID, RequesterInfo requester)
        //{
        //    BaseResponse response = new BaseResponse();

        //    try
        //    {
        //        string requesterUserEmail = requester.Username;
        //        string requesterUserId = requester.UserId.ToString();
        //        string RoleId = requester.RoleId.ToString();

        //        var ipAddress = requester.IpAddress.ToString();
        //        var port = requester.Port.ToString();

        //        var requesterInfo = await _accountRepository.FindUser(null, requesterUserEmail, null);
        //        if (null == requesterInfo)
        //        {
        //            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = "Requester information cannot be found.";
        //            return response;
        //        }


        //        //if (Convert.ToInt32(RoleId) != 2)
        //        //{
        //        //    if (Convert.ToInt32(RoleId) != 4)
        //        //    {
        //        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        //        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
        //        //        return response;

        //        //    }

        //        //}

        //        var LeaveRequest = await _leaveRequestRepository.GetLeaveRequestById(LeaveRequestID);

        //        if (LeaveRequest == null)
        //        {
        //            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = "LeaveRequest not found.";
        //            response.Data = null;
        //            return response;
        //        }

        //        //update action performed into audit log here

        //        response.Data = LeaveRequest;
        //        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = "LeaveRequest fetched successfully.";
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: GetLeaveRequestById(long LeaveRequestID ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured:  GetLeaveRequestById(long LeaveRequestID  ==> {ex.Message}";
        //        response.Data = null;
        //        return response;
        //    }
        //}
        //public async Task<BaseResponse> GetLeaveRequsetByUerId(long UserId, long CompanyId, RequesterInfo requester)
        //{
        //    BaseResponse response = new BaseResponse();

        //    try
        //    {
        //        string requesterUserEmail = requester.Username;
        //        string requesterUserId = requester.UserId.ToString();
        //        string RoleId = requester.RoleId.ToString();

        //        // var ipAddress = requester.IpAddress;
        //        // var port = requester.Port.ToString();

        //        var requesterInfo = await _accountRepository.FindUser(null, requesterUserEmail, null);
        //        if (null == requesterInfo)
        //        {
        //            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = "Requester information cannot be found.";
        //            return response;
        //        }



        //        var LeaveRequest = await _leaveRequestRepository.GetLeaveRequestByUserId(UserId, CompanyId);

        //        if (LeaveRequest == null)
        //        {
        //            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = "User LeaveRequest not found.";
        //            response.Data = null;
        //            return response;
        //        }

        //        //update action performed into audit log here

        //        response.Data = LeaveRequest;
        //        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = "User LeaveRequest fetched successfully.";
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: GetLeaveRequestByUserId(long UserId ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured:  GetLeaveRequestByUserId(long UserId  ==> {ex.Message}";
        //        response.Data = null;
        //        return response;
        //    }
        //}
        //public async Task<BaseResponse> GetLeaveRquestbyCompanyId(string RequestYear, long companyId, RequesterInfo requester)
        //{
        //    BaseResponse response = new BaseResponse();

        //    try
        //    {
        //        string requesterUserEmail = requester.Username;
        //        string requesterUserId = requester.UserId.ToString();
        //        string RoleId = requester.RoleId.ToString();

        //        var ipAddress = requester.IpAddress.ToString();
        //        var port = requester.Port.ToString();

        //        var requesterInfo = await _accountRepository.FindUser(null, requesterUserEmail, null);
        //        if (null == requesterInfo)
        //        {
        //            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = "Requester information cannot be found.";
        //            return response;
        //        }



        //        var LeaveRquest = await _leaveRequestRepository.GetLeaveRequestByCompany(RequestYear, companyId);

        //        if (LeaveRquest == null)
        //        {
        //            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = "LeaveRequest not found.";
        //            response.Data = null;
        //            return response;
        //        }

        //        //update action performed into audit log here

        //        response.Data = LeaveRquest;
        //        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = "LeaveRequest fetched successfully.";
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: GetLeaveRequestByCompanyID(string RequestYear,long companyId) ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: GetLeaveRequestByCompanyID(string RequestYear, long companyId) ==> {ex.Message}";
        //        response.Data = null;
        //        return response;
        //    }
        //}
        //public async Task<BaseResponse> GetLeaveRequestPendingApproval(RequesterInfo requester)
        //{
        //    BaseResponse response = new BaseResponse();

        //    try
        //    {
        //        string requesterUserEmail = requester.Username;
        //        string requesterUserId = requester.UserId.ToString();
        //        string RoleId = requester.RoleId.ToString();

        //        var ipAddress = requester.IpAddress.ToString();
        //        var port = requester.Port.ToString();

        //        var requesterInfo = await _accountRepository.FindUser(null, requesterUserEmail, null);
        //        if (null == requesterInfo)
        //        {
        //            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = "Requester information cannot be found.";
        //            return response;
        //        }

        //        var leave = await _leaveRequestRepository.GetLeaveRequestPendingApproval(requester.UserId);

        //        if (leave.Any())
        //        {
        //            response.Data = leave;
        //            response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = "LeaveRequest fetched successfully.";
        //            return response;
        //        }
        //        response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = "No record found.";
        //        response.Data = null;
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: GetLeaveRequestPendingApproval() ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: GetLeaveRequestPendingApproval() ==> {ex.Message}";
        //        response.Data = null;
        //        return response;
        //    }
        //}
        //public async Task<BaseResponse> CreateLeaveRequest(LeaveRequestCreate payload, RequesterInfo requester)
        //{
        //    //check if us

        //    StringBuilder errorOutput = new StringBuilder();
        //    var response = new BaseResponse();
        //    try
        //    {
        //        if (string.IsNullOrEmpty(payload.Notes))
        //        {
        //            response.ResponseCode = "08";
        //            response.ResponseMessage = "Note is required"; return response;
        //        }
        //        if (payload.RequestYear < 1)
        //        {
        //            response.ResponseCode = "08";
        //            response.ResponseMessage = "Request Year is required";
        //            return response;
        //        }

        //        var repoResponse = await _leaveRequestRepository.CreateLeaveRequest(payload);
        //        if (!repoResponse.Contains("Success"))
        //        {
        //            response.ResponseCode = "08";
        //            response.ResponseMessage = repoResponse;
        //            return response;
        //        }
        //        var userDetails = await _employeeRepository.GetEmployeeById(payload.EmployeeId, payload.CompanyID);

        //        //Send mail to reliever
        //        _mailService.SendLeaveMailToReliever(payload.RelieverUserID, payload.EmployeeId, payload.StartDate, payload.EndDate);

        //        //Send mail to approval
        //        if (userDetails.Employee.UnitHeadEmployeeId == null)
        //        {
        //            _mailService.SendLeaveApproveMailToApprover(userDetails.Employee.HodEmployeeId, payload.EmployeeId, payload.StartDate, payload.EndDate);
        //        }
        //        else
        //        {
        //            _mailService.SendLeaveApproveMailToApprover(userDetails.Employee.UnitHeadEmployeeId, payload.EmployeeId, payload.StartDate, payload.EndDate);
        //        }


        //        response.Data = payload;
        //        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = "leaveRequest created successfully.";
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = "Exception occured Contact Admin";
        //        response.Data = null;



        //        return response;
        //    }
        //}
        //public async Task<BaseResponse> RescheduleLeaveRequest(RescheduleLeaveRequest updateDto, RequesterInfo requester)
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        string requesterUserEmail = requester.Username;
        //        string requesterUserId = requester.UserId.ToString();
        //        string RoleId = requester.RoleId.ToString();

        //        var ipAddress = requester.IpAddress.ToString();
        //        var port = requester.Port.ToString();

        //        var requesterInfo = await _accountRepository.FindUser(null, requesterUserEmail, null);
        //        if (null == requesterInfo)
        //        {
        //            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = "Requester information cannot be found.";
        //            return response;
        //        }


        //        //validate DepartmentDto payload here 
        //        if (/*String.IsNullOrEmpty(updateDto.) ||*/ updateDto.CompanyID <= 0)
        //        {
        //            response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = $"Please ensure all required fields are entered.";
        //            return response;
        //        }

        //        var LeaveRequest = await _leaveRequestRepository.GetLeaveRequestById(updateDto.LeaveRequestID);
        //        if (null == LeaveRequest)
        //        {
        //            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = "No record found for the specified LeaveRequest";
        //            response.Data = null;
        //            return response;
        //        }

        //        dynamic resp = await _leaveRequestRepository.RescheduleLeaveRequest(updateDto, requesterUserEmail);
        //        if (resp > 0)
        //        {
        //            //update action performed into audit log here

        //            var updatedLeaveType = await _leaveRequestRepository.GetLeaveRequestById(updateDto.LeaveRequestID);

        //            _logger.LogInformation("Reschedule Leave Request updated successfully.");
        //            response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = "Reschedule Leave Request updated successfully.";
        //            response.Data = updatedLeaveType;
        //            return response;

        //        }

        //        var userDetails = await _employeeRepository.GetEmployeeById(updateDto.EmployeeId, updateDto.CompanyID);

        //        //Send mail to reliever
        //        _mailService.SendLeaveMailToReliever(updateDto.ReliverUserID, updateDto.EmployeeId, updateDto.StartDate, updateDto.EndDate);

        //        //Send mail to approval
        //        if (userDetails.Employee.UnitHeadEmployeeId == null)
        //        {
        //            _mailService.SendLeaveApproveMailToApprover(userDetails.Employee.HodEmployeeId, updateDto.EmployeeId, updateDto.StartDate, updateDto.EndDate);
        //        }
        //        else
        //        {
        //            _mailService.SendLeaveApproveMailToApprover(userDetails.Employee.UnitHeadEmployeeId, userDetails.Employee.EmployeeID, updateDto.StartDate, updateDto.EndDate);
        //        }

        //        response.Data = updateDto;
        //        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = "Reschedule Leave Request created successfully.";
        //        return response;

        //        //response.ResponseCode = ResponseCode.Exception.ToString();
        //        //response.ResponseMessage = "An error occurred while updating Reschedule Leave Request.";
        //        //response.Data = null;
        //        //return response;
        //    }


        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: RescheduleLeaveRequest ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: RescheduleLeaveRequest ==> {ex.Message}";
        //        response.Data = null;
        //        return response;
        //    }
        //}
        //public async Task<BaseResponse> ApproveLeaveRequest(long LeaveRequestID, RequesterInfo requester)
        //{
        //    //check if us
        //    StringBuilder errorOutput = new StringBuilder();
        //    var response = new BaseResponse();
        //    try
        //    {


        //        var repoResponse = await _leaveRequestRepository.ApproveLeaveRequest(LeaveRequestID, requester.UserId);
        //        if (!repoResponse.Contains("Success"))
        //        {
        //            response.ResponseCode = "08";
        //            response.ResponseMessage = repoResponse;
        //            return response;
        //        }

        //        var leaveRequestDetail = await _leaveRequestRepository.GetLeaveRequestById(LeaveRequestID);

        //        var userDetails = await _accountRepository.FindUser(leaveRequestDetail.UserId);

        //        //Send mail to reliever
        //        _mailService.SendLeaveApproveConfirmationMail(leaveRequestDetail.UserId, requester.UserId, leaveRequestDetail.StartDate, leaveRequestDetail.EndDate);

        //        //Send mail to approval
        //        if (leaveRequestDetail.UnitHeadUserID == null)
        //        {
        //            _mailService.SendLeaveApproveMailToApprover(leaveRequestDetail.HRUserId, leaveRequestDetail.UserId, leaveRequestDetail.StartDate, leaveRequestDetail.EndDate);
        //        }
        //        else
        //        {
        //            if (!leaveRequestDetail.IsHodApproved)
        //            {
        //                _mailService.SendLeaveApproveMailToApprover(leaveRequestDetail.HodUserID, leaveRequestDetail.UserId, leaveRequestDetail.StartDate, leaveRequestDetail.EndDate);
        //            }
        //            else
        //            {
        //                _mailService.SendLeaveApproveMailToApprover(leaveRequestDetail.HRUserId, leaveRequestDetail.UserId, leaveRequestDetail.StartDate, leaveRequestDetail.EndDate);
        //            }

        //        }


        //        response.ResponseCode = "00";
        //        response.ResponseMessage = "Record inserted successfully";
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = "Exception occured";
        //        response.Data = null;



        //        return response;
        //    }
        //}


        #endregion
    }
}
