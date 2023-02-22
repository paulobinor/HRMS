using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
   public class DepartmentDTO
    {
        public string DepartmentName { get; set; }
        public int HODEmployeeID { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public int CompanyID { get; set; }
        public bool isActive { get; set; }

    }

    public class CreateDepartmentDTO
    {
        public string DepartmentName { get; set; }
        public int HODEmployeeID { get; set; }
        public DateTime DateCreated { get; set; }
        public int CompanyID { get; set; }
        public bool isActive { get; set; }

    }
    public class GetDepartmentDTO
    {
        public string DepartmentName { get; set; }
        public int DepartmentID { get; set; }
        public int HODEmployeeID { get; set; }
        public int CompanyID { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedByUserID { get; set; }
        public bool isDeleted { get; set; }
        public bool isActive { get; set; }

    }
    public class UpdateDepartmentDTO
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int HODEmployeeID { get; set; }
        public DateTime DateUpdated { get; set; }
        public int CompanyID { get; set; }

    }

    public class DeleteDepartmentDTO
    {
        public int DepartmentID { get; set; }
        public int CompanyID { get; set; }


    }
}
