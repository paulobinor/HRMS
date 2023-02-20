using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
   public class DepartmentDTO
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int HodID { get; set; }
        public string CompanyID { get; set; }
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

    public class CreateDepartmentDTO
    {
        public string DepartmentName { get; set; }
        public int HodID { get; set; }
        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public string CompanyID { get; set; }
        

    }
    public class GetDepartmentDTO
    {
        public string DepartmentName { get; set; }
        public int DepartmentID { get; set; }
        public int HodID { get; set; }
        public string CompanyID { get; set; }
        //public DateTime DateCreated { get; set; }
        public int CreatedByUserID { get; set; }
        public bool isDeleted { get; set; }
        //public bool isActive { get; set; }

    }
    public class UpdateDepartmentDTO
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int HodID { get; set; }
        public string CompanyID { get; set; }
        [JsonIgnore]
        public string UpdatedBy { get; set; }

    }

    public class DeleteDepartmentDTO
    {
        public int DepartmentID { get; set; }
        public string CompanyID { get; set; }
        [JsonIgnore]
        public string DeletedBy { get; set; }
    }


    public class DisDepartmentDTO
    {
        public int DepartmentID { get; set; }
        public string CompanyID { get; set; }
        [JsonIgnore]
        public string DisableBy { get; set; }

    }

    public class EnDepartmentDTO
    {
        public int DepartmentID { get; set; }
        public string CompanyID { get; set; }
        [JsonIgnore]
        public string EnableBy { get; set; }

    }
}
