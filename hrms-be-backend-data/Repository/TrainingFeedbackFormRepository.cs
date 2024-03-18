using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class TrainingFeedbackFormRepository : ITrainingFeedbackFormRepository
    {
        private string _connectionString;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<TrainingFeedbackFormRepository> _logger;
        private readonly IDapperGenericRepository _dapperGeneric;
        private readonly IConfiguration _configuration;
        public TrainingFeedbackFormRepository(IAccountRepository accountRepository, IConfiguration configuration, ILogger<TrainingFeedbackFormRepository> logger, IDapperGenericRepository dapperGeneric)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _accountRepository = accountRepository;
            _configuration = configuration;
            _dapperGeneric = dapperGeneric;
        }
        public async Task<string> CreateSupervisorTrainingFeedbackForm(SupervisorTrainingFeedbackFormCreate supervisorTrainingFeedbackForm, string createdbyUserEmail)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", TrainingFeedbackFormEnum.CREATESUPERVISOR);
                param.Add("@CompanyId", supervisorTrainingFeedbackForm.CompanyID);
                param.Add("@EmployeeId", supervisorTrainingFeedbackForm.EmployeeId);
                param.Add("@CourseTitle", supervisorTrainingFeedbackForm.CourseTitle);
                param.Add("@EmployeeName", supervisorTrainingFeedbackForm.EmployeeName);
                param.Add("@Date", supervisorTrainingFeedbackForm.Date);
                param.Add("@DepartmentID", supervisorTrainingFeedbackForm.DepartmentID);
                param.Add("@SupervisorName", supervisorTrainingFeedbackForm.SupervisorName);
                param.Add("@Facilitator", supervisorTrainingFeedbackForm.Facilitator);
                param.Add("@ReviewPeriod", supervisorTrainingFeedbackForm.ReviewPeriod);
                param.Add("@WhatAdditionalTrainingDevelopmentEducationDoYouRequire", supervisorTrainingFeedbackForm.WhatAdditionalTrainingDevelopmentEducationDoYouRequire);
                param.Add("@ThreeImpactsofTraningOnEmployee", supervisorTrainingFeedbackForm.ThreeImpactsofTraningOnEmployee);
                param.Add("@TrainingDifferenceOnEmployeeCareerGrowth", supervisorTrainingFeedbackForm.TrainingDifferenceOnEmployeeCareerGrowth);
                param.Add("@TrainingDifferenceOnEmployeeKnowledge", supervisorTrainingFeedbackForm.TrainingDifferenceOnEmployeeKnowledge);
                param.Add("@TrainingDifferenceOnEmployeeEfficiency", supervisorTrainingFeedbackForm.TrainingDifferenceOnEmployeeEfficiency);
                param.Add("@TrainingDifferenceOnEmployeeTAT", supervisorTrainingFeedbackForm.TrainingDifferenceOnEmployeeTAT);
                param.Add("@Comments", supervisorTrainingFeedbackForm.Comments);
                param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());



                return await _dapperGeneric.Get<string>("Sp_supervisor_training_feedback_form", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateTrainingFeedbackForm ===>{ex}");
                throw;
            }
        }

        public async Task<string> CreateTraineeTrainingFeedbackForm(TraineeTrainingFeedbackFormCreate traineeTrainingFeedbackForm, string createdbyUserEmail)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyId", traineeTrainingFeedbackForm.CompanyID);
                param.Add("@EmployeeId", traineeTrainingFeedbackForm.EmployeeId);
                param.Add("@CourseTitle", traineeTrainingFeedbackForm.CourseTitle);
                param.Add("@EmployeeName", traineeTrainingFeedbackForm.EmployeeName);
                param.Add("@Date", traineeTrainingFeedbackForm.Date);
                param.Add("@DepartmentID", traineeTrainingFeedbackForm.DepartmentID);
                param.Add("@Venue", traineeTrainingFeedbackForm.Venue);
                param.Add("@TrainerName", traineeTrainingFeedbackForm.TrainerName);
                param.Add("@ThreeThingsLearned", traineeTrainingFeedbackForm.ThreeThingsLearned);
                param.Add("@TrainingDifferenceOnEmployeeJob", traineeTrainingFeedbackForm.TrainingDifferenceOnEmployeeJob);
                param.Add("@WasAppropraiteMaterialCovered", traineeTrainingFeedbackForm.WasAppropraiteMaterialCovered);
                param.Add("@RateTraining_Clarity", traineeTrainingFeedbackForm.RateTraining_Clarity);
                param.Add("@RateTraining_CulturallyAppropriate", traineeTrainingFeedbackForm.RateTraining_CulturallyAppropriate);
                param.Add("@RateTraining_Expertise", traineeTrainingFeedbackForm.RateTraining_Expertise);
                param.Add("@RateTraining_Responsiveness", traineeTrainingFeedbackForm.RateTraining_Responsiveness);
                param.Add("@WhatAdditionalTrainingDevelopmentEducationDoYouRequire", traineeTrainingFeedbackForm.WhatAdditionalTrainingDevelopmentEducationDoYouRequire);
                param.Add("@Comments", traineeTrainingFeedbackForm.Comments);
                param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());



                return await _dapperGeneric.Get<string>("Sp_trainee_training_feedback_form", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateTrainingFeedbackForm ===>{ex}");
                throw;
            }
        }
    }
}
