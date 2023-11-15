﻿namespace hrms_be_backend_data.ViewModel
{

    public class EmployeeFullVm
    {
        public long EmployeeID { get; set; }
        public string StaffID { get; set; }
        public string ProfileImage { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PersonalEmail { get; set; }
        public string OfficialMail { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumber2 { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public long HodEmployeeId { get; set; }
        public string HodName { get; set; }
        public long UnitID { get; set; }
        public string UnitName { get; set; }
        public long UnitHeadEmployeeId { get; set; }
        public string UnitHeadName { get; set; }
        public string GradeID { get; set; }
        public string GradeName { get; set; }
        public string EmployeeTypeID { get; set; }
        public string EmployeeTypeName { get; set; }
        public string DOB { get; set; }
        public long BranchID { get; set; }
        public string EmploymentStatusID { get; set; }
        public string EmploymentStatusName { get; set; }
        public string JobDescriptionID { get; set; }
        public string JobDescriptionName { get; set; }
        public string BirthPlace { get; set; }
        public int NationalityId { get; set; }
        public int StateOfOriginId { get; set; }
        public int LGAOfOriginId { get; set; }
        public string TownOfOrigin { get; set; }
        public string MaidenName { get; set; }
        public int SexId { get; set; }
        public int MaritalStatusId { get; set; }
        public string SpouseName { get; set; }
        public string NoOfChildren { get; set; }
        public string SpouseContactAddress { get; set; }
        public string SpouseMobile { get; set; }
        public long PositionId { get; set; }
        public string PositionName { get; set; }
        public DateTime ResumptionDate { get; set; }
        public long PayrollId { get; set; }
        public string BaseSalary { get; set; }
        public string SalaryEffectiveFrom { get; set; }
        public string PensionFundAdminstrator { get; set; }
        public string PensionNumber { get; set; }
        public string TaxPayerIdentificationNumber { get; set; }
        public string BankAccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string BVN { get; set; }
        public string Signature { get; set; }
        public string BirthCertificate { get; set; }
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
        public string NextOfKinPhoneNumber { get; set; }
        public string NextOfKinRelationship { get; set; }
        public string NextOfKinEmailAddress { get; set; }
        public long CompanyID { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public long LastUpdatedByUserId { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public long DeletedByUserId { get; set; }
        public DateTime DateDeleted { get; set; }
        public string DeletedComment { get; set; }
        public bool IsApproved { get; set; }
        public long ApprovedByUserId { get; set; }
        public DateTime DateApproved { get; set; }
        public bool IsDisapproved { get; set; }
        public string DisapprovedComment { get; set; }
        public long DisapprovedByUserId { get; set; }
        public DateTime DateDisapproved { get; set; }
        public bool IsFirstEmployee { get; set; }
        public bool HasCompletedBankDetails { get; set; }
        public bool HasCompletedContactDetails { get; set; }
        public bool HasCompletedEduBackGround { get; set; }
        public bool HasCompletedPersonalInfo { get; set; }
        public bool HasCompletedProfBackground { get; set; }
        public bool HasCompletedReferenceDetails { get; set; }
    }

    public class EmployeeVm
    {
        public long EmployeeID { get; set; }
        public string StaffID { get; set; }
        public string ProfileImage { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PersonalEmail { get; set; }
        public string OfficialMail { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumber2 { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public long HodEmployeeId { get; set; }
        public string HodName { get; set; }
        public long UnitID { get; set; }
        public string UnitName { get; set; }
        public long UnitHeadEmployeeId { get; set; }
        public string UnitHeadName { get; set; }
        public string GradeID { get; set; }
        public string GradeName { get; set; }
        public string EmployeeTypeID { get; set; }
        public string EmployeeTypeName { get; set; }
        public string DOB { get; set; }
        public long BranchID { get; set; }
        public string EmploymentStatusID { get; set; }
        public string EmploymentStatusName { get; set; }
        public string JobDescriptionID { get; set; }
        public string JobDescriptionName { get; set; }
        public string BirthPlace { get; set; }
        public int NationalityId { get; set; }
        public int StateOfOriginId { get; set; }
        public int LGAOfOriginId { get; set; }
        public string TownOfOrigin { get; set; }
        public string MaidenName { get; set; }
        public int SexId { get; set; }
        public int MaritalStatusId { get; set; }
        public string SpouseName { get; set; }
        public string NoOfChildren { get; set; }
        public string SpouseContactAddress { get; set; }
        public string SpouseMobile { get; set; }
        public long PositionId { get; set; }
        public string PositionName { get; set; }
        public DateTime ResumptionDate { get; set; }
        public long PayrollId { get; set; }
        public string BaseSalary { get; set; }
        public string SalaryEffectiveFrom { get; set; }
        public string PensionFundAdminstrator { get; set; }
        public string PensionNumber { get; set; }
        public string TaxPayerIdentificationNumber { get; set; }
        public string BankAccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string BVN { get; set; }
        public string Signature { get; set; }
        public string BirthCertificate { get; set; }
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
        public string NextOfKinPhoneNumber { get; set; }
        public string NextOfKinRelationship { get; set; }
        public string NextOfKinEmailAddress { get; set; }
        public long CompanyID { get; set; }      
        public DateTime DateCreated { get; set; }      
        public DateTime DateLastUpdated { get; set; }
        public bool IsDeleted { get; set; }      
        public DateTime DateDeleted { get; set; }
        public string DeletedComment { get; set; }
        public bool IsApproved { get; set; }      
        public DateTime DateApproved { get; set; }
        public bool IsDisapproved { get; set; }
        public string DisapprovedComment { get; set; }       
        public DateTime DateDisapproved { get; set; }

    }
    public class EmployeeWithTotalVm
    {
        public long totalRecords { get; set; }
        public List<EmployeeVm> data { get; set; }
    }
}
