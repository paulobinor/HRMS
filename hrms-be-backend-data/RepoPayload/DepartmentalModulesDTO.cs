using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class DepartmentalModulesDTO
    {
        public long DeparmentalModuleId { get; set; }
        public long DepartmentId { get; set; }
        public int AppModuleId { get; set; }
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


    public class GetDepartmentModuleByDepartmentDTO
    {
        public string DepartmentName { get; set; }
       // public string Email { get; set; }
        public string AppModuleName { get; set; }
        public string AppModuleCode { get; set; }
        public long DepartmentalModuleId { get; set; }
        public long DepartmentId { get; set; }
        public int AppModuleId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public long CreatedByUserId { get; set; }
        public bool IsApproved { get; set; }
        public long ApprovedByUserId { get; set; }
        public DateTime? DateApproved { get; set; }
        public bool IsDeleted { get; set; }
        public long DeletedByUserId { get; set; }
    }


    public class GetDepartmentalModuleCount
    {
        public long DepartmentID { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public int ModuleCount { get; set; }
    }

    public class CreateDepartmentalModuleDTO
    {
        public long DepartmentId { get; set; }
        public List<int> AppModuleId { get; set; }
    }
}
