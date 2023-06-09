using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.GenericResponse;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public interface ICompanyService
    {
        Task<BaseResponse> CreateCompany(CreateCompanyDto CompanyDto, RequesterInfo requester);
        Task<BaseResponse> UpdateCompany(UpdateCompanyDto updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteCompany(DeleteCompanyDto deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveCompanies(RequesterInfo requester);
        Task<BaseResponse> GetAllCompanies(RequesterInfo requester);
        Task<BaseResponse> GetCompanybyId(long companyId, RequesterInfo requester);
    }

   
}
