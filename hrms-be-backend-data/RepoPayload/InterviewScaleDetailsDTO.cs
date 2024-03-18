using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class InterviewScaleDetailsDTO
    {
        public int InterviewScaleDetailsID { get; set; }
        public string Name { get; set; }
        public int Section { get; set; }
        public int MaximumValue { get; set; }
    }
}
