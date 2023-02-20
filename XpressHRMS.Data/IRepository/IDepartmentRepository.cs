using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface IDepartmentRepository
    {
        Task<int> CreateDepartment(CreateDepartmentDTO payload);
        Task<int> DeleteDepartment(int DepartmentID, string CompanyID);
        Task<int> DisableDepartment(DisDepartmentDTO diable);
        Task<int> ActivateDepartment(EnDepartmentDTO enable);
        Task<List<GetDepartmentDTO>> GetAllDepartment(string CompanyID);
        Task<IEnumerable<GetDepartmentDTO>> GetAllDepartmentByID(int DepartmentID, string CompanyID);
        Task<int> UpdateDepartment(UpdateDepartmentDTO payload);
    }
}