using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
   public class UnitDTO
    {
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public int HODEmployeeID { get; set; }
<<<<<<< HEAD:HRMS/XpressHRMS.Data/DTO/UnitDTO.cs
<<<<<<< HEAD:HRMS/XpressHRMS.Data/DTO/UnitDTO.cs
        public DateTime DateCreated { get; set; }
        public int CreatedByUserID { get; set; }
=======
=======
>>>>>>> parent of 55b359c (commit):XpressHRMS.Data/DTO/UnitDTO.cs
        //public DateTime DateCreated { get; set; }
        //public int CreatedByUserID { get; set; }
>>>>>>> parent of 55b359c (commit):XpressHRMS.Data/DTO/UnitDTO.cs
        public int CompanyID { get; set; }
        public int IsDeleted { get; set; }
    }
    public class CreateUnitDTO
    {
        public string UnitName { get; set; }
        public int HODEmployeeID { get; set; }
<<<<<<< HEAD:HRMS/XpressHRMS.Data/DTO/UnitDTO.cs
<<<<<<< HEAD:HRMS/XpressHRMS.Data/DTO/UnitDTO.cs
        public DateTime DateCreated { get; set; }
        public int CreatedByUserID { get; set; }
=======
        //public DateTime DateCreated { get; set; }
        //public int CreatedByUserID { get; set; }
>>>>>>> parent of 55b359c (commit):XpressHRMS.Data/DTO/UnitDTO.cs
=======
        //public DateTime DateCreated { get; set; }
        //public int CreatedByUserID { get; set; }
>>>>>>> parent of 55b359c (commit):XpressHRMS.Data/DTO/UnitDTO.cs
        public int CompanyID { get; set; }

    }
    public class UpdateUnitDTO
    {
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public int HODEmployeeID { get; set; }
        public int DepartmentID { get; set; }
        public int CompanyID { get; set; }

        public DateTime DateUpdated { get; set; }
    }

    public class DeleteUnitDTO
    {
        public int UnitID { get; set; }
        public int CompanyID { get; set; }

    }
}
