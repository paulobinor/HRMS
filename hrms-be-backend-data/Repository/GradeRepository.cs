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
    public  class GradeRepository : IGradeRepository
    {
        private string _connectionString;
        private readonly ILogger<GradeRepository> _logger;
        private readonly IConfiguration _configuration;

        public GradeRepository(IConfiguration configuration, ILogger<GradeRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateGrade(CreateGradeDTO create, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GradeEnum.CREATE);
                    param.Add("@GradeName", create.GradeName.Trim());
                    //param.Add("@NumberOfVacationDays", create.NumberOfVacationDays);
                    //param.Add("@NumberOfVacationSplit", create.NumberOfVacationSplit);
                    param.Add("@CompanyID", create.CompanyID);

                    param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());
                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Grade, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateGrade(CreateGradeDTO create, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateGrade(UpdateGradeDTO update, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GradeEnum.UPDATE);
                    param.Add("@GradeIDUpd", update.GradeID);
                    param.Add("@GradeNameUpd", update.GradeName);
                    //param.Add("@NumberOfVacationDaysUpd", update.NumberOfVacationDays);
                    //param.Add("@NumberOfVacationSplitUpd", update.NumberOfVacationSplit);
                    param.Add("@CompanyIdUpd", update.CompanyID);

                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Grade, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateGrade(UpdateGradeDTO update, string updatedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteGrade(DeleteGradeDTO delete, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GradeEnum.DELETE);
                    param.Add("@GradeIDDelete", Convert.ToInt32(delete.GradeID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Deleting", delete.Reasons_For_Delete == null ? "" : delete.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Grade, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteGrade(DeleteGradeDTO delete, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<GradeDTO>> GetAllActiveGrade()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GradeEnum.GETALLACTIVE);

                    var GradeDetails = await _dapper.QueryAsync<GradeDTO>(ApplicationConstant.Sp_Grade, param: param, commandType: CommandType.StoredProcedure);

                    return GradeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActiveGrade()===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<GradeDTO>> GetAllGrade()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GradeEnum.GETALL);

                    var GradeDetails = await _dapper.QueryAsync<GradeDTO>(ApplicationConstant.Sp_Grade, param: param, commandType: CommandType.StoredProcedure);

                    return GradeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:GetAllGrade() ===>{ex.Message}");
                throw;
            }
        }


        public async Task<GradeDTO> GetGradeById(long GradeID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GradeEnum.GETBYID);
                    param.Add("@GradeIDGet", GradeID);

                    var GradeDetails = await _dapper.QueryFirstOrDefaultAsync<GradeDTO>(ApplicationConstant.Sp_Grade, param: param, commandType: CommandType.StoredProcedure);

                    return GradeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetGradeById(long GradeID)===>{ex.Message}");
                throw;
            }
        }

        public async Task<GradeDTO> GetGradeByName(string GradeName)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GradeEnum.GETBYEMAIL);
                    param.Add("@GradeNameGet", GradeName);

                    var GradeDetails = await _dapper.QueryFirstOrDefaultAsync<GradeDTO>(ApplicationConstant.Sp_Grade, param: param, commandType: CommandType.StoredProcedure);

                    return GradeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetGradeByName(string GradeName) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<GradeDTO> GetGradeByCompany(string GradeName, int companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GradeEnum.GETBYCOMPANY);
                    param.Add("@GradeNameGet", GradeName);
                    param.Add("@CompanyIdGet", companyId);

                    var GradeDetails = await _dapper.QueryFirstOrDefaultAsync<GradeDTO>(ApplicationConstant.Sp_Grade, param: param, commandType: CommandType.StoredProcedure);

                    return GradeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetGradeByName(string GradeName) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<GradeDTO>> GetAllGradeCompanyId(long CompanyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GradeEnum.GETBYCOMPANYID);
                    param.Add("@CompanyIdGet", CompanyId);

                    var GradeDetails = await _dapper.QueryAsync<GradeDTO>(ApplicationConstant.Sp_Grade, param: param, commandType: CommandType.StoredProcedure);

                    return GradeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllGradeCompanyId(long GradeID) ===>{ex.Message}");
                throw;
            }
        }


    }
}
