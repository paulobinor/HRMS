using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface IDeductionsRepository
    {
        Task<string> ProcessDeductions(DeductionReq payload);
        Task<string> DeleteDeduction(DeductionDeleteReq payload);
        Task<List<DeductionVm>> GetDeduction(long CompanyId);
        Task<DeductionVm> GetDeductionById(long Id);

        Task<string> ProcessDeductionComputation(DeductionComputationReq payload);
        Task<List<DeductionComputationVm>> GetDeductionComputation(long DeductionId);
    }
}
