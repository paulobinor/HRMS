using Com.XpressPayments.Bussiness.LearningAndDevelopmentModuleService.Service.ILogic;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using Com.XpressPayments.Data.LearningAndDevelopmentRepository.TrainingInductionRepo;
using Com.XpressPayments.Data.LearningAndDevelopmentRepository.TrainingPlanRepo;
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
    public class TrainingInductionService : ITrainingInductionService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<TrainingInductionService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly ITrainingInductionRepository _trainingInductionRepository;
        private readonly ILearningAndDevelopmentMailService _learningAndDevelopmentmailService;

        public TrainingInductionService(IAccountRepository accountRepository, ILogger<TrainingInductionService> logger,
            ITrainingInductionRepository trainingInductionRepository, IAuditLog audit, ICompanyRepository companyrepository, ILearningAndDevelopmentMailService learningAndDevelopmentmailService)
        {
            _audit = audit;
            _learningAndDevelopmentmailService = learningAndDevelopmentmailService;
            _logger = logger;
            _accountRepository = accountRepository;
            _trainingInductionRepository = trainingInductionRepository;
            _companyrepository = companyrepository;
        }

        public async Task<BaseResponse> CreateTrainingInduction(TrainingInductionCreate payload, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {
                if (string.IsNullOrEmpty(payload.TrainingTitle))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Training Title is required";
                    return response;
                }
                if (string.IsNullOrEmpty(payload.TrainingVenue))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Training venue is required";
                    return response;
                }
                if (string.IsNullOrEmpty(payload.TrainingVenue))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Training venue is required";
                    return response;
                } 
                if (string.IsNullOrEmpty(payload.TrainingVenue))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Training venue is required";
                    return response;
                } 
                if (string.IsNullOrEmpty(payload.TrainingVenue))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Training venue is required";
                    return response;
                }
                if (string.IsNullOrEmpty(payload.TrainingVenue))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Training venue is required";
                    return response;
                }
                if (requester.UserId < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Name is required";
                    return response;
                }


                var createdTrainingInductionID = await _trainingInductionRepository.CreateTrainingInduction(payload, requester.Username);
                if (createdTrainingInductionID < 0)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Error occured";
                    return response;
                }


                var trainingInduction = await _trainingInductionRepository.GetTrainingInductionById(createdTrainingInductionID);


                //Send mail to HCM
                _learningAndDevelopmentmailService.SendTrainingPlanApprovalMailToApprover(trainingInduction.HRUserId, payload.UserID, payload.TrainingProvider);

                //Send mail to Hod/UnitHead
                if (trainingInduction.UnitHeadUserID == null)
                {
                    _learningAndDevelopmentmailService.SendTrainingPlanApprovalMailToApprover(trainingInduction.HodUserID, payload.UserID, payload.TrainingProvider);
                }
                else
                {
                    _learningAndDevelopmentmailService.SendTrainingPlanApprovalMailToApprover(trainingInduction.UnitHeadUserID, payload.UserID, payload.TrainingProvider);
                }

                response.Data = payload;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Training Induction created successfully.";
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

        public async Task<BaseResponse> UpdateTrainingInduction(TrainingInductionUpdate payload, RequesterInfo requester)
        {

            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {
                var trainingInduction = await _trainingInductionRepository.GetTrainingInductionById(payload.TrainingInductionID);
                if (null == trainingInduction)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Record not found";
                    response.Data = null;
                    return response;
                }
                var repoResponse = await _trainingInductionRepository.UpdateTrainingInduction(payload, requester.Username);

                if (repoResponse < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                _logger.LogInformation("Training Induction updated successfully.");
                response.Data = payload;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Training Induction updated successfully.";
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

        public async Task<BaseResponse> DeleteTrainingInduction(TrainingInductionDelete payload, RequesterInfo requester)
        {
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {

                var repoResponse = await _trainingInductionRepository.DeleteTrainingInduction(payload, requester.Username);
                if (repoResponse < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }


                response.Data = payload;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Training induction deleted successfully.";
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


        public async Task<BaseResponse> ApproveTrainingInduction(TrainingInductionApproved payload, RequesterInfo requester)
        {
            var trainingInductionDetail = await _trainingInductionRepository.GetTrainingInductionById(payload.TrainingInductionID);
            if (trainingInductionDetail == null)
            {
                return SetErrorResponse("training Induction not found.");
            }
            var userDetails = await _accountRepository.FindUser(trainingInductionDetail.UserId);

            var response = new BaseResponse();

            try
            {

                if (userDetails == null)
                {
                    return SetErrorResponse("User details not found.");
                }

                if (payload.UserID != userDetails.HODUserId && payload.UserID != userDetails.UnitHeadUserId)
                {
                    if (trainingInductionDetail.IsHodApproved && !trainingInductionDetail.IsUnitHeadApproved)
                    {
                        return SetErrorResponse("This hasn't been approved by Staff's HOD/Unit-Head");
                    }
                    else if (Convert.ToInt32(requester.RoleId) != 4)
                    {
                        return SetErrorResponse("Your role is not authorized to carry out this action.");
                    }
                }

                var repoResponse = await _trainingInductionRepository.ApproveTrainingInduction(payload.TrainingInductionID, payload.UserID);

                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                // Send mail to requester
                _learningAndDevelopmentmailService.SendTrainingPlanApproveConfirmationMail(trainingInductionDetail.UserId, requester.UserId, trainingInductionDetail.TrainingProvider);

                response.ResponseCode = "00";
                response.ResponseMessage = "Approved successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured ==> {ex.Message}");
                return SetErrorResponse("Exception occurred");
            }
        }

        // Helper method to set error responses
        private BaseResponse SetErrorResponse(string message)
        {
            var response = new BaseResponse
            {
                ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'),
                ResponseMessage = message,
                Data = null
            };
            return response;
        }

        public async Task<BaseResponse> DisaproveTrainingInduction(TrainingInductionDisapproved payload, RequesterInfo requester)
        {
            //check if us

            var trainingInductionDetail = await _trainingInductionRepository.GetTrainingInductionById(payload.TrainingInductionID);
            var userDetails = await _accountRepository.FindUser(trainingInductionDetail.UserId);

            var response = new BaseResponse();
            try
            {
                // int requesterUserId = Convert.ToInt32(requester.UserId);

                if (userDetails == null)
                {
                    return SetErrorResponse("User details not found.");
                }

                if (payload.UserID != userDetails.HODUserId && payload.UserID != userDetails.UnitHeadUserId)
                {
                    if (Convert.ToInt32(requester.RoleId) != 4)
                    {
                        return SetErrorResponse("Your role is not authorized to carry out this action.");
                    }
                }

                var repoResponse = await _trainingInductionRepository.DisaproveTrainingInduction(payload.TrainingInductionID, payload.UserID, payload.Reasons_For_Disapprove);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }


                //Send mail to requester
                _learningAndDevelopmentmailService.SendTrainingPlanDisapproveConfirmationMail(trainingInductionDetail.UserId, requester.UserId);


                response.ResponseCode = "00";
                response.ResponseMessage = "Disapproved successfully";
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

        public async Task<BaseResponse> GetAllActiveTrainingInduction(RequesterInfo requester)
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

                var trainingInduction = await _trainingInductionRepository.GetAllActiveTrainingInduction();

                if (trainingInduction.Any())
                {
                    response.Data = trainingInduction;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "training Induction fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No training Induction found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: TrainingTrainingInductionPlan() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: TrainingTrainingInductionPlan() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllTrainingInduction(RequesterInfo requester)
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

                var trainingInduction = await _trainingInductionRepository.GetAllTrainingInduction();

                if (trainingInduction.Any())
                {
                    response.Data = trainingInduction;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "training Induction fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No training Induction found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: TrainingInduction() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: TrainingInduction() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetTrainingInductionbyCompanyId(long companyId, RequesterInfo requester)
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

                var trainingInduction = await _trainingInductionRepository.GetTrainingInductionByCompanyId(companyId);

                if (trainingInduction == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Training Induction not found.";
                    response.Data = null;
                    return response;
                }

                response.Data = trainingInduction;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Training Induction fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetTrainingInductionByCompanyID(long companyId) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetTrainingInductionByCompanyID(long companyId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetTrainingInductionById(long TrainingInductionID, RequesterInfo requester)
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

                var trainingInduction = await _trainingInductionRepository.GetTrainingInductionById(TrainingInductionID);

                if (trainingInduction == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Training Induction not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = trainingInduction;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Training Induction fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetTrainingInductionById(long TrainingInductionID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured:  GetTrainingInductionById(long TrainingInductionID  ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetTrainingInductionPendingApproval(RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                //var requesterInfo = await _accountRepository.FindUser(requester.Username);
                //if (null == requesterInfo)
                //{
                //    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = "Requester information cannot be found.";
                //    return response;
                //}

                var trainingInduction = await _trainingInductionRepository.GetTrainingInductionPendingApproval();

                if (trainingInduction.Any())
                {
                    response.Data = trainingInduction;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Training Induction fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No record found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetTrainingInductionPendingApproval() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetTrainingInductionPendingApproval() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

    
    }
}
