using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.JobDescription
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
