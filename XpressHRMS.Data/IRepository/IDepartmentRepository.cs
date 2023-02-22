using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface IDepartmentRepository
    {
        Task<int> CreateDepartment(CreateDepartmentDTO payload);
        Task<int> DeleteDepartment(int DepartmentID, int CompanyID);
        Task<int> DisableDepartment(int DepartmentID, int CompanyID);
        Task<int> ActivateDepartment(int DepartmentID, int CompanyID);
        Task<List<GetDepartmentDTO>> GetAllDepartment(int CompanyID);
        Task<IEnumerable<GetDepartmentDTO>> GetAllDepartmentByID(int DepartmentID, int CompanyID);
        Task<int> UpdateDepartment(UpdateDepartmentDTO payload);
    }
}