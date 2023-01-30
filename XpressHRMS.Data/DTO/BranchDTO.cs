using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
   public class BranchDTO
    {
        public int BranchID { get; set; }
        public string Address { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public int LgaID { get; set; }
        public string BranchName { get; set; }
        //public DateTime DateCreated { get; set; }
        //public string CreatedBy { get; set; }
        public int CompanyID { get; set; }
        public int IsDeleted { get; set; }
        public int IsHeadQuater { get; set; }
    }

    public class CreateBranchDTO
    {
        public string BranchName { get; set; }
        //public string CreatedBy { get; set; }
        public int CompanyID { get; set; }
        public int IsHeadQuater { get; set; }
        public string Address { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public int LgaID { get; set; }
    }

    public class UpdateBranchDTO
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public int CompanyID { get; set; }
        public int IsHeadQuater { get; set; }
        public string Address { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public int LgaID { get; set; }
    }

    public class DeleteBranchDTO
    {
        public int BranchID { get; set; }
        public int CompanyID { get; set; }
      
    }
}
