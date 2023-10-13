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
    public class RolesRepo : IRolesRepo
    {
        private string _connectionString;
        private readonly ILogger<RolesRepo> _logger;
        private readonly IConfiguration _configuration;

        public RolesRepo(IConfiguration configuration, ILogger<RolesRepo> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<RolesDTO> GetRolesByName(string RoleName)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", RoleEnum.GETBYname);
                    param.Add("@RoleName", RoleName);

                    var PositionDetails = await _dapper.QueryFirstOrDefaultAsync<RolesDTO>(ApplicationConstant.Sp_Roles, param: param, commandType: CommandType.StoredProcedure);

                    return PositionDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetRolesByName(string RoleName) ===>{ex.Message}");
                throw;
            }
        }



        public async Task<IEnumerable<RolesDTO>> GetAllRoles()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", RoleEnum.GetAll);


                    var roles = await _dapper.QueryAsync<RolesDTO>(ApplicationConstant.Sp_Roles, param: param, commandType: CommandType.StoredProcedure);

                    return roles;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetRolesByName(string RoleName) ===>{ex.Message}");
                throw;
            }
        }

    }
}
