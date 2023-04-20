using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.DTOs.Account
{
    public class CreateUserDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be {2} characters long.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be {2} characters long.")]
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string DOB { get; set; }
        public string ResumptionDate { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        public string OfficialMail { get; set; }
        

        public string PhoneNumber { get; set; }

        [Required]
        public int RoleId { get; set; }
        public long CompanyId { get; set; }
        public long DepartmentId { get; set; }
        public long UnitID { get; set; }
        public long UnitHeadID { get; set; }
        public long HodID { get; set; }
        public long GradeID { get; set; }
        public long EmployeeTypeID { get; set; }
        public long PositionID { get; set; }
        public long BranchID { get; set; }
        public long EmploymentStatusID { get; set; }
        public long GroupID { get; set; }

        

    }

    public class CreateUserResponse
    {
        public string ExecMessage { get; set; }
        public int IsCreated { get; set; }
    }
}
