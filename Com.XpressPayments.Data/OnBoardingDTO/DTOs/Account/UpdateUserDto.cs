using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.DTOs.Account
{
    public class UpdateUserDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        public string OfficialMail { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be {2} characters long.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be {2} characters long.")]
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string DOB { get; set; }
        public string ResumptionDate { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(20, ErrorMessage = "The {0} must be {2} characters long.")]
        public string PhoneNumber { get; set; }

        [Required]
        public int RoleId { get; set; }
        public long CompanyId { get; set; }
        public long DeptId { get; set; }
        public long UnitID { get; set; }
 
        public long GradeID { get; set; }
        public long EmployeeTypeID { get; set; }
     
        public long BranchID { get; set; }
        public long EmploymentStatusID { get; set; }
    
        public long JobDescriptionID { get; set; }

    }
    public class UsertoDeptMappingDto
    {
        public List<string> Email { get; set; }
        public long CompanyId { get; set; }
        public long DeptId { get; set; }
    }

    public class UsertoDeptMappingErrorsDto
    {
        public string Email { get; set; }
    }
}
