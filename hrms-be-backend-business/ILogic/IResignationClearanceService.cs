using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
{
    public interface IResignationClearanceService
    {
        Task<BaseResponse> SubmitResignationClearance(RequesterInfo requesterInfo, ResignationClearanceVM payload);
    }
}
