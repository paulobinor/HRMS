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
    public class EmployeeTypeRepository : IEmployeeTypeRepository
    {
        private readonly ILogger<EmployeeTypeRepository> _logger;
        private readonly IDapperGenericRepository _dapper;

        public EmployeeTypeRepository(IConfiguration configuration, ILogger<EmployeeTypeRepository> logger, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
        }

        public async Task<string> ProcessEmployeeType(ProcessEmployeeTypeReq payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeTypeId", payload.EmployeeTypeId);
                param.Add("@EmployeeTypeName", payload.EmployeeTypeName);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                param.Add("@IsModifield", payload.IsModifield);

                return await _dapper.Get<string>("sp_process_employee_type", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeTypeRepository -> ProcessEmployeeType => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> DeleteEmployeeType(DeleteEmployeeTypeReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@EmployeeTypeId", payload.EmployeeTypeId);
                param.Add("@DeletedComment", payload.Comment);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                return await _dapper.Get<string>("sp_delete_employee_type", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> DeleteEmployeeType => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<EmployeeTypeWithTotalVm> GetEmployeeTypes(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new EmployeeTypeWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@PageNumber", PageNumber);
                param.Add("@CompanyId", CompanyId);
                var result = await _dapper.GetMultiple("sp_get_employee_types", param, gr => gr.Read<long>(), gr => gr.Read<EmployeeTypeVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<EmployeeTypeVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeTypeRepository -> GetEmployeeTypees => {ex}");
                return returnData;
            }

        }

        public async Task<List<EmployeeTypeVm>> GetEmployeeTypes(long CompanyId)
        {
            string query = @"Select * from EmployeeType where CompanyId = @CompanyId and IsDeleted = @IsDeleted";
            var param = new DynamicParameters();
            param.Add("CompanyId", CompanyId);
            param.Add("IsDeleted", false);
            return await _dapper.GetAll<EmployeeTypeVm>(query, param, commandType: CommandType.Text);
        }

        public async Task<EmployeeTypeWithTotalVm> GetEmployeeTypesDeleted(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new EmployeeTypeWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@PageNumber", PageNumber);
                param.Add("@CompanyId", CompanyId);
                var result = await _dapper.GetMultiple("sp_get_employee_types_deleted", param, gr => gr.Read<long>(), gr => gr.Read<EmployeeTypeVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<EmployeeTypeVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeTypeRepository -> GetEmployeeTypeesDeleted => {ex}");
                return returnData;
            }

        }
        public async Task<EmployeeTypeVm> GetEmployeeTypeById(long Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                return await _dapper.Get<EmployeeTypeVm>("sp_get_employee_type_by_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeTypeRepository -> GetEmployeeTypeById => {ex}");
                return new EmployeeTypeVm();
            }
        }
        public async Task<EmployeeTypeVm> GetEmployeeTypeByName(string EmployeeTypeName, long CompanyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeTypeName", EmployeeTypeName);
                param.Add("@CompanyId", CompanyId);
                return await _dapper.Get<EmployeeTypeVm>("sp_get_employee_type_by_name", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeTypeRepository -> GetEmployeeTypeByName => {ex}");
                return new EmployeeTypeVm();
            }
        }
    }
}
