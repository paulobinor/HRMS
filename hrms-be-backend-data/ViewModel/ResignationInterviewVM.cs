using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Com.XpressPayments.Common.ViewModels
{
    public class ResignationInterviewVM
    {
        public long UserID { get; set; }
        public int SRFID { get; set; }
        public DateTime Date { get; set; }
        public string ReasonForResignation { get; set; }
        public DateTime LastDayOfWork { get; set; }
        public string QuestionOne { get; set; }
        public string QuestionTwo { get; set; }
        public string QuestionThree { get; set; }
        public string QuestionFour { get; set; }
        public string QuestionFive { get; set; }
        public string Comment { get; set; }

        public List<InterviewDetailsSection> SectionOne { get; set; }
        public List<InterviewDetailsSection> SectionTwo { get; set;}
        
    }

    public class InterviewDetailsSection
    {
        public int ID { get; set; }
        public int Value { get; set; }
        [JsonIgnore]
        public string Scale { get; set; }
    }
}
