using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.Repositories.JobDescription;
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

namespace Com.XpressPayments.Data.Repositories.EmployeeType
{
    public  class EmployeeTypeRepository : IEmployeeTypeRepository
    {
        private string _connectionString;
        private readonly ILogger<EmployeeTypeRepository> _logger;
        private readonly IConfiguration _configuration;

        public EmployeeTypeRepository(IConfiguration configuration, ILogger<EmployeeTypeRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateEmployeeType(CraeteEmployeeTypeDTO create, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmployeeTypeEnum.CREATE);
                    param.Add("@EmployeeTypeName", create.EmployeeTypeName.Trim());
                    param.Add("@CompanyID", create.CompanyID);

                    param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());
                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_EmployeeType, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateEmployeeType(CraeteEmployeeTypeDTO create, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateEmployeeType(UpdateEmployeeTypeDTO update, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmployeeTypeEnum.UPDATE);
                    param.Add("@EmployeeTypeIDUpd", update.EmployeeTypeID);
                    param.Add("@EmployeeTypeNameUpd", update.EmployeeTypeName);
                    param.Add("@CompanyIdUpd", update.CompanyID);

                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_EmployeeType, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateEmployeeType(UpdateEmployeeTypeDTO update, string updatedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteEmployeeType(DeleteEmployeeTypeDTO delete, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmployeeTypeEnum.DELETE);
                    param.Add("@EmployeeTypeIDDelete", Convert.ToInt32(delete.EmployeeTypeID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Deleting", delete.Reasons_For_Delete == null ? "" : delete.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_EmployeeType, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteEmployeeType(DeleteEmployeeTypeDTO delete, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<EmployeeTypeDTO>> GetAllActiveEmployeeType()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmployeeTypeEnum.GETALLACTIVE);

                    var EmployeeTypeDetails = await _dapper.QueryAsync<EmployeeTypeDTO>(ApplicationConstant.Sp_EmployeeType, param: param, commandType: CommandType.StoredProcedure);

                    return EmployeeTypeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActiveEmployeeType() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<EmployeeTypeDTO>> GetAllEmployeeType()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmployeeTypeEnum.GETALL);

                    var EmployeeTypeDetails = await _dapper.QueryAsync<EmployeeTypeDTO>(ApplicationConstant.Sp_EmployeeType, param: param, commandType: CommandType.StoredProcedure);

                    return EmployeeTypeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:GetAllActiveEmployeeType() ===>{ex.Message}");
                throw;
            }
        }
        public async Task<EmployeeTypeDTO> GetEmployeeTypeById(long EmployeeTypeID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmployeeTypeEnum.GETBYID);
                    param.Add("@EmployeeTypeIDUpd", EmployeeTypeID);

                    var EmployeeTypeDetails = await _dapper.QueryFirstOrDefaultAsync<EmployeeTypeDTO>(ApplicationConstant.Sp_EmployeeType, param: param, commandType: CommandType.StoredProcedure);

                    return EmployeeTypeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetEmployeeTypeById(long EmployeeTypeID)===>{ex.Message}");
                throw;
            }
        }

        public async Task<EmployeeTypeDTO> GetEmployeeTypeByName(string EmployeeTypeName)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmployeeTypeEnum.GETBYEMAIL);
                    param.Add("@EmployeeTypeNameGet", EmployeeTypeName);

                    var EmployeeTypeDetails = await _dapper.QueryFirstOrDefaultAsync<EmployeeTypeDTO>(ApplicationConstant.Sp_EmployeeType, param: param, commandType: CommandType.StoredProcedure);

                    return EmployeeTypeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetEmployeeTypeByName(string EmployeeTypeName) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<EmployeeTypeDTO>> GetAllEmployeeTypeCompanyId(long EmployeeTypeID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", 8);
                    param.Add("@CompanyIdGet", EmployeeTypeID);

                    var EmployeeTypeDetails = await _dapper.QueryAsync<EmployeeTypeDTO>(ApplicationConstant.Sp_EmployeeType, param: param, commandType: CommandType.StoredProcedure);

                    return EmployeeTypeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllEmployeeTypeCompanyId(long EmployeeTypeID) ===>{ex.Message}");
                throw;
            }
        }


    }
}
