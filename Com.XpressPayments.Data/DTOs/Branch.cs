using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.DTOs
{
    public class BranchDTO
    {
        public long BranchID { get; set; }
        public string BranchName { get; set; }
        public long CompanyID { get; set; }
      
        public string Address { get; set; }
        public long CountryID { get; set; }
        public string CountryName { get; set; }
        public long StateID { get; set; }
        public string StateName { get; set; }
        public long LgaID { get; set; }
        public string LGA_Name { get; set; }

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

    public class CreateBranchDTO
    {
       
        public string BranchName { get; set; }
        public long CompanyID { get; set; }
        public string Address { get; set; }
        public long CountryID { get; set; }
        public long StateID { get; set; }
        public long LgaID { get; set; }
    }

    public class UpdateBranchDTO
    {
        public long BranchID { get; set; }
        public string BranchName { get; set; }
        public long CompanyID { get; set; }
        public string Address { get; set; }
        public long CountryID { get; set; }
        public long StateID { get; set; }
        public long LgaID { get; set; }
    }

    public class DeleteBranchDTO
    {
        public long BranchID { get; set; }
        public string Reasons_For_Delete { get; set; }
    }
}
