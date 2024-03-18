using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class HospitalProvidersDTO
    {

        public long ID { get; set; }
        public string ProvidersNames { get; set; }
        public long StateID { get; set; }
        public string Town1 { get; set; }
        public string Town2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public long HospitalPlanID { get; set; }
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

    public class CreateHospitalProvidersDTO
    {
        public string ProvidersNames { get; set; }
        public long StateID { get; set; }
        public string Town1 { get; set; }
        public string Town2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public long HospitalPlanID { get; set; } 
        public long CompanyID { get; set; }

        public string Created_By_User_Email { get; set; }
    }


    public class UpdateHospitalProvidersDTO
    {
        public long ID { get; set; }
        public string ProvidersNames { get; set; }
        public long StateID { get; set; }
        public string Town1 { get; set; }
        public string Town2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public long HospitalPlanID { get; set; }
        public long CompanyID { get; set; }
    }


    public class DeleteHospitalProvidersDTO
    {
        public long ID { get; set; }
        public string Reasons_For_Delete { get; set; }
    }
}
