namespace hrms_be_backend_data.ViewModel
{
    public class UserFullView
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string OfficialMail { get; set; }
        public string PhoneNumber { get; set; }
        public string UserStatusName { get; set; }
        public string UserStatusCode { get; set; }
        public string CompanyName { get; set; }
        public long CompanyId { get; set; }
        public int UserStatusId { get; set; }
        public long CreatedByuserId { get; set; }
        public DateTime DateCreated { get; set; }
        public long LastModifiedByUserId { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsLogin { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastLoginAttemptAt { get; set; }
        public int LoginFailedAttemptsCount { get; set; }
        public bool IsApproved { get; set; }
        public long ApprovedByUserId { get; set; }
        public DateTime DateApproved { get; set; }
        public bool IsDisapproved { get; set; }
        public long DisapprovedByUserId { get; set; }
        public string DisapprovedComment { get; set; }
        public bool IsDeactivated { get; set; }
        public long DeactivatedByUserId { get; set; }
        public string DeactivatedComment { get; set; }
        public DateTime DateDeactivated { get; set; }
        public bool IsReactivated { get; set; }
        public long ReactivatedByUserId { get; set; }
        public string ReactivatedComment { get; set; }
        public DateTime DateReactivated { get; set; }
        public string RefreshToken { get; set; }
        public string Token { get; set; }
        public String PasswordHash { get; set; }
        public long EmployeeId { get; set; }
    }
}
