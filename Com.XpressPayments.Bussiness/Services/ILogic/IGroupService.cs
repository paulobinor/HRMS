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
    public interface IGroupService
    {
        Task<BaseResponse> CreateGroup(CreateGroupDTO GroupDto, RequesterInfo requester);
        Task<BaseResponse> CreateGroupBulkUpload(IFormFile payload, RequesterInfo requester);
        Task<BaseResponse> UpdateGroup(UpdateGroupDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteGroup(DeleteGroupDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveGroup(RequesterInfo requester);
        Task<BaseResponse> GetAllGroup(RequesterInfo requester);
        Task<BaseResponse> GetGroupbyId(long GroupID, RequesterInfo requester);
        Task<BaseResponse> GetGroupbyCompanyId(long companyId, RequesterInfo requester);
    }
}
