using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class EmploymentStatusRepository : IEmploymentStatusRepository
    {
        private readonly ILogger<EmploymentStatusRepository> _logger;
        private readonly IDapperGenericRepository _dapper;

        public EmploymentStatusRepository(IConfiguration configuration, ILogger<EmploymentStatusRepository> logger, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
        }

        public async Task<string> ProcessEmploymentStatus(ProcessEmploymentStatusReq payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmploymentStatusId", payload.EmploymentStatusId);
                param.Add("@EmploymentStatusName", payload.EmploymentStatusName);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                param.Add("@IsModifield", payload.IsModifield);

                return await _dapper.Get<string>("sp_process_employment_status", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"EmploymentStatusRepository -> ProcessEmploymentStatus => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> DeleteEmploymentStatus(DeleteEmploymentStatusReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@EmploymentStatusId", payload.EmploymentStatusId);
                param.Add("@DeletedComment", payload.Comment);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                return await _dapper.Get<string>("sp_delete_employment_status", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> DeleteEmploymentStatus => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<EmploymentStatusWithTotalVm> GetEmploymentStatus(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new EmploymentStatusWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@PageNumber", PageNumber);
                param.Add("@CompanyId", CompanyId);
                var result = await _dapper.GetMultiple("sp_get_employment_status", param, gr => gr.Read<long>(), gr => gr.Read<EmploymentStatusVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<EmploymentStatusVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"EmploymentStatusRepository -> GetEmploymentStatus => {ex}");
                return returnData;
            }

        }
        public async Task<EmploymentStatusWithTotalVm> GetEmploymentStatusDeleted(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new EmploymentStatusWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@PageNumber", PageNumber);
                param.Add("@CompanyId", CompanyId);
                var result = await _dapper.GetMultiple("sp_get_employment_status_deleted", param, gr => gr.Read<long>(), gr => gr.Read<EmploymentStatusVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<EmploymentStatusVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"EmploymentStatusRepository -> GetEmploymentStatusDeleted => {ex}");
                return returnData;
            }

        }
        public async Task<EmploymentStatusVm> GetEmploymentStatusById(long Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                return await _dapper.Get<EmploymentStatusVm>("sp_get_employment_status_by_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmploymentStatusRepository -> GetEmploymentStatusById => {ex}");
                return new EmploymentStatusVm();
            }
        }
        public async Task<EmploymentStatusVm> GetEmploymentStatusByName(string EmploymentStatusName, long CompanyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmploymentStatusName", EmploymentStatusName);
                param.Add("@CompanyId", CompanyId);
                return await _dapper.Get<EmploymentStatusVm>("sp_get_employment_status_by_name", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmploymentStatusRepository -> GetEmploymentStatusByName => {ex}");
                return new EmploymentStatusVm();
            }
        }
    }
}
