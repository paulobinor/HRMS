using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.Company.IRepository
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
    }
}
