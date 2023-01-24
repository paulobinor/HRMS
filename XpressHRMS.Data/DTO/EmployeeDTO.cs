using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
    public class EmployeeDTO
    {
        public int EmployeeID { get; set; }
        public string HRTag { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PostalAddress { get; set; }
        public string ResidentialAddress { get; set; }
        public int CompanyID { get; set; }
        public int MaritalID { get; set; }
        public int NationalityID { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DOB { get; set; }
        public int StateID { get; set; }
        public int LGAID { get; set; }
        public string EmailAddress { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfDepRelative { get; set; }
        public decimal height { get; set; }
        public int BloodGroupID { get; set; }
        public int ReligionID { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime DateConfirmed { get; set; }
        public int StartBranchID { get; set; }
        public int CurrentBranchID { get; set; }
        public int StartDepartmentID { get; set; }
        public int CurrentDepartmentID { get; set; }
        public int StartPositionID { get; set; }
        public int CurrentPositionID { get; set; }
        public string Picture { get; set; }
        public string Hobby { get; set; }
        public int BankID { get; set; }
        public string BankAcctNo { get; set; }
        public string PFAAcctNo { get; set; }
        public string PFAID { get; set; }
        public int EmployeeTypeID { get; set; }
        public string EmployeeStatus { get; set; }
        public bool IsFirstLogin { get; set; }
        public int JobTitleID { get; set; }
        public DateTime DateCreated { get; set; }
    }



    public class CreateEmployeeDTO
    {
        public string HRTag { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PostalAddress { get; set; }
        public string ResidentialAddress { get; set; }
        public int CompanyID { get; set; }
        public int MaritalID { get; set; }
        public int NationalityID { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DOB { get; set; }
        public int StateID { get; set; }
        public int LGAID { get; set; }
        public string EmailAddress { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfDepRelative { get; set; }
        public decimal height { get; set; }
        public int BloodGroupID { get; set; }
        public int ReligionID { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime DateConfirmed { get; set; }
        public int StartBranchID { get; set; }
        public int CurrentBranchID { get; set; }
        public int StartDepartmentID { get; set; }
        public int CurrentDepartmentID { get; set; }
        public int StartPositionID { get; set; }
        public int CurrentPositionID { get; set; }
        public string Picture { get; set; }
        public string Hobby { get; set; }
        public int BankID { get; set; }
        public string BankAcctNo { get; set; }
        public string PFAAcctNo { get; set; }
        public string PFAID { get; set; }
        public int EmployeeTypeID { get; set; }
        public string EmployeeStatus { get; set; }
        public bool IsFirstLogin { get; set; }
        public int JobTitleID { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
