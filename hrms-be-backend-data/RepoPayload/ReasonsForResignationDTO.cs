using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class ReasonsForResignationDTO
    {
        public long ReasonsForResignationID { get; set; }
        public long ResignationID { get; set; }
        public long CompanyID { get; set; }
        public string ReasonForResignation { get; set; }
    }
}
