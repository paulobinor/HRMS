using hrms_be_backend_common.DTO;
using hrms_be_backend_common.Models;
using hrms_be_backend_data.RepoPayload;
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
        Task<List<PendingLeaveApprovalItemsDto>> GetPendingAnnualLeaveApprovals(long approvalEmployeeID, string v);
    }
}

