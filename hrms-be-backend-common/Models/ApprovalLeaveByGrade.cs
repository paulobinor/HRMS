using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_common.Models
{
    public class ApprovalLeaveByGrade
    {
        public ApprovalLeaveByGrade() { }

        public int ApprovalLeaveByGradeId { get; set; }
        public int GradeId { get; set; }
        public int EmployeeId { get; set; }
        public int CompanyID { get; set; }
        public int IsMdApprovalRequired { get; set; }
        public int IsUnitHeadApprovalRequired { get; set; }
        public int IsHRApprovalRequired { get; set; }
        public int IsGroupHeadApprovalRequired { get; set; }
    }
}
