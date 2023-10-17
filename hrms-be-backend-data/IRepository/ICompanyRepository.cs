using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface ICompanyRepository
    {
        Task<string> ProcessCompany(ProcessCompanyReq payload);
        Task<string> DeleteCompany(DeleteCompanyReq payload);
        Task<CompanyWithTotalVm> GetCompanies(int PageNumber, int RowsOfPage);
        Task<CompanyWithTotalVm> GetCompaniesActivated(int PageNumber, int RowsOfPage);
        Task<CompanyWithTotalVm> GetCompaniesDeactivated(int PageNumber, int RowsOfPage);
        Task<CompanyWithTotalVm> GetCompaniesPublicSector(int PageNumber, int RowsOfPage);
        Task<CompanyWithTotalVm> GetCompaniesPrivateSector(int PageNumber, int RowsOfPage);
        Task<CompanyFullVm> GetCompanyById(long Id);
        Task<CompanyFullVm> GetCompanyByName(string CompanyName);
    }
}
