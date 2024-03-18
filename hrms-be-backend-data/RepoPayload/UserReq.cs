namespace hrms_be_backend_data.RepoPayload
{
    public class CreateUserReq
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string OfficialMail { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string UserStatusCode { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsModifield { get; set; }

    }
    public class CreateCompanyUserReq
    {
        public long CompanyId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string OfficialMail { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }       
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }       

    }
}
