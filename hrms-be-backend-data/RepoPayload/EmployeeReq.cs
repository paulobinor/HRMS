namespace hrms_be_backend_data.RepoPayload
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
    public class ProcessEmployeePersonalInfoReq
    {
        public long EmployeeId { get; set; }
        public string BirthPlace { get; set; }
        public int NationalityId { get; set; }
        public int StateOfOriginId { get; set; }
        public int LGAOfOriginId { get; set; }
        public string TownOfOrigin { get; set; }
        public string MaidenName { get; set; }
        public int MaritalStatusId { get; set; }
        public string SpouseName { get; set; }
        public int NoOfChildren { get; set; }       
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }       
    }
    public class ProcessEmployeeIdentificationReq
    {
        public long EmployeeId { get; set; }
        public int IdentificationTypeId { get; set; }
        public string IdentificationNumber { get; set; }
        public int CountryIdentificationIssuedId { get; set; }
        public string IdentificationDocument { get; set; }     
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
    public class ProcessEmployeeContactDetailsReq
    {
        public long EmployeeId { get; set; }
        public string PersonalEmail { get; set; }
        public string PhoneNumber { get; set; }
        public int CurrentAddressStateId { get; set; }
        public int CurrentAddressLGAId { get; set; }
        public string CurrentAddressCity { get; set; }
        public string CurrentAddressOne { get; set; }
        public string CurrentAddressTwo { get; set; }
        public int MailingAddressStateId { get; set; }
        public int MailingAddressLGAId { get; set; }
        public string MailingAddressCity { get; set; }
        public string MailingAddressOne { get; set; }
        public string MailingAddressTwo { get; set; }
        public int SpouseAddressStateId { get; set; }
        public int SpouseAddressLGAId { get; set; }
        public string SpouseAddressCity { get; set; }
        public string SpouseAddressOne { get; set; }
        public string SpouseAddressTwo { get; set; }
        public string NextOfKinName { get; set; }
        public string NextOfKinRelationship { get; set; }
        public string NextOfKinPhoneNumber { get; set; }
        public string NextOfKinEmailAddress { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
