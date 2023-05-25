using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.DTOs
{
    public  class EmployeeDTO
    {
        public long EmpID { get; set; }
        public long UserId { get; set; }
        public string StaffID { get; set; }
        public string ProfileImage { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string ResumptionDate { get; set; }
        public string Email { get; set; }
        public string officialMail { get; set; }
        public string PhoneNumber { get; set; }
        public string DeptId { get; set; }
        public string DepartmentName { get; set; }
        public long UnitID { get; set; }
        public string UnitName { get; set; }
        public long UnitHeadID { get; set; }
        public string UnitHeadName { get; set; }
        public long HodID { get; set; }
        public string HODName { get; set; }
        public long GradeID { get; set; }
        public string GradeName { get; set; }
        public long EmployeeTypeID { get; set; }
        public string EmployeeTypeName { get; set; }
        public long PositionID { get; set; }
        public string PositionName { get; set; }
        public long BranchID { get; set; }
        public string BranchName { get; set; }
        public long EmploymentStatusID { get; set; }
        public string EmploymentStatusName { get; set; }
        public long GroupID { get; set; }
        public string GroupName { get; set; }
        public long JobDescriptionID { get; set; }
        public string JobDescriptionName { get; set; }
        public string BirthPlace { get; set; }
        public long Nationality { get; set; }
        public long StateOfOrigin { get; set; }
        public long LGA { get; set; }
        public string Town { get; set; }
        public string MaidenName { get; set; }
        public string MobileNumber2 { get; set; }
        //public string CurrentDesignation { get; set; }
        public long Sex { get; set; }
        public long MaritalStatus { get; set; }
        public string SpouseContactAddress { get; set; }
        public string SpouseMobile { get; set; }


        public string ResidentialAddress { get; set; }
        //public string Position { get; set; }
        //public string ResumptionDate { get; set; }
        public string HomeAddress { get; set; }
        public string MailingAddress { get; set; }
        public string NofName { get; set; }
        public string NofContactAddress { get; set; }
        public string NofMobile { get; set; }
        public string NofRelationship { get; set; }
        public string HighestQualification { get; set; }
        public string HighestQualificationYear { get; set; }
        public string HighestQualificationSchoolAttended { get; set; }
        public string Discipline { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonAddress { get; set; }
        public string ContactPersonPhone { get; set; }
        public string ContactPersonRelationship { get; set; }
        public string FirstDegree { get; set; }
        public string FirstDegreeSchoolAttended { get; set; }
        public string GradeObtained { get; set; }
        public long FirstDegreeYear { get; set; }
        public string SchoolAttended { get; set; }
        public string SecondaryEducationStartDate { get; set; }
        public string SecondaryEducationYearComleted { get; set; }
        public string SecondaryEducationCertificateObtained { get; set; }
        public string MRECompanyName { get; set; }
        public string MREContactAddress { get; set; }
        public string MREPositionHeld { get; set; }
        public string Responsibilities { get; set; }
        public string GrossSalaryPerAnnum { get; set; }
        public string YourPresentPensionFundAdminstrator { get; set; }
        public string YourPensionPinNumber { get; set; }
        public string TaxPayerIdentificationNumber { get; set; }
        public string BankAccountName { get; set; }
        public long AccountNumber { get; set; }
        public string BankName { get; set; }
        public long BVN { get; set; }
        public string FirstReferenceName { get; set; }
        public string FirstReferenceAddress { get; set; }
        public string FirstReferenceOccupation { get; set; }
        public string FirstReferencePeriodKnown { get; set; }
        public string FirstReferenceMobile { get; set; }
        public string FirstReferenceEmail { get; set; }
        public string SecondReferenceName { get; set; }
        public string SecondReferenceAddress { get; set; }
        public string SecondReferenceOccupation { get; set; }
        public string SecondReferencePeriodKnown { get; set; }
        public string SecondReferenceMobile { get; set; }
        public string SecondReferenceEmail { get; set; }
        public string FirstDegreeNameAndLocation { get; set; }
        public long FirstDegreeEntranceYear { get; set; }
        public long FirstDegreeExitYear { get; set; }
        public string FirstDegreeCertificateAndDegreeObtained { get; set; }
        public string FirstDegreeMatricNo { get; set; }
        public string SecondDegreeNameAndLocation { get; set; }
        public long SecondDegreeEntranceYear { get; set; }
        public long SecondDegreeExitYear { get; set; }
        public string SecondDegreeCertificateAndDegreeObtained { get; set; }
        public string SecondDegreeMatricNo { get; set; }
        public string SecondrayEducationNameAndLocation { get; set; }
        public long SecondrayEducationEntranceYear { get; set; }
        public long SecondrayEducationExitYear { get; set; }
        public string SecondrayEducationCertificateAndDegreeObtained { get; set; }
        public string SecondrayEducationExamNo { get; set; }
        public string NameOfProfessionalBody { get; set; }
        public string MembershipNo { get; set; }
        public string MemberStatus { get; set; }
        public string NameOfLastEmployer { get; set; }
        public string EmployerTypeOfBusiness { get; set; }
        public string AddressOfLastEmployer { get; set; }
        public string LocationOrBranch { get; set; }
        public string EmployerStartingDesignation { get; set; }
        public string EmployerLastDesignation { get; set; }
        public string LastEmployerDateEmployed { get; set; }
        public string ReasonForLeaving { get; set; }
        public string HRmail { get; set; }
        public string OfficeTelephoneNo { get; set; }
        public string SupervisorFullName { get; set; }
        public string SecondedBy { get; set; }
        public string NameOfAgency { get; set; }
        public string AddressOfAgency { get; set; }
        public string NameOfPreviousEmployer { get; set; }
        public string TypeOfBusiness { get; set; }
        public string AddressOfPreviousEmployer { get; set; }
        public string LocationAndBranch { get; set; }
        public string StartingDesignation { get; set; }
        public string LastDesignation { get; set; }
        public string DateEmployed { get; set; }
        public string ReasonForLeavingPreviousEmployer { get; set; }
        public string PreviousEmployerHRmail { get; set; }
        public string PreviousEmployerOfficePhone { get; set; }
        public string PreviousEmployerSupervisorFullName { get; set; }
        public string PreviousEmployerSecondedBy { get; set; }
        public string PreviousEmployerNameOfAgency { get; set; }
        public string PreviousEmployerAddressOfAgency { get; set; }
        public string DoYouHaveAnyPendingIssuesWithAformerEmployer { get; set; }
        public long IdentificationCountryOfIssue { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string IdentificationDocument { get; set; }

        public string StampedResignationLetterfromPreviousEmployer { get; set; }
        public string DegreeCertificate { get; set; }
        public string NYSCCertificate { get; set; }
        public string SSCEorWAECCertificate { get; set; }
        public string BirthCertificate { get; set; }
        public string AdditionalQualification { get; set; }
        public string Signature { get; set; }
        public long CompanyID { get; set; }
        public bool IsUpdateSession1 { get; set; }
        public bool IsUpdateSession2 { get; set; }
        public bool IsUpdateSession3 { get; set; }
        public bool IsUpdateSession4 { get; set; }
        public bool IsUpdateSession5 { get; set; }
        public bool IsActive { get; set; }


        public DateTime Created_Date { get; set; }
        public string Created_By_User_Email { get; set; }

        public bool IsUpdated { get; set; }
        public DateTime? Updated_Date { get; set; }
        public string Updated_By_User_Email { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? Deleted_Date { get; set; }
        public string Deleted_By_User_Email { get; set; }
        public string Reasons_For_Delete { get; set; }
    }

    public class UpdateEmployeeDTO
    {
        public long EmpID { get; set; }
        public string ProfileImage { get; set; }
        public string BirthPlace { get; set; }
        public long Nationality { get; set; }
        public long StateOfOrigin { get; set; }
        public long LGA { get; set; }
        public string Town { get; set; }
        public string MaidenName { get; set; }
        public string MobileNumber2 { get; set; }
        //public string CurrentDesignation { get; set; }
        public long Sex { get; set; }
        public long MaritalStatus { get; set; }
        public string SpouseContactAddress { get; set; }
        public string SpouseMobile { get; set; }
       
       
        public string ResidentialAddress { get; set; }
        //public string Position { get; set; }
        //public string ResumptionDate { get; set; }
        public string HomeAddress { get; set; }
        public string MailingAddress { get; set; }
        public string NofName { get; set; }
        public string NofContactAddress { get; set; }
        public string NofMobile { get; set; }
        public string NofRelationship { get; set; }
        public string HighestQualification { get; set; }
        public string HighestQualificationYear { get; set; }
        public string HighestQualificationSchoolAttended { get; set; }
        public string Discipline { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonAddress { get; set; }
        public string ContactPersonPhone { get; set; }
        public string ContactPersonRelationship { get; set; }
        public string FirstDegree { get; set; }
        public string FirstDegreeSchoolAttended { get; set; }
        public string GradeObtained { get; set; }
        public long FirstDegreeYear { get; set; }
        public string SchoolAttended { get; set; }
        public string SecondaryEducationStartDate { get; set; }
        public string SecondaryEducationYearComleted { get; set; }
        public string SecondaryEducationCertificateObtained { get; set; }
        public string MRECompanyName { get; set; }
        public string MREContactAddress { get; set; }
        public string MREPositionHeld { get; set; }
        public string Responsibilities { get; set; }
        public string GrossSalaryPerAnnum { get; set; }
        public string YourPresentPensionFundAdminstrator { get; set; }
        public string YourPensionPinNumber { get; set; }
        public string TaxPayerIdentificationNumber { get; set; }
        public string BankAccountName { get; set; }
        public long AccountNumber { get; set; }
        public string BankName { get; set; }
        public long BVN { get; set; }
        public string FirstReferenceName { get; set; }
        public string FirstReferenceAddress { get; set; }
        public string FirstReferenceOccupation { get; set; }
        public string FirstReferencePeriodKnown { get; set; }
        public string FirstReferenceMobile { get; set; }
        public string FirstReferenceEmail { get; set; }
        public string SecondReferenceName { get; set; }
        public string SecondReferenceAddress { get; set; }
        public string SecondReferenceOccupation { get; set; }
        public string SecondReferencePeriodKnown { get; set; }
        public string SecondReferenceMobile { get; set; }
        public string SecondReferenceEmail { get; set; }
        public string FirstDegreeNameAndLocation { get; set; }
        public long FirstDegreeEntranceYear { get; set; }
        public long FirstDegreeExitYear { get; set; }
        public string FirstDegreeCertificateAndDegreeObtained { get; set; }
        public string FirstDegreeMatricNo { get; set; }
        public string SecondDegreeNameAndLocation { get; set; }
        public long SecondDegreeEntranceYear { get; set; }
        public long SecondDegreeExitYear { get; set; }
        public string SecondDegreeCertificateAndDegreeObtained { get; set; }
        public string SecondDegreeMatricNo { get; set; }
        public string SecondrayEducationNameAndLocation { get; set; }
        public long SecondrayEducationEntranceYear { get; set; }
        public long SecondrayEducationExitYear { get; set; }
        public string SecondrayEducationCertificateAndDegreeObtained { get; set; }
        public string SecondrayEducationExamNo { get; set; }
        public string NameOfProfessionalBody { get; set; }
        public string MembershipNo { get; set; }
        public string MemberStatus { get; set; }
        public string NameOfLastEmployer { get; set; }
        public string EmployerTypeOfBusiness { get; set; }
        public string AddressOfLastEmployer { get; set; }
        public string LocationOrBranch { get; set; }
        public string EmployerStartingDesignation { get; set; }
        public string EmployerLastDesignation { get; set; }
        public string LastEmployerDateEmployed { get; set; }
        public string ReasonForLeaving { get; set; }
        public string HRmail { get; set; }
        public string OfficeTelephoneNo { get; set; }
        public string SupervisorFullName { get; set; }
        public string SecondedBy { get; set; }
        public string NameOfAgency { get; set; }
        public string AddressOfAgency { get; set; }
        public string NameOfPreviousEmployer { get; set; }
        public string TypeOfBusiness { get; set; }
        public string AddressOfPreviousEmployer { get; set; }
        public string LocationAndBranch { get; set; }
        public string StartingDesignation { get; set; }
        public string LastDesignation { get; set; }
        public string DateEmployed { get; set; }
        public string ReasonForLeavingPreviousEmployer { get; set; }
        public string PreviousEmployerHRmail { get; set; }
        public string PreviousEmployerOfficePhone { get; set; }
        public string PreviousEmployerSupervisorFullName { get; set; }
        public string PreviousEmployerSecondedBy { get; set; }
        public string PreviousEmployerNameOfAgency { get; set; }
        public string PreviousEmployerAddressOfAgency { get; set; }
        public string DoYouHaveAnyPendingIssuesWithAformerEmployer { get; set; }
        public long IdentificationCountryOfIssue { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string IdentificationDocument { get; set; }
        public string Signature { get; set; }
        public bool IsUpdateSession1 { get; set; }
        public bool IsUpdateSession2 { get; set; }
        public bool IsUpdateSession3 { get; set; }
        public bool IsUpdateSession4 { get; set; }
        public bool IsUpdateSession5 { get; set; }
        public long CompanyID { get; set; }
        public string StampedResignationLetterfromPreviousEmployer { get; set; }
        public string DegreeCertificate { get; set; }
        public string NYSCCertificate { get; set; }
        public string SSCEorWAECCertificate { get; set; }
        public string BirthCertificate { get; set; }
        public string AdditionalQualification { get; set; }

    }

    public class ApproveEmp
    {
        [Required]
        public string Email { get; set; }
    }

    public class DisapproveEmpDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string DisapprovedComment { get; set; }
    }
}
