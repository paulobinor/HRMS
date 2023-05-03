using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.Repositories.Gender;
using Com.XpressPayments.Data.Repositories.HospitalPlan;
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

namespace Com.XpressPayments.Data.Repositories.MaritalStatus
{
    public  class HospitalPlanRepository : IHospitalPlanRepository
    {

        private string _connectionString;
        private readonly ILogger<HospitalPlanRepository> _logger;
        private readonly IConfiguration _configuration;

        public HospitalPlanRepository(IConfiguration configuration, ILogger<HospitalPlanRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IEnumerable<HospitalPlanDTO>> GetAllHospitalPlan()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HospitalPlanEnum.GETALL);

                    var HospitalPlanDetails = await _dapper.QueryAsync<HospitalPlanDTO>(ApplicationConstant.Sp_HospitalPlan, param: param, commandType: CommandType.StoredProcedure);

                    return HospitalPlanDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllHospitalPlan() ===>{ex.Message}");
                throw;
            }
        }
    }
}
