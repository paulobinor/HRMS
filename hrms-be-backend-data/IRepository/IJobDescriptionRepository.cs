using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface IJobDescriptionRepository
    {
        Task<string> ProcessJobDescription(ProcessJobDescriptionReq payload);
        Task<string> DeleteJobDescription(DeleteJobDescriptionReq payload);
        Task<JobDescriptionWithTotalVm> GetJobDescriptions(long CompanyId, int PageNumber, int RowsOfPage);
        Task<JobDescriptionWithTotalVm> GetJobDescriptionsDeleted(long CompanyId, int PageNumber, int RowsOfPage);
        Task<JobDescriptionVm> GetJobDescriptionById(long Id);
        Task<JobDescriptionVm> GetJobDescriptionByName(string JobDescriptionName, long CompanyId);
    }
}
