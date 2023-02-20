using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
    public class CreatePositionDTO
    {
        [JsonIgnore]
        public string CompanyID { get; set; }
        //public int PositionID { get; set; }
        public string PositionName { get; set; }
        [JsonIgnore]
        public string CreatedBy { get; set; }
        //public DateTime DateCreated { get; set; }
        //public int IsActive { get; set; }

    }

    public class   UPdatePositionDTO
    {
        public string CompanyID { get; set; }
        public int PositionID { get; set; }
        public string PositionName { get; set; }
        [JsonIgnore]
        //public string UpdatedByUpd { get; set; }
        //[JsonIgnore]
        public DateTime DateUpdate { get; set; }
        //public int IsActive { get; set; }

    }

    public class DeletePositionDTO
    {
        public string CompanyID { get; set; }
        public int PositionID { get; set; }
        [JsonIgnore]
        public string DeletedBy { get; set; }
        [JsonIgnore]
        public DateTime DateDeleted { get; set; }
        //public int IsActive { get; set; }

    }


    public class PositionDTO
    {
        public int CompanyID { get; set; }
        public int PositionID { get; set; }
        public string PositionName { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTime DateCreated { get; set; }
        //public int IsActive { get; set; }

    }
}
