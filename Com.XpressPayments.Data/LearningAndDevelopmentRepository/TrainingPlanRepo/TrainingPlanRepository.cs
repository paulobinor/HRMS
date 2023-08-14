using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DapperGeneric;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
using Com.XpressPayments.Data.LeaveModuleRepository.LeaveRequestRepo;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.LearningAndDevelopmentRepository.TrainingPlanRepo
{
    internal class TrainingPlanRepository : ITrainingPlanRepository
    {
        private string _connectionString;
        private readonly ILogger<TrainingPlanRepository> _logger;
        private readonly IDapperGenericRepository _dapperGeneric;
        private readonly IConfiguration _configuration;

        public TrainingPlanRepository(IConfiguration configuration, ILogger<TrainingPlanRepository> logger, IDapperGenericRepository dapperGeneric)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
            _dapperGeneric = dapperGeneric;
        }

        public async Task<string> CreateTrainingPlan(TrainingPlanCreate TrainingPlan)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", TrainingPlanEnum.CREATE);
                param.Add("@UserId", TrainingPlan.UserId);
                param.Add("@Name", TrainingPlan.Name);
                param.Add("@Department", TrainingPlan.Department);
                param.Add("@IdentifiedSkills", TrainingPlan.IdentifiedSkills);
                param.Add("@TrainingNeeds", TrainingPlan.TrainingNeeds);
                param.Add("@TrainingProvider", TrainingPlan.TrainingProvider);
                param.Add("@EstimatedCost", TrainingPlan.EstimatedCost);
                param.Add("@Created_By_User_Email", TrainingPlan.Created_By_User_Email.Trim());

                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_LeaveRequest, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateTrainingPlan ===>{ex}");
                throw;
            }
        }
        public Task<string> ApproveTrainingPlan(long TrainingPlanID, long ApprovedByUserId)
        {
            throw new NotImplementedException();
        }

        public Task<string> DisaproveTrainingPlant(long TrainingPlanID, long DisapprovedByUserId, string DisapprovedComment)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrainingPlanDTO>> GetAllTrainingPlan()
        {
            throw new NotImplementedException();
        }

        public Task<TrainingPlanDTO> GetTrainingPlanByUserId(long UserId)
        {
            throw new NotImplementedException();
        }

        public Task<LeaveRequestDTO> GetTrainingPlanById(long TrainingPlanID)
        {
            throw new NotImplementedException();
        }
    }
}
