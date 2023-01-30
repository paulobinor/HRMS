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
        Task<BaseResponse<CreatePositionDTO>> CreatePosition(CreatePositionDTO createpostion, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<UPdatePositionDTO>> UpdatePosition(UPdatePositionDTO UpdatePosition, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<DeletePositionDTO>> DeletePosition(DeletePositionDTO DelPostion, string RemoteIpAddress, string RemotePort);

        Task<BaseResponse<int>> DisablePosition(int PositionID, int CompanyID, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<int>> ActivatePosition(int PositionID, int CompanyID, string RemoteIpAddress, string RemotePort);

        Task<BaseResponse<List<PositionDTO>>> GetAllPositions(int CompanyID);

        Task<BaseResponse<PositionDTO>> GetPositionByID(int CompanyID, int PositionID);

    }
}
