using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Com.XpressPayments.Common.ViewModels
{
    public class ResignationInterviewVM
    {

        public long EmployeeId { get; set; }
        public long CompanyId { get; set; }
        //public int ResignationId { get; set; }
        //public DateTime ResumptionDate { get; set; }
        //public string ReasonForResignation { get; set; }
        //public DateTime ExitDate { get; set; }
        public string OtherRemarks { get; set; }
        public string Signature { get; set; }
        public DateTime Date { get; set; }
        public string WhatDidYouLikeMostAboutTheCompanyAndYourJob { get; set; }
        public string WhatDidYouLeastLikeAboutTheCompanyAndYourJob { get; set; }
        public string DoYouFeelYouWerePlacedInAPositionCompatibleWithYourSkillSet { get; set; }
        public string IfYouAreTakingAnotherJob_WhatKindOfJobWillYouBeTaking { get; set; }
        public string CouldOurCompanyHaveMadeAnyImprovementsThatMightHaveMadeYouStay { get; set; }

        public List<InterviewDetailsSection> SectionOne { get; set; }
        public List<InterviewDetailsSection> SectionTwo { get; set;}
        
    }

    public class InterviewDetailsSection
    {
        public int ID { get; set; }
        public int Value { get; set; }
        //[JsonIgnore]
        public string Scale { get; set; }
        public string InterviewDetailName { get; set; }
    }
}
