using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs
{
    public class TrainingInductionDTO
    {
        public long TrainingInductionID { get; set; }
        public long UserId { get; set; }
        public long CompanyID { get; set; }
        public string TrainingTitle { get; set; }
        public string TrainingVenue { get; set; }
        public string TrainingProvider { get; set; }
        public string TrainingTime { get; set; }
        public string TrainingMode { get; set; }
        public string Documents { get; set; }
        public string Media { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long UnitHeadUserID { get; set; }
        public long HodUserID { get; set; }
        public long HrUserID { get; set; }
        public bool IsHodApproved { get; set; }
        public bool IsUnitHeadApproved { get; set; }
        public bool IsHrApproved { get; set; }
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
    public class TrainingInductionCreate
    {
        public long UserID { get; set; }
        public long CompanyID { get; set; }
        public string TrainingTitle { get; set; }
        public string TrainingVenue { get; set; }
        public string TrainingProvider { get; set; }
        public string TrainingTime { get; set; }
        public string TrainingMode { get; set; }
        public string Documents { get; set; }
        public string Media { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Created_Date { get; set; }
        public string Created_By_User_Email { get; set; }

    }
    public class TrainingInductionUpdate
    {
        public long TrainingInductionID { get; set; }
        public long CompanyID { get; set; }
        public string TrainingTitle { get; set; }
        public string TrainingVenue { get; set; }
        public string TrainingProvider { get; set; }
        public string TrainingTime { get; set; }
        public string TrainingMode { get; set; }
        public string Documents { get; set; }
        public string Media { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Updated_Date { get; set; }
        public string Updated_By_User_Email { get; set; }

    }
    public class TrainingInductionApproved
    {
        public long TrainingInductionID { get; set; }
        public long UserID { get; set; }
    }
    public class TrainingInductionDisapproved
    {
        public long TrainingInductionID { get; set; }
        public long UserID { get; set; }
        public string Reasons_For_Disapprove { get; set; }
    }
    public class TrainingInductionDelete
    {
        public long TrainingInductionID { get; set; }
        public string Reasons_For_Delete { get; set; }
    }
}
