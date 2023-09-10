using Com.XpressPayments.Common.ViewModels;
using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{
    public class ResignationRepository : IResignationRepository
    {

        private string _connectionString;
        private readonly IDapperGenericRepository _repository;
        private readonly ILogger<ResignationRepository> _logger;
        private readonly IConfiguration _configuration;
        public ResignationRepository(ILogger<ResignationRepository> logger, IConfiguration configuration, IDapperGenericRepository repository)
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
                _logger.LogError($"MethodName: CreateResignation(ResignationDTO resignation) => {ex.Message}");
                throw;
            }
        } 


        public async Task<ResignationDTO> GetResignationByID(long ID)
        {
            try
            {
                    string query = "select * from SubmitResignation where SRFID = @SRFID and IsDeleted = @IsDeleted";
                    var param = new DynamicParameters();
                    param.Add("SRFID", ID);
                    param.Add("IsDeleted", false);

                    var response = await _repository.Get<ResignationDTO>(query, param, commandType: CommandType.Text);

                    return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation by ID - {ID}", ex);
                throw;
            }
        }

        public async Task<ResignationFormDTO> GetResignationByUserID(long UserID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    //Verbatin string
                    string query = @"select  u.LastName + ' ' + u.FirstName as 'Name', HU.LastName + ' ' + HU.FirstName as 'HodName',
                                    C.CompanyName  as 'CompanyName', UU.LastName +' '+ UU.FirstName as 'UnitHeadName' , 
                                    UN.UnitName as 'UnitName', D.DepartmentName as 'DepartmentName' ,SR.* 
                                    from SubmitResignation SR JOIN Users u ON SR.UserId = u.UserId 
                                    JOIN Company C ON SR.CompanyID = C.CompanyId
                                    LEFT JOIN Users HU ON SR.HodUserID = HU.UserId 
                                    LEFT JOIN Users UU ON SR.UnitHeadUserID = UU.UserId
                                    LEFT JOIN Unit UN ON U.UnitID = UN.UnitID
                                    LEFT JOIN Department D ON U.DeptId = D.DeptId
                                    where SR.UserId = @UserID and SR.IsDeleted = @IsDeleted";
                    var param = new DynamicParameters();
                    param.Add("UserID", UserID);
                    param.Add("IsDeleted", false);

                    var response = (await _dapper.QueryAsync<ResignationFormDTO>(query, param: param, commandType: CommandType.Text)).FirstOrDefault();

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
                    param.Add("IsDeleted", false);

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
                _logger.LogError($"MethodName: GetPendingResignationByUserID(long userID) => {ex.Message}");
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
                _logger.LogError($"MethodName: ApprovePendingResignationAsync(long userID, long SRFID) => {ex.Message}");
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
                param.Add("DateDisapproved", DateTime.Now);
                param.Add("DisapprovedReason", reason);
                param.Add("Resp", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _repository.Execute<int>("Sp_DisapprovePendingResignation", param, commandType: CommandType.StoredProcedure);
                var response = param.Get<int>("Resp");
                return response;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DisapprovePendingResignationAsync(long userID, long SRFID, string reason) => {ex.Message}");
                throw;
            }
        }


        public async Task<int> UpdateResignation(ResignationDTO resignation)
        {
            try
            {
                string sql = @"Update SubmitResignation set ReasonForResignation = @ReasonForResignation , LastDayOfWork = @LastDayOfWork, SignedResignationLetter = @SignedResignationLetter where SRFID = @SRFID";
                var param = new DynamicParameters();
                param.Add("SRFID", resignation.SRFID);
                param.Add("UserId", resignation.UserId);
                param.Add("LastDayOfWork", resignation.LastDayOfWork);
                param.Add("ReasonForResignation", resignation.ReasonForResignation);
                param.Add("SignedResignationLetter", resignation.SignedResignationLetter);

                return await _repository.Update<int>(sql, param, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateResignation(ResignationDTO resignation) ===>{ex.Message}");
                throw;
            }
        }


    }
}
