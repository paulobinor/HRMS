using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class DisapproveResignationInterviewDTO
    {
        
            public long userID { get; set; }
            public bool IsDisapproved { get; set; }
            public long InterviewID { get; set; }
            public string DisapprovedComment { get; set; }


    }
}
