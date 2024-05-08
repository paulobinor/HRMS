using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Common.ViewModels
{
    public class ResignationClearanceVM
    {
        public long EmployeeID { get; set; }
        public long ResignationID { get; set; }
        public long CompanyID { get; set; }
        public string ItemsReturnedToDepartment { get; set; }
        public string ItemsReturnedToAdmin { get; set; }
        public string ItemsReturnedToHr { get; set; }
        public string LoansOutstanding { get; set; }
        public long CreatedByUserID { get; set; }
        public DateTime DateCreated { get; set; }
        public string Signature { get; set; }



    }
}
