using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface IEmployeeTypeService
    {
        Task<BaseResponse> CreateEmployeeType(CreateEmployeeTypeDTO createEmployeeType, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> UpdateEmployeeType(UpdateEmployeeTypeDTO UpdateEmployeeType, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> DeleteEmployeeType(DelEmployeeTypeDTO DelEmployeeType, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> DisableEmployeeType(int EmployeeTypeID, int CompanyID, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> ActivateEmployeeType(int EmployeeTypeID, int CompanyID, string RemoteIpAddress, string RemotePort);
<<<<<<< HEAD
        Task<BaseResponse> GetAllEmployeeType(int CompanyID);
=======
        Task<BaseResponse> GetAllEmployeeType();
>>>>>>> e2edf564460ff757ff7e79041bfc7a224d357bef

        Task<BaseResponse> GetEmployeeTypeByID(int CompanyID, int PositionID);


    }
}
