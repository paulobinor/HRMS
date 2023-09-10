using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class UpdateResignationDTO
    {
        public long SRFID { get; set; }

        public string ReasonForResignation { get; set; }
        public DateTime LastDayOfWork { get; set; }
        public string SignedResignationLetter { get; set; }

    }
}
