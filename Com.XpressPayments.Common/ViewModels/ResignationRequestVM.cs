using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Com.XpressPayments.Common.ViewModels
{
    public class ResignationRequestVM
    {
        public long UserId { get; set; }
        public DateTime Date { get; set; }
        public string ReasonForResignation { get; set; }
        public DateTime LastDayOfWork { get; set; }
       // public IFormFile SignedResignationLetter { get; set; }
        public string fileName { get; set; }    
        public long CompanyID { get; set; }
    }
}
