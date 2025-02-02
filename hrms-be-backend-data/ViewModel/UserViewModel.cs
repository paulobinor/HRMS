﻿using System;

namespace hrms_be_backend_data.ViewModel
{
    public class UserViewModel
    {
        public long UserId { get; set; }
        public long DeptId { get; set; }
        public long EmpID { get; set; }
        public string StaffID { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string ProfileImage { get; set; }
        public string CompanyLogo { get; set; }
        public bool IsUpdated { get; set; }
        public bool IsUpdateSession1 { get; set; }
        public bool IsUpdateSession2 { get; set; }
        public bool IsUpdateSession3 { get; set; }
        public bool IsUpdateSession4 { get; set; }
        public bool IsUpdateSession5 { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string officialMail { get; set; }
        public string DOB { get; set; }
        public string ResumptionDate { get; set; }

        public string PhoneNumber { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public long UnitID { get; set; }
        public string UnitName { get; set; }
       
        public long GradeID { get; set; }
        public string GradeName { get; set; }
        public long EmployeeTypeID { get; set; }
        public string EmployeeTypeName { get; set; }
    
        public long EmploymentStatusID { get; set; }
        public string EmploymentStatusName { get; set; }
        public long JobDescriptionID { get; set; }
        public string JobDescriptionName { get; set; }

        public long? CreatedByUserId { get; set; }
        public string? CreatedByUserEmail { get; set; }
        public DateTime DateCreated { get; set; }

        public bool IsModified { get; set; }
        public long? LastModifiedByUserId { get; set; }
        public string LastModifiedByUserEmail { get; set; }

        public DateTime? DateModified { get; set; }

        public bool IsLogin { get; set; }
        public bool IsActive { get; set; }

        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastLoginAttemptAt { get; set; }
        public int LoginFailedAttemptsCount { get; set; }
        public string LoggedInWithIPAddress { get; set; }

        public bool IsApproved { get; set; }
        public long? ApprovedByUserId { get; set; }
        public string ApprovedByUserEmail { get; set; }
        public DateTime? DateApproved { get; set; }

        public bool IsDisapproved { get; set; }
        public string DisapprovedComment { get; set; }
        public long? DisapprovedByUserId { get; set; }
        public string DeactivatedByUserEmail { get; set; }
        public string DisapprovedByUserEmail { get; set; }
        public DateTime? DateDisapproved { get; set; }

        public bool IsDeactivated { get; set; }
        public string DeactivatedComment { get; set; }
        public long? DeactivatedByUserId { get; set; }
        public DateTime? DateDeactivated { get; set; }

        public bool IsReactivated { get; set; }
        public string ReactivatedComment { get; set; }
        public long? ReactivatedByUserId { get; set; }
        public string ReactivatedByUserEmail { get; set; }
        public DateTime? DateReactivated { get; set; }
        //public string RoleName { get; set; }

        public string UserStatus { get; set; }
        public int UserStatusId { get; set; }
        public bool IsLockedOut { get; set; }

    }
}
