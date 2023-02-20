using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
    public class CreateHodDTO
    {
        //public int HodID { get; set; }
        public string HODName { get; set; }
        public int DepartmentID { get; set; }
        [JsonIgnore]
        public string CompanyID { get; set; }
        
        [JsonIgnore]
        public string CreatedBy { get; set; }
        
    }
    public class UpdateHodDTO
    {
        public int HodID { get; set; }
        public string HODName { get; set; }
        public int DepartmentID { get; set; }
        public string CompanyID { get; set; }
        [JsonIgnore]
        public string UpdatedBy { get; set; }
    }
    public class DelHodDTO
    {
        public int HodID { get; set; }
        public int DepartmentID { get; set; }
        public string CompanyID { get; set; }
        [JsonIgnore]
        public string DeletedBy { get; set; }
    }
    public class HodDTO
    {
        public int HodID { get; set; }
        //public string HODName { get; set; }
        public int DepartmentID { get; set; }
        public string CompanyID { get; set; }
    }

    public class DisableHodDTO
    {
        public int HodID { get; set; }
        public int DepartmentID { get; set; }
        public string CompanyID { get; set; }
        public string DisableBy { get; set; }

    }

    public class EnableHodDTO
    {
        public int HodID { get; set; }
        public int DepartmentID { get; set; }
        public string CompanyID { get; set; }
        public string EnableBy { get; set; }

    }
} 

