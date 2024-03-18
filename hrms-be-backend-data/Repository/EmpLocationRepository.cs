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
    public class EmpLocationRepository : IEmpLocationRepository
    {
        private string _connectionString;
        private readonly ILogger<EmpLocationRepository> _logger;
        private readonly IConfiguration _configuration;

        public EmpLocationRepository(IConfiguration configuration, ILogger<EmpLocationRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateEmpLocation(CreateEmpLocationDTO emplocation, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmpLocationEnum.CREATE);
                    param.Add("@BranchID", emplocation.BranchID);
                    param.Add("@LocationAddress", emplocation.LocationAddress.Trim());
                    param.Add("@PhoneNumber", emplocation.PhoneNumber);
                    param.Add("@Email", emplocation.Email.Trim());
                    param.Add("@CompanyId", emplocation.CompanyID);

                    param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.sp_EmployeeLocation, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateEmpLocationDTO(CreateEmpLocationDTO emplocation, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateEmpLocation(UpdateEmpLocationDTO Update, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmpLocationEnum.UPDATE);
                    param.Add("@BranchIDUpd", Update.EmpLocationID);
                    param.Add("@BranchIDUpd", Update.BranchID);
                    param.Add("@LocationAddressUpd", Update.LocationAddress.Trim());
                    param.Add("@PhoneNumberUpd", Update.PhoneNumber);
                    param.Add("@EmailUpd", Update.Email.Trim());
                    param.Add("@CompanyIdUpd", Update.CompanyID);

                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.sp_EmployeeLocation, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateEmpLocation(UpdateEmpLocationDTO Update, string updatedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteEmLocation(DeleteEmpLocationDTO DelEmpLocation, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmpLocationEnum.DELETE);
                    param.Add("@EmpLocationIDDelete", Convert.ToInt32(DelEmpLocation.EmpLocationID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Deleting_Department", DelEmpLocation.Reasons_For_Delete == null ? "" : DelEmpLocation.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.sp_EmployeeLocation, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteEmLocation(DeleteEmpLocationDTO DelEmpLocation, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<EmpLocationDTO>> GetAllActiveEmLocation()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmpLocationEnum.GETALLACTIVE);

                    var EmpLocationDetails = await _dapper.QueryAsync<EmpLocationDTO>(ApplicationConstant.sp_EmployeeLocation, param: param, commandType: CommandType.StoredProcedure);

                    return EmpLocationDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActiveEmLocation() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<EmpLocationDTO>> GetAllEmpLocation()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmpLocationEnum.GETALL);

                    var EmpLocationDetails = await _dapper.QueryAsync<EmpLocationDTO>(ApplicationConstant.sp_EmployeeLocation, param: param, commandType: CommandType.StoredProcedure);

                    return EmpLocationDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:GetAllEmpLocation() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<EmpLocationDTO> GetEmpLocationById(long EmpLocationID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmpLocationEnum.GETBYID);
                    param.Add("@EmpLocationIDGet", EmpLocationID);

                    var EmpLocationDetails = await _dapper.QueryFirstAsync<EmpLocationDTO>(ApplicationConstant.sp_EmployeeLocation, param: param, commandType: CommandType.StoredProcedure);

                    return EmpLocationDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetEmpLocationById(long EmpLocationID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<EmpLocationDTO> GetEmpLocationByName(string LocationAddress)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmpLocationEnum.GETBYEMAIL);
                    param.Add("@LocationAddressGet", LocationAddress);

                    var EmpLocationDetails = await _dapper.QueryFirstOrDefaultAsync<EmpLocationDTO>(ApplicationConstant.sp_EmployeeLocation, param: param, commandType: CommandType.StoredProcedure);

                    return EmpLocationDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetEmpLocationByName(string LocationAddress) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<EmpLocationDTO>> GetAllEmpLocationCompanyId(long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", 8);
                    param.Add("@CompanyIdGet", companyId);

                    var EmpLocationDetails = await _dapper.QueryAsync<EmpLocationDTO>(ApplicationConstant.sp_EmployeeLocation, param: param, commandType: CommandType.StoredProcedure);

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
