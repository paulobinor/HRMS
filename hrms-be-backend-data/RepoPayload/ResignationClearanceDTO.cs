using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class ResignationClearanceDTO
    {
        public long UserID { get; set; }
        public long SRFID { get; set; }
        public string LastName { get; set;}
        public string FirstName { get; set;}    
        public string MiddleName { get; set;}
        public string ReasonForResignation { get; set; }
        public string ItemsReturnedToDepartment { get; set; }
        public string ItemsReturnedToAdmin { get; set; }
        //public string Loans/AdvanceOutstanding { get; set; }
        public string Collateral { get; set; }
        public string ItemsReturnedToHR { get; set; }
        public DateTime LastDayOfWork { get; set; }
        public string Signature { get; set; }
    }
}
