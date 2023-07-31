using System.Collections.Generic;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface IAuditTrailRepository
    {
        Task<long> CreateAuditTrail(AuditTrailReq payload);
        Task<List<AuditTrail>> GetAllAuditTrail(string DateFrom, string DateTo);
    }
}