using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface IPositionService
    {
        Task<BaseResponse> CreatePosition(CreatePositionDTO createpostion, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> UpdatePosition(UPdatePositionDTO UpdatePosition, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> DeletePosition(DeletePositionDTO DelPostion, string RemoteIpAddress, string RemotePort);

        Task<BaseResponse> DisablePosition(int PositionID, int CompanyID, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> ActivatePosition(int PositionID, int CompanyID, string RemoteIpAddress, string RemotePort);
<<<<<<< HEAD
        Task<BaseResponse> GetAllPositions(int CompanyID);
=======
        Task<BaseResponse> GetAllPositions();
>>>>>>> e2edf564460ff757ff7e79041bfc7a224d357bef
        Task<BaseResponse> GetPositionByID(int CompanyID, int PositionID);

    }
}
