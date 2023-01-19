using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
    
    [Authorize]
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ILogger<CompanyRepository> _logger;
        private readonly IDapperGeneric _dapper;
        private readonly string _connectionString;

        public CompanyRepository(ILogger<CompanyRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("HRMSConnectionString");


        }

        public async Task<int> CreateCompany(CreateCompanyDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.INSERT);
                    param.Add("@CompanyName", payload.CompanyName);
                    param.Add("@Companyphonenumber", payload.Companyphonenumber);
                    param.Add("@CompanyTheme", JsonConvert.SerializeObject(payload.CompanyTheme));
                    param.Add("@Email", payload.Email);
                    param.Add("@EstablishmentDate", payload.EstablishmentDate);
                    param.Add("@MissionStmt", payload.MissionStmt);
                    param.Add("@VisionStmt", payload.VisionStmt);
                    param.Add("@Website", payload.Website);
                    param.Add("@CompanyLogo", payload.CompanyLogo);

                    dynamic response = await _dapper.ExecuteAsync("Sp_Company", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
                 

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }
        public async Task<int> DeleteCompany(int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DELETE);
                    param.Add("@CompanyID", CompanyID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_Company", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
                    


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }

        public async Task<int> DisableCompany(int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DELETE);
                    param.Add("@CompanyID", CompanyID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_Company", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
                

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }

        public async Task<int> ActivateCompany(int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DELETE);
                    param.Add("@CompanyID", CompanyID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_Company", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
               

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }

        public async Task<int> UpdateCompany(UpdateCompanyDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.UPDATE);
                    param.Add("@CompanyLogo", payload.CompanyLogo);
                    param.Add("@CompanyName", payload.CompanyName);
                    param.Add("@Companyphonenumber", payload.Companyphonenumber);
                    param.Add("@CompanyID", payload.CompanyID);
                    param.Add("@Email", payload.Email);
                    param.Add("@EstablishmentDate", payload.EstablishmentDate);
                    param.Add("@MissionStmt", payload.MissionStmt);
                    param.Add("@VisionStmt", payload.VisionStmt);
                    param.Add("@Website", payload.Website);

                    dynamic response = await _dapper.ExecuteAsync("Sp_Company", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
                


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }
        public async Task<IEnumerable<CompanyDTO>> GetAllCompanies()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.SELECTALL);
                    var companies = await _dapper.QueryAsync<CompanyDTO>("Sp_Company", param: param, commandType: CommandType.StoredProcedure);
                    return companies;
                    //return await _dapper.GetAll<CompanyDTO>("Sp_Company", param, commandType: CommandType.StoredProcedure);
                }
                    

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }

        }

        public async Task<IEnumerable<CompanyDTO>> GetCompanyByID(int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    int d = (int)GetAllDefault.GetAll;
                    param.Add("@Status", ACTION.SELECTBYID);
                    param.Add("@CompanyID", CompanyID);
                    var response = await _dapper.QueryAsync<CompanyDTO>("Sp_Company", param: param, commandType: CommandType.StoredProcedure);
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
