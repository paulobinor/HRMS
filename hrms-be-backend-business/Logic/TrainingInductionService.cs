using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
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
        private readonly IAuthService _authService;
        private readonly IEmployeeRepository _employeeRepository;

        public TrainingInductionService(IAccountRepository accountRepository, ILogger<TrainingInductionService> logger,
            ITrainingInductionRepository trainingInductionRepository, IAuditLog audit, ICompanyRepository companyrepository, ILearningAndDevelopmentMailService learningAndDevelopmentmailService, IAuthService authService, IEmployeeRepository employeeRepository)
        {
            _audit = audit;
            _learningAndDevelopmentmailService = learningAndDevelopmentmailService;
            _logger = logger;
            _accountRepository = accountRepository;
            _trainingInductionRepository = trainingInductionRepository;
            _companyrepository = companyrepository;
            _authService = authService;
            _employeeRepository = employeeRepository;
        }

        public async Task<ExecutedResult<string>> CreateTrainingInduction(TrainingInductionCreate payload, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }

            bool isModelStateValidate = true;
            string validationMessage = "";
            try
            {
                if (string.IsNullOrEmpty(payload.TrainingTitle))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Training Title is required";
                }
                if (string.IsNullOrEmpty(payload.TrainingVenue))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Training Venue is required";
                }
                if (string.IsNullOrEmpty(payload.Documents))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Documents is required";
                }
                if (string.IsNullOrEmpty(payload.TrainingTime))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Training time is required";
                }
                if (string.IsNullOrEmpty(payload.TrainingProvider))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Training provider is required";
                }
                if (string.IsNullOrEmpty(payload.TrainingMode))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Training mode is required";
                }
                if (accessUser.data.UserId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  User not found";
                }
                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoResponse = await _trainingInductionRepository.CreateTrainingInduction(payload, accessUser.data.OfficialMail);
                if (repoResponse < 0)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                //Retrieve the training plan that was just created
                var trainingInduction = await _trainingInductionRepository.GetTrainingInductionById(repoResponse);

                //Send mail to Hod/UnitHead
                if (trainingInduction.UnitHeadUserID == null)
                {
                    _learningAndDevelopmentmailService.SendTrainingInductionApprovalMailToApprover(trainingInduction.HodEmployeeID, payload.EmployeeID, payload.TrainingProvider);
                }
                else
                {
                    _learningAndDevelopmentmailService.SendTrainingInductionApprovalMailToApprover(trainingInduction.UnitHeadEmployeeID, payload.EmployeeID, payload.TrainingProvider);
                }

                _logger.LogInformation("Training Induction created successfully.");
                return new ExecutedResult<string>() { responseMessage = "Training Induction Created Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"TraingnPlanService (CreateTrainingInduction)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> UpdateTrainingInduction(TrainingInductionUpdate payload, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }

            bool isModelStateValidate = true;
            string validationMessage = "";
            try
            {

                if (string.IsNullOrEmpty(payload.TrainingTitle))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Training Title is required";
                }
                if (string.IsNullOrEmpty(payload.TrainingVenue))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Training Venue is required";
                }
                if (string.IsNullOrEmpty(payload.Documents))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Documents is required";
                }
                if (string.IsNullOrEmpty(payload.TrainingTime))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Training time is required";
                }
                if (string.IsNullOrEmpty(payload.TrainingProvider))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Training provider is required";
                }
                if (string.IsNullOrEmpty(payload.TrainingMode))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Training mode is required";
                }
                if (accessUser.data.UserId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  User not found";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }

                var trainingInduction = await _trainingInductionRepository.GetTrainingInductionById(payload.TrainingInductionID);
                if (trainingInduction == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Not Found", responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }
                var repoResponse = await _trainingInductionRepository.UpdateTrainingInduction(payload, accessUser.data.OfficialMail);

                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                _logger.LogInformation("Training Induction updated successfully.");
                return new ExecutedResult<string>() { responseMessage = "Training Induction updated successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };

            }
            catch (Exception ex)
            {

                _logger.LogError($"TrainingInductionService (UpdateTrainingInduction)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> DeleteTrainingInduction(TrainingInductionDelete payload, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }

            try
            {

                var repoResponse = await _trainingInductionRepository.DeleteTrainingInduction(payload, accessUser.data.OfficialMail);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }


                _logger.LogInformation("Training Induction deleted successfully.");
                return new ExecutedResult<string>() { responseMessage = "Training Induction deleted successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"TrainingInductionService (DeleteTrainingInduction)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> ApproveTrainingInduction(long TrainingInductionID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }

            try
            {
                var trainingInductionDetail = await _trainingInductionRepository.GetTrainingInductionById(TrainingInductionID);
                var userDetails = await _employeeRepository.GetEmployeeByUserId(trainingInductionDetail.EmployeeId);
                int requesterUserId = Convert.ToInt32(accessUser.data.UserId);
                var requester = await _employeeRepository.GetEmployeeByUserId(requesterUserId);


                if (requesterUserId != userDetails.HodEmployeeId || requesterUserId != userDetails.UnitHeadEmployeeId)
                {
                    //I NEED TO GET ROLE ID FROM ACCESS USER
                    //if (Convert.ToInt32(requester.RoleId) != 4)
                    //{
                    //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    //    return response;

                    //}

                }

                var repoResponse = await _trainingInductionRepository.ApproveTrainingInduction(TrainingInductionID, requesterUserId);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                }

                //Send mail to requester
                _learningAndDevelopmentmailService.SendTrainingInductionApproveConfirmationMail(trainingInductionDetail.EmployeeId, requesterUserId, trainingInductionDetail.TrainingTitle);

                _logger.LogInformation("Training Induction approved successfully.");
                return new ExecutedResult<string>() { responseMessage = "Training Induction approved successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };


            }
            catch (Exception ex)
            {
                _logger.LogError($"TrainingInductionService (ApproveTrainingInduction)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> DisaproveTrainingInduction(TrainingInductionDisapproved payload, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {
                var trainingInductionDetail = await _trainingInductionRepository.GetTrainingInductionById(payload.TrainingInductionID);
                var userDetails = await _employeeRepository.GetEmployeeByUserId(trainingInductionDetail.EmployeeId);
                int requesterUserId = Convert.ToInt32(accessUser.data.UserId);
                var requester = await _employeeRepository.GetEmployeeByUserId(requesterUserId);

                if (userDetails == null)
                {
                    return new ExecutedResult<string>() { responseMessage = "User details not found", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
                }
                if (requesterUserId != userDetails.HodEmployeeId || requesterUserId != userDetails.UnitHeadEmployeeId)
                {
                    //if (Convert.ToInt32(requester.RoleId) != 4)
                    //{
                    //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    //    return response;

                    //}

                }

                var repoResponse = await _trainingInductionRepository.DisapproveTrainingInduction(payload.TrainingInductionID, requesterUserId, payload.Reasons_For_Disapprove);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                }


                //Send mail to requester
                _learningAndDevelopmentmailService.SendTrainingPlanDisapproveConfirmationMail(trainingInductionDetail.EmployeeId, requesterUserId);

                _logger.LogInformation("Training Induction Disapproved successfully.");
                return new ExecutedResult<string>() { responseMessage = "Training Induction Disapproved successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };


            }
            catch (Exception ex)
            {
                _logger.LogError($"TrainingInductionService (DisapproveTrainingInduction)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<TrainingInductionDTO>>> GetAllTrainingInduction(string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<IEnumerable<TrainingInductionDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {
                var trainingInductions = await _trainingInductionRepository.GetAllTrainingInduction();

                if (trainingInductions == null)
                {
                    return new ExecutedResult<IEnumerable<TrainingInductionDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Training Inductions fetched successfully.");
                return new ExecutedResult<IEnumerable<TrainingInductionDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = trainingInductions };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllActiveTrainingInduction({ex.Message}");
                return new ExecutedResult<IEnumerable<TrainingInductionDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<TrainingInductionDTO>>> GetAllActiveTrainingInduction(string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<IEnumerable<TrainingInductionDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {
                var trainingInductions = await _trainingInductionRepository.GetAllActiveTrainingInduction();

                if (trainingInductions == null)
                {
                    return new ExecutedResult<IEnumerable<TrainingInductionDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Training Inductions fetched successfully.");
                return new ExecutedResult<IEnumerable<TrainingInductionDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = trainingInductions };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllActiveTrainingInduction({ex.Message}");
                return new ExecutedResult<IEnumerable<TrainingInductionDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<TrainingInductionDTO>>> GetTrainingInductionbyCompanyId(long companyId, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<IEnumerable<TrainingInductionDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {
                var trainingInductions = await _trainingInductionRepository.GetTrainingInductionByCompany(companyId);

                if (trainingInductions == null)
                {
                    return new ExecutedResult<IEnumerable<TrainingInductionDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Training Inductions fetched successfully.");
                return new ExecutedResult<IEnumerable<TrainingInductionDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = trainingInductions };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetTrainingInductionbyCompanyId(long companyId ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<TrainingInductionDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<TrainingInductionDTO>> GetTrainingInductionById(long TrainingInductionID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<TrainingInductionDTO>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {
                var trainingInduction = await _trainingInductionRepository.GetTrainingInductionById(TrainingInductionID);

                if (trainingInduction == null)
                {
                    return new ExecutedResult<TrainingInductionDTO>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Training Inductions fetched successfully.");
                return new ExecutedResult<TrainingInductionDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = trainingInduction };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetTrainingInductionById(long TrainingInductionID ==> {ex.Message}");
                return new ExecutedResult<TrainingInductionDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<TrainingInductionDTO>>> GetTrainingInductionPendingApproval(string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<IEnumerable<TrainingInductionDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {
                var trainingInductions = await _trainingInductionRepository.GetAllActiveTrainingInduction();

                if (trainingInductions == null)
                {
                    return new ExecutedResult<IEnumerable<TrainingInductionDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Training Inductions fetched successfully.");
                return new ExecutedResult<IEnumerable<TrainingInductionDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = trainingInductions };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetTrainingInductionPendingApproval({ex.Message}");
                return new ExecutedResult<IEnumerable<TrainingInductionDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

    }
}
