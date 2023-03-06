using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.Repositories.Departments.IRepository;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Com.XpressPayments.Data.Repositories.Branch
{
    public class BranchRepository : IBranchRepository
    {
        private string _connectionString;
        private readonly ILogger<BranchRepository> _logger;
        private readonly IConfiguration _configuration;

        public BranchRepository(IConfiguration configuration, ILogger<BranchRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateBranch(CreateBranchDTO branch, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", BranchEnum.CREATE);
                    param.Add("@BranchName", branch.BranchName.Trim());
                    param.Add("@Address", branch.Address.Trim());
                    param.Add("@CountryID", branch.CountryID);
                    param.Add("@StateID", branch.StateID);
                    param.Add("@LgaID", branch.LgaID);
                    param.Add("@CompanyId", branch.CompanyID);

                    param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Branch, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateBranch(CreateBranchDTO branch, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<int> UpdateBranch(UpdateBranchDTO payload, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", BranchEnum.UPDATE);
                    param.Add("@BranchIDUpd", payload.BranchID);
                    param.Add("@BranchNameUpd", payload.BranchName.Trim());
                    param.Add("@AddressUpd", payload.Address.Trim());
                    param.Add("@CountryIDUpd", payload.CountryID);
                    param.Add("@StateIDUpd", payload.StateID);
                    param.Add("@LgaIDUpd", payload.LgaID);
                    param.Add("@CompanyIdUpd", payload.CompanyID);

                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync("Sp_Branch", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }

        public async Task<dynamic> DeleteBranch(DeleteBranchDTO Dept, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", BranchEnum.DELETE);
                    param.Add("@BranchIDDelete", Convert.ToInt32(Dept.BranchID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@@Reasons_For_Deleting_Branch", Dept.Reasons_For_Delete == null ? "" : Dept.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Branch, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteBranch(DeleteBranchDTO Dept, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<BranchDTO>> GetAllActiveBranch()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", BranchEnum.GETALLACTIVE);

                    var BranchDetails = await _dapper.QueryAsync<BranchDTO>(ApplicationConstant.Sp_Branch, param: param, commandType: CommandType.StoredProcedure);

                    return BranchDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActiveBranch() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<BranchDTO>> GetAllBranch()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", BranchEnum.GETALL);

                    var BranchDetails = await _dapper.QueryAsync<BranchDTO>(ApplicationConstant.Sp_Branch, param: param, commandType: CommandType.StoredProcedure);

                    return BranchDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllDepartments() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<BranchDTO> GetBranchById(long BranchID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Department.GETBYID);
                    param.Add("@BranchIdGet", BranchID);

                    var BranchDetails = await _dapper.QueryFirstOrDefaultAsync<BranchDTO>(ApplicationConstant.Sp_Branch, param: param, commandType: CommandType.StoredProcedure);

                    return BranchDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetBranchById(long BranchID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<BranchDTO> GetBranchByName(string BranchName)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Department.GETBYEMAIL);
                    param.Add("@BranchNameGet", BranchName);

                    var BranchDetails = await _dapper.QueryFirstOrDefaultAsync<BranchDTO>(ApplicationConstant.Sp_Branch, param: param, commandType: CommandType.StoredProcedure);

                    return BranchDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetBranchByName(string BranchName) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<BranchDTO>> GetAllBranchbyCompanyId(long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", 8);
                    param.Add("@CompanyIdGet", companyId);

                    var BranchDetails = await _dapper.QueryAsync<BranchDTO>(ApplicationConstant.Sp_Branch, param: param, commandType: CommandType.StoredProcedure);

                    return BranchDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllBranchbyCompanyId(long companyId) ===>{ex.Message}");
                throw;
            }
        }


    }
}
