using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface ILeaveTypeService
    {
        Task<BaseResponse> CreateLeaveType(CreateLeaveTypeDTO creatDto, RequesterInfo requester);
        Task<BaseResponse> UpdateLeaveType(UpdateLeaveTypeDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteLeaveType(DeleteLeaveTypeDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveLeaveType(RequesterInfo requester);
        Task<BaseResponse> GetAllLeaveType(RequesterInfo requester);
        Task<BaseResponse> GetLeaveTypeById(long LeaveTypeId, RequesterInfo requester);
        Task<BaseResponse> GetLeavebyCompanyId(long companyId, hrms_be_backend_data.ViewModel.UserFullView accessUser);
    }
}
