using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
    public class CreateGradeDTO
    {
        public int CompanyID { get; set; }
        //public int GradeID { get; set; }
        public string GradeName { get; set; }
        //public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int IsActive { get; set; }
    }

    public class UpdateGradeDTO
    {
        public int CompanyID { get; set; }
        public int GradeID { get; set; }
        public string GradeName { get; set; }
        //public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int IsActive { get; set; }
    }

    public class DelGradeDTO
    {
        public int CompanyID { get; set; }
        public int GradeID { get; set; }
        //public string GradeName { get; set; }
        //public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int IsActive { get; set; }
    }

    public class GradeDTO
    {
        public int CompanyID { get; set; }
        public int GradeID { get; set; }
        public string GradeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int IsActive { get; set; }
    }
}
