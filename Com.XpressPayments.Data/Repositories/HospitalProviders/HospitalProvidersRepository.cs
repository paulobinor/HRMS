using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.Repositories.HOD;
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

namespace Com.XpressPayments.Data.Repositories.HospitalProviders
{
    public class HospitalProvidersRepository : IHospitalProvidersRepository
    {
        private string _connectionString;
        private readonly ILogger<HospitalProvidersRepository> _logger;
        private readonly IConfiguration _configuration;

        public HospitalProvidersRepository(IConfiguration configuration, ILogger<HospitalProvidersRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateHospitalProviders(CreateHospitalProvidersDTO create, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HospitalProvidersEnum.CREATE);
                    param.Add("@ProvidersNames", create.ProvidersNames.Trim());
                    param.Add("@State", create.State.Trim());
                    param.Add("@Town1", create.Town1.Trim());
                    param.Add("@Town2", create.Town2.Trim());
                    param.Add("@Address1", create.Address1.Trim());
                    param.Add("@Address2", create.Address2.Trim());
                    param.Add("@HospitalPlan", create.HospitalPlan.Trim());
                    param.Add("@CompanyId", create.CompanyID);

                    param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_HospitalProviders, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateHospitalProviders(CreateHospitalProvidersDTO create, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateHospitalProviders(UpdateHospitalProvidersDTO update, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HospitalProvidersEnum.UPDATE);
                    param.Add("@IDUpd", update.ID);
                    param.Add("@ProvidersNamesUpd", update.ProvidersNames.Trim());
                    param.Add("@StateUpd", update.State.Trim());
                    param.Add("@Town1Upd", update.Town1.Trim());
                    param.Add("@Town2Upd", update.Town2.Trim());
                    param.Add("@Address1Upd", update.Address1.Trim());
                    param.Add("@Address2Upd", update.Address2.Trim());
                    param.Add("@HospitalPlanUpd", update.HospitalPlan.Trim());
                    param.Add("@CompanyIdUpd", update.CompanyID);

                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_HospitalProviders, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateHospitalProviders(UpdateHospitalProvidersDTO update, string updatedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteHospitalProviders(DeleteHospitalProvidersDTO del, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HospitalProvidersEnum.DELETE);
                    param.Add("@IDDelete", Convert.ToInt32(del.ID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Deleting_Department", del.Reasons_For_Delete == null ? "" : del.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_HospitalProviders, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteHospitalProviders(DeleteHospitalProvidersDTO del, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<HospitalProvidersDTO>> GetAllActiveHospitalProviders()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HospitalProvidersEnum.GETALLACTIVE);

                    var Details = await _dapper.QueryAsync<HospitalProvidersDTO>(ApplicationConstant.Sp_HospitalProviders, param: param, commandType: CommandType.StoredProcedure);

                    return Details;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActiveHospitalProviders() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<HospitalProvidersDTO>> GetAllHospitalProviders()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HospitalProvidersEnum.GETALL);

                    var Details = await _dapper.QueryAsync<HospitalProvidersDTO>(ApplicationConstant.Sp_HospitalProviders, param: param, commandType: CommandType.StoredProcedure);

                    return Details;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:GetAllHospitalProviders() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<HospitalProvidersDTO> GetHospitalProvidersById(long ID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HODenum.GETBYID);
                    param.Add("@IDGet", ID);

                    var Details = await _dapper.QueryFirstOrDefaultAsync<HospitalProvidersDTO>(ApplicationConstant.Sp_HospitalProviders, param: param, commandType: CommandType.StoredProcedure);

                    return Details;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetHospitalProvidersById(long ID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<HospitalProvidersDTO> GetHospitalProvidersByName(string ProvidersNames)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HODenum.GETBYEMAIL);
                    param.Add("@ProvidersNamesGet", ProvidersNames);

                    var HospitalProvidersDetails = await _dapper.QueryFirstOrDefaultAsync<HospitalProvidersDTO>(ApplicationConstant.Sp_HospitalProviders, param: param, commandType: CommandType.StoredProcedure);

                    return HospitalProvidersDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetHospitalProvidersByName(string ProvidersNames) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<HodDTO>> GetAllHospitalProvidersCompanyId(long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", 8);
                    param.Add("@CompanyIdGet", companyId);

                    var Details = await _dapper.QueryAsync<HodDTO>(ApplicationConstant.Sp_HospitalProviders, param: param, commandType: CommandType.StoredProcedure);

                    return Details;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllHospitalProvidersCompanyId(long companyId) ===>{ex.Message}");
                throw;
            }
        }


    }
}
