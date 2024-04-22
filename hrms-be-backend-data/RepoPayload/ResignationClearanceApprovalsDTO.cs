using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class ResignationClearanceApprovalsDTO
    {
        public long ResignationClearanceApprovalsID { get; set; }
        public long ResignationClearanceID { get; set; }
        public long ExitClearanceSetupID { get; set; }
        public long CompanyID { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ApprovalDate { get; set; }
        public long ApprovedByUserId { get; set; }

        //public bool IsDisApproved { get; set; }
        // public DateTime DisApprovalDate { get; set; }
        // public long DisapprovedByUserId { get; set; }

    }
}
