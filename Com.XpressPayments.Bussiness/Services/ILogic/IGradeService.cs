using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public  interface IGradeService
    {
        Task<BaseResponse> CreateGrade(CreateGradeDTO creatDto, RequesterInfo requester);
        Task<BaseResponse> UpdateGrade(UpdateGradeDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteGrade(DeleteGradeDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveGrade(RequesterInfo requester);
        Task<BaseResponse> GetAllGrade(RequesterInfo requester);
        Task<BaseResponse> GetGradeById(long GradeID, RequesterInfo requester);
    }
}
