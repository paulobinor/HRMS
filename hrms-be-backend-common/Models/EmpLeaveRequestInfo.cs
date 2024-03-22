using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_common.Models
{
    public class EmpLeaveRequestInfo
    {
        public EmpLeaveRequestInfo() { }

        public long LeaveRequestId { get; set; }
        public long CompanyID { get; set; }
        public long EmployeeId { get; set; }
        public long LeaveByGradeId { get; set; }
        public int MaximumSplitCount { get; set; }
        public int CurrentSplitCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateCreated { get; set; }
        public int TotalDays { get; set; }
        public int NoOfDays { get; set; }
        public int RemainingDays { get; set; }
        public int RequestYear { get; set; }
        public long RelieverUserID { get; set; }
        public long CreatedByUserId { get; set; }
        public byte IsHrDeclined { get; set; }
        public byte IsMDApproved { get; set; }
        public byte IsMDDeclined { get; set; }
        public string LeaveStatus { get; set; }

    }
}
