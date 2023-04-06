using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.DTOs.Account
{
    public partial class User
    {
        public long UserId { get; set; }
        public long DeptId { get; set; }
        //public long DepartmentId { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        //public string OfficalMail { get; set; }

        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }

        public long? CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }

        public bool IsModified { get; set; }
        public long? LastModifiedByUserId { get; set; }
        public DateTime? DateModified { get; set; }

        public bool IsLogin { get; set; }
        public bool IsActive { get; set; }

        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastLoginAttemptAt { get; set; }
        public int LoginFailedAttemptsCount { get; set; }
        public string LoggedInWithIPAddress { get; set; }
   
        public bool IsApproved { get; set; }
        public long? ApprovedByUserId { get; set; }
        public DateTime? DateApproved { get; set; }

        public bool IsDisapproved { get; set; }
        public string DisapprovedComment { get; set; }
        public long? DisapprovedByUserId { get; set; }
        public DateTime? DateDisapproved { get; set; }

        public bool IsDeactivated { get; set; }
        public string DeactivatedComment { get; set; }
        public long? DeactivatedByUserId { get; set; }
        public DateTime? DateDeactivated { get; set; }

        public bool IsReactivated { get; set; }
        public string ReactivatedComment { get; set; }
        public long? ReactivatedByUserId { get; set; }
        public DateTime? DateReactivated { get; set; }

        public string RefreshToken { get; set; }        
        public string Token { get; set; }
        public string RoleName { get; set; }

        public string CreatedByUserEmail { get; set; }
        public string LastUpdatedByUserEmail { get; set; }
        public string ApprovedByUserEmail { get; set; }
        public string DisapprovedByUserEmail { get; set; }
        public string DeactivatedByUserEmail { get; set; }
        public string ReactivatedByUserEmail { get; set; }

    }

    public partial class Dispatchers
    {
        public long UserId { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
