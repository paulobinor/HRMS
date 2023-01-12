using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
    public class CreateEmployeeTypeDTO
    {
        public int CompanyID { get; set; }
        //public int EmployeeTypeID { get; set; }
        public string EmployeeTypeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int IsActive { get; set; }
    }

    public class UpdateEmployeeTypeDTO
    {
        public int CompanyID { get; set; }
        public int EmployeeTypeID { get; set; }
        public string EmployeeTypeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int IsActive { get; set; }
    }

    public class DelEmployeeTypeDTO
    {
        public int CompanyID { get; set; }
        public int EmployeeTypeID { get; set; }
        //public string EmployeeTypeName { get; set; }
        //public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int IsActive { get; set; }
    }

    public class EmployeeTypeDTO
    {
        public int CompanyID { get; set; }
        public int EmployeeTypeID { get; set; }
        public string EmployeeTypeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int IsActive { get; set; }
    }
}
