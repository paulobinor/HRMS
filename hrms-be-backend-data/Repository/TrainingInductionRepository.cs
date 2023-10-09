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
        private readonly IDapperGenericRepository _dapperGeneric;
        private readonly IConfiguration _configuration;
        public TrainingInductionRepository(IAccountRepository accountRepository, IConfiguration configuration, ILogger<TrainingInductionRepository> logger, IDapperGenericRepository dapperGeneric)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _accountRepository = accountRepository;
            _configuration = configuration;
            _dapperGeneric = dapperGeneric;
        }
        public async Task<string> CreateTrainingInduction(TrainingInductionCreate TrainingInduction, string createdbyUserEmail)
        {
            try
            {
                var userDetails = await _accountRepository.FindUser(TrainingInduction.UserID);
                var param = new DynamicParameters();
                param.Add("@Status", TrainingInductionEnum.CREATE);
                param.Add("@UserId", TrainingInduction.UserID);
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


                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_TrainingInduction, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateTrainingInduction ===>{ex}");
                throw;
            }
        }

        public async Task<dynamic> UpdateTrainingInduction(TrainingInductionUpdate TrainingInduction, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingInductionEnum.UPDATE);
                    param.Add("@TrainingInductionIDUpd", TrainingInduction.TrainingInductionID);
                    param.Add("@CompanyIdUpd", TrainingInduction.CompanyID);
                    param.Add("@TrainingTitleUpd", TrainingInduction.TrainingTitle);
                    param.Add("@TrainingVenueUpd", TrainingInduction.TrainingVenue);
                    param.Add("@TrainingProviderUpd", TrainingInduction.TrainingProvider);
                    param.Add("@TrainingTimeUpd", TrainingInduction.TrainingTime);
                    param.Add("@TrainingModeUpd", TrainingInduction.TrainingMode);
                    param.Add("@DocumentsUpd", TrainingInduction.Documents);
                    param.Add("@MediaUpd", TrainingInduction.Media);
                    param.Add("@StartDateUpd", TrainingInduction.StartDate);
                    param.Add("@EndDateUpd", TrainingInduction.EndDate);
                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_TrainingInduction, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }

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
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingInductionEnum.DELETE);
                    param.Add("@TrainingInductionIDDelete", delete.TrainingInductionID);
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Delete", delete.Reasons_For_Delete == null ? "" : delete.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_TrainingInduction, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<dynamic> DeleteTrainingInduction(TrainingInductionDelete delete, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<string> ApproveTrainingInduction(long TrainingInductionID, long ApprovedByUserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", 10);
                param.Add("@TrainingInductionID", TrainingInductionID);
                param.Add("@ApprovedByUserId", ApprovedByUserId);
                param.Add("@DateApproved", DateTime.Now);
                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_TrainingInduction, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: ApproveTrainingInduction ===>{ex}");
                throw;
            }
        }

        public async Task<string> DisaproveTrainingInduction(long TrainingInductionID, long DisapprovedByUserId, string DisapprovedComment)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", 11);
                param.Add("@TrainingInductionID", TrainingInductionID);
                param.Add("@DisapprovedByUserId", DisapprovedByUserId);
                param.Add("@DisapprovedComment", DisapprovedComment);
                param.Add("@DateDisapproved", DateTime.Now);
                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_TrainingInduction, param, commandType: CommandType.StoredProcedure);

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
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingInductionEnum.GETALLACTIVE);

                    var TrainingInductions = await _dapper.QueryAsync<TrainingInductionDTO>(ApplicationConstant.Sp_TrainingInduction, param: param, commandType: CommandType.StoredProcedure);

                    return TrainingInductions;
                }
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
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingInductionEnum.GETALL);

                    var TrainingInductions = await _dapper.QueryAsync<TrainingInductionDTO>(ApplicationConstant.Sp_TrainingInduction, param: param, commandType: CommandType.StoredProcedure);

                    return TrainingInductions;
                }
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
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingInductionEnum.GETBYCOMPANYID);
                    param.Add("@CompanyIdGet", companyId);

                    var TrainingInductionDetails = await _dapper.QueryAsync<TrainingInductionDTO>(ApplicationConstant.Sp_TrainingInduction, param: param, commandType: CommandType.StoredProcedure);

                    return TrainingInductionDetails;
                }
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
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingInductionEnum.GETBYID);
                    param.Add("@TrainingInductionIDGet", TrainingInductionID);


                    var TrainingInductionDetails = await _dapper.QueryFirstOrDefaultAsync<TrainingInductionDTO>(ApplicationConstant.Sp_TrainingInduction, param: param, commandType: CommandType.StoredProcedure);

                    return TrainingInductionDetails;
                }
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
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingInductionEnum.GETPENDINGAPPROVAL);

                    var response = await _dapper.QueryAsync<TrainingInductionDTO>(ApplicationConstant.Sp_TrainingInduction, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetTrainingInductionPendingApproval() ===>{ex.Message}");
                throw;
            }
        }


    }
}
