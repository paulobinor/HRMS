using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;
using XpressHRMS.Data.IRepository;
using XpressHRMS.IRepository;

namespace XpressHRMS.Data.Repository
{
    public class AuditTrailRepository : IAuditTrailRepository
    {
        private readonly ILogger<AuditTrailRepository> _logger;
        private readonly string _connectionString;
        private readonly IDapperGeneric _dapper;
        public AuditTrailRepository(IConfiguration configuration, ILogger<AuditTrailRepository> logger, IDapperGeneric dapper)
        {
            _connectionString = configuration.GetConnectionString("");
            _logger = logger;
            _dapper = dapper;
        }
        public async Task<List<AuditTrail>> GetAllAuditTrail(string DateFrom, string DateTo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@DateFrom", DateFrom);
                param.Add("@DateTo", DateTo);

                return await _dapper.GetAll<AuditTrail>("sp_get_audit_trail", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }

        }

        public async Task<long> CreateAuditTrail(AuditTrailReq payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", payload.UserId);
                param.Add("@AccessDate", payload.AccessDate);
                param.Add("@Operation", payload.Operation);
                param.Add("@AccessedFromIpAddress", payload.AccessedFromIpAddress);
                param.Add("@AccessedFromPort", payload.AccessedFromPort);
                param.Add("@Payload", payload.Payload);
                param.Add("@Response", payload.Response);


                return await _dapper.Get<long>("sp_process_audit_trail", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }
    }
}
