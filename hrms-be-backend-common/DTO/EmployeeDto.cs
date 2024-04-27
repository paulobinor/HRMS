namespace hrms_be_backend_common.DTO
{
    public class CreateEmployeeBasisDto
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string PersonalEmail { get; set; }
        public string OfficialEmail { get; set; }
        public string PhoneNumber { get; set; }
        public long GradeId { get; set; }
        public long EmploymentStatusId { get; set; }
        public long BranchId { get; set; }
        public long EmployeeTypeId { get; set; }
        public long DepartmentId { get; set; }
        public DateTime ResumptionDate { get; set; }
        public long JobRoleId { get; set; }
        public long UnitId { get; set; }
    }
    public class UpdateEmployeeCompensationDto
    {
        public long EmployeeId { get; set; }
        public long PayrollId { get; set; }
        public decimal BaseSalary { get; set; }
        public DateTime SalaryEffectiveFrom { get; set; }
    }
    public class UpdateEmployeeBasisDto
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
        public long BranchId { get; set; }
        public long EmployeeTypeId { get; set; }
        public long DepartmentId { get; set; }
        public DateTime ResumptionDate { get; set; }
        public long JobRoleId { get; set; }
        public long UnitId { get; set; }
        public long GradeId { get; set; }
    }
    public class UpdateEmployeePersonalInfoDto
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
        public List<UpdateEmployeeIdentificationDto> Identifications { get; set; }
    }
    public class UpdateEmployeeIdentificationDto
    {
        public int IdentificationTypeId { get; set; }
        public string IdentificationNumber { get; set; }
        public int CountryIdentificationIssuedId { get; set; }
        public string IdentificationDocument { get; set; }
    }
    public class UpdateEmployeeContactDetailsDto
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
    }
    public class UpdateEmployeeProfBackgroundDto
    {
        public long EmployeeId { get; set; }
        public List<EmployeeProfBackgroundDetailsDto> ProfBackgroundDetails { get; set; }
    }
    public class EmployeeProfBackgroundDetailsDto
    {
        public string CompanyName { get; set; }
        public string PositionHead { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ContactEmail { get; set; }
    }
    public class UpdateEmployeeEduBackgroundDto
    {
        public long EmployeeId { get; set; }
        public List<EmployeeEduBackgroundDto> EmployeeEduBackground { get; set; }
    }
    public class EmployeeEduBackgroundDto
    {
        public string InstitutionName { get; set; }
        public string CertificateName { get; set; }
        public string CertificateDoc { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class UpdateEmployeeReferenceDto
    {
        public long EmployeeId { get; set; }
        public List<EmployeeReferenceDto> EmployeeReferences { get; set; }
    }
    public class EmployeeReferenceDto
    {
        public string FullName { get; set; }
        public string ContactAddress { get; set; }
        public string Occupation { get; set; }
        public string PeriodKnown { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }
    public class UpdateEmployeeBankDetailsDto
    {
        public long EmployeeId { get; set; }
        public string BankName { get; set; }
        public string BVN { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string PensionAdministrator { get; set; }
        public string PensionPinNumber { get; set; }
        public string TaxNumber { get; set; }
    }
}
