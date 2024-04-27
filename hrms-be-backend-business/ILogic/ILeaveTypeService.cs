using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface ILeaveTypeService
    {
        Task<BaseResponse> CreateLeaveType(CreateLeaveTypeDTO creatDto);
        Task<BaseResponse> UpdateLeaveType(UpdateLeaveTypeDTO updateDto);
        Task<BaseResponse> DeleteLeaveType(DeleteLeaveTypeDTO deleteDto);
        Task<BaseResponse> GetAllActiveLeaveType();
        Task<BaseResponse> GetAllLeaveType();
        Task<BaseResponse> GetLeaveTypeById(long LeaveTypeId);
        Task<BaseResponse> GetLeavebyCompanyId(long companyId);
    }
}
