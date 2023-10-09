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
    public class HospitalPlanRepository : IHospitalPlanRepository
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

        public async Task<HospitalPlanDTO> GetHospitalPlanByName(string HospitalPlan)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HospitalPlanEnum.GETBYNAME);
                    param.Add("@HospitalPlanGet", HospitalPlan);

                    var GradeDetails = await _dapper.QueryFirstOrDefaultAsync<HospitalPlanDTO>(ApplicationConstant.Sp_HospitalPlan, param: param, commandType: CommandType.StoredProcedure);

                    return GradeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetHospitalPlanByName(string HospitalPlan) ===>{ex.Message}");
                throw;
            }
        }
    }
}
