using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;
using XpressHRMS.Data.Enums;
using XpressHRMS.Data.IRepository;
using XpressHRMS.IRepository;

namespace XpressHRMS.Data.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<RoleRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDapperGeneric _dapperr;

        public RoleRepository(IConfiguration configuration, ILogger<RoleRepository> logger, IDapperGeneric dapperr)
        {
            _connectionString = configuration.GetConnectionString("HRMSConnectionString");
            _logger = logger;
            _configuration = configuration;
            _dapperr = dapperr;
        }

        public async Task<int> CreateRole(CreateRoleDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.INSERT);
                    param.Add("@RoleName", payload.RoleName);
                    param.Add("@CompanyID", payload.CompanyID);
                    param.Add("@CreatedBy", payload.CreatedBy);

                    dynamic response = await _dapper.ExecuteAsync("Sp_Role", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }
        public async Task<int> DeleteRole(DeleteRoleDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DELETE);
                    param.Add("@CompanyID", payload.CompanyID);
                    param.Add("@RoleID", payload.RoleID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_Role", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }



            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }



        public async Task<int> UpdateRole(UpdateRoleDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.UPDATE);
                    param.Add("@RoleID", payload.RoleID);
                    param.Add("@RoleName", payload.RoleName);
                    param.Add("@CompanyID", payload.CompanyID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_Role", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }



            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }
        public async Task<List<RoleDTO>> GetAllRoles(int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.SELECTALL);
                    param.Add("@CompanyID", CompanyID);
                    var roles = await _dapperr.GetAll<RoleDTO>("Sp_Role", param, commandType: CommandType.StoredProcedure);
                    return roles;
                    //return await _dapper.GetAll<CompanyDTO>("Sp_Company", param, commandType: CommandType.StoredProcedure);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }

        }

        public async Task<RoleDTO> GetRolesByID(DeleteRoleDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    int d = (int)GetAllDefault.GetAll;
                    param.Add("@Status", ACTION.SELECTBYID);
                    param.Add("@CompanyID", payload.CompanyID);
                    param.Add("@RoleID", payload.RoleID);
                    var response = await _dapperr.Get<RoleDTO>("Sp_Role", param, commandType: CommandType.StoredProcedure);
                    return response;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }

        }
    }
}
