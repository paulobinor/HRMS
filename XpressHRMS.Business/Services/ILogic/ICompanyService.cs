using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface ICompanyService
    {
        Task<BaseResponse<int>> ActivateCompany(int CompanyID, string EnableBy, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<CreateCompanyDTO>> CreateCompany(CreateCompanyDTO payload, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<int>> DeleteCompany(int CompanyID, string DeletedBy, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<int>> DisableCompany(int CompanyID, string DisableBy, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<List<CompanyDTO>>>GetAllCompanies();
        Task<BaseResponse<CompanyDTO>> GetCompanyByID(int CompanyID);
        Task<BaseResponse<UpdateCompanyDTO>> UpdateCompany(UpdateCompanyDTO payload, string RemoteIpAddress, string RemotePort);
    }
}