using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface IGradeLeaveService
    {
        Task<BaseResponse> CreateGradeLeave(CreateGradeLeaveDTO creatDto, string AccessKey, string RemoteIpAddress);
        Task<BaseResponse> UpdateGradeLeave(UpdateGradeLeaveDTO updateDto);
        Task<BaseResponse> DeleteGradeLeave(DeleteGradeLeaveDTO deleteDto);
        Task<BaseResponse> GetAllActiveGradeLeave(RequesterInfo requester);
        Task<BaseResponse> GetAllGradeLeave( string AccessKey, string RemoteIpAddress);
        Task<BaseResponse> GetGradeLeaveById(long GradeLeaveID, RequesterInfo requester);
        Task<BaseResponse> GetGradeLeavebyCompanyId(long companyId , string AccessKey, string RemoteIpAddress);
    }
}
