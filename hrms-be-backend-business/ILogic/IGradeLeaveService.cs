using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface IGradeLeaveService
    {
        Task<BaseResponse> CreateGradeLeave(CreateGradeLeaveDTO creatDto, RequesterInfo requester);
        Task<BaseResponse> UpdateGradeLeave(UpdateGradeLeaveDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteGradeLeave(DeleteGradeLeaveDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveGradeLeave(RequesterInfo requester);
        Task<BaseResponse> GetAllGradeLeave(RequesterInfo requester);
        Task<BaseResponse> GetGradeLeaveById(long GradeLeaveID, RequesterInfo requester);
        Task<BaseResponse> GetGradeLeavebyCompanyId(long companyId, RequesterInfo requester);
    }
}
