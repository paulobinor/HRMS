using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IResignationClearanceRepository
    {
        Task<dynamic> CreateResignationClearance(ResignationClearanceDTO resignation);
        Task<ResignationClearanceDTO> GetResignationClearanceByID(long ID);
        Task<ResignationClearanceDTO> GetResignationClearanceByEmployeeID(long UserID);
        Task<IEnumerable<ResignationClearanceDTO>> GetAllResignationClearanceByCompany(long companyID, int PageNumber, int RowsOfPage, string SearchVal, long employeeID);

        Task<IEnumerable<ResignationClearanceDTO>> GetPendingResignationClearanceByEmployeeID(long employeeID);
        Task<IEnumerable<ResignationClearanceDTO>> GetPendingResignationClearanceByCompnayID(long companyID,long employeeID);
        Task<dynamic> ApprovePendingResignationClearance(long userID, long ID);
        Task<dynamic> DisapprovePendingResignationClearance(long userID, long ID, string reason);
    }
}
