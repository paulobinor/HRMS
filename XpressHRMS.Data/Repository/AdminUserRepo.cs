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

namespace XpressHRMS.Data.Repository
{
    public class AdminUserRepo : IAdminUserRepo
    {
        private readonly string _connectionString;
        private readonly ILogger<AdminUserRepo> _logger;
        private readonly IConfiguration _configuration;
        public AdminUserRepo(IConfiguration configuration, ILogger<AdminUserRepo> logger)
        {
            _connectionString = configuration.GetConnectionString("HRMSConnectionString");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateAdminUser(CreateAdminUserLoginDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.INSERT);
                    param.Add("@Email", payload.Email == null ? "" : payload.Email.ToString().Trim());
                    param.Add("@Password", payload.Password == null ? "" : payload.Password.ToString().Trim());
                    param.Add("@FirstName", payload.FirstName == null ? "" : payload.FirstName.ToString().Trim());
                    param.Add("@LastName", payload.LastName == null ? "" : payload.LastName.ToString().Trim());
                    param.Add("@CompanyID", payload.CompanyID == null ? "" : payload.CompanyID.ToString().Trim());
                    dynamic response = await _dapper.ExecuteAsync("Sp_AdminUser", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateAdminUser(CreateAdminUserLoginDTO payload) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<AdminDTO>> LoginAdmin(UserLoginDTO payload
           )
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.SELECTALL);
                    param.Add("@Email", payload.Email);
                    param.Add("@Password", payload.Password);
                    var login = await _dapper.QueryAsync<AdminDTO>("Sp_AdminUser", param: param, commandType: CommandType.StoredProcedure);
                    return login;
                    //return await _dapper.GetAll<CompanyDTO>("Sp_Company", param, commandType: CommandType.StoredProcedure);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }

        }

        public async Task<IEnumerable<AdminDTO>> GetAdminUser(UserLoginDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    int d = (int)GetAllDefault.GetAll;
                    param.Add("@Status", 3);
                    param.Add("@Email", payload.Email);
                    var response = await _dapper.QueryAsync<AdminDTO>("Sp_AdminUser", param: param, commandType: CommandType.StoredProcedure);

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
