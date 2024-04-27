using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class ExitClearanceSetupDTO
    {
        public long ExitClearanceSetupID { get; set; }
        public long CompanyID { get; set; }
        public long DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public long HodEmployeeID { get; set; }
        public bool IsFinalApproval { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime DateDeleted { get; set; }

    }
    public class UpdateExitClearanceSetupDTO
    {
        public long ExitClearanceSetupID { get; set; }
        public bool IsFinalApproval { get; set; }

    }

}
