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
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.ComponentModel.Design;
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
        private readonly ILeaveApprovalRepository _leaveApprovalRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IGradeLeaveRepo _gradeLeaveRepo;
        private readonly ILeaveTypeRepository _leaveTypeService;
        private readonly IMailService _mailService;
        private bool processmail = true;

        public LeaveRequestService(IAccountRepository accountRepository, ILogger<LeaveRequestService> logger,
            ILeaveRequestRepository leaveRequestRepository, IAuditLog audit, ICompanyRepository companyrepository, IMailService mailService, ILeaveApprovalRepository leaveApprovalRepository, IGradeLeaveRepo leaveTypeRepository, ILeaveTypeRepository leaveTypeService, IEmployeeRepository employeeRepository)
        {
            _audit = audit;
            _mailService = mailService;
            _logger = logger;
            _accountRepository = accountRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _companyrepository = companyrepository;
            _leaveApprovalRepository = leaveApprovalRepository;
            _gradeLeaveRepo = leaveTypeRepository;
            _leaveTypeService = leaveTypeService;
            _employeeRepository = employeeRepository;
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
            if (dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                return false;
            }
            return true;
        }


        public async Task<BaseResponse> CreateLeaveRequestLineItem(List<LeaveRequestLineItem> RequestLineItems)
        {
          
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            GradeLeave gradeLeave = null;
            int noOfDaysTaken = 0;
            List<LeaveRequestLineItem> responseItems = new List<LeaveRequestLineItem>();
            var leaveRequestItem = RequestLineItems.FirstOrDefault();
            string approvalStatus = string.Empty;
            #region Validate annual Leave Request

            
            var existingAnnualLeave = await CheckAnnualLeaveInfo(leaveRequestItem);
            if (existingAnnualLeave != null)
            {

                //Multiple requests not allowed
                var leaveApproval = await _leaveApprovalRepository.GetLeaveApprovalInfo(existingAnnualLeave.ApprovalID);
                approvalStatus = leaveApproval.Comments.Split(",").First().Split(" ").First();
                if (approvalStatus.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning($"Invalid request. Pending annual leave for approval exists:{JsonConvert.SerializeObject(existingAnnualLeave)}");
                    response.ResponseCode = "08";
                    response.ResponseMessage = $"Invalid request. Pending annual leave for approval exists";
                    response.Data = existingAnnualLeave;
                    return response;
                }

                //Only one approved request shall stand
               
            }
            foreach (var leaveRequestLineItem in RequestLineItems)
            {
                if (approvalStatus.Equals("Approved", StringComparison.OrdinalIgnoreCase)) // == .Comments.Contains("Approved", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var existingItem in existingAnnualLeave.leaveRequestLineItems)
                    {
                        if (DateTime.Now.Date > existingItem.startDate || DateTime.Now.Date > existingItem.endDate)
                        {
                            _logger.LogWarning($"Invalid request. Part of this leave has already been taken:{JsonConvert.SerializeObject(existingAnnualLeave)}");
                            response.ResponseCode = "400";
                            response.ResponseMessage = $"Invalid request. You have already taken part of this leave.";
                            return response;
                        }
                    }
                   
                }
                //Start date must fall within a weekday
                if (!IsValidWeekeday(leaveRequestLineItem.startDate))
                {
                    _logger.LogWarning($"Invalid startdate specified. {leaveRequestLineItem.startDate.DayOfWeek}: {leaveRequestLineItem.startDate} does not fall within a weekday");
                    response.ResponseCode = "08";
                    response.ResponseMessage = $"Invalid startdate specified. {leaveRequestLineItem.startDate.DayOfWeek}: {leaveRequestLineItem.startDate} does not fall within a weekday";
                    return response;
                }

                //End date date must fall within a weekday
                if (!IsValidWeekeday(leaveRequestLineItem.endDate))
                {
                    _logger.LogWarning($"Invalid endDate specified. {leaveRequestLineItem.endDate.DayOfWeek}: {leaveRequestLineItem.endDate} does not fall within a weekday");
                    response.ResponseCode = "08";
                    response.ResponseMessage = $"Invalid endDate specified. {leaveRequestLineItem.endDate.DayOfWeek}: {leaveRequestLineItem.endDate} does not fall within a weekday";
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

                //You cannot relieve yourself

                if (leaveRequestLineItem.EmployeeId == leaveRequestLineItem.RelieverUserId)
                {
                    _logger.LogError("Invalid reliever specified specified. You cannot relieve yourself");
                    response.ResponseCode = "08";
                    response.ResponseMessage = $"Invalid reliever specified. You cannot relieve yourself";
                    return response;
                }

                //Validate Gender
                if (ConfigSettings.leaveRequestConfig.ValidateGender)
                {
                    var empGender = await _employeeRepository.GetEmployeeById(leaveRequestLineItem.EmployeeId, leaveRequestLineItem.CompanyId);
                    if (empGender == null)
                    {
                        _logger.LogError("We could not get the employee information while trying to validate gender");
                        response.ResponseCode = "08";
                        response.ResponseMessage = $"We could not get the employee information while trying to validate gender for the leavetype selected";
                        return response;
                    }
                    var LeaveTypeGender = (await _gradeLeaveRepo.GetEmployeeGradeLeaveTypes(leaveRequestLineItem.CompanyId, leaveRequestLineItem.EmployeeId)).FirstOrDefault(x => x.LeaveTypeId == leaveRequestLineItem.LeaveTypeId && x.GradeID == Convert.ToInt32(empGender.Employee.GradeID));

                    if (LeaveTypeGender == null)
                    {
                        _logger.LogError("We could not get the GradeLeave gender information");
                        response.ResponseCode = "08";
                        response.ResponseMessage = $"We could not get the GradeLeave gender information while trying to validate gender for the leavetype selected";
                        return response;
                    }

                    if (empGender.Employee.SexId != LeaveTypeGender.GenderID)
                    {
                        _logger.LogError($"You cannot apply for this leave type. Invalid gender. Employee GenderID:{empGender.Employee.SexId}, GradeLeave GenderID:{LeaveTypeGender.GenderID}");
                        response.ResponseCode = "08";
                        response.ResponseMessage = $"You cannot apply for this leave type. Invalid gender";
                        return response;
                    }
                }
            }
          
         

            //Number of days requested must not exceed number of days allocated to leave type
            gradeLeave = await _leaveRequestRepository.GetEmployeeGradeLeave(leaveRequestItem.EmployeeId, leaveRequestItem.LeaveTypeId);

            _logger.LogInformation($"GradeLeave info for  {leaveRequestItem.EmployeeId} is {JsonConvert.SerializeObject(gradeLeave)}");

            var totalDaysRequested = RequestLineItems.Sum(x => x.LeaveLength);

            if (totalDaysRequested > gradeLeave.NumbersOfDays)
            {
                _logger.LogWarning($"Invalid request. The total number of days requested exceeds the allocated days to this Leave type. Total number of days requested - {totalDaysRequested}, total number of days allocated - {gradeLeave.NumbersOfDays}");
                response.ResponseCode = "08";
                response.ResponseMessage = $"Invalid request. The total number of days requested exceeds the allocated days to this Leave type. Total number of days requested - {totalDaysRequested}, total number of days allocated to this leave type - {gradeLeave.NumbersOfDays}";
                return response;
            }
            #endregion

            EmpLeaveRequestInfo empLeaveRequestInfo = null;
            empLeaveRequestInfo = await _leaveRequestRepository.GetEmpLeaveInfo(leaveRequestItem.EmployeeId, leaveRequestItem.CompanyId, leaveRequestItem.startDate.Year.ToString());
            if (empLeaveRequestInfo == null)
            {
                empLeaveRequestInfo = await _leaveRequestRepository.CreateEmpLeaveInfo(leaveRequestItem.EmployeeId, leaveRequestItem.CompanyId);
                if (empLeaveRequestInfo == null)
                {
                    _logger.LogError($"Could not create leave request in database.");
                    response.ResponseCode = "08";
                    response.ResponseMessage = $"Could not create leave request. Please try again later or contact support for assistance";
                    return response;
                    //throw new Exception("Could not create leave request");
                }
                //  IsExistingRequest = false;
            }
            bool IsExistingRequest = true;

           
            _logger.LogInformation($"About to create annual leave request for EmployeeId: {leaveRequestItem.EmployeeId} and CompanyId: {leaveRequestItem.CompanyId}");

            var newAnnualRes = await _leaveRequestRepository.CreateAnnualLeaveRequest(RequestLineItems);
            _logger.LogError($"Response from CreateAnnualLeaveRequest:{JsonConvert.SerializeObject(newAnnualRes)}");
            if (newAnnualRes == null)
            {
                response.ResponseCode = "08";
                response.ResponseMessage = $"Annual Leave could not be created. Conact Admin for assistance";
                return response;
            }

            bool processmail = true;
            bool CreateApproval = true;
            LeaveApprovalInfo currentLeaveApprovalInfo = null;

            
            #region Post Activities
            if (newAnnualRes.leaveRequestLineItems.Count == RequestLineItems.Count)
            {
                var leaveRequestLineItem = newAnnualRes.leaveRequestLineItems.FirstOrDefault();

                _logger.LogInformation($"About to get next leave approver using LeaveApprovalId: {newAnnualRes.ApprovalID}.");

                var nextApprovalLineItem = (await GetleaveApprovalLineItems(newAnnualRes.ApprovalID)).FirstOrDefault(x => x.ApprovalStep == 1);
                _logger.LogInformation($"response from getting next approver: {JsonConvert.SerializeObject(nextApprovalLineItem)}.");

             //   LeaveApproval leaveApproval = null;

                if (nextApprovalLineItem == null || nextApprovalLineItem.LeaveApprovalId == 0)
                {
                    _logger.LogError($"Could not get next leave approver");
                    _logger.LogError($"an error occured while processing your request. Please contact your administrator for further assistance");
                    processmail = false;
                }

                if (processmail)
                {
                    Process_AnnualLeaveEmail(newAnnualRes.leaveRequestLineItems, nextApprovalLineItem);
                }
            } 
            #endregion

            response.Data = newAnnualRes;
            response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
            response.ResponseMessage = "Leave request created successfully.";
            return response;
        }
        public async Task<BaseResponse> CreateLeaveRequestLineItem(LeaveRequestLineItem leaveRequestLineItem)
        {

            _logger.LogInformation($"About to create leave request for EmployeeId: {leaveRequestLineItem.EmployeeId} and CompanyId: {leaveRequestLineItem.CompanyId}");
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            GradeLeave gradeLeave = null;
            int noOfDaysTaken = 0;

            var empLeaveRequestInfo = await _leaveRequestRepository.GetEmpLeaveInfo(leaveRequestLineItem.EmployeeId, leaveRequestLineItem.CompanyId, leaveRequestLineItem.startDate.Year.ToString());
            if (empLeaveRequestInfo == null)
            {
                empLeaveRequestInfo = await _leaveRequestRepository.CreateEmpLeaveInfo(leaveRequestLineItem.EmployeeId, leaveRequestLineItem.CompanyId);
                if (empLeaveRequestInfo == null)
                {
                    _logger.LogError($"Could not create leave request in database.");
                    response.ResponseCode = "08";
                    response.ResponseMessage = $"Could not create leave request. Please try again later or contact support for assistance";
                    return response;
                    //throw new Exception("Could not create leave request");
                }
                //  IsExistingRequest = false;
            }


            #region Validate Leave Request
            //check if any pending leave approvals

            //Check if already applied for this leavetype before
            var leaveApplicationExists = await _leaveRequestRepository.GetLeaveRequest(empLeaveRequestInfo.LeaveRequestId, leaveRequestLineItem.LeaveTypeId);
          //  var existingLeaveApproval = await _leaveApprovalRepository.GetExistingLeaveApproval(leaveRequestLineItem.EmployeeId);
            if (leaveApplicationExists != null)
            {
                var approvalStatus = leaveApplicationExists.Comments.Split(",").First().Split(" ").First();
                if (approvalStatus.Equals("Pending"))
                {
                    _logger.LogError($"A pending approval for this leavetype already exists for EmployeeId: {leaveRequestLineItem.EmployeeId} and CompanyId: {leaveRequestLineItem.CompanyId}, payload: {JsonConvert.SerializeObject(leaveApplicationExists)}");
                    response.ResponseCode = "08";
                    response.ResponseMessage = "leave detected";
                    response.Data = leaveApplicationExists;
                    return response;
                }
                else if(approvalStatus.Equals("Approved"))
                {
                    _logger.LogError($"Invalid request. Leave already applied for:{JsonConvert.SerializeObject(leaveApplicationExists)}");
                    response.ResponseCode = "08";
                    response.ResponseMessage = "You have already appliead for this leavetype";
                    response.Data = leaveApplicationExists;
                    return response;
                }
            }

           
            //Start date must fall within a weekday
            if (!IsValidWeekeday(leaveRequestLineItem.startDate))
            {
                _logger.LogWarning($"Invalid startdate specified. {leaveRequestLineItem.startDate.DayOfWeek} does not fall within a weekday");
                response.ResponseCode = "08";
                response.ResponseMessage = $"Invalid startdate specified. {leaveRequestLineItem.startDate.DayOfWeek} does not fall within a weekday";
                return response;
            }

            //End date date must fall within a weekday
            if (!IsValidWeekeday(leaveRequestLineItem.endDate))
            {
                _logger.LogWarning($"Invalid endDate specified. {leaveRequestLineItem.endDate.DayOfWeek} does not fall within a weekday");
                response.ResponseCode = "08";
                response.ResponseMessage = $"Invalid end date specified. {leaveRequestLineItem.endDate.DayOfWeek} does not fall within a weekday";
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

            //You cannot relieve yourself
            if (leaveRequestLineItem.EmployeeId == leaveRequestLineItem.RelieverUserId)
            {
                _logger.LogError("Invalid reliever specified specified. You cannot relieve yourself");
                response.ResponseCode = "08";
                response.ResponseMessage = $"Invalid reliever specified. You cannot relieve yourself";
                return response;
            }

            //Check Reliever Status
            _logger.LogInformation($"Check Reliever Status");
            var relieverLeaveRequestInfo = await _leaveRequestRepository.GetEmpLeaveInfo(leaveRequestLineItem.RelieverUserId, leaveRequestLineItem.CompanyId, leaveRequestLineItem.startDate.Year.ToString());
            if (relieverLeaveRequestInfo != null)
            {
                var leaveRequestLineItems1 = await _leaveRequestRepository.GetLeaveRequestLineItems(relieverLeaveRequestInfo.LeaveRequestId);
                var maxRelItemId = leaveRequestLineItems1.Max(x => x.LeaveRequestLineItemId);
                var overlappingLeave = leaveRequestLineItems1.Where(lr => lr.EmployeeId == leaveRequestLineItem.RelieverUserId &&
                         lr.IsApproved && // Only consider approved leaves
                         lr.startDate < leaveRequestLineItem.endDate && // End date of existing leave is after the start date of the new leave
                         lr.endDate > leaveRequestLineItem.startDate).FirstOrDefault();   // Start date of existing leave is before the end date of the new leave
            
                if (overlappingLeave != null)
                {
                    _logger.LogError($"Invalid Reliever selected. It appears that the reliever selected is still on vacation or is unavailable. see leave details: {JsonConvert.SerializeObject(overlappingLeave)}");
                    response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                    response.ResponseMessage = $"Invalid Reliever selected. It appears that the reliever selected is still on vacation, ending on {overlappingLeave.endDate}";
                    //  response.Data = repoResponse;
                    return response;
                    _logger.LogInformation($"Details of last leave taken by Reliever: {JsonConvert.SerializeObject(overlappingLeave)}");
                }
            }
            //Validate Gender
            if (ConfigSettings.leaveRequestConfig.ValidateGender)
            {
                var empGender = await _employeeRepository.GetEmployeeById(leaveRequestLineItem.EmployeeId, leaveRequestLineItem.CompanyId);
                if (empGender == null)
                {
                    _logger.LogError("We could not get the employee information while trying to validate gender");
                    response.ResponseCode = "08";
                    response.ResponseMessage = $"We could not get the employee information while trying to validate gender for the leavetype selected";
                    return response;
                }
                var LeaveTypeGender = (await _gradeLeaveRepo.GetEmployeeGradeLeaveTypes(leaveRequestLineItem.CompanyId, leaveRequestLineItem.EmployeeId)).FirstOrDefault(x => x.LeaveTypeId == leaveRequestLineItem.LeaveTypeId && x.GradeID == Convert.ToInt32(empGender.Employee.GradeID));

                if (LeaveTypeGender == null)
                {
                    _logger.LogError("We could not get the GradeLeave gender information");
                    response.ResponseCode = "08";
                    response.ResponseMessage = $"We could not get the GradeLeave gender information while trying to validate gender for the leavetype selected";
                    return response;
                }

                if (empGender.Employee.SexId != LeaveTypeGender.GenderID)
                {
                    _logger.LogError($"You cannot apply for this leave type. Invalid gender. Employee GenderID:{empGender.Employee.SexId}, GradeLeave GenderID:{LeaveTypeGender.GenderID}");
                    response.ResponseCode = "08";
                    response.ResponseMessage = $"You cannot apply for this leave type. Invalid gender";
                    return response;
                }
            }
            //else
            //{
            //    _mailService.SendLeaveApproveMailToApprover(userDetails.Employee.UnitHeadEmployeeId, payload.EmployeeId, payload.StartDate, payload.EndDate);
            //}

            ////resumption date must be greater or equal to end date
            //if (leaveRequestLineItem.ResumptionDate < leaveRequestLineItem.endDate)
            //{
            //    _logger.LogError("Invalid resumption date specified.");
            //    response.ResponseCode = "08";
            //    response.ResponseMessage = "Invalid resumption specified.";
            //    return response;
            //}

            //Validate leave length
            //int totaldays = CountWeekdays(leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);

            //if (totaldays != leaveRequestLineItem.LeaveLength)
            //{
            //    _logger.LogError($"Invalid leave length specified! The leave length is {leaveRequestLineItem.LeaveLength} but the total weekdays between {leaveRequestLineItem.startDate.ToShortDateString()} and {leaveRequestLineItem.endDate.ToShortDateString()} is {totaldays}");
            //    response.ResponseCode = "08";
            //    response.ResponseMessage = $"Invalid leave length specified! The leave length is {leaveRequestLineItem.LeaveLength} but the total weekdays between {leaveRequestLineItem.startDate.ToShortDateString()} and {leaveRequestLineItem.endDate.ToShortDateString()} is {totaldays}";
            //    return response;
            //}

            #endregion

          //  bool IsExistingRequest = true;
            try
            {
               
                {
                    var leaveRequestLineItems = await _leaveRequestRepository.GetLeaveRequestLineItems(empLeaveRequestInfo.LeaveRequestId);
                    if (leaveRequestLineItems != null)
                    {
                        if (leaveRequestLineItems.Count > 0)
                        {
                            gradeLeave = await _leaveRequestRepository.GetEmployeeGradeLeave(leaveRequestLineItem.EmployeeId, leaveRequestLineItem.LeaveTypeId);
                            _logger.LogInformation($"GradeLeave info for  {leaveRequestLineItem.EmployeeId} is {JsonConvert.SerializeObject(gradeLeave)}");

                            //Check number of days left
                            //Only sum up the days of the items that were approved
                            noOfDaysTaken = leaveRequestLineItems.Where(x => x.IsApproved == true && x.LeaveTypeId == leaveRequestLineItem.LeaveTypeId).Sum(x => x.LeaveLength);
                            _logger.LogInformation($"no. of approved days taken for {leaveRequestLineItem.EmployeeId} is {noOfDaysTaken}");
                            _logger.LogInformation($"Calculate permissible days for EmployeeId - {leaveRequestLineItem.EmployeeId}: {gradeLeave.NumbersOfDays} - ({noOfDaysTaken} + {leaveRequestLineItem.LeaveLength}) = {gradeLeave.NumbersOfDays - (noOfDaysTaken + leaveRequestLineItem.LeaveLength)}");

                            var diff = gradeLeave.NumbersOfDays - (noOfDaysTaken + leaveRequestLineItem.LeaveLength);
                            if (diff > 0)
                            {
                                _logger.LogInformation($"no. of permissable days for - {leaveRequestLineItem.EmployeeId} is {diff}");
                            }
                            else
                            {
                                _logger.LogError($"Leave length will be exceeded by {diff} days");
                            }


                            if ((noOfDaysTaken + leaveRequestLineItem.LeaveLength) > gradeLeave.NumbersOfDays)
                            {
                                _logger.LogError($"Leave length exceeded. No. of days allocated: {gradeLeave.NumbersOfDays}. No. of approved days already taken: {noOfDaysTaken}, No. of days requested: {leaveRequestLineItem.LeaveLength}");
                                response.ResponseCode = "08";
                                response.ResponseMessage = $"Leave length exceeded. No. of days allocated: {gradeLeave.NumbersOfDays}. No. of approved days already taken: {noOfDaysTaken}, No. of days requested: {leaveRequestLineItem.LeaveLength}";
                                return response;
                            }

                        }
                    }
                }
                bool CreateApproval = true;
                leaveRequestLineItem.LeaveRequestId = empLeaveRequestInfo.LeaveRequestId;
                var res = await _leaveRequestRepository.CreateLeaveRequestLineItem(leaveRequestLineItem);

                _logger.LogInformation($"Response from CreateLeaveRequest: {JsonConvert.SerializeObject(res)}");
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

                LeaveApprovalLineItem nextApprovalLineItem = null;
                LeaveApproval leaveApproval = null;
                if (CreateApproval)
                {
                    res.CompanyId = leaveRequestLineItem.CompanyId;
                    leaveApproval = await CreateLeaveApproval(res);
                    if (leaveApproval != null && leaveApproval.leaveApprovalLineItems.Count > 0)
                    {
                        leaveRequestLineItem.leaveApprovalId = leaveApproval.LeaveApprovalId;
                 
                        _logger.LogInformation($"About to get next leave approver using LeaveApprovalId: {leaveApproval.LeaveApprovalId}.");

                        nextApprovalLineItem = leaveApproval.leaveApprovalLineItems.FirstOrDefault(x => x.ApprovalStep == 1);

                        _logger.LogInformation($"response from getting next approver: {JsonConvert.SerializeObject(nextApprovalLineItem)}.");
                    }
                }

                if (nextApprovalLineItem == null || nextApprovalLineItem.LeaveApprovalId == 0)
                {
                    _logger.LogError($"Could not get next leave approver");
                    _logger.LogError($"an error occured while processing your request. Please contact your administrator for further assistance");
         
                    processmail = false;
                }

                if (processmail)
                {
                    Process_Email(leaveRequestLineItem, nextApprovalLineItem);
                }
               
                response.Data = res;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "leave request created successfully.";
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

        private async Task Process_AnnualLeaveEmail(List<LeaveRequestLineItemDto> leaveRequestLineItems, LeaveApprovalLineItem? leaveApprovalLineItem)
        {
            StringBuilder mailBody = null;
            MailRequest mailPayload = null;
            bool sendmail = true;
            try
            {
                var leaveRequestLineItem = leaveRequestLineItems.FirstOrDefault();
                //Fetch recepient details
                var requester = await _accountRepository.GetUserByEmployeeId(leaveRequestLineItem.EmployeeId);
                var approver = await _accountRepository.GetUserByEmployeeId(leaveApprovalLineItem.ApprovalEmployeeId);

                if (requester == null || approver == null)
                {
                    _logger.LogError($"Email parameters are needed: requester payload:{JsonConvert.SerializeObject(requester)}. Approver payload:{JsonConvert.SerializeObject(approver)}");
                    sendmail = false;
                }

                if (sendmail)
                {
                    var leaveType = await _leaveTypeService.GetLeaveTypeById(leaveRequestLineItem.LeaveTypeId);
                    leaveType.LeaveTypeName = leaveType.LeaveTypeName.Replace("leave", "", StringComparison.OrdinalIgnoreCase);

                    //Send mail to requester
                    mailBody = new StringBuilder();
                    mailBody.Append($"Dear <b>{requester.FirstName}</b> <br/>");
                    mailBody.Append($"Kindly note that your request for {leaveType.LeaveTypeName} leave was successfully sent for approval. The approval is currently pending on {approver.FirstName} {approver.LastName} <br/> <br/>");
                    mailBody.Append($"<p>See leave details bellow</p>");
                    int count = 1;

                    if (leaveRequestLineItems.Count > 1)
                    {
                        foreach (var item in leaveRequestLineItems)
                        {

                            mailBody.Append($"<div id='{item.LeaveRequestLineItemId}'>");
                            mailBody.Append($"<h4>Leave {count}</h4>");
                            mailBody.Append($"<b>Starts : </b> {item.startDate.ToString("dd/MM/yyyy")}  <br/> ");
                            mailBody.Append($"<b>Ends : </b> {item.endDate.ToString("dd/MM/yyyy")}   <br/> ");
                            mailBody.Append($"<b>Duration : </b> {item.LeaveLength} day(s)<br/> ");
                            mailBody.Append($"</div>");

                            count++;
                        }
                    }
                    else
                    {
                        mailBody.Append($"<div id='{leaveRequestLineItem.LeaveRequestLineItemId}'>");
                        mailBody.Append($"<h4>Details</h4>");
                        mailBody.Append($"<b>Starts : </b> {leaveRequestLineItem.startDate.ToString("dd/MM/yyyy")}  <br/> ");
                        mailBody.Append($"<b>Ends : </b> {leaveRequestLineItem.endDate.ToString("dd/MM/yyyy")}   <br/> ");
                        mailBody.Append($"<b>Duration : </b> {leaveRequestLineItem.LeaveLength} day(s)<br/> ");
                        mailBody.Append($"</div>");
                    }

                    mailPayload = new MailRequest
                    {
                        Body = mailBody.ToString(),
                        Subject = $"{leaveType.LeaveTypeName} leave Request",
                        ToEmail =  requester.OfficialMail,
                        DisplayName = "HRMS",
                        EmailTitle = $"{leaveType.LeaveTypeName} Leave Request"
                    };

                    _logger.LogInformation($"Email payload to send: {JsonConvert.SerializeObject(mailPayload)}.");
                    _mailService.SendEmailAsync(mailPayload, null);


                    //Send mail to Approver
                    mailBody = new StringBuilder();
                    mailBody.Append($"Dear {approver.FirstName} <br/> <br/>");
                    mailBody.Append($"Kindly login to approve an {leaveType.LeaveTypeName} leave request by {requester.FirstName} {requester.MiddleName}  {requester.LastName}<br/> <br/>");
                   
                    count = 1;
                    if (leaveRequestLineItems.Count > 1)
                    {
                        foreach (var item in leaveRequestLineItems)
                        {

                            mailBody.Append($"<div id='{item.LeaveRequestLineItemId}'>");
                            mailBody.Append($"<h4>Leave {count}</h4>");
                            mailBody.Append($"<b>Starts : </b> {item.startDate.ToString("dd/MM/yyyy")}  <br/> ");
                            mailBody.Append($"<b>Ends : </b> {item.endDate.ToString("dd/MM/yyyy")}   <br/> ");
                            mailBody.Append($"<b>Duration : </b> {item.LeaveLength} day(s)<br/> ");
                            mailBody.Append($"</div>");

                            count++;
                        }
                    }
                    else
                    {
                        mailBody.Append($"<div id='{leaveRequestLineItem.LeaveRequestLineItemId}'>");
                        mailBody.Append($"<h4>Details</h4>");
                        mailBody.Append($"<b>Starts : </b> {leaveRequestLineItem.startDate.ToString("dd/MM/yyyy")}  <br/> ");
                        mailBody.Append($"<b>Ends : </b> {leaveRequestLineItem.endDate.ToString("dd/MM/yyyy")}   <br/> ");
                        mailBody.Append($"<b>Duration : </b> {leaveRequestLineItem.LeaveLength} day(s)<br/> ");
                        mailBody.Append($"</div>");
                    }

                    mailPayload = new MailRequest
                    {
                        Body = mailBody.ToString(),
                        Subject = $"{leaveType.LeaveTypeName} leave Request",
                        ToEmail = approver.OfficialMail,
                        DisplayName = $"HRMS {leaveType.LeaveTypeName} leave Request",
                        EmailTitle = $"{leaveType.LeaveTypeName} Leave Request"

                    };

                    _logger.LogError($"Email payload to send: {JsonConvert.SerializeObject(mailPayload)}.");
                    _mailService.SendEmailAsync(mailPayload, null);
                }

                //  _mailService.SendLeaveApproveMailToApprover(leaveApprovalLineItem.ApprovalEmployeeId, leaveRequestLineItem.EmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private async Task Process_AnnualLeaveRescheduleEmail(List<LeaveRequestLineItem> leaveRequestLineItems, LeaveApprovalLineItem? leaveApprovalLineItem)
        {
            StringBuilder mailBody = null;
            MailRequest mailPayload = null;
            bool sendmail = true;
            try
            {
                var leaveRequestLineItem = leaveRequestLineItems.FirstOrDefault();
                //Fetch recepient details
                var requester = await _accountRepository.GetUserByEmployeeId(leaveRequestLineItem.EmployeeId);
                var approver = await _accountRepository.GetUserByEmployeeId(leaveApprovalLineItem.ApprovalEmployeeId);

                if (requester == null || approver == null)
                {
                    _logger.LogError($"Email parameters are needed: requester payload:{JsonConvert.SerializeObject(requester)}. Approver payload:{JsonConvert.SerializeObject(approver)}");
                    sendmail = false;
                }

                if (sendmail)
                {
                    var leaveType = await _leaveTypeService.GetLeaveTypeById(leaveRequestLineItem.LeaveTypeId);
                    leaveType.LeaveTypeName = leaveType.LeaveTypeName.Replace("leave", "", StringComparison.OrdinalIgnoreCase);

                    //Send mail to requester
                    mailBody = new StringBuilder();
                    mailBody.Append($"Dear <b>{requester.FirstName}</b> <br/>");
                    mailBody.Append($"Kindly note that your request to reschedule {leaveType.LeaveTypeName} leave was successfully sent for approval. The approval is currently pending on {approver.FirstName} {approver.LastName} <br/> <br/>");
                    mailBody.Append($"<p>See leave details bellow</p>");
                    int count = 1;

                    if (leaveRequestLineItems.Count > 1)
                    {
                        foreach (var item in leaveRequestLineItems)
                        {

                            mailBody.Append($"<div id='{item.LeaveRequestLineItemId}'>");
                            mailBody.Append($"<h4>Leave {count}</h4>");
                            mailBody.Append($"<b>Starts : </b> {item.startDate.ToString("dd/MM/yyyy")}  <br/> ");
                            mailBody.Append($"<b>Ends : </b> {item.endDate.ToString("dd/MM/yyyy")}   <br/> ");
                            mailBody.Append($"<b>Duration : </b> {item.LeaveLength} day(s)<br/> ");
                            mailBody.Append($"</div>");

                            count++;
                        }
                    }
                    else
                    {
                        mailBody.Append($"<div id='{leaveRequestLineItem.LeaveRequestLineItemId}'>");
                        mailBody.Append($"<h4>Details</h4>");
                        mailBody.Append($"<b>Starts : </b> {leaveRequestLineItem.startDate.ToString("dd/MM/yyyy")}  <br/> ");
                        mailBody.Append($"<b>Ends : </b> {leaveRequestLineItem.endDate.ToString("dd/MM/yyyy")}   <br/> ");
                        mailBody.Append($"<b>Duration : </b> {leaveRequestLineItem.LeaveLength} day(s)<br/> ");
                        mailBody.Append($"</div>");
                    }

                    mailPayload = new MailRequest
                    {
                        Body = mailBody.ToString(),
                        Subject = $"{leaveType.LeaveTypeName} leave Reschedule Request",
                        ToEmail = requester.OfficialMail,
                        DisplayName = "HRMS",
                        EmailTitle = $"{leaveType.LeaveTypeName} Leave Reschedule Request"
                    };

                    _logger.LogInformation($"Email payload to send: {JsonConvert.SerializeObject(mailPayload)}.");
                    _mailService.SendEmailAsync(mailPayload, null);


                    //Send mail to Approver
                    mailBody = new StringBuilder();
                    mailBody.Append($"Dear {approver.FirstName} <br/> <br/>");
                    mailBody.Append($"Kindly login to approve an {leaveType.LeaveTypeName} leave reschedule request by {requester.FirstName} {requester.MiddleName}  {requester.LastName}<br/> <br/>");

                    count = 1;
                    if (leaveRequestLineItems.Count > 1)
                    {
                        foreach (var item in leaveRequestLineItems)
                        {

                            mailBody.Append($"<div id='{item.LeaveRequestLineItemId}'>");
                            mailBody.Append($"<h4>Leave {count}</h4>");
                            mailBody.Append($"<b>Starts : </b> {item.startDate.ToString("dd/MM/yyyy")}  <br/> ");
                            mailBody.Append($"<b>Ends : </b> {item.endDate.ToString("dd/MM/yyyy")}   <br/> ");
                            mailBody.Append($"<b>Duration : </b> {item.LeaveLength} day(s)<br/> ");
                            mailBody.Append($"</div>");

                            count++;
                        }
                    }
                    else
                    {
                        mailBody.Append($"<div id='{leaveRequestLineItem.LeaveRequestLineItemId}'>");
                        mailBody.Append($"<h4>Details</h4>");
                        mailBody.Append($"<b>Starts : </b> {leaveRequestLineItem.startDate.ToString("dd/MM/yyyy")}  <br/> ");
                        mailBody.Append($"<b>Ends : </b> {leaveRequestLineItem.endDate.ToString("dd/MM/yyyy")}   <br/> ");
                        mailBody.Append($"<b>Duration : </b> {leaveRequestLineItem.LeaveLength} day(s)<br/> ");
                        mailBody.Append($"</div>");
                    }

                    mailPayload = new MailRequest
                    {
                        Body = mailBody.ToString(),
                        Subject = $"{leaveType.LeaveTypeName} Leave Reschedule Request for {requester.FirstName} {requester.FirstName}",
                        ToEmail = approver.OfficialMail,
                        DisplayName = $"HRMS {leaveType.LeaveTypeName} leave Reschedule Request",
                        EmailTitle = $"{leaveType.LeaveTypeName} Leave Reschedule Request"

                    };

                    _logger.LogError($"Email payload to send: {JsonConvert.SerializeObject(mailPayload)}.");
                    _mailService.SendEmailAsync(mailPayload, null);
                }

                //  _mailService.SendLeaveApproveMailToApprover(leaveApprovalLineItem.ApprovalEmployeeId, leaveRequestLineItem.EmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);
            }
            catch (Exception)
            {

                throw;
            }

        }
        private async Task<LeaveApproval> CreateLeaveApproval(LeaveRequestLineItem leaveRequestItem)
        {
            try
            {
                var employeeInfo = await _employeeRepository.GetEmployeeById(leaveRequestItem.EmployeeId, leaveRequestItem.CompanyId);

                long SupervisorID = employeeInfo.Employee.SupervisorID;
                long GroupHeadID = employeeInfo.Employee.GroupHeadID;
                long HR_ID = await _employeeRepository.GetHR_ID(leaveRequestItem.CompanyId, leaveRequestItem.EmployeeId);

                LeaveApproval leaveApproval = new()
                {
                    LeaveRequestLineItemId = leaveRequestItem.LeaveRequestLineItemId.Value,
                    RequiredApprovalCount = 3,
                    EmployeeID = leaveRequestItem.EmployeeId,
                    LastApprovalEmployeeID = SupervisorID,
                    Comments = "Pending on SUPERVISOR",
                    //  ApprovalEmployeeId = (int)SupervisorID,
                    //  ApprovalStatus = "Pending",
                    //  EntryDate = DateTime.Now,
                    //  ApprovalPosition = "Supervisor"
                };

                LeaveApprovalLineItem approvalsLineItem = null;

                approvalsLineItem = new LeaveApprovalLineItem();
                approvalsLineItem.ApprovalEmployeeId = SupervisorID;
                approvalsLineItem.ApprovalPosition = "SUPERVISOR";
                approvalsLineItem.Comments = "Pending on SUPERVISOR";
                approvalsLineItem.ApprovalStep = 1;
                leaveApproval.leaveApprovalLineItems.Add(approvalsLineItem);

                approvalsLineItem = new LeaveApprovalLineItem();
                approvalsLineItem.ApprovalEmployeeId = GroupHeadID;
                approvalsLineItem.ApprovalPosition = "GROUP HEAD";
                approvalsLineItem.Comments = "Pending on GROUP HEAD";
                approvalsLineItem.ApprovalStep = 2;
                leaveApproval.leaveApprovalLineItems.Add(approvalsLineItem);

                approvalsLineItem = new LeaveApprovalLineItem();
                approvalsLineItem.ApprovalEmployeeId = HR_ID;
                approvalsLineItem.ApprovalPosition = "HR";
                approvalsLineItem.Comments = "Pending on HR";
                approvalsLineItem.ApprovalStep = 3;
                leaveApproval.leaveApprovalLineItems.Add(approvalsLineItem);

                var res = await _leaveApprovalRepository.CreateLeaveApproval(leaveApproval);
                if (res != null)
                {
                    return res;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        private async Task<Approvals> CreateApproval(LeaveRequestLineItem leaveRequestItem)
        {
            var employeeInfo = await _employeeRepository.GetEmployeeById(leaveRequestItem.EmployeeId, leaveRequestItem.CompanyId);

            long SupervisorID = employeeInfo.Employee.SupervisorID;
            long GroupHeadID = employeeInfo.Employee.GroupHeadID;
            long HR_ID = await _employeeRepository.GetHR_ID(leaveRequestItem.CompanyId, leaveRequestItem.EmployeeId);

            Approvals approvals = new Approvals()
            {
                ApprovalDescription = $"Annual leave Request for {employeeInfo.Employee.FirstName} {employeeInfo.Employee.LastName} for the period of {leaveRequestItem.startDate.Year} ",
                ApprovalEmployeeID = (int)SupervisorID,
                ApprovalStatus = "Pending",
                Comment = "Pending on SUPERVISOR",
                EntryDate = DateTime.Now,
                RequiredApprovalCount = 3,
                CompanyID = (int)employeeInfo.Employee.CompanyID
                
            };

            ApprovalsLineItem approvalsLineItem = null;


            approvalsLineItem = new ApprovalsLineItem();
            approvalsLineItem.ApprovalEmployeeID = SupervisorID;
            approvalsLineItem.ApprovalPosition = "SUPERVISOR";
            approvalsLineItem.Comments = "Pending on SUPERVISOR";
            approvalsLineItem.ApprovalStep = 1;
            approvals.ApprovalsLineItems.Add(approvalsLineItem);

            approvalsLineItem = new ApprovalsLineItem();
            approvalsLineItem.ApprovalEmployeeID = GroupHeadID;
            approvalsLineItem.ApprovalPosition = "GROUP HEAD";
            approvalsLineItem.Comments = "Pending on GROUP HEAD";
            approvalsLineItem.ApprovalStep = 2;
            approvals.ApprovalsLineItems.Add(approvalsLineItem);

            approvalsLineItem = new ApprovalsLineItem();
            approvalsLineItem.ApprovalEmployeeID = HR_ID;
            approvalsLineItem.ApprovalPosition = "HR";
            approvalsLineItem.Comments = "Pending on HR";
            approvalsLineItem.ApprovalStep = 3;
            approvals.ApprovalsLineItems.Add(approvalsLineItem);

            var res = await _leaveApprovalRepository.CreateApproval(approvals);
            if (res != null)
            {
                return res;
            }
            return default;
        }

        private async Task<LeaveApproval> CreateAnnualLeaveApproval(LeaveRequestLineItemDto leaveRequestItem)
        {
            var employeeInfo = await _employeeRepository.GetEmployeeById(leaveRequestItem.EmployeeId, leaveRequestItem.CompanyId);

            long SupervisorID = employeeInfo.Employee.SupervisorID;
            long GroupHeadID = employeeInfo.Employee.GroupHeadID;
            long HR_ID = await _employeeRepository.GetHR_ID(leaveRequestItem.CompanyId, leaveRequestItem.EmployeeId);

            LeaveApproval leaveApproval = new ()
            {
                LeaveRequestLineItemId = leaveRequestItem.LeaveRequestLineItemId.Value,
                RequiredApprovalCount = 3,
                EmployeeID = leaveRequestItem.EmployeeId,
                LastApprovalEmployeeID = SupervisorID,
                Comments = "Pending on SUPERVISOR",
              //  ApprovalEmployeeId = (int)SupervisorID,
              //  ApprovalStatus = "Pending",
              //  EntryDate = DateTime.Now,
              //  ApprovalPosition = "Supervisor"
            };

            LeaveApprovalLineItem approvalsLineItem = null;

            approvalsLineItem = new LeaveApprovalLineItem();
            approvalsLineItem.ApprovalEmployeeId = SupervisorID;
            approvalsLineItem.ApprovalPosition = "SUPERVISOR";
            approvalsLineItem.Comments = "Pending on SUPERVISOR";
            approvalsLineItem.ApprovalStep = 1;
            leaveApproval.leaveApprovalLineItems.Add(approvalsLineItem);

            approvalsLineItem = new LeaveApprovalLineItem();
            approvalsLineItem.ApprovalEmployeeId = GroupHeadID;
            approvalsLineItem.ApprovalPosition = "GROUP HEAD";
            approvalsLineItem.Comments = "Pending on GROUP HEAD";
            approvalsLineItem.ApprovalStep = 2;
            leaveApproval.leaveApprovalLineItems.Add(approvalsLineItem);

            approvalsLineItem = new LeaveApprovalLineItem();
            approvalsLineItem.ApprovalEmployeeId = HR_ID;
            approvalsLineItem.ApprovalPosition = "HR";
            approvalsLineItem.Comments = "Pending on HR";
            approvalsLineItem.ApprovalStep = 3;
            leaveApproval.leaveApprovalLineItems.Add(approvalsLineItem);

            var res = await _leaveApprovalRepository.CreateAnnualLeaveApproval(leaveApproval);
            if (res != null)
            {
                return res;
            }
            return null;
        }
        public async Task<BaseResponse> RescheduleAnnualLeaveRequest(List<LeaveRequestLineItem> RequestLineItems)
        {
            var response = new BaseResponse();
            bool sendMail = true;
            //var rescheduleItems = new List<LeaveRequestLineItem>();
            int noOfDaysTaken = 0;

            var LeaveRequestLineItemId =  RequestLineItems.FirstOrDefault(x => x.LeaveRequestLineItemId != null).LeaveRequestLineItemId;
            var resp = await GetLeaveRequestLineItem(LeaveRequestLineItemId.Value);
            var requestlineItem = (LeaveRequestLineItem)resp.Data;
            var emplAnnualLeaveInfo = await _leaveRequestRepository.GetAnnualLeaveInfo(requestlineItem.AnnualLeaveId.Value);
          //  var emplAnnualLeaveInfo = await _leaveRequestRepository.GetAnnualLeaveInfo((int)singleRequestLineItem.EmployeeId, (int)singleRequestLineItem.CompanyId, DateTime.Now.Year);
            if (emplAnnualLeaveInfo == null)
            {
                _logger.LogWarning($"We could not get information on annual leave from db");
                response.ResponseCode = "500";
                response.ResponseMessage = $"We could not get information on annual leave from db";
                return response;

            }
            #region Validate Leave Request

            var leaveApprovalInfo = await _leaveApprovalRepository.GetLeaveApprovalInfo(emplAnnualLeaveInfo.ApprovalID);
            if (leaveApprovalInfo.Comments.Contains("Disapproved", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning($"Invalid request. Annual Leave already disapproved:{JsonConvert.SerializeObject(emplAnnualLeaveInfo)}");
                response.ResponseCode = "400";
                response.ResponseMessage = $"You cannot change a disapproved leave.";
                return response;
            }

            bool isApprovedExists = false;
            bool isPendingExists = false;

            //Check if approval process has already began. if so then request is invalid
            foreach (var item in leaveApprovalInfo.LeaveApprovalLineItems)
            {
                if (item.ApprovalStatus.Contains("Pending", StringComparison.OrdinalIgnoreCase))
                {
                    isPendingExists = true;
                }
                if (item.ApprovalStatus.Contains("Approved", StringComparison.OrdinalIgnoreCase))
                {
                    isApprovedExists = true;
                }
            }
            if (isApprovedExists && isPendingExists)
            {
                _logger.LogWarning($"Invalid request. The approval process has already began. Leave request cannot be re-adjusted at this time:{JsonConvert.SerializeObject(emplAnnualLeaveInfo)}");
                response.ResponseCode = "400";
                response.ResponseMessage = $"The approval process has already began. Leave request cannot be re-adjusted at this time. ";
                return response;
            }

            var leaveTypeId = emplAnnualLeaveInfo.leaveRequestLineItems.FirstOrDefault().LeaveTypeId;
            var gradeLeave = await _leaveRequestRepository.GetEmployeeGradeLeave(emplAnnualLeaveInfo.EmployeeId, leaveTypeId);
            _logger.LogInformation($"GradeLeave info for {emplAnnualLeaveInfo.EmployeeId} is {JsonConvert.SerializeObject(gradeLeave)}");

            var totalDays = RequestLineItems.Sum(x => x.LeaveLength);
            if (totalDays > gradeLeave.NumbersOfDays)
            {
                _logger.LogWarning($"Invalid request. The total number of days requested exceeds the allocated days to this Leave type. Total number of days requested - {totalDays}, total number of days allocated - {gradeLeave.NumbersOfDays}");
                response.ResponseCode = "08";
                response.ResponseMessage = $"Invalid request. Invalid request. The total number of days requested exceeds the allocated days to this Leave type. Total number of days requested - {totalDays}, total number of days allocated - {gradeLeave.NumbersOfDays}";
                return response;
            }
            
            foreach (var leaveRequestLineItem in RequestLineItems)
            {

                //Start date must fall within a weekday
                if (!IsValidWeekeday(leaveRequestLineItem.startDate))
                {
                    _logger.LogWarning($"Invalid startdate specified. {leaveRequestLineItem.startDate.DayOfWeek}: {leaveRequestLineItem.startDate} does not fall within a weekday");
                    response.ResponseCode = "08";
                    response.ResponseMessage = $"Invalid startdate specified. {leaveRequestLineItem.startDate.DayOfWeek}: {leaveRequestLineItem.startDate} does not fall within a weekday";
                    return response;
                }

                //End date date must fall within a weekday
                if (!IsValidWeekeday(leaveRequestLineItem.endDate))
                {
                    _logger.LogWarning($"Invalid endDate specified. {leaveRequestLineItem.endDate.DayOfWeek}: {leaveRequestLineItem.endDate} does not fall within a weekday");
                    response.ResponseCode = "08";
                    response.ResponseMessage = $"Invalid endDate specified. {leaveRequestLineItem.endDate.DayOfWeek}: {leaveRequestLineItem.endDate} does not fall within a weekday";
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

                //You cannot relieve yourself

                if (leaveRequestLineItem.EmployeeId == leaveRequestLineItem.RelieverUserId)
                {
                    _logger.LogError("Invalid reliever specified specified. You cannot relieve yourself");
                    response.ResponseCode = "08";
                    response.ResponseMessage = $"Invalid reliever specified. You cannot relieve yourself";
                    return response;
                }

                //Check Reliever Status
                //_logger.LogInformation($"Check Reliever Status");
                //var relieverLeaveRequestInfo = await _leaveRequestRepository.GetEmpLeaveInfo(leaveRequestLineItem.RelieverUserId, leaveRequestLineItem.CompanyId);
                //if (relieverLeaveRequestInfo != null)
                //{
                //    var leaveRequestLineItems1 = await _leaveRequestRepository.GetLeaveRequestLineItems(relieverLeaveRequestInfo.LeaveRequestId);
                //    var maxRelItemId = leaveRequestLineItems1.Max(x => x.LeaveRequestLineItemId);
                //    var lastRelLeaveTaken = leaveRequestLineItems1.FirstOrDefault(x => x.LeaveRequestLineItemId == maxRelItemId);
                //    if (lastRelLeaveTaken != null)
                //    {
                //        if (lastRelLeaveTaken.endDate.Date > leaveRequestLineItem.startDate.Date)
                //        {
                //            _logger.LogError($"Invalid Reliever selected. It appears that the reliever selected is still on vacation or is unavailable. see leave details: {JsonConvert.SerializeObject(lastRelLeaveTaken)}");
                //            response.ResponseCode = ((int)ResponseCode.Ok).ToString();
                //            response.ResponseMessage = $"Invalid Reliever selected. It appears that the reliever selected is still on vacation, ending on {lastRelLeaveTaken.endDate}";
                //            //  response.Data = repoResponse;
                //            return response;
                //        }
                //        _logger.LogInformation($"Details of last leave taken by Reliever: {JsonConvert.SerializeObject(lastRelLeaveTaken)}");
                //    }
                //}
                //Validate Gender
                if (ConfigSettings.leaveRequestConfig.ValidateGender)
                {
                    var empGender = await _employeeRepository.GetEmployeeById(leaveRequestLineItem.EmployeeId, leaveRequestLineItem.CompanyId);
                    if (empGender == null)
                    {
                        _logger.LogError("We could not get the employee information while trying to validate gender");
                        response.ResponseCode = "08";
                        response.ResponseMessage = $"We could not get the employee information while trying to validate gender for the leavetype selected";
                        return response;
                    }
                    var LeaveTypeGender = (await _gradeLeaveRepo.GetEmployeeGradeLeaveTypes(leaveRequestLineItem.CompanyId, leaveRequestLineItem.EmployeeId)).FirstOrDefault(x => x.LeaveTypeId == leaveRequestLineItem.LeaveTypeId && x.GradeID == Convert.ToInt32(empGender.Employee.GradeID));

                    if (LeaveTypeGender == null)
                    {
                        _logger.LogError("We could not get the GradeLeave gender information");
                        response.ResponseCode = "08";
                        response.ResponseMessage = $"We could not get the GradeLeave gender information while trying to validate gender for the leavetype selected";
                        return response;
                    }

                    if (empGender.Employee.SexId != LeaveTypeGender.GenderID)
                    {
                        _logger.LogError($"You cannot apply for this leave type. Invalid gender. Employee GenderID:{empGender.Employee.SexId}, GradeLeave GenderID:{LeaveTypeGender.GenderID}");
                        response.ResponseCode = "08";
                        response.ResponseMessage = $"You cannot apply for this leave type. Invalid gender";
                        return response;
                    }
                }
               
            }
            #endregion
           
            try
            {
                bool IsSuccessful = false;
                bool IsRescheduled = true;
                List<LeaveApprovalLineItem> leaveApprovalLineItems = null;
                LeaveApprovalLineItem pendingleaveapproval = null;
                bool IsValidated = false;

                var lineItem = await _leaveRequestRepository.RescheduleAnnualLeaveRequest(RequestLineItems, emplAnnualLeaveInfo);


                _logger.LogInformation($"About to get next leave approver using LeaveApprovalId: {leaveApprovalInfo.LeaveApprovalId}.");

                var nextApprovalLineItem = leaveApprovalInfo.LeaveApprovalLineItems.FirstOrDefault(x => x.ApprovalStep == 1);

                _logger.LogInformation($"response from getting next approver: {JsonConvert.SerializeObject(nextApprovalLineItem)}.");

                Process_AnnualLeaveRescheduleEmail(RequestLineItems, nextApprovalLineItem);


                response.Data = RequestLineItems;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Reschedule Leave was successful.";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: RescheduleLeaveRequest ==> {ex.Message} StackTrace: {ex.StackTrace}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: RescheduleLeaveRequest ==> {ex.Message}  StackTrace: {ex.StackTrace}";
                response.Data = null;
                return response;
            }

            return response;
        }

        public async Task<BaseResponse> RescheduleLeaveRequest(LeaveRequestLineItem leaveRequestLineItem)
        {
            var response = new BaseResponse();
            bool IsExistingRequest = false;
            try
            {
                //var empLeaveRequestInfo = await _leaveRequestRepository.GetEmpLeaveInfo(leaveRequestLineItem.EmployeeId, leaveRequestLineItem.CompanyId, leaveRequestLineItem.startDate.Year.ToString());

                //if (empLeaveRequestInfo == null)
                //{
                //    _logger.LogError($"Could not find leave request in database.");
                //    response.ResponseCode = "08";
                //    response.ResponseMessage = $"Could not find leave request. Please make sure you have already applied for leave first.";
                //    return response;
                //}

                var currentLeave = await _leaveRequestRepository.GetLeaveRequestLineItem(leaveRequestLineItem.LeaveRequestLineItemId.Value);
                #region Validate Leave Request
                
                if (currentLeave.IsApproved && leaveRequestLineItem.startDate < DateTime.Now.Date) // start date must be in the future of an approved leave request
                {
                    _logger.LogError($"You cannot adjust a scheduled leave that is in the past.");
                    response.ResponseCode = "08";
                    response.ResponseMessage = $"Start date already in the past.";
                    return response;
                }

                #region Depricated
                //var leaveRequestLineItems = await _leaveRequestRepository.GetLeaveRequestLineItems(empLeaveRequestInfo.LeaveRequestId);

                //if (leaveRequestLineItems != null && leaveRequestLineItems.Count > 0)
                //{
                //    if (leaveRequestLineItems.Count > 0)
                //    {
                //        var gradeLeave = await _leaveRequestRepository.GetEmployeeGradeLeave(leaveRequestLineItem.EmployeeId, leaveRequestLineItem.LeaveTypeId);
                //        _logger.LogInformation($"GradeLeave info for  {leaveRequestLineItem.EmployeeId} is {JsonConvert.SerializeObject(gradeLeave)}");

                //        //Check number of days left
                //        //Only sum up the days of the items that were approved
                //        var noOfDaysTaken = leaveRequestLineItems.Where(x => x.IsApproved == true && x.LeaveTypeId == leaveRequestLineItem.LeaveTypeId && x.LeaveRequestLineItemId != leaveRequestLineItem.LeaveRequestLineItemId).Sum(x => x.LeaveLength);

                //        _logger.LogInformation($"no. of approved days taken for {leaveRequestLineItem.EmployeeId} is {noOfDaysTaken}");
                //        _logger.LogInformation($"Calculate permissible days for EmployeeId - {leaveRequestLineItem.EmployeeId}: {gradeLeave.NumbersOfDays} - ({noOfDaysTaken} + {leaveRequestLineItem.LeaveLength}) = {gradeLeave.NumbersOfDays - (noOfDaysTaken + leaveRequestLineItem.LeaveLength)}");

                //        var diff = gradeLeave.NumbersOfDays - (noOfDaysTaken + leaveRequestLineItem.LeaveLength);
                //        if (diff > 0)
                //        {
                //            _logger.LogInformation($"no. of permissable days for - {leaveRequestLineItem.EmployeeId} is {diff}");
                //        }
                //        else
                //        {
                //            _logger.LogError($"Leave length will be exceeded by {diff} days");
                //        }


                //        if ((noOfDaysTaken + leaveRequestLineItem.LeaveLength) > gradeLeave.NumbersOfDays)
                //        {
                //            _logger.LogError($"Leave length exceeded. No. of days allocated: {gradeLeave.NumbersOfDays}. No. of approved days already taken: {noOfDaysTaken}, No. of days requested: {leaveRequestLineItem.LeaveLength}");
                //            response.ResponseCode = "08";
                //            response.ResponseMessage = $"Leave length exceeded. No. of days allocated: {gradeLeave.NumbersOfDays}. No. of approved days already taken: {noOfDaysTaken}, No. of days requested: {leaveRequestLineItem.LeaveLength}";
                //            return response;
                //        }
                //    }
                //}
                //else
                //{
                //    _logger.LogError($" We could not retrieve infomation about your requested leave. Please contact support for further assistance");
                //    response.ResponseCode = "08";
                //    response.ResponseMessage = $" We could not retrieve infomation about your requested leave. Please contact support for further assistance";
                //    return response;
                //} 
                #endregion


                //Check approval status
                var leaveApprovalInfo = await GetLeaveApprovalInfo(0, currentLeave.LeaveRequestLineItemId.Value);
                if (leaveApprovalInfo == null)
                {
                    _logger.LogError($" We could not retrieve infomation about your leave approval. Please contact support for further assistance");
                    response.ResponseCode = "08";
                    response.ResponseMessage = $" We could not retrieve infomation about your leave approval. Please contact support for further assistance";
                    return response;
                }
                bool isApprovedExists = false;
                bool isPendingExists = false;

                foreach (var item in leaveApprovalInfo.LeaveApprovalLineItems)
                {
                    if (item.ApprovalStatus.Contains("Pending", StringComparison.OrdinalIgnoreCase))
                    {
                        isPendingExists = true;
                    }
                    if (item.ApprovalStatus.Contains("Approved", StringComparison.OrdinalIgnoreCase))
                    {
                        isApprovedExists = true;
                    }
                }
                if (isApprovedExists && isPendingExists) //we cannot have both of these uccurence in a given request
                {
                    _logger.LogWarning($"Invalid request. The approval process has already began. Leave request cannot be re-adjusted at this time:{JsonConvert.SerializeObject(leaveApprovalInfo)}");
                    response.ResponseCode = "400";
                    response.ResponseMessage = $"The approval process has already began. Leave request cannot be re-adjusted at this time. ";
                    return response;
                }

                //Confirm that leave has not already been dissapproved
                if (leaveApprovalInfo.Comments.Contains("Disapproved", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning($"Invalid request. Leave already disapproved:{JsonConvert.SerializeObject(leaveApprovalInfo)}");
                    response.ResponseCode = "400";
                    response.ResponseMessage = $"You cannot change a disapproved leave.";
                    return response;
                }

                //Start date must fall within a weekday
                if (!IsValidWeekeday(leaveRequestLineItem.startDate))
                {
                    _logger.LogWarning($"Invalid startdate specified. {leaveRequestLineItem.startDate.DayOfWeek}: {leaveRequestLineItem.startDate} does not fall within a weekday");
                    response.ResponseCode = "08";
                    response.ResponseMessage = $"Invalid startdate specified. {leaveRequestLineItem.startDate.DayOfWeek}: {leaveRequestLineItem.startDate} does not fall within a weekday";
                    return response;
                }

                //End date date must fall within a weekday
                if (!IsValidWeekeday(leaveRequestLineItem.endDate))
                {
                    _logger.LogWarning($"Invalid endDate specified. {leaveRequestLineItem.endDate.DayOfWeek}: {leaveRequestLineItem.endDate} does not fall within a weekday");
                    response.ResponseCode = "08";
                    response.ResponseMessage = $"Invalid endDate specified. {leaveRequestLineItem.endDate.DayOfWeek}: {leaveRequestLineItem.endDate} does not fall within a weekday";
                    return response;
                }

                //startdate must come first
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

                //You cannot relieve yourself

                if (leaveRequestLineItem.EmployeeId == leaveRequestLineItem.RelieverUserId)
                {
                    _logger.LogError("Invalid reliever specified specified. You cannot relieve yourself");
                    response.ResponseCode = "08";
                    response.ResponseMessage = $"Invalid reliever specified. You cannot relieve yourself";
                    return response;
                }


                //Validate Gender
                if (ConfigSettings.leaveRequestConfig.ValidateGender)
                {
                    var empGender = await _employeeRepository.GetEmployeeById(leaveRequestLineItem.EmployeeId, leaveRequestLineItem.CompanyId);
                    if (empGender == null)
                    {
                        _logger.LogError("We could not get the employee information while trying to validate gender");
                        response.ResponseCode = "08";
                        response.ResponseMessage = $"We could not get the employee information while trying to validate gender for the leavetype selected";
                        return response;
                    }
                    var LeaveTypeGender = (await _gradeLeaveRepo.GetEmployeeGradeLeaveTypes(leaveRequestLineItem.CompanyId, leaveRequestLineItem.EmployeeId)).FirstOrDefault(x => x.LeaveTypeId == leaveRequestLineItem.LeaveTypeId && x.GradeID == Convert.ToInt32(empGender.Employee.GradeID));

                    if (LeaveTypeGender == null)
                    {
                        _logger.LogError("We could not get the GradeLeave gender information");
                        response.ResponseCode = "08";
                        response.ResponseMessage = $"We could not get the GradeLeave gender information while trying to validate gender for the leavetype selected";
                        return response;
                    }

                    if (empGender.Employee.SexId != LeaveTypeGender.GenderID)
                    {
                        _logger.LogError($"You cannot apply for this leave type. Invalid gender. Employee GenderID:{empGender.Employee.SexId}, GradeLeave GenderID:{LeaveTypeGender.GenderID}");
                        response.ResponseCode = "08";
                        response.ResponseMessage = $"You cannot apply for this leave type. Invalid gender";
                        return response;
                    }
                }

                #endregion

                var lineItem = await _leaveRequestRepository.RescheduleLeaveRequest(leaveRequestLineItem);
                Task t = new Task(() =>

                {
                    if (lineItem != null)
                    {
                        var userDetails = _accountRepository.GetUserByEmployeeId(leaveRequestLineItem.EmployeeId).Result;
                        //  var app = await _accountRepository.GetUserByEmployeeId(leaveApprovalLineItem.ApprovalEmployeeId);
                        StringBuilder mailBody = new StringBuilder();
                        mailBody.Append($"Dear {userDetails.FirstName} {userDetails.LastName} <br/> <br/>");
                        mailBody.Append($"Kindly note that you have successfully rescheduled your leave request. See details below<br/> <br/>");
                        mailBody.Append($"<b>Start Date : <b/> {leaveRequestLineItem.startDate}  <br/> ");
                        mailBody.Append($"<b>End Date : <b/> {leaveRequestLineItem.endDate}   <br/> ");

                        var mailPayload = new MailRequest
                        {
                            Body = mailBody.ToString(),
                            Subject = "Reschedule Leave Request",
                            ToEmail = userDetails.OfficialMail,
                        };

                        _logger.LogError($"Email payload to send: {mailPayload}.");
                        _mailService.SendEmailAsync(mailPayload, null);
                    }
                });

                t.Start();
                response.Data = lineItem;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Reschedule Leave was successful.";
                return response;

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
        private async Task Process_Email(LeaveRequestLineItem leaveRequestLineItem, LeaveApprovalLineItem leaveApprovalLineItem)
        {
            StringBuilder mailBody = null;
            MailRequest mailPayload = null;
            bool sendmail = true;
            //Fetch recepient details
            var requester = await _accountRepository.GetUserByEmployeeId(leaveRequestLineItem.EmployeeId);
            var approver = await _accountRepository.GetUserByEmployeeId(leaveApprovalLineItem.ApprovalEmployeeId);

            if (requester == null || approver == null)
            {
                _logger.LogError($"Email parameters are needed: requester payload:{JsonConvert.SerializeObject(requester)}. Approver payload:{JsonConvert.SerializeObject(approver)}");
                sendmail = false;
            }

            if (sendmail)
            {
                var leaveType = await _leaveTypeService.GetLeaveTypeById(leaveRequestLineItem.LeaveTypeId);
                leaveType.LeaveTypeName = leaveType.LeaveTypeName.Replace("leave", "", StringComparison.OrdinalIgnoreCase);
                //Send mail to requester
                mailBody = new StringBuilder();
                mailBody.Append($"Dear <b>{requester.FirstName} {requester.LastName} {requester.MiddleName}</b> <br/> <br/>");
                mailBody.Append($"Kindly note that your request for {leaveType.LeaveTypeName} leave was successfully created and sent for approval. The approval is currently pending on {approver.FirstName} {approver.LastName} <br/> <br/>");
                mailBody.Append($"<b>Start Date : <b/> {leaveRequestLineItem.startDate.ToString("dd/MM/yyyy")}  <br/> ");
                mailBody.Append($"<b>End Date : <b/> {leaveRequestLineItem.endDate.ToString("dd/MM/yyyy")}   <br/> ");
                mailBody.Append($"<b>Duration: <b/> {leaveRequestLineItem.LeaveLength} day(s)<br/> ");

                mailPayload = new MailRequest
                {
                    Body = mailBody.ToString(),
                    Subject = "Leave Request",
                    ToEmail = requester.OfficialMail,
                    DisplayName = "HRMS",
                    EmailTitle = $"{leaveType.LeaveTypeName} leave Request"
                };

                _logger.LogInformation($"Email payload to send: {JsonConvert.SerializeObject(mailPayload)}.");
                _mailService.SendEmailAsync(mailPayload, null);


                //Send mail to Approver
                mailBody = new StringBuilder();
                mailBody.Append($"Dear {approver.FirstName} {approver.LastName} {approver.MiddleName} <br/> <br/>");
                mailBody.Append($"Kindly login to approve a {leaveType.LeaveTypeName} leave request by {requester.FirstName} {requester.MiddleName}  {requester.LastName}<br/> <br/>");
                mailBody.Append($"<b>Start Date : <b/> {leaveRequestLineItem.startDate}  <br/> ");
                mailBody.Append($"<b>End Date : <b/> {leaveRequestLineItem.endDate}   <br/> ");

                mailPayload = new MailRequest
                {
                    Body = mailBody.ToString(),
                    Subject = $"{leaveType.LeaveTypeName} leave Request",
                    ToEmail = approver.OfficialMail,
                    DisplayName = $"{leaveType.LeaveTypeName} Leave Request",
                    EmailTitle = $"{leaveType.LeaveTypeName} leave Request"

                };

                _logger.LogError($"Email payload to send: {JsonConvert.SerializeObject(mailPayload)}.");
                _mailService.SendEmailAsync(mailPayload, null);
            }
           
            //  _mailService.SendLeaveApproveMailToApprover(leaveApprovalLineItem.ApprovalEmployeeId, leaveRequestLineItem.EmployeeId, leaveRequestLineItem.startDate, leaveRequestLineItem.endDate);

        }

        public async Task<List<AnnualLeave>> GetEmpAnnualLeaveInfoList(long employeeId, long companyId)
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

                var res = await _leaveRequestRepository.GetAnnualLeaveInfo((int)employeeId, (int)companyId);
                if (res != null)
                {
                    return res;
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetEmpAnnualLeaveInfoList ===>{ex.Message}, StackTrace: {ex.StackTrace}, Source: {ex.Source}");
                return default;
            }
        }

        public async Task<AnnualLeave> CheckAnnualLeaveInfo(LeaveRequestLineItem leaveRequestLineItem)
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

                var res = await _leaveRequestRepository.CheckAnnualLeaveInfo(leaveRequestLineItem);
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

        public async Task<AnnualLeave> GetEmpAnnualLeaveInfo(int EmployeeId, int CompanyId, int LeavePeriod)
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

                var res = await _leaveRequestRepository.GetAnnualLeaveInfo(EmployeeId, CompanyId, LeavePeriod);
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
        public async Task<AnnualLeave> GetEmpAnnualLeaveInfo(int AnnualLeaveId)
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

                var res = await _leaveRequestRepository.GetAnnualLeaveInfo(AnnualLeaveId);
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

        public async Task<EmpLeaveRequestInfo> GetEmpLeaveInfo(long employeeId, long companyId, string year)
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

                var res = await _leaveRequestRepository.GetEmpLeaveInfo(employeeId, companyId, year);
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
        public async Task<BaseResponse> GetLeaveRequestLineItem(long leaveRequestLineItemId)
        {
            try
            {
                var leaveRequestLineItem = await _leaveRequestRepository.GetLeaveRequestLineItem(leaveRequestLineItemId);
                return new BaseResponse { Data = leaveRequestLineItem, ResponseCode = "00", ResponseMessage = "LeaveRequestLineItem fetched successfully" };
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<BaseResponse> UpdateLeaveApproveLineItem(LeaveApprovalLineItem leaveApprovalLineItem)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            bool sendMail = false;
            bool sendMailToReliever = false;
            var response = new BaseResponse();
            LeaveApprovalLineItem nextApprovalLineItem = null;
            try
            {
                var repoResponse = await  _leaveApprovalRepository.UpdateLeaveApprovalLineItem(leaveApprovalLineItem);
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

                        var empLeaveRequestInfo = await _leaveRequestRepository.GetEmpLeaveInfo(leaveRequestLineItem.EmployeeId, leaveRequestLineItem.CompanyId, leaveRequestLineItem.startDate.Year.ToString());
                        var gradeLeave = await _leaveRequestRepository.GetEmployeeGradeLeave(leaveRequestLineItem.EmployeeId, leaveRequestLineItem.LeaveTypeId);
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
                        repoResponse.ApprovalStep += 1;
                        currentLeaveApprovalInfo.ApprovalStatus = $"Pending on Approval count: {repoResponse.ApprovalStep}";

                        nextApprovalLineItem = await _leaveApprovalRepository.GetLeaveApprovalLineItem(repoResponse.LeaveApprovalLineItemId, repoResponse.ApprovalStep);
                        currentLeaveApprovalInfo.ApprovalKey = nextApprovalLineItem.LeaveApprovalLineItemId;
                        sendMail = true;
                    }


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
                else if (!repoResponse.IsApproved) // Leave approval is denied
                {
                    currentLeaveApprovalInfo.ApprovalStatus = "Completed";
                    // currentLeaveApprovalInfo.ApprovalStatus = $"Denied on Approval count: {repoResponse.ApprovalStep}";
                    currentLeaveApprovalInfo.Comments = repoResponse.Comments;

                    // leaveRequestLineItem = await _leaveRequestRepository.GetLeaveRequestLineItem(currentLeaveApprovalInfo.LeaveRequestLineItemId);

                    _mailService.SendLeaveDisapproveConfirmationMail(leaveRequestLineItem.EmployeeId, repoResponse.ApprovalEmployeeId);
                }
               // currentLeaveApprovalInfo.
                var updateLeaveApproval = await _leaveApprovalRepository.UpdateLeaveApprovalInfo(currentLeaveApprovalInfo);

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
                    leaveApproval = await _leaveRequestRepository.GetLeaveApprovalInfoByRequestLineItemId(leaveReqestLineItemId);
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
        public async Task<LeaveApprovalInfo> GetLeaveApprovalByLineItem(long leaveRequestLineitemId)
        {
            try
            {
                var leaveApproval = await _leaveRequestRepository.GetLeaveApprovalInfoByRequestLineItemId(leaveRequestLineitemId);
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
        public async Task<LeaveApprovalInfo> GetLeaveApprovalInfoByRequestLineItemId(long leaveRequestLineItemId)
        {
            try
            {
                var LeaveApprovalInfo = await _leaveRequestRepository.GetLeaveApprovalInfoByRequestLineItemId(leaveRequestLineItemId);
                return LeaveApprovalInfo;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<BaseResponse> GetAllLeaveRequest(string CompanyID, DateTime? startDate, DateTime? endDate, string ApprovalPosition = "All", string approvalStatus = "All", int pageNumber = 1, int pageSize = 10)
        {
            List<EmpLeaveRequestInfo> finalRes = new List<EmpLeaveRequestInfo>();
            BaseResponse response = new BaseResponse();
            try
            {
                var leaveRequests = (await _leaveRequestRepository.GetAllLeaveRequest(CompanyID)).ToList();

                if (leaveRequests.Any())
                {
                    if (startDate == null)
                    {
                        startDate = new DateTime(DateTime.Now.Year, 1, 1);
                    }
                    else
                    {
                        startDate = startDate.Value.AddDays(-1);
                    }
                    if (endDate == null)
                    {
                        endDate = new DateTime(DateTime.Now.Year, 12, 31);
                    }
                    else
                    {
                        endDate = endDate.Value.AddDays(1);
                    }
                    foreach (var request in leaveRequests)
                    {
                        var Lineitems = request.leaveRequestLineItems.FindAll(x => x.startDate >= startDate && x.endDate <= endDate).ToList();
                        if (Lineitems.Any())
                        {
                            finalRes.Add(request); //.leaveRequestLineItems
                        }
                    }

                    if (!string.IsNullOrEmpty(approvalStatus) && finalRes.Any())
                    {
                        if (!approvalStatus.Equals("All", StringComparison.OrdinalIgnoreCase))
                        {
                            finalRes = finalRes.FindAll(x => x.ApprovalStatus.Equals(approvalStatus, StringComparison.OrdinalIgnoreCase));
                        }
                    }
                    if (!string.IsNullOrEmpty(ApprovalPosition) && finalRes.Any())
                    {
                        if (!ApprovalPosition.Equals("All", StringComparison.OrdinalIgnoreCase))
                        {
                            finalRes = finalRes.FindAll(x => x.ApprovalPosition.Equals(ApprovalPosition, StringComparison.OrdinalIgnoreCase));
                        }
                    }
                    if (finalRes.Count > 0)
                    {
                        finalRes = finalRes.OrderByDescending(x => x.DateCreated).ToList();
                    }
                    response.Data = finalRes;
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
        public async Task<BaseResponse> GetAllLeaveRequest(string CompanyID)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var leave = await _leaveRequestRepository.GetAllLeaveRequest(CompanyID);

                if (leave.Any())
                {
                    response.Data = leave.OrderByDescending(x => x.DateCreated).ToList();
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
        public async Task<List<LeaveRequestLineItemDto>> GetEmployeeLeaveRequests(long CompanyID, long EmployeeID)
        {
            try
            {
                var leave = await _leaveRequestRepository.GetEmployeeLeaveRequests(CompanyID, EmployeeID);
                leave = leave.OrderByDescending(x => x.startDate).ToList();
                return leave;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetEmployeeLeaveRequests() ==> {ex.Message}");
                return null;
            }
        }
        public async Task<List<LeaveRequestLineItemDto>> GetEmployeeLeaveRequests(long CompanyID, long EmployeeID, DateTime? startDate, DateTime? endDate, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
               
                var leaveRequests = await _leaveRequestRepository.GetEmployeeLeaveRequests(CompanyID, EmployeeID);
                if (leaveRequests.Any())
                {
                    if (startDate == null)
                    {
                        startDate = new DateTime(DateTime.Now.Year, 1, 1);
                    }
                    else
                    {
                        startDate = startDate.Value.AddDays(-1);
                    }
                    if (endDate == null)
                    {
                        endDate = new DateTime(DateTime.Now.Year, 12, 31);
                    }
                    else
                    {
                        endDate = endDate.Value.AddDays(1);
                    }

                    leaveRequests = leaveRequests.FindAll(x => x.startDate >= startDate && x.endDate <= endDate).OrderByDescending(x=>x.DateCreated).ToList();
                }
                return leaveRequests;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetEmployeeLeaveRequests() ==> {ex.Message}");
                return null;
            }
        }
        public async Task<BaseResponse> GetEmpAnnualLeaveRquestLineItems(long CompanyID)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var leave = await _leaveRequestRepository.GetAllAnnualLeaveRequestLineItems(CompanyID);

                if (leave.Any())
                {
                    response.Data = leave.OrderByDescending(x => x.endDate).ToList();
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Annual leave items fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No items returned.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllAnnualLeaveRequestLineItems() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllAnnualLeaveRequestLineItems() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
        public async Task<BaseResponse> GetAnnualLeaveRequests(long CompanyID, DateTime? startdate, DateTime? endDate, string ApprovalPosition = "All", string approvalStatus = "All", int pageNumber = 1, int pageSize = 10, string year = null)
        {
            BaseResponse response = new BaseResponse();

            if (string.IsNullOrEmpty(year))
            {
                year = DateTime.Now.ToString("yyyy");
            }
            try
            {
                var leaveRequests = await _leaveRequestRepository.GetAllAnnualLeaveRequests(CompanyID, year);

                if (leaveRequests.Any())
                {
                    List<AnnualLeaveDto> finalRes = new List<AnnualLeaveDto>();

                    if (startdate == null)
                    {
                        startdate = new DateTime(DateTime.Now.Year, 1, 1);
                    }
                    else
                    {
                        startdate = startdate.Value.AddDays(-1);
                    }
                  
                    if (endDate == null)
                    {
                        endDate = new DateTime(DateTime.Now.Year, 12, 31);
                    }
                    else
                    {
                        endDate = endDate.Value.AddDays(1);
                    }
                  

                    foreach (var request in leaveRequests)
                    {
                      var lineItems = request.leaveRequestLineItems.FindAll(x => x.startDate >= startdate && x.endDate <= endDate).ToList();
                      
                        if (lineItems.Any())
                        {
                            finalRes.Add(request); //.leaveRequestLineItems
                        }
                    }
                   
                    //foreach (var request in leaveRequests) //only add items with request greater than 0
                    //{
                        
                    //}
                 
                    if (!string.IsNullOrEmpty(approvalStatus) && finalRes.Count > 0)
                    {
                        if (!approvalStatus.Equals("All", StringComparison.OrdinalIgnoreCase))
                        {
                            finalRes = finalRes.FindAll(x => x.ApprovalStatus.Equals(approvalStatus, StringComparison.OrdinalIgnoreCase));
                        }
                    }
                 
                    if (!string.IsNullOrEmpty(ApprovalPosition) && finalRes.Count > 0)
                    {
                        if (!ApprovalPosition.Equals("All", StringComparison.OrdinalIgnoreCase))
                        {
                            finalRes = finalRes.FindAll(x => x.ApprovalPosition.Equals(ApprovalPosition, StringComparison.OrdinalIgnoreCase));
                        }
                    }
                    if (finalRes.Count > 0)
                    {
                        finalRes = finalRes.OrderByDescending(x => x.DateCreated).ToList();
                    }
                    response.Data = finalRes;

                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Annual leave items fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No items returned.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllAnnualLeaveRequestLineItems() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllAnnualLeaveRequestLineItems() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAnnualLeaveRequests(long CompanyID, string year = null)
        {
            BaseResponse response = new BaseResponse();
           
            if (string.IsNullOrEmpty(year))
            {
                year = DateTime.Now.ToString("yyyy");
            }
            try
            {
                var leave = await _leaveRequestRepository.GetAllAnnualLeaveRequests(CompanyID, year);

                if (leave.Any())
                {
                    response.Data = leave.OrderByDescending(x => x.DateCreated).ToList();
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Annual leave items fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No items returned.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllAnnualLeaveRequestLineItems() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllAnnualLeaveRequestLineItems() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllLeaveRquestLineItems(long CompanyID)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var leave = (await _leaveRequestRepository.GetAllLeaveRequestLineItems(CompanyID)).OrderByDescending(x=>x.startDate).ToList();

                if (leave.Any())
                {
                    response.Data = leave.OrderByDescending(x=>x.startDate).ToList();
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Leave request fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No Leave request found.";
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

        public Task<BaseResponse> GetEmployCumulativeForLeaveType(LeaveRequestLineItem leaveRequestLineItem)
        {
            throw new NotImplementedException();
        }

    }
}
