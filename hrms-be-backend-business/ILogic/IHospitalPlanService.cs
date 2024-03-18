using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface IHospitalPlanService
    {
        Task<BaseResponse> GetAllHospitalPlan(RequesterInfo requester);
    }
}
