using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class CompanyAppModuleDTO
    {
        public long CompanyAppModuleId { get; set; }
        public string AppModuleName { get; set; }
        public long CompanyId { get; set; }
        public int AppModuleId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public long CreatedByUserId { get; set; }
        public bool IsApproved { get; set; }
        public long ApprovedByUserId { get; set; }
        public DateTime? DateApproved { get; set; }
        public bool IsDeleted { get;set; }
        public long DeletedByUserId { get;set; }
        public bool IsDisapproved { get; set; }
        public long DisapprovedByUserId { get; set; }   
    }


    public class GetCompanyAppModuleByCompanyDTO
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string AppModuleName { get; set; }
        public string AppModuleCode { get; set; }
        public long CompanyAppModuleId { get; set; }
        public long CompanyId { get; set; }
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

    public class AppModuleDTO
    {
        public int AppModuleId { get; set; }
        public string? AppModuleName { get; set; }
        public string? AppModuleCode { get; set; }
    }

    public class GetCompanyAppModuleCount
    {
        public long CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public int ModuleCount { get; set; }
    }

    public class CreateCompanyAppModuleDTO
    {
        //public long CompanyAppModuleId { get; set; }
        public long CompanyId { get; set; }
        public List<int> AppModuleId { get; set; }
    }
}
