using Com.XpressPayments.Common.ViewModels;
using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.Enums;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.XpressPayments.Data.Repositories.Branch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Com.XpressPayments.Data.DapperGeneric;

namespace Com.XpressPayments.Data.ExitModuleRepository.Repositories
{
    public class ResignationRepository : IResignationRepository
    {

        private string _connectionString;
        private readonly IDapperGenericRepository _repository;
        private readonly ILogger<BranchRepository> _logger;
        private readonly IConfiguration _configuration;
        public ResignationRepository(ILogger<BranchRepository> logger, IConfiguration configuration, IDapperGenericRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<int> CreateResignation(ResignationDTO resignation)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("UserId", resignation.UserId);
                param.Add("SignedResignationLetter", resignation.SignedResignationLetter);
                param.Add("CompanyID", resignation.CompanyID);
                param.Add("DateAdded", resignation.Date);
                param.Add("LastDayOfWork", resignation.LastDayOfWork);
                param.Add("Created_By_User_Email", resignation.Created_By_User_Email);
                param.Add("ReasonForResignation", resignation.ReasonForResignation);
                param.Add("DateCreated", resignation.DateCreated);
                param.Add("Resp", dbType: DbType.Int32, direction: ParameterDirection.Output);

                //int response =  _dapper.ExecuteReader("Sp_SubmitResignation", param: param, commandType: CommandType.StoredProcedure);

                await _repository.Insert<int>("Sp_SubmitResignation", param, commandType: CommandType.StoredProcedure);

                int resp = param.Get<int>("Resp");

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateBranch(CreateBranchDTO branch, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }


        public async Task<ResignationDTO> GetResignationByID(long ID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    string query = "select * from SubmitResignation where SRFID = @SRFID and IsDeleted = @IsDeleted";
                    var param = new DynamicParameters();
                    param.Add("SRFID", ID);
                    param.Add("IsDeleted", false);

                    var response = (await _dapper.QueryAsync<ResignationDTO>(query, param: param, commandType: CommandType.Text)).FirstOrDefault();

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation by ID - {ID}", ex);
                throw;
            }
        }

        public async Task<ResignationDTO> GetResignationByUserID(long UserID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    string query = "select * from SubmitResignation where UserId = @UserID and IsDeleted = @IsDeleted";
                    var param = new DynamicParameters();
                    param.Add("UserID", UserID);
                    param.Add("IsDeletd", false);

                    var response = (await _dapper.QueryAsync<ResignationDTO>(query, param: param, commandType: CommandType.Text)).FirstOrDefault();

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation by UserID - {UserID}", ex);
                throw;
            }
        }

        public async Task<List<ResignationDTO>> GetResignationByCompanyID(long companyID, bool isApproved)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    string query = "select * from SubmitResignation where CompanyID = @CompanyID and ISApproved = @ISApproved and IsDeleted = @IsDeleted";
                    var param = new DynamicParameters();
                    param.Add("CompanyID", companyID);
                    param.Add("ISApproved", isApproved);
                    param.Add("IsDeletd", false);

                    var response = (await _dapper.QueryAsync<ResignationDTO>(query, param: param, commandType: CommandType.Text)).ToList();

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation by Company - {companyID}", ex);
                throw;
            }
        }


        public async Task<int> DeleteResignation(long ID, string deletedBy, string deleteReason)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    string query = "Update SubmitResignation set IsDeleted = @IsDeleted , Deleted_By_User_Email = @DeletedBy , Deleted_Date = @DateDeleted ,Reasons_For_Delete = @deleteReason where SRFID = @SRFID";
                    var param = new DynamicParameters();
                    param.Add("SRFID", ID);
                    param.Add("DeletedBy", deletedBy);
                    param.Add("IsDeleted", true);
                    param.Add("deleteReason", deleteReason);
                    param.Add("DateDeleted", DateTime.Now);


                    int response = await _dapper.ExecuteAsync(query, param: param, commandType: CommandType.Text);

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


        public async Task<List<ResignationDTO>> GetPendingResignationByUserID(long userID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("UserID", userID);

                var response = await _repository.GetAll<ResignationDTO>("Sp_GetPendingResignationByUserID", param, commandType: CommandType.StoredProcedure);

                return response;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateBranch(CreateBranchDTO branch, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<int> ApprovePendingResignation(long userID, long SRFID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("UserID", userID);
                param.Add("SRFID", SRFID);
                param.Add("DateApproved", DateTime.Now);
                param.Add("Resp", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _repository.Execute<int>("Sp_ApprovePendingResignation", param, commandType: CommandType.StoredProcedure);
                var response = param.Get<int>("Resp");

                return response;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateBranch(CreateBranchDTO branch, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<int> DisapprovePendingResignation(long userID, long SRFID, string reason)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("UserID", userID);
                param.Add("SRFID", SRFID);
                param.Add("DateApproved", DateTime.Now);
                param.Add("DisapprovedReason", reason);
                param.Add("Resp", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _repository.Execute<int>("Sp_DisapprovePendingResignation", param, commandType: CommandType.StoredProcedure);
                var response = param.Get<int>("Resp");
                return response;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateBranch(CreateBranchDTO branch, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

    }
}
