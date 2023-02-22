using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface ICompanyRepository
    {
        Task<int> ActivateCompany(int CompanyID);
        Task<int> CreateCompany(CreateCompanyDTO payload);
        Task<int> DeleteCompany(int CompanyID);
        Task<int> DisableCompany(int CompanyID);
        Task<List<CompanyDTO>> GetAllCompanies();
        Task<CompanyDTO> GetCompanyByID(int CompanyID);
        Task<int> UpdateCompany(UpdateCompanyDTO payload);
    }
}