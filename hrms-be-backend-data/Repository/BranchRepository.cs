using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class BranchRepository : IBranchRepository
    {

        private readonly ILogger<BranchRepository> _logger;
        private readonly IDapperGenericRepository _dapper;

        public BranchRepository(IDapperGenericRepository dapper, ILogger<BranchRepository> logger)
        {
            _logger = logger;
            _dapper = dapper;
        }

        public async Task<string> ProcessBranch(ProcessBranchReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@BranchId", payload.BranchId);
                param.Add("@BranchName", payload.BranchName);
                param.Add("@Address", payload.Address);
                param.Add("@IsHeadQuater", payload.IsHeadQuater);
                param.Add("@LgaId", payload.LgaId);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                param.Add("@IsModifield", payload.IsModifield);

                return await _dapper.Get<string>("sp_process_branch", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> ProcessBranch => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> DeleteBranch(DeleteBranchReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@BranchId", payload.BranchId);
                param.Add("@DeletedComment", payload.Comment);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                return await _dapper.Get<string>("sp_delete_branch", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> DeleteBranch => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }

        public async Task<BranchWithTotalVm> GetBranches(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new BranchWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@PageNumber", PageNumber);
                param.Add("@CompanyId", CompanyId);
                var result = await _dapper.GetMultiple("sp_get_branches", param, gr => gr.Read<long>(), gr => gr.Read<BranchVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<BranchVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"BranchRepository -> GetBranches => {ex}");
                return returnData;
            }

        }
        public async Task<BranchWithTotalVm> GetBranchesDeleted(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new BranchWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@PageNumber", PageNumber);
                param.Add("@CompanyId", CompanyId);
                var result = await _dapper.GetMultiple("sp_get_branches_deleted", param, gr => gr.Read<long>(), gr => gr.Read<BranchVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<BranchVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"BranchRepository -> GetCompaniesDeleted => {ex}");
                return returnData;
            }

        }
        public async Task<BranchVm> GetBranchById(long Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                return await _dapper.Get<BranchVm>("sp_get_branch_by_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"BranchRepository -> GetBranchById => {ex}");
                return new BranchVm();
            }
        }
        public async Task<BranchVm> GetBranchByName(string BranchName, long CompanyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@BranchName", BranchName);
                param.Add("@CompanyId", CompanyId);
                return await _dapper.Get<BranchVm>("sp_get_branch_by_name", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"BranchRepository -> GetBranchByName => {ex}");
                return new BranchVm();
            }
        }
    }
}
