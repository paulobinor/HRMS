using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IJobDescriptionRepository
    {
        Task<dynamic> CreateJobDescription(CreateJobDescriptionDTO create, string createdbyUserEmail);
        Task<dynamic> UpdateJobDescription(UpdateJobDescriptionDTO update, string updatedbyUserEmail);
        Task<dynamic> DeleteJobDescription(DeletedJobDescriptionDTO delete, string deletedbyUserEmail);
        Task<IEnumerable<JobDescriptionDTO>> GetAllActiveJobDescription();
        Task<IEnumerable<JobDescriptionDTO>> GetAllJobDescription();
        Task<JobDescriptionDTO> GetJobDescriptionById(long JobDescriptionID);
        Task<JobDescriptionDTO> GetJobDescriptionByName(string JobDescriptionName);
        Task<JobDescriptionDTO> GetJobDescriptionByCompany(string JobDescriptionName, int companyId);
        Task<IEnumerable<JobDescriptionDTO>> GetAllJobDescriptionCompanyId(long JobDescriptionID);
    }
}
