using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class InterviewScaleValue
    {
        public int InterviewID { get; set; }
        public int ResignationDetailID { get; set; }
        public int Value { get; set; }
        public string Scale { get; set; }
    }
}
