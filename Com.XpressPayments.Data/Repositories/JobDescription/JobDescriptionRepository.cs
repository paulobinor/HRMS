using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.Repositories.UnitHead;
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

namespace Com.XpressPayments.Data.Repositories.JobDescription
{ 

    public class JobDescriptionRepository : IJobDescriptionRepository
    {
        private string _connectionString;
        private readonly ILogger<JobDescriptionRepository> _logger;
        private readonly IConfiguration _configuration;

        public JobDescriptionRepository(IConfiguration configuration, ILogger<JobDescriptionRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateJobDescription(CreateJobDescriptionDTO create, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", JobDescriptionEnum.CREATE);
                    param.Add("@JobDescriptionName", create.JobDescriptionName.Trim());
                    param.Add("@CompanyId", create.CompanyID);

                    param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());
                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_JobDescription, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateJobDescription(CreateJobDescriptionDTO create, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateJobDescription(UpdateJobDescriptionDTO update, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", JobDescriptionEnum.UPDATE);
                    param.Add("@JobDescriptionID", update.JobDescriptionID);
                    param.Add("@CompanyId", update.CompanyID);

                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_JobDescription, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateJobDescription(UpdateJobDescriptionDTO update, string updatedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteJobDescription(DeletedJobDescriptionDTO delete, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", JobDescriptionEnum.DELETE);
                    param.Add("@JobDescriptionIDGet", Convert.ToInt32(delete.JobDescriptionID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Deleting", delete.Reasons_For_Delete == null ? "" : delete.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_JobDescription, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteJobDescription(DeletedJobDescriptionDTO delete, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<JobDescriptionDTO>> GetAllActiveJobDescription()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", JobDescriptionEnum.GETALLACTIVE);

                    var JobDescriptionDetails = await _dapper.QueryAsync<JobDescriptionDTO>(ApplicationConstant.Sp_JobDescription, param: param, commandType: CommandType.StoredProcedure);

                    return JobDescriptionDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActiveJobDescription() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<JobDescriptionDTO>> GetAllJobDescription()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", JobDescriptionEnum.GETALL);

                    var JobDescriptionDetails = await _dapper.QueryAsync<JobDescriptionDTO>(ApplicationConstant.Sp_JobDescription, param: param, commandType: CommandType.StoredProcedure);

                    return JobDescriptionDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:GetAllJobDescription() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<JobDescriptionDTO> GetJobDescriptionById(long JobDescriptionID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", JobDescriptionEnum.GETBYID);
                    param.Add("@JobDescriptionID", JobDescriptionID);

                    var JobDescriptionDetails = await _dapper.QueryFirstOrDefaultAsync<JobDescriptionDTO>(ApplicationConstant.Sp_JobDescription, param: param, commandType: CommandType.StoredProcedure);

                    return JobDescriptionDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetJobDescriptionById(long JobDescriptionID)===>{ex.Message}");
                throw;
            }
        }

        public async Task<UnitHeadDTO> GetJobDescriptionByName(string JobDescriptionName)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", JobDescriptionEnum.GETBYEMAIL);
                    param.Add("@JobDescriptionNameGet", JobDescriptionName);

                    var JobDescriptionDetails = await _dapper.QueryFirstOrDefaultAsync<UnitHeadDTO>(ApplicationConstant.Sp_JobDescription, param: param, commandType: CommandType.StoredProcedure);

                    return JobDescriptionDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetJobDescriptionByName(string JobDescriptionName) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<UnitHeadDTO>> GetAllJobDescriptionCompanyId(long JobDescriptionID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", 8);
                    param.Add("@CompanyIdGet", JobDescriptionID);

                    var UnitHeadDetails = await _dapper.QueryAsync<UnitHeadDTO>(ApplicationConstant.Sp_JobDescription, param: param, commandType: CommandType.StoredProcedure);

                    return UnitHeadDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllJobDescriptionCompanyId(long JobDescriptionID) ===>{ex.Message}");
                throw;
            }
        }



    } 
}

