using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public  interface IGenderService
    {
        Task<BaseResponse> GetAllGender(RequesterInfo requester);
    }
}
