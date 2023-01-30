using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface IDepartmentService
    {
        //Task<BaseResponse<DeleteDepartmentDTO>> ActivateDepartment(DeleteDepartmentDTO payload);
        Task<BaseResponse<DepartmentDTO>> CreateDepartment(DepartmentDTO payload);
        Task<BaseResponse<DeleteDepartmentDTO>> DeleteDepartment(DeleteDepartmentDTO payload);
        //Task<BaseResponse<DeleteDepartmentDTO>> DisableDepartment(DeleteDepartmentDTO payload);
        Task<BaseResponse<UpdateDepartmentDTO>> UpdateDepartment(UpdateDepartmentDTO payload);
        Task<BaseResponse<GetDepartmentDTO>> GetAllDepartmentByID(int CompanyID, int DepartmentID);
        Task<BaseResponse<List<GetDepartmentDTO>>> GetAllDepartments(int CompanyID);
    }
}