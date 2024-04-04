using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class ResignationInterviewDTO
    {
        public long ResignationInterviewID { get; set; }
        public long EmployeeId { get; set; }
        public long ResignationId { get; set; }
        public long CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string SupervisorName { get; set; }
        public string Department { get; set; }
        public string OfficialEmail { get; set; }
        public DateTime DateCreated { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime ResumptionDate { get; set; }
        public string ReasonForResignation { get; set; }
        public DateTime ExitDate { get; set; }
        public string OtherRemarks { get; set; }
        public string Signature  { get; set; }
        public long HrEmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string WhatDidYouLikeMostAboutTheCompanyAndYourJob { get; set; }
        public string WhatDidYouLeastLikeAboutTheCompanyAndYourJob { get; set; }
        public string DoYouFeelYouWerePlacedInAPositionCompatibleWithYourSkillSet { get; set; }
        public string IfYouAreTakingAnotherJob_WhatKindOfJobWillYouBeTaking { get; set; }
        public string CouldOurCompanyHaveMadeAnyImprovementsThatMightHaveMadeYouStay { get; set; }

    }
}
