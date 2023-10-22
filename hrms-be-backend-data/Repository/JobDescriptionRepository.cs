using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{

    public class JobDescriptionRepository : IJobDescriptionRepository
    {
        private readonly ILogger<JobDescriptionRepository> _logger;
        private readonly IDapperGenericRepository _dapper;

        public JobDescriptionRepository(IConfiguration configuration, ILogger<JobDescriptionRepository> logger, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
        }

        public async Task<string> ProcessJobDescription(ProcessJobDescriptionReq payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@JobDescriptionId", payload.JobDescriptionId);
                param.Add("@JobDescriptionName", payload.JobDescriptionName);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                param.Add("@IsModifield", payload.IsModifield);

                return await _dapper.Get<string>("sp_process_job_description", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"JobDescriptionRepository -> ProcessJobDescription => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> DeleteJobDescription(DeleteJobDescriptionReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@JobDescriptionId", payload.JobDescriptionId);
                param.Add("@DeletedComment", payload.Comment);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                return await _dapper.Get<string>("sp_delete_job_description", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> DeleteJobDescription => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<JobDescriptionWithTotalVm> GetJobDescriptions(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new JobDescriptionWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@PageNumber", PageNumber);
                param.Add("@CompanyId", CompanyId);
                var result = await _dapper.GetMultiple("sp_get_job_descriptions", param, gr => gr.Read<long>(), gr => gr.Read<JobDescriptionVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<JobDescriptionVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"JobDescriptionRepository -> GetJobDescriptiones => {ex}");
                return returnData;
            }

        }
        public async Task<JobDescriptionWithTotalVm> GetJobDescriptionsDeleted(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new JobDescriptionWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@PageNumber", PageNumber);
                param.Add("@CompanyId", CompanyId);
                var result = await _dapper.GetMultiple("sp_get_job_description_deleted", param, gr => gr.Read<long>(), gr => gr.Read<JobDescriptionVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<JobDescriptionVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"JobDescriptionRepository -> GetJobDescriptionesDeleted => {ex}");
                return returnData;
            }

        }
        public async Task<JobDescriptionVm> GetJobDescriptionById(long Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                return await _dapper.Get<JobDescriptionVm>("sp_get_job_description_by_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"JobDescriptionRepository -> GetJobDescriptionById => {ex}");
                return new JobDescriptionVm();
            }
        }
        public async Task<JobDescriptionVm> GetJobDescriptionByName(string JobDescriptionName, long CompanyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@JobDescriptionName", JobDescriptionName);
                param.Add("@CompanyId", CompanyId);
                return await _dapper.Get<JobDescriptionVm>("sp_get_job_description_by_name", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"JobDescriptionRepository -> GetJobDescriptionByName => {ex}");
                return new JobDescriptionVm();
            }
        }
    }
}

