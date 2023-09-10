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
    public class DepartmentRepository : IDepartmentRepository
    {
        private string _connectionString;
        private readonly ILogger<DepartmentRepository> _logger;
        private readonly IConfiguration _configuration;

        public DepartmentRepository(IConfiguration configuration, ILogger<DepartmentRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateDepartment(CreateDepartmentDto Dept, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Department.CREATE);
                    param.Add("@DepartmentName", Dept.DepartmentName.Trim());
                    param.Add("@HODUserId", Dept.HODUserId);
                   
                    param.Add("@CompanyId", Dept.CompanyId);

                    param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Departments, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateDepartment(CreateDepartmentDto Department, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateDepartment(UpdateDepartmentDto Dept, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Department.UPDATE);
                    param.Add("@DepartmentIdUpd", Convert.ToInt32(Dept.DeptId));
                    param.Add("@DepartmentNameUpd", Dept.DepartmentName == null ? "" : Dept.DepartmentName.ToString().Trim());
                    param.Add("@HODUserIdUpd", Dept.HODUserId);
                  
                    param.Add("@CompanyIdUpd", Dept.CompanyId);

                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Departments, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateDepartment(UpdateDepartmentDto Department, string updtedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteDepartment(DeleteDepartmentDto Dept, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Department.DELETE);
                    param.Add("@DepartmentIdDelete", Convert.ToInt32(Dept.DeptId));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Deleting_Department", Dept.Reasons_For_Delete == null ? "" : Dept.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Departments, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteDepartment(DeleteDepartmentDto Department, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<DepartmentsDTO>> GetAllActiveDepartments()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Department.GETALLACTIVE);

                    var DepartmentDetails = await _dapper.QueryAsync<DepartmentsDTO>(ApplicationConstant.Sp_Departments, param: param, commandType: CommandType.StoredProcedure);

                    return DepartmentDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActiveDepartments() ===>{ ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<DepartmentsDTO>> GetAllDepartments()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Department.GETALL);

                    var DepartmentDetails = await _dapper.QueryAsync<DepartmentsDTO>(ApplicationConstant.Sp_Departments, param: param, commandType: CommandType.StoredProcedure);

                    return DepartmentDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllDepartments() ===>{ ex.Message}");
                throw;
            }
        }

        public async Task<DepartmentsDTO> GetDepartmentById(long DepartmentId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Department.GETBYID);
                    param.Add("@DepartmentIdGet", DepartmentId);

                    var DepartmentDetails = await _dapper.QueryFirstOrDefaultAsync<DepartmentsDTO>(ApplicationConstant.Sp_Departments, param: param, commandType: CommandType.StoredProcedure);

                    return DepartmentDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetDepartmentById(int DepartmentId) ===>{ ex.Message}");
                throw;
            }
        }

        public async Task<DepartmentsDTO> GetDepartmentByName(string DepartmentName)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Department.GETBYEMAIL);
                    param.Add("@DepartmentNameGet", DepartmentName);

                    var DepartmentDetails = await _dapper.QueryFirstOrDefaultAsync<DepartmentsDTO>(ApplicationConstant.Sp_Departments, param: param, commandType: CommandType.StoredProcedure);

                    return DepartmentDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetDepartmentById(int DepartmentId) ===>{ ex.Message}");
                throw;
            }
        }

        public async Task<DepartmentsDTO> GetDepartmentByCompany(string DepartmentName, int companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Department.GETBYCOMPANY);
                    param.Add("@DepartmentNameGet", DepartmentName);
                    param.Add("@CompanyIdGet", companyId);

                    var DepartmentDetails = await _dapper.QueryFirstOrDefaultAsync<DepartmentsDTO>(ApplicationConstant.Sp_Departments, param: param, commandType: CommandType.StoredProcedure);

                    return DepartmentDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetDepartmentById(int DepartmentId) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<DepartmentsDTO>> GetAllDepartmentsbyCompanyId(long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Department.GETBYCOMPANYID);
                    param.Add("@CompanyIdGet", companyId);

                    var DepartmentDetails = await _dapper.QueryAsync<DepartmentsDTO>(ApplicationConstant.Sp_Departments, param: param, commandType: CommandType.StoredProcedure);

                    return DepartmentDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllDepartmentsbyCompanyId(long companyId) ===>{ ex.Message}");
                throw;
            }
        }
    }
}
