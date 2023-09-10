using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class ApproveResignationInterviewDTO
    {
        public long userID { get; set; }
        public bool isApproved { get; set; }
        public long InterviewID { get; set; }

    }
}
