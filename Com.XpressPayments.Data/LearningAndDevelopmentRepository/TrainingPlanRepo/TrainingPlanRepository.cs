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
using System.Data.SqlClient;
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

                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_TrainingPlan, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateTrainingPlan ===>{ex}");
                throw;
            }
        }
        public async Task<string> ApproveTrainingPlan(long TrainingPlanID, long ApprovedByUserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", 10);
                param.Add("@TrainingPlanID", TrainingPlanID);
                param.Add("@ApprovedByUserId", ApprovedByUserId);
                param.Add("@DateApproved", DateTime.Now);
                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_TrainingPlan, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: ApproveTrainingPlan ===>{ex}");
                throw;
            }
        }

        public async Task<string> DisaproveTrainingPlan(long TrainingPlanID, long DisapprovedByUserId, string DisapprovedComment)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", 11);
                param.Add("@TrainingPlanID", TrainingPlanID);
                param.Add("@DisapprovedByUserId", DisapprovedByUserId);
                param.Add("@DisapprovedComment", DisapprovedComment);
                param.Add("@DateDisapproved", DateTime.Now);
                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_TrainingPlan, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: DisaproveTrainingPlan ===>{ex}");
                throw;
            }
        }

        public async Task<IEnumerable<TrainingPlanDTO>> GetAllTrainingPlan()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingPlanEnum.GETALL);

                    var TrainingPlans = await _dapper.QueryAsync<TrainingPlanDTO>(ApplicationConstant.Sp_TrainingPlan, param: param, commandType: CommandType.StoredProcedure);

                    return TrainingPlans;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllTrainingPlan() ===>{ex.Message}");
                throw;
            }
        }


        public async Task<TrainingPlanDTO> GetTrainingPlanById(long TrainingPlanID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingPlanEnum.GETBYID);
                    param.Add("@TrainingPlanIDGet", TrainingPlanID);


                    var TrainingPlanDetails = await _dapper.QueryFirstOrDefaultAsync<TrainingPlanDTO>(ApplicationConstant.Sp_TrainingPlan, param: param, commandType: CommandType.StoredProcedure);

                    return TrainingPlanDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<TrainingPlanDTO> GetTrainingPlanById(long TrainingPlanID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteTrainingPlan(TrainingPlanDelete delete, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingPlanEnum.DELETE);
                    param.Add("@TrainingPlanIDDelete", Convert.ToInt32(delete.TrainingPlanID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Delete", delete.Reasons_For_Delete == null ? "" : delete.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_TrainingPlan, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<dynamic> DeleteTrainingPlan(TrainingPlanDelete delete, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TrainingPlanDTO>> GetTrainingPlanPendingApproval(long UserIdGet)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingPlanEnum.GETPENDINGAPPROVAL);
                    param.Add("@UserIdGet", UserIdGet);

                    var userDetails = await _dapper.QueryAsync<TrainingPlanDTO>(ApplicationConstant.Sp_TrainingPlan, param: param, commandType: CommandType.StoredProcedure);

                    return userDetails;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetTrainingPlanPendingApproval() ===>{ex.Message}");
                throw;
            }
        }
        public async Task<IEnumerable<TrainingPlanDTO>> GetTrainingPlanByCompany(long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingPlanEnum.GETBYCOMPANYID);
                    param.Add("@CompanyIdGet", companyId);

                    var TrainingPlanDetails = await _dapper.QueryAsync<TrainingPlanDTO>(ApplicationConstant.Sp_TrainingPlan, param: param, commandType: CommandType.StoredProcedure);

                    return TrainingPlanDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetTrainingPlanByCompany(int companyId) ===>{ex.Message}");
                throw;
            }
        }
    }
}
