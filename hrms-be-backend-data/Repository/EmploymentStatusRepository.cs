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
    public  class EmploymentStatusRepository : IEmploymentStatusRepository
    {
        private string _connectionString;
        private readonly ILogger<EmploymentStatusRepository> _logger;
        private readonly IConfiguration _configuration;

        public EmploymentStatusRepository(IConfiguration configuration, ILogger<EmploymentStatusRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateEmploymentStatus(CreateEmploymentStatusDTO create, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmploymentStatusEnum.CREATE);
                    param.Add("@EmploymentStatusName", create.EmploymentStatusName.Trim());
                    param.Add("@CompanyId", create.CompanyID);

                    param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.sp_EmploymentStatus, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateEmploymentStatus(CreateEmploymentStatusDTO emplocation, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateEmploymentStatus(UpdateEmploymentStatusDTO Update, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmploymentStatusEnum.UPDATE);
                    param.Add("@EmploymentStatusIDUpd", Update.EmploymentStatusID);
                    param.Add("@EmploymentStatusNameUpd", Update.EmploymentStatusName.Trim());
                    param.Add("@CompanyIdUpd", Update.CompanyID);

                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.sp_EmploymentStatus, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateEmploymentStatus(UpdateEmploymentStatusDTO Update, string updatedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteEmploymentStatus(DeleteEmploymentStatusDTO DelEmpStatus, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmploymentStatusEnum.DELETE);
                    param.Add("@EmploymentStatusIDDelete", Convert.ToInt32(DelEmpStatus.EmploymentStatusID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Deleting", DelEmpStatus.Reasons_For_Delete == null ? "" : DelEmpStatus.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.sp_EmploymentStatus, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteEmploymentStatus(DeleteEmploymentStatusDTO DelEmpStatus, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<EmploymentStatusDTO>> GetAllActiveEmploymentStatus()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmploymentStatusEnum.GETALLACTIVE);

                    var EmploymentStatusDetails = await _dapper.QueryAsync<EmploymentStatusDTO>(ApplicationConstant.sp_EmploymentStatus, param: param, commandType: CommandType.StoredProcedure);

                    return EmploymentStatusDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActiveEmploymentStatus() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<EmploymentStatusDTO>> GetAllEmpLoymentStatus()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmploymentStatusEnum.GETALL);

                    var EmpLoymentStatusDetails = await _dapper.QueryAsync<EmploymentStatusDTO>(ApplicationConstant.sp_EmploymentStatus, param: param, commandType: CommandType.StoredProcedure);

                    return EmpLoymentStatusDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:GetAllEmpLoymentStatus() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<EmploymentStatusDTO> GetEmpLoymentStatusById(long EmploymentStatusID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmploymentStatusEnum.GETBYID);
                    param.Add("@EmploymentStatusIDGet", EmploymentStatusID);

                    var EmpLoymentStatusDetails = await _dapper.QueryFirstAsync<EmploymentStatusDTO>(ApplicationConstant.sp_EmploymentStatus, param: param, commandType: CommandType.StoredProcedure);

                    return EmpLoymentStatusDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetEmpLoymentStatusById(long EmploymentStatusID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<EmploymentStatusDTO> GetEmpLoymentStatusByName(string EmploymentStatusName)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmploymentStatusEnum.GETBYEMAIL);
                    param.Add("@EmploymentStatusNameGet", EmploymentStatusName);
                   

                    var EmpLocationDetails = await _dapper.QueryFirstOrDefaultAsync<EmploymentStatusDTO>(ApplicationConstant.sp_EmploymentStatus, param: param, commandType: CommandType.StoredProcedure);

                    return EmpLocationDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetEmpLoymentStatusByName(string EmploymentStatusName) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<EmploymentStatusDTO> GetEmpLoymentStatusByCompany(string EmploymentStatusName, int companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmploymentStatusEnum.GETBYNAMEandCOMAPMNY);
                    param.Add("@EmploymentStatusNameGet", EmploymentStatusName);
                    param.Add("@CompanyIdGet", companyId);

                    var EmpLocationDetails = await _dapper.QueryFirstOrDefaultAsync<EmploymentStatusDTO>(ApplicationConstant.sp_EmploymentStatus, param: param, commandType: CommandType.StoredProcedure);

                    return EmpLocationDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetEmpLoymentStatusByName(string EmploymentStatusName) ===>{ex.Message}");
                throw;
            }
        }


        public async Task<IEnumerable<EmploymentStatusDTO>> GetAllEmploymentStatusCompanyId(long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", 8);
                    param.Add("@CompanyIdGet", companyId);

                    var EmpLocationDetails = await _dapper.QueryAsync<EmploymentStatusDTO>(ApplicationConstant.sp_EmploymentStatus, param: param, commandType: CommandType.StoredProcedure);

                    return EmpLocationDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllEmpLocationCompanyId(long companyId) ===>{ex.Message}");
                throw;
            }
        }



    }
}
