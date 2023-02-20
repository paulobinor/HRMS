using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        public string CreatedBy { get; set; }
        public int CompanyID { get; set; }
        public int IsHeadQuater { get; set; }
        public string IsActive { get; set; }
        public string UpdatedBy { get; set; }
        public string DateUpdated { get; set; }
        public string DeletedBy { get; set; }
        public string DateDeleted { get; set; }
        public string EnableBy { get; set; }
        public string DisableBy { get; set; }
        public string EnableDate { get; set; }
        public string DisableDate { get; set; }
    }

    public class CreateBranchDTO
    {
        public string BranchName { get; set; }
        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public string CompanyID { get; set; }
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
        public string CompanyID { get; set; }
        public int IsHeadQuater { get; set; }
        public string Address { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public int LgaID { get; set; }
        [JsonIgnore]
        public string UpdatedBy { get; set; }
    }

    public class DeleteBranchDTO
    {
        public int BranchID { get; set; }
        public string CompanyID { get; set; }
        [JsonIgnore]
        public string DeletedBy { get; set; }

    }

    public class DisBranchDTO
    {
        public int BranchID { get; set; }
        public string CompanyID { get; set; }
        [JsonIgnore]
        public string DisableBy { get; set; }

    }

    public class EnBranchDTO
    {
        public int BranchID { get; set; }
        public string CompanyID { get; set; }
        [JsonIgnore]
        public string EnableBy { get; set; }

    }
}
