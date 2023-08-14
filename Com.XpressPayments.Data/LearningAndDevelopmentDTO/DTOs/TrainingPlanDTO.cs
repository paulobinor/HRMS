using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs
{
    internal class TrainingPlanDTO
    {
            public long TrainingPlanID { get; set; }
            public long UserId { get; set; }
            public string Name { get; set; }
            public string Department { get; set; }
            public DateTime Created_Date { get; set; }
            public bool IsApproved { get; set; }
            public bool IsDisapproved { get; set; }
            public string ApprovedBy { get; set; } 
            public string TrainingProvider { get; set; } 
            public double TotalCost { get; set; }
       
     
    }
    public class TrainingPlanCreate
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string IdentifiedSkills { get; set; }
        public string TrainingNeeds { get; set; }

        public string TrainingProvider { get; set; }
        public double EstimatedCost { get; set; }
        public string Created_By_User_Email { get; set; }
    }
    public class TrainingPlanDisapproved
    {
        public long TrainingPlanID { get; set; }
        public string Reasons_For_Disapprove { get; set; }
    }
}
