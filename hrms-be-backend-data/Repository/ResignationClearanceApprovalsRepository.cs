using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.Repository
{
    public class ResignationClearanceApprovalsRepository : IResignationClearanceApprovalsRepository
    {
        private string _connectionString;
        private readonly IDapperGenericRepository _dapper;
        private readonly ILogger<ResignationClearanceApprovalsRepository> _logger;
        private readonly IConfiguration _configuration;
        public ResignationClearanceApprovalsRepository(ILogger<ResignationClearanceApprovalsRepository> logger, IConfiguration configuration, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task<dynamic> CreateResignationClearanceApprovals(long CompanyId, long resignationClearanceID, long exitClearanceSetupID,long UserID, string departmentName)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("CompanyID", CompanyId);
                param.Add("DepartmentName", departmentName);
                param.Add("ResignationClearanceID", resignationClearanceID);
                param.Add("ExitClearanceSetupID", exitClearanceSetupID);
                param.Add("IsApproved", true);
                param.Add("DateApproved", DateTime.Now);
                param.Add("ApprovedByUserId", UserID);


                dynamic response = await _dapper.Get<string>("Sp_create_resignation_clearance_approvals", param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateExitClearanceSetup(ExitClearanceSetupDTO request) => {ex.Message}");
                throw;
            }
        }
        public async Task<IEnumerable<ResignationClearanceApprovalsDTO>> GetAllResignationClearanceApprovalsByResignationClearanceID(long clearanceID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("ResignationClearanceID", clearanceID);

                var response = await _dapper.GetAll<ResignationClearanceApprovalsDTO>("Sp_get_all_resignation_clearance_approvals_by_resignation_clearance_id", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation by Company - {clearanceID}", ex);
                throw;
            }
        }

    }
}
