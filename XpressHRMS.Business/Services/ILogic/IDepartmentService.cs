using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface IDepartmentService
    {
       
        Task<BaseResponse<DepartmentDTO>> CreateDepartment(CreateDepartmentDTO payload);
        Task<BaseResponse<DeleteDepartmentDTO>> DeleteDepartment(DeleteDepartmentDTO payload);
        Task<BaseResponse<UpdateDepartmentDTO>> UpdateDepartment(UpdateDepartmentDTO payload);
        Task<BaseResponse<GetDepartmentDTO>> GetAllDepartmentByID(string CompanyID, int DepartmentID);
        Task<BaseResponse<List<GetDepartmentDTO>>> GetAllDepartments(string CompanyID);
        Task<BaseResponse<DisDepartmentDTO>> DisableDepartment(DisDepartmentDTO Disdepartment, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<DisDepartmentDTO>> ActivateDepartment(EnDepartmentDTO enable, string RemoteIpAddress, string RemotePort);
    }
}