using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class ResignationInterviewDTO
    {
        public long UserID { get; set; }
        public int SRFID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateCreated { get; set; }
        public string Created_By_User_Email { get; set; }
        public string ReasonForResignation { get; set; }
        public DateTime LastDayOfWork { get; set; }
        public string QuestionOne { get; set; }
        public string QuestionTwo { get; set; }
        public string QuestionThree { get; set; }
        public string QuestionFour { get; set; }
        public string QuestionFive { get; set; }
        public string Comment { get; set; }
        public string Signature  { get; set; }
        public DateTime Date { get; set; }
        public bool IsApproved { get; set; }
        public int ApprovedByUserID { get; set; }
    }
}
