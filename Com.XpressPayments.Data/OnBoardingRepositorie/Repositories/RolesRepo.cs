using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.Repositories.Gender;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories
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

    }
}
