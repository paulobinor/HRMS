namespace hrms_be_backend_data.ViewModel
{
    public class UserWithTotalVm
    {
        public long totalRecords { get; set; }
        public List<UserVm> data { get; set; }
    }
    public class UserVm
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }      
        public string OfficialMail { get; set; }
        public string PhoneNumber { get; set; }
        public string UserStatusName { get; set; }
        public string CompanyName { get; set; }     
        public bool IsApproved { get; set; }
        public bool IsDisapproved { get; set; }
        public bool IsDeactivated { get; set; }
    }
    public class AccessUserVm
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string OfficialMail { get; set; }
        public string PhoneNumber { get; set; }
        public string UserStatusName { get; set; }
        public string UserStatusCode { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDisapproved { get; set; }
        public bool IsDeactivated { get; set; }      
    }
    public class EmployeeDetailsVm
    {
        public long EmployeeID { get; set; }
        public bool IsFirstEmployee { get; set; }
        public bool HasCompletedBankDetails { get; set; }
        public bool HasCompletedContactDetails { get; set; }
        public bool HasCompletedEduBackGround { get; set; }
        public bool HasCompletedPersonalInfo { get; set; }
        public bool HasCompletedProfBackground { get; set; }
    }
    public class UserModulesVm
    {
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
    }

}
