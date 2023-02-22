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
        Task<BaseResponse<CreateEmployeeTypeDTO>> CreateEmployeeType(CreateEmployeeTypeDTO createEmployeeType, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<UpdateEmployeeTypeDTO>> UpdateEmployeeType(UpdateEmployeeTypeDTO UpdateEmployeeType, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<DelEmployeeTypeDTO>> DeleteEmployeeType(DelEmployeeTypeDTO DelEmployeeType, string RemoteIpAddress, string RemotePort);
        //Task<BaseResponse<DelEmployeeTypeDTO>> DisableEmployeeType(int EmployeeTypeID, int CompanyID, string RemoteIpAddress, string RemotePort);
        //Task<BaseResponse<DelEmployeeTypeDTO>> ActivateEmployeeType(int EmployeeTypeID, int CompanyID, string RemoteIpAddress, string RemotePort);

        Task<BaseResponse<List<EmployeeTypeDTO>>> GetAllEmployeeType(int CompanyID);

        Task<BaseResponse<EmployeeTypeDTO>> GetEmployeeTypeByID(int CompanyID, int PositionID);


    }
}
