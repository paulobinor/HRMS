using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.DTOs
{
    public class CompanyDTO
    {
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public long LastStaffNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string CompanyLogo { get; set; }
        public string ContactPhone { get; set; }

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

    public class CreateCompanyDto
    {
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public long LastStaffNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public string Website { get; set; }
        public string CompanyLogo { get; set; }
    }

    public class UpdateCompanyDto
    {
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public long LastStaffNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public string Website { get; set; }
        public string CompanyLogo { get; set; }
    }

    public class DeleteCompanyDto
    {
        public long CompanyId { get; set; }
        public string Reasons_For_Delete { get; set; }
    }
}
