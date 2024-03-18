using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{
    public class TrainingInductionRepository : ITrainingInductionRepository
    {
        private string _connectionString;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<TrainingInductionRepository> _logger;
        private readonly IDapperGenericRepository _dapper;
        private readonly IConfiguration _configuration;
        public TrainingInductionRepository(IAccountRepository accountRepository, IConfiguration configuration, ILogger<TrainingInductionRepository> logger, IDapperGenericRepository dapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _accountRepository = accountRepository;
            _configuration = configuration;
            _dapper = dapper;
        }
        public async Task<dynamic> CreateTrainingInduction(TrainingInductionCreate TrainingInduction, string createdbyUserEmail)
        {
            try
            {
                var userDetails = await _accountRepository.FindUser(TrainingInduction.EmployeeID, null, null);
                var param = new DynamicParameters();
                param.Add("@Status", TrainingInductionEnum.CREATE);
                param.Add("@EmployeeId", TrainingInduction.EmployeeID);
                param.Add("@CompanyId", TrainingInduction.CompanyID);
                param.Add("@TrainingTitle", TrainingInduction.TrainingTitle);
                param.Add("@TrainingVenue", TrainingInduction.TrainingVenue);
                param.Add("@TrainingProvider", TrainingInduction.TrainingProvider);
                param.Add("@TrainingTime", TrainingInduction.TrainingTime);
                param.Add("@TrainingMode", TrainingInduction.TrainingMode);
                param.Add("@Documents", TrainingInduction.Documents);
                param.Add("@Media", TrainingInduction.Media);
                param.Add("@StartDate", TrainingInduction.StartDate);
                param.Add("@EndDate", TrainingInduction.EndDate);
                param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());

                // Add an output parameter to capture the TrainingPlanID
                param.Add("@TrainingInductionIDOut", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _dapper.Get<string>("Sp_create_training_induction", param, commandType: CommandType.StoredProcedure);

                // Retrieve the TrainingInductionID from the output parameter
                int trainingInductionId = param.Get<int>("@TrainingInductionIDOut");

                return trainingInductionId;
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateTrainingInduction ===>{ex}");
                return "Unable to perform task, kindly contact support";
            }
        }

        public async Task<dynamic> UpdateTrainingInduction(TrainingInductionUpdate TrainingInduction, string updatedbyUserEmail)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@TrainingInductionID", TrainingInduction.TrainingInductionID);
                param.Add("@CompanyId", TrainingInduction.CompanyID);
                param.Add("@TrainingTitle", TrainingInduction.TrainingTitle);
                param.Add("@TrainingVenue", TrainingInduction.TrainingVenue);
                param.Add("@TrainingProvider", TrainingInduction.TrainingProvider);
                param.Add("@TrainingTime", TrainingInduction.TrainingTime);
                param.Add("@TrainingMode", TrainingInduction.TrainingMode);
                param.Add("@Documents", TrainingInduction.Documents);
                param.Add("@Media", TrainingInduction.Media);
                param.Add("@StartDate", TrainingInduction.StartDate);
                param.Add("@EndDate", TrainingInduction.EndDate);
                param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());


                dynamic response = await _dapper.Get<string>("Sp_update_training_induction", param, commandType: CommandType.StoredProcedure);

                return response;
                

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: UpdateTrainingInduction ===>{ex}");
                throw;
            }
        }

        public async Task<dynamic> DeleteTrainingInduction(TrainingInductionDelete delete, string deletedbyUserEmail)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("@TrainingInductionIDDelete", delete.TrainingInductionID);
                param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                param.Add("@Reasons_For_Delete", delete.Reasons_For_Delete == null ? "" : delete.Reasons_For_Delete.ToString().Trim());

                dynamic response = await _dapper.Get<string>("Sp_delete_training_induction",param, commandType: CommandType.StoredProcedure);

                return response;
                
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<dynamic> DeleteTrainingInduction(TrainingInductionDelete delete, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<string> ApproveTrainingInduction(long TrainingInductionID, long ApprovedByEmployeeId)
        {
            try
            {
                var param = new DynamicParameters();;
                param.Add("@TrainingInductionID", TrainingInductionID);
                param.Add("@ApprovedByEmployeeId", ApprovedByEmployeeId);
                param.Add("@DateApproved", DateTime.Now);
                return await _dapper.Get<string>("Sp_approve_training_induction", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: ApproveTrainingInduction ===>{ex}");
                throw;
            }
        }

        public async Task<string> DisapproveTrainingInduction(long TrainingInductionID, long DisapprovedByEmployeeId, string DisapprovedComment)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@TrainingInductionID", TrainingInductionID);
                param.Add("@DisapprovedByEmployeeId", DisapprovedByEmployeeId);
                param.Add("@DisapprovedComment", DisapprovedComment);
                param.Add("@DateDisapproved", DateTime.Now);
                return await _dapper.Get<string>("Sp_disapprove_training_induction", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: DisaproveTrainingInduction ===>{ex}");
                throw;
            }
        }

        public async Task<IEnumerable<TrainingInductionDTO>> GetAllActiveTrainingInduction()
        {
            try
            {

                var param = new DynamicParameters();

                var TrainingInductions = await _dapper.GetAll<TrainingInductionDTO>("Sp_get_all_active_training_induction",param, commandType: CommandType.StoredProcedure);

                return TrainingInductions;
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllActiveTrainingInduction() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TrainingInductionDTO>> GetAllTrainingInduction()
        {
            try
            {
                var param = new DynamicParameters();
                var TrainingInductions = await _dapper.GetAll<TrainingInductionDTO>("Sp_get_all_training_induction",param, commandType: CommandType.StoredProcedure);

                return TrainingInductions;
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllTrainingInduction() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TrainingInductionDTO>> GetTrainingInductionByCompany(long companyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyId", companyId);

                var TrainingInductions = await _dapper.GetAll<TrainingInductionDTO>("Sp_get_all_training_induction_by_company",param, commandType: CommandType.StoredProcedure);

                return TrainingInductions;
                
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetTrainingInductionByCompany(int companyId) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<TrainingInductionDTO> GetTrainingInductionById(long TrainingInductionID)
        {
            try
            {
                
                    var param = new DynamicParameters();
                    param.Add("@TrainingInductionID", TrainingInductionID);

                    var TrainingInductionDetails = await _dapper.Get<TrainingInductionDTO>("Sp_get_training_induction_by_id",param, commandType: CommandType.StoredProcedure);

                    return TrainingInductionDetails;
                
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<TrainingInductionDTO> GetTrainingInductionById(long TrainingInductionID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TrainingInductionDTO>> GetTrainingInductionPendingApproval()
        {
            try
            {
           
                var param = new DynamicParameters();

                var response = await _dapper.GetAll<TrainingInductionDTO>("Sp_get_training_induction_pending_approval", param, commandType: CommandType.StoredProcedure);

                return response;
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetTrainingInductionPendingApproval() ===>{ex.Message}");
                throw;
            }
        }


    }
}
