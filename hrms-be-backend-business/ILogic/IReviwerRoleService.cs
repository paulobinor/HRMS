using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface IReviwerRoleService
    {
        Task<BaseResponse> GetReviwerRolebyId(long ReviwerRoleID, RequesterInfo requester);
        Task<BaseResponse> GetReviwerRolebyCompanyId(long companyId, RequesterInfo requester);
    }
}
