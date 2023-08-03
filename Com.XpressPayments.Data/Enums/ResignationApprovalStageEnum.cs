using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Enums
{
    public enum ResignationApprovalStageEnum
    {
        PendingOnUnitHead = 1,
        PendingOnGroupHead,
        PendingOnHR,
        PendingOnMD,
        Approved,
        Disapproved,
    }
}
