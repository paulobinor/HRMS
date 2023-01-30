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
using XpressHRMS.Data.Repository;
using XpressHRMS.IRepository;

namespace XpressHRMS.Data.Repository
{
    public class BranchRepository : IBranchRepository
    {
        private readonly ILogger<BranchRepository> _logger;
        private readonly IDapperGeneric _dapper;
        private readonly string _connectionString;

        public BranchRepository(ILogger<BranchRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("HRMSConnectionString");


        }

        public async Task<int> CreateBranch(CreateBranchDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.INSERT);
                    param.Add("@BranchName", payload.BranchName);
                    param.Add("@CompanyID", payload.CompanyID);
                    param.Add("@IsHeadQuater", payload.IsHeadQuater);
                    //param.Add("@CreatedBy", payload.CreatedBy);
                    param.Add("@Address", payload.Address);
                    param.Add("@CountryID", payload.CountryID);
                    param.Add("@StateID", payload.StateID);
                    param.Add("@LgaID", payload.LgaID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_Branch", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }



            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }

        public async Task<int> UpdateBranch(UpdateBranchDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.UPDATE);
                    param.Add("@BranchName", payload.BranchName);
                    param.Add("@BranchID", payload.BranchID);
                    param.Add("@CompanyID", payload.CompanyID);
                    param.Add("@IsHeadQuater", payload.IsHeadQuater);
                    param.Add("@Address", payload.Address);
                    param.Add("@CountryID", payload.CountryID);
                    param.Add("@StateID", payload.StateID);
                    param.Add("@LgaID", payload.LgaID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_Branch", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }




            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }
        public async Task<int> DeleteBranch(DeleteBranchDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DELETE);
                    param.Add("@CompanyID", payload.CompanyID);
                    param.Add("@BranchID", payload.BranchID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_Branch", param: param, commandType: CommandType.StoredProcedure);
                    return response;

                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }
        public async Task<IEnumerable<BranchDTO>> GetAllBranches(int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.SELECTALL);
                    param.Add("@CompanyID", CompanyID);
                    var response = await _dapper.QueryAsync<BranchDTO>("Sp_Branch", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }

        }

        public async Task<IEnumerable<BranchDTO>> GetBranchByID(DeleteBranchDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.SELECTBYID);
                    param.Add("@BranchID", payload.BranchID);
                    param.Add("@CompanyID", payload.CompanyID);
                    var response = await _dapper.QueryAsync<BranchDTO>("Sp_Branch", param: param, commandType: CommandType.StoredProcedure);
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
