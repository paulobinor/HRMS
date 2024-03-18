using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface IEmpLocationService
    {
        Task<BaseResponse> CreateEmpLocation(CreateEmpLocationDTO creatDto, RequesterInfo requester);
        Task<BaseResponse> UpdateEmpLocation(UpdateEmpLocationDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteEmpLocation(DeleteEmpLocationDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveEmpLocation(RequesterInfo requester);
        Task<BaseResponse> GetAllEmpLocation(RequesterInfo requester);
        Task<BaseResponse> GetEmpLocationbyId(long EmpLocationID, RequesterInfo requester);
        Task<BaseResponse> GetEmpLocationbyCompanyId(long CompanyID, RequesterInfo requester);
    }
}
