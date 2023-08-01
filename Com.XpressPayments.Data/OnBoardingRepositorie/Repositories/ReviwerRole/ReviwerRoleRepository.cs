using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.OnBoardingDTO.DTOs;
using Com.XpressPayments.Data.OnBoardingRepositorie.Repositories.Reviwer;
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

namespace Com.XpressPayments.Data.OnBoardingRepositorie.Repositories.ReviwerRole
{
    public  class ReviwerRoleRepository : IReviwerRoleRepository
    {
        private string _connectionString;
        private readonly ILogger<ReviwerRoleRepository> _logger;
        private readonly IConfiguration _configuration;

        public ReviwerRoleRepository(IConfiguration configuration, ILogger<ReviwerRoleRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<ReviwerRoleDTO> GetReviwerRoleById(long ReviwerRoleID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ReviwerEnum.GETBYID);
                    param.Add("@ReviwerRoleID", ReviwerRoleID);

                    var ReviwerRoleDetails = await _dapper.QueryFirstOrDefaultAsync<ReviwerRoleDTO>(ApplicationConstant.Sp_ReviwersRole, param: param, commandType: CommandType.StoredProcedure);

                    return ReviwerRoleDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetReviwerRoleById(long ReviwerRoleID) ===>{ex.Message}");
                throw;
            }
        }
        public async Task<IEnumerable<ReviwerRoleDTO>> GetAllReviwerRoleCompanyId(long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ReviwerRoleEnum.GETBYCOMPANYID);
                    param.Add("@CompanyIdGet", companyId);

                    var ReviwerDetails = await _dapper.QueryAsync<ReviwerRoleDTO>(ApplicationConstant.Sp_ReviwersRole, param: param, commandType: CommandType.StoredProcedure);

                    return ReviwerDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllReviwerRoleCompanyId(long companyId) ===>{ex.Message}");
                throw;
            }
        }

        
    }
}
