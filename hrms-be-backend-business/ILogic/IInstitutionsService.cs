using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public  interface IInstitutionsService
    {
        Task<BaseResponse> GetAllInstitutions(RequesterInfo requester);
    }
}
