﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.DTOs
{
    public  class GradeDTO
    {
        public long GradeID { get; set; }
        public string GradeName { get; set; }
        public string NumberOfVacationDays { get; set; }
        public string NumberOfVacationSplit { get; set; }
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

    public class CreateGradeDTO
    {
        public string GradeName { get; set; }
        public string NumberOfVacationDays { get; set; }
        public string NumberOfVacationSplit { get; set; }
        public long CompanyID { get; set; }
    }

    public class UpdateGradeDTO
    {
        public long GradeID { get; set; }
        public string GradeName { get; set; }
        public string NumberOfVacationDays { get; set; }
        public string NumberOfVacationSplit { get; set; }
        public long CompanyID { get; set; }
    }

    public class DeleteGradeDTO
    {
        public long GradeID { get; set; }
        public string Reasons_For_Delete { get; set; }
    }
}
