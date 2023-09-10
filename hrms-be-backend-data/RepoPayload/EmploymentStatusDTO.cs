using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class EmploymentStatusDTO
    {
        public long EmploymentStatusID { get; set; }
        public string EmploymentStatusName { get; set; }
        public long CompanyID { get; set; }

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

    public class CreateEmploymentStatusDTO
    {

        public string EmploymentStatusName { get; set; }
        public long CompanyID { get; set; }

        public string Created_By_User_Email { get; set; }
    }

    public class UpdateEmploymentStatusDTO
    {
        public long EmploymentStatusID { get; set; }
        public string EmploymentStatusName { get; set; }
        public long CompanyID { get; set; }

        public string Created_By_User_Email { get; set; }
    }

    public class DeleteEmploymentStatusDTO
    {
        public long EmploymentStatusID { get; set; }
        public string Reasons_For_Delete { get; set; }
    }
}
