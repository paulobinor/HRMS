using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{
    public class TrainingPlanRepository : ITrainingPlanRepository
    {
        private string _connectionString;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<TrainingPlanRepository> _logger;
        private readonly IDapperGenericRepository _dapperGeneric;
        private readonly IConfiguration _configuration;

        public TrainingPlanRepository(IAccountRepository accountRepository, IConfiguration configuration, ILogger<TrainingPlanRepository> logger, IDapperGenericRepository dapperGeneric)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _accountRepository = accountRepository;
            _configuration = configuration;
            _dapperGeneric = dapperGeneric;
        }




        public async Task<dynamic> CreateTrainingPlan(TrainingPlanCreate TrainingPlan, string createdbyUserEmail)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var userDetails = await _accountRepository.FindUser(TrainingPlan.UserId);
                    if (userDetails == null)
                    {
                        _logger.LogError($"MethodName: CreateTrainingPlan");
                        throw new Exception("User details not found.");
                    }
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingPlanEnum.CREATE);
                    param.Add("@UserId", TrainingPlan.UserId);
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
                    await _dapper.ExecuteAsync(ApplicationConstant.Sp_TrainingPlan, param, commandType: CommandType.StoredProcedure);
                    // Retrieve the TrainingPlanID from the output parameter
                    int trainingPlanId = param.Get<int>("@TrainingPlanIDOut");

                    return trainingPlanId;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateTrainingPlan ===>{ex}");
                throw;
            }
        }
        public async Task<dynamic> UpdateTrainingPlan(TrainingPlanUpdate TrainingPlan, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingPlanEnum.UPDATE);
                    param.Add("@TrainingPlanIDUpd", TrainingPlan.TrainingPlanID);
                    param.Add("@IdentifiedSkillsUpd", TrainingPlan.IdentifiedSkills);
                    param.Add("@TrainingNeedsUpd", TrainingPlan.TrainingNeeds);
                    param.Add("@TrainingProviderUpd", TrainingPlan.TrainingProvider);
                    param.Add("@EstimatedCostUpd", TrainingPlan.EstimatedCost);
                    param.Add("@CompanyIdUpd", TrainingPlan.CompanyID);
                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());
                    //creaating an unused parameter called @TrainingPlanIDOut to avoid error
                    param.Add("@TrainingPlanIDOut", dbType: DbType.Int64, direction: ParameterDirection.Output);

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_TrainingPlan, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: UpdateTrainingPlan ===>{ex}");
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
                    param.Add("@TrainingPlanIDDelete", delete.TrainingPlanID);
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Delete", delete.Reasons_For_Delete == null ? "" : delete.Reasons_For_Delete.ToString().Trim());
                    //creaating an unused parameter called @TrainingPlanIDOut to avoid error
                    param.Add("@TrainingPlanIDOut", dbType: DbType.Int64, direction: ParameterDirection.Output);

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
        public async Task<string> ApproveTrainingPlan(long TrainingPlanID, long ApprovedByUserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", 10);
                param.Add("@TrainingPlanID", TrainingPlanID);
                param.Add("@ApprovedByUserId", ApprovedByUserId);
                param.Add("@DateApproved", DateTime.Now);
                //creaating an unused parameter called @TrainingPlanIDOut to avoid error
                param.Add("@TrainingPlanIDOut", dbType: DbType.Int64, direction: ParameterDirection.Output);
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
                //creaating an unused parameter called @TrainingPlanIDOut to avoid error
                param.Add("@TrainingPlanIDOut", dbType: DbType.Int64, direction: ParameterDirection.Output);
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
                    //creaating an unused parameter called @TrainingPlanIDOut to avoid error
                    param.Add("@TrainingPlanIDOut", dbType: DbType.Int64, direction: ParameterDirection.Output);

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

        public async Task<IEnumerable<TrainingPlanDTO>> GetAllActiveTrainingPlan()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingPlanEnum.GETALLACTIVE);
                    //creaating an unused parameter called @TrainingPlanIDOut to avoid error
                    param.Add("@TrainingPlanIDOut", dbType: DbType.Int64, direction: ParameterDirection.Output);

                    var TrainingPlans = await _dapper.QueryAsync<TrainingPlanDTO>(ApplicationConstant.Sp_TrainingPlan, param: param, commandType: CommandType.StoredProcedure);

                    return TrainingPlans;
                }
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
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingPlanEnum.GETBYID);
                    param.Add("@TrainingPlanIDGet", TrainingPlanID);
                    //creaating an unused parameter called @TrainingPlanIDOut to avoid error
                    param.Add("@TrainingPlanIDOut", dbType: DbType.Int64, direction: ParameterDirection.Output);



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

        public async Task<IEnumerable<TrainingPlanDTO>> GetTrainingPlanPendingApproval()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingPlanEnum.GETPENDINGAPPROVAL);
                    //creaating an unused parameter called @TrainingPlanIDOut to avoid error
                    param.Add("@TrainingPlanIDOut", dbType: DbType.Int64, direction: ParameterDirection.Output);

                    var response = await _dapper.QueryAsync<TrainingPlanDTO>(ApplicationConstant.Sp_TrainingPlan, param: param, commandType: CommandType.StoredProcedure);

                    return response;
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
                    //creaating an unused parameter called @TrainingPlanIDOut to avoid error
                    param.Add("@TrainingPlanIDOut", dbType: DbType.Int64, direction: ParameterDirection.Output);

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

        public async Task<IEnumerable<TrainingPlanDTO>> GetAllTrainingPlanByUserId(long UserId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingPlanEnum.GETBYUSERID);
                    param.Add("@UserIDGet", UserId);
                    //creaating an unused parameter called @TrainingPlanIDOut to avoid error
                    param.Add("@TrainingPlanIDOut", dbType: DbType.Int64, direction: ParameterDirection.Output);


                    var TrainingPlans = await _dapper.QueryAsync<TrainingPlanDTO>(ApplicationConstant.Sp_TrainingPlan, param: param, commandType: CommandType.StoredProcedure);

                    return TrainingPlans;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllTrainingPlanByUserId() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> ScheduleTrainingPlan(TrainingPlanSchedule TrainingPlan, string scheduledbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingPlanEnum.SCHEDULE);
                    param.Add("@TrainingPlanIDSch", TrainingPlan.TrainingPlanID);
                    param.Add("@TrainingOrganizer", TrainingPlan.TrainingOrganizer);
                    param.Add("@StartDate", TrainingPlan.StartDate);
                    param.Add("@EndDate", TrainingPlan.EndDate);
                    param.Add("@TrainingVenue", TrainingPlan.TrainingVenue);
                    param.Add("@TrainingTopic", TrainingPlan.TrainingTopic);
                    param.Add("@TrainingTime", TrainingPlan.TrainingTime);
                    param.Add("@TrainingMode", TrainingPlan.TrainingMode);
                    param.Add("@CompanyIdSch", TrainingPlan.CompanyID);
                    param.Add("@Scheduled_By_User_Email", scheduledbyUserEmail.Trim());
                    //creaating an unused parameter called @TrainingPlanIDOut to avoid error
                    param.Add("@TrainingPlanIDOut", dbType: DbType.Int64, direction: ParameterDirection.Output);

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_TrainingPlan, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: ScheduleTrainingPlan ===>{ex}");
                throw;
            }
        }
    }
}
