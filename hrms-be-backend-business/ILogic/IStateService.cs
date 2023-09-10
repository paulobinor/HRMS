using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface IStateService
    {
        Task<BaseResponse> GetAllState(long CountryID, RequesterInfo requester);
        Task<BaseResponse> GetStateByCountryId(long CountryID, RequesterInfo requester);
    }
}
