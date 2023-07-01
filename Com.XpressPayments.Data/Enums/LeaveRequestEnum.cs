using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Enums
{
    public enum  LeaveRequestEnum
    {
        CREATE = 1,
        UPDATE = 2,
        DELETE = 3,
        GETALLACTIVE = 4,
        GETALL = 5,
        GETBYID = 6,
        GETBYEMAIL = 7,
        GETBYCOMPANYID = 8,
        GETBYCOMPANY = 9,
        approval = 10,
        disapproval = 11,
            GETUNITHEADAPPROVAL =12,
            GETHODAPPROVAL = 13,
            GETHRAPPROVAL = 14,
    }
}
