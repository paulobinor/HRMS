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
    public class TrainingFeedbackFormService : ITrainingFeedbackFormService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<TrainingFeedbackFormService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly ITrainingFeedbackFormRepository _trainingFeedbackFormRepository;
        private readonly IAuthService _authService;
        private readonly IEmployeeRepository _employeeRepository;


        public TrainingFeedbackFormService(IAccountRepository accountRepository, ILogger<TrainingFeedbackFormService> logger,
            ITrainingFeedbackFormRepository trainingFeedbackFormRepository, IAuditLog audit, ICompanyRepository companyrepository, IAuthService authService, IEmployeeRepository employeeRepository)
        {
            _audit = audit;
            _logger = logger;
            _accountRepository = accountRepository;
            _trainingFeedbackFormRepository = trainingFeedbackFormRepository;
            _companyrepository = companyrepository;
            _authService = authService;
            _employeeRepository = employeeRepository;
        }
        public async Task<ExecutedResult<string>> CreateSupervisorTrainingFeedbackForm(SupervisorTrainingFeedbackFormCreate payload, string AccessKey, string RemoteIpAddress)
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

                if (string.IsNullOrEmpty(payload.CourseTitle))
                {
                    isModelStateValidate = false;
                    validationMessage += "Course Title is required";
                }
                if (string.IsNullOrEmpty(payload.Comments))
                {
                    isModelStateValidate = false;
                    validationMessage += "Comments is required";
                }
                if (string.IsNullOrEmpty(payload.EmployeeName))
                {
                    isModelStateValidate = false;
                    validationMessage += "Employee Name is required";
                }
                if (string.IsNullOrEmpty(payload.Facilitator))
                {
                    isModelStateValidate = false;
                    validationMessage += "Facilitator is required";
                }
                if (string.IsNullOrEmpty(payload.ReviewPeriod))
                {
                    isModelStateValidate = false;
                    validationMessage += "Review Period is required";
                }
                if (string.IsNullOrEmpty(payload.SupervisorName))
                {
                    isModelStateValidate = false;
                    validationMessage += "Supervisor Name is required";
                }
                if (string.IsNullOrEmpty(payload.ThreeImpactsofTraningOnEmployee))
                {
                    isModelStateValidate = false;
                    validationMessage += "Three Impacts of Traning On Employee is required";
                }
                if (payload.TrainingDifferenceOnEmployeeCareerGrowth < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "Training Difference On Employee Career Growth is required";
                }
                if (payload.TrainingDifferenceOnEmployeeEfficiency < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "Training Difference On Employee Efficiency is required";
                }
                if (payload.TrainingDifferenceOnEmployeeKnowledge < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "Training Difference On Employee Knowledge is required";
                }
                if (payload.TrainingDifferenceOnEmployeeTAT < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "Training Difference On Employee TAT is required";
                }
                if (string.IsNullOrEmpty(payload.WhatAdditionalTrainingDevelopmentEducationDoYouRequire))
                {
                    isModelStateValidate = false;
                    validationMessage += "What Additional Training Development Education Do You Require is required";
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
                var repoResponse = await _trainingFeedbackFormRepository.CreateSupervisorTrainingFeedbackForm(payload, accessUser.data.OfficialMail);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                _logger.LogInformation("Supervisor Training Feedback form created successfully.");
                return new ExecutedResult<string>() { responseMessage = "Supervisor Training Feedback form created successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };


            }
            catch (Exception ex)
            {
                _logger.LogError($"TraingnPlanService (CreateTrainingInduction)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> CreateTraineeTrainingFeedbackForm(TraineeTrainingFeedbackFormCreate payload, string AccessKey, string RemoteIpAddress)
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

                if (string.IsNullOrEmpty(payload.CourseTitle))
                {
                    isModelStateValidate = false;
                    validationMessage += "Course Title is required";
                }
                if (string.IsNullOrEmpty(payload.Comments))
                {
                    isModelStateValidate = false;
                    validationMessage += "Comments is required";
                }
                if (string.IsNullOrEmpty(payload.Venue))
                {
                    isModelStateValidate = false;
                    validationMessage += "Venue is required";
                }
                if (string.IsNullOrEmpty(payload.TrainerName))
                {
                    isModelStateValidate = false;
                    validationMessage += "Trainer Name is required";
                }
                if (string.IsNullOrEmpty(payload.ThreeThingsLearned))
                {
                    isModelStateValidate = false;
                    validationMessage += "Three Things Learned is required";
                }
                if (string.IsNullOrEmpty(payload.EmployeeName))
                {
                    isModelStateValidate = false;
                    validationMessage += "Employee Name is required";
                }
                if (payload.RateTraining_Clarity < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "Training Clarity is required";
                }
                if (payload.RateTraining_CulturallyAppropriate < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += " Training Culturally Appropriate is required";
                }
                if (payload.RateTraining_Expertise < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += " Training Expertise is required";
                }
                if (payload.RateTraining_Responsiveness < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += " Training Responsiveness is required";
                }
                if (string.IsNullOrEmpty(payload.ThreeThingsLearned))
                {
                    isModelStateValidate = false;
                    validationMessage += "Three Things Learned is required";
                }
                if (string.IsNullOrEmpty(payload.TrainerName))
                {
                    isModelStateValidate = false;
                    validationMessage += "Trainer Name is required";
                }
                if (payload.TrainingDifferenceOnEmployeeJob < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "Training Difference On EmployeeJob is required";
                }
                if (string.IsNullOrEmpty(payload.WasAppropraiteMaterialCovered))
                {
                    isModelStateValidate = false;
                    validationMessage += "Was Appropraite Material Covered is required";
                }
                if (string.IsNullOrEmpty(payload.WhatAdditionalTrainingDevelopmentEducationDoYouRequire))
                {
                    isModelStateValidate = false;
                    validationMessage += "What Additional Training Development Education Do You Require is required";
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

                var repoResponse = await _trainingFeedbackFormRepository.CreateTraineeTrainingFeedbackForm(payload, accessUser.data.OfficialMail);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                _logger.LogInformation("Trainee Training Feedback form created successfully.");
                return new ExecutedResult<string>() { responseMessage = "Trainee Training Feedback form created successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"TraingnPlanService (CreateTrainingInduction)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }
    }
   
}
