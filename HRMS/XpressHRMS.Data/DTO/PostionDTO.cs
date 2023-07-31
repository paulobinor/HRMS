using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
    public class CreatePositionDTO
    {
        public int CompanyID { get; set; }
        //public int PositionID { get; set; }
        public string PositionName { get; set; }
        //public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int IsActive { get; set; }

    }

    public class   UPdatePositionDTO
    {
        public int CompanyID { get; set; }
        public int PositionID { get; set; }
        public string PositionName { get; set; }
        //public string CreatedBy { get; set; }
<<<<<<< HEAD:HRMS/XpressHRMS.Data/DTO/PostionDTO.cs
<<<<<<< HEAD:HRMS/XpressHRMS.Data/DTO/PostionDTO.cs
        public DateTime DateCreated { get; set; }
        public int IsActive { get; set; }
=======
=======
>>>>>>> parent of 55b359c (commit):XpressHRMS.Data/DTO/PostionDTO.cs
        //public DateTime DateCreated { get; set; }
        //public int IsActive { get; set; }
>>>>>>> parent of 55b359c (commit):XpressHRMS.Data/DTO/PostionDTO.cs

    }

    public class DeletePositionDTO
    {
        public int CompanyID { get; set; }
        public int PositionID { get; set; }
        //public string PositionName { get; set; }
        //public string CreatedBy { get; set; }
<<<<<<< HEAD:HRMS/XpressHRMS.Data/DTO/PostionDTO.cs
<<<<<<< HEAD:HRMS/XpressHRMS.Data/DTO/PostionDTO.cs
        public DateTime DateCreated { get; set; }
        public int IsActive { get; set; }
=======
=======
>>>>>>> parent of 55b359c (commit):XpressHRMS.Data/DTO/PostionDTO.cs
        //public DateTime DateCreated { get; set; }
        //public int IsActive { get; set; }
>>>>>>> parent of 55b359c (commit):XpressHRMS.Data/DTO/PostionDTO.cs

    }


    public class PositionDTO
    {
        public int CompanyID { get; set; }
        public int PositionID { get; set; }
        public string PositionName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int IsActive { get; set; }

    }
}
