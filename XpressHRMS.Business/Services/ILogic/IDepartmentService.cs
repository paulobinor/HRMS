using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface IDepartmentService
    {
        Task<BaseResponse> ActivateDepartment(DeleteDepartmentDTO payload);
        Task<BaseResponse> CreateDepartment(DepartmentDTO payload);
        Task<BaseResponse> DeleteDepartment(DeleteDepartmentDTO payload);
        Task<BaseResponse> DisableDepartment(DeleteDepartmentDTO payload);
        Task<BaseResponse> UpdateDepartment(UpdateDepartmentDTO payload);
        Task<BaseResponse> GetAllDepartmentByID(int CompanyID, int DepartmentID);
        Task<BaseResponse> GetAllDepartments(int CompanyID);
    }
}