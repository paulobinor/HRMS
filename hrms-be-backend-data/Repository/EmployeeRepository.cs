using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private string _connectionString;
        private readonly ILogger<EmployeeRepository> _logger;
        private readonly IDapperGenericRepository _dapper;

        public EmployeeRepository(ILogger<EmployeeRepository> logger, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
        }
        public async Task<string> ProcessEmployeeBasis(ProcessEmployeeBasisReq payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", payload.EmployeeId);
                param.Add("@FirstName", payload.FirstName);
                param.Add("@MiddleName", payload.MiddleName);
                param.Add("@LastName", payload.LastName);
                param.Add("@DOB", payload.DOB);
                param.Add("@PersonalEmail", payload.PersonalEmail);
                param.Add("@OfficialEmail", payload.OfficialEmail);
                param.Add("@PhoneNumber", payload.PhoneNumber);
                param.Add("@EmploymentStatusId", payload.EmploymentStatusId);
                param.Add("@BranchId", payload.BranchId);
                param.Add("@EmployeeTypeId", payload.EmployeeTypeId);
                param.Add("@DepartmentId", payload.DepartmentId);
                param.Add("@ResumptionDate", payload.ResumptionDate);
                param.Add("@JobRoleId", payload.JobRoleId);
                param.Add("@UnitId", payload.UnitId);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                param.Add("@IsModifield", payload.IsModifield);

                return await _dapper.Get<string>("sp_process_employee_basis", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => ProcessEmployeeBasis ===> {ex.Message}");
                throw;
            }
        }
        public async Task<string> ApproveEmployee(long Id, long CreatedByUserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                param.Add("@CreatedByUserId", CreatedByUserId);
                param.Add("@DateCreated", DateTime.Now);

                return await _dapper.Get<string>("sp_approve_employee", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => ApproveEmployee ===> {ex.Message}");
                throw;
            }
        }
        public async Task<string> DisapproveEmployee(long Id, string Comment, long CreatedByUserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                param.Add("@Comment", Comment);
                param.Add("@CreatedByUserId", CreatedByUserId);
                param.Add("@DateCreated", DateTime.Now);

                return await _dapper.Get<string>("sp_disapprove_employee", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => DisapproveEmployee ===> {ex.Message}");
                throw;
            }
        }
        public async Task<string> DeleteEmployee(long Id, string Comment, long CreatedByUserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                param.Add("@Comment", Comment);
                param.Add("@CreatedByUserId", CreatedByUserId);
                param.Add("@DateCreated", DateTime.Now);

                return await _dapper.Get<string>("sp_delete_employee", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => DeleteEmployee ===> {ex.Message}");
                throw;
            }
        }
        public async Task<EmployeeWithTotalVm> GetEmployees(int PageNumber, int RowsOfPage, long AccessByUserId)
        {
            var returnData = new EmployeeWithTotalVm();
            try
            {
                var param = new DynamicParameters();

                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@AccessByUserId", AccessByUserId);
                var result = await _dapper.GetMultiple("sp_get_employees", param, gr => gr.Read<long>(), gr => gr.Read<EmployeeVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<EmployeeVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository -> GetEmployees => {ex}");
                return returnData;
            }

        }
        public async Task<EmployeeWithTotalVm> GetEmployeesApproved(int PageNumber, int RowsOfPage, long AccessByUserId)
        {
            var returnData = new EmployeeWithTotalVm();
            try
            {
                var param = new DynamicParameters();

                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@AccessByUserId", AccessByUserId);
                var result = await _dapper.GetMultiple("sp_get_employees_approved", param, gr => gr.Read<long>(), gr => gr.Read<EmployeeVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<EmployeeVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository -> GetEmployeesApproved => {ex}");
                return returnData;
            }

        }
        public async Task<EmployeeWithTotalVm> GetEmployeesDisapproved(int PageNumber, int RowsOfPage, long AccessByUserId)
        {
            var returnData = new EmployeeWithTotalVm();
            try
            {
                var param = new DynamicParameters();

                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@AccessByUserId", AccessByUserId);
                var result = await _dapper.GetMultiple("sp_get_employees_disapproved", param, gr => gr.Read<long>(), gr => gr.Read<EmployeeVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<EmployeeVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository -> GetEmployeesDisapproved => {ex}");
                return returnData;
            }

        }
        public async Task<EmployeeWithTotalVm> GetEmployeesDeleted(int PageNumber, int RowsOfPage, long AccessByUserId)
        {
            var returnData = new EmployeeWithTotalVm();
            try
            {
                var param = new DynamicParameters();

                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@AccessByUserId", AccessByUserId);
                var result = await _dapper.GetMultiple("sp_get_employees_deleted", param, gr => gr.Read<long>(), gr => gr.Read<EmployeeVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<EmployeeVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository -> GetEmployeesDeleted => {ex}");
                return returnData;
            }

        }
        public async Task<EmployeeFullVm> GetEmployeeById(long Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                return await _dapper.Get<EmployeeFullVm>("sp_get_employee_by_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository => GetEmployeeById || {ex}");
                return new EmployeeFullVm();
            }
        }
        public async Task<int> AddEmployeeBulk(DataTable dataTable, RequesterInfo requester, long currentStaffCount, int listCount, long companyID)
        {
            try
            {
                var param = new
                {
                    DateCreated = DateTime.Now,
                    CreatedBy = requester.Username,
                    UserID = requester.UserId,
                    CurrentStaffCount = currentStaffCount,
                    Count = listCount,
                    CompanyID = companyID,
                    Users = dataTable.AsTableValuedParameter("UserType"),
                };
                var resp = await _dapper.BulkInsert<int>(param, "sp_CreateEmployeeBulk");
                return resp;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: AddUserBulk(AddEmployeeBulk user) ===>{ex.Message}");
                throw;
            }
        }
        public async Task<EmployeeFullVm> GetEmployeeByUserId(long UserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", UserId);
                return await _dapper.Get<EmployeeFullVm>("sp_get_employee_by_userid", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository => GetEmployeeByUserId || {ex}");
                return new EmployeeFullVm();
            }
        }

    }
}
