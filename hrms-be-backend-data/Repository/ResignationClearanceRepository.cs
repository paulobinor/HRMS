using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{
    public class ResignationClearanceRepository : IResignationClearanceRepository
    {
        private string _connectionString;
        private readonly IDapperGenericRepository _repository;
        private readonly ILogger<IResignationClearanceRepository> _logger;
        private readonly IConfiguration _configuration;
        public ResignationClearanceRepository(ILogger<ResignationClearanceRepository> logger, IConfiguration configuration, IDapperGenericRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<int> CreateResignationClearance(ResignationClearanceDTO resignation)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("EmployeeID", resignation.EmployeeID);
                param.Add("ResignationID", resignation.ResignationID);
                param.Add("InterviewID", resignation.InterviewID);
                param.Add("ItemsReturnedToDepartment", resignation.ItemsReturnedToDepartment);
                param.Add("ItemsReturnedToAdmin", resignation.ItemsReturnedToAdmin);
                param.Add("Created_By_User_Email", resignation.Created_By_User_Email);
                param.Add("Collateral", resignation.Collateral);
                param.Add("ItemsReturnedToHR", resignation.ItemsReturnedToHR);
                param.Add("Loans", resignation.Loans);
                param.Add("LastDayOfWork", resignation.ExitDate);
                param.Add("Resp", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _repository.Insert<int>("Sp_SubmitResignationClearance", param, commandType: CommandType.StoredProcedure);

                int resp = param.Get<int>("Resp");

                return resp;

            }

            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateResignationClearance(CreateResignationClearance resignation) => {ex.Message}");
                throw;
            }
        }


        public async Task<ResignationClearanceDTO> GetResignationClearanceByID(long ID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("ResignationCleranceID", ID);
                param.Add("IsDeleted", false);

                var response = await _repository.Get<ResignationClearanceDTO>("Sp_get_resignation_clearance_by_id", param, commandType: CommandType.Text);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation by ID - {ID}", ex);
                throw;
            }
        }
        public async Task<ResignationClearanceDTO> GetResignationClearanceByUserID(long UserID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {

                    var param = new DynamicParameters();
                    param.Add("EmployeeID", UserID);
                    param.Add("IsDeleted", false);

                    var response = (await _dapper.QueryAsync<ResignationClearanceDTO>("Sp_get_resignation_clearance_by_user_id", param: param, commandType: CommandType.Text)).FirstOrDefault();

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation Clearance by UserID - {UserID}", ex);
                throw;
            }
        }

        public async Task<List<ResignationClearanceDTO>> GetPendingResignationClearanceByUserID(long userID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("UserID", userID);
                //change stored proceedure
                var response = await _repository.GetAll<ResignationClearanceDTO>("Sp_GetPendingResignationClearanceByUserID", param, commandType: CommandType.StoredProcedure);

                return response;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetPendingResignationClearanceByUserID(long userID) => {ex.Message}");
                throw;
            }
        }

        public async Task<int> ApprovePendingResignationClearance(long userID, long ID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("UserID", userID);
                param.Add("ID", ID);
                param.Add("DateApproved", DateTime.Now);
                param.Add("Resp", dbType: DbType.Int32, direction: ParameterDirection.Output);

                //Change storedProceedure
                await _repository.Execute<int>("Sp_ApprovePendingResignationClearance", param, commandType: CommandType.StoredProcedure);
                var response = param.Get<int>("Resp");

                return response;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: ApprovePendingResignationClearance(long userID, long ID) => {ex.Message}");
                throw;
            }
        }

        public async Task<int> DisapprovePendingResignationClearance(long userID, long ID, string reason)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("UserID", userID);
                param.Add("ID", ID);
                param.Add("DateDisapproved", DateTime.Now);
                param.Add("DisapprovedReason", reason);
                param.Add("Resp", dbType: DbType.Int32, direction: ParameterDirection.Output);

                //change storedProceedure
                await _repository.Execute<int>("Sp_DisapprovePendingResignationClearance", param, commandType: CommandType.StoredProcedure);
                var response = param.Get<int>("Resp");
                return response;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DisapprovePendingResignationClearance(long userID, long SRFID, string reason) => {ex.Message}");
                throw;
            }
        }
    }
}
