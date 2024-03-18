using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;


namespace hrms_be_backend_data.Repository
{
    public class TrainingPlanRepository : ITrainingPlanRepository
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<TrainingPlanRepository> _logger;
        private readonly IDapperGenericRepository _dapper;
        private readonly IConfiguration _configuration;

        public TrainingPlanRepository(IAccountRepository accountRepository, IConfiguration configuration, ILogger<TrainingPlanRepository> logger, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _accountRepository = accountRepository;
            _configuration = configuration;
            _dapper = dapper;
        }

        public async Task<dynamic> CreateTrainingPlan(TrainingPlanCreate TrainingPlan, string createdbyUserEmail)
        {
            try
            {
                
                var userDetails = await _accountRepository.FindUser(TrainingPlan.EmployeeID, null, null);
                if (userDetails == null)
                {
                    _logger.LogError($"MethodName: CreateTrainingPlan");
                    throw new Exception("User details not found.");
                }
                var param = new DynamicParameters();
                param.Add("@EmployeeId", TrainingPlan.EmployeeID);
                param.Add("@CompanyId", TrainingPlan.CompanyID);
                param.Add("@StaffName", TrainingPlan.StaffName);
                param.Add("@Department", TrainingPlan.Department);
                param.Add("@IdentifiedSkills", TrainingPlan.IdentifiedSkills);
                param.Add("@TrainingNeeds", TrainingPlan.TrainingNeeds);
                param.Add("@TrainingProvider", TrainingPlan.TrainingProvider);
                param.Add("@EstimatedCost", TrainingPlan.EstimatedCost);
                param.Add("@RequestedBy", string.Concat(userDetails.FirstName, " ", userDetails.LastName));
                param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());

                // Add an output parameter to capture the TrainingPlanID
                param.Add("@TrainingPlanIDOut", dbType: DbType.Int32, direction: ParameterDirection.Output);
                /*   return*/
                await _dapper.Get<string>("Sp_create_training_plan", param, commandType: CommandType.StoredProcedure);

                // Retrieve the TrainingPlanID from the output parameter
                int trainingPlanId = param.Get<int>("@TrainingPlanIDOut");

                return trainingPlanId;

            }
            catch (Exception ex)
            {
                _logger.LogError($"TrainingPlanRepository -> CreateTrainingPlan => {ex}");
                return "Unable to perform task, kindly contact support";
            }
        }
        public async Task<dynamic> UpdateTrainingPlan(TrainingPlanUpdate TrainingPlan, string updatedbyUserEmail)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@TrainingPlanID", TrainingPlan.TrainingPlanID);
                param.Add("@IdentifiedSkills", TrainingPlan.IdentifiedSkills);
                param.Add("@TrainingNeeds", TrainingPlan.TrainingNeeds);
                param.Add("@TrainingProvider", TrainingPlan.TrainingProvider);
                param.Add("@EstimatedCost", TrainingPlan.EstimatedCost);
                param.Add("@CompanyId", TrainingPlan.CompanyID);
                param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                dynamic response = await _dapper.Get<string>("Sp_update_training_plan", param, commandType: CommandType.StoredProcedure);

                return response;
                

            }
            catch (Exception ex)
            {
                _logger.LogError($"TrainingPlanRepository -> UpdateTrainingPlan => {ex}");
                return "Unable to perform task, kindly contact support";
            }
        }
        public async Task<dynamic> DeleteTrainingPlan(TrainingPlanDelete delete, string deletedbyUserEmail)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@TrainingPlanID", delete.TrainingPlanID);
                param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                param.Add("@Reasons_For_Delete", delete.Reasons_For_Delete == null ? "" : delete.Reasons_For_Delete.ToString().Trim());
      

                dynamic response = await _dapper.Get<string>("Sp_delete_training_plan", param, commandType: CommandType.StoredProcedure);

                return response;
              
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<dynamic> DeleteTrainingPlan(TrainingPlanDelete delete, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }
        public async Task<string> ApproveTrainingPlan(long TrainingPlanID, long ApprovedByEmployeeId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@TrainingPlanID", TrainingPlanID);
                param.Add("@ApprovedByEmployeeId", ApprovedByEmployeeId);
                param.Add("@DateApproved", DateTime.Now);

                return await _dapper.Get<string>("Sp_approve_training_plan", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"TrainingPlanRepository -> ApproveTrainingPlan => {ex}");
                return "Unable to perform task, kindly contact support";
            }
        }

        public async Task<string> DisapproveTrainingPlan(long TrainingPlanID, long DisapprovedByEmployeeId, string DisapprovedComment)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@TrainingPlanID", TrainingPlanID);
                param.Add("@DisapprovedByEmployeeId", DisapprovedByEmployeeId);
                param.Add("@DisapprovedComment", DisapprovedComment);
                param.Add("@DateDisapproved", DateTime.Now);

                return await _dapper.Get<string>("Sp_disapprove_training_plan", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"TrainingPlanRepository -> DisapproveTrainingPlan => {ex}");
                return "Unable to perform task, kindly contact support";
            }
        }

        public async Task<IEnumerable<TrainingPlanDTO>> GetAllTrainingPlan()
        {
            try
            {
                var param = new DynamicParameters();
                var TrainingPlans = await _dapper.GetAll<TrainingPlanDTO>("Sp_get_all_training_plan", param, commandType: CommandType.StoredProcedure);

                return TrainingPlans;
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllTrainingPlan() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TrainingPlanDTO>> GetAllActiveTrainingPlan()
        {
            try
            {
                    var param = new DynamicParameters();
                    var trainingPlans = await _dapper.GetAll<TrainingPlanDTO>("Sp_get_all_active_training_plan",param, commandType: CommandType.StoredProcedure);

                    return trainingPlans;
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllActiveTrainingPlan() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<TrainingPlanDTO> GetTrainingPlanById(long TrainingPlanID)
        {
            try
            {
                var param = new DynamicParameters();
  
                param.Add("@TrainingPlanID", TrainingPlanID);

                var trainingPlanDetails = await _dapper.Get<TrainingPlanDTO>("Sp_get_training_plan_by_id", param, commandType: CommandType.StoredProcedure);

                return trainingPlanDetails;
                
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<TrainingPlanDTO> GetTrainingPlanById(long TrainingPlanID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TrainingPlanDTO>> GetTrainingPlanPendingApproval()
        {
            try
            {
                var param = new DynamicParameters();
               
                var response = await _dapper.GetAll<TrainingPlanDTO>("Sp_get_training_plan_pending_approval",param, commandType: CommandType.StoredProcedure);

                return response;
                
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
                var param = new DynamicParameters();
               
                param.Add("@CompanyId", companyId);

                var trainingPlans = await _dapper.GetAll<TrainingPlanDTO>("Sp_get_training_plan_by_company", param, commandType: CommandType.StoredProcedure);

                return trainingPlans;
                
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetTrainingPlanByCompany(int companyId) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TrainingPlanDTO>> GetAllTrainingPlanByUserId(long UserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserID", UserId);
                var trainingPlans = await _dapper.GetAll<TrainingPlanDTO>("Sp_get_training_plan_by_user",param, commandType: CommandType.StoredProcedure);
                return trainingPlans;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllTrainingPlanByUserId() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> ScheduleTrainingPlan(TrainingPlanSchedule TrainingPlan, string scheduledbyUserEmail, long loggedInUserEmployeeId)
        {
            try
            {
                    var param = new DynamicParameters();
                    param.Add("@TrainingPlanID", TrainingPlan.TrainingPlanID);
                    param.Add("@TrainingOrganizer", TrainingPlan.TrainingOrganizer);
                    param.Add("@StartDate", TrainingPlan.StartDate);
                    param.Add("@EndDate", TrainingPlan.EndDate);
                    param.Add("@TrainingVenue", TrainingPlan.TrainingVenue);
                    param.Add("@TrainingTopic", TrainingPlan.TrainingTopic);
                    param.Add("@TrainingTime", TrainingPlan.TrainingTime);
                    param.Add("@TrainingMode", TrainingPlan.TrainingMode);
                    param.Add("@CompanyId", TrainingPlan.CompanyID);
                    param.Add("@Scheduled_By_User_Email", scheduledbyUserEmail.Trim());
                    param.Add("@LoggedInUserEmployeeId", loggedInUserEmployeeId);

                dynamic response = await _dapper.Get<string>("Sp_schedule_trainingplan", param, commandType: CommandType.StoredProcedure);

                    return response;
                

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: ScheduleTrainingPlan ===>{ex}");
                throw;
            }
        }
    }
}
