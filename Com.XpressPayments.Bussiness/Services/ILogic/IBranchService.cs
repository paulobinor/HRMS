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
    public interface IBranchService
    {
        Task<BaseResponse> CreateBranch(CreateBranchDTO BranchDto, RequesterInfo requester);
        Task<BaseResponse> CreateBranchBulkUpload(IFormFile payload, RequesterInfo requester);
        Task<BaseResponse> UpdateBranch(UpdateBranchDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteBranch(DeleteBranchDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveBranch(RequesterInfo requester);
        Task<BaseResponse> GetAllBranch(RequesterInfo requester);
        Task<BaseResponse> GetBranchbyId(long BranchID, RequesterInfo requester);
        Task<BaseResponse> GetBranchbyCompanyId(long companyId, RequesterInfo requester);



    }
}
