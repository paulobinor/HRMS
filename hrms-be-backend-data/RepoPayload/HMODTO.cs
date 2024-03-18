using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class HMODTO
    {
        public long ID { get; set; }
        public string HMONumber { get; set; }
        public string StaffID { get; set; }
        

        public string DOB { get; set; }
        public string Telephone { get; set; }
        public string ActiveEmail { get; set; }
        public string MaritalStatus { get; set; }
        public string NumberOfChildren { get; set; }
        public string BloodGrp { get; set; }
        public string Genotype { get; set; }
        public long ChioceOfHospital { get; set; }
        public string SpouseName { get; set; }
        public string SpouseSex { get; set; }
        public string SpouseDOB { get; set; }
        public string SpouseBloodGrp { get; set; }
        public string SpouseGenotype { get; set; }
        public long SpouseChioceOfHospital { get; set; }
        public string Child1Name { get; set; }
        public string Child1Sex { get; set; }
        public string Child1DOB { get; set; }
        public string Child1Genotype { get; set; }
        public string Child1BloodGrp { get; set; }
        public string Child1ChioceOfHospital { get; set; }
        public string Child2Name { get; set; }
        public string Child2Sex { get; set; }
        public string Child2DOB { get; set; }
        public string Child2Genotype { get; set; }
        public string Child2BloodGrp { get; set; }
        public string Child2ChioceOfHospital { get; set; }

        public string Signature { get; set; }
        public string Date { get; set; }
        public string StaffPassport { get; set; }
        public string SpousePassport { get; set; }
        public string Child1Passport { get; set; }
        public string Child2Passport { get; set; }

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

    public class CreateHMODTO
    {
        public string HMONumber { get; set; }
        public string StaffID { get; set; }
       
        
        public string DOB { get; set; }
        public string Telephone { get; set; }
        public string ActiveEmail { get; set; }
        public string MaritalStatus { get; set; }
        public string NumberOfChildren { get; set; }
        public string BloodGrp { get; set; }
        public string Genotype { get; set; }
        public long ChioceOfHospital { get; set; }
        public string SpouseName { get; set; }
        public string SpouseSex { get; set; }
        public string SpouseDOB { get; set; }
        public string SpouseBloodGrp { get; set; }
        public string SpouseGenotype { get; set; }
        public long SpouseChioceOfHospital { get; set; }
        public string Child1Name { get; set; }
        public string Child1Sex { get; set; }
        public string Child1DOB { get; set; }
        public string Child1Genotype { get; set; }
        public string Child1BloodGrp { get; set; }
        public string Child1ChioceOfHospital { get; set; }
        public string Child2Name { get; set; }
        public string Child2Sex { get; set; }
        public string Child2DOB { get; set; }
        public string Child2Genotype { get; set; }
        public string Child2BloodGrp { get; set; }
        public string Child2ChioceOfHospital { get; set; }

        public string Signature { get; set; }
        public string Date { get; set; }
        public string StaffPassport { get; set; }
        public string SpousePassport { get; set; }
        public string Child1Passport { get; set; }
        public string Child2Passport { get; set; }

        public long CompanyID { get; set; }

        public string Created_By_User_Email { get; set; }
    }

    public class UpdateHMODTO
    {
        public long ID { get; set; }
        public string HMONumber { get; set; }
        public string StaffID { get; set; }
       
 
        public string DOB { get; set; }
        public string Telephone { get; set; }
        public string ActiveEmail { get; set; }
        public string MaritalStatus { get; set; }
        public string NumberOfChildren { get; set; }
        public string BloodGrp { get; set; }
        public string Genotype { get; set; }
        public long ChioceOfHospital { get; set; }
        public string SpouseName { get; set; }
        public string SpouseSex { get; set; }
        public string SpouseDOB { get; set; }
        public string SpouseBloodGrp { get; set; }
        public string SpouseGenotype { get; set; }
        public long SpouseChioceOfHospital { get; set; }
        public string Child1Name { get; set; }
        public string Child1Sex { get; set; }
        public string Child1DOB { get; set; }
        public string Child1Genotype { get; set; }
        public string Child1BloodGrp { get; set; }
        public string Child1ChioceOfHospital { get; set; }
        public string Child2Name { get; set; }
        public string Child2Sex { get; set; }
        public string Child2DOB { get; set; }
        public string Child2Genotype { get; set; }
        public string Child2BloodGrp { get; set; }
        public string Child2ChioceOfHospital { get; set; }

        public string Signature { get; set; }
        public string Date { get; set; }
        public string StaffPassport { get; set; }
        public string SpousePassport { get; set; }
        public string Child1Passport { get; set; }
        public string Child2Passport { get; set; }

        public long CompanyID { get; set; }
    }

    public class DeleteHMODTO
    {

        public long ID { get; set; }
        public string Reasons_For_Delete { get; set; }
    }
}
