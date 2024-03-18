using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Common.ViewModels
{
    public class ResignationDTO
    {
        public long ResignationID { get; set; }
        public long EmployeeId { get; set; }
        public string StaffName { get; set; }
        public string StaffID { get; set; }
        public DateTime ExitDate { get; set; }
        public string ReasonForResignation { get; set; }
        public DateTime LastDayOfWork { get; set; }
        public string SignedResignationLetter { get; set; }
        public long CompanyID { get; set; }
        public DateTime DateCreated { get; set; }
        public long CreatedByUserId { get; set; }
        public long UnitHeadEmployeeID { get; set; }
        public bool IsUnitHeadApproved { get; set; }
        public DateTime UnitHeadDateApproved { get; set; }
        public bool IsUnitHeadDisapproved { get; set; }
        public DateTime UnitHeadDateDisapproved { get; set; }
        public string UnitHeadDisapprovedComment { get; set; }
        public long HodEmployeeID { get; set; }
        public bool IsHodApproved { get; set; }
        public DateTime HodDateApproved { get; set; }
        public bool IsHodDisapproved { get; set; }
        public string HodDisapprovedComment { get; set; }
        public DateTime HodDateDisapproved { get; set; }
        public long HrEmployeeID { get; set; }
        public bool IsHrApproved { get; set; }    
        public DateTime HrDateApproved { get; set; }
        public bool IsHrDisapproved { get; set; }
        public string HrDisapprovedComment { get; set; }
        public DateTime HrDateDisapproved { get; set; }

    }
   
}
