namespace hrms_be_backend_data.ViewModel
{
    public class CompanyWithTotalVm
    {
        public long totalRecords { get; set; }
        public List<CompanyVm> data { get; set; }
    }
    public class CompanyVm
    {
        public long CompanyId { get; set; }
        public string CompanyName { get; set;}
        public string CompanyCode { get; set; }
        public long LastStaffNumber { get; set;}
        public string Website { get; set;}
        public string CompanyLogo { get; set;}
        public string FullAddress { get; set;}
        public string Email { get; set;}
        public string ContactPhone { get; set;}
        public bool IsPublicSector { get; set;}
        public DateTime DateCreated { get; set;}
    }
    public class CompanyFullVm
    {
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public long LastStaffNumber { get; set; }
        public string Website { get; set; }
        public string CompanyLogo { get; set; }
        public string FullAddress { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public bool IsPublicSector { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public long LastUpdatedByUserId { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public long DeletedByUserId { get; set; }
        public string DeletedComment { get; set; }
        public DateTime DateDeleted { get; set; }
        public bool IsApproved { get; set; }
        public long ApprovedByUserId { get;set; }
        public DateTime DateApproved { get;set; }
    }
}
