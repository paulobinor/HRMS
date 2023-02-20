using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
    public class EmployeeDTO
    {
        //public int EmployeeID { get; set; }
        public int CompanyId { get; set; }
        public string HRTag { get; set; }
        public int Title { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PostalAddress { get; set; }
        public string ResidentialAddress { get; set; }
        public string Sex { get; set; }
        public DateTime DOB { get; set; }
        public int GenotypeID { get; set; }
        public int MaritalID { get; set; }
        public int LgaID { get; set; }
        public int StateID { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfDepRelative { get; set; }
        public decimal Height { get; set; }
        public int BloodGroupID { get; set; }
        public int ReligionID { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime DateConfimed { get; set; }
        public int StartBranch { get; set; }
        public int CurrentBranch { get; set; }
        public int StartDepartment { get; set; }
        public int CurrentDepartment { get; set; }
        public int StartPosition { get; set; }
        public int CurrentPosition { get; set; }
        public string Image { get; set; }
        public string Hobby { get; set; }
        public int BankId { get; set; }
        public string BankAcctNo { get; set; }
        public string PFAAcctNo { get; set; }
        public int PFAId { get; set; }
        public int EmpTypeId { get; set; }
        public string EmpPassword { get; set; }
        
        public string EmployeeStatus { get; set; }
        public int IsFirstLogin { get; set; }
        public int DepartmentID { get; set; }
        public int PositionID { get; set; }
        public int BranchID { get; set; }
        public int JobTitleId { get; set; }
        public int DrivingLicence { get; set; }
        //public DateTime DateCreated { get; set; }
    }

    public class CreateEmployeeDTO
    {
        //public int EmployeeID { get; set; }
        public int CompanyId { get; set; }
        public string HRTag { get; set; }
        public int Title { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PostalAddress { get; set; }
        public string ResidentialAddress { get; set; }
        public string Sex { get; set; }
        public DateTime DOB { get; set; }
        public int GenotypeID { get; set; }
        public int MaritalID { get; set; }
        public int LgaID { get; set; }
        public int StateID { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfDepRelative { get; set; }
        public decimal Height { get; set; }
        public int BloodGroupID { get; set; }
        public int ReligionID { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime DateConfimed { get; set; }
        public int StartBranch { get; set; }
        public int CurrentBranch { get; set; }
        public int StartDepartment { get; set; }
        public int CurrentDepartment { get; set; }
        public int StartPosition { get; set; }
        public int CurrentPosition { get; set; }
        public string Image { get; set; }
        public string Hobby { get; set; }
        public int BankID { get; set; }
        public string BankAcctNo { get; set; }
        public string PFAAcctNo { get; set; }
        public int PFAId { get; set; }
        public int EmpTypeId { get; set; }
        public string EmpPassword { get; set; }
       
        public string EmployeeStatus { get; set; }
        public int IsFirstLogin { get; set; }
        public int DepartmentID { get; set; }
        public int PositionID { get; set; }
        public int BranchID { get; set; }
        public int JobTitleId { get; set; }
        public int DrivingLicence { get; set; }
    }

    public class updateEmployeeDTO
    {
        public int EmployeeID { get; set; }
        public string HRTag { get; set; }
        public int Title { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PostalAddress { get; set; }
        public string ResidentialAddress { get; set; }
        public string Sex { get; set; }
        public DateTime DOB { get; set; }
        public int GenotypeID { get; set; }
        public int MaritalID { get; set; }
        public int LgaID { get; set; }
        public int StateID { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfDepRelative { get; set; }
        public decimal Height { get; set; }
        public int BloodGroupID { get; set; }
        public int ReligionID { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime DateConfimed { get; set; }
        public int StartBranch { get; set; }
        public int CurrentBranch { get; set; }
        public int StartDepartment { get; set; }
        public int CurrentDepartment { get; set; }
        public int StartPosition { get; set; }
        public int CurrentPosition { get; set; }
        public string Image { get; set; }
        public string Hobby { get; set; }
        public int BankId { get; set; }
        public string BankAcctNo { get; set; }
        public string PFAAcctNo { get; set; }
        public int PFAId { get; set; }
        public int EmpTypeId { get; set; }
        public string EmpPassword { get; set; }
        public int CompanyId { get; set; }
        public string EmployeeStatus { get; set; }
        public int IsFirstLogin { get; set; }
        public int DepartmentID { get; set; }
        public int PositionID { get; set; }
        public int BranchID { get; set; }
        public int JobTitleId { get; set; }
        public int DrivingLicence { get; set; }
    }

    public class DelEmployeeDTO
    {
        public int EmployeeID { get; set; }
        public int CompanyId { get; set; }
    }




    public class EmployeeBulkDTO
    {
        //public int EmployeeID { get; set; }
        public int CompanyId { get; set; }
        public string HRTag { get; set; }
        public int Title { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PostalAddress { get; set; }
        public string ResidentialAddress { get; set; }
        public string Sex { get; set; }
        public DateTime DOB { get; set; }
        public int GenotypeID { get; set; }
        public int MaritalID { get; set; }
        public int LgaID { get; set; }
        public int StateID { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfDepRelative { get; set; }
        public decimal Height { get; set; }
        public int BloodGroupID { get; set; }
        public int ReligionID { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime DateConfimed { get; set; }
        public int StartBranch { get; set; }
        public int CurrentBranch { get; set; }
        public int StartDepartment { get; set; }
        public int CurrentDepartment { get; set; }
        public int StartPosition { get; set; }
        public int CurrentPosition { get; set; }
        public string Image { get; set; }
        public string Hobby { get; set; }
        public int BankId { get; set; }
        public string BankAcctNo { get; set; }
        public string PFAAcctNo { get; set; }
        public int PFAId { get; set; }
        public int EmpTypeId { get; set; }
        public string EmpPassword { get; set; }

        public string EmployeeStatus { get; set; }
        public int IsFirstLogin { get; set; }
        public int DepartmentID { get; set; }
        public int PositionID { get; set; }
        public int BranchID { get; set; }
        public int JobTitleId { get; set; }
        public int DrivingLicence { get; set; }
        //public DateTime DateCreated { get; set; }
    }

    public class CreateEmployeeDTOBulk
    { //public int EmployeeID { get; set; }
        public int CompanyId { get; set; }
        public string HRTag { get; set; }
        public int Title { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PostalAddress { get; set; }
        public string ResidentialAddress { get; set; }
        public string Sex { get; set; }
        public DateTime DOB { get; set; }
        public int GenotypeID { get; set; }
        public int MaritalID { get; set; }
        public int LgaID { get; set; }
        public int StateID { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfDepRelative { get; set; }
        public decimal Height { get; set; }
        public int BloodGroupID { get; set; }
        public int ReligionID { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime DateConfimed { get; set; }
        public int StartBranch { get; set; }
        public int CurrentBranch { get; set; }
        public int StartDepartment { get; set; }
        public int CurrentDepartment { get; set; }
        public int StartPosition { get; set; }
        public int CurrentPosition { get; set; }
        public string Image { get; set; }
        public string Hobby { get; set; }
        public int BankId { get; set; }
        public string BankAcctNo { get; set; }
        public string PFAAcctNo { get; set; }
        public int PFAId { get; set; }
        public int EmpTypeId { get; set; }
        public string EmpPassword { get; set; }

        public string EmployeeStatus { get; set; }
        public int IsFirstLogin { get; set; }
        public int DepartmentID { get; set; }
        public int PositionID { get; set; }
        public int BranchID { get; set; }
        public int JobTitleId { get; set; }
        public int DrivingLicence { get; set; }
        //public DateTime DateCreated { get; set; }

    }

    public class UpdateEmployeeDTOBulk
    {
        public int EmployeeID { get; set; }
        public string HRTag { get; set; }
        public int Title { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PostalAddress { get; set; }
        public string ResidentialAddress { get; set; }
        public string Sex { get; set; }
        public DateTime DOB { get; set; }
        public int GenotypeID { get; set; }
        public int MaritalID { get; set; }
        public int LgaID { get; set; }
        public int StateID { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfDepRelative { get; set; }
        public decimal Height { get; set; }
        public int BloodGroupID { get; set; }
        public int ReligionID { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime DateConfimed { get; set; }
        public int StartBranch { get; set; }
        public int CurrentBranch { get; set; }
        public int StartDepartment { get; set; }
        public int CurrentDepartment { get; set; }
        public int StartPosition { get; set; }
        public int CurrentPosition { get; set; }
        public string Image { get; set; }
        public string Hobby { get; set; }
        public int BankId { get; set; }
        public string BankAcctNo { get; set; }
        public string PFAAcctNo { get; set; }
        public int PFAId { get; set; }
        public int EmpTypeId { get; set; }
        public string EmpPassword { get; set; }
        public int CompanyId { get; set; }
        public string EmployeeStatus { get; set; }
        public int IsFirstLogin { get; set; }
        public int DepartmentID { get; set; }
        public int PositionID { get; set; }
        public int BranchID { get; set; }
        public int JobTitleId { get; set; }
        public int DrivingLicence { get; set; }
    }

    public class DelEmployeeDTOBulk
    {
        public int EmployeeID { get; set; }
        public int CompanyId { get; set; }

    }




    public class EmployeeUpload
    {
        public IFormFile EmployeeFiles { get; set; }
        public int CompanyID { get; set; }

    }
}
