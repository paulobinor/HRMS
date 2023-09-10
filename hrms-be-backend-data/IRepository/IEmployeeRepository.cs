using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public  interface IEmployeeRepository
    {
        Task<dynamic> UpdateEmployee(UpdateEmployeeDTO Emp, string Updated_By_User_Email);
        Task<IEnumerable<EmployeeDTO>> GetAllActiveEmployee();
        Task<IEnumerable<EmployeeDTO>> GetAllEmployee();
        Task<EmployeeDTO> GetEmployeeById(long EmpID);
        Task<EmployeeDTO> GetEmployeeByStaffID(long StaffID);
        Task<IEnumerable<EmployeeDTO>> GetAllEmployeeCompanyId(long CompanyId);
        Task<IEnumerable<EmployeeDTO>> GetEmpPendingApproval(long CompanyID);
        Task<dynamic> ApproveEmp(long approvedByuserId, string officialMail);
        Task<dynamic> DeclineEmp(long disapprovedByuserId, string userEmail, string comment);
    }
}
