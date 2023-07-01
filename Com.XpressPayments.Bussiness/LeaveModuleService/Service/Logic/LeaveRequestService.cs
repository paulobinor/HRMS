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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.LeaveModuleService.Service.Logic
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<LeaveRequestService> _logger;       
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
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
                if (payload.UserId<1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Note is required";
                    return response;
                }

                var repoResponse = await _leaveRequestRepository.CreateLeaveRequest(payload);
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
                _mailService.SendLeaveApproveMailToApprover(userDetails.UnitHeadUserId,payload.UserId,payload.StartDate,payload.EndDate);  


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

                var leaveRequestDetail=await _leaveRequestRepository.GetLeaveRequestById(LeaveRequestID);

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
        public async Task<BaseResponse> DisaproveLeaveRequest(LeaveRequestDisapproved payload,  RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {


                var repoResponse = await _leaveRequestRepository.DisaproveLeaveRequest(payload.LeaveRequestID, requester.UserId,payload.Reasons_For_Disapprove);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                var leaveRequestDetail = await _leaveRequestRepository.GetLeaveRequestById(payload.LeaveRequestID);

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
        
    }
}
