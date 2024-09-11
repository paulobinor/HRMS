using hrms_be_backend_common.DTO;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using System.Threading.Tasks;

namespace hrms_be_backend_data.IRepository
{
    public interface ILeaveApprovalRepository
    {

        Task<LeaveApprovalInfo> GetLeaveApprovalInfo(long leaveApprovalId);
        Task<LeaveApprovalInfo> GetAnnualLeaveApprovalInfo(long leaveApprovalId);
        Task<List<LeaveApprovalInfoDto>> GetLeaveApprovalInfoByCompanyID(long CompanyID);
        Task<LeaveApprovalInfo> GetLeaveApprovalInfoByRequestLineItemId(long leaveRequestLineitemId);
        Task<LeaveApprovalLineItem> GetLeaveApprovalLineItem(long LeaveApprovalId, int approvalStep);
        Task<LeaveApprovalInfo> UpdateLeaveApprovalInfo(LeaveApprovalInfo leaveApproval);
        Task<LeaveApprovalLineItem> UpdateLeaveApprovalLineItem(LeaveApprovalLineItem leaveApprovalLineItem);
        Task<GradeLeave> GetEmployeeGradeLeave(long employeeId);
        Task<LeaveApprovalInfo> GetLeaveApprovalInfoByEmployeeId(long EmployeeId);
        Task<LeaveApprovalInfo> GetExistingLeaveApproval(long EmployeeId);
        Task<List<LeaveApprovalLineItem>> GetLeaveApprovalLineItems(long leaveApprovalId);
        Task<List<PendingLeaveApprovalItemsDto>> GetPendingLeaveApprovals(long approvalEmployeeID, string v);
        Task<List<PendingAnnualLeaveApprovalItemDto>> GetAnnualLeaveApprovals(long approvalEmployeeID);
        Task<List<LeaveApproval>> GetLeaveApprovals(long approvalEmployeeId, long employeeID);
        Task<Approvals> CreateApproval(Approvals approvals);
        Task<LeaveApproval> CreateLeaveApproval(LeaveApproval approvals);
        Task<LeaveApproval> CreateAnnualLeaveApproval(LeaveApproval leaveApproval);
        Task<AnnualLeave> GetAnnualLeaveInfo(long leaveApprovalId);
        Task<bool> GetLeaveApprovalInfoByApprovalKey(long approvalKey);
        //  Task<AnnualLeave> UpdateAnnualLeave(AnnualLeave annualLeave);
    }
}

