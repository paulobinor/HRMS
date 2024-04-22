using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.ViewModel
{
    public class CreateExitClearanceSetupVm
    {
        public long CompanyID { get; set; }
        public long DepartmentID { get; set; }
        public bool IsFinalApproval { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
