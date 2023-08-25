using Com.XpressPayments.Bussiness.LearningAndDevelopmentModuleService.Service.ILogic;
using Com.XpressPayments.Bussiness.LeaveModuleService.Service.Logic;
using Com.XpressPayments.Data.LearningAndDevelopmentRepository.TrainingPlanRepo;
using Com.XpressPayments.Bussiness.Util;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using Com.XpressPayments.Data.Repositories.Company.IRepository;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.LearningAndDevelopmentModuleService.Service.Logic
{
    internal class TrainingPlanService : ITrainingPlanService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<TrainingPlanService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly ITrainingPlanRepository _trainingPlanRepository;
        private readonly IMailService _mailService;

        public TrainingPlanService(IAccountRepository accountRepository, ILogger<TrainingPlanService> logger,
            ITrainingPlanRepository trainingPlanRepository, IAuditLog audit, ICompanyRepository companyrepository, IMailService mailService)
        {
            _audit = audit;
            _mailService = mailService;
            _logger = logger;
            _accountRepository = accountRepository;
            _trainingPlanRepository = trainingPlanRepository;
            _companyrepository = companyrepository;
        }
        public async Task<BaseResponse> CreateTrainingPlan(TrainingPlanCreate payload, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {
                if (string.IsNullOrEmpty(payload.Name))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Name is required";
                    return response;
                }
                if (string.IsNullOrEmpty(payload.TrainingNeeds))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Training needs is required";
                    return response;
                }
                if (string.IsNullOrEmpty(payload.Department))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Department is required";
                    return response;
                }
                if (string.IsNullOrEmpty(payload.IdentifiedSkills))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "identified skills is required";
                    return response;
                }
                if (payload.UserId < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Name is required";
                    return response;
                }

                var repoResponse = await _trainingPlanRepository.CreateTrainingPlan(payload);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }
                var userDetails = await _accountRepository.FindUser(payload.UserId);


                //response.ResponseCode = "00";
                //response.ResponseMessage = "Record inserted successfully";
                //return response;

                response.Data = payload;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Training Plan created successfully.";
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

        public async Task<BaseResponse> ApproveTrainingPlan(long TrainingPlanID, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {


                var repoResponse = await _trainingPlanRepository.ApproveTrainingPlan(TrainingPlanID, requester.UserId);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                var trainingPlanDetails = await _trainingPlanRepository.GetTrainingPlanById(TrainingPlanID);

                var userDetails = await _accountRepository.FindUser(trainingPlanDetails.UserId);

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

        public async Task<BaseResponse> DisaproveTrainingPlan(TrainingPlanDisapproved payload, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {


                var repoResponse = await _trainingPlanRepository.DisaproveTrainingPlan(payload.TrainingPlanID, requester.UserId, payload.Reasons_For_Disapprove);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                var trainingPlanDetails = await _trainingPlanRepository.GetTrainingPlanById(payload.TrainingPlanID);

                var userDetails = await _accountRepository.FindUser(trainingPlanDetails.UserId);

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

        public async Task<BaseResponse> GetAllTrainingPlan(RequesterInfo requester)
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

                var trainingPlan = await _trainingPlanRepository.GetAllTrainingPlan();

                if (trainingPlan.Any())
                {
                    response.Data = trainingPlan;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "TrainingPlan fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No TrainingPlan found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: TrainingPlan() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: TrainingPlan() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetTrainingPlanById(long TrainingPlanID, RequesterInfo requester)
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

                var trainingPlan = await _trainingPlanRepository.GetTrainingPlanById(TrainingPlanID);

                if (trainingPlan == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "TrainingPlan not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = trainingPlan;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "TrainingPlan fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetTrainingPlanById(long TrainingPlanID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured:  GetTrainingPlanById(long TrainingPlanID  ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetTrainingPlanPendingApproval(RequesterInfo requester)
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

                var leave = await _trainingPlanRepository.GetTrainingPlanPendingApproval(requester.UserId);

                if (leave.Any())
                {
                    response.Data = leave;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "TrainingPlan fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No record found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetTrainingPlanPendingApproval() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetTrainingPlanPendingApproval() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
        public async Task<BaseResponse> GetTrainingPlanbyCompanyId( long companyId, RequesterInfo requester)
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



                var TrainingPlan = await _trainingPlanRepository.GetTrainingPlanByCompany(companyId);

                if (TrainingPlan == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "TrainingPlan not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = TrainingPlan;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "TrainingPlan fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetTrainingPlanByCompanyID(long companyId) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetTrainingPlanByCompanyID(long companyId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
    }
}
