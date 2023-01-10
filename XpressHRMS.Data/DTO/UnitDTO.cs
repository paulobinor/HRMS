using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
   public class UnitDTO
    {
        public string UnitName { get; set; }
        public int HODEmployeeID { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedByUserID { get; set; }
        public bool isActive { get; set; }
    }
    public class UpdateUnitDTO
    {
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public int HODEmployeeID { get; set; }
        public int DepartmentID { get; set; }
        public DateTime DateUpdated { get; set; }
    }

    public class DeleteUnitDTO
    {
        public int UnitID { get; set; }

    }
}
