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
    public class TrainingPlanService : ITrainingPlanService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<TrainingPlanService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly ITrainingPlanRepository _trainingPlanRepository;
        private readonly ILearningAndDevelopmentMailService _learningAndDevelopmentmailService;

        public TrainingPlanService(IAccountRepository accountRepository, ILogger<TrainingPlanService> logger,
            ITrainingPlanRepository trainingPlanRepository, IAuditLog audit, ICompanyRepository companyrepository, ILearningAndDevelopmentMailService learningAndDevelopmentmailService)
        {
            _audit = audit;
            _learningAndDevelopmentmailService = learningAndDevelopmentmailService;
            _logger = logger;
            _accountRepository = accountRepository;
            _trainingPlanRepository = trainingPlanRepository;
            _companyrepository = companyrepository;
        }
        public async Task<BaseResponse> CreateTrainingPlan(TrainingPlanCreate payload, RequesterInfo requester )
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {
                if (string.IsNullOrEmpty(payload.TrainingNeeds))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Training needs is required";
                    return response;
                }
                if (string.IsNullOrEmpty(payload.TrainingProvider))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Training Provider is required";
                    return response;
                } 
                if (payload.EstimatedCost == null)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Estimated cost is required";
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

                var repoResponse = await _trainingPlanRepository.CreateTrainingPlan(payload,requester.Username);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }
                var userDetails = await _accountRepository.FindUser(payload.UserId);

                var HrDetails = await _accountRepository.FindUser(4);


                //Send mail to HCM
                _learningAndDevelopmentmailService.SendTrainingPlanApprovalMailToApprover(HrDetails.UserId, payload.UserId, payload.TrainingProvider);

                //Send mail to Hod/UnitHead
                if (userDetails.UnitHeadUserId == null)
                {
                    _learningAndDevelopmentmailService.SendTrainingPlanApprovalMailToApprover(userDetails.HODUserId, payload.UserId, payload.TrainingProvider);
                }
                else
                {
                    _learningAndDevelopmentmailService.SendTrainingPlanApprovalMailToApprover(userDetails.UnitHeadUserId, payload.UserId, payload.TrainingProvider);
                }


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

        public async Task<BaseResponse> UpdateTrainingPlan(TrainingPlanUpdate payload, RequesterInfo requester)
        {
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {
                var trainingplan = await _trainingPlanRepository.GetTrainingPlanById(payload.TrainingPlanID);
                if (null == trainingplan)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Record not found";
                    response.Data = null;
                    return response;
                }
                var repoResponse = await _trainingPlanRepository.UpdateTrainingPlan(payload, requester.Username);

                if (repoResponse < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                _logger.LogInformation("Training Plan updated successfully.");
                response.Data = payload;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Training Plan updated successfully.";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occurred ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Exception occurred";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> DeleteTrainingPlan(TrainingPlanDelete payload, RequesterInfo requester)
        {
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {

                var repoResponse = await _trainingPlanRepository.DeleteTrainingPlan(payload, requester.Username);
                if (repoResponse < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }


                response.Data = payload;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Training Plan deleted successfully.";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occurred ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Exception occurred";
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
                if (Convert.ToInt32(requester.RoleId) != 2)
                {
                    if (Convert.ToInt32(requester.RoleId) != 4)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;

                    }

                }

                var repoResponse = await _trainingPlanRepository.ApproveTrainingPlan(TrainingPlanID, requester.UserId);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                var trainingPlanDetail = await _trainingPlanRepository.GetTrainingPlanById(TrainingPlanID);

                var userDetails = await _accountRepository.FindUser(trainingPlanDetail.UserId);

                //Send mail to requester
                _learningAndDevelopmentmailService.SendTrainingPlanApproveConfirmationMail(trainingPlanDetail.UserId, requester.UserId, trainingPlanDetail.TrainingProvider);

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
                if (Convert.ToInt32(requester.RoleId) != 2)
                {
                    if (Convert.ToInt32(requester.RoleId) != 4)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;

                    }

                }

                var repoResponse = await _trainingPlanRepository.DisaproveTrainingPlan(payload.TrainingPlanID, requester.UserId, payload.Reasons_For_Disapprove);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                var trainingPlanDetails = await _trainingPlanRepository.GetTrainingPlanById(payload.TrainingPlanID);

                var userDetails = await _accountRepository.FindUser(trainingPlanDetails.UserId);

                //Send mail to requester
                _learningAndDevelopmentmailService.SendTrainingPlanDisapproveConfirmationMail(trainingPlanDetails.UserId, requester.UserId);

                //Send mail to approval
                //if (!trainingPlanDetails.IsHodApproved)
                //{
                //    _learningAndDevelopmentmailService.SendTrainingPlanDisapproveConfirmationMail(userDetails.HODUserId, trainingPlanDetails.UserId);
                //}

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

        public async Task<BaseResponse> GetAllActiveTrainingPlan(RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                var requesterInfo = await _accountRepository.FindUser(requester.Username);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                var trainingPlan = await _trainingPlanRepository.GetAllActiveTrainingPlan();

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

                if (Convert.ToInt32(requester.RoleId) != 2)
                {
                    if (Convert.ToInt32(requester.RoleId) != 4)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;

                    }

                }

                var trainingPlan = await _trainingPlanRepository.GetTrainingPlanPendingApproval();

                if (trainingPlan.Any())
                {
                    response.Data = trainingPlan;
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

        public async Task<BaseResponse> GetAllTrainingPlanByUserId(long UserID, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                var requesterInfo = await _accountRepository.FindUser(requester.Username);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                var trainingPlan = await _trainingPlanRepository.GetAllTrainingPlanByUserId(UserID);

                if (trainingPlan.Any())
                {
                    response.Data = trainingPlan;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Training Plan fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No Training Plan found.";
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

        public async Task<BaseResponse> ScheduleTrainingPlan(TrainingPlanSchedule payload, RequesterInfo requester)
        {
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {

                if (Convert.ToInt32(requester.RoleId) != 2)
                {
                    if (Convert.ToInt32(requester.RoleId) != 4)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;

                    }

                }
                var trainingplan = await _trainingPlanRepository.GetTrainingPlanById(payload.TrainingPlanID);
                if (null == trainingplan)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Record not found";
                    response.Data = null;
                    return response;
                }
                var repoResponse = await _trainingPlanRepository.ScheduleTrainingPlan(payload, requester.Username);

                if (repoResponse < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                var userDetails = await _accountRepository.FindUser(trainingplan.UserId);

                _learningAndDevelopmentmailService.SendTrainingScheduleNotificationMail(userDetails.UserId, payload.TrainingOrganizer, payload.TrainingVenue, payload.TrainingMode, payload.TrainingTime, payload.TrainingTopic, payload.StartDate.ToString());


                _logger.LogInformation("Training Plan Scheduled successfully.");
                response.Data = payload;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Training Plan Scheduled successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occurred ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Exception occurred";
                response.Data = null;
                return response;
            }
        }


    }
}
