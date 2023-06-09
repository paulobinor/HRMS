using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.GenericResponse;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public interface IPositionService
    {
        Task<BaseResponse> CreatePosition(CreatePositionDTO creatDto, RequesterInfo requester);
        Task<BaseResponse> CreatePositionBulkUpload(IFormFile payload, RequesterInfo requester);
        Task<BaseResponse> UpdatePosition(UpadtePositionDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeletePosition(DeletePositionDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActivePosition(RequesterInfo requester);
        Task<BaseResponse> GetAllPosition(RequesterInfo requester);
        Task<BaseResponse> GetPositionById(long PositionID, RequesterInfo requester);
        Task<BaseResponse> GetPositionbyCompanyId(long companyId, RequesterInfo requester);
    }
}
