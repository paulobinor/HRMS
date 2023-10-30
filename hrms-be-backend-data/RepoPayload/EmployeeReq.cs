﻿namespace hrms_be_backend_data.RepoPayload
{
    public class ProcessEmployeeBasisReq
    {
        public long EmployeeId { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string PersonalEmail { get; set; }
        public string OfficialEmail { get; set; }
        public string PhoneNumber { get; set; }
        public long EmploymentStatusId { get; set; }
        public long BranchId { get; set;}
        public long EmployeeTypeId { get; set;}
        public long DepartmentId { get; set; }
        public long ResumptionDate { get; set; }
        public long JobRoleId { get; set; }
        public long UnitId { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsModifield { get; set; }
    }
}
