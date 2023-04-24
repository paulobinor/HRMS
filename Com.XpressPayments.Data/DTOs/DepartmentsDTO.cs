using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.DTOs
{
    public class DepartmentsDTO
    {
        public long DeptId { get; set; }
        public string DepartmentName { get; set; }
        //public string DepartmentMail { get; set; }
       
        public long HodID { get; set; }
        public string HODName { get; set; }
        public long GroupID { get; set; }
        public string GroupName { get; set; }
        public long BranchID { get; set; }
        public string BranchName { get; set; }

        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public string CompanyName { get; set; }
        public long CompanyId { get; set; }

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

    public class CreateDepartmentDto
    {
        public string DepartmentName { get; set; }
        
       
        public long HodID { get; set; }
        public long GroupID { get; set; }
        
        public long BranchID { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public long CompanyId { get; set; }
    }

    public class UpdateDepartmentDto
    {
        public long DeptId { get; set; }
        public string DepartmentName { get; set; }
        
       
        public long HodID { get; set; }
        public long GroupID { get; set; }
        
        public long BranchID { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public long CompanyId { get; set; }
    }

    public class DeleteDepartmentDto
    {
        public long DeptId { get; set; }
        public string Reasons_For_Delete { get; set; }
    }
}
