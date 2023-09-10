using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface ICompanyRepository
    {

        Task<dynamic> CreateCompany(CreateCompanyDto Comp, string createdbyUserEmail);
        Task<dynamic> UpdateCompany(UpdateCompanyDto Company, string updatedbyUserEmail);
        Task<dynamic> DeleteCompany(DeleteCompanyDto Company, string deletedbyUserEmail);
        Task<IEnumerable<CompanyDTO>> GetAllActiveCompanys();
        Task<IEnumerable<CompanyDTO>> GetAllCompanys();
        Task<CompanyDTO> GetCompanyById(long CompanyId);
        Task<CompanyDTO> GetCompanyByName(string CompanyEmail);
        Task<CompanyDTO> GetCompanyByEmail(string Email);
    }
}
