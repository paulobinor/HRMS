using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
   public class RoleDTO
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string CreatedBy { get; set; }
        public int CompanyID { get; set; }
        public DateTime DateCreated { get; set; }

    }
    public class CreateRoleDTO
    {
        public string RoleName { get; set; }
        public string CreatedBy { get; set; }
        public int CompanyID { get; set; }

    }

    public class UpdateRoleDTO
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public int CompanyID { get; set; }

    }

    public class DeleteRoleDTO
    {
        public int RoleID { get; set; }
        public int CompanyID { get; set; }

    }
}
