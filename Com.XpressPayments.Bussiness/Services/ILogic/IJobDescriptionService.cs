using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public interface IJobDescriptionService
    {
        Task<BaseResponse> CreateJobDescription(CreateJobDescriptionDTO creatDto, RequesterInfo requester);
        Task<BaseResponse> UpdateJobDescription(UpdateJobDescriptionDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteJobDescription(DeletedJobDescriptionDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveJobDescription(RequesterInfo requester);
        Task<BaseResponse> GetAllJobDescription(RequesterInfo requester);
        Task<BaseResponse> GetJobDescriptionById(long JobDescriptionID, RequesterInfo requester);
        Task<BaseResponse> GetJobDescriptionbyCompanyId(long companyId, RequesterInfo requester);
    }
}
