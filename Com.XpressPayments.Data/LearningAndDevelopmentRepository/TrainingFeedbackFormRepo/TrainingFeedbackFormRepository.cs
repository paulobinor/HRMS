using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DapperGeneric;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using Com.XpressPayments.Data.LearningAndDevelopmentRepository.TrainingInductionRepo;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.LearningAndDevelopmentRepository.TrainingFeedbackFormRepo
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
                param.Add("@Status", TrainingFeedbackFormEnum.CREATESUPERVISOR );
                param.Add("@CompanyIdSup", supervisorTrainingFeedbackForm.CompanyID);
                param.Add("@UserIdSup", supervisorTrainingFeedbackForm.UserId);
                param.Add("@CourseTitleSup", supervisorTrainingFeedbackForm.CourseTitle);
                param.Add("@EmployeeNameSup", supervisorTrainingFeedbackForm.EmployeeName);
                param.Add("@DateSup", supervisorTrainingFeedbackForm.Date);
                param.Add("@DepartmentSup", supervisorTrainingFeedbackForm.Department);
                param.Add("@SupervisorName", supervisorTrainingFeedbackForm.SupervisorName);
                param.Add("@Facilitator", supervisorTrainingFeedbackForm.Facilitator);
                param.Add("@ReviewPeriod", supervisorTrainingFeedbackForm.ReviewPeriod);
                param.Add("@WhatAdditionalTrainingDevelopmentEducationDoYouRequireSup", supervisorTrainingFeedbackForm.WhatAdditionalTrainingDevelopmentEducationDoYouRequire);
                param.Add("@ThreeImpactsofTraningOnEmployee", supervisorTrainingFeedbackForm.ThreeImpactsofTraningOnEmployee);
                param.Add("@TrainingDifferenceOnEmployeeCareerGrowth", supervisorTrainingFeedbackForm.TrainingDifferenceOnEmployeeCareerGrowth);
                param.Add("@TrainingDifferenceOnEmployeeKnowledge", supervisorTrainingFeedbackForm.TrainingDifferenceOnEmployeeKnowledge);
                param.Add("@TrainingDifferenceOnEmployeeEfficiency", supervisorTrainingFeedbackForm.TrainingDifferenceOnEmployeeEfficiency);
                param.Add("@TrainingDifferenceOnEmployeeTAT", supervisorTrainingFeedbackForm.TrainingDifferenceOnEmployeeTAT);
                param.Add("@CommentsSup", supervisorTrainingFeedbackForm.Comments);
                param.Add("@Created_By_User_EmailSup", createdbyUserEmail.Trim());



                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_TrainingFeedbackForm, param, commandType: CommandType.StoredProcedure);

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
                param.Add("@Status", TrainingFeedbackFormEnum.CREATETRAINEE);
                param.Add("@CompanyIdTra", traineeTrainingFeedbackForm.CompanyID);
                param.Add("@UserIdTra", traineeTrainingFeedbackForm.UserId);
                param.Add("@CourseTitleTra", traineeTrainingFeedbackForm.CourseTitle);
                param.Add("@EmployeeNameTra", traineeTrainingFeedbackForm.EmployeeName);
                param.Add("@DateTra", traineeTrainingFeedbackForm.Date);
                param.Add("@DepartmentTra", traineeTrainingFeedbackForm.Department);
                param.Add("@Venue", traineeTrainingFeedbackForm.Venue);
                param.Add("@TrainerName", traineeTrainingFeedbackForm.TrainerName);
                param.Add("@ThreeThingsLearned", traineeTrainingFeedbackForm.ThreeThingsLearned);
                param.Add("@TrainingDifferenceOnEmployeeJob", traineeTrainingFeedbackForm.TrainingDifferenceOnEmployeeJob);
                param.Add("@WasAppropraiteMaterialCovered", traineeTrainingFeedbackForm.WasAppropraiteMaterialCovered);
                param.Add("@RateTraining_Clarity", traineeTrainingFeedbackForm.RateTraining_Clarity);
                param.Add("@RateTraining_CulturallyAppropriate", traineeTrainingFeedbackForm.RateTraining_CulturallyAppropriate);
                param.Add("@RateTraining_Expertise", traineeTrainingFeedbackForm.RateTraining_Expertise);
                param.Add("@RateTraining_Responsiveness", traineeTrainingFeedbackForm.RateTraining_Responsiveness);
                param.Add("@WhatAdditionalTrainingDevelopmentEducationDoYouRequireTra", traineeTrainingFeedbackForm.WhatAdditionalTrainingDevelopmentEducationDoYouRequire);
                param.Add("@CommentsTra", traineeTrainingFeedbackForm.Comments);
                param.Add("@Created_By_User_EmailTra", createdbyUserEmail.Trim());



                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_TrainingFeedbackForm, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateTrainingFeedbackForm ===>{ex}");
                throw;
            }
        }
    }
}
