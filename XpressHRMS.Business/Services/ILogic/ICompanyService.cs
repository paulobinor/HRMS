using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface ICompanyService
    {
        Task<BaseResponse> ActivateCompany(int CompanyID, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> CreateCompany(CreateCompanyDTO payload, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> DeleteCompany(int CompanyID, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> DisableCompany(int CompanyID, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> GetAllCompanies();
        Task<BaseResponse> GetCompanyByID(int CompanyID);
        Task<BaseResponse> UpdateCompany(UpdateCompanyDTO payload, string RemoteIpAddress, string RemotePort);
    }
}