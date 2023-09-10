using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface ILgaService
    {
        Task<BaseResponse> GetAllLga(long StateID, RequesterInfo requester);
        Task<BaseResponse> GetLgaByStateId(long StateID, RequesterInfo requester);


    }
}
