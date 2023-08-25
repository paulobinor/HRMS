using Com.XpressPayments.Bussiness.LearningAndDevelopmentModuleService.Service.ILogic;
using Com.XpressPayments.Bussiness.Util;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using Com.XpressPayments.Data.LearningAndDevelopmentRepository.TrainingPlanRepo;
using Com.XpressPayments.Data.LearningAndDevelopmentRepository.TrainingScheduleRepo;
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
    public class TrainingScheduleService : ITrainingScheduleService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<TrainingScheduleService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly ITrainingScheduleRepository _trainingScheduleRepository;
        private readonly IMailService _mailService;

        public TrainingScheduleService(IAccountRepository accountRepository, ILogger<TrainingScheduleService> logger,
            ITrainingScheduleRepository trainingScheduleRepository, IAuditLog audit, ICompanyRepository companyrepository, IMailService mailService)
        {
            _audit = audit;
            _mailService = mailService;
            _logger = logger;
            _accountRepository = accountRepository;
            _trainingScheduleRepository = trainingScheduleRepository;
            _companyrepository = companyrepository;
        }

        public async Task<BaseResponse> CreateTrainingSchedule(TrainingScheduleCreate payload, RequesterInfo requester)
        {
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {
                if (string.IsNullOrEmpty(payload.StaffName))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "StaffName is required";
                    return response;
                }
                if (string.IsNullOrEmpty(payload.Department))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Department is required";
                    return response;
                }
                if (string.IsNullOrEmpty(payload.TrainingMode))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "TrainingMode is required";
                    return response;
                }
                if (string.IsNullOrEmpty(payload.TrainingVenue))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "TrainingVenue skills is required";
                    return response;
                }
                if (payload.UserId < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Name is required";
                    return response;
                }

                var repoResponse = await _trainingScheduleRepository.CreateTrainingSchedule(payload);
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

        public async Task<BaseResponse> ApproveTrainingSchedule(long TrainingScheduleID, RequesterInfo requester)
        {
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {


                var repoResponse = await _trainingScheduleRepository.ApproveTrainingSchedule(TrainingScheduleID, requester.UserId);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                var trainingPlanDetails = await _trainingScheduleRepository.GetTrainingScheduleById(TrainingScheduleID);

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
        public async Task<BaseResponse> DisaproveTrainingSchedule(TrainingScheduleDisapproved payload, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {


                var repoResponse = await _trainingScheduleRepository.DisaproveTrainingSchedule(payload.TrainingScheduleID, requester.UserId, payload.Reasons_For_Disapprove);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                var trainingPlanDetails = await _trainingScheduleRepository.GetTrainingScheduleById(payload.TrainingScheduleID);

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

        public async Task<BaseResponse> GetAllTrainingSchedule(RequesterInfo requester)
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

                var trainingSchedule = await _trainingScheduleRepository.GetAllTrainingSchedule();

                if (trainingSchedule.Any())
                {
                    response.Data = trainingSchedule;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "trainingSchedule fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No trainingSchedule found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: TrainingSchedule() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: TrainingSchedule() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetTrainingSchedulebyCompanyId(long companyId, RequesterInfo requester)
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



                var TrainingSchedule = await _trainingScheduleRepository.GetTrainingScheduleByCompany(companyId);

                if (TrainingSchedule == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "TrainingSchedule not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = TrainingSchedule;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "TrainingSchedule fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetTrainingScheduleByCompanyID(long companyId) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetTrainingScheduleByCompanyID(long companyId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }

        }

        public async Task<BaseResponse> GetTrainingScheduleById(long TrainingScheduleID, RequesterInfo requester)
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

                var trainingSchedule = await _trainingScheduleRepository.GetTrainingScheduleById(TrainingScheduleID);

                if (trainingSchedule == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "trainingSchedule not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = trainingSchedule;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "trainingSchedule fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetTrainingScheduleById(long TrainingScheduleID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured:  GetTrainingScheduleById(long TrainingScheduleID  ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
    }
}
