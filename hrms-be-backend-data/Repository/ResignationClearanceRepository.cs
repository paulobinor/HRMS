using Com.XpressPayments.Common.ViewModels;
using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{
    public class ResignationClearanceRepository : IResignationClearanceRepository
    {
        private string _connectionString;
        private readonly IDapperGenericRepository _dapper;
        private readonly ILogger<IResignationClearanceRepository> _logger;
        private readonly IConfiguration _configuration;
        public ResignationClearanceRepository(ILogger<ResignationClearanceRepository> logger, IConfiguration configuration, IDapperGenericRepository repository)
        {
            _logger = logger;
            _dapper = repository;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<dynamic> CreateResignationClearance(ResignationClearanceDTO resignation)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("EmployeeID", resignation.EmployeeID);
                param.Add("ResignationID", resignation.ResignationID);
                param.Add("CompanyID", resignation.CompanyID);
                param.Add("ItemsReturnedToDepartment", resignation.ItemsReturnedToDepartment);
                param.Add("ItemsReturnedToAdmin", resignation.ItemsReturnedToAdmin);
                param.Add("ItemsReturnedToHR", resignation.ItemsReturnedToHr);
                param.Add("LoansOutstanding", resignation.LoansOutstanding);
                param.Add("Signature", resignation.Signature);
                param.Add("ExitDate", resignation.ExitDate);
                //param.Add("ReasonForExit", resignation.ReasonForExit);
                param.Add("CreatedByUserID", resignation.CreatedByUserID);
                param.Add("DateCreated", resignation.DateCreated);
                param.Add("Resp", dbType: DbType.Int32, direction: ParameterDirection.Output);

                dynamic response = await _dapper.Insert<int>("Sp_SubmitResignationClearance", param, commandType: CommandType.StoredProcedure);

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
                param.Add("ResignationClearanceID", ID);

                var response = await _dapper.Get<ResignationClearanceDTO>("Sp_get_resignation_clearance_by_id", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation clearance by ID - {ID}", ex);
                throw;
            }
        }
        public async Task<ResignationClearanceDTO> GetResignationClearanceByEmployeeID(long EmployeeId)
        {
            try
            {
       
                var param = new DynamicParameters();
                param.Add("EmployeeID", EmployeeId);

                var response = await _dapper.Get<ResignationClearanceDTO>("Sp_get_resignation_clearance_by_employee_id", param, commandType: CommandType.StoredProcedure);
                
                return response;
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation Clearance by UserID - {EmployeeId}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<ResignationClearanceDTO>> GetAllResignationClearanceByCompany(long companyID, int PageNumber, int RowsOfPage, string SearchVal)
        {
            try
            {

                {
                    var param = new DynamicParameters();
                    param.Add("CompanyID", companyID);
                    param.Add("@PageNumber", PageNumber);
                    param.Add("@RowsOfPage", RowsOfPage);
                    param.Add("@SearchVal", SearchVal.ToLower());
                    var response = await _dapper.GetAll<ResignationClearanceDTO>("Sp_get_all_resignation_clearance_by_company", param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation Clearances", ex);
                throw;
            }
        }

        public async Task<IEnumerable<ResignationClearanceDTO>> GetPendingResignationClearanceByEmployeeID(long employeeID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("employeeID", employeeID);

                var response = await _dapper.GetAll<ResignationClearanceDTO>("Sp_GetPendingResignationClearanceByUserID", param, commandType: CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetPendingResignationByUserID(long userID) => {ex.Message}");
                throw;
            }
        }
        public async Task<IEnumerable<ResignationClearanceDTO>> GetPendingResignationClearanceByCompnayID(long companyID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("CompanyID", companyID);

                var response = await _dapper.GetAll<ResignationClearanceDTO>("Sp_GetPendingResignationClearanceByCompanyID", param, commandType: CommandType.StoredProcedure);

                return response;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetPendingResignationByUserID(long userID) => {ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> ApprovePendingResignationClearance(long userID, long resignationClearanceId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("UserID", userID);
                param.Add("resignationClearanceId", resignationClearanceId);
                param.Add("DateApproved", DateTime.Now);
                //param.Add("Resp", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var response = await _dapper.Get<string>("Sp_ApprovePendingResignationClearance", param, commandType: CommandType.StoredProcedure);

                return response;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: ApprovePendingResignationClearance(long userID, long resignationClearanceId) => {ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DisapprovePendingResignationClearance(long userID, long resignationClearanceId, string reason)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("UserID", userID);
                param.Add("resignationClearanceId", resignationClearanceId);
                param.Add("DateDisapproved", DateTime.Now);
                param.Add("DisapprovedReason", reason);
                param.Add("Resp", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var response = await _dapper.Get<string>("Sp_DisapprovePendingResignationClearance", param, commandType: CommandType.StoredProcedure);

                return response;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DisapprovePendingResignationClearance(long userID, long resignationClearanceId, string reason) => {ex.Message}");
                throw;
            }
        }

       
    }
}
