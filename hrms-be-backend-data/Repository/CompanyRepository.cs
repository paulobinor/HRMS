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
    public class CompanyRepository : ICompanyRepository
    {
        private string _connectionString;
        private readonly ILogger<CompanyRepository> _logger;
        private readonly IConfiguration _configuration;

        public CompanyRepository(IConfiguration configuration, ILogger<CompanyRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateCompany(CreateCompanyDto Comp, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Cmpany.CREATE);
                    param.Add("@CompanyName", Comp.CompanyName.Trim());
                    param.Add("@CompanyCode", Comp.CompanyCode.Trim());
                    param.Add("@LastStaffNumber", Comp.LastStaffNumber);
                    param.Add("@Address", Comp.Address.Trim());
                    param.Add("@Email", Comp.Email.Trim());
                    param.Add("@Website", Comp.Website.Trim());
                    param.Add("@CompanyLogo", Comp.CompanyLogo);
                    param.Add("@ContactPhone", Comp.ContactPhone.Trim());

                    param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Company, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateCompany(CreateCompanyDto Company, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateCompany(UpdateCompanyDto Comp, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Cmpany.UPDATE);
                    param.Add("@CompanyIdUpd", Convert.ToInt32(Comp.CompanyId));
                    param.Add("@CompanyCodeUpd", Comp.CompanyCode.Trim());
                    param.Add("@CompanyNameUpd", Comp.CompanyName == null ? "" : Comp.CompanyName.ToString().Trim());
                    param.Add("@LastStaffNumber", Comp.LastStaffNumber);
                    param.Add("@AddressUpd", Comp.Address.Trim());
                    param.Add("@EmailUpd", Comp.Email.Trim());
                    param.Add("@WebsiteUpd", Comp.Website.Trim());
                    param.Add("@CompanyLogoUpd", Comp.CompanyLogo.Trim());
                    param.Add("@ContactPhoneUpd", Comp.ContactPhone.Trim());
                    param.Add("@CompanyIdUpd", Comp.CompanyId);

                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Company, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateCompany(UpdateCompanyDto Company, string updtedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteCompany(DeleteCompanyDto Comp, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Cmpany.DELETE);
                    param.Add("@CompanyIdDelete", Convert.ToInt32(Comp.CompanyId));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Deleting_Company", Comp.Reasons_For_Delete == null ? "" : Comp.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Company, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteCompany(DeleteCompanyDto Company, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<CompanyDTO>> GetAllActiveCompanys()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Cmpany.GETALLACTIVE);

                    var CompanyDetails = await _dapper.QueryAsync<CompanyDTO>(ApplicationConstant.Sp_Company, param: param, commandType: CommandType.StoredProcedure);

                    return CompanyDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActiveCompanys() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<CompanyDTO>> GetAllCompanys()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Cmpany.GETALL);

                    var CompanyDetails = await _dapper.QueryAsync<CompanyDTO>(ApplicationConstant.Sp_Company, param: param, commandType: CommandType.StoredProcedure);

                    return CompanyDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllCompanys() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<CompanyDTO> GetCompanyById(long CompanyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Cmpany.GETBYID);
                    param.Add("@CompanyIdGet", CompanyId);

                    var CompanyDetails = await _dapper.QueryFirstOrDefaultAsync<CompanyDTO>(ApplicationConstant.Sp_Company, param: param, commandType: CommandType.StoredProcedure);

                    return CompanyDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetCompanyById(int CompanyId) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<CompanyDTO> GetCompanyByName(string CompanyName)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Cmpany.GETBYname);
                    param.Add("@CompanyNameGet", CompanyName);

                    var CompanyDetails = await _dapper.QueryFirstOrDefaultAsync<CompanyDTO>(ApplicationConstant.Sp_Company, param: param, commandType: CommandType.StoredProcedure);

                    return CompanyDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetCompanyById(int CompanyId) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<CompanyDTO> GetCompanyByEmail(string Email)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Cmpany.GETBYEmail);
                    param.Add("@EmailGet", Email.Trim());

                    var CompanyDetails = await _dapper.QueryFirstOrDefaultAsync<CompanyDTO>(ApplicationConstant.Sp_Company, param: param, commandType: CommandType.StoredProcedure);

                    return CompanyDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetCompanyById(int CompanyId) ===>{ex.Message}");
                throw;
            }
        }


    }
}
