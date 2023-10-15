using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class UserAppModulePrivilegesDTO
    {
        public long UserAppModulePrivilegesId { get; set; }
        public long AppModulePrivilegeID { get; set; }
        public long UserID { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public long CreatedByUserId { get; set; }
        public bool IsApproved { get; set; }
        public long ApprovedByUserId { get; set; }
        public DateTime? DateApproved { get; set; }
        public bool IsDeleted { get; set; }
        public long DeletedByUserId { get; set; }
        public bool IsDisapproved { get; set; }
        public long DisapprovedByUserId { get; set; }
    }


    public class GetUserAppModulePrivilegesDTO
    {
        public string StaffID { get; set; }
        public string FirstName { get; set; }
        public string LAstName { get; set; }
        public string Email { get; set; }
        public string PrivilegeName { get; set; }
        public string PrivilegeCode { get; set; }
        public string AppModuleName { get; set; }
        public string AppModuleCode { get; set; }
        public long UserAppModulePrivilegesId { get; set; }
        public long AppModulePrivilegeID { get; set; }
        public long UserID { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public long CreatedByUserId { get; set; }
        public bool IsApproved { get; set; }
        public long ApprovedByUserId { get; set; }
        public DateTime? DateApproved { get; set; }
        public bool IsDeleted { get; set; }
        public long DeletedByUserId { get; set; }
        public bool IsDisapproved { get; set; }
        public long DisapprovedByUserId { get; set; }

    }


    //public class GetDepartmentalModuleCount
    //{
    //    public long DepartmentID { get; set; }
    //    public string CompanyName { get; set; }
    //    public string DepartmentName { get; set; }
    //    public string Email { get; set; }
    //    public DateTime Created_Date { get; set; }
    //    public int ModuleCount { get; set; }
    //}

    public class AppModulePrivilegeDTO
    {
        public long AppModulePrivilegeID { get; set; }
        public string PrivilegeName { get; set; }
        public int AppModuleID { get; set; }
    }

    public class CreateUserAppModulePrivilegesDTO
    {
        public long UserID { get; set; }
        public List<int> PrivilegeID { get; set; }
    }
}
