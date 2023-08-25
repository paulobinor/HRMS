using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs
{
    internal class TrainingInductionDTO
    {
        public long TrainingInductionID { get; set; }
        public long UserId { get; set; }
        public string CompanyId { get; set; }
        public string Department { get; set; }
        public string TrainingTitle { get; set; }
        public string TrainingVenue { get; set; }
        public string TrainingTopic { get; set; }
        public string TrainingTime { get; set; }
        public string TrainingMode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

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
}
