using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
    public class DisCompanyDTO
    {
        public int CompanyID { get; set; }
        [JsonIgnore]
        public string DisableBy { get; set; }
       
    }

    public class EnCompanyDTO
    {
        public int CompanyID { get; set; }
        [JsonIgnore]
        public string EnableBy { get; set; }

    }

    public class CompanyDTO
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyLogo { get; set; }
        public string Website { get; set; }
        public string CreatedBy { get; set; }
        public string DateCreated { get; set; }
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

    public class CreateCompanyDTO
    {
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyLogo { get; set; }
        public string Website { get; set; }
        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public string DateCreated { get; set; }
        [JsonIgnore]
        public string IsActive { get; set; }
        [JsonIgnore]
        public string UpdatedBy { get; set; }
        [JsonIgnore]
        public string DateUpdated { get; set; }
        [JsonIgnore]
        public string DeletedBy { get; set; }
        [JsonIgnore]
        public string DateDeleted { get; set; }
        [JsonIgnore]
        public string EnableBy { get; set; }
        [JsonIgnore]
        public string DisableBy { get; set; }
        [JsonIgnore]
        public string EnableDate { get; set; }
        [JsonIgnore]
        public string DisableDate { get; set; }

    }
    public class UpdateCompanyDTO
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyLogo { get; set; }
        public string Website { get; set; }
        [JsonIgnore]
        public string UpdatedBy { get; set; }
    }

    public class DeleteCompanyDTO
    {
        public int CompanyID { get; set; }
        [JsonIgnore]
        public string DeletedBy { get; set; }

    }
}
