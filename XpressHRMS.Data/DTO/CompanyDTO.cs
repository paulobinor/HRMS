using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
    public class CompanyDTO
    {
        public int CompanyID { get; set; }

        public string CompanyName { get; set; }

        public string Companyphonenumber { get; set; }
        public string MissionStmt { get; set; }
        public string VisionStmt { get; set; }
        public string CompanyTheme { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public string CompanyLogo { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
    }

    public class CreateCompanyDTO
    {
        public string CompanyName { get; set; }
        public string Companyphonenumber { get; set; }
        public string MissionStmt { get; set; }
        public string VisionStmt { get; set; }
        public List<string> CompanyTheme { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public string CompanyLogo { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
    }
    public class UpdateCompanyDTO
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Companyphonenumber { get; set; }
        public string MissionStmt { get; set; }
        public string VisionStmt { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public string CompanyLogo { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
    }
}
