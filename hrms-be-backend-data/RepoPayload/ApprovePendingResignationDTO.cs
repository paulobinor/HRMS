using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class ApprovePendingResignationDTO
    {
        public long EmployeeID { get; set; }
        public long ResignationId { get; set; }

    }
}
