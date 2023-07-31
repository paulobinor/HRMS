using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface IHodService
    {
        Task<BaseResponse<CreateHodDTO>> CreateHOD(CreateHodDTO createHOD, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<UpdateHodDTO>> UpdateHOD(UpdateHodDTO UpdateHOD, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<DelHodDTO>> DeleteHOD(DelHodDTO DelHOD, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<int>> DisableHOD(DisableHodDTO disable, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<int>> ActivateHOD(EnableHodDTO enable, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<List<HodDTO>>> GetAllHOD(string CompanyID);
        Task<BaseResponse<List<HodDTO>>> GetHODByID(string CompanyID, int HodID, int DepartmentID);


    }
}
