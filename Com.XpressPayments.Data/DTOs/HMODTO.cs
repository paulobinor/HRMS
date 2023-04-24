using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.DTOs
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
        public string NameOfChildren { get; set; }
        public string ChildrenSex { get; set; }
        public string ChildrenDOB { get; set; }
        public string ChildrenBloodGrp { get; set; }
        public string ChildrenGenotype { get; set; }
        public long ChildrenChioceOfHospital { get; set; }
        public string Signature { get; set; }
        public string Date { get; set; }
        public string StaffPassport { get; set; }
        public string SpousePassport { get; set; }
        public string Child1Passport { get; set; }
        public string Child2Passport { get; set; }
        public string Child3Passport { get; set; }
        public string Child4Passport { get; set; }
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
        public string NameOfChildren { get; set; }
        public string ChildrenSex { get; set; }
        public string ChildrenDOB { get; set; }
        public string ChildrenBloodGrp { get; set; }
        public string ChildrenGenotype { get; set; }
        public long ChildrenChioceOfHospital { get; set; }
        public string Signature { get; set; }
        public string Date { get; set; }
        public string StaffPassport { get; set; }
        public string SpousePassport { get; set; }
        public string Child1Passport { get; set; }
        public string Child2Passport { get; set; }
        public string Child3Passport { get; set; }
        public string Child4Passport { get; set; }
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
        public string NameOfChildren { get; set; }
        public string ChildrenSex { get; set; }
        public string ChildrenDOB { get; set; }
        public string ChildrenBloodGrp { get; set; }
        public string ChildrenGenotype { get; set; }
        public long ChildrenChioceOfHospital { get; set; }
        public string Signature { get; set; }
        public string Date { get; set; }
        public string StaffPassport { get; set; }
        public string SpousePassport { get; set; }
        public string Child1Passport { get; set; }
        public string Child2Passport { get; set; }
        public string Child3Passport { get; set; }
        public string Child4Passport { get; set; }
        public long CompanyID { get; set; }
    }

    public class DeleteHMODTO
    {

        public long ID { get; set; }
        public string Reasons_For_Delete { get; set; }
    }
}
