using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Common.ViewModels
{
    public class SubmitResignationForm
    {
        public long SRFID { get; set; }
        public long UserId { get; set; }
        public DateTime Date { get; set; }
        public string ReasonForResignation { get; set; }
        public DateTime LastDayOfWork { get; set; }
        public string SignedResignationLetter { get; set; }
        public long CompanyID { get; set; }
        public DateTime DateCreated { get; set; }
        public string Created_By_User_Email { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Deleted_Date { get; set; }
        public string Deleted_By_User_Email { get; set; }
        public string Reasons_For_Delete { get; set; }
        public bool IsApproved { get; set; }
        public long ApprovedByUserId { get; set; }
        public DateTime DateApproved { get; set; }
        public bool IsDisapproved { get; set; }
        public string DisapprovedComment { get; set; }
        public long DisapprovedByUserId { get; set; }
        public DateTime DateDisapproved { get; set; }
        public long UnitHeadUserID { get; set; }
        public bool IsUnitHeadApproved { get; set; }
        public DateTime UnitHeadDateApproved { get; set; }
        public bool IsUnitHeadDeclined { get; set; }
        public string UnitHeadDisapprovedComment { get; set; }
        public bool IsHodApproved { get; set; }
        public long HodUserID { get; set; }
        public DateTime HodDateApproved { get; set; }
        public bool IsHodDeclined { get; set; }
        public string HodDisapprovedComment { get; set; }
        public DateTime HodDateDisapproved { get; set; }
        public bool IsHrApproved { get; set; }
        public long IsHrApprovedByUserId { get; set; }
        public DateTime HrDateApproved { get; set; }
        public bool IsHrDeclined { get; set; }
        public string HrDisapprovedComment { get; set; }
        public DateTime HrDateDisapproved { get; set; }
    }
}
