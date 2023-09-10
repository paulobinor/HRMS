using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class ApprovePendingResignationDTO
    {
        public long userID { get; set; }
        public long SRFID { get; set; }

    }
}
