namespace hrms_be_backend_data.RepoPayload
{
    public class ProcessCompanyReq
    {
        public long CompanyId { get; set; }
        public string CompanyName { get; set;}
        public string CompanyCode { get; set;}
        public string CompanyLogo { get; set;}
        public string Website { get; set;}
        public string FullAddress { get; set;}
        public string Email { get; set;}
        public string ContactPhone { get; set;}
        public bool IsPublicSector { get; set;}
        public string AdminFirstName { get; set; }
        public string AdminMiddleName { get; set; }
        public string AdminLastName { get; set; }
        public string AdminOfficialMail { get; set; }
        public string AdminPhoneNumber { get; set; }
        public string AdminPasswordHash { get; set; }
        public long CreatedByUserId { get; set;}
        public DateTime DateCreated { get; set; }
        public bool IsModifield { get; set; }
    }
    public class DeleteCompanyReq
    {
        public long CompanyId { get; set; }
        public string Comment { get; set; }        
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }       
    }
    public class ApproveCompanyReq
    {
        public long CompanyId { get; set; }      
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
