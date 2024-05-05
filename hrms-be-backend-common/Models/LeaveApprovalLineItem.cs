using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_common.Models
{
    public class LeaveApprovalLineItem
    {

        public long LeaveApprovalLineItemId { get; set; }
        public long LeaveApprovalId { get; set; }
        public bool IsApproved { get; set; }
        public long ApprovalEmployeeId { get; set; }
        public string Comments { get; set; }
        public string ApprovalPosition { get; set; }
        public DateTime EntryDate { get; set; }
        public int ApprovalStep { get; set; }
        public string ApprovalStatus { get; set; }
    }
}
