﻿namespace hrms_be_backend_data.RepoPayload
{
    public class EmployeeBasisReq
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
        public bool IsMD { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsModifield { get; set; }
        public long SupervisorId { get; set; }
        public long GroupHeadId { get; set; }
    }
    public class EmployeePersonalInfoReq
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
        public string ProfileImage { get; set; }
        public long GenderId { get; set; }
        public string NIN { get; set; }
      //  public string PreviousEmployerAddress { get; set; }
        public bool HasChildren { get; set; }
    }
    public class EmployeeIdentificationReq
    {
        public long EmployeeId { get; set; }
        public int IdentificationTypeId { get; set; }
        public string IdentificationNumber { get; set; }
        public int CountryIdentificationIssuedId { get; set; }
        public string IdentificationDocument { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
    public class EmployeeContactDetailsReq
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
    public class EmployeeProfesionalBackgroundReq
    {
        public long EmployeeId { get; set; }
        public string CompanyName { get; set; }
        public string PositionHead { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ContactEmail { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public string CompanyAddress { get; set; }
    }
    public class EmployeeReferenceReq
    {
        public long EmployeeId { get; set; }
        public string FullName { get; set; }
        public string ContactAddress { get; set; }
        public string Occupation { get; set; }
        public string PeriodKnown { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
    public class EmployeeEduBackgroundReq
    {
        public long EmployeeId { get; set; }
        public string InstitutionName { get; set; }
        public string CertificateName { get; set; }
        public string CertificateDoc { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
    public class EmployeeBankDetailsReq
    {
        public long EmployeeId { get; set; }
        public string BankName { get; set; }
        public string BVN { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string PensionAdministrator { get; set; }
        public string PensionPinNumber { get; set; }
        public string TaxNumber { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public string NIN { get; set; }
    }
    public class EmployeeCompensationReq
    {
        public long EmployeeId { get; set; }
        public long PayrollId { get; set; }
        public decimal BaseSalary { get; set; }
        public DateTime SalaryEffectiveFrom { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
