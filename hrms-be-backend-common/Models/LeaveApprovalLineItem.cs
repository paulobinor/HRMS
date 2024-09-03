using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_common.Models
{
    public class LeaveApprovalLineItem
    {

        public long LeaveApprovalLineItemId { get; set; }
        public long LeaveApprovalId { get; set; }
        public long EmployeeID { get; set; }
        public bool IsApproved { get; set; }
        public long ApprovalEmployeeId { get; set; }
        public string Comments { get; set; }
        public string ApprovalPosition { get; set; }
        public DateTime EntryDate { get; set; }
        public int ApprovalStep { get; set; }
        public string ApprovalStatus { get; set; }
        public DateTime DateCompleted { get; set; }
    }

    public class LeaveApproval
    {

        public long LeaveApprovalLineItemId { get; set; }
        public long LeaveApprovalId { get; set; }
        public long EmployeeID { get; set; }
        public bool IsApproved { get; set; }
        public long ApprovalEmployeeId { get; set; }
        public string Comments { get; set; }
        public string ApprovalPosition { get; set; }
        public DateTime EntryDate { get; set; }
        public int ApprovalStep { get; set; }
        public string ApprovalStatus { get; set; }
        public List<LeaveApprovalLineItem> leaveApprovalLineItems { get; set; } = new List<LeaveApprovalLineItem>();
        public long LeaveRequestLineItemId { get; set; }
        public object RequiredApprovalCount { get; set; }
        public object LastApprovalEmployeeID { get; set; }
    }

    public class Approvals
    {
        public int ApprovalID { get; set; }
        public int CompanyID { get; set; }
        public int ApprovalEmployeeID { get; set; }
        public int CurrentApprovalCount { get; set; }
        public bool IsApproved { get; set; }
        public string Comment { get; set; }
        public string ApprovalDescription { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime DateCompleted { get; set; }
        public int RequiredApprovalCount { get; set; }
        public string ApprovalStatus { get; set; }
        public List<ApprovalsLineItem> ApprovalsLineItems { get; set; } = new List<ApprovalsLineItem>();
    }

    public class ApprovalsLineItem
    {

        public long ApprovalLineItemID { get; set; }
        public long ApprovalID { get; set; }
        public bool IsApproved { get; set; }
        public long ApprovalEmployeeID { get; set; }
        public string Comments { get; set; }
        public string ApprovalPosition { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime DateCompleted { get; set; }
        public int ApprovalStep { get; set; }
        public string ApprovalStatus { get; set; }
    }
}
