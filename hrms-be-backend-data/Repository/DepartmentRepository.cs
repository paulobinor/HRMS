using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {     
        private readonly ILogger<DepartmentRepository> _logger;
        private readonly IDapperGenericRepository _dapper;

        public DepartmentRepository(ILogger<DepartmentRepository> logger, IDapperGenericRepository dapper)
        {           
            _logger = logger;    
            _dapper = dapper;
        }

        public async Task<string> ProcessDepartment(ProcessDepartmentReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@DepartmentId", payload.DepartmentId);
                param.Add("@DepartmentName", payload.DepartmentName);              
                param.Add("@IsHr", payload.IsHr);
                param.Add("@HodEmployeeId", payload.HodEmployeeId);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                param.Add("@IsModifield", payload.IsModifield);

                return await _dapper.Get<string>("sp_process_department", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"DepartmentRepository -> ProcessDepartment => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> DeleteDepartment(DeleteDepartmentReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@DepartmentId", payload.DepartmentId);
                param.Add("@DeletedComment", payload.Comment);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                return await _dapper.Get<string>("sp_delete_department", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> DeleteDepartment => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<DepartmentWithTotalVm> GetDepartmentes(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new DepartmentWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@PageNumber", PageNumber);
                param.Add("@CompanyId", CompanyId);
                var result = await _dapper.GetMultiple("sp_get_departments", param, gr => gr.Read<long>(), gr => gr.Read<DepartmentVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<DepartmentVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"DepartmentRepository -> GetDepartmentes => {ex}");
                return returnData;
            }

        }
        public async Task<DepartmentWithTotalVm> GetDepartmentesDeleted(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new DepartmentWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@PageNumber", PageNumber);
                param.Add("@CompanyId", CompanyId);
                var result = await _dapper.GetMultiple("sp_get_departments_deleted", param, gr => gr.Read<long>(), gr => gr.Read<DepartmentVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<DepartmentVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"DepartmentRepository -> GetDepartmentesDeleted => {ex}");
                return returnData;
            }

        }
        public async Task<DepartmentVm> GetDepartmentById(long Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                return await _dapper.Get<DepartmentVm>("sp_get_department_by_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"DepartmentRepository -> GetDepartmentById => {ex}");
                return new DepartmentVm();
            }
        }
        public async Task<DepartmentVm> GetDepartmentByName(string DepartmentName, long CompanyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@DepartmentName", DepartmentName);
                param.Add("@CompanyId", CompanyId);
                return await _dapper.Get<DepartmentVm>("sp_get_department_by_name", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"DepartmentRepository -> GetDepartmentByName => {ex}");
                return new DepartmentVm();
            }
        }
    }
}
