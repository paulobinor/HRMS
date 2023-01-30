using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
    public class AuditDTO
    {
        public long AuditTrailId { get; set; }
        public long UserId { get; set; }
        //public DateTime AccessDate { get; set; }
        public string Operation { get; set; }
        public string AccessedFromIpAddress { get; set; }
        public string AccessedFromPort { get; set; }
        public string Payload { get; set; }
        public string Response { get; set; }
        public string OperationBy { get; set; }
    }
    //public class AuditTrailReq
    //{
    //    public long UserId { get; set; }
    //    public DateTime AccessDate { get; set; }
    //    public string Operation { get; set; }
    //    public string AccessedFromIpAddress { get; set; }
    //    public string AccessedFromPort { get; set; }
    //    public string Payload { get; set; }
    //    public string Response { get; set; }
    //}
}
