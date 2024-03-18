using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.Repository;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using System.Text;

namespace hrms_be_backend_business.Logic
{
    public class TrainingPlanService : ITrainingPlanService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<TrainingPlanService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly ITrainingPlanRepository _trainingPlanRepository;
        private readonly ILearningAndDevelopmentMailService _learningAndDevelopmentmailService;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAuthService _authService;

        public TrainingPlanService(IAccountRepository accountRepository, ILogger<TrainingPlanService> logger,ITrainingPlanRepository trainingPlanRepository, IAuditLog audit, ICompanyRepository companyrepository, ILearningAndDevelopmentMailService learningAndDevelopmentmailService, IEmployeeRepository employeeRepository, IAuthService authService)
        {
            _audit = audit;
            _learningAndDevelopmentmailService = learningAndDevelopmentmailService;
            _logger = logger;
            _accountRepository = accountRepository;
            _trainingPlanRepository = trainingPlanRepository;
            _companyrepository = companyrepository;
            _employeeRepository = employeeRepository;
            _authService = authService;
        }
        public async Task<ExecutedResult<string>> CreateTrainingPlan(TrainingPlanCreate payload, string AccessKey, string RemoteIpAddress)
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
                if (string.IsNullOrEmpty(payload.TrainingNeeds))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Training Needs is required";
                }
                if (string.IsNullOrEmpty(payload.TrainingProvider))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Training Provider is required";
                }
                if (payload.EstimatedCost == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  Estimated Costs is required";
                }
                if (string.IsNullOrEmpty(payload.IdentifiedSkills))
                {
                    isModelStateValidate = false;
                    validationMessage += "  IdentifiedS kills is required";
                }
                if (string.IsNullOrEmpty(payload.Department))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Department is required";
                }
                if (payload.EmployeeID < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  Name is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                //Create a training plan and return the ID
                var repoResponse = await _trainingPlanRepository.CreateTrainingPlan(payload, accessUser.data.OfficialMail);
                if (repoResponse < 0)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                //Retrieve the training plan that was just created
                var trainingPlan = await _trainingPlanRepository.GetTrainingPlanById(repoResponse);

                //Send mail to Hod/UnitHead
                if (trainingPlan.UnitHeadUserID == null)
                {
                    _learningAndDevelopmentmailService.SendTrainingPlanApprovalMailToApprover(trainingPlan.HodEmployeeID, payload.EmployeeID, payload.TrainingProvider);
                }
                else
                {
                    _learningAndDevelopmentmailService.SendTrainingPlanApprovalMailToApprover(trainingPlan.UnitHeadEmployeeID, payload.EmployeeID, payload.TrainingProvider);
                }

                _logger.LogInformation("Training Plan created successfully.");
                return new ExecutedResult<string>() { responseMessage = "Training Plan Created Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };


            }
            catch (Exception ex)
            {
                _logger.LogError($"TraingnPlanService (CreateTrainingPlan)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> UpdateTrainingPlan(TrainingPlanUpdate payload, string AccessKey, string RemoteIpAddress)
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
                if (string.IsNullOrEmpty(payload.TrainingNeeds))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Training Needs is required";
                }
                if (string.IsNullOrEmpty(payload.TrainingProvider))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Training Provider is required";
                }
                if (payload.EstimatedCost == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  Estimated Costs is required";
                }
                if (string.IsNullOrEmpty(payload.IdentifiedSkills))
                {
                    isModelStateValidate = false;
                    validationMessage += "  IdentifiedS kills is required";
                }
                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }


                var trainingplan = await _trainingPlanRepository.GetTrainingPlanById(payload.TrainingPlanID);
                if (trainingplan == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Not Found", responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }
                var repoResponse = await _trainingPlanRepository.UpdateTrainingPlan(payload, accessUser.data.OfficialMail);

                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                _logger.LogInformation("Training Plan updated successfully.");
                return new ExecutedResult<string>() { responseMessage = "Training Plan updated successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"TrainingPlanService (UpdateTrainingPlan)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

        public async Task<ExecutedResult<string>> DeleteTrainingPlan(TrainingPlanDelete payload, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }

            try
            {

                var repoResponse = await _trainingPlanRepository.DeleteTrainingPlan(payload, accessUser.data.OfficialMail);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                _logger.LogInformation("Training Plan deleted successfully.");
                return new ExecutedResult<string>() { responseMessage = "Training Plan deleted successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"TrainingPlanService (DeleteTrainingPlan)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> ApproveTrainingPlan(TrainingPlanApproved payload, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }

            var trainingPlanDetail = await _trainingPlanRepository.GetTrainingPlanById(payload.TrainingPlanID);
            var userDetails = await _employeeRepository.GetEmployeeByUserId(trainingPlanDetail.EmployeeID);
            if (userDetails == null)
            {
                return new ExecutedResult<string>() { responseMessage = "User details not found", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
            try
            {
                int requesterUserId = Convert.ToInt32(accessUser.data.UserId);
                var requester = await _employeeRepository.GetEmployeeByUserId(requesterUserId);

                if (requesterUserId != trainingPlanDetail.HodEmployeeID || requesterUserId != userDetails.UnitHeadEmployeeId || requesterUserId !=trainingPlanDetail.HREmployeeId)
                {
                   
                   return new ExecutedResult<string>() { responseMessage = "You dont have permission to approve", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                  
                }
                if (requesterUserId == trainingPlanDetail.HodEmployeeID || !trainingPlanDetail.IsUnitHeadApproved)
                {
                    return new ExecutedResult<string>() { responseMessage = "This hasn't been approved by Staff's Unit-Head", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                }
                if (requesterUserId == trainingPlanDetail.HREmployeeId || !trainingPlanDetail.IsHodApproved)
                {
                    return new ExecutedResult<string>() { responseMessage = "This hasn't been approved by Staff's HOD", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                }

                var repoResponse = await _trainingPlanRepository.ApproveTrainingPlan(payload.TrainingPlanID, accessUser.data.UserId);

                //SENDING MAIL TO THE HOD/HR BASED ON WHO APPROVES

                if (accessUser.data.EmployeeId == trainingPlanDetail.HodEmployeeID || accessUser.data.EmployeeId == trainingPlanDetail.UnitHeadEmployeeID)
                {
                    _learningAndDevelopmentmailService.SendTrainingPlanApprovalMailToApprover(trainingPlanDetail.HrEmployeeID, trainingPlanDetail.EmployeeID, trainingPlanDetail.TrainingProvider);

                }
                else
                {
                    _learningAndDevelopmentmailService.SendTrainingPlanApprovalMailToApprover(trainingPlanDetail.HodEmployeeID, trainingPlanDetail.EmployeeID, trainingPlanDetail.TrainingProvider);

                }

                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                }

                // Send mail to requester
                _learningAndDevelopmentmailService.SendTrainingPlanApproveConfirmationMail(trainingPlanDetail.EmployeeID, accessUser.data.UserId, trainingPlanDetail.TrainingProvider);

                _logger.LogInformation("Training Plan approved successfully.");
                return new ExecutedResult<string>() { responseMessage = "Training Plan approved successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"TrainingPlanService (ApproveTrainingPlan)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> DisapproveTrainingPlan(TrainingPlanDisapproved payload, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            var trainingPlanDetail = await _trainingPlanRepository.GetTrainingPlanById(payload.TrainingPlanID);
            var userDetails = await _employeeRepository.GetEmployeeByUserId(trainingPlanDetail.EmployeeID);

            if (userDetails == null)
            {
                return new ExecutedResult<string>() { responseMessage = "User details not found", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
            try
            {
                int requesterUserId = Convert.ToInt32(accessUser.data.UserId);
                var requester = await _employeeRepository.GetEmployeeByUserId(requesterUserId);


                if (requesterUserId != trainingPlanDetail.HodEmployeeID || requesterUserId != userDetails.UnitHeadEmployeeId || requesterUserId != trainingPlanDetail.HREmployeeId)
                {

                    return new ExecutedResult<string>() { responseMessage = "You dont have permission to disapprove", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };


                }
                //if (requesterUserId == trainingPlanDetail.HodEmployeeID || trainingPlanDetail.IsUnitHeadDisapproved == false)
                //{
                //    return new ExecutedResult<string>() { responseMessage = "This hasn't been disapproved by Staff's Unit-Head", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                //}
                //if (requesterUserId == trainingPlanDetail.HREmployeeId || trainingPlanDetail.IsHodDisapproved ==false)
                //{
                //    return new ExecutedResult<string>() { responseMessage = "This hasn't been disapproved by Staff's HOD", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                //}

                var repoResponse = await _trainingPlanRepository.DisapproveTrainingPlan(payload.TrainingPlanID, accessUser.data.UserId, payload.Reasons_For_Disapprove);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                }


                //Send mail to requester
                _learningAndDevelopmentmailService.SendTrainingPlanDisapproveConfirmationMail(trainingPlanDetail.EmployeeID, accessUser.data.UserId);


                _logger.LogInformation("Training Plan Disapproved successfully.");
                return new ExecutedResult<string>() { responseMessage = "Training Plan Disapproved successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"TrainingPlanService (DisapproveTrainingPlan)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<TrainingPlanDTO>>> GetAllTrainingPlan(string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {
                var trainingPlans = await _trainingPlanRepository.GetAllTrainingPlan();

                if (trainingPlans == null)
                {
                    return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Training Plans fetched successfully.");
                return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = trainingPlans };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllTrainingPlan({ex.Message}");
                return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<TrainingPlanDTO>>> GetAllActiveTrainingPlan(string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {
                var trainingPlan = await _trainingPlanRepository.GetAllActiveTrainingPlan();

                if (trainingPlan == null)
                {
                    return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Training Plan fetched successfully.");
                return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = trainingPlan };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllActiveTrainingPlan({ex.Message}");
                return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<TrainingPlanDTO>> GetTrainingPlanById(long TrainingPlanID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<TrainingPlanDTO>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {
                var trainingPlan = await _trainingPlanRepository.GetTrainingPlanById(TrainingPlanID);

                if (trainingPlan == null)
                {
                    return new ExecutedResult<TrainingPlanDTO>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Training Plan fetched successfully.");
                return new ExecutedResult<TrainingPlanDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = trainingPlan };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetTrainingPlanById(long TrainingPlanID ==> {ex.Message}");
                return new ExecutedResult<TrainingPlanDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<TrainingPlanDTO>>> GetTrainingPlanPendingApproval(string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {
                var trainingPlan = await _trainingPlanRepository.GetTrainingPlanPendingApproval();

                if (trainingPlan == null)
                {
                    return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Training Plan fetched successfully.");
                return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = trainingPlan };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetTrainingPlanPendingApproval({ex.Message}");
                return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<TrainingPlanDTO>>> GetTrainingPlanbyCompanyId(long companyId, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {
                var trainingPlan = await _trainingPlanRepository.GetTrainingPlanByCompany(companyId);

                if (trainingPlan == null)
                {
                    return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Training Plan fetched successfully.");
                return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = trainingPlan };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetTrainingPlanbyCompanyId(long companyId ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<TrainingPlanDTO>>> GetAllTrainingPlanByUserId(long UserID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {
                var trainingPlan = await _trainingPlanRepository.GetAllTrainingPlanByUserId(UserID);

                if (trainingPlan == null)
                {
                    return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Training Plan fetched successfully.");
                return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = "Training Plan fetched Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = trainingPlan };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllTrainingPlanByUserId(long UserID ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<TrainingPlanDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> ScheduleTrainingPlan(TrainingPlanSchedule payload, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            var loggedInUserEmployeeId = accessUser.data.EmployeeId;
            try
            {
                var trainingplan = await _trainingPlanRepository.GetTrainingPlanById(payload.TrainingPlanID);

                //if (loggedInUserEmployeeId != trainingplan.HodEmployeeID || loggedInUserEmployeeId != trainingplan.HREmployeeId || loggedInUserEmployeeId != trainingplan.UnitHeadEmployeeID )
                //{
                //    return new ExecutedResult<string>() { responseMessage = $"Your role is not authorized to carry out this action.", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                //}

                
                if (null == trainingplan)
                {
                    return new ExecutedResult<string>() { responseMessage = "record not found", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                }
                var repoResponse = await _trainingPlanRepository.ScheduleTrainingPlan(payload, accessUser.data.OfficialMail, loggedInUserEmployeeId);

                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
                }

                var userDetails = await _accountRepository.FindUser(trainingplan.EmployeeID);

                _learningAndDevelopmentmailService.SendTrainingScheduleNotificationMail(userDetails.UserId, payload.TrainingOrganizer, payload.TrainingVenue, payload.TrainingMode, payload.TrainingTime, payload.TrainingTopic, payload.StartDate.ToString());


                _logger.LogInformation("Training Plan Scheduled successfully.");
                return new ExecutedResult<string>() { responseMessage = "Training Plan Scheduled successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"TrainingPlanService (ScheduleTrainingPlan)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

    }
}
