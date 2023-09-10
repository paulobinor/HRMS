using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.ILogic
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
