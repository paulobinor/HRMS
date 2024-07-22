using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_common.Models
{
    public class LeaveRequestConfig
    {
        public bool ValidateGender { get; set; }
        public bool EnableSingleApproval { get; set; }
        public bool UseApprovalTable { get; set; } = false;
    }
}
