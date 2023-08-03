using Com.XpressPayments.Common.ViewModels;
using Com.XpressPayments.Data;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.GenericResponse;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.ExitModuleService.Services.ILogic
{
    public interface IResignationService
    {
        Task<BaseResponse> SubmitResignation(RequesterInfo requesterInfo, ResignationRequestVM request);
        Task<BaseResponse> UploadLetter(IFormFile signedResignationLetter);
        Task<BaseResponse> GetResignationByID(long ID, RequesterInfo requester);
        Task<BaseResponse> GetResignationByUserID(long UserID, RequesterInfo requester);
        Task<BaseResponse> GetResignationByCompanyID(long companyID, bool isApproved, RequesterInfo requester);
        Task<BaseResponse> DeleteResignation(DeleteResignationDTO request, RequesterInfo requester);
        Task<BaseResponse> GetPendingResignationByUserID(RequesterInfo requester, long userID);
        Task<BaseResponse> ApprovePendingResignation(ApprovePendingResignationDTO request, RequesterInfo requester);
        Task<BaseResponse> DisapprovePendingResignation(DisapprovePendingResignation request, RequesterInfo requester);
    }
}
