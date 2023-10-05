using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Common.ViewModels
{
    public class ResignationClearanceVM
    {
        public long UserID { get; set; }
        public long SRFID { get; set; }
        public long InterviewID { get; set; }
        public string ItemsReturnedToDepartment { get; set; }
        public string ItemsReturnedToAdmin { get; set; }
        public string Loans { get; set; }
        public string Collateral { get; set; }
        public string ItemsReturnedToHR { get; set; }
        public DateTime LastDayOfWork { get; set; }
        public string FileName { get; set; }


    }
}
