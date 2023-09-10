using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IDepartmentRepository
    {
        Task<dynamic> CreateDepartment(CreateDepartmentDto Department, string createdbyUserEmail);
        Task<dynamic> UpdateDepartment(UpdateDepartmentDto Department, string updatedbyUserEmail);
        Task<dynamic> DeleteDepartment(DeleteDepartmentDto Department, string deletedbyUserEmail);
        Task<IEnumerable<DepartmentsDTO>> GetAllActiveDepartments();
        Task<IEnumerable<DepartmentsDTO>> GetAllDepartments();
        Task<DepartmentsDTO> GetDepartmentById(long DepartmentId);
        Task<DepartmentsDTO> GetDepartmentByName(string DepartmentEmail);
        Task<DepartmentsDTO> GetDepartmentByCompany(string DepartmentName, int companyId);
        Task<IEnumerable<DepartmentsDTO>> GetAllDepartmentsbyCompanyId(long companyId);
    }
}
