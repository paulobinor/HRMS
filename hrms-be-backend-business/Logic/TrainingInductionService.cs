using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using System.Text;

namespace hrms_be_backend_business.Logic
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

                var repoResponse = await _trainingInductionRepository.CreateTrainingInduction(payload, requester.Username);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
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


        public async Task<BaseResponse> ApproveTrainingInduction(long TrainingInductionID, RequesterInfo requester)
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

                var repoResponse = await _trainingInductionRepository.ApproveTrainingInduction(TrainingInductionID, requester.UserId);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                var trainingInductionDetail = await _trainingInductionRepository.GetTrainingInductionById(TrainingInductionID);

                var userDetails = await _accountRepository.FindUser(trainingInductionDetail.UserId);

                //Send mail to requester
                _learningAndDevelopmentmailService.SendTrainingPlanApproveConfirmationMail(trainingInductionDetail.UserId, requester.UserId, trainingInductionDetail.TrainingTitle);

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

        public async Task<BaseResponse> DisaproveTrainingInduction(TrainingInductionDisapproved payload, RequesterInfo requester)
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

                var repoResponse = await _trainingInductionRepository.DisaproveTrainingInduction(payload.TrainingInductionID, requester.UserId, payload.Reasons_For_Disapprove);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                var trainingPlanDetails = await _trainingInductionRepository.GetTrainingInductionById(payload.TrainingInductionID);

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

                var requesterInfo = await _accountRepository.FindUser(null,requesterUserEmail,null);
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

                var requesterInfo = await _accountRepository.FindUser(null,requesterUserEmail,null);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                var trainingInduction = await _trainingInductionRepository.GetTrainingInductionByCompany(companyId);

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

                var requesterInfo = await _accountRepository.FindUser(null,requesterUserEmail,null);
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
