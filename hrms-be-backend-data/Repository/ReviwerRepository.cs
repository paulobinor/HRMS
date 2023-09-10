using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload.OnBoardingDTO.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{
    public class ReviwerRepository : IReviwerRepository
    {
        private string _connectionString;
        private readonly ILogger<ReviwerRepository> _logger;
        private readonly IConfiguration _configuration;

        public ReviwerRepository(IConfiguration configuration, ILogger<ReviwerRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateReviwer(CreateReviwerDTO CreateReviwer, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ReviwerEnum.CREATE);
                    param.Add("@UserId", CreateReviwer.UserId);
                    param.Add("@ReviwerRoleID", CreateReviwer.ReviwerRoleID);
                    param.Add("@CompanyId", CreateReviwer.CompanyID);

                    param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Reviwer, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateReviwer(CreateReviwerDTO CreateReviwer, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteReviwer(DeleteReviwerDTO del, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ReviwerEnum.DELETE);
                    param.Add("@ReviwerIDDelete", del.ReviwerID);
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Deleting", del.Reasons_For_Delete == null ? "" : del.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Reviwer, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteReviwer(DeleteReviwerDTO del, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<ReviwerDTO> GetReviwerById(long ReviwerID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ReviwerEnum.GETBYID);
                    param.Add("@ReviwerIDGet", ReviwerID);

                    var ReviwerDetails = await _dapper.QueryFirstOrDefaultAsync<ReviwerDTO>(ApplicationConstant.Sp_Reviwer, param: param, commandType: CommandType.StoredProcedure);

                    return ReviwerDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetReviwerById(long ReviwerID) ===>{ex.Message}");
                throw;
            }
        }
        public async Task<ReviwerDTO> GetReviwerByName(long UserId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ReviwerEnum.getbyname);
                    param.Add("@UserIdGet", UserId);

                    var ReviwerDetails = await _dapper.QueryFirstOrDefaultAsync<ReviwerDTO>(ApplicationConstant.Sp_Reviwer, param: param, commandType: CommandType.StoredProcedure);

                    return ReviwerDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<ReviwerDTO> GetReviwerByName(string UserId) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<ReviwerDTO>> GetAllReviwerCompanyId(long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ReviwerEnum.GETBYCOMPANYID);
                    param.Add("@CompanyIdGet", companyId);

                    var ReviwerDetails = await _dapper.QueryAsync<ReviwerDTO>(ApplicationConstant.Sp_Reviwer, param: param, commandType: CommandType.StoredProcedure);

                    return ReviwerDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllGroupCompanyId(long companyId) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<ReviwerDTO> GetReviwerByCompany(long UserId, long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ReviwerEnum.GETBYCOMPANY);
                    param.Add("@UserIdGet", UserId);
                    param.Add("@CompanyIdGet", companyId);

                    var ReviwerDetails = await _dapper.QueryFirstOrDefaultAsync<ReviwerDTO>(ApplicationConstant.Sp_Reviwer, param: param, commandType: CommandType.StoredProcedure);

                    return ReviwerDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetHODByName(string DepartmentName) ===>{ex.Message}");
                throw;
            }
        }

    }
}
