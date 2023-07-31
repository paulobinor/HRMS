using Com.XpressPayments.Bussiness.LeaveModuleService.Service.ILogic;
using Com.XpressPayments.Bussiness.Util;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
using Com.XpressPayments.Data.LeaveModuleRepository.LeaveRequestRepo;
using Com.XpressPayments.Data.Repositories.Company.IRepository;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.LeaveModuleService.Service.Logic
{
    public class RescheduleLeaveService : IRescheduleLeaveService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<RescheduleLeaveService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IRescheduleLeaveRepository _rescheduleLeaveRepository;
        private readonly IMailService _mailService;

        public RescheduleLeaveService(IAccountRepository accountRepository, ILogger<RescheduleLeaveService> logger,
            IRescheduleLeaveRepository rescheduleLeaveRepository, IAuditLog audit, ICompanyRepository companyrepository, IMailService mailService)
        {
            _audit = audit;
            _mailService = mailService;
            _logger = logger;
            _accountRepository = accountRepository;

            _rescheduleLeaveRepository = rescheduleLeaveRepository;
            _companyrepository = companyrepository;
        }


        public async Task<BaseResponse> CreateRescheduleLeaveRequest(RescheduleLeaveRequestCreate payload, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {
                if (string.IsNullOrEmpty(payload.Notes))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Note is required";
                    return response;
                }
                if (payload.RequestYear < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Request Year is required";
                    return response;
                }
                if (string.IsNullOrEmpty(payload.LeaveEvidence))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Leave Evidence is required";
                    return response;
                }
                if (payload.UserId < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Note is required";
                    return response;
                }

                var repoResponse = await _rescheduleLeaveRepository.CreateRescheduleLeaveRequest(payload);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }
                var userDetails = await _accountRepository.FindUser(payload.UserId);

                //Send mail to reliever
                _mailService.SendLeaveMailToReliever(payload.ReliverUserID, payload.UserId, payload.StartDate, payload.EndDate);

                //Send mail to approval
                if (userDetails.UnitHeadUserId == null)
                {
                    _mailService.SendLeaveApproveMailToApprover(userDetails.HODUserId, payload.UserId, payload.StartDate, payload.EndDate);
                }
                else
                {
                    _mailService.SendLeaveApproveMailToApprover(userDetails.UnitHeadUserId, payload.UserId, payload.StartDate, payload.EndDate);
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


        public async Task<BaseResponse> ApproveRescheduleLeaveRequest(long RescheduleLeaveRequestID, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {


                var repoResponse = await _rescheduleLeaveRepository.ApproveRescheduleLeaveRequest(RescheduleLeaveRequestID, requester.UserId);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                var leaveRequestDetail = await _rescheduleLeaveRepository.GetRescheduleLeaveRequestById(RescheduleLeaveRequestID);

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

        public async Task<BaseResponse> DisaproveRescheduleLeaveRequest(RescheduleLeaveRequestDisapproved payload, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {


                var repoResponse = await _rescheduleLeaveRepository.DisaproveRescheduleLeaveRequest(payload.RescheduleLeaveRequestID, requester.UserId, payload.Reasons_For_Disapprove);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                var leaveRequestDetail = await _rescheduleLeaveRepository.GetRescheduleLeaveRequestById(payload.RescheduleLeaveRequestID);

                var userDetails = await _accountRepository.FindUser(leaveRequestDetail.UserId);

                //Send mail to reliever
                _mailService.SendLeaveApproveConfirmationMail(leaveRequestDetail.UserId, requester.UserId, leaveRequestDetail.StartDate, leaveRequestDetail.EndDate);

                //Send mail to approval
                if (!leaveRequestDetail.IsHodApproved)
                {
                    _mailService.SendLeaveApproveMailToApprover(userDetails.HODUserId, leaveRequestDetail.UserId, leaveRequestDetail.StartDate, leaveRequestDetail.EndDate);
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

        public async Task<BaseResponse> GetAllRescheduleLeaveRquest(RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
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

                var leave = await _rescheduleLeaveRepository.GetAllRescheduleLeaveRequest();

                if (leave.Any())
                {
                    response.Data = leave;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "RescheduleLeaveRequest fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No RescheduleLeaveRequest found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllRescheduleLeaveRequest() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllRescheduleLeaveRequest() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetRescheduleLeaveRequsetById(long RescheduleLeaveRequestID, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
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

                var LeaveRequest = await _rescheduleLeaveRepository.GetRescheduleLeaveRequestById(RescheduleLeaveRequestID);

                if (LeaveRequest == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "RescheduleLeaveRequest not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = LeaveRequest;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "RescheduleLeaveRequest fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetRescheduleLeaveRequestById(long RescheduleLeaveRequestID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured:  GetRescheduleLeaveRequestById(long RescheduleLeaveRequestID  ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetRescheduleLeaveRequestbyCompanyId(string RequestYear, long companyId, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
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

                var LeaveRquest = await _rescheduleLeaveRepository.GetRescheduleLeaveRequestByCompanyId(RequestYear, companyId);

                if (LeaveRquest == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "RescheduleLeaveRequest not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = LeaveRquest;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "RescheduleLeaveRequest fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetRescheduleLeaveRequestByCompanyId(string RequestYear,long companyId) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetRescheduleLeaveRequestByCompanyId(string RequestYear, long companyId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetRescheduleLeaveRequestPendingApproval(RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                var leave = await _rescheduleLeaveRepository.GetRescheduleLeaveRequestPendingApproval(requester.UserId);

                if (leave.Any())
                {
                    response.Data = leave;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "RescheduleLeaveRequest fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No record found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetRescheduleLeaveRequestPendingApproval() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetRescheduleLeaveRequestPendingApproval() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

    }
}
