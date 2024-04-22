using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class DisapprovePendingResignation
    {
        public long EmployeeID { get; set; }
        public long ResignationID { get; set; }
        public string reason { get; set; }
    }
}
