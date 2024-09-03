using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_data.RepoPayload;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_data.IRepository
{
    public interface IResignationRepository
    {
        Task<dynamic> CreateResignation(ResignationDTO request);
        Task<dynamic> UpdateResignation(UpdateResignationDTO resignation);
        Task<dynamic> CreateReasonsForResignation(int resignationID,string[] reasons,long CompanyID);
        Task<IEnumerable<ReasonsForResignationDTO>> GetReasonsForResignationByID(long ID);
        Task<ResignationDTO> GetResignationByID(long ID);
        Task<ResignationDTO> GetResignationByEmployeeID(long EmployeeID);
        Task<IEnumerable<ResignationDTO>> GetResignationByCompanyID(long companyID, int PageNumber, int RowsOfPage, string SearchVal, DateTime? startDate, DateTime? endDate);
       // Task<IEnumerable<ResignationDTO>> GetAllResignations();

        //Task<dynamic> DeleteResignation(long ID, string deletedBy, string deleteReason);
        Task<IEnumerable<ResignationDTO>> GetPendingResignationByEmployeeID(long employeeID);
        Task<IEnumerable<ResignationDTO>> GetPendingResignationByCompanyID(long companyID,long employeeID, int PageNumber, int RowsOfPage, string SearchVal, DateTime? startDate, DateTime? endDate);
        Task<string> ApprovePendingResignation(long EmployeeID, long ResignationId);
        Task<string> DisapprovePendingResignation(long EmployeeID, long ResignationId, string reason);
        

    }
}
